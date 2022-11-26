using System.ComponentModel.DataAnnotations;

namespace Assignment3MovieApi.DTOs.CharacterDTOs
{
    public class CharacterCreateDTO
    {
        [Required]
        [MaxLength(50)]
        public string FullName { get; set; }

        [MaxLength(50)]
        public string Alias { get; set; }

        [MaxLength(10)]
        public string Gender { get; set; }

        [MaxLength(255)]
        public string Picture { get; set; }
    }
}
