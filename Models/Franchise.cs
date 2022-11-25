using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Assignment3MovieApi.Models
{
    public class Franchise
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public ICollection<Movie> Movies { get; set; }
    }
}
