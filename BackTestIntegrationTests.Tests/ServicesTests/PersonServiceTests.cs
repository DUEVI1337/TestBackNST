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
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BackTestIntegrationTests.Tests.ServicesTests
{
    public class PersonServiceTests
    {
        private readonly IPersonService _personService;
        private readonly IPersonRepository _repoPerson;
        private WebApplicationFactory<Program> _app;
        private DataContext _db;
        public PersonServiceTests()
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
            _repoPerson = new PersonRepository(_db);
            _personService = new PersonService(_repoPerson);
            _db.Database.EnsureDeleted();
            _db.Database.EnsureCreated();
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
                DisplayName = "Duvanov"
            };
            //Act
            await _personService.CreatePersonAsync(model);

            //Assert
            var personActual = await _repoPerson.GetPersonByIdAsync(1);
            personActual.Should().BeEquivalentTo(personExpected);
        }

        [Theory]
        [ClassData(typeof(UpdatePersonDtoData))]
        public async Task UpdatePersonAsync_ReturnFromDatabaseUpdatedPerson(UpdatePersonDto model, long idPerson)
        {
            var personInit = new Person()
            {
                Id = idPerson,
                Name = "Egor",
                DisplayName = "Duvanov"
            };
            var personExpected = new Person()
            {
                Id = idPerson,
                Name = model.Name,
                DisplayName = model.DisplayName
            };
            await _repoPerson.AddPersonAsync(personInit);
            await _repoPerson.SaveAsync();

            await _personService.UpdatePersonAsync(model, idPerson);

            var personActual = await _repoPerson.GetPersonByIdAsync(idPerson);
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
                DisplayName = "Duvanov"
            };
            await _repoPerson.AddPersonAsync(personInit);
            await _repoPerson.SaveAsync();

            await _personService.DeletePersonAsync(idPerson);

            Assert.Null(await _repoPerson.GetPersonByIdAsync(idPerson));
        }
    }
}
