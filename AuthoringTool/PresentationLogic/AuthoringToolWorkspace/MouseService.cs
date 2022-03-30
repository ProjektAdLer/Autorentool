using Microsoft.AspNetCore.Components.Web;

namespace AuthoringTool.PresentationLogic.AuthoringToolWorkspace;

public class MouseService : IMouseService
{
    public event EventHandler<MouseEventArgs> OnMove;
    public event EventHandler<MouseEventArgs> OnUp;
    public event EventHandler<MouseEventArgs> OnOut;

    public void FireMove(object obj, MouseEventArgs evt) => OnMove?.Invoke(obj, evt);
    public void FireUp(object obj, MouseEventArgs evt) => OnUp?.Invoke(obj, evt);
    public void FireOut(object obj, MouseEventArgs evt) => OnOut?.Invoke(obj, evt);
}
