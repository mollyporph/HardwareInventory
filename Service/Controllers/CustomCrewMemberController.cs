using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using dhcchardwareService.DataObjects;
using Microsoft.WindowsAzure.Mobile.Service;

namespace dhcchardwareService.Controllers
{
    public class CustomCrewMemberController : ApiController
    {
        public ApiServices Services { get; set; }

        // GET api/CustomCrewMember
        public CrewMember Get(int id)
        {
            using (var client = new DhccService())
            {
                return client.GetCrewMember(id);
            }
        }
        public CrewMember Get(string username)
        {
            using (var client = new DhccService())
            {
                return client.GetCrewMember(username);
            }
        }

    }
}
