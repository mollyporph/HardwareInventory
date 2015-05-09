using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.WindowsAzure.Mobile.Service;
using dhcchardwareService.DataObjects;
using dhcchardwareService.Models;
using Microsoft.WindowsAzure.Mobile.Service.Security;
namespace dhcchardwareService.Controllers
{
    [AuthorizeLevel(AuthorizationLevel.User)]
    public class HardwareItemController : TableController<HardwareItemDTO>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            dhcchardwareContext context = new dhcchardwareContext();
            DomainManager = new EntityDomainManager<HardwareItemDTO>(context, Request, Services);
        }

        // GET tables/HardwareItem
        public IQueryable<HardwareItemDTO> GetAllHardwareItems()
        {
            return Query();
        }

        // GET tables/HardwareItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<HardwareItemDTO> GetHardwareItem(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/HardwareItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<HardwareItemDTO> PatchHardwareItem(string id, Delta<HardwareItemDTO> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/HardwareItem
        public async Task<IHttpActionResult> PostHardwareItem(HardwareItemDTO item)
        {
            HardwareItemDTO current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/HardwareItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteHardwareItem(string id)
        {
            return DeleteAsync(id);
        }
    }
}