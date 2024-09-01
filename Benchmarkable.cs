using System.Reflection;
using System.Collections.Concurrent;
using BenchmarkDotNet.Attributes;
using Newtonsoft.Json;

namespace Benchmarking;

public class Benchmarkable
{
    public static POCO Obj;

    static Benchmarkable()
    {
        Obj = POCO.GetRandomInstance();
        _lockObject = new object();
        _knownTypes = new Dictionary<Type, PropertyInfo[]>();
        _knownTypesConcurrent = new ConcurrentDictionary<Type, PropertyInfo[]>();
    }

    [Benchmark]
    public Dictionary<string, object> WithReflection() => WithReflection(Obj);
    public Dictionary<string, object> WithReflection(POCO obj)
    {
        var result = new Dictionary<string, object>();

        var properties = obj.GetType().GetProperties();
        foreach(var prop in properties)
        {
            result.Add(prop.Name, prop.GetValue(obj, null));
        }

        return result;
    }

    protected static Dictionary<Type, PropertyInfo[]> _knownTypes;

    protected static object _lockObject;

    [Benchmark]
    public Dictionary<string, object> WithReflectionAndStaticLockedDictionary() => WithReflectionAndStaticLockedDictionary(Obj);
    public Dictionary<string, object> WithReflectionAndStaticLockedDictionary(POCO obj)
    {
        var result = new Dictionary<string, object>();

        var type = obj.GetType();

        PropertyInfo[] properties = Array.Empty<PropertyInfo>();
        lock(_lockObject)
        {
            if(!_knownTypes.TryGetValue(type, out properties))
            {
                properties = type.GetProperties();
                _knownTypes.Add(type, properties);

            }
        }

        foreach(var prop in properties)
        {
            result.Add(prop.Name, prop.GetValue(obj, null));
        }

        return result;
    }

    protected static ConcurrentDictionary<Type, PropertyInfo[]> _knownTypesConcurrent;

    [Benchmark]
    public Dictionary<string, object> WithReflectionAndStaticConcurrentDictionary() => WithReflectionAndStaticConcurrentDictionary(Obj);
    public Dictionary<string, object> WithReflectionAndStaticConcurrentDictionary(POCO obj)
    {
        var result = new Dictionary<string, object>();

        var type = obj.GetType();

        var properties = _knownTypesConcurrent.GetOrAdd(type, x => type.GetProperties());

        foreach(var prop in properties)
        {
            result.Add(prop.Name, prop.GetValue(obj, null));
        }

        return result;
    }

    [Benchmark]
    public Dictionary<string, object> WithReflectionNoLock() => WithReflectionNoLock(Obj);
    public Dictionary<string, object> WithReflectionNoLock(POCO obj)
    {
        var result = new Dictionary<string, object>();

        var type = obj.GetType();

            if(!_knownTypes.TryGetValue(type, out PropertyInfo[] properties))
            {
                properties = type.GetProperties();
                _knownTypes.Add(type, properties);

            }

        foreach(var prop in properties)
        {
            result.Add(prop.Name, prop.GetValue(obj, null));
        }

        return result;
    }

    [Benchmark]
    public Dictionary<string, object> WithNewtonsoft() => WithNewtonsoft(Obj);
    public Dictionary<string, object> WithNewtonsoft(POCO obj)
    {
        var result = JsonConvert.DeserializeObject<Dictionary<string, object>>
        (JsonConvert.SerializeObject(obj, Formatting.Indented));

        return result;
    }

    [Benchmark]
    public Dictionary<string, object> WithSystemJson() => WithSystemJson(Obj);
    public Dictionary<string, object> WithSystemJson(POCO obj)
    {
        var result = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>
        (System.Text.Json.JsonSerializer.Serialize(obj));

        return result;
    }
}
