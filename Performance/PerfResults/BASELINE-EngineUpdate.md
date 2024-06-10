```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3672/23H2/2023Update/SunValley3)
12th Gen Intel Core i9-12900HK, 1 CPU, 20 logical and 14 physical cores
.NET SDK 8.0.300
  [Host]     : .NET 8.0.5 (8.0.524.21615), X64 RyuJIT AVX2 [AttachedDebugger]
  DefaultJob : .NET 8.0.5 (8.0.524.21615), X64 RyuJIT AVX2


```
| Method   | Mean     | Error    | StdDev   | Gen0   | Allocated |
|--------- |---------:|---------:|---------:|-------:|----------:|
| PerfTest | 24.44 ns | 0.510 ns | 0.645 ns | 0.0070 |      88 B |
