﻿using Assignment3MovieApi.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Assignment3MovieApi.DTOs.MovieDTOs
{
    public class CreateMovieDTO
    {
       
        [Required]
        [MaxLength(100)]
        public string MovieTitle { get; set; }

        [MaxLength(255)]
        public string Genre { get; set; }

        public int ReleaseYear { get; set; }

        [MaxLength(50)]
        public string Director { get; set; }

        [MaxLength(255)]
        public string Picture { get; set; }

        [MaxLength(255)]
        public string Trailer { get; set; }

    }
}
