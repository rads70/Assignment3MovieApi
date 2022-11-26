using System.ComponentModel.DataAnnotations;

namespace Assignment3MovieApi.DTOs.FranchiseDTOs
{
    public class FranchiseUpdateDTO
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }
    }
}
