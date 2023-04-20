using AutoMapper;
using trimlink.api.Contracts;
using trimlink.data.Models;

namespace trimlink.api.Configuration;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<LinkCreate, Link>();

    }
}