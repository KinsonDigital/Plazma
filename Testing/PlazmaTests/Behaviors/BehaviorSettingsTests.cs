// <copyright file="BehaviorSettingsTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace PlazmaTests.Behaviors;

using Fakes;
using Plazma;
using Xunit;

public class BehaviorSettingsTests
{
    #region Method Tests
    [Fact]
    public void Equals_WhenInvokedWithDifferentObjectType_ReturnsFalse()
    {
        // Arrange
        var settingsA = new FakeBehaviorSettings();
        object settingsB = "other-type";

        // Act
        var actual = settingsA.Equals(settingsB);

        // Assert
        Assert.False(actual);
    }

    [Theory]
    [InlineData(ParticleAttribute.Angle, true)]
    public void Equals_WhenInvokedWithSameObjectType_ReturnsTrue(ParticleAttribute attribute, bool expected)
    {
        // Arrange
        var settingsA = new FakeBehaviorSettings
        {
            ApplyToAttribute = ParticleAttribute.Angle,
        };
        var settingsB = new FakeBehaviorSettings
        {
            ApplyToAttribute = attribute,
        };

        // Act
        var actual = settingsA.Equals(settingsB);

        // Assert
        Assert.Equal(expected, actual);
    }
    #endregion
}