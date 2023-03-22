// <copyright file="ParticleEffectSerializerService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Partithyst.Services;

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Newtonsoft.Json;

/// <summary>
/// Serializes and deserializes and object of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of object to serialize.</typeparam>
[ExcludeFromCodeCoverage]
public class ParticleEffectSerializerService : ISerializerService<ParticleEffect>
{
    private readonly JsonSerializerSettings jsonSettings;

    /// <summary>
    /// Initializes a new instance of the <see cref="ParticleEffectSerializerService"/> class.
    /// </summary>
    public ParticleEffectSerializerService() => this.jsonSettings = new JsonSerializerSettings()
    {
        Formatting = Formatting.Indented,
        TypeNameHandling = TypeNameHandling.Objects,
    };

    /// <inheritdoc/>
    public void Serialize(string filePath, ParticleEffect effect)
    {
        if (effect is null)
        {
            throw new ArgumentNullException(nameof(effect), "Parameter must not be null.");
        }

        var jsonData = JsonConvert.SerializeObject(effect, this.jsonSettings);

        File.WriteAllText(filePath, jsonData);
    }

    /// <inheritdoc/>
    public ParticleEffect? Deserialize(string filePath)
    {
        var jsonData = File.ReadAllText(filePath);

        return JsonConvert.DeserializeObject<ParticleEffect>(jsonData, this.jsonSettings);
    }
}