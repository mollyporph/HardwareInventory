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
using HardwareInventory.Menu;
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
        private readonly LoginViewModel _loginViewModel = new LoginViewModel();
        private MobileServiceUser _user;
        public LoginPage()
        {
            this.InitializeComponent();
            this.LoginUserControl.DataContext = _loginViewModel;
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
                username = _loginViewModel.Username,
                password = _loginViewModel.Password
            };

            try
            {
                var jTokenResult = await App.MobileService
                    .InvokeApiAsync("CustomLogin", JToken.FromObject(credentials));
                _user = JsonConvert.DeserializeObject<MobileServiceUser>(jTokenResult.ToString());  
            }
            catch (InvalidOperationException)
            {
                _loginViewModel.ValidationErrorMessage = "Wrong username or password entered.";
                _loginViewModel.ValidationFailed = true;

            }
            if(_user != null)
            {
                App.MobileService.CurrentUser = _user;
                NavigationHandler.Navigate(NavigationCommand.Home, Frame);
            }
        }
    }
}
