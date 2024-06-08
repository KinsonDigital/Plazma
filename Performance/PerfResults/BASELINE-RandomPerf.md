```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3672/23H2/2023Update/SunValley3)
12th Gen Intel Core i9-12900HK, 1 CPU, 20 logical and 14 physical cores
.NET SDK 8.0.300
  [Host]     : .NET 8.0.5 (8.0.524.21615), X64 RyuJIT AVX2 [AttachedDebugger]
  DefaultJob : .NET 8.0.5 (8.0.524.21615), X64 RyuJIT AVX2


```
| Method                            | RangeMax | Mean     | Error    | StdDev   | Median   | Allocated |
|---------------------------------- |--------- |---------:|---------:|---------:|---------:|----------:|
| **GetValue_UsingIntParamOverload**    | **10**       | **71.14 ns** | **1.449 ns** | **2.460 ns** | **70.00 ns** |         **-** |
| GetValue_UsingFloatParamOverload  | 10       | 82.11 ns | 1.379 ns | 1.222 ns | 82.38 ns |         - |
| GetValue_UsingDoubleParamOverload | 10       | 79.85 ns | 1.313 ns | 1.228 ns | 79.72 ns |         - |
| **GetValue_UsingIntParamOverload**    | **100**      | **57.92 ns** | **0.536 ns** | **0.501 ns** | **58.02 ns** |         **-** |
| GetValue_UsingFloatParamOverload  | 100      | 62.73 ns | 0.898 ns | 0.840 ns | 62.97 ns |         - |
| GetValue_UsingDoubleParamOverload | 100      | 62.10 ns | 0.498 ns | 0.441 ns | 62.10 ns |         - |
| **GetValue_UsingIntParamOverload**    | **1000**     | **45.74 ns** | **0.437 ns** | **0.387 ns** | **45.58 ns** |         **-** |
| GetValue_UsingFloatParamOverload  | 1000     | 49.87 ns | 0.419 ns | 0.392 ns | 49.98 ns |         - |
| GetValue_UsingDoubleParamOverload | 1000     | 49.04 ns | 0.459 ns | 0.430 ns | 49.04 ns |         - |
| **GetValue_UsingIntParamOverload**    | **10000**    | **74.47 ns** | **1.292 ns** | **1.269 ns** | **74.09 ns** |         **-** |
| GetValue_UsingFloatParamOverload  | 10000    | 79.91 ns | 0.974 ns | 0.911 ns | 79.77 ns |         - |
| GetValue_UsingDoubleParamOverload | 10000    | 81.61 ns | 0.952 ns | 0.844 ns | 81.66 ns |         - |
| **GetValue_UsingIntParamOverload**    | **100000**   | **60.26 ns** | **0.902 ns** | **0.844 ns** | **60.05 ns** |         **-** |
| GetValue_UsingFloatParamOverload  | 100000   | 63.91 ns | 0.819 ns | 0.767 ns | 63.85 ns |         - |
| GetValue_UsingDoubleParamOverload | 100000   | 62.91 ns | 0.928 ns | 0.822 ns | 62.89 ns |         - |
