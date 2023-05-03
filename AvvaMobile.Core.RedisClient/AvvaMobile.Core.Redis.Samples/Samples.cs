using Microsoft.AspNetCore.Mvc.Rendering;

namespace AvvaMobile.Core.Redis.Samples;

public class Samples
{
    private readonly IRedisClient _redis;

    public Samples(IRedisClient redis)
    {
        _redis = redis;
    }

    public async Task CheckIsExists()
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

    public async Task StringSet()
    {
        var key = "foo";
        var value = "bar";

        var result = await _redis.String_Set(key, value);
    }

    public async Task StringGet()
    {
        var key = "foo";

        var stringValue = await _redis.String_Get(key);
    }

    public async Task IntSet()
    {
        var key = "Turkiye";
        var countryCode = 90;

        var result = await _redis.Int_Set(key, countryCode);
    }

    public async Task IntGet()
    {
        var key = "Turkiye";

        var countryCode = await _redis.Int_Get(key);
    }

    public async Task Serialize_Set()
    {
        var key = "foo";
        var value = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };

        var result = await _redis.Serialize_Set(key, value);
    }

    public async Task Deserialize_Get()
    {
        var key = "foo";

        var valueList = await _redis.Deserialize_Get<List<int>>(key);
    }
    
    public async Task List_SelectListItem_Set()
    {
        var key = "foo";
        var value = new List<SelectListItem>
        {
            new SelectListItem{Text = "Item 1", Value = "1"},
            new SelectListItem{Text = "Item 2", Value = "2"},
            new SelectListItem{Text = "Item 3", Value = "3"},
            new SelectListItem{Text = "Item 4", Value = "4"}
        };

        var result = await _redis.List_SelectListItem_Set(key, value);
    }

    public async Task List_SelectListItem_Get()
    {
        var key = "foo";

        var valueList = await _redis.List_SelectListItem_Get(key);
    }
}