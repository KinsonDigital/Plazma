// <copyright file="Program.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

#pragma warning disable SA1200
using BenchmarkDotNet.Running;
#pragma warning restore SA1200

var perfType = args.Length > 0 ? args[0] : string.Empty;

if (!string.IsNullOrEmpty(perfType))
{
    Console.WriteLine($"--------Running {perfType} Performance Test--------");
}

switch (perfType)
{
    case "UPDATE":
        var updateSummary = BenchmarkRunner.Run<ParticlePoolPerf.UpdateBenchmarks>();
        Console.WriteLine(updateSummary);
        break;
    case "KILL_ALL_PARTICLES":
        var killAllSummary = BenchmarkRunner.Run<ParticlePoolPerf.KillAllParticlesBenchmarks>();
        Console.WriteLine(killAllSummary);
        break;
    default:
        throw new Exception("No arguments were provided.");
}

Console.ReadLine();
