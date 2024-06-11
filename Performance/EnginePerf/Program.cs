// <copyright file="Program.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace EnginePerf;

// ReSharper disable RedundantUsingDirective
using BenchmarkDotNet.Running;
using System.Diagnostics;
using Benchmarks;

// ReSharper enable RedundantUsingDirective

/// <summary>
/// Main class for the application.
/// </summary>
internal static class Program
{
    /// <summary>
    /// Main entry point for the application.
    /// </summary>
    public static void Main()
    {
#if DEBUG
        var benchmark = new LimitSpawnRateBenchmarks();
        benchmark.GlobalSetup();
        benchmark.IterationSetup();
        benchmark.LimitSpawnRate();
#else
        var summary = BenchmarkRunner.Run<LimitSpawnRateBenchmarks>();

        Console.WriteLine(summary);
        Console.ReadLine();
#endif
    }
}
