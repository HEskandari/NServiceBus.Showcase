using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using NServiceBus;
using Store.Messages.Events;
using Store.Shared;

public class OrderIsReadyHandler :
    IHandleMessages<DownloadIsReady>
{
    IHubContext<OrdersHub> ordersHubContext;
    
    public OrderIsReadyHandler(IHubContext<OrdersHub> ordersHubContext)
    {
        this.ordersHubContext = ordersHubContext;
    }

    public Task Handle(DownloadIsReady message, IMessageHandlerContext context)
    {
        var logger = context.GetOrderWorkflowLogger(message);
        logger.Information("Order became ready. Sending product info.");

        return ordersHubContext.Clients.Client(message.ClientId).SendAsync("orderReady",
            new
            {
                message.OrderNumber,
                ProductUrls = message.ProductUrls.Select(pair => new {Id = pair.Key, Url = pair.Value}).ToArray()
            });
    }
}