using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareInventory.Datamodel
{
    public enum NavigationCommand
    {
        Home,
        NewLoan,
        HardwareItemList,
        TeamLoanOverview,
        TeamDetails,
        UserDetails,
        HardwareItemDetails
    }
    public class MenuItem
    {
        public NavigationCommand Command { get; set; }
        public string FriendlyName { get; set; }
    }
}
