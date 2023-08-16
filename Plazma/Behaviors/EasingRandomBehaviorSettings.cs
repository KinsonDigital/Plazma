// <copyright file="EasingRandomBehaviorSettings.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Plazma.Behaviors;

using System;

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
    /// Gets a delegate that will set the <see cref="RandomStartMin"/> value.
    /// </summary>
    /// <remarks>This is invoked during the behavior update process.</remarks>
    public Func<float>? UpdateRandomStartMin { get; init; }

    /// <summary>
    /// Gets a delegate that will set the <see cref="RandomStartMax"/> value.
    /// </summary>
    /// <remarks>This is invoked during the behavior update process.</remarks>
    public Func<float>? UpdateRandomStartMax { get; init; }

    /// <summary>
    /// Gets a delegate that will set the <see cref="RandomStopMin"/> value.
    /// </summary>
    /// <remarks>This is invoked during the behavior update process.</remarks>
    public Func<float>? UpdateRandomStopMin { get; init; }

    /// <summary>
    /// Gets a delegate that will set the <see cref="RandomStopMax"/> value.
    /// </summary>
    /// <remarks>This is invoked during the behavior update process.</remarks>
    public Func<float>? UpdateRandomStopMax { get; init; }

    /// <summary>
    /// Gets the minimum amount of change used in randomization.
    /// </summary>
    /// <remarks>
    ///     Negative values can be used as a stopping point.
    /// </remarks>
    public float RandomStopMin { get; init; }

    /// <summary>
    /// Gets the maximum stopping value used in randomization.
    /// </summary>
    /// <remarks>
    ///     Negative values can be used as a stopping point.
    /// </remarks>
    public float RandomStopMax { get; init; }

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
