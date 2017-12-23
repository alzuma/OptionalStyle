# OptionalStyle

```csharp
private Optional<IOrientDatabaseConnection> _connection;

public IOrientDatabaseConnection GetConnection()
{
    return _connection.OrElseGet(CreateConnection);
}
```
