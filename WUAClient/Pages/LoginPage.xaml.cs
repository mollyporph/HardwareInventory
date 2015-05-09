using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using HardwareInventory.Datamodel;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HardwareInventory.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        private LoginViewModel LoginViewModel = new LoginViewModel();
        private MobileServiceUser user;
        public LoginPage()
        {
            this.InitializeComponent();
            this.LoginUserControl.DataContext = LoginViewModel;
        }
        private async void OnKeyDownHandler(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                await AuthenticateAsync();
            }
        }

        private async Task AuthenticateAsync()
        {
            var credentials = new AccountModel
            {
                username = LoginViewModel.Username,
                password = LoginViewModel.Password
            };

            try
            {
                var jTokenResult = await App.MobileService
                    .InvokeApiAsync("CustomLogin", JToken.FromObject(credentials));
                user = JsonConvert.DeserializeObject<MobileServiceUser>(jTokenResult.ToString());  
            }
            catch (InvalidOperationException)
            {
                LoginViewModel.ValidationErrorMessage = "Wrong username or password entered.";
                LoginViewModel.ValidationFailed = true;

            }
            if(user != null)
            {
                App.MobileService.CurrentUser = user;
                this.Frame.Navigate(typeof (MainPage));
            }
        }

        public async void CreateAccount(object sende, RoutedEventArgs e)
        {
            var credentials = new AccountModel
            {
                username = "test",
                password = "TestingStuff"
            };
            await App.MobileService.InvokeApiAsync("CustomRegistration", JToken.FromObject(credentials));
        }
    }
}
