// <copyright file="UpdateBenchmarks.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ArrangeMethodOrOperatorBody
namespace ParticlePoolPerf;

using BenchmarkDotNet.Attributes;
using PerfShared;
using Plazma;

/// <summary>
/// Performance benchmarks for the <see cref="ParticlePool{TTexture}"/>.<see cref="ParticlePool{TTexture}.Update"/> method.
/// </summary>
[MemoryDiagnoser]
public class UpdateBenchmarks
{
    private ParticlePool<IFakeTexture>? pool;

    /// <summary>
    /// Gets or sets the total number of particles.
    /// </summary>
    [Params(100, 1_000, 10_000, 100_000, 1_000_000)]
    public int TotalParticles { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether spawn rate will be limited.
    /// </summary>
    [Params(true, false)]
    public bool LimitSpawnRate { get; set; }

    /// <summary>
    /// Sets up the benchmark for the entire run.
    /// </summary>
    [GlobalSetup]
    public void GlobalSetup()
    {
        var effect = new ParticleEffect
        {
            LimitSpawnRate = LimitSpawnRate,
            TotalParticles = TotalParticles,
        };
        var fakeTextureLoader = new FakeTextureLoader();
        this.pool = new ParticlePool<IFakeTexture>(effect, fakeTextureLoader);
    }

    /// <summary>
    /// Runs the benchmark for the <see cref="ParticlePool{TTexture}.Update(TimeSpan)"/> method.
    /// </summary>
    [Benchmark(Description = "Limited Spawn Rate")]
    public void Update_LimitSpawnRate()
    {
        this.pool?.Update(new TimeSpan(0, 0, 0, 0, 16));
    }

    /// <summary>
    /// Runs the benchmark for the <see cref="ParticlePool{TTexture}.Update(TimeSpan)"/> method.
    /// </summary>
    [Benchmark(Description = "Unlimited Spawn Rate")]
    public void Update_UnlimitedSpawnRate()
    {
        this.pool?.Update(new TimeSpan(0, 0, 0, 0, 16));
    }
}
