using BackTest.Controllers;
using BackTest.Data;
using BackTest.Data.Repository;
using BackTest.Data.Repository.Interfaces;
using BackTest.Models;
using BackTest.Services;
using BackTest.Services.Interface;
using BackTestUnitTests.Tests.Data;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BackTestIntegrationTests.Tests.ControllersTests
{
    public class PersonControllerTests
    {
        private PersonController _personController;
        private WebApplicationFactory<Program> _app;
        private DataContext _db;
        public PersonControllerTests()
        {
            _app = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(d => d.ServiceType ==
                                                              typeof(DbContextOptions<DataContext>));
                    services.Remove(descriptor);
                    services.AddDbContext<DataContext>(opt =>
                    {
                        opt.UseInMemoryDatabase("test");
                    });
                });
            });

            _db = _app.Services.CreateScope().ServiceProvider.GetService<DataContext>();
            IPersonRepository _repoPerson = new PersonRepository(_db);
            IPersonSkillsRepository _repoPersonSkills = new PersonSkillsRepository(_db);
            ISkillRepository _repoSkill = new SkillRepository(_db);
            IPersonService _personService = new PersonService(_repoPerson);
            IPersonSkillsService _personSkillsService = new PersonSkillsService(_repoPersonSkills);
            ISkillService _skillService = new SkillService(_repoSkill);
            _personController = new PersonController(_personService, _personSkillsService, _skillService);
        }

        [Fact]
        public async Task GetAllPersonsAsync_ReturnCountPersons_2()
        {
            var personsInit = new List<Person>()
            {
                new Person() { Id = 1, Name = "Egor", DisplayName="Duvanov"},
                new Person() { Id = 2, Name = "Egor2", DisplayName="Duvanov2"},
            };
            await _db.Persons.AddRangeAsync(personsInit);
            await _db.SaveChangesAsync();

            var response = (OkObjectResult)await _personController.GetAllPersonsAsync();

            var personsActual = (List<PersonDto>)response.Value;
            Assert.Equal(personsInit.Count, personsActual.Count());
            _db.Database.EnsureDeleted();
        }

        [Theory]
        [InlineData(1)]
        public async Task GetPersonAsync_ReturnFindedPerson(long id)
        {
            var personSkillsInit = new List<PersonSkills>()
            {
                new PersonSkills() {PersonId = id, SkillName="c#", Level = 2 },
                new PersonSkills() {PersonId = id, SkillName="c++", Level = 1 }
            };
            var personInit = new Person() { Id = id, Name = "Egor", DisplayName = "Duvanov", PersonSkills = personSkillsInit };
            var personExpected = new PersonDto() { Id = id, Name = "Egor", DisplayName = "Duvanov", SkillsPerson = new Dictionary<string, byte> { { "c#", 2 }, { "c++", 1 } } };
            await _db.Persons.AddAsync(personInit);
            await _db.SaveChangesAsync();

            var response = (OkObjectResult)await _personController.GetPersonAsync(id);

            var personActual = (PersonDto)response.Value;
            personActual.Should().BeEquivalentTo(personExpected);
            _db.Database.EnsureDeleted();
        }
    }
}
