using System.Diagnostics;
using System.Threading.Tasks;
using NServiceBus;
using Store.Messages.RequestResponse;
using Store.Shared;

public class ProvisionDownloadHandler :
    IHandleMessages<ProvisionDownloadRequest>
{
    public Task Handle(ProvisionDownloadRequest message, IMessageHandlerContext context)
    {
        if (DebugFlagMutator.Debug)
        {
            Debugger.Break();
        }

        var products = string.Join(", ", message.ProductIds);

        var logger = context.GetOrderWorkflowLogger(message);
        logger.Information("Provision the products and make the Urls available to the Content management for download ...{products} product(s) to provision", message.ProductIds);

        var response = new ProvisionDownloadResponse
        {
            OrderNumber = message.OrderNumber,
            ProductIds = message.ProductIds,
            ClientId = message.ClientId
        };
        return context.Reply(response);
    }
}
