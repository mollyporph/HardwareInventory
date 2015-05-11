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
    public sealed partial class TeamLoanPage : Page
    {
        public TeamLoanPage()
        {
            this.InitializeComponent();
           

        }


        private async void TeamLoanPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            //TODO: autoload
            this.GridView.ItemsSource = await GetLoanPerTeam();
        }

        private static async Task<IEnumerable<TeamLoanViewModel>> GetLoanPerTeam()
        {
            var items = await LoanRepository.GetLoanItems();
            var loanItems = items as LoanItem[] ?? items.ToArray();
            var teamLoanViewModels = loanItems.Select(x => x.TeamName).Distinct().Select(teamName =>
            {
               
                 return new TeamLoanViewModel{
                          TeamName = teamName, LoanItemCount = loanItems.Count(x => x.TeamName == teamName && !x.IsReturned),
                          LoanItems = loanItems.Where(x => x.TeamName == teamName).ToList()
                          };
            }).ToList();
            return teamLoanViewModels;
        }

        private void GridViewItemClick(object sender, ItemClickEventArgs e)
        {
            var teamLoanViewModel = e.ClickedItem as TeamLoanViewModel;
            if (teamLoanViewModel == null) return; //TODO: Throw something?
            var teamName = teamLoanViewModel.TeamName;
            this.Frame.Navigate(typeof(TeamDetailsPage), teamName);
        }

        private void BackButton_Pressed(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }
    }
}
