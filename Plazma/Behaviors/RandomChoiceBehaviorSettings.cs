// <copyright file="RandomChoiceBehaviorSettings.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Plazma.Behaviors;

using System.Collections.ObjectModel;

/// <summary>
/// Various settings for behaviors that choose values randomly from a list of choices.
/// </summary>
public class RandomChoiceBehaviorSettings : IBehaviorSettings
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RandomChoiceBehaviorSettings"/> class.
    /// </summary>
    public RandomChoiceBehaviorSettings()
    {
    }

    /// <inheritdoc/>
    public ParticleAttribute ApplyToAttribute { get; set; }

    /// <summary>
    /// Gets or sets the data for the use by an <see cref="IBehavior"/> implementation.
    /// </summary>
    public ReadOnlyCollection<string>? Data { get; set; }

    /// <summary>
    /// Gets or sets the amount of time that the behavior should be enabled.
    /// </summary>
    public double LifeTime { get; set; }
}
