// <copyright file="Program.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

#pragma warning disable SA1200
using BenchmarkDotNet.Running;
#pragma warning restore SA1200

var summary = BenchmarkRunner.Run<ParticlePoolPerf.Benchmarks>();

Console.WriteLine(summary);
Console.ReadLine();
