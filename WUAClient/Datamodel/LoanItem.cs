using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HardwareInventory.Utilities;
using Newtonsoft.Json;

namespace HardwareInventory.Datamodel
{
    public class LoanItem
    {
        public string Id { get; set; }

        [JsonProperty(PropertyName = "TeamName")]
        public string TeamName { get; set; }

        [JsonProperty(PropertyName = "LoanedBy")]
        public string LoanedBy { get; set; }
        [JsonProperty(PropertyName = "IsReturned")]
        public bool IsReturned { get; set; }
        [JsonProperty(PropertyName = "LoanedAt")]
        public DateTime? LoanedAt { get; set; }
        [JsonProperty(PropertyName = "ReturnedAt")]
        public DateTime? ReturnedAt { get; set; }
        [JsonProperty(PropertyName = "Item")]
        public HardwareItem Item { get; set; }
    }
}
