// <copyright file="AnitiVirusFriendlyConfig.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace EnginePerf;

using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Toolchains.InProcess.NoEmit;

public class AnitiVirusFriendlyConfig : ManualConfig
{
    public AnitiVirusFriendlyConfig()
    {
        AddJob(Job.Default.WithToolchain(InProcessNoEmitToolchain.Instance));
    }
}
