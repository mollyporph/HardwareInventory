using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HardwareInventory.Utilities
{
    public class LoanItemExpandHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage>
        SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            bool requestToTodoTable = request.RequestUri.PathAndQuery
                .StartsWith("/tables/loanItem", StringComparison.OrdinalIgnoreCase)
                    && request.Method == HttpMethod.Get;
            if (requestToTodoTable)
            {
                UriBuilder builder = new UriBuilder(request.RequestUri);
                string query = builder.Query;
                if (!query.Contains("$expand"))
                {
                    if (string.IsNullOrEmpty(query))
                    {
                        query = string.Empty;
                    }
                    else
                    {
                        query = query + "&";
                    }

                    query = query + "$expand=item";
                    builder.Query = query.TrimStart('?');
                    request.RequestUri = builder.Uri;
                }
            }

            var result = await base.SendAsync(request, cancellationToken);
            return result;
        }
    }
}
