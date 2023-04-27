namespace Presentation.PresentationLogic;

/// <summary>
/// Delegate type for asynchronous event handlers.
/// </summary>
public delegate Task AsyncEventHandler(object? sender);

/// <summary>
/// Delegate type for asynchronous event handlers.
/// </summary>
/// <typeparam name="TEventArgs">The type of event arguments.</typeparam>
public delegate Task AsyncEventHandler<in TEventArgs>(object? sender, TEventArgs e);