using System.Runtime.InteropServices;
using Assignment3MovieApi.DTOs.MovieDTOs;
using AutoMapper;

namespace Assignment3MovieApi.Models.Profiles
{
    public class MovieProfile : Profile
    {
        public MovieProfile()
        {
            CreateMap<Movie, ReadMovieDTO>();
            CreateMap<CreateMovieDTO, Movie>();
            CreateMap<UpdateMovieDTO, Movie>(); 
        }
    }
}
