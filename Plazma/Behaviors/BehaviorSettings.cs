// <copyright file="BehaviorSettings.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Plazma.Behaviors;

using System;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Holds various settings for a particle behavior.
/// </summary>
public abstract class BehaviorSettings
{
    /// <summary>
    /// Gets or sets the particle attribute to set the behavior value to.
    /// </summary>
    public ParticleAttribute ApplyToAttribute { get; set; }

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns>True if the specified object is equal to the current object; otherwise, false.</returns>
    public override bool Equals(object? obj)
    {
        if (!(obj is BehaviorSettings setting))
        {
            return false;
        }

        return ApplyToAttribute == setting.ApplyToAttribute;
    }

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    [ExcludeFromCodeCoverage]
    public override int GetHashCode() => HashCode.Combine(ApplyToAttribute);
}