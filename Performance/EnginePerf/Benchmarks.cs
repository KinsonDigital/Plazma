// <copyright file="Benchmarks.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace EnginePerf;

using BenchmarkDotNet.Attributes;
using Plazma;

/// <summary>
/// Runs performance benchmarks for various parts of the <see cref="ParticleEngine{TTexture}"/>.
/// </summary>
[MemoryDiagnoser]
public class Benchmarks
{
    private readonly ParticleEngine<IFakeTexture> engine;

    /// <summary>
    /// Initializes a new instance of the <see cref="Benchmarks"/> class.
    /// </summary>
    public Benchmarks()
    {
        var effect = new ParticleEffect();

        var fakeTextureLoader = new FakeTextureLoader();
        var pool = new ParticlePool<IFakeTexture>(effect, fakeTextureLoader);

        this.engine = new ParticleEngine<IFakeTexture>();
        this.engine.AddPool(pool);
        this.engine.LoadTextures();
    }

    [Benchmark]
    public void PerfTest()
    {
        this.engine.Update(new TimeSpan(0, 0, 0, 0, 16));
    }
}
