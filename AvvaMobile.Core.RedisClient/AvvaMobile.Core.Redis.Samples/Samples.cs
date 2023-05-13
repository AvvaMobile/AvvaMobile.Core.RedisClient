using Microsoft.AspNetCore.Mvc.Rendering;

namespace AvvaMobile.Core.Redis.Samples;

public class Samples
{
    private readonly IRedisClient _redis;

    public Samples(IRedisClient redis)
    {
        _redis = redis;
    }

    public async Task IsExists()
    {
        var key = "foo";

        var result = await _redis.IsExists(key);

        if (result)
        {
            Console.Write("Key is exists");
        }
        else
        {
            Console.Write("Key is NOT exists");
        }
    }

    public async Task Set()
    {
        var key = "foo";
        var value = "bar";

        var result = await _redis.Set(key, value);
    }
    
    public async Task SetWithTTL()
    {
        var key = "foo";
        var value = "bar";

        var result = await _redis.Set(key, value, TimeSpan.FromHours(1));
    }

    public async Task Get()
    {
        var key = "foo";

        var stringValue = await _redis.Get_String(key);
    }

    public async Task Set_Serialized()
    {
        var key = "foo";
        var value = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };

        var result = await _redis.Set_Serialized(key, value);
    }

    public async Task Get_Deserialized()
    {
        var key = "foo";

        var valueList = await _redis.Get_Deserialized<List<int>>(key);
    }
    
    public async Task Set_SelectListItems()
    {
        var key = "foo";
        var value = new List<SelectListItem>
        {
            new SelectListItem{Text = "Item 1", Value = "1"},
            new SelectListItem{Text = "Item 2", Value = "2"},
            new SelectListItem{Text = "Item 3", Value = "3"},
            new SelectListItem{Text = "Item 4", Value = "4"}
        };

        var result = await _redis.Set_SelectListItems(key, value);
    }

    public async Task Get_SelectListItems()
    {
        var key = "foo";

        var valueList = await _redis.Get_SelectListItems(key);
    }
}