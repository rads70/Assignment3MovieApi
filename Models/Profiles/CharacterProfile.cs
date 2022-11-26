using Assignment3MovieApi.DTOs.CharacterDTOs;
using AutoMapper;
using System.Linq;

namespace Assignment3MovieApi.Models.Profiles
{
    public class CharacterProfile : Profile
    {
        public CharacterProfile()
        {
            CreateMap<Character, CharacterReadDTO>()
                .ForMember(charDto => charDto.Movies, opt => opt
                .MapFrom(ch => ch.Movies.Select(mo => mo.Id).ToArray()));

            CreateMap<CharacterCreateDTO, Character>();
            CreateMap<CharacterUpdateDTO, Character>(); 
           
        }
    }
}
