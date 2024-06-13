```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3672/23H2/2023Update/SunValley3)
12th Gen Intel Core i9-12900HK, 1 CPU, 20 logical and 14 physical cores
.NET SDK 8.0.300
  [Host]     : .NET 8.0.5 (8.0.524.21615), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.5 (8.0.524.21615), X64 RyuJIT AVX2


```
| Method | TotalParticles | Mean           | Error        | StdDev       | Allocated |
|------- |--------------- |---------------:|-------------:|-------------:|----------:|
| **KillAllParticles** | **100**            |      **1.161 μs** |   **0.0250 μs** |   **0.0734 μs** |         **-** |
| **KillAllParticles** | **1000**           |     **12.739 μs** |   **0.3959 μs** |   **1.1102 μs** |         **-** |
| **KillAllParticles** | **10000**          |    **109.475 μs** |   **2.4201 μs** |   **6.6249 μs** |         **-** |
| **KillAllParticles** | **100000**         |  **1,235.759 μs** |  **18.3696 μs** |  **15.3394 μs** |       **1 B** |
| **KillAllParticles** | **1000000**        | **12,636.229 μs** | **211.6956 μs** | **187.6626 μs** |       **6 B** |

