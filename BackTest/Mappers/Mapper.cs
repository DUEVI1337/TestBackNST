using BackTest.Models;

namespace BackTest.Mappers
{
    /// <summary>
    /// This class contains mappers for models
    /// </summary>
    public class Mapper
    {
        /// <summary>
        /// Mapping object <seealso cref="Person"/> in <seealso cref="PersonDto"/>
        /// </summary>
        /// <param name="person"></param>
        /// <returns>Object <seealso cref="PersonDto"/></returns>
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
        /// Mapping List objects <see cref="Person"/> in list objects <see cref="PersonDto"/>
        /// </summary>
        /// <param name="persons"></param>
        /// <returns>List objects <see cref="PersonDto"/></returns>
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
