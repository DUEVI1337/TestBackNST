using BackTest.Data;
using BackTest.Data.Repository.Interfaces;
using BackTest.Models;
using BackTest.Services;
using BackTestUnitTests.Tests.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace BackTestUnitTests.Tests.ServicesTests
{
    public class PersonServiceTests
    {
        private readonly PersonService _personService;
        private readonly Mock<IPersonRepository> _repoPerson;

        public PersonServiceTests()
        {
            _repoPerson = new Mock<IPersonRepository>();
            _personService = new PersonService(_repoPerson.Object);
        }

        [Fact]
        public async Task GetAllPersonsAsync_ReturnListPersonDto()
        {
            //Arrange
            var personsInit = new List<Person>
            {
                new Person { Id= 1, Name = "Egor", DisplayName = "duevi", PersonSkills = new List<PersonSkills>
                {
                    new PersonSkills { PersonId = 1, Level = 1, SkillName = "sdf"}
                }}
            };
            var personsDtoExpected = new List<PersonDto>
            {
                new PersonDto { Id = 1, Name = "Egor", DisplayName = "duevi", SkillsPerson = new Dictionary<string,byte>{ { "sdf", 1 } } },
            };
            _repoPerson.Setup(x => x.GetAllPersonsAsync()).ReturnsAsync(personsInit);

            //Act
            var personsDtoActual = await _personService.GetAllPersonsAsync();

            //Assert
            personsDtoExpected.Should().BeEquivalentTo(personsDtoActual);
        }

        [Theory]
        [InlineData(1)]
        public async Task GetPersonAsync_ReturnFindPersonDto(long idPerson)
        {
            //Arrange
            var personInit = new Person
            {
                Id = idPerson,
                Name = "Egor",
                DisplayName = "duevi",
                PersonSkills = new List<PersonSkills>
                {
                    new PersonSkills { PersonId = 1, Level = 1, SkillName = "sdf" }
                }
            };
            var personDtoExpected = new PersonDto
            {
                Id = idPerson,
                Name = "Egor",
                DisplayName = "duevi",
                SkillsPerson = new Dictionary<string, byte> { { "sdf", 1 } }
            };
            _repoPerson.Setup(x => x.GetPersonByIdAsync(idPerson)).ReturnsAsync(personInit);

            //Act
            var personDtoActual = await _personService.GetPersonAsync(idPerson);

            //Assert
            personDtoActual.Should().BeEquivalentTo(personDtoExpected);
        }

        [Theory]
        [InlineData(2)]
        public async Task GetPersonAsync_ReturnNotFindPersonDto(long idPerson)
        {
            //Arrange

            //Act
            var personDtoActual = await _personService.GetPersonAsync(idPerson);

            //Assert
            Assert.Null(personDtoActual);
        }

        [Theory]
        [ClassData(typeof(CreatePersonDtoData))]
        public async Task CreatePersonAsync_ReturnCreatePerson(CreatePersonDto model)
        {
            //Arrange
            var personExpected = new Person()
            {
                Name="Egor",
                DisplayName="duevi"
            };
            //Act
            var personActual = await _personService.CreatePersonAsync(model);
            //Assert
            personActual.Should().BeEquivalentTo(personExpected);
        }

        [Theory]
        [ClassData(typeof(UpdatePersonDtoData))]
        public async Task UpdatePersonAsync_SuccessResult_PersonUpdated(UpdatePersonDto model, long idPerson)
        {
            //Arrange
            var personInit = new Person
            {
                Id = 1,
                Name = "Egor",
                DisplayName = "duevi",
                PersonSkills = new List<PersonSkills>
                {
                    new PersonSkills { PersonId = 1, Level = 1, SkillName = "sdf" }
                }
            };
            _repoPerson.Setup(x => x.GetPersonByIdAsync(idPerson)).ReturnsAsync(personInit);
            //Act
            var result = await _personService.UpdatePersonAsync(model, idPerson);

            //Assert
            Assert.True(result);
        }

        [Theory]
        [ClassData(typeof(UpdatePersonDtoData))]
        public async Task UpdatePersonAsync_UnsuccessResult_PersonNotFind(UpdatePersonDto model, long idPerson)
        {
            //Arrange

            //Act
            var result = await _personService.UpdatePersonAsync(model, idPerson);

            //Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(1)]
        public async Task DeletePersonAsync_ReturnSuccessResult(long idPerson)
        {
            //Arrange
            var personInit = new Person
            {
                Id = 1,
                Name = "Egor",
                DisplayName = "duevi",
                PersonSkills = new List<PersonSkills>
                {
                    new PersonSkills { PersonId = 1, Level = 1, SkillName = "sdf" }
                }
            };
            _repoPerson.Setup(x => x.GetPersonByIdAsync(idPerson)).ReturnsAsync(personInit);

            //Act
            var resukt = await _personService.DeletePersonAsync(idPerson);

            //Assert
            Assert.True(resukt);
        }

        [Theory]
        [InlineData(2)]
        public async Task DeletePersonAsync_ReturnUnsuccessResult_PersonNotFound(long idPerson)
        {
            //Arrange

            //Act
            var resukt = await _personService.DeletePersonAsync(idPerson);

            //Assert
            Assert.False(resukt);
        }
    }
}
