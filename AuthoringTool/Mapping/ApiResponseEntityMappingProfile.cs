using ApiAccess.BackendEntities;
using AutoMapper;
using BusinessLogic.Entities.ApiElements;

namespace AuthoringTool.Mapping;

public class ApiResponseEntityMappingProfile : Profile
{
    private ApiResponseEntityMappingProfile()
    {
        CreateMap<UserTokenWebApiBE, UserToken>();
    }

    public static Action<IMapperConfigurationExpression> Configure => cfg =>
    {
        cfg.AddProfile(new ApiResponseEntityMappingProfile());
    };
}