using System;
using System.Collections.Generic;
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
        private IEnumerable<HardwareItem> _items = new List<HardwareItem>(); 
        private DateTime _lastKeyDownAt = DateTime.MinValue;
        readonly DispatcherTimer _searchTimer = new DispatcherTimer();
        public HardwarePage()
        {
            this.InitializeComponent();
            RefreshData();
            _searchTimer.Interval = TimeSpan.FromMilliseconds(300);
            _searchTimer.Tick += (sender, o) => { FilterData(); };
        }

        private async Task RefreshData()
        {
            _items = (await HardwareRepository.GetHardwareItems()).OrderBy(x => x.Name).ToList();
           itemGrid.ItemsSource = _items;
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

    

        public void FilterTextBox_TextChanged(object sender, TextChangedEventArgs te)
        {
            _searchTimer.Start();
        }

        private void FilterData()
        {
            _searchTimer.Stop();
            var searchWords = filterTextbox.Text;
            itemGrid.ItemsSource = string.IsNullOrWhiteSpace(searchWords) ? 
                _items : _items.Where(x => x.Name.ToLower()
                .StartsWith(searchWords.ToLower()) || x.Name.Split(' ')
                 .Any(namePart => namePart.ToLower()
                 .StartsWith(searchWords.ToLower()))).ToList();
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
