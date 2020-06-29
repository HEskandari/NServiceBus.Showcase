using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using NServiceBus;
using Store.Messages.Events;
using Store.Shared;

public class OrderPlacedHandler :
    IHandleMessages<HaveOrderPlaced>
{
    IHubContext<OrdersHub> ordersHubContext;
    
    public OrderPlacedHandler(IHubContext<OrdersHub> ordersHubContext)
    {
        this.ordersHubContext = ordersHubContext;
    }

    public Task Handle(HaveOrderPlaced message, IMessageHandlerContext context)
    {
        var logger = context.GetOrderWorkflowLogger(message);
        logger.Information("Order placed for customer {ClientId}.", message.ClientId);
        
        return ordersHubContext.Clients.Client(message.ClientId).SendAsync("orderReceived",
            new
            {
                message.OrderNumber,
                message.ProductIds
            });
    }
}