using System;
using BenchmarkDotNet.Attributes;

namespace Benchmarking;

public class Benchmarkable
{
    public static POCO Obj;

    static Benchmarkable()
    {
        Obj = new POCO()
        {
            StringProperty1 = "sdfsdasfddfs",
            StringProperty2 = "sdfsdgffs",
            StringProperty3 = "syfgddfs",
            StringProperty4 = "sdfasdsas",
            StringProperty5 = "sdfsadad",
            IntProperty1 = 125,
            IntProperty2 = 178525,
            IntProperty3 = 12565,
            IntProperty4 = 568125,
            IntProperty5 = 568564125
        };
    }

    [Benchmark]
    public Dictionary<string, object> WithReflection() => WithReflection(Obj);
    public Dictionary<string, object> WithReflection(POCO obj)
    {
        return new Dictionary<string, object>();
    }

    [Benchmark]
    public Dictionary<string, object> WithReflectionAndStatics() => WithReflectionAndStatics(Obj);
    public Dictionary<string, object> WithReflectionAndStatics(POCO obj)
    {
        return new Dictionary<string, object>();
    }

    [Benchmark]
    public Dictionary<string, object> WithNewtonsoft() => WithNewtonsoft(Obj);
    public Dictionary<string, object> WithNewtonsoft(POCO obj)
    {
        return new Dictionary<string, object>();
    }

    [Benchmark]
    public Dictionary<string, object> WithSystemJson() => WithSystemJson(Obj);
    public Dictionary<string, object> WithSystemJson(POCO obj)
    {
        return new Dictionary<string, object>();
    }

    [Benchmark]
    public Dictionary<string, object> WithAttributesMarkings() => WithAttributesMarkings(Obj);
    public Dictionary<string, object> WithAttributesMarkings(POCO obj)
    {
        return new Dictionary<string, object>();
    }
}
