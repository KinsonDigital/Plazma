// <copyright file="EasingFunctions.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Plazma;

/// <summary>
/// Provides various easing functions.
/// </summary>
public static class EasingFunctions
{
    /// <summary>
    /// Ease out bounce easing function.
    /// </summary>
    /// <param name="t">The current time/step in milliseconds.</param>
    /// <param name="b">The starting value.</param>
    /// <param name="c">The amount of change. (end - start).</param>
    /// <param name="d">The total amount of time(milliseconds)/steps.</param>
    /// <returns>The result of the easing function.</returns>
    public static float EaseOutBounce(float t, float b, float c, float d)
    {
        t /= 1000.0f; // Convert to seconds
        d /= 1000.0f; // Convert to seconds

        if ((t /= d) < 0.36363636363636363636363636363636)
        {
            return (c * (7.5625f * t * t)) + b;
        }
        else if (t < 0.72727272727272727272727272727273)
        {
            return (c * ((7.5625f * (t -= 0.54545454545454545454545454545455f) * t) + 0.75f)) + b;
        }
        else if (t < 0.90909090909090909090909090909091)
        {
            return (c * ((7.5625f * (t -= 0.81818181818181818181818181818182f) * t) + 0.9375f)) + b;
        }
        else
        {
            return (c * ((7.5625f * (t -= 0.9f) * t) + 0.95454545454545454545454545454545f)) + b;
        }
    }

    /// <summary>
    /// Ease in quad easing function.
    /// </summary>
    /// <param name="t">The current time/step.</param>
    /// <param name="b">The starting value.</param>
    /// <param name="c">The amount of change. (end - start).</param>
    /// <param name="d">The total amount of time(milliseconds)/steps.</param>
    /// <returns>The result of the easing function.</returns>
    public static float EaseInQuad(float t, float b, float c, float d)
    {
        t /= 1000.0f; // Convert to seconds
        d /= 1000.0f; // Convert to seconds

        t /= d;

        return (c * t * t) + b;
    }
}
