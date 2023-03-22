using ApiAccess.ApiResponses;
using AutoMapper;
using BusinessLogic.Entities.ApiElements;

namespace AuthoringTool.Mapping;

public class ApiResponseEntityMappingProfile : Profile
{
    private ApiResponseEntityMappingProfile()
    {
        CreateMap<UserTokenWebApiResponse, UserToken>();
    }

    public static Action<IMapperConfigurationExpression> Configure => cfg =>
    {
        cfg.AddProfile(new ApiResponseEntityMappingProfile());
    };
}