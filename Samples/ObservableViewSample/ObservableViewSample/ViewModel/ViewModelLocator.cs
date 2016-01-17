using GalaSoft.MvvmLight.Ioc;

using Microsoft.Practices.ServiceLocation;

using ObservableViewSample.Service;

namespace ObservableViewSample.ViewModel
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