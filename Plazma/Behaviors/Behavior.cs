// <copyright file="Behavior.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Plazma.Behaviors;

using System;

/// <summary>
/// Represents a behavior that can be applied to a particle.
/// </summary>
public abstract class Behavior : IBehavior
{
    private readonly EasingRandomBehaviorSettings setting;

    /// <summary>
    /// Initializes a new instance of the <see cref="Behavior"/> class.
    /// </summary>
    /// <param name="settings">The settings used to dictate how the behavior makes a particle behave.</param>
    protected Behavior(EasingRandomBehaviorSettings settings) => this.setting = settings;

    /// <summary>
    /// Gets or sets the current value of the behavior.
    /// </summary>
    public float Value { get; protected set; }

    /// <summary>
    /// Gets the current amount of time that has elapsed for the behavior in milliseconds.
    /// </summary>
    public float ElapsedTime { get; private set; }

    /// <summary>
    /// Gets the particle attribute to apply the behavior value to.
    /// </summary>
    public BehaviorAttribute BehaviorType => this.setting.ApplyToAttribute;

    /// <summary>
    /// Gets a value indicating whether the behavior is enabled.
    /// </summary>
    public bool Enabled { get; private set; } = true;

    /// <inheritdoc/>
    public float LifeTime { get; protected set; }

    /// <summary>
    /// Updates the behavior.
    /// </summary>
    /// <param name="timeElapsed">The amount of time that has elapsed since the last frame.</param>
    public virtual void Update(TimeSpan timeElapsed)
    {
        ElapsedTime += (float)timeElapsed.TotalMilliseconds;
        Enabled = ElapsedTime < LifeTime;
    }

    /// <summary>
    /// Resets the behavior.
    /// </summary>
    public virtual void Reset()
    {
        Value = 0.0f;
        ElapsedTime = 0;
        Enabled = true;
    }
}
