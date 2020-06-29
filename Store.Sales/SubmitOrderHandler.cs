using System.Diagnostics;
using System.Threading.Tasks;
using NServiceBus;
using Store.Messages.Commands;
using Store.Messages.Events;
using Store.Shared;

public class SubmitOrderHandler :
    IHandleMessages<SubmitHaveOrder>
{
    public Task Handle(SubmitHaveOrder message, IMessageHandlerContext context)
    {
        if (DebugFlagMutator.Debug)
        {
            Debugger.Break();
        }

        var logger = context.GetOrderWorkflowLogger(message);
        
        logger.Information("Received an order {OrderNumber} for {ProductIds} products(s).", message.OrderNumber, message.ProductIds);
        logger.Information("The credit card values will be encrypted when looking at the messages in the queues");
        logger.Information("CreditCard Number is {CreditCardNumber}", message.CreditCardNumber.EncryptedValue);
        logger.Information("CreditCard Expiration Date is {ExpirationDate}", message.ExpirationDate.EncryptedValue);
    
        // tell the client the order was received
        var orderPlaced = new HaveOrderPlaced
        {
            ClientId = message.ClientId,
            OrderNumber = message.OrderNumber,
            ProductIds = message.ProductIds
        };
        return context.Publish(orderPlaced);
    }
}

