#if __ANDROID__
using StratCrossPlatformApp.Droid;
#elif __IOS__
using StratCrossPlatformApp.iOS.Helpers;
#elif WINDOWS_UWP
using StratCrossPlatformApp.UWP.Helpers;
#endif
using StratCrossPlatformApp.Helpers;
using StratCrossPlatformApp.Interfaces;
using StratCrossPlatformApp.Services;
using StratCrossPlatformApp.Model;

namespace StratCrossPlatformApp
{
    public partial class App 
    {
        public App()
        {
        }

        public static void Initialize()
        {
            ServiceLocator.Instance.Register<IDataStore<Item>, MockDataStore>();
            ServiceLocator.Instance.Register<IMessageDialog, MessageDialog>();
            ServiceLocator.Instance.Register<IDataStore<Item>, MockDataStore>();
        }
    }
}