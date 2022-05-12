using BackTest.Data;
using BackTest.Models;
using BackTest.Services;
using Microsoft.EntityFrameworkCore;

namespace BackTest.Repository
{
    public class SkillRepository
    {
        private readonly DataContext _db;
        public SkillRepository(DataContext db)
        {
            _db = db;
        }

        public async Task<List<Skill>> FindAllSkillsAsync()
        {
            List<Skill> skills = await _db.Skills.ToListAsync();
            return skills;
        }

        public async Task InsertSkillAsync(Skill skill)
        {
            await _db.Skills.AddAsync(skill);
            await _db.SaveChangesAsync();
        }
    }
}
