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
    private ParticleEngine<IFakeTexture>? engine;

    /// <summary>
    /// Gets the total number of particles and iterations to run for each benchmark.
    /// </summary>
    public static RunStats[] TotalParticlesAndIterations =>
    [
        new RunStats { TotalParticles = 1_000, TotalIterations = 150_000 },
        new RunStats { TotalParticles = 10_000, TotalIterations = 150_000 },
        new RunStats { TotalParticles = 100_000, TotalIterations = 1_500 },
    ];

    /// <summary>
    /// Gets or sets the run states to use for the benchmark.
    /// </summary>
    [ParamsSource(nameof(TotalParticlesAndIterations))]
    public RunStats RunStats { get; set; }

    /// <summary>
    /// Sets up the benchmark for the entire run.
    /// </summary>
    [GlobalSetup]
    public void GlobalSetup() => this.engine = new ParticleEngine<IFakeTexture>();

    /// <summary>
    /// Sets up the benchmark for each iteration.
    /// </summary>
    [IterationSetup]
    public void IterationSetup()
    {
        ArgumentNullException.ThrowIfNull(this.engine);

        var effect = new ParticleEffect
        {
            LimitSpawnRate = false,
            TotalParticles = RunStats.TotalParticles,
        };

        var fakeTextureLoader = new FakeTextureLoader();
        var pool = new ParticlePool<IFakeTexture>(effect, fakeTextureLoader);
        this.engine.AddPool(pool);
        this.engine.LoadTextures();
    }

    /// <summary>
    /// Cleans up the benchmark for each iteration.
    /// </summary>
    [IterationCleanup]
    public void IterationCleanup()
    {
        ArgumentNullException.ThrowIfNull(this.engine);

        this.engine.ClearPools();
    }

    /// <summary>
    /// Runs the engine to measure the performance of the spawn rate limitation and
    /// other core processes of the engine.
    /// </summary>
    [Benchmark(Description = "Limit Spawn Rate")]
    public void LimitSpawnRate()
    {
        for (var i = 0; i < RunStats.TotalIterations; i++)
        {
            this.engine?.Update(new TimeSpan(0, 0, 0, 0, 16));
        }
    }
}
