using System;
using Microsoft.AspNetCore.Components.Web;

namespace AuthoringTool.BusinessLogic;

public interface IMouseService
{
    event EventHandler<MouseEventArgs> OnMove;
    event EventHandler<MouseEventArgs> OnUp;
    event EventHandler<MouseEventArgs> OnOut;
}
