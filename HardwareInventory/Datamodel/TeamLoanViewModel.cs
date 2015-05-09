using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareInventory.Datamodel
{
    public class TeamLoanViewModel
    {
        public string TeamName { get; set; }
        public int LoanItemCount { get; set; }
        public IEnumerable<LoanItem> LoanItems { get; set; } 
    }
}
