using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;

using static System.Console;

IConfiguration _configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddUserSecrets("12345678-AZ204-All-Demos")
    .Build();

// Reference: https://docs.microsoft.com/en-us/graph/sdks/choose-authentication-providers?tabs=CS
// The client credentials flow requires that you request the
// /.default scope, and preconfigure your permissions on the
// app registration in Azure. An administrator must grant consent
// to those permissions beforehand.
var scopes = new[] { "https://graph.microsoft.com/.default" };

// Multi-tenant apps can use "common",
// single-tenant apps must use the tenant ID from the Azure portal
var tenantId = _configuration["AzADGraphApiDemo:TenantId"];

// Values from app registration
var clientId = _configuration["AzADGraphApiDemo:ClientId"];
var clientSecret = _configuration["AzADGraphApiDemo:ClientSecret"];

// using Azure.Identity;
var options = new TokenCredentialOptions
{
    AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
};

// https://docs.microsoft.com/dotnet/api/azure.identity.clientsecretcredential
var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret, options);

var graphClient = new GraphServiceClient(clientSecretCredential, scopes);

var users = await graphClient.Users
    .Request()
    .GetAsync();

ForegroundColor = ConsoleColor.Yellow;
WriteLine("\nUsers Information");
foreach (var user in users)
{
    WriteLine($"Display Name: {user.DisplayName} | Given Name: {user.GivenName}");
}

var groups = await graphClient.Groups
    .Request()
    .GetAsync();
ForegroundColor = ConsoleColor.Green;
WriteLine("\nGroups Information");
foreach (var group in groups)
{
    WriteLine($"Display Name: {group.DisplayName} | Description: {group.Description}");
}

ResetColor();
WriteLine("\n\nPress any key ...");
