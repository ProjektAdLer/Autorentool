using Microsoft.AspNetCore.Components.Web;

namespace AuthoringToolLib.PresentationLogic.AuthoringToolWorkspace;

public interface IMouseService
{
    event EventHandler<MouseEventArgs> OnMove;
    event EventHandler<MouseEventArgs> OnUp;
    event EventHandler<MouseEventArgs> OnOut;
    void FireMove(object obj, MouseEventArgs evt);
    void FireUp(object obj, MouseEventArgs evt);
    void FireOut(object obj, MouseEventArgs evt);
}
