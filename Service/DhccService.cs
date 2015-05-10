using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using dhcchardwareService.DataObjects;
using Microsoft.WindowsAzure;
using Newtonsoft.Json;
using OAuth;

namespace dhcchardwareService
{
    public class DhccService : IDisposable
    {
        private OAuthRequest client;
        private string consumerKey, consumerSecret;
        private OAuthToken authToken;
        private string AuthHeader;
        public DhccService()
        {
            ServicePointManager.ServerCertificateValidationCallback =
                (sender, certificate, chain, errors) => true;
            //TODO: Fix oauth
            AuthHeader = CloudConfigurationManager.GetSetting("DHCC_BASIC_AUTH");

            //consumerKey = CloudConfigurationManager.GetSetting("DHCC_CONSUMER_KEY");
            //consumerSecret = CloudConfigurationManager.GetSetting("DHCC_CONSUMER_SECRET");
            //client = OAuthRequest.ForRequestToken(consumerKey, consumerSecret);
            //client.RequestUrl = "http://api.crew.dreamhack.se/oauth/request_token";
            //var auth = client.GetAuthorizationHeader();
            //var request = (HttpWebRequest)WebRequest.Create(client.RequestUrl);

            //request.Headers.Add("Authorization", auth);
            //var response = (HttpWebResponse)request.GetResponse();
            //authToken = JsonConvert.DeserializeObject<OAuthToken>(ReadHttpResponseString(response));
            //response.Close();
        }
        
    
    public CrewMember GetCrewMember(int id)
        {
            var request = (HttpWebRequest)WebRequest.Create($"https://api.crew.dreamhack.se/1/user/get/{id}");
            request.Method = "GET";
            request.ServicePoint.Expect100Continue = false;
        request.Headers["Authorization"] = AuthHeader;
            var response = (HttpWebResponse)request.GetResponse();
            return JsonConvert.DeserializeObject<CrewMember>(ReadHttpResponseString(response));
        }

        private static string ReadHttpResponseString(WebResponse response)
        {
            var receiveStream = response.GetResponseStream();
            if (receiveStream == null) return null;
            using (var readStream = new StreamReader(receiveStream, Encoding.UTF8))
            {

                return readStream.ReadToEnd();
            }

        }

        public void Dispose()
        {
            client = null;
            consumerSecret = consumerKey = null;
            authToken = null;
        }

        public CrewMember GetCrewMember(string username)
        {
            var request = (HttpWebRequest)WebRequest.Create($"https://api.crew.dreamhack.se/1/user/search/{username}");
            request.Method = "GET";
            request.ServicePoint.Expect100Continue = false;
            request.Headers["Authorization"] = AuthHeader;
            var response = (HttpWebResponse)request.GetResponse();
            var result = ReadHttpResponseString(response);
            return result == "false" ? new CrewMember() : JsonConvert.DeserializeObject<IEnumerable<CrewMember>>(result).FirstOrDefault();
        }
    }
}
