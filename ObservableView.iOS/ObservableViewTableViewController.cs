using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

#if __UNIFIED__
using Foundation;
using UIKit;
#else
using MonoTouch.Foundation;
using MonoTouch.UIKit;
#endif

namespace ObservableView
{
    public class ObservableViewTableViewController<T> : UITableViewController, INotifyPropertyChanged
    {
        public const string SelectedItemPropertyName = "SelectedItem";

        private ObservableView<T> dataSource;
        private bool isViewLoaded;
        private Thread mainThread;
        ////private INotifyCollectionChanged notifier;
        private ObservableTableSource<T> tableSource;
        ////private TableSource<T> tableSource;

        /// <summary>
        ///     Occurs when a new item gets selected in the list.
        /// </summary>
        public event EventHandler SelectionChanged;

        /// <summary>
        ///     When set, specifies which animation should be used when rows change.
        /// </summary>
        public UITableViewRowAnimation AddAnimation { get; set; }

        /// <summary>
        ///     A delegate to a method taking a <see cref="UITableViewCell" />
        ///     and setting its elements' properties according to the item
        ///     passed as second parameter.
        ///     The cell must be created first in the <see cref="CreateCellDelegate" />
        ///     delegate.
        /// </summary>
        public Action<UITableViewCell, T, NSIndexPath> BindCellDelegate { get; set; }

        /// <summary>
        ///     A delegate to a method creating or reusing a <see cref="UITableViewCell" />.
        ///     The cell will then be passed to the <see cref="BindCellDelegate" />
        ///     delegate to set the elements' properties.
        /// </summary>
        public Func<NSString, UITableViewCell> CreateCellDelegate { get; set; }

        /// <summary>
        ///     The data source of this list controller.
        /// </summary>
        public ObservableView<T> DataSource
        {
            get
            {
                return this.dataSource;
            }

            set
            {
                if (Equals(this.dataSource, value))
                {
                    return;
                }

                if (this.dataSource != null)
                {
                    this.dataSource.SourceCollectionChanged -= this.HandleCollectionChanged;
                }

                this.dataSource = value;

                if (this.dataSource != null)
                {
                    this.dataSource.SourceCollectionChanged += this.HandleCollectionChanged;
                }

                if (this.isViewLoaded)
                {
                    this.TableView.ReloadData();
                }
            }
        }

        private void HandleCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!this.isViewLoaded)
            {
                return;
            }
#if __UNIFIED__
            Action act = () =>
#else
            NSAction act = () =>
#endif
            {
                this.TableView.ReloadData();
            };

            var isMainThread = Thread.CurrentThread == this.mainThread;

