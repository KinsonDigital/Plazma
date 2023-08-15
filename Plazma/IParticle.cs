﻿// <copyright file="IParticle.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Plazma;

using System;
using System.Drawing;
using System.Numerics;

/// <summary>
/// Represents a single particle with various properties that dictate how the <see cref="Particle"/>
/// behaves and looks on the screen.
/// </summary>
public interface IParticle
{
    /// <summary>
    /// Gets or sets the position of the <see cref="Particle"/>.
    /// </summary>
    public Vector2 Position { get; set; }

    /// <summary>
    /// Gets or sets the angle of the <see cref="Particle"/>.
    /// </summary>
    public float Angle { get; set; }

    /// <summary>
    /// Gets or sets the color that the texture will be tinted to.
    /// </summary>
    public Color TintColor { get; set; }

    /// <summary>
    /// Gets or sets the size of the <see cref="Particle"/>.
    /// </summary>
    public float Size { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the <see cref="Particle"/> is alive or dead.
    /// </summary>
    public bool IsAlive { get; set; }

    /// <summary>
    /// Updates the particle.
    /// </summary>
    /// <param name="timeElapsed">The amount of time that has elapsed since the last frame.</param>
    public void Update(TimeSpan timeElapsed);

    /// <summary>
    /// Resets the particle.
    /// </summary>
    public void Reset();
}
