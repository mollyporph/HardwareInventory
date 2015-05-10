using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dhcchardwareService.DataObjects
{
    public class OAuthToken
    {
        public string oauth_token { get; set; }
        public string oauth_token_secret { get; set; }
        public bool oauth_callback_confirmed { get; set; }
    }
}
