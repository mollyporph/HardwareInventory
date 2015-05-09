using System;
using System.Collections.Generic;
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
            var request = new {Id = item.Id};
            await App.MobileService
                .InvokeApiAsync("CustomLoanReturn", JToken.FromObject(request));
        }
    }
}
