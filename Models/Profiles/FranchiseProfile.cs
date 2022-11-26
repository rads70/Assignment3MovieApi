using Assignment3MovieApi.DTOs.FranchiseDTOs;
using AutoMapper;
using System.Linq;

namespace Assignment3MovieApi.Models.Profiles
{
    public class FranchiseProfile : Profile 
    {
        public FranchiseProfile()
        {
            CreateMap<Franchise, FranchiseReadDTO>()
                .ForMember(frDto => frDto.Movies, opt => opt
                .MapFrom(fr => fr.Movies.Select(moObj => moObj.Id).ToArray()));

            CreateMap<FranchiseCreateDTO, Franchise>();
            CreateMap<FranchiseUpdateDTO, Franchise>();
                
        }
    }
}
