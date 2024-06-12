// <copyright file="Program.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using BenchmarkDotNet.Running;
using ParticlePoolPerf;

var summary = BenchmarkRunner.Run<Benchmarks>();

Console.WriteLine(summary);
Console.ReadLine();
