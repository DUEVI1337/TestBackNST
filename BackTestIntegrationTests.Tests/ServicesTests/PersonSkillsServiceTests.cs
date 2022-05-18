using BackTest.Data;
using BackTest.Data.Repository;
using BackTest.Data.Repository.Interfaces;
using BackTest.Models;
using BackTest.Services;
using BackTest.Services.Interface;
using BackTestIntegrationTests.Tests.Data;
using BackTestUnitTests.Tests.Data;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BackTestIntegrationTests.Tests.ServicesTests
{
    public class PersonSkillsServiceTests
    {
        private readonly IPersonSkillsService _personSkillsService;
        private readonly IPersonSkillsRepository _repoPersonSkills;
        private readonly IPersonRepository _repoPerson;
        private WebApplicationFactory<Program> _app;
        private DataContext _db;

        public PersonSkillsServiceTests()
        {
            _app = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
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
            _repoPersonSkills = new PersonSkillsRepository(_db);
            _repoPerson = new PersonRepository(_db);
            _personSkillsService = new PersonSkillsService(_repoPersonSkills);
            _db.Database.EnsureDeleted();
            _db.Database.EnsureCreated();
        }


        [Theory]
        [ClassData(typeof(PersonSkillsData))]
        public async Task ChangePersonSkillsAsync_ReturnUpdatedPersonSkillsFromDatabase(Dictionary<string, byte> newPersonSkills, long idPerson)
        {
            var person = new Person()
            {
                Id = idPerson,
                Name = "Egor",
                DisplayName = "Duvanov"
            };
            var personSkillsInit = new List<PersonSkills>()
            {
                new PersonSkills
                {
                    Person = person,
                    PersonId = idPerson,
                    SkillName = "c##",
                    Level = 5
                },
                new PersonSkills
                {
                    Person = person,
                    PersonId = idPerson,
                    SkillName = "c--",
                    Level = 7
                }
            };
            await _repoPerson.AddPersonAsync(person);
            foreach(var personSkill in personSkillsInit)
            {
                await _repoPersonSkills.AddPersonSkillAsync(personSkill);
            }
            await _repoPerson.SaveAsync();

            var personSkillsExpected = new List<PersonSkills>()
            {
                new PersonSkills
                {
                    Person = person,
                    PersonId = idPerson,
                    SkillName = "c#",
                    Level = 3
                },
                new PersonSkills
                {
                    Person = person,
                    PersonId =idPerson,
                    SkillName = "c++",
                    Level = 2
                }
            };

            await _personSkillsService.ChangePersonSkillsAsync(newPersonSkills, idPerson);

            var personSkillsActual = await _repoPersonSkills.GetAllPersonSkillsAsync(idPerson);
            personSkillsActual.Should().BeEquivalentTo(personSkillsExpected);
        }

        [Theory]
        [ClassData(typeof(PersonSkillsData))]
        public async Task AddPersonSkillsAsync_ReturnAddedPersonSkillsFromDatabase(Dictionary<string,byte> newPersonSkills, long idPerson)
        {
            var person = new Person() 
            {
                Id = idPerson,
                Name = "Egor",
                DisplayName = "Duvanov"
            };

            var personSkillsExpected = new List<PersonSkills>()
            {
                new PersonSkills
                {
                    Person = person,
                    PersonId = idPerson,
                    SkillName = "c#",
                    Level = 3
                },
                new PersonSkills
                {
                    Person = person,
                    PersonId =idPerson,
                    SkillName = "c++",
                    Level = 2
                }
            };
            await _repoPerson.AddPersonAsync(person);
            await _repoPerson.SaveAsync();

            await _personSkillsService.AddPersonSkillsAsync(newPersonSkills, idPerson);

            var personSkillsActual = await _repoPersonSkills.GetAllPersonSkillsAsync(idPerson);
            personSkillsActual.Should().BeEquivalentTo(personSkillsExpected);
        }
    }
}
