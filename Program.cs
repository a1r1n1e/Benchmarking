using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Benchmarking;

namespace Bench
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<Benchmarkable>();
        }
    }
}