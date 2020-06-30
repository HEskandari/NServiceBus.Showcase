using System;
using System.Threading.Tasks;
using NServiceBus;
using Store.Shared;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Store.Operations";

        SerilogConfigurer.Configure();

        var endpointConfiguration = new EndpointConfiguration("Store.Operations");
        endpointConfiguration.ApplyCommonConfiguration();
        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}
