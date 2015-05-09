using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;
using System.Security.Claims;
using dhcchardwareService.Models;
using dhcchardwareService.DataObjects;
using dhcchardwareService.Utils;

namespace dhcchardwareService.Controllers
{
    [AuthorizeLevel(AuthorizationLevel.Anonymous)]
    public class CustomLoginController : ApiController
    {
        public ApiServices Services { get; set; }
        public IServiceTokenHandler handler { get; set; }

        // POST api/CustomLogin
        public HttpResponseMessage Post(LoginRequest loginRequest)
        {
            var context = new dhcchardwareContext();
            var account = context.Accounts.SingleOrDefault(a => a.Username == loginRequest.username);
            if (account == null)
                return this.Request.CreateResponse(HttpStatusCode.Unauthorized,
                    "Invalid username or password");
            byte[] incoming = CustomLoginProviderUtils
                .hash(loginRequest.password, account.Salt);

            if (!CustomLoginProviderUtils.slowEquals(incoming, account.SaltedAndHashedPassword))
                return this.Request.CreateResponse(HttpStatusCode.Unauthorized,
                    "Invalid username or password");
            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, loginRequest.username));
            var loginResult = new CustomLoginProvider(handler)
                .CreateLoginResult(claimsIdentity, Services.Settings.MasterKey);
            var customLoginResult = new CustomLoginResult()
            {
                UserId = loginResult.User.UserId,
                MobileServiceAuthenticationToken = loginResult.AuthenticationToken
            };
            return this.Request.CreateResponse(HttpStatusCode.OK, customLoginResult);
        }
    }

}
