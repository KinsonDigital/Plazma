// <copyright file="FakeBehavior.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace PlazmaTests.Fakes;

using System.Diagnostics.CodeAnalysis;
using Plazma.Behaviors;

/// <summary>
/// Used for testing the <see cref="Behavior"/> class.
/// </summary>
[ExcludeFromCodeCoverage]
public class FakeBehavior : Behavior
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FakeBehavior"/> class.
    /// Creates a new instance of <see cref="FakeBehavior"/>.
    /// </summary>
    /// <param name="setting">The settings for the behavior.</param>
    public FakeBehavior(EasingRandomBehaviorSettings setting)
        : base(setting)
    {
    }
}