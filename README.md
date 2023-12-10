# PSAsbUtils
Azure Service Bus messaging module. The module is able to receive, send and peek Azure Service Bus messages. Dead letters are supported. It is useful with frameworks like NServiceBus when you need to resubmit a message from an error queue.

## Connect to ServiceBus namespace

1. Connect with powershell credentials
```powershell
Connect-AzAccount
$connection = Connect-AsbNamespace -Namespace <namespace>.servicebus.windows.net
```

2. Connect with connection string
```powershell
$connection = Connect-AsbNamespace -ConnectionString '<connection string>'
```

3. List active connections
```powershell
Get-AsbNamespaceConnection
```

4. Set default service bus connection. Allows to skip Connection parameter
```powershell
Connect-AsbNamespace -Namespace <namespace>.servicebus.windows.net|Set-AsbDefaultConnection
```
or

```Powershell
Set-AsbDefaultConnection -Connection $connection
```

## Receive messages from a queue

1. Peek messages from a queue
```powershell
Get-AsbMessage -Connection $connection -QueueName <queue>
```

2. Receive message from q queue
```powershell
$messages = Receive-AsbMessage -Connection $connection -QueueName <queue>
```

3. Messages are received in PeekLock mode and need to be completed
```powershell
$messages|Complete-AsbMessage -Connection $connection
```

## Create new message
1. Create new message
```powershell
New-AsbMessage -Body '123456'
```
2. Create message from the received message
```powershell
New-AsbMessage -ReceivedMessage $message -MessageId (New-Guid) -CustomProperties @{Test = '123456'}
```

```powershell
$message|New-AsbMessage -MessageId (New-Guid) -CustomProperties @{Test = '123456'}
```

## Send message to a queue

1. Send message to a queue
```powershell
Send-AsbMessage -Connection $connection -Message $message
```

```powershell
New-AsbMessage -Body '1234567'|Send-AsbMessage -Connection $connection -QueueName <queue>
```

## Disconnect from service bus namespace

1. Close single connection
```powershell
Disconnect-AsbNamespace -Connection $connection
```