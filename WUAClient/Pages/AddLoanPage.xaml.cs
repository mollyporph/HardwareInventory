using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
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
using HardwareInventory.Repository;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HardwareInventory.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddLoanPage : Page
    {
        public AddLoanPage()
        {
            this.InitializeComponent();

        }

        public async void Dismiss(object sender, RoutedEventArgs e)
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
    }
}
