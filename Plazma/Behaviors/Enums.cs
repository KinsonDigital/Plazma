// <copyright file="Enums.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Plazma.Behaviors;

/// <summary>
/// Represents different types of easing functions that a behavior can apply to a <see cref="Particle"/> attribute.
/// </summary>
public enum EasingFunction
{
    /// <summary>
    /// Ease in easing function.
    /// </summary>
    EaseIn,

    /// <summary>
    /// Ease out bounce easing function.
    /// </summary>
    EaseOutBounce,
}
