namespace photoContainer.helpers;

public class AutoMapperProfiles : Profile
{

    public AutoMapperProfiles()
    {
        _ = CreateMap<data.models.Image, ImageDto>().ReverseMap().MaxDepth(32);



    }

}
