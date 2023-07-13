# PSAsbUtils
Azure Service Bus messaging module

## Connect to ServiceBus namespace

1. Connect with powershell credentials
```powershell
Connect-AzAccount
$connection = Connect-AsbNamespace -Namespace <namespace>.servicebus.windows.net -Verbose -AzurePowershell
```

2. Connect with connection string
```powershell
$connection = Connect-AsbNamespace -Namespace <namespace>.servicebus.windows.net -Verbose -ConnectionString '<connection string>'
```

3. List active connections
```powershell
Get-AsbNamespaceConnection
```

## Receive messages from a queue

1. Set default service bus connection. Allows to skip Connection parameter
```powershell
Connect-AsbNamespace -Namespace <namespace>.servicebus.windows.net -Verbose -AzurePowershell|Set-AsbDefaultConnection
```
or

```Powershell
Set-AsbDefaultConnection -Connection $connection
```

2. Peek messages from a queue
```powershell
Get-AsbMessage -Connection $connection -QueueName <queue>
```

3. Receive message from q queue
```powershell
$messages = Receive-AsbMessage -Connection $connection -QueueName <queue>
```

4. Messages are received in PeekLock mode and need to be completed
```powershell
$messages|Complete-AsbMessage -Connection $connection
```

## Send message to a queue

1. Send message to a queue
```powershell
Send-AsbMessage -Connection $connection -Message $message
```
2. Resend message to a queue
```powershell
Send-AsbMessage -Connection $connection -ReceivedMessage $message
```

## Disconnect from service bus namespace

1. Close single connection
```powershell
Disconnect-AsbNamespace -Connection $connection
```