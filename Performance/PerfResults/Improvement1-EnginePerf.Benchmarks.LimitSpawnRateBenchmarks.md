```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3672/23H2/2023Update/SunValley3)
12th Gen Intel Core i9-12900HK, 1 CPU, 20 logical and 14 physical cores
.NET SDK 8.0.300
  [Host] : .NET 8.0.5 (8.0.524.21615), X64 RyuJIT AVX2

Toolchain=InProcessNoEmitToolchain  InvocationCount=1  UnrollFactor=1  

```
| Method             | RunStats        | Mean       | Error    | StdDev  | Gen0      | Gen1      | Allocated   |
|------------------- |---------------- |-----------:|---------:|--------:|----------:|----------:|------------:|
| **&#39;Limit Spawn Rate&#39;** | **(1000, 150000)**  |   **127.7 ms** |  **2.40 ms** | **2.36 ms** | **1000.0000** |         **-** |  **12891.3 KB** |
| **&#39;Limit Spawn Rate&#39;** | **(10000, 150000)** | **1,060.5 ms** | **10.87 ms** | **9.63 ms** | **1000.0000** | **1000.0000** | **12896.91 KB** |
| **&#39;Limit Spawn Rate&#39;** | **(100000, 1500)**  |   **152.9 ms** |  **2.97 ms** | **3.97 ms** |         **-** |         **-** |   **129.58 KB** |
