// <copyright file="RandomChoiceBehaviorSettingsTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

// ReSharper disable UseObjectOrCollectionInitializer
namespace PlazmaTests.Behaviors;

using System.Collections.ObjectModel;
using FluentAssertions;
using Plazma.Behaviors;
using Xunit;

/// <summary>
/// Tests the <see cref="RandomChoiceBehaviorSettings"/> class.
/// </summary>
public class RandomChoiceBehaviorSettingsTests
{
    #region Prop Tests
    [Fact]
    public void Data_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var settings = new RandomChoiceBehaviorSettings();

        // Act
        settings.Data = new ReadOnlyCollection<string>(new[] { "item-1", "item-2" });

        // Assert
        settings.Data.Should().HaveCount(2);
        settings.Data.Should().BeEquivalentTo("item-1", "item-2");
    }

    [Fact]
    public void LifeTime_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var settings = new RandomChoiceBehaviorSettings();

        // Act
        settings.LifeTime = 1234;

        // Assert
        settings.LifeTime.Should().Be(1234);
    }
    #endregion
}
