using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using dhcchardwareService.Models;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace dhcchardwareService.Controllers
{
    [AuthorizeLevel(AuthorizationLevel.User)]
    public class CustomLoanReturnController : ApiController
    {
        private readonly dhcchardwareContext context = new dhcchardwareContext();
        public ApiServices Services { get; set; }

        // Post api/LoanReturn
        public HttpResponseMessage Post(CustomLoanReturnRequest request)
        {
            try
            {
                var loan = context.LoanItems.FirstOrDefault(x => x.Id == request.Id);
                if (loan == null) return Request.CreateBadRequestResponse("Loanitem does not exist");
                loan.IsReturned = true;
                context.Entry(loan).State = EntityState.Modified;
                context.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateBadRequestResponse(e.Message);
            }

        }

    }
}
