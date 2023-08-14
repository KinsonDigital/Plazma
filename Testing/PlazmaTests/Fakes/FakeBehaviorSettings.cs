// <copyright file="FakeBehaviorSettings.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace PlazmaTests.Fakes;

using Plazma;
using Plazma.Behaviors;

/// <summary>
/// Used for testing the <see cref="IBehaviorSettings"/> class.
/// </summary>
public class FakeBehaviorSettings : IBehaviorSettings
{
    /// <summary>
    /// Gets or sets a value used for testing.
    /// </summary>
    public ParticleAttribute ApplyToAttribute { get; set; }
}
