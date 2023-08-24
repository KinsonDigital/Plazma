// <copyright file="IParticleFactory.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Plazma.Factories;

using Behaviors;

/// <summary>
/// Creates new instances of <see cref="IParticle"/>s with various behaviors.
/// </summary>
public interface IParticleFactory
{
    /// <summary>
    /// Creates a new <see cref="IParticle"/> with the given <paramref name="behaviors"/> applied.
    /// </summary>
    /// <param name="behaviors">The behaviors to apply.</param>
    /// <returns>A new <see cref="IParticle"/> instance.</returns>
    IParticle Create(IBehavior[] behaviors);
}
