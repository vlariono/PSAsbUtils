using System.Management.Automation;
using Azure.Identity;
using Azure.Messaging.ServiceBus;
using PsAsbUtils.Cmdlets.Cmdlets.Base;
using PsAsbUtils.Cmdlets.Constants;
using PsAsbUtils.Cmdlets.Core;
using PsAsbUtils.Cmdlets.Interfaces;

namespace PsAsbUtils.Cmdlets.Cmdlets;

[Cmdlet(VerbsCommunications.Connect, $"{CmdletConst.Prefix}Namespace")]
[OutputType(typeof(IServiceBusConnection))]
public class ConnectServiceBusNamespace : ServiceBusClientCmdlet
{
    private static readonly ServiceBusClientOptions s_serviceBusClientOptions = new()
    {
        TransportType = ServiceBusTransportType.AmqpWebSockets
    };

    /// <summary>
    /// ServiceBus namespace
    /// </summary>
    [Parameter(Mandatory = true)]
    public string Namespace { get; set; } = null!;

    /// <summary>
    /// ServiceBus's connection string
    /// </summary>
    [Parameter(Mandatory = true, ParameterSetName = nameof(ServiceBusCredentialType.ConnectionString))]
    [ValidateNotNullOrEmpty]
    public string ConnectionString { get; set; } = null!;

    /// <summary>
    /// ServiceBus's connection string
    /// </summary>
    [Parameter(Mandatory = true, ParameterSetName = nameof(ServiceBusCredentialType.Powershell))]
    public SwitchParameter AzurePowershell { get; set; }

    protected override Task ProcessRecordAsync(CancellationToken cancellationToken)
    {
        var client = CreateServiceBusClient();
        var connection = PSServiceBusConnection.Create(client);

        WriteObject(connection);
        return Task.CompletedTask;
    }

    private ServiceBusClient CreateServiceBusClient() => ParameterSetName switch
    {
        nameof(ServiceBusCredentialType.Powershell) => new ServiceBusClient(Namespace, new AzurePowerShellCredential(), s_serviceBusClientOptions),
        nameof(ServiceBusCredentialType.ConnectionString) => new ServiceBusClient(ConnectionString, s_serviceBusClientOptions),
        _ => throw new NotImplementedException()
    };
}
