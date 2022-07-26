using System.Threading.Tasks;
using AngleSharp.Dom;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using NUnit.Framework;

namespace AuthoringToolTest;

/// <summary>
/// This class provides some shortcut extensions for testing stuff.
/// </summary>
public static class TestExtensions
{
    /// <summary>
    /// Will either find the first element matching the selector on the component, or throw an assert failure.
    /// </summary>
    /// <param name="component">The component on which the element shall be searched for.</param>
    /// <param name="selector">The selector which shall be found.</param>
    /// <typeparam name="T">The type of component.</typeparam>
    /// <returns>The first element found for <paramref name="selector"/> on <paramref name="component"/></returns>
    public static IElement FindOrFail<T>(this IRenderedComponent<T> component, string selector)
        where T : IComponent
    {
        IElement? element = null;
        Assert.That(() => element = component.Find(selector), Throws.Nothing);
        if (element == null)
        {
            Assert.Fail("Couldn't find element with selector: {0}", selector);
        }
        return element!;
    }

    /// <summary>
    /// Raises the <c>@onmouseleave</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
    /// to the event handler.
    /// </summary>
    /// <param name="element">The element to raise the event on.</param>
    /// <param name="eventArgs">The event arguments to pass to the event handler.</param>
    public static void MouseLeave(this IElement element, MouseEventArgs eventArgs) =>
        _ = MouseLeaveAsync(element, eventArgs);

    /// <summary>
    /// Raises the <c>@onmouseleave</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
    /// to the event handler.
    /// </summary>
    /// <param name="element">The element to raise the event on.</param>
    /// <param name="eventArgs">The event arguments to pass to the event handler.</param>
    /// <returns>A task that completes when the event handler is done.</returns>
    public static Task MouseLeaveAsync(this IElement element, MouseEventArgs eventArgs) =>
        element.TriggerEventAsync("onmouseleave", eventArgs);
}