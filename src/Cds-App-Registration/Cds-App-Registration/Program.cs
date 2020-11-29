using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cds_App_Registration
{
    class Program
    {
        static void Main(string[] args)
        {
            var query = new FetchExpression(FetchQuery);
            //Use Oath Connection string
            //use a using here to make a vain attempt to ensure the client is actually disposed before the second query
            using (var connectionStringClient = new CrmServiceClient(ConfigurationManager.ConnectionStrings["cds-connection-string"].ConnectionString))
            {
                if (connectionStringClient.IsReady)
                {
                    WhoAmIRequest req = new WhoAmIRequest();
                    WhoAmIResponse res = (WhoAmIResponse)connectionStringClient.Execute(req);
                    Console.WriteLine("WhoAmI Response:" + res.UserId);
                }

                //Execute a simple query
                
                var connectionStringresults = connectionStringClient.RetrieveMultiple(query);
                foreach (var entity in connectionStringresults.Entities)
                {
                    Console.WriteLine(entity.TryGetAttributeValue("name", "DefaultName"));
                }
            }            

            //Retrieve the connection parameters from the app.config
            var organizationUrl = ConfigurationManager.AppSettings["organizationUrl"];
            var clientId = ConfigurationManager.AppSettings["clientId"];
            var appKey = ConfigurationManager.AppSettings["appKey"];
            var tenantID = ConfigurationManager.AppSettings["tenantID"]; 

            //Set the AuthOverrideHook property on the CrmServiceClient
            CrmServiceClient.AuthOverrideHook = new AuthHook(organizationUrl, clientId, appKey, tenantID);
            //Create a new instance of the CrmServiceClient
            //The useUniqueInstanceParameter can be true if your needs require it
            var authHookClient = new CrmServiceClient(new Uri(organizationUrl), false);
            if (authHookClient.IsReady)
            {
                WhoAmIRequest req = new WhoAmIRequest();
                WhoAmIResponse res = (WhoAmIResponse)authHookClient.Execute(req);
                Console.WriteLine("WhoAmI Response:" + res.UserId);
            }

            //Execute a simple query
            var authHookResults = authHookClient.RetrieveMultiple(query);
            foreach(var entity in authHookResults.Entities)
            {
                Console.WriteLine(entity.TryGetAttributeValue("name", "DefaultName"));
            }

            
        }

        static string FetchQuery = @"<fetch version=""1.0"" output-format=""xml-platform"" mapping=""logical"" distinct=""false"">
                                      <entity name=""account"">
                                        <attribute name=""name"" />
                                        <attribute name=""primarycontactid"" />
                                        <attribute name=""telephone1"" />
                                        <attribute name=""accountid"" />
                                        <order attribute=""name"" descending=""false"" />
                                        <filter type=""and"">
                                          <condition attribute=""name"" operator=""like"" value=""%Datum%"" />
                                        </filter>
                                      </entity>
                                    </fetch>";
    }
}
