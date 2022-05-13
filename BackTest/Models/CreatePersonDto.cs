using System.ComponentModel.DataAnnotations;

namespace BackTest.Models
{
    public class CreatePersonDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string DisplayName { get; set; }
        [Required]
        public Dictionary<string, byte> PersonSkills { get; set; }
    }
}
