using BackTest.Models;

namespace BackTest.Mappers
{
    /// <summary>
    /// 
    /// </summary>
    public class Mapper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public static PersonDto MapperPerson(Person person)
        {
            var personDto = new PersonDto()
            {
                Name = person.Name,
                DisplayName = person.DisplayName,
                SkillsPerson = person.PersonSkills.ToDictionary(x => x.SkillName, x => x.Level),
                Id = person.Id
            };
            return personDto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="persons"></param>
        /// <returns></returns>
        public static List<PersonDto> MapperListPersons(List<Person> persons)
        {
            var personsDto = new List<PersonDto>(persons.Select(x => new PersonDto
            {
                Name = x.Name,
                DisplayName = x.DisplayName,
                SkillsPerson = x.PersonSkills.ToDictionary(x => x.SkillName, x => x.Level),
                Id = x.Id
            }).ToList());
            return personsDto;
        }
    }
}
