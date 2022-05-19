using BackTest.Data;
using BackTest.Models;
using BackTestUnitTests.Tests.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit;

[assembly: CollectionBehavior(CollectionBehavior.CollectionPerClass, DisableTestParallelization = true)]
namespace BackTestIntegrationTests.Tests.ControllersTests
{
    public class PersonControllerTests : IClassFixture<WebAppFactoryTest<Program>>
    {
        private readonly WebAppFactoryTest<Program> _factory;
        private readonly HttpClient _client;
        private DataContext _db;
        public PersonControllerTests(WebAppFactoryTest<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
            _db = _factory.Services.CreateScope().ServiceProvider.GetService<DataContext>();
            _db.Database.EnsureDeleted();
            _db.Database.EnsureCreated();
        }

        [Fact]
        public async Task GetAllPersonsAsync_ReturnCountPersons_2()
        {
            var personsInit = new List<Person>()
            {
                new Person() { Id = 1, Name = "Egor", DisplayName="duevi"},
                new Person() { Id = 2, Name = "Egor2", DisplayName="duevi2"},
            };
            await _db.Persons.AddRangeAsync(personsInit);
            await _db.SaveChangesAsync();

            var response = await _client.GetAsync("api/v1/persons");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var personsActual = await response.Content.ReadFromJsonAsync<List<Person>>();
            Assert.Equal(personsInit.Count, personsActual.Count());
        }

        [Fact]
        public async Task GetPersonAsync_ReturnFindedPerson()
        {
            var personSkillsInit = new List<PersonSkills>()
            {
                new PersonSkills() {PersonId = 1, SkillName="c#", Level = 2 },
                new PersonSkills() {PersonId = 1, SkillName="c++", Level = 1 }
            };
            var personInit = new Person() { Id = 1, Name = "Egor", DisplayName = "duevi", PersonSkills = personSkillsInit };
            var personExpected = new PersonDto()
            {
                Id = 1,
                Name = "Egor",
                DisplayName = "duevi",
                SkillsPerson = new Dictionary<string, byte> { { "c#", 2 }, { "c++", 1 } }
            };
            await _db.Persons.AddAsync(personInit);
            await _db.SaveChangesAsync();

            var person = await _db.Persons.FindAsync((long)1);

            var response = await _client.GetAsync("api/v1/person/1");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var personActual = await response.Content.ReadFromJsonAsync<PersonDto>();
            personActual.Should().BeEquivalentTo(personExpected);
        }

        [Theory]
        [ClassData(typeof(CreatePersonDtoData))]
        public async Task CreatePersonAsync_ReturnOkResult_CheckInDataBase(CreatePersonDto model)
        {
            var personExpected = new Person() { Id = 1, Name = "Egor", DisplayName = "duevi" };
            var personSkillsExpected = new List<PersonSkills>()
            {
                new PersonSkills() {PersonId = 1, SkillName="c#", Level = 3, Person = personExpected},
                new PersonSkills() {PersonId = 1, SkillName="c++", Level = 2, Person = personExpected}
            };

            var response = await _client.PostAsJsonAsync("api/v1/person", model);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var personActual = await _db.Persons.FindAsync(personExpected.Id);
            Assert.Equal(personActual.PersonSkills.Count(), personSkillsExpected.Count());
        }

        [Theory]
        [ClassData(typeof(UpdatePersonDtoData))]
        public async Task UpdatePersonAsync_ReturnOkResult_CheckInDatabase(UpdatePersonDto model, long id)
        {
            var personInit = new Person()
            {
                Id = 1,
                Name = "Egor",
                DisplayName = "duevi",
                PersonSkills = new List<PersonSkills>()
                {
                    new PersonSkills() { PersonId = 1, SkillName="c#", Level = 3},
                    new PersonSkills() { PersonId = 1, SkillName="c++", Level = 2},
                }
            };
            await _db.Persons.AddAsync(personInit);
            await _db.SaveChangesAsync();
            var personExpected = new Person()
            {
                Id = 1,
                Name = model.Name,
                DisplayName = model.DisplayName,
                PersonSkills = new List<PersonSkills>()
                {
                    new PersonSkills() {PersonId = 1, SkillName="c#", Level =2 },
                    new PersonSkills() {PersonId = 1, SkillName="c--", Level = 3 },
                    new PersonSkills() {PersonId = 1, SkillName="c##", Level = 4 }
                }
            };

            var response = await _client.PutAsJsonAsync("api/v1/person/1", model);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var personActual = await response.Content.ReadFromJsonAsync<PersonDto>();
            personActual.Name.Should().BeEquivalentTo(personExpected.Name);
            personActual.DisplayName.Should().BeEquivalentTo(personExpected.DisplayName);
            Assert.Equal(personActual.SkillsPerson.Count(), personExpected.PersonSkills.Count());
        }

        [Fact]
        public async Task DeletePersonAsync_ReturnOkResult_CheckInDatabase()
        {
            var person = new Person()
            {
                Id = 1,
                Name = "Egor",
                DisplayName = "duevi"
            };
            await _db.Persons.AddAsync(person);
            await _db.SaveChangesAsync();

            var response = await _client.DeleteAsync("api/v1/person/1");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var personActual = await _db.Persons.FirstOrDefaultAsync(x=>x.Id==1);
            Assert.Null(personActual);
        }
    }
}
