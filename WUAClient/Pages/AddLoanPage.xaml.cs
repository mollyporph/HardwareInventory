using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using HardwareInventory.Datamodel;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Core;
using HardwareInventory.Repository;
using HardwareInventory.Utilities;

namespace HardwareInventory.Pages
{

    public sealed partial class AddLoanPage : Page
    {
        public ObservableCollection<string> suggestedHardWareItems = new ObservableCollection<string>(); 
        public ObservableCollection<HwItemNameViewModel> hardWareItemList = new ObservableCollection<HwItemNameViewModel>(); 
        public AddLoanPage()
        {
            this.InitializeComponent();
            hardWareItemList.Add(new HwItemNameViewModel());
            this.itemList.ItemsSource = hardWareItemList;
            

        }

        public void Dismiss(object sender, RoutedEventArgs e)
        {
            userPanel.Visibility = Visibility.Collapsed;
            searchBox.Visibility = Visibility.Visible;
        }

        public async Task SearchUserId(int userId)
        {
            var result = await App.MobileService.InvokeApiAsync<CrewMember>("CustomCrewMember", HttpMethod.Get, new Dictionary<string, string> { { "id", userId.ToString() } });
            await SetCrewPanel(result);
        }
        public async Task SearchUsername(string username)
        {
            var result = await App.MobileService.InvokeApiAsync<CrewMember>("CustomCrewMember", HttpMethod.Get, new Dictionary<string, string> { { "username", username } });
            await SetCrewPanel(result);
        }

        private async Task SetCrewPanel(CrewMember result)
        {
            if (result.username != null)
            {
                await LoadPreviousLoanData(result.username);
            }
            usernameTextBlock.Text = result.username ?? "N/A";
            nameTextBlock.Text = $"{result.firstname ?? "N/A"} {result.lastname ?? "N/A"}";
            BitmapImage bitmap;
            if (result.badge_picture != null)
            {
                var badgeUri = new Uri("http://" + result.badge_picture);
                bitmap = new BitmapImage(badgeUri);
            }
            else
            {
                var badgeUri = new Uri("ms-appx:///Assets/Logo.png");
                bitmap = new BitmapImage(badgeUri);
            }
            badgeImage.Source = bitmap;
        }

        private async Task LoadPreviousLoanData(string username)
        {
           var loanItems = await LoanRepository.GetLoanItemsForUser(username);
           
        }


        public async void SearchBox_OnQuerySubmitted(SearchBox sb, SearchBoxQuerySubmittedEventArgs se)
        {
            userPanel.Visibility = Visibility.Collapsed;
            searchBox.Visibility = Visibility.Collapsed;
            int userIdParsed;
            if (!int.TryParse(se.QueryText, out userIdParsed))
            {
                await SearchUsername(se.QueryText);
            }
            else
            {
                await SearchUserId(userIdParsed);
            }
            userPanel.Visibility = Visibility.Visible;
        }

        public async void AutoSuggestBox_OnQuerySubmitted(AutoSuggestBox asb,
            AutoSuggestBoxQuerySubmittedEventArgs asArgs)
        {
           // this.hardWareItemList.Add("");
        }

        public void SubmitButtonClicked(object sender, RoutedEventArgs e)
        {
            var username = usernameTextBlock.Text;

            LoanRepository.CreateLoan(username, hardWareItemList.Where(x =>!string.IsNullOrEmpty(x.Value)).Select(x => x.Value).ToList());
        }

        public async void ItemTextBox_KeyUp(object sender, KeyRoutedEventArgs ke)
        {
            if (ke.Key != VirtualKey.Enter) return;
            this.hardWareItemList.Add(new HwItemNameViewModel());
           //await Dispatcher.RunAsync(CoreDispatcherPriority.Low, () =>
           // {
           //     var textBoxes = VisualTreeTraverser.FindVisualChildren<TextBox>(itemList);
           //     textBoxes.Last().Focus(FocusState.Keyboard);
           // });

        }
        public void BackButton_Pressed(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }
    }

    public class HwItemNameViewModel : INotifyPropertyChanged
    {
        private string _value;
        public string Value { get { return _value; } set { _value = value; OnPropertyChanged(); } }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
