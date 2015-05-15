using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.WindowsAzure.Mobile.Service;
using dhcchardwareService.DataObjects;
using dhcchardwareService.Models;

namespace dhcchardwareService.Controllers
{
    public class LoanItemController : TableController<LoanItemDTO>
    {
        private dhcchardwareContext context;
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            context = new dhcchardwareContext();
            DomainManager = new EntityDomainManager<LoanItemDTO>(context, Request, Services);
        }

        // GET tables/Loan
        public IQueryable<LoanItemDTO> GetAllLoan()
        {
            return Query(); 
        }

        // GET tables/Loan/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<LoanItemDTO> GetLoan(string id)
        {

            return Lookup(id);

        }

        // PATCH tables/Loan/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<LoanItemDTO> PatchLoan(string id, Delta<LoanItemDTO> patch)
        {
            try
            {
                return UpdateAsync(id, patch);
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Debug.WriteLine("Type {0} Sate {1} Errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Debug.WriteLine(" --prop {0}  fault {1}", ve.PropertyName, ve.ErrorMessage);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return null;

        }

        // POST tables/Loan
        public async Task<IHttpActionResult> PostLoan(LoanItemDTO item)
        {

            //Oldschool
            //foreach (var hwItem in item.Items)

            //{
            //    context.Entry(hwItem).State = EntityState.Added;
            //}
            //context.Entry(item).State = EntityState.Added;
            try
            {
                LoanItemDTO current = await InsertAsync(item);
                return CreatedAtRoute("Tables", new {id = current.Id}, current);
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Debug.WriteLine("Type {0} Sate {1} Errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Debug.WriteLine(" --prop {0}  fault {1}", ve.PropertyName, ve.ErrorMessage);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return null;
           

        }

        // DELETE tables/Loan/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteLoan(string id)
        {
             return DeleteAsync(id);
        }

    }
}