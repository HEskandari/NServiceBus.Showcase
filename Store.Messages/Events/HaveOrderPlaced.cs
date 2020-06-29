namespace Store.Messages.Events
{
    using NServiceBus;

    public class HaveOrderPlaced :
        IEvent, IHaveOrderNumber, IHaveCustomerInfo
    {
        public int OrderNumber { get; set; }
        public string[] ProductIds { get; set; }
        public string ClientId { get; set; }
    }
}