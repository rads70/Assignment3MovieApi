using Assignment3MovieApi.DTOs.FranchiseDTOs;
using AutoMapper;
using System.Linq;

namespace Assignment3MovieApi.Models.Profiles
{
    public class FranchiseProfile : Profile 
    {
        public FranchiseProfile()
        {
            CreateMap<Franchise, ReadFranchiseDTO>()
                .ForMember(frDto => frDto.Movies, opt => opt
                .MapFrom(fr => fr.Movies.Select(moObj => moObj.Id).ToArray()));
                
        }
    }
}
