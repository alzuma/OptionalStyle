# OptionalStyle

```
Install-Package OptionalStyle
```

Use case #1.
If connection is empty, create new
```csharp
private Optional<IOrientDatabaseConnection> _connection = Optional<IOrientDatabaseConnection>.Empty();

public IOrientDatabaseConnection GetConnection()
{
    return _connection.OrElseGet(CreateConnection);
}
```
Use case #2
Throw excelption if value is null. Thus avoid the `if` statement

```csharp
var car = Optional<Car>.OfNullable(null);
car.Map(c =>
{
    c.Name = "new Car";
    return c;
}).OrElseThrow(() => new ArgumentException());
```
