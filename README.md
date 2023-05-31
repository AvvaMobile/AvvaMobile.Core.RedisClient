# AvvaMobile.Core.Redis.RedisClient

## Join Our Open Source Team

If you love at least two of the followings, you are welcome to join our open source team, please contact us at opensource@avvamobile.com.

- Pizza :pizza: or Hamburger :hamburger:
- Beer :beer: or Whiskey :tumbler_glass:
- Video Games :video_game:
- Car Racing :racing_car:
- Electronic Music :control_knobs: or Rock Music :guitar:

## Developers

- [@jackmuratyilmaz](https://www.github.com/jackmuratyilmaz)
- [@Onurryilmazz](https://www.github.com/Onurryilmazz)
- [@ocalesmer](https://www.github.com/ocalesmer)
- [@cativ3](https://www.github.com/cativ3)
- [@avvamobiledogukan](https://github.com/orgs/AvvaMobile/people/avvamobiledogukan)

## NuGet Package
To use NuGet package, please go to [https://www.nuget.org/packages/AvvaMobile.Core.RedisClient](https://www.nuget.org/packages/AvvaMobile.Core.RedisClient)

## Namespace
All necessary classes are in the namespace below.

``` csharp
using AvvaMobile.Core.Redis;
``` 

## Dependency Injection

Client interface is `IRedisClient` and implementation is `RedisClient`.

Register the client in the `program.cs` file.
    
``` csharp
builder.Services.AddSingleton<IRedisClient, RedisClient>(r => new RedisClient(host: "localhost", port: 6379, user: "default", password: "redispw"));
```

Then initiliaze the instance in the constructor of the class.

``` csharp
namespace AvvaMobile.Core.Redis.Samples;

public class Samples
{
    private readonly IRedisClient _redis;
    
    public Samples(IRedisClient redis)
    {
        _redis = redis;
    }
}
```

# Samples

## Check Key Is Exists

``` csharp
var key = "foo";
    
var isExists = await _redis.IsExists(key);

if (isExists)
{
    Console.Write("Key is exists.");
}
else
{
    Console.Write("Key is NOT exists.");
}
```

## Set Value

``` csharp
var key = "foo";
var valueString = "bar";

var resultString = await _redis.Set(key, valueString);
```

## Get String

``` csharp
var key = "foo";

var stringValue = await _redis.Get_String(key);
```

## Set Serialized

``` csharp
var key = "foo";
var value = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };

var result = await _redis.Set_Serialized(key, value);
```

## Get Deserialized

``` csharp
var key = "foo";

var valueList = await _redis.Get_Deserialized<List<int>>(key);
```

## Set Select List Items (for Web)

``` csharp
var key = "foo";
var value = new List<SelectListItem>
{
    new SelectListItem{Text = "Item 1", Value = "1"},
    new SelectListItem{Text = "Item 2", Value = "2"},
    new SelectListItem{Text = "Item 3", Value = "3"},
    new SelectListItem{Text = "Item 4", Value = "4"}
};

var result = await _redis.Set_SelectListItems(key, value);

```

## Get Select List Items (for Web)

``` csharp
var key = "foo";

var valueList = await _redis.Get_SelectListItems(key);
```

## Remove a Key

``` csharp
var key = "foo";

await _redis.Remove(key);
```

## Clear Database

``` csharp
await _redis.ClearAll();
```