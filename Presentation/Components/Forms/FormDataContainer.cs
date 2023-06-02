using AutoMapper;

namespace Presentation.Components.Forms;

public class FormDataContainer<TForm, TEntity> : IFormDataContainer<TForm, TEntity> where TForm : new()
{
    public FormDataContainer(IMapper mapper)
    {
        FormModel = new TForm();
        Mapper = mapper;
    }

    public TForm FormModel { get; set; }
    private IMapper Mapper { get; }

    public TEntity GetMappedEntity() => Mapper.Map<TForm, TEntity>(FormModel);
}