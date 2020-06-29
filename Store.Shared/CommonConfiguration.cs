using System;
using System.Text;
using NServiceBus;
using NServiceBus.Encryption.MessageProperty;
using NServiceBus.MessageMutator;
using Serilog;

public static class CommonConfiguration
{
    public static void ApplyCommonConfiguration(this EndpointConfiguration endpointConfiguration,
        Action<TransportExtensions<LearningTransport>> messageEndpointMappings = null)
    {
        var serilogTracing = endpointConfiguration.EnableSerilogTracing(Log.Logger);
        var transport = endpointConfiguration.UseTransport<LearningTransport>();
        messageEndpointMappings?.Invoke(transport);
        endpointConfiguration.UsePersistence<LearningPersistence>();
        var defaultKey = "2015-10";
        var ascii = Encoding.ASCII;
        var encryptionService = new RijndaelEncryptionService(
            encryptionKeyIdentifier: defaultKey,
            key: ascii.GetBytes("gdDbqRpqdRbTs3mhdZh9qCaDaxJXl+e6"));
        endpointConfiguration.EnableMessagePropertyEncryption(encryptionService);
        endpointConfiguration.RegisterMessageMutator(new DebugFlagMutator());
        endpointConfiguration.AuditProcessedMessagesTo("audit");
        endpointConfiguration.SendFailedMessagesTo("error");
    }
}