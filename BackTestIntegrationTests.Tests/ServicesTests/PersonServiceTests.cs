using BackTest.Data;
using BackTest.Data.Repository;
using BackTest.Data.Repository.Interfaces;
using BackTest.Models;
using BackTest.Services;
using BackTest.Services.Interface;
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
    public class PersonServiceTests : IClassFixture<WebAppFactoryTest<Program>>
    {
        private readonly WebAppFactoryTest<Program> _factory;
        private readonly IPersonService _personService;
        private DataContext _db;
        public PersonServiceTests(WebAppFactoryTest<Program> factory)
        {
            _factory = factory;
            _db = _factory.Services.CreateScope().ServiceProvider.GetService<DataContext>();
            _personService = _factory.Services.CreateScope().ServiceProvider.GetService<IPersonService>();
            _db.Database.EnsureDeleted();
            _db.Database.EnsureCreated();
        }

        [Fact]
        public async Task GetAllPersonAsync_ReturnListPersonDto()
        {
            var personsInit = new List<Person>()
            {
                new Person() {Id = 1, Name = "Egor", DisplayName="duevi" },
                new Person() {Id = 2, Name = "Egor2", DisplayName="duevi2" }
            };
            await _db.Persons.AddRangeAsync(personsInit);
            await _db.SaveChangesAsync();
            var personsExpected = new List<PersonDto>()
            {
                new PersonDto() { Id = 1, Name = "Egor", DisplayName="duevi"},
                new PersonDto() { Id = 2, Name = "Egor2", DisplayName="duevi2"},
            };

            List<PersonDto> personsActual = await _personService.GetAllPersonsAsync();

            Assert.Equal(personsActual.Count, personsExpected.Count);
        }

        [Theory]
        [InlineData(1)]
        public async Task GetPersonAsync_ReturnFindPerson(long id)
        {
            var personSkills = new List<PersonSkills>();
            var personInit = new Person() { Id = id, Name = "Egor", DisplayName = "duevi", PersonSkills = personSkills };
            await _db.Persons.AddAsync(personInit);
            await _db.SaveChangesAsync();

            PersonDto personActual = await _personService.GetPersonAsync(id);

            Assert.NotNull(personActual);
        }

        [Theory]
        [ClassData(typeof(CreatePersonDtoData))]
        public async Task CreatePersonAsync_CreateAndCheckInDatabase(CreatePersonDto model)
        {
            //Arrange
            var personExpected = new Person()
            {
                Id = 1,
                Name = "Egor",
                DisplayName = "duevi"
            };
            //Act
            await _personService.CreatePersonAsync(model);

            //Assert
            var personActual = await _db.Persons.FindAsync((long)1);
            Assert.NotNull(personActual);
        }

        [Theory]
        [ClassData(typeof(UpdatePersonDtoData))]
        public async Task UpdatePersonAsync_ReturnFromDatabaseUpdatedPerson(UpdatePersonDto model, long idPerson)
        {
            var personInit = new Person()
            {
                Id = idPerson,
                Name = "Egor",
                DisplayName = "duevi"
            };
            var personExpected = new Person()
            {
                Id = idPerson,
                Name = model.Name,
                DisplayName = model.DisplayName
            };
            await _db.Persons.AddAsync(personInit);
            await _db.SaveChangesAsync();

            await _personService.UpdatePersonAsync(model, idPerson);

            await _db.Entry(personInit).ReloadAsync();
            var personActual = await _db.Persons.FindAsync(idPerson);
            personActual.Should().BeEquivalentTo(personExpected);
        }

        [Theory]
        [InlineData(1)]
        public async Task DeletePersonAsync_DeleteFromDatabase(long idPerson)
        {
            var personInit = new Person()
            {
                Id = idPerson,
                Name = "Egor",
                DisplayName = "duevi"
            };
            await _db.Persons.AddAsync(personInit);
            await _db.SaveChangesAsync();

            await _personService.DeletePersonAsync(idPerson);

            await _db.Entry(personInit).ReloadAsync();
            var personActual = await _db.Persons.FindAsync(idPerson);
            Assert.Null(personActual);
        }
    }
}
