// <copyright file="IBehaviorSettings.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Plazma.Behaviors;

public interface IBehaviorSettings
{
    /// <summary>
    /// Gets or sets the particle attribute to set the behavior value to.
    /// </summary>
    ParticleAttribute ApplyToAttribute { get; set; }
}
