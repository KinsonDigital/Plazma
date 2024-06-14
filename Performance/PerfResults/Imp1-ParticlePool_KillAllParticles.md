```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3672/23H2/2023Update/SunValley3)
12th Gen Intel Core i9-12900HK, 1 CPU, 20 logical and 14 physical cores
.NET SDK 8.0.300
  [Host]     : .NET 8.0.5 (8.0.524.21615), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.5 (8.0.524.21615), X64 RyuJIT AVX2


```
| Method | TotalParticles | Mean          | Error       | StdDev      | Gen0   | Allocated |
|------- |--------------- |--------------:|------------:|------------:|-------:|----------:|
| **KillAllParticles** | **100**     | **0.8151 μs**    | **0.0120 μs**  | **0.0112 μs**   |   **-** |
| **KillAllParticles** | **1000**    | **8.1101 μs**    | **0.1333 μs**  | **0.1247 μs**   |   **-** |
| **KillAllParticles** | **10000**   | **80.4861 μs**   | **1.2545 μs**  | **1.1734 μs**   |   **-** |
| **KillAllParticles** | **100000**  | **810.9346 μs**  | **7.7116 μs**  | **6.8361 μs**   |   **-** |
| **KillAllParticles** | **1000000** | **8641.6809 μs** | **89.1184 μs** | **79.0011 μs**  | **6 B** |
