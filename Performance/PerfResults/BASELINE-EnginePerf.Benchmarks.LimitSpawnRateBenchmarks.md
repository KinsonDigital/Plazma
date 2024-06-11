```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3672/23H2/2023Update/SunValley3)
12th Gen Intel Core i9-12900HK, 1 CPU, 20 logical and 14 physical cores
.NET SDK 8.0.300
  [Host] : .NET 8.0.5 (8.0.524.21615), X64 RyuJIT AVX2

Toolchain=InProcessNoEmitToolchain  InvocationCount=1  UnrollFactor=1  

```
| Method             | TotalParticles | Mean       | Error    | StdDev   | Gen0      | Gen1      | Allocated |
|------------------- |--------------- |-----------:|---------:|---------:|----------:|----------:|----------:|
| **&#39;Limit Spawn Rate&#39;** | **1000**           |   **139.9 ms** |  **2.73 ms** |  **5.27 ms** | **1000.0000** |         **-** |  **12.59 MB** |
| **&#39;Limit Spawn Rate&#39;** | **10000**          | **1,130.8 ms** | **19.74 ms** | **18.46 ms** | **1000.0000** | **1000.0000** |  **12.59 MB** |
