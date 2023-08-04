// <copyright file="IEasingCapable.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Plazma.Behaviors;

/// <summary>
/// Provides the ability that something has easing function capabilities.
/// </summary>
public interface IEasingCapable
{
    /// <summary>
    /// Gets or sets the type of easing function that will be used.
    /// </summary>
    EasingFunction EasingFunctionType { get; set; }
}
