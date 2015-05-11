using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using HardwareInventory.Datamodel;
using HardwareInventory.Menu;
using HardwareInventory.Repository;

namespace HardwareInventory.Pages
{
    public sealed partial class HardwarePage : Page
    {
        public HardwarePage()
        {
            this.InitializeComponent();
            RefreshData();
        }

        private async Task RefreshData()
        {
           itemGrid.ItemsSource = (await HardwareRepository.GetHardwareItems()).OrderBy(x => x.Name).ToList();
        }

        public void HardwareItem_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as HardwareItem;
            NavigationManager.Navigate(NavigationCommand.HardwareItemDetails,Frame, item);
        }

        public async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            await RefreshData();
        }
        public void BackButton_Pressed(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }
    }
}
