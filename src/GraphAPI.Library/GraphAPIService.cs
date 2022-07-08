using Azure.Identity;
using GraphAPI.Library.Interfaces;
using GraphAPI.Library.Models;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;

namespace GraphAPI.Library
{
    public class GraphAPIService : IGraphAPIService
    {
        protected readonly GraphApiConfig _graphApiConfig;
        protected readonly GraphServiceClient _graphServiceClient;
        
        // The client credentials flow requires that you request the
        // /.default scope, and preconfigure your permissions on the
        // app registration in Azure. An administrator must grant consent
        // to those permissions beforehand.
        public static readonly string[] scopesClientCredentials = { "https://graph.microsoft.com/.default" };

        public GraphAPIService(GraphApiConfig graphApiConfig)
        {
            _graphApiConfig = graphApiConfig;

            var confidentialClient = ConfidentialClientApplicationBuilder
                .Create(_graphApiConfig.ClientId)
                .WithAuthority($"https://login.microsoftonline.com/{_graphApiConfig.TenantId}/v2.0")
                .WithClientSecret(_graphApiConfig.ClientSecret)
                .Build();

            _graphServiceClient = new GraphServiceClient(new DelegateAuthenticationProvider(async (requestMessage) => {

                // Retrieve an access token for Microsoft Graph (gets a fresh token if needed).
                var authResult = await confidentialClient.AcquireTokenForClient(scopesClientCredentials).ExecuteAsync();

                // Add the access token in the Authorization header of the API
                requestMessage.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", authResult.AccessToken);

            }));
        }
        public async Task<bool> CheckGroupByUserAsync(string groupId, string userObjectId)
        {
            var securityEnabledOnly = false;

            var prova = await _graphServiceClient.DirectoryObjects[userObjectId].GetMemberGroups(securityEnabledOnly).Request().PostAsync();
            if(prova.CurrentPage.Count(z => z.Contains(groupId)) > 0)
            {
                return true;
            } 
            return false;
        }

     
    }
}