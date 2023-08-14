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
    public float RandomStartMin { get; set; }

    /// <summary>
    /// Gets or sets the maximum starting value used in randomization.
    /// </summary>
    public float RandomStartMax { get; set; }

    /// <summary>
    /// Gets or sets a delegate that will set the <see cref="RandomStartMin"/> value.
    /// </summary>
    /// <remarks>This is invoked during the behavior update process.</remarks>
    public Func<float>? UpdateRandomStartMin { get; set; }

    /// <summary>
    /// Gets or sets a delegate that will set the <see cref="RandomStartMax"/> value.
    /// </summary>
    /// <remarks>This is invoked during the behavior update process.</remarks>
    public Func<float>? UpdateRandomStartMax { get; set; }

    /// <summary>
    /// Gets or sets a delegate that will set the <see cref="RandomChangeMin"/> value.
    /// </summary>
    /// <remarks>This is invoked during the behavior update process.</remarks>
    public Func<float>? UpdateRandomChangeMin { get; set; }

    /// <summary>
    /// Gets or sets a delegate that will set the <see cref="RandomChangeMax"/> value.
    /// </summary>
    /// <remarks>This is invoked during the behavior update process.</remarks>
    public Func<float>? UpdateRandomChangeMax { get; set; }

    /// <summary>
    /// Gets or sets the minimum amount of change used in randomization.
    /// </summary>
    /// <remarks>
    ///     Use positive values to increase and negative values to decrease.
    /// </remarks>
    public float RandomChangeMin { get; set; }

    /// <summary>
    /// Gets or sets the maximum amount of change used in randomization.
    /// </summary>
    /// <remarks>
    ///     Use positive values to increase and negative values to decrease.
    /// </remarks>
    public float RandomChangeMax { get; set; }

    /// <summary>
    /// Gets or sets the minimum total amount of time in milliseconds to complete the behavior.
    /// </summary>
    /// <remarks>A value less then or equal to 0 will result in the behavior not working.</remarks>
    public float LifeTimeMinMilliseconds { get; set; } // TODO: Needs to be required somehow

    /// <summary>
    /// Gets or sets the maximum total amount of time in milliseconds to complete the behavior.
    /// </summary>
    /// <remarks>A value less then or equal to 0 will result in the behavior not working.</remarks>
    public float LifeTimeMaxMilliseconds { get; set; } // TODO: Needs to be required somehow
}
