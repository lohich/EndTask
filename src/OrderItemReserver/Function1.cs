using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;

namespace OrderItemReserver
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task Run(
            [ServiceBusTrigger("cloudxqueue", Connection = "ServiceBusConnection")] string req, ExecutionContext context)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var serviceClient = new BlobServiceClient(config.GetValue<string>("ConnectionString"));

            var containerClient = serviceClient.GetBlobContainerClient(config.GetValue<string>("ContainerName"));

            var blobClient = containerClient.GetBlobClient($"Order {Guid.NewGuid()}.json");

            await blobClient.UploadAsync(new MemoryStream(Encoding.UTF8.GetBytes(req)));
        }
    }
}
