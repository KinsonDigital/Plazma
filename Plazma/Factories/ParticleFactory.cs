// <copyright file="ParticleFactory.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Plazma.Factories;

using Behaviors;

/// <inheritdoc/>
internal sealed class ParticleFactory : IParticleFactory
{
    /// <inheritdoc/>
    public IParticle Create(IBehavior[] behaviors) => new Particle(behaviors);
}
