// <copyright file="RandomChoiceBehaviorSettings.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Plazma.Behaviors;

using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Various settings for behaviors that choose values randomly from a list of choices.
/// </summary>
public class RandomChoiceBehaviorSettings : BehaviorSettings
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RandomChoiceBehaviorSettings"/> class.
    /// </summary>
    public RandomChoiceBehaviorSettings()
    {
    }

    /// <summary>
    /// Gets or sets the data for the use by an <see cref="IBehavior"/> implementation.
    /// </summary>
    public ReadOnlyCollection<string>? Data { get; set; }

    /// <summary>
    /// Gets or sets the amount of time that the behavior should be enabled.
    /// </summary>
    public double LifeTime { get; set; }

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns>True if the specified object is equal to the current object; otherwise, false.</returns>
    public override bool Equals(object? obj)
    {
        if (!(obj is RandomChoiceBehaviorSettings setting))
        {
            return false;
        }

        return !(Data is null) && !(setting.Data is null) && Data.ItemsAreEqual(setting.Data) &&
            LifeTime == setting.LifeTime &&
            ApplyToAttribute == setting.ApplyToAttribute;
    }

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    [ExcludeFromCodeCoverage]
    public override int GetHashCode() => HashCode.Combine(Data);
}
