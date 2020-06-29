using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using NServiceBus;
using Store.Messages.Events;
using Store.Shared;

public class OrderCancelledHandler :
    IHandleMessages<HaveOrderCancelled>
{
    IHubContext<OrdersHub> ordersHubContext;
    
    public OrderCancelledHandler(IHubContext<OrdersHub> ordersHubContext)
    {
        this.ordersHubContext = ordersHubContext;
    }

    public Task Handle(HaveOrderCancelled message, IMessageHandlerContext context)
    {
        var logger = context.GetOrderWorkflowLogger(message);
        logger.Information("Order for client {ClientId} was cancelled.", message.ClientId);
        
        return ordersHubContext.Clients.Client(message.ClientId).SendAsync("orderCancelled",
            new
            {
                message.OrderNumber,
            });
    }
}
