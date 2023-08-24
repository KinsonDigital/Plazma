// <copyright file="IParticle.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Plazma;

using System;
using System.Collections.Immutable;
using System.Drawing;
using System.Numerics;
using Behaviors;

/// <summary>
/// Represents a single particle with various properties that dictate how the <see cref="Particle"/>
/// behaves and looks on the screen.
/// </summary>
public interface IParticle
{
    /// <summary>
    /// Gets or sets the position of the <see cref="Particle"/>.
    /// </summary>
    Vector2 Position { get; set; }

    /// <summary>
    /// Gets or sets the angle of the <see cref="Particle"/>.
    /// </summary>
    float Angle { get; set; }

    /// <summary>
    /// Gets or sets the color that the texture will be tinted to.
    /// </summary>
    Color TintColor { get; set; }

    /// <summary>
    /// Gets or sets the size of the <see cref="Particle"/>.
    /// </summary>
    float Size { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the <see cref="Particle"/> is alive or dead.
    /// </summary>
    bool IsAlive { get; set; }

    /// <summary>
    /// Gets a list of the types of behaviors that the particle has.
    /// </summary>
    ImmutableArray<BehaviorAttribute> Behaviors { get; }

    /// <summary>
    /// Updates the particle.
    /// </summary>
    /// <param name="timeElapsed">The amount of time that has elapsed since the last frame.</param>
    void Update(TimeSpan timeElapsed);

    /// <summary>
    /// Adds the given behavior to the particle.
    /// </summary>
    /// <param name="behavior">The behavior to add.</param>
    void AddBehavior(IBehavior behavior);

    /// <summary>
    /// Removes a behavior that matches the given <paramref name="behaviorType"/>.
    /// </summary>
    /// <param name="behaviorType">The type of behavior to remove.</param>
    void RemoveBehavior(BehaviorAttribute behaviorType);

    /// <summary>
    /// Resets the particle.
    /// </summary>
    void Reset();
}
