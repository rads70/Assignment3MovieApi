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

        public int? FranchiseId { get; set; }
        public Franchise Franchise { get; set; }

        public ICollection<Character> Characters { get; set; }
    }
}
