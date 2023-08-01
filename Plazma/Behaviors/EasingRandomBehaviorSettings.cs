// <copyright file="EasingRandomBehaviorSettings.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Plazma.Behaviors;

using System;

/// <summary>
/// Stores settings for creating an <see cref="EasingRandomBehavior"/>.
/// </summary>
public class EasingRandomBehaviorSettings : BehaviorSettings, IEasingCapable
{
    /// <inheritdoc/>
    public EasingFunction EasingFunctionType { get; set; } = EasingFunction.EaseIn;

    /// <summary>
    /// Gets or sets the minimum starting value used in randomization.
    /// </summary>
    public float StartMin { get; set; }

    /// <summary>
    /// Gets or sets the maximum starting value used in randomization.
    /// </summary>
    public float StartMax { get; set; }

    /// <summary>
    /// Gets or sets a delegate that when invoked updates the <see cref="StartMin"/> value.
    /// </summary>
    public Func<float>? UpdateStartMin { get; set; }

    /// <summary>
    /// Gets or sets a delegate that when invoked updates the <see cref="StartMax"/> value.
    /// </summary>
    public Func<float>? UpdateStartMax { get; set; }

    /// <summary>
    /// Gets or sets a delegate that when invoked updates the <see cref="ChangeMin"/> value.
    /// </summary>
    public Func<float>? UpdateChangeMin { get; set; }

    /// <summary>
    /// Gets or sets a delegate that when invoked updates the <see cref="ChangeMax"/> value.
    /// </summary>
    public Func<float>? UpdateChangeMax { get; set; }

    /// <summary>
    /// Gets or sets the minimum amount of change used in randomization.
    /// </summary>
    /// <remarks>
    ///     Use positive values to increase and negative values to decrease.
    /// </remarks>
    public float ChangeMin { get; set; }

    /// <summary>
    /// Gets or sets the maximum amount of change used in randomization.
    /// </summary>
    /// <remarks>
    ///     Use positive values to increase and negative values to decrease.
    /// </remarks>
    public float ChangeMax { get; set; }

    /// <summary>
    /// Gets or sets the minimum total amount of time in milliseconds to complete the behavior.
    /// </summary>
    /// <remarks>A value less then or equal to 0 will result in the behavior not working.</remarks>
    public float TotalTimeMin { get; set; }

    /// <summary>
    /// Gets or sets the maximum total amount of time in milliseconds to complete the behavior.
    /// </summary>
    /// <remarks>A value less then or equal to 0 will result in the behavior not working.</remarks>
    public float TotalTimeMax { get; set; }

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns>True if the specified object is equal to the current object; otherwise, false.</returns>
    public override bool Equals(object? obj)
    {
        if (!(obj is EasingRandomBehaviorSettings setting))
        {
            return false;
        }

        return ApplyToAttribute == setting.ApplyToAttribute &&
               StartMin == setting.StartMin &&
               StartMax == setting.StartMax &&
               ChangeMin == setting.ChangeMin &&
               ChangeMax == setting.ChangeMax &&
               TotalTimeMin == setting.TotalTimeMin &&
               TotalTimeMax == setting.TotalTimeMax;
    }

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode() =>
        HashCode.Combine(
            ApplyToAttribute,
            StartMin,
            StartMax,
            ChangeMin,
            ChangeMax,
            TotalTimeMin,
            TotalTimeMax);
}