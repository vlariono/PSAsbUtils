using System.Runtime.CompilerServices;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using PsAsbUtils.Cmdlets.Cmdlets.Base;
using PsAsbUtils.Cmdlets.Core;
using PsAsbUtils.Cmdlets.Interfaces;

namespace PsAsbUtils.Cmdlets;

public class ServiceBusConnectionCmdlet:PsAsyncCmdlet
{
    protected static readonly ConditionalWeakTable<IServiceBusConnection, ServiceBusClient> s_connections = new();

    protected static IServiceBusConnection CreateConnection(ServiceBusClient client, ServiceBusAdministrationClient adminClient)
    {
        var completion = new PsCompletion(adminClient);
        var connection = new PsServiceBusConnection(client, completion);
        s_connections.TryAdd(connection, client);
        return connection;
    }

    protected static IEnumerable<IServiceBusConnection> GetConnection()
    {
        foreach (var connection in s_connections)
        {
            yield return connection.Key;
        }
    }

    protected static void CloseConnection(IServiceBusConnection connection)
    {
        connection.Dispose();
        s_connections.Remove(connection);
    }
}
