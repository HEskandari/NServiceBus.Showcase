namespace Store.Messages.Events
{
    using NServiceBus;

    public class HaveOrderCancelled :
        IEvent, IHaveOrderNumber, IHaveCustomerInfo
    {
        public int OrderNumber { get; set; }
        public string ClientId { get; set; }
    }
}