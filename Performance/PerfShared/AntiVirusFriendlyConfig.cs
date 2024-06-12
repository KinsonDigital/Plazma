// <copyright file="AntiVirusFriendlyConfig.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace PerfShared;

using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Toolchains.InProcess.NoEmit;

/// <summary>
/// Represents the configuration for the benchmarks that are antivirus friendly.
/// </summary>
public class AntiVirusFriendlyConfig : ManualConfig
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AntiVirusFriendlyConfig"/> class.
    /// </summary>
    public AntiVirusFriendlyConfig() => AddJob(Job.Default.WithToolchain(InProcessNoEmitToolchain.Instance));
}
