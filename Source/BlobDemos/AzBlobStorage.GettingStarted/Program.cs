// See https://aka.ms/new-console-template for more information
using AzBlobStorage.GettingStarted;
using Azure.Identity;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;

IConfiguration _configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddUserSecrets("12345678-AZ204-All-Demos")
    .Build();

// To Show Case Delete Blob with Snapshots
var deleteFile = false;
var deleteBlobContainer = false;

// Copy the connection string from the portal in the variable below.
//string storageConnectionString = _configuration["AzStorage:BlobStorageConnectionString"];
//BlobServiceClient blobServiceClient = new(storageConnectionString);

// AD App with RBAC
var tenantId = _configuration["AzADStBlobDemo:TenantId"];
var clientId = _configuration["AzADStBlobDemo:ClientId"];
var clientSecret = _configuration["AzADStBlobDemo:ClientSecret"];

// using Azure.Identity;
var options = new TokenCredentialOptions
{
    AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
};

var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret, options);
var blobUri = "https://staz204authdev001.blob.core.windows.net";
BlobServiceClient blobServiceClient = new(new Uri(blobUri), clientSecretCredential);

string containerName = _configuration["AzStorage:BlobContainerName"];
var blobContainerClient = await BlobGettingStartedHelper.CreateContainerAsync(blobServiceClient, containerName);

string fileForSnapshotAndDelete = _configuration["AzStorage:FileForSnapshotAndDelete"];
foreach (var fileEntry in Directory.GetFiles(_configuration["AzStorage:FilesLocation"]))
{
    await BlobGettingStartedHelper.UploadBlobAsync(blobContainerClient, fileEntry, fileForSnapshotAndDelete, deleteFile);
}

await BlobGettingStartedHelper.ListBlobAsync(blobContainerClient);

if (deleteBlobContainer)
{
    await blobContainerClient.DeleteIfExistsAsync();
}

Console.WriteLine("\n\nPress any key ...");


