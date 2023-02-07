namespace Presentation.Components.Forms;

public interface IBaseForm
{
    void Reset();
    Task SubmitAsync();
}