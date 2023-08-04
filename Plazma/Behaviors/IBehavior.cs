// <copyright file="IBehavior.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Plazma.Behaviors;

using System;

/// <summary>
/// Represents a behavior that can be applied to a particle.
/// </summary>
public interface IBehavior
{
    /// <summary>
    /// Gets the current value of the behavior.
    /// </summary>
    string Value { get; }

    /// <summary>
    /// Gets the current amount of time that has elapsed for the behavior.
    /// </summary>
    double ElapsedTime { get; }

    /// <summary>
    /// Gets the particle attribute to apply the behavior result to.
    /// </summary>
    ParticleAttribute ApplyToAttribute { get; }

    /// <summary>
    /// Gets a value indicating whether the behavior is enabled.
    /// </summary>
    bool Enabled { get; }

    /// <summary>
    /// Gets the life time of the behavior in milliseconds.
    /// </summary>
    /// <remarks>
    ///     Once the amount of time has elapsed the life time of the
    ///     behavior, the behavior will be disabled.
    /// </remarks>
    double LifeTime { get; }

    /// <summary>
    /// Updates the behavior.
    /// </summary>
    /// <param name="timeElapsed">The amount of time that has elapsed since the last frame.</param>
    void Update(TimeSpan timeElapsed);

    /// <summary>
    /// Resets the behavior.
    /// </summary>
    void Reset();
}