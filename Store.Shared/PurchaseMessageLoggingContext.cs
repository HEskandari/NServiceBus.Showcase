namespace Store.Shared
{
    using NServiceBus;
    using Serilog;
    using Serilog.Core;
    using Serilog.Events;
    using Store.Messages;

    public static class MessageHandlerContextExtension
    {
        public static ILogger GetOrderWorkflowLogger(this IMessageHandlerContext context, object data)
        {
            var logger = context.Logger();
            return logger.ForContext(new PurchaseMessageLoggingContext(data));
        }
    } 

    public class PurchaseMessageLoggingContext : ILogEventEnricher
    {
        object data;

        public PurchaseMessageLoggingContext(object data)
        {
            this.data = data;
        }
        
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (data is IHaveCustomerInfo customerMsg)
            {
                AddProperty(logEvent, propertyFactory, nameof(customerMsg.ClientId), customerMsg.ClientId);
            }

            if (data is IHaveOrderNumber orderMsg)
            {
                AddProperty(logEvent, propertyFactory, nameof(orderMsg.OrderNumber), orderMsg.OrderNumber);
            }
        }

        void AddProperty(LogEvent logEvent, ILogEventPropertyFactory propertyFactory, string name, object value)
        {
            var property = propertyFactory.CreateProperty(name, value);
            logEvent.AddPropertyIfAbsent(property);
        }
    }
}