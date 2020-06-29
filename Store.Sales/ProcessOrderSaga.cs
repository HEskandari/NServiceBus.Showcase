using System;
using System.Diagnostics;
using System.Threading.Tasks;
using NServiceBus;
using Store.Messages;
using Store.Messages.Commands;
using Store.Messages.Events;
using Store.Shared;

public class ProcessOrderSaga :
    Saga<ProcessOrderSaga.OrderData>,
    IAmStartedByMessages<SubmitHaveOrder>,
    IHandleMessages<CancelHaveOrder>,
    IHandleTimeouts<ProcessOrderSaga.BuyersRemorseIsOver>
{
    public Task Handle(SubmitHaveOrder message, IMessageHandlerContext context)
    {
        if (DebugFlagMutator.Debug)
        {
            Debugger.Break();
        }

        Data.OrderNumber = message.OrderNumber;
        Data.ProductIds = message.ProductIds;
        Data.ClientId = message.ClientId;

        var logger = context.GetOrderWorkflowLogger(message);
        logger.Information("Starting cool down period for order {OrderNumber}.", Data.OrderNumber);

        return RequestTimeout(context, TimeSpan.FromSeconds(20), new BuyersRemorseIsOver());
    }

    public Task Timeout(BuyersRemorseIsOver state, IMessageHandlerContext context)
    {
        if (DebugFlagMutator.Debug)
        {
            Debugger.Break();
        }

        MarkAsComplete();

        var logger = context.GetOrderWorkflowLogger(Data);
        logger.Information("Cooling down period for order {OrderNumber} has elapsed.", Data.OrderNumber);
        
        var orderAccepted = new HaveOrderAccepted
        {
            OrderNumber = Data.OrderNumber,
            ProductIds = Data.ProductIds,
            ClientId = Data.ClientId
        };
        return context.Publish(orderAccepted);
    }

    public Task Handle(CancelHaveOrder message, IMessageHandlerContext context)
    {
        if (DebugFlagMutator.Debug)
        {
            Debugger.Break();
        }

        var logger = context.GetOrderWorkflowLogger(message);
        logger.Information("Order {OrderNumber} was cancelled.", message.OrderNumber);

        MarkAsComplete();

        var orderCancelled = new HaveOrderCancelled
        {
            OrderNumber = message.OrderNumber,
            ClientId = message.ClientId
        };
        return context.Publish(orderCancelled);
    }

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderData> mapper)
    {
        mapper.ConfigureMapping<SubmitHaveOrder>(message => message.OrderNumber)
            .ToSaga(sagaData => sagaData.OrderNumber);
        mapper.ConfigureMapping<CancelHaveOrder>(message => message.OrderNumber)
            .ToSaga(sagaData => sagaData.OrderNumber);
    }

    public class OrderData :
        ContainSagaData, IHaveCustomerInfo, IHaveOrderNumber
    {
        public int OrderNumber { get; set; }
        public string[] ProductIds { get; set; }
        public string ClientId { get; set; }
    }

    public class BuyersRemorseIsOver
    {
    }

}