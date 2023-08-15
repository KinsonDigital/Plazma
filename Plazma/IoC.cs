// <copyright file="IoC.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Plazma;

using System.Diagnostics.CodeAnalysis;
using Factories;
using Services;
using SimpleInjector;

/// <summary>
/// Provides dependency injection for the application.
/// </summary>
[ExcludeFromCodeCoverage(Justification = "Used for non-testable dependency injection code.")]
internal static class IoC
{
    private static readonly Container IoCContainer = new ();
    private static bool isInitialized;

    /// <summary>
    /// Gets the inversion of control container used to get instances of objects.
    /// </summary>
    public static Container Container => isInitialized ? IoCContainer : SetupContainer();

    /// <summary>
    /// Sets up the IoC container.
    /// </summary>
    private static Container SetupContainer()
    {
        IoCContainer.Register<IRandomizerService, TrueRandomizerService>(Lifestyle.Singleton);
        IoCContainer.Register<IBehaviorFactory, BehaviorFactory>(Lifestyle.Singleton);
        IoCContainer.Register<IParticleFactory, ParticleFactory>(Lifestyle.Singleton);

        isInitialized = true;

        return IoCContainer;
    }
}
