using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.ApplicationCore.Options;
public class OrderItemReserverOptions
{
    public string OrderItemReserverQueueUrl { get; set; }
    public string OrderItemReserverQueueName { get; set; }
}
