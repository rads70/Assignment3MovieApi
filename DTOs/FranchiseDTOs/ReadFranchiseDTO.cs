using Assignment3MovieApi.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Assignment3MovieApi.DTOs.FranchiseDTOs
{
    public class ReadFranchiseDTO
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public int[] Movies { get; set; }
    }
}
