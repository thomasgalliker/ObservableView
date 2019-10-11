using System.Diagnostics;

namespace ObservableViewSample.Model
{
    [DebuggerDisplay("Mall: {this.Title}, {this.Subtitle}")]
    public class Mall
    {
        public string Title { get; set; }

        public string Subtitle { get; set; }

        public Mall(string title, string subtitle)
        {
            this.Title = title;
            this.Subtitle = subtitle;
        }
    }

}