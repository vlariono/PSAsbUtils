﻿using System.Management.Automation;
using Azure.Identity;
using Azure.Messaging.ServiceBus;
using PsAsbUtils.Cmdlets.Cmdlets.Base;
using PsAsbUtils.Cmdlets.Constants;
using PsAsbUtils.Cmdlets.Core;
using PsAsbUtils.Cmdlets.Interfaces;

namespace PsAsbUtils.Cmdlets.Cmdlets;

[Cmdlet(VerbsCommunications.Connect, $"{PsModule.Prefix}Namespace")]
[OutputType(typeof(IServiceBusConnection))]
public class ConnectServiceBusNamespace : PsAsyncCmdlet
{
    private static readonly ServiceBusClientOptions s_serviceBusClientOptions = new()
    {
        TransportType = ServiceBusTransportType.AmqpWebSockets
    };

    /// <summary>
    /// ServiceBus namespace
    /// </summary>
    [Parameter(Mandatory = true, ParameterSetName = PsConnection.Powershell, Position = PsPosition.First)]
    public string Namespace { get; set; } = null!;

    /// <summary>
    /// ServiceBus's connection string
    /// </summary>
    [Parameter(Mandatory = true, ParameterSetName = PsConnection.ConnectionString, Position = PsPosition.First)]
    public string ConnectionString { get; set; } = null!;

    protected override Task ProcessRecordAsync(CancellationToken cancellationToken)
    {
        var client = CreateServiceBusClient();
        var connection = PSServiceBusConnection.Create(client);

        WriteObject(connection);
        return Task.CompletedTask;
    }

    private ServiceBusClient CreateServiceBusClient() => ParameterSetName switch
    {
        PsConnection.Powershell => new ServiceBusClient(Namespace, new AzurePowerShellCredential(), s_serviceBusClientOptions),
        PsConnection.ConnectionString => new ServiceBusClient(ConnectionString, s_serviceBusClientOptions),
        _ => throw new NotImplementedException()
    };
}
