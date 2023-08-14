// <copyright file="IBehaviorSettings.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Plazma.Behaviors;

/// <summary>
/// The settings for a particle behavior.
/// </summary>
public interface IBehaviorSettings
{
    /// <summary>
    /// Gets the particle attribute to set the behavior value to.
    /// </summary>
    ParticleAttribute ApplyToAttribute { get; }

    /// <summary>
    /// Gets the amount of time the behavior will run.
    /// </summary>
    public double LifeTime { get; }
}
