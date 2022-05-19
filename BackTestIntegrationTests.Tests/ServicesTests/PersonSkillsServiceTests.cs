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
    public class PersonSkillsServiceTests : IClassFixture<WebAppFactoryTest<Program>>
    {
        private readonly WebAppFactoryTest<Program> _factory;
        private readonly IPersonSkillsService _personSkillsService;
        private DataContext _db;

        public PersonSkillsServiceTests(WebAppFactoryTest<Program> factory)
        {
            _factory = factory;
            _db = _factory.Services.CreateScope().ServiceProvider.GetService<DataContext>();
            _personSkillsService = _factory.Services.CreateScope().ServiceProvider.GetService<IPersonSkillsService>();
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
                DisplayName = "duevi"
            };
            var personSkillsInit = new List<PersonSkills>()
            {
                new PersonSkills
                {
                    PersonId = idPerson,
                    SkillName = "c##",
                    Level = 5
                },
                new PersonSkills
                {
                    PersonId = idPerson,
                    SkillName = "c--",
                    Level = 7
                }
            };
            await _db.Persons.AddAsync(person);
            await _db.PersonSkills.AddRangeAsync(personSkillsInit);
            await _db.SaveChangesAsync();

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

            var personSkillsActual = await _db.PersonSkills.Where(x=>x.PersonId==idPerson).ToListAsync();
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
                DisplayName = "duevi"
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
            await _db.Persons.AddAsync(person);
            await _db.SaveChangesAsync();

            await _personSkillsService.AddPersonSkillsAsync(newPersonSkills, idPerson);

            var personSkillsActual = await _db.PersonSkills.Where(x=>x.PersonId==idPerson).ToListAsync();
            personSkillsActual.Should().BeEquivalentTo(personSkillsExpected);
        }
    }
}
