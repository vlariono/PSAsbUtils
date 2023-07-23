﻿using System.Management.Automation;
using System.Runtime.CompilerServices;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;

namespace PsAsbUtils.Cmdlets.Interfaces;

public interface IServiceBusConnection : IDisposable
{
    public string Namespace { get; }
    public string Identifier { get; }

    public bool IsClosed { get; }

    public ServiceBusReceiver GetReceiver(string queueName);

    public bool TryGetReceiver(object message, out ServiceBusReceiver? receiver);

    public ServiceBusSender GetSender(string queueName);

    /// <summary>
    /// Returns powershell argument completion
    /// </summary>
    /// <param name="pattern">Pattern to match queue name</param>
    /// <param name="maxCount">Max number of completions</param>
    /// <returns></returns>
    public IEnumerable<CompletionResult> GetQueueCompletion(string pattern, int maxCount);
}
