using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;

using ObservableViewSample.Data.Model;
using ObservableViewSample.Data.Service;

namespace ObservableViewSample.Data.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<IMallManager, MallManager>();
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }
    }
}