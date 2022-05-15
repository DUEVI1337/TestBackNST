using BackTest.Data.Repository.Interfaces;
using BackTest.Models;
using Microsoft.EntityFrameworkCore;

namespace BackTest.Data.Repository
{
    /// <summary>
    /// This class contains methods for working with data about a skill in database
    /// </summary>
    public class SkillRepository : ISkillRepository
    {
        private readonly DataContext _db;
        public SkillRepository(DataContext db)
        {
            _db = db;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns><inheritdoc/></returns>
        public async Task<List<Skill>> GetAllSkillsAsync()
        {
            List<Skill> skills = await _db.Skills.ToListAsync();
            return skills;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="skill"></param>
        public async Task AddSkillAsync(Skill skill)
        {
            await _db.Skills.AddAsync(skill);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
