namespace Microsoft.eShopWeb.ApplicationCore.Options;
public class OrderItemReserverOptions
{
    public string OrderItemReserverQueueUrl { get; set; }
    public string OrderItemReserverQueueName { get; set; }

    public string DeliveryOrderProcessorUrl { get; set; }
}
