using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using HardwareInventory.Datamodel;
using HardwareInventory.Repository;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HardwareInventory.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TeamDetailsPage : Page
    {
        private TeamLoanViewModel viewModel;
        private string TeamName;
        public TeamDetailsPage()
        {
            this.InitializeComponent();
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            TeamName = e.Parameter as string;
            await RefreshViewModel();
        }

        public async Task RefreshViewModel()
        {
            var result = (await LoanRepository.GetLoanItems(TeamName)).GroupBy(x => x.LoanedBy);
            loanList.ItemsSource = result;
        }
        public async void ReturnAllItems(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var source = button.DataContext as IGrouping<string, LoanItem>;
            var items = source.ToList();
           
            await LoanRepository.ReturnItems(items);
            await RefreshViewModel();
        }

        public async void ReturnSingleItem(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var item = button.DataContext as LoanItem;
            await LoanRepository.ReturnItem(item);
            await RefreshViewModel();
        }
    }
}
