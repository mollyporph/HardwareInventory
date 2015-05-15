using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
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

        public static async Task UpdateItem(HardwareItem hardwareItem)
        {
            await hardwareTable.UpdateAsync(hardwareItem);
        }

        public static async Task<IEnumerable<string>> GetSuggestedImagesForItem(string name)
        {
            //TODO: Replace with actual google ajaxapi
            var returnList = new List<string>();
            using (var client = new HttpClient())
            {
                var query = $"http://ajax.googleapis.com/ajax/services/search/images?v=1.0&q={name}";
                string result =
                    await client.GetStringAsync(query);
                var jObject = JsonObject.Parse(result);
                returnList.AddRange(jObject.GetNamedObject("responseData").GetNamedArray("results").Select(res => res.GetObject().GetNamedString("tbUrl")));
                return returnList;
            }
            //    return new List<string>()
            //{
            //    "http://t0.gstatic.com/images?q=tbn:ANd9GcTML1hjtmiEag-i5WtidvZ-HciT1DtsZIFhN_CDZgIJ2MJnNIymZXXUCC-C",
            //    "http://t0.gstatic.com/images?q=tbn:ANd9GcSCx7O-hOPlPkzKgf0tRJg-GNLU9cRyfOFC3eHP7swBKLWiHn02DwmwgkM",
            //    "http://t1.gstatic.com/images?q=tbn:ANd9GcScGU5hZJV7zTteD-eDM-twVz2OOgMo6qWVQCT7EzPEZnN_4rOhIborMQ",
            //    "http://t0.gstatic.com/images?q=tbn:ANd9GcTZr0b6AnlAppABimY7VPxm7a-pdOATumMpgYZV13Ox01SkbSk4u4GaJ6Y"
            //};

        }
    }
}
