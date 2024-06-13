```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3672/23H2/2023Update/SunValley3)
12th Gen Intel Core i9-12900HK, 1 CPU, 20 logical and 14 physical cores
.NET SDK 8.0.300
  [Host]     : .NET 8.0.5 (8.0.524.21615), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.5 (8.0.524.21615), X64 RyuJIT AVX2


```
| Method | TotalParticles | Mean          | Error       | StdDev      | Gen0   | Allocated |
|------- |--------------- |--------------:|------------:|------------:|-------:|----------:|
| **Unlimited SpawnRate** | **100**            |      **1.596 μs** |   **0.0180 μs** |   **0.0168 μs** |  **96 B** |
| **Unlimited SpawnRate** | **1000**           |     **15.039 μs** |   **0.1192 μs** |   **0.1115 μs** |  **96 B** |
| **Unlimited SpawnRate** | **10000**          |    **141.080 μs** |   **1.7654 μs** |   **1.6514 μs** |  **96 B** |
| **Unlimited SpawnRate** | **100000**         |  **1,470.546 μs** |  **20.0929 μs** |  **18.7949 μs** |  **97 B** |
| **Unlimited SpawnRate** | **1000000**        | **14,619.421 μs** | **114.6509 μs** | **107.2446 μs** | **102 B** |
