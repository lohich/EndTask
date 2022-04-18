﻿using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using System.Web.Http;
using System.Text;

namespace OrderItemReserver
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req, ExecutionContext context)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var order = JsonConvert.DeserializeObject<Order>(requestBody);

            var serviceClient = new BlobServiceClient(config.GetValue<string>("ConnectionString"));

            var containerClient = serviceClient.GetBlobContainerClient(config.GetValue<string>("ContainerName"));

            var blobClient = containerClient.GetBlobClient($"Order {order.Id}.json");

            await blobClient.UploadAsync(new MemoryStream(Encoding.UTF8.GetBytes(requestBody)));

            return new OkResult();
        }
    }
}