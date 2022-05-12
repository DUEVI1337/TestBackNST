using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackTest.Models
{
    public class PersonSkills
    {
        [ForeignKey("Person")]
        public long PersonId { get; set; }
        [InverseProperty("PersonSkills")]
        public virtual Person Person { get; set; }
        [ForeignKey("Skill")]
        public string SkillName { get; set; }
        [InverseProperty("PersonSkills")]
        public virtual Skill Skill { get; set; }
        [Required]
        public byte Level { get; set; }
    }
}
