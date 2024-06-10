// <copyright file="Program.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace EnginePerf;

// ReSharper disable once RedundantUsingDirective
using BenchmarkDotNet.Running;
using Plazma;

/// <summary>
/// Main class for the application.
/// </summary>
internal static class Program
{
    /// <summary>
    /// Main entry point for the application.
    /// </summary>
    /// <param name="args">Application arguments.</param>
    public static void Main(string[] args)
    {
#if DEBUG
        var effect = new ParticleEffect();
        var fakeTextureLoader = new FakeTextureLoader();
        var pool = new ParticlePool<IFakeTexture>(effect, fakeTextureLoader);

        var engine = new ParticleEngine<IFakeTexture>();
        engine.AddPool(pool);
        engine.LoadTextures();

        engine.Update(new TimeSpan(0, 0, 0, 0, 0));
#else
        var summary = BenchmarkRunner.Run<Benchmarks>();

        Console.WriteLine(summary);
        Console.ReadLine();
#endif
    }
}
