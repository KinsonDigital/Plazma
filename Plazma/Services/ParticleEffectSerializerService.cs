// <copyright file="ParticleEffectSerializerService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Plazma.Services;

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json;

/// <summary>
/// Serializes and deserializes and <see cref="ParticleEffect"/> objects.
/// </summary>
[ExcludeFromCodeCoverage]
public class ParticleEffectSerializerService : ISerializerService<ParticleEffect>
{
    private readonly JsonSerializerOptions jsonOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="ParticleEffectSerializerService"/> class.
    /// </summary>
    public ParticleEffectSerializerService() => this.jsonOptions = new JsonSerializerOptions
    {
        WriteIndented = true,
    };

    /// <inheritdoc/>
    public void Serialize(string filePath, ParticleEffect effect)
    {
        if (effect is null)
        {
            throw new ArgumentNullException(nameof(effect), "Parameter must not be null.");
        }

        var jsonData = JsonSerializer.Serialize(effect, this.jsonOptions);

        File.WriteAllText(filePath, jsonData);
    }

    /// <inheritdoc/>
    public ParticleEffect? Deserialize(string filePath)
    {
        var jsonData = File.ReadAllText(filePath);

        return JsonSerializer.Deserialize<ParticleEffect>(jsonData, this.jsonOptions);
    }
}
