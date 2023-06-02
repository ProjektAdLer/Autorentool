using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Presentation;

//Define 'onmouseleave' event handler which doesn't bubble up to parents which we need for svg drag and drop
[EventHandler("onmouseenter", typeof(MouseEventArgs), true, true)]
public static class EventHandlers
{
}