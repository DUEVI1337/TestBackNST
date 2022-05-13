using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackTest.Models
{
    public class Skill
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Name { get; set; }

        [InverseProperty("Skill")]
        public virtual ICollection<PersonSkills> PersonSkills { get; set; }
    }
}
