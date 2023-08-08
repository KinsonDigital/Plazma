// <copyright file="ColorTransitionBehaviorTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace PlazmaTests.Behaviors;

using System;
using Plazma;
using Plazma.Behaviors;
using Xunit;

public class ColorTransitionBehaviorTests
{
    #region Prop Tests
    [Fact]
    public void Value_WhenGettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var settings = new ColorTransitionBehaviorSettings
        {
            LifeTime = 1000,
            StartColor = new ParticleColor(255, 62, 125, 200),
            StopColor = new ParticleColor(255, 62, 125, 200),
        };
        var behavior = new ColorTransitionBehavior(settings);

        // Act
        behavior.Update(new TimeSpan(0, 0, 0, 0, 1500));

        // Assert
        Assert.Equal("clr:255,62,125,200", behavior.Value);
    }

    [Fact]
    public void ElapsedTime_WhenGettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var settings = new ColorTransitionBehaviorSettings
        {
            LifeTime = 1000,
            StartColor = new ParticleColor(255, 62, 125, 200),
            StopColor = new ParticleColor(255, 62, 125, 200),
        };
        var behavior = new ColorTransitionBehavior(settings);

        // Act
        behavior.Update(new TimeSpan(0, 0, 0, 0, 1500));

        // Assert
        Assert.Equal(1500, behavior.ElapsedTime);
    }

    [Fact]
    public void ApplyToAttribute_WhenGettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var settings = new ColorTransitionBehaviorSettings
        {
            LifeTime = 1000,
            StartColor = new ParticleColor(255, 62, 125, 200),
            StopColor = new ParticleColor(255, 62, 125, 200),
        };
        var behavior = new ColorTransitionBehavior(settings);

        // Act
        behavior.Update(new TimeSpan(0, 0, 0, 0, 1500));

        // Assert
        Assert.Equal(ParticleAttribute.Color, behavior.ApplyToAttribute);
    }

    [Fact]
    public void Enabled_WhenGettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var settings = new ColorTransitionBehaviorSettings
        {
            LifeTime = 1000,
            StartColor = new ParticleColor(255, 62, 125, 200),
            StopColor = new ParticleColor(255, 62, 125, 200),
        };
        var behavior = new ColorTransitionBehavior(settings);

        // Act
        behavior.Update(new TimeSpan(0, 0, 0, 0, 800));

        // Assert
        Assert.True(behavior.Enabled);
    }
    #endregion

    #region Method Tests
    [Theory]
    //                easingFunction                            StartColor                      StopColor                Elapsed Time               Expected Result(ARGB)
    [InlineData(EasingFunction.EaseIn,          new byte[] {   0,   0,   0,   0 },  new byte[] { 255,   0,   0,   0 },      1500,           "clr:255,0,0,0")]   // Increasing of alpha
    [InlineData(EasingFunction.EaseIn,          new byte[] { 255,   0,   0,   0 },  new byte[] {   0,   0,   0,   0 },       800,           "clr:91,0,0,0")]    // Decreasing of alpha
    [InlineData(EasingFunction.EaseIn,          new byte[] { 255,   0,   0,   0 },  new byte[] { 255, 255,   0,   0 },      1500,           "clr:255,255,0,0")] // Increasing of red
    [InlineData(EasingFunction.EaseIn,          new byte[] { 255, 255,   0,   0 },  new byte[] { 255,   0,   0,   0 },       800,           "clr:255,91,0,0")]  // Decreasing of red
    [InlineData(EasingFunction.EaseIn,          new byte[] { 255,   0,   0,   0 },  new byte[] { 255,   0, 255,   0 },      1500,           "clr:255,0,255,0")] // Increasing of green
    [InlineData(EasingFunction.EaseIn,          new byte[] { 255,   0, 255,   0 },  new byte[] { 255,   0,   0,   0 },       800,           "clr:255,0,91,0")]  // Decreasing of green
    [InlineData(EasingFunction.EaseIn,          new byte[] { 255,   0,   0,   0 },  new byte[] { 255,   0,   0, 255 },      1500,           "clr:255,0,0,255")] // Increasing of blue
    [InlineData(EasingFunction.EaseIn,          new byte[] { 255,   0,   0, 255 },  new byte[] { 255,   0,   0,   0 },       800,           "clr:255,0,0,91")]  // Decreasing of blue
    [InlineData(EasingFunction.EaseOutBounce,   new byte[] {   0,   0,   0,   0 },  new byte[] { 255,   0,   0,   0 },      1500,           "clr:255,0,0,0")]   // Increasing of alpha
    [InlineData(EasingFunction.EaseOutBounce,   new byte[] { 255,   0,   0,   0 },  new byte[] {   0,   0,   0,   0 },       800,           "clr:15,0,0,0")]    // Decreasing of alpha
    [InlineData(EasingFunction.EaseOutBounce,   new byte[] { 255,   0,   0,   0 },  new byte[] { 255, 255,   0,   0 },      1500,           "clr:255,255,0,0")] // Increasing of red
    [InlineData(EasingFunction.EaseOutBounce,   new byte[] { 255, 255,   0,   0 },  new byte[] { 255,   0,   0,   0 },       800,           "clr:255,15,0,0")]  // Decreasing of red
    [InlineData(EasingFunction.EaseOutBounce,   new byte[] { 255,   0,   0,   0 },  new byte[] { 255,   0, 255,   0 },      1500,           "clr:255,0,255,0")] // Increasing of green
    [InlineData(EasingFunction.EaseOutBounce,   new byte[] { 255,   0, 255,   0 },  new byte[] { 255,   0,   0,   0 },       800,           "clr:255,0,15,0")]  // Decreasing of green
    [InlineData(EasingFunction.EaseOutBounce,   new byte[] { 255,   0,   0,   0 },  new byte[] { 255,   0,   0, 255 },      1500,           "clr:255,0,0,255")] // Increasing of blue
    [InlineData(EasingFunction.EaseOutBounce,   new byte[] { 255,   0,   0, 255 },  new byte[] { 255,   0,   0,   0 },       800,           "clr:255,0,0,15")]  // Decreasing of blue
    public void Update_WhenInvoked_CorrectlySetsValueProperty(EasingFunction easingFunction, byte[] startComponents, byte[] stopComponents, int timeElapsed, string expected)
    {
        // Arrange
        var settings = new ColorTransitionBehaviorSettings
        {
            LifeTime = 1000,
            StartColor = new ParticleColor(startComponents[0], startComponents[1], startComponents[2], startComponents[3]),
            StopColor = new ParticleColor(stopComponents[0], stopComponents[1], stopComponents[2], stopComponents[3]),
            EasingFunctionType = easingFunction,
        };
        var behavior = new ColorTransitionBehavior(settings);

        // Act
        behavior.Update(new TimeSpan(0, 0, 0, 0, timeElapsed));

        // Assert
        Assert.Equal(expected, behavior.Value);
    }

    [Fact]
    public void Reset_WhenInvoked_ProperlyResetsBehavior()
    {
        // Arrange
        var settings = new ColorTransitionBehaviorSettings
        {
            LifeTime = 1000,
            StartColor = new ParticleColor(255, 62, 125, 200),
            StopColor = new ParticleColor(255, 62, 125, 200),
        };
        var behavior = new ColorTransitionBehavior(settings);

        // Act
        // NOTE: The elapsed time must be greater than the lifetime
        // of the behavior to properly test the behavior 'Enabled' property
        behavior.Update(new TimeSpan(0, 0, 0, 0, 1500));
        behavior.Reset();

        // Assert
        Assert.Equal(string.Empty, behavior.Value);
        Assert.Equal(0, behavior.ElapsedTime);
        Assert.True(behavior.Enabled);
    }
    #endregion
}