// <copyright file="Particle.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Plazma;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Numerics;
using Behaviors;

/// <summary>
/// <inheritdoc/>
/// </summary>
public class Particle : IParticle
{
    private readonly IBehavior[] behaviors;

    /// <summary>
    /// Initializes a new instance of the <see cref="Particle"/> class.
    /// </summary>
    /// <param name="behaviors">The list of behaviors to add to the <see cref="Particle"/>.</param>
    public Particle(IBehavior[] behaviors) => this.behaviors = behaviors;

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
    public Color TintColor { get; set; } = Color.White;

    /// <summary>
    /// Gets or sets the size of the <see cref="Particle"/>.
    /// </summary>
    public float Size { get; set; } = 1;

    /// <summary>
    /// Gets or sets a value indicating whether the <see cref="Particle"/> is alive or dead.
    /// </summary>
    public bool IsAlive { get; set; }

    /// <summary>
    /// Updates the particle.
    /// </summary>
    /// <param name="timeElapsed">The amount of time that has elapsed since the last frame.</param>
    public void Update(TimeSpan timeElapsed)
    {
        IsAlive = false;

        static byte ClampClrValue(float value)
        {
            return (byte)(value < 0 ? 0 : value);
        }

        // Apply the behavior values to the particle attributes
        foreach (var behavior in this.behaviors)
        {
            if (behavior.Enabled)
            {
                behavior.Update(timeElapsed);
                IsAlive = true;

                var value = (float)behavior.Value;

                switch (behavior.ApplyToAttribute)
                {
                    case ParticleAttribute.X:
                        Position = new Vector2(value, Position.Y);
                        break;
                    case ParticleAttribute.Y:
                        Position = new Vector2(Position.X, value);
                        break;
                    case ParticleAttribute.Angle:
                        Angle = value;
                        break;
                    case ParticleAttribute.Size:
                        Size = value;
                        break;
                    case ParticleAttribute.AlphaColorComponent:
                        TintColor = Color.FromArgb(ClampClrValue(value), TintColor.R, TintColor.G, TintColor.B);
                        break;
                    case ParticleAttribute.RedColorComponent:
                        TintColor = Color.FromArgb(TintColor.A, ClampClrValue(value), TintColor.G, TintColor.B);
                        break;
                    case ParticleAttribute.GreenColorComponent:
                        TintColor = Color.FromArgb(TintColor.A, TintColor.R, ClampClrValue(value), TintColor.B);
                        break;
                    case ParticleAttribute.BlueColorComponent:
                        TintColor = Color.FromArgb(TintColor.A, TintColor.R, TintColor.G, ClampClrValue(value));
                        break;
                    default:
                        throw new InvalidEnumArgumentException(nameof(ParticleAttribute), (int)behavior.ApplyToAttribute, typeof(ParticleAttribute));
                }
            }
        }
    }

    /// <summary>
    /// Resets the particle.
    /// </summary>
    public void Reset()
    {
        foreach (var behavior in this.behaviors)
        {
            behavior.Reset();
        }

        Size = 1;
        Angle = 0;
        TintColor = Color.White;
        IsAlive = true;
    }
}
