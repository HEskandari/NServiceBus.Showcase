using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Config;
using Store.Shared;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Store.ContentManagement";

        SerilogConfigurer.Configure();
        
        var endpointConfiguration = new EndpointConfiguration("Store.ContentManagement");
        endpointConfiguration.ApplyCommonConfiguration(transport =>
        {
            var routing = transport.Routing();
            routing.RouteToEndpoint(typeof(Store.Messages.RequestResponse.ProvisionDownloadRequest), "Store.Operations");
        });

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}
