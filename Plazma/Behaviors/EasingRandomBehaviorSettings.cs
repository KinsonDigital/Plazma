// <copyright file="EasingRandomBehaviorSettings.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Plazma.Behaviors;

using System;

/// <summary>
/// Stores settings for creating an <see cref="EasingRandomBehavior"/>.
/// </summary>
public class EasingRandomBehaviorSettings : IBehaviorSettings, IEasingCapable
{
    /// <inheritdoc/>
    public ParticleAttribute ApplyToAttribute { get; set; }

    /// <inheritdoc/>
    public double LifeTime { get; } = 1000.0;

    /// <inheritdoc/>
    public EasingFunction EasingFunctionType { get; set; } = EasingFunction.EaseIn;

    /// <summary>
    /// Gets or sets the minimum starting value used in randomization.
    /// </summary>
    /// <remarks>
    ///     Negative values can be used as a stopping point.
    /// </remarks>
    public float RandomStartMin { get; set; }

    /// <summary>
    /// Gets or sets the maximum starting value used in randomization.
    /// </summary>
    /// <remarks>
    ///     Negative values can be used as a stopping point.
    /// </remarks>
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
    /// Gets or sets a delegate that will set the <see cref="RandomStopMin"/> value.
    /// </summary>
    /// <remarks>This is invoked during the behavior update process.</remarks>
    public Func<float>? UpdateRandomChangeMin { get; set; }

    /// <summary>
    /// Gets or sets a delegate that will set the <see cref="RandomStopMax"/> value.
    /// </summary>
    /// <remarks>This is invoked during the behavior update process.</remarks>
    public Func<float>? UpdateRandomChangeMax { get; set; }

    /// <summary>
    /// Gets or sets the minimum amount of change used in randomization.
    /// </summary>
    /// <remarks>
    ///     Negative values can be used as a stopping point.
    /// </remarks>
    public float RandomStopMin { get; set; }

    /// <summary>
    /// Gets or sets the maximum stopping value used in randomization.
    /// </summary>
    /// <remarks>
    ///     Negative values can be used as a stopping point.
    /// </remarks>
    public float RandomStopMax { get; set; }

    /// <summary>
    /// Gets or sets the minimum total amount of time in milliseconds to complete the behavior.
    /// </summary>
    /// <remarks>A value less then or equal to 0 will result in the behavior not working.</remarks>
    public float LifeTimeMillisecondsMin { get; set; } // TODO: Needs to be required somehow

    /// <summary>
    /// Gets or sets the maximum total amount of time in milliseconds to complete the behavior.
    /// </summary>
    /// <remarks>A value less then or equal to 0 will result in the behavior not working.</remarks>
    public float LifeTimeMillisecondsMax { get; set; } // TODO: Needs to be required somehow
}
