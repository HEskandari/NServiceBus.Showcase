using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Serilog;
using Serilog;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Store.ContentManagement";
        
        CreateLogger();
        
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
    
    static void CreateLogger()
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Seq(serverUrl: "http://localhost:8889")
            .WriteTo.Console()
            .CreateLogger();
            
        LogManager.Use<SerilogFactory>();
    }
}
