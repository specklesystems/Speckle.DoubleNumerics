using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.InProcess.Emit;
using Speckle.DoubleNumerics.Benchmarks;

// InProcessEmit is required: the benchmark host is compiled against either the scalar
// (netstandard2.0) or SIMD (net10.0) build of the library via the ScalarBaseline MSBuild
// property, and an out-of-process toolchain would rebuild with default properties.
var config = DefaultConfig.Instance.AddJob(
  Job.Default.WithToolchain(InProcessEmitToolchain.Instance).WithId(args.Length > 0 ? args[0] : "default")
);

BenchmarkRunner.Run<SimdBenchmarks>(config);
