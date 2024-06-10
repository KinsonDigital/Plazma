// <copyright file="Particle.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Plazma;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using Behaviors;

/// <inheritdoc cref="Particle"/>
public readonly record struct Particle
{
    private readonly List<IBehavior> behaviors = new ();

    /// <summary>
    /// Initializes a new instance of the <see cref="Particle"/> class.
    /// </summary>
    /// <param name="behaviors">The list of behaviors to add to the <see cref="Particle"/>.</param>
    public Particle(IBehavior[] behaviors)
    {
        ArgumentNullException.ThrowIfNull(behaviors);

        this.behaviors = behaviors.ToList();
    }

    /// <summary>
    /// Gets or sets the position of the <see cref="Particle"/>.
    /// </summary>
    public Vector2 Position { get; init; }

    /// <summary>
    /// Gets or sets the angle of the <see cref="Particle"/>.
    /// </summary>
    public float Angle { get; init; }

    /// <summary>
    /// Gets or sets the color that the texture will be tinted to.
    /// </summary>
    public Color TintColor { get; init; } = Color.White;

    /// <summary>
    /// Gets or sets the size of the <see cref="Particle"/>.
    /// </summary>
    public float Size { get; init; } = 1;

    /// <summary>
    /// Gets or sets a value indicating whether the <see cref="Particle"/> is alive or dead.
    /// </summary>
    public bool IsAlive { get; init; }

    /// <inheritdoc/>
    /// Gets the list of particle behaviors.
    public List<IBehavior> Behaviors => this.behaviors;

    /// <summary>
    /// Updates the particle.
    /// </summary>
    /// <param name="timeElapsed">The amount of time that has elapsed since the last frame.</param>
    public void AddBehavior(IBehavior behavior)
    {
        if (this.behaviors.Exists(b => b.BehaviorType == behavior.BehaviorType))
        {
            return;
        }

        this.behaviors.Add(behavior);
    }

    /// <inheritdoc/>
    public void RemoveBehavior(BehaviorAttribute behaviorType)
    {
        var behavior = this.behaviors.Find(b => b.BehaviorType == behaviorType);

        if (behavior is null)
        {
            return;
        }

        this.behaviors.Remove(behavior);
    }
}
