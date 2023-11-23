using AutoMapper;
using BackendAccess.BackendEntities;
using BusinessLogic.Entities.BackendAccess;

namespace AuthoringTool.Mapping;

public class ApiResponseEntityMappingProfile : Profile
{
    private ApiResponseEntityMappingProfile()
    {
        CreateMap<UserTokenBE, UserToken>()
            .ForMember(m => m.Token, opt => opt
                .MapFrom(src => src.LmsToken));

        CreateMap<UserInformationBE, UserInformation>()
            .ForMember(m => m.LmsEmail, opt => opt
                .MapFrom(src => src.UserEmail))
            .ForMember(m => m.LmsId, opt => opt
                .MapFrom(src => src.UserId))
            .ForMember(m => m.LmsUsername, opt => opt
                .MapFrom(src => src.LmsUserName))
            .ForMember(m => m.IsLmsAdmin, opt => opt
                .MapFrom(src => src.IsAdmin));
        CreateMap<UploadResponseBE, UploadResponse>();

        CreateMap<LmsWorldBE, LmsWorld>();
    }

    public static Action<IMapperConfigurationExpression> Configure => cfg =>
    {
        cfg.AddProfile(new ApiResponseEntityMappingProfile());
        cfg.DisableConstructorMapping();
    };
}