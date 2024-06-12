// <copyright file="Benchmarks.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ArrangeMethodOrOperatorBody
namespace ParticlePoolPerf;

using BenchmarkDotNet.Attributes;
using PerfShared;
using Plazma;

/// <summary>
/// Performance benchmarks for the <see cref="ParticlePool{TTexture}"/> class.
/// </summary>
[MemoryDiagnoser]
public class Benchmarks
{
    private ParticlePool<IFakeTexture>? pool;

    /// <summary>
    /// Gets or sets the total number of particles.
    /// </summary>
    [Params(100, 1_000, 10_000, 100_000, 1_000_000)]
    public int TotalParticles { get; set; }

    /// <summary>
    /// Sets up the benchmark for the entire run.
    /// </summary>
    [GlobalSetup]
    public void GlobalSetup()
    {
        var effect = new ParticleEffect
        {
            LimitSpawnRate = false,
            TotalParticles = TotalParticles,
        };
        var fakeTextureLoader = new FakeTextureLoader();

        this.pool = new ParticlePool<IFakeTexture>(effect, fakeTextureLoader);
    }

    /// <summary>
    /// Runs the benchmark for the <see cref="ParticlePool{TTexture}.Update(TimeSpan)"/> method.
    /// </summary>
    // [Benchmark]
    public void Update()
    {
        this.pool?.Update(new TimeSpan(0, 0, 0, 0, 16));
    }

    /// <summary>
    /// Runs the benchmark for the <see cref="ParticlePool{TTexture}.KillAllParticles"/> method.
    /// </summary>
    [Benchmark]
    public void KillAllParticles()
    {
        this.pool?.KillAllParticles();
    }
}
