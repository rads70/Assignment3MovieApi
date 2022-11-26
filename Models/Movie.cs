using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Assignment3MovieApi.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }

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

        // Foreign key Franchise set nullable
        public int? FranchiseId { get; set; }

        // Navigation property one to many Movies - Franchises
        public Franchise Franchise { get; set; }

        // Naviagtion property many to many realtionship to Characters
        public ICollection<Character> Characters { get; set; }
    }
}
