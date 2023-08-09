// <copyright file="IBehaviorFactory.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Plazma.Behaviors;

using Services;

/// <summary>
/// Creates behaviors using behavior settings.
/// </summary>
public interface IBehaviorFactory
{
    /// <summary>
    /// Creates all of the behaviors using the given <paramref name="settings"/> and <paramref name="randomService"/>.
    /// </summary>
    /// <param name="settings">The list of settings used to create each behavior.</param>
    /// <param name="randomService">Used to generate random values from the given <paramref name="settings"/> parameter.</param>
    /// <returns>A list of behaviors.</returns>
    IBehavior[] CreateBehaviors(BehaviorSettings[] settings, IRandomizerService randomService);
}
