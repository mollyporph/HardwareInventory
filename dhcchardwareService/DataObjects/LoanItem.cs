using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Mobile.Service;

namespace dhcchardwareService.DataObjects
{
    public class LoanItem  
    {
        public string Id { get; set; }
        public string LoanedBy { get; set; }
        public string TeamName { get; set; }
        public DateTime? LoanedAt { get; set; }
        public DateTime? ReturnedAt { get; set; }
        public bool IsReturned { get; set; }
        public virtual HardwareItem Item { get; set; }
    }

    public class LoanItemDTO: EntityData
    {
        public string LoanedBy { get; set; }
        public string TeamName { get; set; }
        public DateTime? LoanedAt { get; set; }
        public DateTime? ReturnedAt { get; set; }
        public bool IsReturned { get; set; }
        public virtual HardwareItemDTO Item { get; set; } 

    }
}
