using BackTest.Mappers;
using BackTest.Models;
using BackTestUnitTests.Tests.Data;
using FluentAssertions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace BackTestUnitTests.Tests.MappersTests
{
    public class MapperPersonTests
    {
        [Theory]
        [ClassData(typeof(PersonData))]
        public async Task MapPerson_ReturnPersonDto(Person person)
        {
            var personDtoExpected = new PersonDto
            {
                Id = 1,
                Name = "Egor",
                DisplayName = "duevi",
                SkillsPerson = new Dictionary<string, byte> { {"c#", 3 }, {"c++", 2} }
            };

            var personDtoActual = MapperPerson.MapPerson(person);

            personDtoActual.Should().BeEquivalentTo(personDtoExpected);
        }

        [Theory]
        [ClassData(typeof(ListPersonData))]
        public async Task MapListPersons_ReturnListPersonsDto(List<Person> persons)
        {
            var personsDtoExpected = new List<PersonDto>
            {
                new PersonDto
                {
                    Id = 1,
                    Name = "Egor",
                    DisplayName = "duevi",
                    SkillsPerson = new Dictionary<string, byte> { { "c#", 3 }, { "c++", 3 } }
                },
                new PersonDto
                {
                    Id = 2,
                    Name = "Egor2",
                    DisplayName = "duevi2",
                    SkillsPerson = new Dictionary<string, byte> { { "go", 2 }, { "python", 3 } }
                }
            };

            var personsDtoActual = MapperPerson.MapListPersons(persons);

            personsDtoActual.Should().BeEquivalentTo(personsDtoExpected);
        }
    }
}
