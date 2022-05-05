using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using System.Text.Json;

namespace DeliveryOrderProcessor
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

            try
            {
                using var client = new CosmosClient(config.GetValue<string>("ConnectionString"), new CosmosClientOptions { SerializerOptions = new CosmosSerializationOptions { PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase } });
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var order = JsonSerializer.Deserialize<OrderCosmosModel>(requestBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                order.Id = Guid.NewGuid().ToString();
                order.PartitionKey = Guid.NewGuid().ToString();

                var db = await client.CreateDatabaseIfNotExistsAsync(config.GetValue<string>("DbName"));

                var container = await db.Database.CreateContainerIfNotExistsAsync(config.GetValue<string>("ContainerName"), "/partitionKey");

                await container.Container.CreateItemAsync(order, new PartitionKey(order.PartitionKey));

                return new OkResult();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
