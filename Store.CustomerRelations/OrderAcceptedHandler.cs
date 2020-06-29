using System;
using System.Diagnostics;
using System.Threading.Tasks;
using NServiceBus;
using Store.Messages.Events;
using Store.Shared;

class OrderAcceptedHandler :
    IHandleMessages<HaveOrderAccepted>
{
    public Task Handle(HaveOrderAccepted message, IMessageHandlerContext context)
    {
        if (DebugFlagMutator.Debug)
        {
            Debugger.Break();
        }

        var logger = context.GetOrderWorkflowLogger(message);
        logger.Information("Customer: {ClientId} is now a preferred customer publishing for other service concerns", message.ClientId);
        
        // publish this event as an asynchronous event
        var clientBecamePreferred = new ClientBecamePreferred
        {
            ClientId = message.ClientId,
            PreferredStatusExpiresOn = DateTime.Now.AddMonths(2)
        };
        return context.Publish(clientBecamePreferred);
    }
}