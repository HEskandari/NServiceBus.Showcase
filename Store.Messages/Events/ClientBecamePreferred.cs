namespace Store.Messages.Events
{
    using System;
    using NServiceBus;

    public class ClientBecamePreferred :
        IEvent, IHaveCustomerInfo
    {
        public string ClientId { get; set; }
        public DateTime PreferredStatusExpiresOn { get; set; }
    }
}