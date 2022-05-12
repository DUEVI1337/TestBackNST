using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackTest.Models
{
    public class Person
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string DisplayName { get; set; }

        [InverseProperty("Person")]
        public virtual ICollection<PersonSkills> PersonSkills { get; set; }
    }
}
