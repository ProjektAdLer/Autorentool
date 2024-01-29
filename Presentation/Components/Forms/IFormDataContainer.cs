namespace Presentation.Components.Forms;

public interface IFormDataContainer<TForm, out TEntity>
{
    TForm FormModel { get; set; }
    TEntity GetMappedEntity();
    void Reset();
}