            if (isMainThread)
            {
                act();
            }
            else
            {
                NSOperationQueue.MainQueue.AddOperation(act);
                NSOperationQueue.MainQueue.WaitUntilAllOperationsAreFinished();
            }
        }

        /// <summary>
        ///     When set, specifieds which animation should be used when a row is deleted.
        /// </summary>
        public UITableViewRowAnimation DeleteAnimation { get; set; }

        /// <summary>
        ///     When set, returns the height of the view that will be used for the TableView's footer.
        /// </summary>
        /// <seealso cref="GetViewForFooterDelegate" />
        public Func<UITableView, int, float> GetHeightForFooterDelegate { get; set; }

        /// <summary>
        ///     When set, returns the height of the view that will be used for the TableView's header.
        /// </summary>
        /// <seealso cref="GetHeightForHeaderDelegate" />
        public Func<UITableView, int, float> GetHeightForHeaderDelegate { get; set; }

        /// <summary>
        ///     When set, returns a view that can be used as the TableView's footer.
        /// </summary>
        /// <seealso cref="GetHeightForFooterDelegate" />
        public Func<UITableView, int, string, UIView> GetViewForFooterDelegate { get; set; }

        /// <summary>
        ///     When set, returns a view that can be used as the TableView's header.
        /// </summary>
        /// <seealso cref="GetViewForHeaderDelegate" />
        public Func<UITableView, int, string, UIView> GetViewForHeaderDelegate { get; set; }

        /// <summary>
        ///     Gets the TableView's selected item.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public T SelectedItem { get; private set; }

        /// <summary>
        ///     The source of the TableView.
        /// </summary>
        public UITableViewSource TableSource
        {
            get
            {
                return this.tableSource;
            }
        }

        /// <summary>
        ///     Overrides <see cref="UITableViewController.TableView" />.
        ///     Sets or gets the controllers TableView. If you use a TableView
        ///     placed in the UI manually, use this property's setter to assign
        ///     your TableView to this controller.
        /// </summary>
        public override UITableView TableView
        {
            get
            {
                return base.TableView;
            }
            set
            {
                base.TableView = value;

                if (this.tableSource == null)
                {
                    base.TableView.Source = this.CreateSource();
                }
                else
                {
                    base.TableView.Source = this.tableSource;
                }

                this.isViewLoaded = true;
            }
        }

        /// <summary>
        ///     Initializes a new instance of this class with a plain style.
        /// </summary>
        public ObservableViewTableViewController()
            : base(UITableViewStyle.Plain)
        {
            this.Initialize();
        }

        /// <summary>
        ///     Initializes a new instance of this class with a specific style.
        /// </summary>
        /// <param name="tableStyle">The style that will be used for this controller.</param>
        public ObservableViewTableViewController(UITableViewStyle tableStyle)
            : base(tableStyle)
        {
            this.Initialize();
        }

        /// <summary>
        ///     Occurs when a property of this instance changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Overrides the <see cref="UIViewController.ViewDidLoad" /> method.
        /// </summary>
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.TableView.Source = this.CreateSource();
            this.isViewLoaded = true;
        }

        /// <summary>
        ///     Binds a <see cref="UITableViewCell" /> to an item's properties.
        ///     If a <see cref="BindCellDelegate" /> is available, this delegate will be used.
        ///     If not, a simple text will be shown.
        /// </summary>
        /// <param name="cell">The cell that will be prepared.</param>
        /// <param name="item">The item that should be used to set the cell up.</param>
        /// <param name="indexPath">The <see cref="NSIndexPath" /> for this cell.</param>
        protected virtual void BindCell(UITableViewCell cell, object item, NSIndexPath indexPath)
        {
            if (this.BindCellDelegate == null)
            {
                cell.TextLabel.Text = item.ToString();
            }
            else
            {
                this.BindCellDelegate(cell, (T)item, indexPath);
            }
        }

        /// <summary>
        ///     Creates a <see cref="UITableViewCell" /> corresponding to the reuseId.
        ///     If it is set, the <see cref="CreateCellDelegate" /> delegate will be used.
        /// </summary>
        /// <param name="reuseId">A reuse identifier for the cell.</param>
        /// <returns>The created cell.</returns>
        protected virtual UITableViewCell CreateCell(NSString reuseId)
        {
            if (this.CreateCellDelegate == null || this.BindCellDelegate == null)
            {
                return new UITableViewCell(UITableViewCellStyle.Default, reuseId);
            }

            return this.CreateCellDelegate(reuseId);
        }

        /// <summary>
        ///     Created the ObservableTableSource for this controller.
        /// </summary>
        /// <returns>The created ObservableTableSource.</returns>
        protected virtual ObservableTableSource<T> CreateSource()
        {
            this.tableSource = new ObservableTableSource<T>(this);
            return this.tableSource;
        }

        /// <summary>
        ///     Called when a row gets selected. Raises the SelectionChanged event.
        /// </summary>
        /// <param name="item">The selected item.</param>
        /// <param name="indexPath">The NSIndexPath for the selected row.</param>
        protected virtual void OnRowSelected(object item, NSIndexPath indexPath)
        {
            this.SelectedItem = (T)item;

            // ReSharper disable ExplicitCallerInfoArgument
            this.RaisePropertyChanged(SelectedItemPropertyName);
            // ReSharper restore ExplicitCallerInfoArgument

            var handler = this.SelectionChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Raises the <see cref="PropertyChanged" /> event.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void Initialize()
        {
            this.mainThread = Thread.CurrentThread;

            this.AddAnimation = UITableViewRowAnimation.Automatic;
            this.DeleteAnimation = UITableViewRowAnimation.Automatic;
        }

        /// <summary>
        ///     A <see cref="UITableViewSource" /> that handles changes to the underlying
        ///     data source if this data source is an <see cref="INotifyCollectionChanged" />.
        /// </summary>
        /// <typeparam name="T2">The type of the items that the data source contains.</typeparam>
        /// <remarks>In the current implementation, only one section is supported.</remarks>
        protected class ObservableTableSource<T2> : UITableViewSource
        {
            private readonly ObservableViewTableViewController<T2> controller;
            private readonly NSString reuseId = new NSString("C");

            /// <summary>
            ///     Initializes an instance of this class.
            /// </summary>
            /// <param name="controller">The controller associated to this instance.</param>
            public ObservableTableSource(ObservableViewTableViewController<T2> controller)
            {
                this.controller = controller;
            }

            /// <summary>
            ///     Attempts to dequeue or create a cell for the list.
            /// </summary>
            /// <param name="tableView">The TableView that is the cell's parent.</param>
            /// <param name="indexPath">The NSIndexPath for the cell.</param>
            /// <returns>The created or recycled cell.</returns>
            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {
                var cell = tableView.DequeueReusableCell(this.reuseId) ?? this.controller.CreateCell(this.reuseId);

                try
                {
                    var coll = this.controller.dataSource;

                    if (coll != null)
                    {
                        var obj = coll.Source[indexPath.Row];
                        this.controller.BindCell(cell, obj, indexPath);
                    }

                    return cell;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }

                return cell;
            }

            /// <summary>
            ///     When called, checks if the ObservableTableViewController{T}.GetHeightForFooter
            ///     delegate has been set. If yes, calls that delegate to get the TableView's footer height.
            /// </summary>
            /// <param name="tableView">The active TableView.</param>
            /// <param name="section">The section index.</param>
            /// <returns>The footer's height.</returns>
            /// <remarks>In the current implementation, only one section is supported.</remarks>
#if __UNIFIED__
            public override nfloat GetHeightForFooter(UITableView tableView, nint section)
#else
            public override float GetHeightForFooter(UITableView tableView, int section)
#endif
            {
                if (this.controller.GetHeightForFooterDelegate != null)
                {
                    return this.controller.GetHeightForFooterDelegate(tableView, (int)section);
                }

                return 0;
            }

            /// <summary>
            ///     When called, checks if the ObservableTableViewController{T}.GetHeightForHeader
            ///     delegate has been set. If yes, calls that delegate to get the TableView's header height.
            /// </summary>
            /// <param name="tableView">The active TableView.</param>
            /// <param name="section">The section index.</param>
            /// <returns>The header's height.</returns>
            /// <remarks>In the current implementation, only one section is supported.</remarks>
#if __UNIFIED__
            public override nfloat GetHeightForHeader(UITableView tableView, nint section)

#else
            public override float GetHeightForHeader(UITableView tableView, int section)
#endif
            
            {
                if (this.controller.GetHeightForHeaderDelegate != null)
                {
                    return this.controller.GetHeightForHeaderDelegate(tableView, (int)section);
                }

                return 0;
            }

            /// <summary>
            ///     When called, checks if the ObservableTableViewController{T}.GetViewForFooter
            ///     delegate has been set. If yes, calls that delegate to get the TableView's footer.
            /// </summary>
            /// <param name="tableView">The active TableView.</param>
            /// <param name="section">The section index.</param>
            /// <returns>The UIView that should appear as the section's footer.</returns>
            /// <remarks>In the current implementation, only one section is supported.</remarks>
#if __UNIFIED__
           public override UIView GetViewForFooter(UITableView tableView, nint section)

#else
            public override UIView GetViewForFooter(UITableView tableView, int section)
#endif
            
            {
                if (this.controller.GetViewForFooterDelegate != null)
                {
                    string title = this.TitleForFooter(tableView, section);
                    return this.controller.GetViewForFooterDelegate(tableView, (int)section, title);
                }

                return null;
            }

            /// <summary>
            ///     When called, checks if the ObservableTableViewController{T}.GetViewForHeader
            ///     delegate has been set. If yes, calls that delegate to get the TableView's header.
            /// </summary>
            /// <param name="tableView">The active TableView.</param>
            /// <param name="section">The section index.</param>
            /// <returns>The UIView that should appear as the section's header.</returns>
            /// <remarks>In the current implementation, only one section is supported.</remarks>
#if __UNIFIED__
           public override UIView GetViewForHeader(UITableView tableView, nint section)

#else
            public override UIView GetViewForHeader(UITableView tableView, int section)
#endif
            
            {
                if (this.controller.GetViewForHeaderDelegate != null)
                {
                    string title = this.TitleForHeader(tableView, section);
                    return this.controller.GetViewForHeaderDelegate(tableView, (int)section, title);
                }

                return null;
            }

            /// <summary>
            ///     Called by the TableView to determine how many sections(groups) there are.
            /// </summary>
#if __UNIFIED__
           public override nint NumberOfSections(UITableView tableView)

#else
            public override int NumberOfSections(UITableView tableView)
#endif
            
            {
                return this.controller.DataSource.Groups.Count();
            }

            /// <summary>
            ///     Called by the TableView to determine how many cells to create for that particular section.
            /// </summary>
#if __UNIFIED__
           public override nint RowsInSection(UITableView tableview, nint section)

#else
            public override int RowsInSection(UITableView tableview, int section)
#endif
            
            {
                if (this.controller.DataSource.GroupKey != null)
                {
                    return this.controller.DataSource.Groups.ToArray()[section].Count;
                }

                return this.controller.DataSource.View.Count;
            }

            /// <summary>
            ///     Called by the TableView to retrieve the header text for the particular section(group)
            /// </summary>
#if __UNIFIED__
           public override string TitleForHeader(UITableView tableView, nint section)

#else
            public override string TitleForHeader(UITableView tableView, int section)
#endif
            {
                return this.controller.DataSource.Groups.ToArray()[section].Key;
            }

            /// <summary>
            ///     Called by the TableView to retrieve the footer text for the particular section(group)
            /// </summary>
#if __UNIFIED__
           public override string TitleForFooter(UITableView tableView, nint section)

#else
            public override string TitleForFooter(UITableView tableView, int section)
#endif
            
            {
                return this.controller.DataSource.Groups.ToArray()[section].Key;
            }

            /// <summary>
            ///     Overrides the <see cref="UITableViewSource.RowSelected" /> method
            ///     and notifies the associated <see cref="ObservableTableViewController{T}" />
            ///     that a row has been selected, so that the corresponding events can be raised.
            /// </summary>
            /// <param name="tableView">The active TableView.</param>
            /// <param name="indexPath">The row's NSIndexPath.</param>
            public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
            {
                var item = this.controller.dataSource != null ? this.controller.dataSource.Source[indexPath.Row] : default(T2);
                this.controller.OnRowSelected(item, indexPath);
            }
        }
    }
}