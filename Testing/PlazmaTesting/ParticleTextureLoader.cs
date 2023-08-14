namespace PlazmaTesting;

using Plazma;
using Velaptor.Content;
using Velaptor.Factories;

/// <summary>
/// Loads particle textures.
/// </summary>
public class ParticleTextureLoader : ITextureLoader<ITexture>
{
    private readonly ILoader<ITexture> loader = ContentLoaderFactory.CreateTextureLoader();

    /// <summary>
    /// Loads a single particle texture with the given <paramref name="assetName"/>.
    /// </summary>
    /// <param name="assetName">The name of the asset to load.</param>
    /// <returns></returns>
    public ITexture LoadTexture(string assetName)
    {
        return this.loader.Load(assetName);
    }
}
