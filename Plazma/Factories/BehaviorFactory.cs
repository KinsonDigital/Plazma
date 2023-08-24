// <copyright file="BehaviorFactory.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Plazma.Factories;

using Behaviors;
using Services;

/// <inheritdoc/>
internal sealed class BehaviorFactory : IBehaviorFactory
{
    /// <inheritdoc/>
    public EasingRandomBehavior CreateEasingRandomBehavior(EasingRandomBehaviorSettings settings)
    {
        var randomService = IoC.Container.GetInstance<IRandomizerService>();

        return new EasingRandomBehavior(settings, randomService);
    }
}
