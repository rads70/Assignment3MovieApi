using System.Linq;
using System.Runtime.InteropServices;
using Assignment3MovieApi.DTOs.MovieDTOs;
using AutoMapper;

namespace Assignment3MovieApi.Models.Profiles
{
    public class MovieProfile : Profile
    {
        public MovieProfile()
        {
            CreateMap<Movie, MovieReadDTO>()
                .ForMember(moDto => moDto.Characters, opt => opt
                .MapFrom(mo => mo.Characters.Select(ch => ch.Id).ToArray()));

            CreateMap<MovieCreateDTO, Movie>();

            CreateMap<MovieUpdateDTO, Movie>();

            CreateMap<Character, MovieCharacterReadDTO>();
        }
    }
}
