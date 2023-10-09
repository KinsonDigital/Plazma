// <copyright file="EasingRandomBehaviorSettings.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Plazma.Behaviors;

using System;
using System.Text.Json.Serialization;

/// <summary>
/// Stores settings for creating an <see cref="EasingRandomBehavior"/>.
/// </summary>
public readonly record struct EasingRandomBehaviorSettings
{
    /// <summary>
    /// Gets the attribute of the particle to apply the easing function to.
    /// </summary>
    public BehaviorAttribute ApplyToAttribute { get; init; }

    /// <summary>
    /// Gets the type of easing function to use.
    /// </summary>
    public EasingFunction EasingFunctionType { get; init; }

    /// <summary>
    /// Gets the minimum starting value used in randomization.
    /// </summary>
    /// <remarks>
    ///     Negative values can be used as a stopping point.
    /// </remarks>
    public float RandomStartMin { get; init; }

    /// <summary>
    /// Gets the maximum starting value used in randomization.
    /// </summary>
    /// <remarks>
    ///     Negative values can be used as a stopping point.
    /// </remarks>
    public float RandomStartMax { get; init; }

    /// <summary>
    /// Gets a delegate that will give the current value and return a value.
    /// </summary>
    /// <remarks>
    /// <para>
    ///     This is invoked during the behavior update process.
    /// </para>
    /// <para>
    ///     The value returned from the delegate will be used as the new value for the behavior.
    /// </para>
    /// </remarks>
    [JsonIgnore]
    public Func<double, double>? UpdateValue { get; init; }

    /// <summary>
    /// Gets a delegate that will give the current value of the behavior and
    /// returns a value that will be used as the new  <see cref="RandomStartMin"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    ///     This is invoked during the behavior update process.
    /// </para>
    /// <para>
    ///     The value returned from the delegate will be used as the new value for the <see cref="RandomStartMin"/>.
    /// </para>
    /// </remarks>
    [JsonIgnore]
    public Func<double, float>? UpdateRandomStartMin { get; init; }

    /// <summary>
    /// Gets a delegate that will give the current value of the behavior and
    /// returns a value that will be used as the new  <see cref="RandomStartMax"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    ///     This is invoked during the behavior update process.
    /// </para>
    /// <para>
    ///     The value returned from the delegate will be used as the new value for the <see cref="RandomStartMax"/>.
    /// </para>
    /// </remarks>
    [JsonIgnore]
    public Func<double, float>? UpdateRandomStartMax { get; init; }

    /// <summary>
    /// Gets the minimum amount of change used in randomization.
    /// </summary>
    /// <remarks>
    ///     Negative values can be used.
    /// </remarks>
    public float RandomChangeMin { get; init; }

    /// <summary>
    /// Gets the maximum amount of change used in randomization.
    /// </summary>
    /// <remarks>
    ///     Negative values can be used.
    /// </remarks>
    public float RandomChangeMax { get; init; }

    /// <summary>
    /// Gets the minimum total amount of time in milliseconds to complete the behavior.
    /// </summary>
    /// <remarks>A value less then or equal to 0 will result in the behavior not working.</remarks>
    public float LifeTimeMillisecondsMin { get; init; }

    /// <summary>
    /// Gets the maximum total amount of time in milliseconds to complete the behavior.
    /// </summary>
    /// <remarks>A value less then or equal to 0 will result in the behavior not working.</remarks>
    public float LifeTimeMillisecondsMax { get; init; }
}
