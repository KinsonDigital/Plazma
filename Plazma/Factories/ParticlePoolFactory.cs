// <copyright file="ParticlePoolFactory.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Plazma.Factories;

using System;

/// <inheritdoc/>
public sealed class ParticlePoolFactory : IParticlePoolFactory
{
    /// <inheritdoc/>
    public IParticlePool<TTexture> Create<TTexture>(ParticleEffect effect, ITextureLoader<TTexture> textureLoader)
        where TTexture : class
    {
        ArgumentNullException.ThrowIfNull(effect);
        ArgumentNullException.ThrowIfNull(textureLoader);

        return new ParticlePool<TTexture>(effect, textureLoader);
    }
}
