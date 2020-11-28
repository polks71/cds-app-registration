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
            //Retrieve the connection parameters from the app.config
            var organizationUrl = ConfigurationManager.AppSettings["organizationUrl"];
            var clientId = ConfigurationManager.AppSettings["clientId"]; ;
            var appKey = ConfigurationManager.AppSettings["appKey"]; ;
            var tenantID = ConfigurationManager.AppSettings["tenantID"]; ;

            //Set the AuthOverrideHook property on the CrmServiceClient
            CrmServiceClient.AuthOverrideHook = new AuthHook(organizationUrl, clientId, appKey, tenantID);
            //Create a new instance of the CrmServiceClient
            //The useUniqueInstanceParameter can be true if your needs require it
            var client = new CrmServiceClient(new Uri(organizationUrl), false);
            
            //Execute a simple query
            var query = new FetchExpression(FetchQuery);
            var results = client.RetrieveMultiple(query);
            foreach(var entity in results.Entities)
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
