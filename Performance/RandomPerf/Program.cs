// <copyright file="Program.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RandomPerf;

using BenchmarkDotNet.Running;

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
        var summary = BenchmarkRunner.Run<Benchmarks>();

        Console.WriteLine(summary);
        Console.ReadLine();
    }
}
