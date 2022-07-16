// <copyright file="AssertExtensions.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Xunit.Sdk;

namespace GitHubReleaseCheckerTests.Helpers;

/// <summary>
/// Provides helper methods for the <see cref="Xunit"/>'s <see cref="Assert"/> class.
/// </summary>
[ExcludeFromCodeCoverage]
// ReSharper disable once ClassNeverInstantiated.Global
public class AssertExtensions : Assert
{
    private const string TableFlip = "(╯'□')╯︵┻━┻  ";

    /// <summary>
    /// Asserts that object properties set and get the same value without alteration.
    /// </summary>
    /// <param name="propName">The name of the <c>object</c> property.</param>
    /// <param name="value">The value to set the <c>object</c> property to.</param>
    /// <typeparam name="TObj">The type of <c>object</c> to test.</typeparam>
    /// <typeparam name="TValue">The type of value to set the property to.</typeparam>
    /// <exception cref="AssertActualExpectedException">
    /// Thrown for the following reasons:
    ///     <list type="bullet">
    ///         <item>The <paramref name="propName"/> parameter is null or empty.</item>
    ///         <item>The <paramref name="value"/> parameter is null or empty.</item>
    ///         <item>A property that matches the given <paramref name="propName"/> was not found.</item>
    ///     </list>
    /// </exception>
    /// <remarks>
    ///     Best for simplifying the test of an auto property.
    /// </remarks>
    public static void PropertyGetsAndSets<TObj, TValue>(string propName, TValue? value)
        where TObj : class, new()
    {
        if (string.IsNullOrEmpty(propName))
        {
            throw new AssertActualExpectedException(
                expected: $"Parameter '{nameof(propName)} not to be null or empty.",
                actual: "was null or empty.",
                $"{TableFlip} The parameter {nameof(propName)}' must not be null or empty to perform the assertion.");
        }

        if (value is null)
        {
            throw new AssertActualExpectedException(
                expected: $"Parameter '{nameof(value)} not to be null.",
                actual: "was null or empty.",
                $"{TableFlip} The parameter {nameof(value)}' must not be null to perform the assertion.");
        }

        var props = typeof(TObj).GetProperties();

        var foundProp = (from prop in props
            where prop.Name == propName
            select prop).FirstOrDefault();

        if (foundProp is null)
        {
            throw new AssertActualExpectedException(
                expected: $"Property '{propName}'.",
                actual: "null",
                $"{TableFlip} Property {propName}' for class '{typeof(TObj).Name} must exist to perform the assertion.");
        }

        var obj = new TObj();

        foundProp.SetValue(obj, value);

        var actual = foundProp.GetValue(obj);

        Equal(value, actual);
    }
}
