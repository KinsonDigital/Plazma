// <copyright file="LimitSpawnRateBenchmarks.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace EnginePerf.Benchmarks;

using BenchmarkDotNet.Attributes;
using Plazma;

/// <summary>
/// Runs performance benchmarks for various parts of the <see cref="ParticleEngine{TTexture}"/>.
/// </summary>
[MemoryDiagnoser]
[Config(typeof(AnitiVirusFriendlyConfig))]
public class LimitSpawnRateBenchmarks
{
    private ParticleEngine<IFakeTexture> engine;

    /// <summary>
    /// Initializes a new instance of the <see cref="LimitSpawnRateBenchmarks"/> class.
    /// </summary>
    public LimitSpawnRateBenchmarks()
    {
        // var effect = new ParticleEffect { LimitSpawnRate = false, };
        //
        // var fakeTextureLoader = new FakeTextureLoader();
        // var pool = new ParticlePool<IFakeTexture>(effect, fakeTextureLoader);
        //
        // this.engine = new ParticleEngine<IFakeTexture>();
        // this.engine.AddPool(pool);
        // this.engine.LoadTextures();
    }

    [GlobalSetup]
    public void GlobalSetup()
    {
        this.engine = new ParticleEngine<IFakeTexture>();
    }

    [IterationSetup]
    public void IterationSetup()
    {
        var effect = new ParticleEffect
        {
            LimitSpawnRate = false,
            TotalParticles = TotalParticles,
        };

        var fakeTextureLoader = new FakeTextureLoader();
        var pool = new ParticlePool<IFakeTexture>(effect, fakeTextureLoader);
        this.engine.AddPool(pool);
        this.engine.LoadTextures();
    }

    [IterationCleanup]
    public void IterationCleanup()
    {
        this.engine.ClearPools();
    }

    [Params(1_000, 10_000)]
    public int TotalParticles { get; set; }

    [Benchmark(Description = "Limit Spawn Rate")]
    public void LimitSpawnRate()
    {
        for (var i = 0; i < 150_000; i++)
        {
            this.engine.Update(new TimeSpan(0, 0, 0, 0, 16));
        }
    }
}
