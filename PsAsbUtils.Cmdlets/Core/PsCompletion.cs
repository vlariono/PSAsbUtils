using System.Management.Automation;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Azure.Messaging.ServiceBus.Administration;
using PsAsbUtils.Cmdlets.Interfaces;

namespace PsAsbUtils.Cmdlets.Core;

internal sealed class PsCompletion : ICompletion
{
    private readonly ConditionalWeakTable<IServiceBusConnection, IReadOnlyList<CompletionResult>> _queueCompletion = new();
    private readonly ServiceBusAdministrationClient _client;

    private Task? _completionTask;

    internal PsCompletion(ServiceBusAdministrationClient client)
    {
        _client = client;
    }

    public IEnumerable<CompletionResult> QueueCompletion(IServiceBusConnection connection, string pattern, int maxCount)
    {
        _completionTask = RefreshQueueCompletionAsync(connection);

        if (_queueCompletion.TryGetValue(connection, out var result))
        {
            return GetResult(result);
        }

        _completionTask.Wait();
        if (_queueCompletion.TryGetValue(connection, out result))
        {
            return GetResult(result);
        }

        return Array.Empty<CompletionResult>();

        IEnumerable<CompletionResult> GetResult(IReadOnlyList<CompletionResult> completionResults)
        {
            return completionResults.Where(p => Regex.IsMatch(p.CompletionText, pattern, RegexOptions.IgnoreCase)).Take(maxCount);
        }
    }

    private async Task RefreshQueueCompletionAsync(IServiceBusConnection connection)
    {
        if(_completionTask?.IsCompleted == false)
        {
            await _completionTask;
            return;
        }

        try
        {
            var completionList = new List<CompletionResult>();
            await foreach (var queue in _client.GetQueuesAsync())
            {
                var completion = new CompletionResult(queue.Name, queue.Name, CompletionResultType.ParameterValue, queue.Name);
                completionList.Add(completion);
            }

            _queueCompletion.AddOrUpdate(connection, completionList);
        }
        catch
        {
            if (!_queueCompletion.TryGetValue(connection, out var _))
            {
                _queueCompletion.TryAdd(connection, new List<CompletionResult>());
            }
        }
    }
}
