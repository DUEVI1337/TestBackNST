using System;
using BackTest.Data;
using BackTest.Data.Repository;
using BackTest.Data.Repository.Interfaces;
using BackTest.Models;
using BackTest.Services;
using BackTest.Services.Interface;
using BackTestIntegrationTests.Tests.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using System.Linq;

namespace BackTestIntegrationTests.Tests.ServicesTests
{
    public class SkillServiceTests : IClassFixture<WebAppFactoryTest<Program>>
    {
        private readonly WebAppFactoryTest<Program> _factory;
        private readonly ISkillService _skillService;
        private DataContext _db;

        public SkillServiceTests(WebAppFactoryTest<Program> factory)
        {
            _factory = factory;
            _db = _factory.Services.CreateScope().ServiceProvider.GetService<DataContext>();
            _skillService = _factory.Services.CreateScope().ServiceProvider.GetService<ISkillService>();
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

            var skillsActual = await _db.Skills.ToListAsync();
            Assert.Equal(skillsExpected.Count(), skillsActual.Count());
        }
    }
}
