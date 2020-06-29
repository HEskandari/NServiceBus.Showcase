using System.Diagnostics;
using System.Threading.Tasks;
using NServiceBus;
using Store.Messages.Events;
using Store.Shared;

class SendWelcomePacket :
    IHandleMessages<ClientBecamePreferred>
{
    public Task Handle(ClientBecamePreferred message, IMessageHandlerContext context)
    {
        if (DebugFlagMutator.Debug)
        {
            Debugger.Break();
        }
        
        var logger = context.GetOrderWorkflowLogger(message);
        logger.Information("Handler WhenCustomerIsPreferredSendWelcomeEmail invoked for CustomerId: {ClientId}", message.ClientId);
        
        return Task.CompletedTask;
    }
}