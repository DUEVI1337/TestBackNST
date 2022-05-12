using System.ComponentModel.DataAnnotations;

namespace BackTest.Models
{
    public class UpdatePersonDto
    {
        [Required]
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public Dictionary<string, byte> PersonSkills { get; set; }
    }
}
