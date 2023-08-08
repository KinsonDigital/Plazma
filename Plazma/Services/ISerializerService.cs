// <copyright file="ISerializerService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Plazma.Services;

/// <summary>
/// Serializes and deserializes and object of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of object to serialize.</typeparam>
public interface ISerializerService<in T>
{
    /// <summary>
    /// Serializes the given <paramref name="obj"/> at the given <paramref name="filePath"/>.
    /// </summary>
    /// <param name="filePath">The path and file name of the data file.</param>
    /// <param name="obj">The object to serialize.</param>
    void Serialize(string filePath, T obj);

    /// <summary>
    /// Deserializes the JSON data at the given <paramref name="filePath"/>
    /// and returns it as type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="filePath">The path to the file of the JSON data to deserialize.</param>
    /// <returns>The deserialized object.</returns>
    ParticleEffect? Deserialize(string filePath);
}
