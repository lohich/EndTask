using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;

namespace DeliveryOrderProcessor;
internal class OrderCosmosModel
{
    [JsonIgnore]
    public string Id { get; set; }
    public string PartitionKey { get; set; }
    public Address ShipToAddress { get; set; }
    public OrderItem[] OrderItems { get; set; }
    public decimal Total => OrderItems.Sum(x => x.UnitPrice);    
}
