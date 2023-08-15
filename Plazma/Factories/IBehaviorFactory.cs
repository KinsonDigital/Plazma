// <copyright file="IBehaviorFactory.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Plazma.Factories;

using Behaviors;

/// <summary>
/// Creates behaviors to use in particle effects.
/// </summary>
internal interface IBehaviorFactory
{
    /// <summary>
    /// Creates a new instance of an <see cref="EasingRandomBehavior"/>.
    /// </summary>
    /// <param name="settings">The settings to apply to the behavior.</param>
    /// <returns>A new <see cref="EasingRandomBehavior"/> instance.</returns>
    EasingRandomBehavior CreateEasingRandomBehavior(EasingRandomBehaviorSettings settings);
}
