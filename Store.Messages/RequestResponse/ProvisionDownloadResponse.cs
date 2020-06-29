namespace Store.Messages.RequestResponse
{
    using NServiceBus;
    public class ProvisionDownloadResponse :
        IMessage, IHaveOrderNumber, IHaveCustomerInfo
    {
        public int OrderNumber { get; set; }
        public string[] ProductIds { get; set; }
        public string ClientId { get; set; }
    }
}