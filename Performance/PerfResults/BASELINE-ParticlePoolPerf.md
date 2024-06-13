```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3672/23H2/2023Update/SunValley3)
12th Gen Intel Core i9-12900HK, 1 CPU, 20 logical and 14 physical cores
.NET SDK 8.0.300
  [Host]     : .NET 8.0.5 (8.0.524.21615), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.5 (8.0.524.21615), X64 RyuJIT AVX2


```
| Method | TotalParticles | Mean           | Error        | StdDev       | Allocated |
|------- |--------------- |---------------:|-------------:|-------------:|----------:|
| **Unlimited Spawn Rate** | **100**            | **0.13 μs**    | **0.0016 μs**  | **0.0015 μs**  |         **-** |
| **Unlimited Spawn Rate** | **1000**           | **0.57 μs**    | **0.0089 μs**  | **0.0083 μs**  |         **-** |
| **Unlimited Spawn Rate** | **10000**          | **4.88 μs**    | **0.0882 μs**  | **0.0825 μs**  |         **-** |
| **Unlimited Spawn Rate** | **100000**         | **78.96 μs**   | **1.4104 μs**  | **1.3193 μs**  |         **-** |
| **Unlimited Spawn Rate** | **1000000**        | **2461.08 μs** | **48.5792 μs** | **45.4410 μs** |       **2 B** |
