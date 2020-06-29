namespace Store.Messages.Commands
{
    using NServiceBus;

    public class CancelHaveOrder :
        ICommand, IHaveOrderNumber, IHaveCustomerInfo
    {
        public int OrderNumber { get; set; }
        public string ClientId { get; set; }
    }
}