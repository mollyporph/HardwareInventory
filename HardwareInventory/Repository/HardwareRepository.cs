using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HardwareInventory.Datamodel;
using Microsoft.WindowsAzure.MobileServices;

namespace HardwareInventory.Repository
{

    public static class HardwareRepository
    {
        private static readonly IMobileServiceTable<HardwareItem> hardwareTable = App.MobileService.GetTable<HardwareItem>();
        public static async Task<IEnumerable<HardwareItem>> GetHardwareItems()
        {
            return await hardwareTable.ToListAsync();
        }

        public static async Task<HardwareItem> GetHardwareItem(string id)
        {
            return await hardwareTable.LookupAsync(id);
        }

        public static async Task AddItems(List<HardwareItem> hwItems)
        {
            //TODO: Figure out why linq .ForEach doesn't work
            foreach (var item in hwItems)
            {
                await hardwareTable.InsertAsync(item);
            }
           
        }

        public static async Task AddItem(HardwareItem singleHwItem)
        {
            await hardwareTable.InsertAsync(singleHwItem);
        }
    }
}
