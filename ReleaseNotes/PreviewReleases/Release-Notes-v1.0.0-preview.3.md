<h1 align="center" style="color: mediumseagreen;font-weight: bold;">
Plazma Preview Release Notes - v1.0.0-preview.3
</h1>

<h2 align="center" style="font-weight: bold;">Quick Reminder</h2>

<div align="center">

As with all software, there is always a chance for issues and bugs, especially for preview releases, so your input is greatly appreciated. 🙏🏼
</div>

<h2 align="center" style="font-weight: bold;">Enhancements ✨</h2>

1. [#80](https://github.com/KinsonDigital/Plazma/issues/80) - Improved various areas of the engine from ~10% to ~30%.
2. [#46](https://github.com/KinsonDigital/Plazma/issues/46) - Improved randomization by removing the `RNGCryptoServiceProvider` method deprecated by **Microsoft**. The new `RandomNumberGenerator` is easier to use, performs better, and is recommended way forward by **Microsoft**. Check out the resources below for more information.
   - [RNGCryptoServiceProvider is obsolete](https://learn.microsoft.com/en-us/dotnet/fundamentals/syslib-diagnostics/syslib0023)
   - [RandomNumberGenerator Class](https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.randomnumbergenerator?view=net-8.0)

<h2 align="center" style="font-weight: bold;">Breaking Changes 🧨</h2>

1. [#46](https://github.com/KinsonDigital/Plazma/issues/46) - Introduced the following breaking changes:
   - Removed the `PseudoRandomizerService` from the code base.
   - Changed the name of the `TrueRandomizerService` to `RandomizerService`
   - Removed the `IDisposable` interface from the `IRandomizerService` interface and its implementations from the rest of the code base.
2. [#80](https://github.com/KinsonDigital/Plazma/issues/80) - Introduced the following breaking changes:
   - Changed the setter of the `Behavior.ElapsedTime` property from `protected` to `private`.
   - Changed the setter of the `Behavior.LifeTime` property from `public` to `protected`.
   - Removed the `IParticle` interface.
   - Changed the `Particle` class to a `readonly record struct`.
   - Changed the `ParticlePool.Particles` property type from `ImmutableArray<IParticle>` to `ImmutableArray<Particle>`.
   - Changed the `IParticlePool.Particles` property type from `ImmutableArray<IParticle>` to `ImmutableArray<Particle>`.
   - Changed the `IParticleFactory.Create` method return type from `IParticle` to `Particle`.
   - Changed the `Particle.Behaviors` property type from `ImmutableArray<BehaviorAttribute>` to `List<BehaviorAttribute>`
   - Removed the `Update()` method from the `Particle` class.
   - Removed the `Reset()` method from the `Particle` class.
   - Data type for the following rate properties of the `ParticleEffect` class changed from `int` to `float`
     - `SpawnRateMin`
     - `SpawnRateMax`
     - `BurstSpawnRateMin`
     - `BurstSpawnRateMax`
     - `BurstOnMilliseconds`
     - `BurstOffMilliseconds`
   - Changed the `ParticleEffect` class to a `readonly record struct`. Doing this enforced **ALL** of the property setters to be `init` properties.
   - Changed the return and parameter types of the `EasingFunctions.EaseOutBounce()` method from `double` to `float`
   - Changed the return and parameter types of the `EasingFunctions.EaseInQuad()` method from `double` to `float`
   - Changed the following `IBehavior` interface and `Behavior` class types for the following properties from `double` to `float`
      - `Value`
      - `ElapsedTime`
      - `LifeTime`
   - Changed the `EasingRandomBehaviorSettings.UpdateValue` property type from `Func<double, double>` to `Func<float, float>`
   - Changed the `EasingRandomBehaviorSettings.UpdateRandomStartMin` property type from `Func<double, float>` to `Func<float, float>`
   - Changed the `EasingRandomBehaviorSettings.UpdateRandomStartMax` property type from `Func<double, float>` to `Func<float, float>`
   - Changed the following `EasingRandomBehavior` properties from `double` to `float`
     - `Start`
     - `Change`
   - Removed the `AddBehavior()` and `RemoveBehavior()` method from the `Particle` class.
     - Behaviors are now added directly using the `Behaviors` property.
   - Changed the `Particle.Behaviors` property from non-nullable to nullable.
   - Added the `ObjectDisposedException` to the following `ParticlePool` members.
     - `TotalLivingParticles`
     - `TotalDeadParticles`
     - `Particles`
     - `TextureLoaded`
     - `Update()`
     - `LoadTexture()`
     - `AddBehavior()`
     - `RemoveBehavior()`

<h2 align="center" style="font-weight: bold;">Dependency Updates 📦</h2>

1. [#81](https://github.com/KinsonDigital/Plazma/pull/81) - Updated project to _**dotnet**_ to _**v8.0.0**_
2. [#79](https://github.com/KinsonDigital/Plazma/pull/79) - Updated dependency _**simpleinjector**_ to _**v5.4.6**_
3. [#78](https://github.com/KinsonDigital/Plazma/pull/78) - Updated dependency _**fluentassertions.analyzers**_ to _**v0.32.0**_
4.  [#76](https://github.com/KinsonDigital/Plazma/pull/76) - Updated dependency _**xunit-dotnet mono repo**_ to _**v2.8.1**_
5.  [#74](https://github.com/KinsonDigital/Plazma/pull/74) - Updated dependency _**microsoft.net.test.sdk**_ to _**v17.10.0**_
6.  [#71](https://github.com/KinsonDigital/Plazma/pull/71) - Updated dependency _**xunit**_ to _**v2.6.6**_
7.  [#81](https://github.com/KinsonDigital/Plazma/pull/81) - Updated dependency _**kinsondigital.velaptor**_ to _**v1.0.0-preview.36**_
8.  [#66](https://github.com/KinsonDigital/Plazma/pull/66) - Updated _**kinsondigital/infrastructure**_ action to _**v13.6.3**_
9.  [#77](https://github.com/KinsonDigital/Plazma/pull/77) - Updated dependency _**coverlet.msbuild**_ to _**v6.0.2**_
10. [#75](https://github.com/KinsonDigital/Plazma/pull/75) - Updated dependency _**nsubstitute.analyzers.csharp**_ to _**v1.0.17**_
11. [#59](https://github.com/KinsonDigital/Plazma/pull/59) - Updated dependency _**microsoft.codeanalysis.netanalyzers**_ to _**v8.0.0**_

<h2 align="center" style="font-weight: bold;">Other 🪧</h2>

1. [#50](https://github.com/KinsonDigital/Plazma/issues/50) - Update sync workflow.
