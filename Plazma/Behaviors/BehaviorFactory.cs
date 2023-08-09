// <copyright file="BehaviorFactory.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

#pragma warning disable CA1303 // Do not pass literals as localized parameters
namespace Plazma.Behaviors;

using System;
using System.Collections.Generic;
using Services;

/// <summary>
/// Creates behaviors using behavior settings.
/// </summary>
public class BehaviorFactory : IBehaviorFactory
{
    /// <summary>
    /// Creates all of the behaviors using the given <paramref name="randomizerService"/>.
    /// </summary>
    /// <param name="settings">The list of settings used to create each behavior.</param>
    /// <param name="randomizerService">The random used to randomly generate values.</param>
    /// <returns>A list of behaviors.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="settings"/> parameter is null.</exception>
    public IBehavior[] CreateBehaviors(BehaviorSettings[] settings, IRandomizerService randomizerService)
    {
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(randomizerService);

        var behaviors = new List<IBehavior>();

        // Creates all of the behaviors using the given settings
        foreach (var setting in settings)
        {
            switch (setting)
            {
                case EasingRandomBehaviorSettings easingRandomBehaviorSettings:
                    behaviors.Add(new EasingRandomBehavior(easingRandomBehaviorSettings, randomizerService));
                    break;
                case ColorTransitionBehaviorSettings clrTransitionBehaviorSettings:
                    behaviors.Add(new ColorTransitionBehavior(clrTransitionBehaviorSettings));
                    break;
                case RandomChoiceBehaviorSettings randomChoiceBehaviorSettings:
                    behaviors.Add(new RandomColorBehavior(randomChoiceBehaviorSettings, randomizerService));
                    break;
                default:
                    throw new Exception($"Unknown behavior settings of type '{setting.GetType().Name}'.");
            }
        }

        return behaviors.ToArray();
    }
}
