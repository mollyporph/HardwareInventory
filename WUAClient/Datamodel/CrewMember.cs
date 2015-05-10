using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareInventory.Datamodel
{
    public class CrewMember
    {
        public int uid { get; set; }
        public string username { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string badge_picture { get; set; }
        public string profile_picture { get; set; }
    }
}
