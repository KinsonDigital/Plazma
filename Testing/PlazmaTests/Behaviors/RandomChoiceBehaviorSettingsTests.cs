// <copyright file="RandomChoiceBehaviorSettingsTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace PlazmaTests.Behaviors;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using Plazma;
using Plazma.Behaviors;
using Xunit;

public class RandomChoiceBehaviorSettingsTests
{
    /// <summary>
    /// Gets test data for testing the
    /// <see cref="Equals_WhenInvokingWithSameObjectType_ReturnsCorrectResult(int, ParticleAttribute, ReadOnlyCollection{string}, bool)"/>
    /// unit test for testing the <see cref="RandomChoiceBehaviorSettings.Equals(object?)"/> method.
    /// </summary>
    public static List<object[]> EqualsTestData =>
        new List<object[]>
        {
            //           lifeTime           attribute                           data                                  expected
            new object[] { 123,     ParticleAttribute.Angle,    new ReadOnlyCollection<string>(new[] { "item-1" }),     true },
            new object[] { 456,     ParticleAttribute.Angle,    new ReadOnlyCollection<string>(new[] { "item-1" }),     false },
            new object[] { 123,     ParticleAttribute.Color,    new ReadOnlyCollection<string>(new[] { "item-1" }),     false },
            new object[] { 123,     ParticleAttribute.Angle,    new ReadOnlyCollection<string>(new[] { "item-2" }),     false },
            new object[] { 123,     ParticleAttribute.Angle,                    null,                                   false },
        };

    #region Prop Tests
    [Fact]
    public void Data_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var settings = new RandomChoiceBehaviorSettings();

        // Act
        settings.Data = new ReadOnlyCollection<string>(new[] { "item-1", "item-2" });

        // Assert
        Assert.Equal(2, settings.Data.Count);
        Assert.Contains("item-1", settings.Data);
        Assert.Contains("item-2", settings.Data);
    }

    [Fact]
    public void LifeTime_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var settings = new RandomChoiceBehaviorSettings();

        // Act
        settings.LifeTime = 1234;

        // Assert
        Assert.Equal(1234, settings.LifeTime);
    }

    [Fact]
    public void Equals_WhenInvokingWithDifferentObjectType_ReturnsFalse()
    {
        // Arrange
        var settingsA = new RandomChoiceBehaviorSettings();
        object settingsB = "other-settings-type";

        // Act
        var actual = settingsA.Equals(settingsB);

        // Assert
        Assert.False(actual);
    }

    [Theory]
    [MemberData(nameof(EqualsTestData))]
    public void Equals_WhenInvokingWithSameObjectType_ReturnsCorrectResult(
        int lifeTime,
        ParticleAttribute attribute,
        ReadOnlyCollection<string> data,
        bool expected)
    {
        // Arrange
        var settingsA = new RandomChoiceBehaviorSettings
        {
            LifeTime = 123,
            ApplyToAttribute = ParticleAttribute.Angle,
            Data = new ReadOnlyCollection<string>(new[] { "item-1" }),
        };
        var settingsB = new RandomChoiceBehaviorSettings
        {
            LifeTime = lifeTime,
            ApplyToAttribute = attribute,
            Data = data,
        };

        // Act
        var actual = settingsA.Equals(settingsB);

        // Assert
        Assert.Equal(expected, actual);
    }
    #endregion
}