using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using HardwareInventory.Datamodel;
using HardwareInventory.Repository;
using HardwareInventory.Utilities;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HardwareInventory.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TeamDetailsPage : Page
    {
        private TeamLoanViewModel viewModel;
        public static string TeamName;
        private IEnumerable<LoanItem> _selectedLoanItems = new List<LoanItem>();
        public TeamDetailsPage()
        {
            this.InitializeComponent();
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            TeamName = e.Parameter as string;
            teamNameTextBlock.Text = TeamName;
            await RefreshViewModel();
        }

        private async Task RefreshViewModel()
        {
            var result = (await LoanRepository.GetLoanItems(TeamName)).GroupBy(x => x.LoanedBy);
            loanList.ItemsSource = result;
        }
     
        private async void ReturnItemsForUser(object sender, RoutedEventArgs e)
        {
            var button = sender as HyperlinkButton;
            var source = button.DataContext as IGrouping<string, LoanItem>;
            var items = source.ToList();
            await ReturnItems(items);
        }

        private async void ReturnSelected(object sender, RoutedEventArgs e)
        {
            var selectedLoanItems = (from ListItem in VisualTreeTraverser.FindVisualChildren<ListViewItem>(loanList)
                                     where ListItem.IsSelected
                                     select ListItem.Content as LoanItem).ToList();
            await ReturnItems(selectedLoanItems);

        }

        private async Task ReturnItems(IEnumerable<LoanItem> loanItems)
        {
            var selectedLoanItems = loanItems as LoanItem[] ?? loanItems.ToArray();

            //add to public variable for commandinvoker to be aware of it.
            _selectedLoanItems = selectedLoanItems;
            var contentString = string.Join(Environment.NewLine,
                selectedLoanItems.Select(x => $"Team: {x.TeamName} User:{x.LoanedBy} Item: {x.Item.Name}"));

            var messageDialog = new MessageDialog(contentString, "Really return the following items?");

            messageDialog.Commands.Add(new UICommand(
                "Yes",
                CommandInvokedHandler));
            messageDialog.Commands.Add(new UICommand(
                "No",
                CommandInvokedHandler));
            // Set the command that will be invoked by default
            messageDialog.DefaultCommandIndex = 0;

            // Set the command to be invoked when escape is pressed
            messageDialog.CancelCommandIndex = 1;
            await messageDialog.ShowAsync();
        }
        private async void CommandInvokedHandler(IUICommand command)
        {
            // Display message showing the label of the command that was invoked
            if (!string.Equals(command.Label, "Yes", StringComparison.CurrentCultureIgnoreCase)) return;
            await LoanRepository.ReturnItems(_selectedLoanItems);
            await RefreshViewModel();
        }
    }
}
