using AutoMapper;
using trimlink.website.Contracts;
using trimlink.core.Records;

namespace trimlink.website.Configuration;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<LinkDetails, LinkGetDto>();
    }
}