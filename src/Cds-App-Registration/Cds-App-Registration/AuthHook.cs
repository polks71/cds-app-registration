using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cds_App_Registration
{
    public class AuthHook : Microsoft.Xrm.Tooling.Connector.IOverrideAuthHookWrapper
    {
        private readonly string _organizationUrl;
        private readonly string _clientId;
        private readonly string _appKey;
        private readonly string _aadInstance = "https://login.microsoftonline.com/";
        private readonly string _tenantID;


        public AuthHook(string organizationUrl, string clientId, string appKey, string tenantID, string aadInstance = "")
        {
            _organizationUrl = organizationUrl;
            _clientId = clientId;
            _appKey = appKey;
            if (aadInstance != "")
                _aadInstance = aadInstance;
            _tenantID = tenantID;
        }


        // In memory cache of access tokens
        Dictionary<string, AuthenticationResult> accessTokens = new Dictionary<string, AuthenticationResult>();

        public void AddAccessToken(Uri orgUri, AuthenticationResult accessToken)
        {
            // Access tokens can be matched on the hostname,
            // different endpoints in the same organization can use the same access token
            accessTokens[orgUri.Host] = accessToken;
        }

        public string GetAuthToken(Uri connectedUri)
        {
            // Check if you have an access token for this host
            if (accessTokens.ContainsKey(connectedUri.Host) && accessTokens[connectedUri.Host].ExpiresOn > DateTime.Now)
            {
                return accessTokens[connectedUri.Host].AccessToken;
            }
            else
            {
                accessTokens[connectedUri.Host] = GetAccessTokenFromAzureAD(connectedUri);
            }
            return accessTokens[connectedUri.Host].AccessToken;
        }

        private AuthenticationResult GetAccessTokenFromAzureAD(Uri orgUrl)
        {
            ClientCredential clientcred = new ClientCredential(_clientId, _appKey);
            AuthenticationContext authenticationContext = new AuthenticationContext(_aadInstance + _tenantID);
            AuthenticationResult authenticationResult = authenticationContext.AcquireTokenAsync(_organizationUrl, clientcred).Result;

            accessTokens[new Uri(_organizationUrl).Host] = authenticationResult;

            return authenticationResult;
        }
    }
}
