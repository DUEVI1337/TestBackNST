using System;
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
    public class SkillServiceTests
    {
        private readonly ISkillService _skillService;
        private readonly ISkillRepository _repoSkill;
        private WebApplicationFactory<Program> _app;
        private DataContext _db;

        public SkillServiceTests()
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
            _repoSkill = new SkillRepository(_db);
            _skillService = new SkillService(_repoSkill);
            _db.Database.EnsureDeleted();
            _db.Database.EnsureCreated();
        }

        [Theory]
        [ClassData(typeof(ListSkillsData))]
        public async Task CheckContainsSkillDbAsync_ReturnAddedSkillsFromDataBase(List<string> nameSkills)
        {
            var skillsExpected = new List<Skill>()
            {
                new Skill() {Name = "c#" },
                new Skill() {Name = "c++" }
            };

            await _skillService.CheckContainsSkillDbAsync(nameSkills);

            var skillsActual = await _repoSkill.GetAllSkillsAsync();
            skillsActual.Should().BeEquivalentTo(skillsExpected);
        }
    }
}
