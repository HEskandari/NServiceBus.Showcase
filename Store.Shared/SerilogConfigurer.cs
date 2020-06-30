using NServiceBus.Logging;
using NServiceBus.Serilog;
using Serilog;

namespace Store.Shared
{
    public static class SerilogConfigurer
    {
        public static void Configure()
        {
            Log.Logger = new LoggerConfiguration()
              .Enrich.FromLogContext()
              .WriteTo.Seq(serverUrl: "http://192.168.0.110:8889")
              .WriteTo.Console()
              .CreateLogger();

            LogManager.Use<SerilogFactory>();
        }
    }
}
