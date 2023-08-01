// <copyright file="Enums.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Plazma;

/// <summary>
/// Represents the different types of particle attributes that a behavior can be applied to.
/// </summary>
public enum ParticleAttribute
{
    /// <summary>
    /// The X position of a particle.
    /// </summary>
    X,

    /// <summary>
    /// The Y position of a particle.
    /// </summary>
    Y,

    /// <summary>
    /// The angle of a particle.
    /// </summary>
    Angle,

    /// <summary>
    /// The size of a particle.
    /// </summary>
    Size,

    /// <summary>
    /// The entire color of a particle.
    /// </summary>
    Color,

    /// <summary>
    /// The red color component of a particle color.
    /// </summary>
    RedColorComponent,

    /// <summary>
    /// The green color component of a particle color.
    /// </summary>
    GreenColorComponent,

    /// <summary>
    /// The blue color component of a particle color.
    /// </summary>
    BlueColorComponent,

    /// <summary>
    /// The alpha color component of a particle color.
    /// </summary>
    AlphaColorComponent,
}
