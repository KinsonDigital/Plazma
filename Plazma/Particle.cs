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
    /// <summary>
    /// Initializes a new instance of the <see cref="Particle"/> class.
    /// </summary>
    /// <param name="behaviors">The list of behaviors to add to the <see cref="Particle"/>.</param>
    public Particle(IBehavior[] behaviors)
    {
        ArgumentNullException.ThrowIfNull(behaviors);

        Behaviors = behaviors.ToList();
    }

    /// <summary>
    /// Gets the position of the <see cref="Particle"/>.
    /// </summary>
    public Vector2 Position { get; init; }

    /// <summary>
    /// Gets the angle of the <see cref="Particle"/>.
    /// </summary>
    public float Angle { get; init; }

    /// <summary>
    /// Gets the color that the texture will be tinted to.
    /// </summary>
    public Color TintColor { get; init; } = Color.White;

    /// <summary>
    /// Gets the size of the <see cref="Particle"/>.
    /// </summary>
    public float Size { get; init; } = 1;

    /// <summary>
    /// Gets a value indicating whether the <see cref="Particle"/> is alive or dead.
    /// </summary>
    public bool IsAlive { get; init; }

    /// <summary>
    /// Gets the list of particle behaviors.
    /// </summary>
    public List<IBehavior> Behaviors { get; } = [];

    /// <summary>
    /// Adds the given <paramref name="behavior"/>.
    /// </summary>
    /// <param name="behavior">The behavior to add.</param>
    public void AddBehavior(IBehavior behavior)
    {
        if (Behaviors.Exists(b => b.BehaviorType == behavior.BehaviorType))
        {
            return;
        }

        Behaviors.Add(behavior);
    }

    /// <summary>
    /// Removes the given <paramref name="behaviorType"/>.
    /// </summary>
    /// <param name="behaviorType">The type of behavior to remove.</param>
    public void RemoveBehavior(BehaviorAttribute behaviorType)
    {
        var behavior = Behaviors.Find(b => b.BehaviorType == behaviorType);

        if (behavior is null)
        {
            return;
        }

        Behaviors.Remove(behavior);
    }
}
