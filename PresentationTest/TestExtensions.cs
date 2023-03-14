using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp.Dom;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using NUnit.Framework;

namespace PresentationTest;

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

    public static IElement FindOrFail(this IRenderedFragment fragment, string selector)
    {
        IElement? element = null;
        Assert.That(() => element = fragment.Find(selector), Throws.Nothing);
        if (element == null)
        {
            Assert.Fail("Couldn't find element with selector: {0}", selector);
        }
        return element!;
    }
    
    /// <summary>
    /// Will either find all elements matching the selector on the component, or throw an assert failure
    /// </summary>
    /// <param name="component">The component on which the element shall be searched for.</param>
    /// <param name="selector">The selector which shall be found.</param>
    /// <typeparam name="T">The type of component.</typeparam>
    /// <returns>All elements found for <paramref name="selector"/> on <paramref name="component"/></returns>
    public static IEnumerable<IElement> FindAllOrFail<T>(this IRenderedComponent<T> component, string selector) where T : IComponent
    {
        IEnumerable<IElement>? elements = null;
        Assert.That(() => elements = component.FindAll(selector), Throws.Nothing);
        if (elements == null)
        {
            Assert.Fail("Couldn't find elements with selector: {0}", selector);
        }
        return elements!;
    }

    /// <summary>
    /// Will either find the first component of type <typeparamref name="T"/> on the fragment, or throw an assert failure.
    /// </summary>
    /// <param name="fragment">The fragment on which the component shall be searched for.</param>
    /// <typeparam name="T">The type of component being searched for.</typeparam>
    /// <returns>The first component of type <typeparamref name="T"/> in <paramref name="fragment"/>.</returns>
    public static IRenderedComponent<T> FindComponentOrFail<T>(this IRenderedFragment fragment) where T : IComponent
    {
        IRenderedComponent<T>? component = null;
        Assert.That(() => component = fragment.FindComponent<T>(), Throws.Nothing);
        return component!;
    }

    /// <summary>
    /// Will either find all components of type <typeparam name="T"> on the fragment, or throw an assert failure.</typeparam>
    /// </summary>
    /// <param name="fragment">The fragment on which the component shall be searched for.</param>
    /// <typeparam name="T">The type of component being searched for.</typeparam>
    /// <returns>All components of type <typeparamref name="T"/> in <paramref name="fragment"/></returns>
    public static IEnumerable<IRenderedComponent<T>> FindComponentsOrFail<T>(this IRenderedFragment fragment)
        where T : IComponent
    {
        IEnumerable<IRenderedComponent<T>>? components = null;
        Assert.That(() => components = fragment.FindComponents<T>(), Throws.Nothing);
        return components!;

    }

    /// <summary>
    /// Will either find a component of type <typeparamref name="T"/> on the fragment, which contains the provided <paramref name="markup"/>,
    /// or throw an assert failure.
    /// </summary>
    /// <param name="fragment">The fragment on which the component shall be searched for.</param>
    /// <param name="markup">The markup that must be matched.</param>
    /// <typeparam name="T">The type of component being searched for.</typeparam>
    /// <returns>A component of type <typeparamref name="T"/> in <paramref name="fragment"/> which contains the provided <paramref name="markup"/></returns>
    public static IRenderedComponent<T> FindComponentWithMarkup<T>(this IRenderedFragment fragment, string markup)
        where T : IComponent =>
        fragment.FindComponentsWithMarkup<T>(markup).First();
    
    /// <summary>
    /// Will either find all components of type <typeparamref name="T"/> on the fragment, which contain the provided <paramref name="markup"/>,
    /// or throw an assert failure.
    /// </summary>
    /// <param name="fragment">The fragment on which the component shall be searched for.</param>
    /// <param name="markup">The markup that must be matched.</param>
    /// <typeparam name="T">The type of component being searched for.</typeparam>
    /// <returns>A components of type <typeparamref name="T"/> in <paramref name="fragment"/> which contain the provided <paramref name="markup"/></returns>
    public static IEnumerable<IRenderedComponent<T>> FindComponentsWithMarkup<T>(this IRenderedFragment fragment, string markup)
        where T : IComponent =>
        fragment.FindComponentsOrFail<T>().Where(cmp => cmp.Markup.Contains(markup));

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