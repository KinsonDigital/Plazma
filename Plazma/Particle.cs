// <copyright file="Particle.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Plazma;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Numerics;
using Behaviors;

/// <inheritdoc cref="IParticle"/>
public class Particle : IParticle
{
    private readonly List<IBehavior> behaviors = new ();

    /// <summary>
    /// Initializes a new instance of the <see cref="Particle"/> class.
    /// </summary>
    public Particle()
    {
    }

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

    /// <inheritdoc/>
    public ImmutableArray<BehaviorAttribute> Behaviors => this.behaviors.Select(b => b.BehaviorType).ToImmutableArray();

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

                switch (behavior.BehaviorType)
                {
                    case BehaviorAttribute.X:
                        Position = new Vector2(value, Position.Y);
                        break;
                    case BehaviorAttribute.Y:
                        Position = new Vector2(Position.X, value);
                        break;
                    case BehaviorAttribute.Angle:
                        Angle = value;
                        break;
                    case BehaviorAttribute.Size:
                        Size = value;
                        break;
                    case BehaviorAttribute.AlphaColorComponent:
                        TintColor = Color.FromArgb(ClampClrValue(value), TintColor.R, TintColor.G, TintColor.B);
                        break;
                    case BehaviorAttribute.RedColorComponent:
                        TintColor = Color.FromArgb(TintColor.A, ClampClrValue(value), TintColor.G, TintColor.B);
                        break;
                    case BehaviorAttribute.GreenColorComponent:
                        TintColor = Color.FromArgb(TintColor.A, TintColor.R, ClampClrValue(value), TintColor.B);
                        break;
                    case BehaviorAttribute.BlueColorComponent:
                        TintColor = Color.FromArgb(TintColor.A, TintColor.R, TintColor.G, ClampClrValue(value));
                        break;
                    default:
                        throw new InvalidEnumArgumentException(nameof(BehaviorAttribute), (int)behavior.BehaviorType, typeof(BehaviorAttribute));
                }
            }
        }
    }

    /// <inheritdoc/>
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
