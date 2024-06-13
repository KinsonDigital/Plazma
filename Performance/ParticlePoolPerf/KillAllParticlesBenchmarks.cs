// <copyright file="KillAllParticlesBenchmarks.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

// ReSharper disable ArrangeMethodOrOperatorBody
// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace ParticlePoolPerf;

using BenchmarkDotNet.Attributes;
using PerfShared;
using Plazma;

/// <summary>
/// Performance benchmarks for the <see cref="ParticlePool{TTexture}"/>.<see cref="ParticlePool{TTexture}.KillAllParticles"/> method.
/// </summary>
public class KillAllParticlesBenchmarks
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
            LimitSpawnRate = true,
            TotalParticles = TotalParticles,
        };
        var fakeTextureLoader = new FakeTextureLoader();
        this.pool = new ParticlePool<IFakeTexture>(effect, fakeTextureLoader);
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
