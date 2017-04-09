using StratCrossPlatformApp.Model;
using StratCrossPlatformApp.ViewModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace StratCrossPlatformApp.UWP.Views
{
	public sealed partial class BrowseItemDetail : Page
	{
		public ItemDetailViewModel ViewModel { get; set; }

		public BrowseItemDetail()
		{
			InitializeComponent();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			ViewModel = new ItemDetailViewModel((Item)e.Parameter);
			DataContext = ViewModel;
		}
	}
}