using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using HardwareInventory.Datamodel;
using HardwareInventory.Pages;

namespace HardwareInventory.Menu
{
   
    public class NavigationHandler
    {
        public static IEnumerable<MenuItem> MenuItems = new List<MenuItem>()
        {
            new MenuItem
            {
                Command = NavigationCommand.Home,
                FriendlyName = "Home"
            },
            new MenuItem
            {
                Command = NavigationCommand.InventoryList,
                FriendlyName = "Inventory"
            },
            new MenuItem
            {
                Command = NavigationCommand.NewLoan,
                FriendlyName = "New Loan"
            },
            new MenuItem
            {
                Command = NavigationCommand.TeamDetails,
                FriendlyName = "Team Details"
            },
            new MenuItem
            {
                Command = NavigationCommand.TeamLoanOverview,
                FriendlyName = "Team Overview"
            },
            new MenuItem
            {
                Command = NavigationCommand.UserDetails,
                FriendlyName = "Loans For User"
            }
        };

        public static void Navigate(NavigationCommand navigationCommand, Frame frame,object navigationData = null)
        {
            switch (navigationCommand)
            {
                case NavigationCommand.Home:
                    frame.Navigate(typeof (MainPage), navigationData);
                    break;
                case NavigationCommand.NewLoan:
                    frame.Navigate(typeof(AddLoanPage), navigationData);
                    break;
                case NavigationCommand.InventoryList:
                    throw new ArgumentException("Not implemented yet");

                case NavigationCommand.TeamLoanOverview:
                    frame.Navigate(typeof(TeamLoanPage), navigationData);
                    break;
                case NavigationCommand.TeamDetails:
                    frame.Navigate(typeof(TeamDetailsPage), navigationData);
                    break;
                case NavigationCommand.UserDetails:
                    throw new ArgumentException("Not implemented yet");
                default:
                    throw new ArgumentOutOfRangeException(nameof(navigationCommand), navigationCommand, null);
            }
        }
    }
}
