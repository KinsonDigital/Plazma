// <copyright file="RunStats.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace EnginePerf;

/// <summary>
/// Provides different stats to use for performance tests.
/// </summary>
public readonly record struct RunStats
{
    /// <summary>
    /// Gets the total number of particles to use in a test.
    /// </summary>
    public int TotalParticles { get; init; }

    /// <summary>
    /// Gets the total number of iterations to use in a test.
    /// </summary>
    public int TotalIterations { get; init; }
}
