// <copyright file="IParticlePoolFactory.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Plazma.Factories;

/// <summary>
/// Creates new particle pools.
/// </summary>
public interface IParticlePoolFactory
{
    /// <summary>
    /// Creates a new instance of a particle pool with the given particle <paramref name="effect"/>
    /// and the given <paramref name="textureLoader"/> to load the textures for rendering.
    /// </summary>
    /// <param name="effect">The particle effect to apply to the pool.</param>
    /// <param name="textureLoader">The texture loader to use to load textures.</param>
    /// <typeparam name="TTexture">The type of texture to use in the pool.</typeparam>
    /// <returns>A new <see cref="IParticlePool{TTexture}"/> instance.</returns>
    IParticlePool<TTexture> Create<TTexture>(ParticleEffect effect, ITextureLoader<TTexture> textureLoader)
        where TTexture : class;
}
