using BackTest.Models;

namespace BackTest.Mappers
{
    public class Mapper
    {
        public static PersonDto MapperPerson(Person person)
        {
            PersonDto personDto = new PersonDto()
            {
                Name = person.Name,
                DisplayName = person.DisplayName,
                SkillsPerson = person.PersonSkills.ToDictionary(x => x.SkillName, x => x.Level),
                Id = person.Id
            };
            return personDto;
        }

        public static List<PersonDto> MapperListPersons(List<Person> persons)
        {
            List<PersonDto> personsDto = new List<PersonDto>(persons.Select(x => new PersonDto
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
