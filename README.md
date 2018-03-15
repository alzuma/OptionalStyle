# OptionalStyle

Use case #1.
If connection is empty, create new
```csharp
private Optional<IOrientDatabaseConnection> _connection = Optional<IOrientDatabaseConnection>.Empty();

public IOrientDatabaseConnection GetConnection()
{
    return _connection.OrElseGet(CreateConnection);
}
```
