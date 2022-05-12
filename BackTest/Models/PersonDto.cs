namespace BackTest.Models
{
    public class PersonDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public Dictionary<string, byte> SkillsPerson { get; set; }
    }
}
