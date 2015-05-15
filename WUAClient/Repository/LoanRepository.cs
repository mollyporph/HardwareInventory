using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HardwareInventory.Datamodel;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;

namespace HardwareInventory.Repository
{
    public static class LoanRepository
    {
        private static readonly IMobileServiceTable<LoanItem> LoanTable = App.MobileService.GetTable<LoanItem>();
        public static async Task<IEnumerable<LoanItem>> GetLoanItems(bool IncludeReturned = false)
        {
            return await LoanTable.Where(x => x.IsReturned == IncludeReturned).ToListAsync();
        }
        public static async Task<IEnumerable<LoanItem>> GetLoanItems(string TeamName, bool IncludeReturned = false)
        {
            return await LoanTable.Where(x => x.TeamName == TeamName && x.IsReturned == IncludeReturned).ToListAsync();
        }
        public static async Task<IEnumerable<LoanItem>> GetLoanItemsForUser(string UserName, bool IncludeReturned = false)
        {
            return await LoanTable.Where(x => x.LoanedBy == UserName && x.IsReturned == IncludeReturned).ToListAsync();
        }
        public static async Task<LoanItem> GetLoanItem(string Id)
        {
            return await LoanTable.LookupAsync(Id);
        }

        public static async Task AddLoan(LoanItem loanItem)
        {

            await LoanTable.InsertAsync(loanItem);
        }

        public static async Task ReturnItems(IEnumerable<LoanItem> loanItems)
        {
            foreach (var item in loanItems)
            {
                await ReturnItem(item);
            }
        }

        public static async Task ReturnItem(LoanItem item)
        {
            item.ReturnedAt = DateTime.Now;
            item.IsReturned = true;
            //To avoid conflicts
            item.Item = null;
            await LoanTable.UpdateAsync(item);
        }

        public static async void CreateLoan(string username, IEnumerable<string> hardWareItemList)
        {
            var availableItems = await HardwareRepository.GetHardwareItems();
            foreach (var hwItem in hardWareItemList)
            {
                var newLoan = new LoanItem
                {
                    IsReturned = false,
                    LoanedAt = DateTime.Now,
                    LoanedBy = username
                };
                var hardwareItems = availableItems as HardwareItem[] ?? availableItems.ToArray();
                if (hardwareItems.Select(x => x.Name.ToLower()).Contains(hwItem.ToLower()))
                {
                    newLoan.ItemId = hardwareItems.FirstOrDefault(x => string.Equals(x.Name, hwItem, StringComparison.CurrentCultureIgnoreCase)).Id;
                }
                else
                {
                    var newItem = new HardwareItem
                    {
                        Category = "",
                        ImageUrl = "",
                        Name = hwItem
                    };
                    await HardwareRepository.AddItem(newItem);
                    newLoan.ItemId = newItem.Id;


                }
                try
                {
                    await LoanTable.InsertAsync(newLoan);
                }
                catch (Exception e)
                {
                    //Item allready exist, create duplicate
                    newLoan.Item.Id = Guid.NewGuid().ToString();
                    await LoanTable.InsertAsync(newLoan);
                }
            }
        }
    }
}
