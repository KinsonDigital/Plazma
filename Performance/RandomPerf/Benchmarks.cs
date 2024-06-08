// <copyright file="Benchmarks.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

// ReSharper disable ArrangeMethodOrOperatorBody
// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace RandomPerf;

using BenchmarkDotNet.Attributes;
using Plazma.Services;

/// <summary>
/// Performance benchmarks for the <see cref="RandomizerService"/> class.
/// </summary>
[MemoryDiagnoser]
public class Benchmarks
{
    private readonly RandomizerService randomService;

    /// <summary>
    /// Initializes a new instance of the <see cref="Benchmarks"/> class.
    /// </summary>
    public Benchmarks() => this.randomService = new RandomizerService();

    /// <summary>
    /// Gets or sets the maximum range value.
    /// </summary>
    [Params(10, 100, 1_000, 10_000, 100_000)]
    public int RangeMax { get; set; }

    /// <summary>
    /// Tests the <see cref="RandomizerService.GetValue(int,int)"/> overload of the <see cref="RandomizerService"/> class.
    /// </summary>
    [Benchmark]
    public void GetValue_UsingIntParamOverload()
    {
        this.randomService.GetValue(0, RangeMax);
    }

    /// <summary>
    /// Tests the <see cref="RandomizerService.GetValue(float,float)"/> overload of the <see cref="RandomizerService"/> class.
    /// </summary>
    [Benchmark]
    public void GetValue_UsingFloatParamOverload()
    {
        this.randomService.GetValue(0f, RangeMax);
    }

    /// <summary>
    /// Tests the <see cref="RandomizerService.GetValue(double,double)"/> overload of the <see cref="RandomizerService"/> class.
    /// </summary>
    [Benchmark]
    public void GetValue_UsingDoubleParamOverload()
    {
        this.randomService.GetValue(0.0, RangeMax);
    }
}
