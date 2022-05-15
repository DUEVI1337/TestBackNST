using BackTest.Data.Repository.Interfaces;
using BackTest.Models;
using Microsoft.EntityFrameworkCore;

namespace BackTest.Data.Repository
{
    /// <summary>
    /// This class contains methods for working with data about a person skills in database
    /// </summary>
    public class PersonSkillsRepository : IPersonSkillsRepository
    {
        private readonly DataContext _db;
        public PersonSkillsRepository(DataContext db)
        {
            _db = db;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="idPerson"></param>
        /// <returns><inheritdoc/></returns>
        public async Task<List<PersonSkills>> GetAllPersonSkillsAsync(long idPerson)
        {
            var personSkills = await _db.PersonSkills.Where(x => x.PersonId == idPerson).ToListAsync();
            return personSkills;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="idPerson"></param>
        /// <param name="skillName"></param>
        /// <returns><inheritdoc/></returns>
        public async Task<PersonSkills> GetPersonSkillAsync(long idPerson, string skillName)
        {
            var personSkill = await _db.PersonSkills.FirstOrDefaultAsync(x => x.PersonId == idPerson && x.SkillName == skillName);
            return personSkill;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="personSkill"></param>
        public async Task AddPersonSkillAsync(PersonSkills personSkill)
        {
            await _db.PersonSkills.AddAsync(personSkill);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="personSkill"></param>
        public async Task UpdatePersonSkillAsync(PersonSkills personSkill)
        {
            _db.PersonSkills.Update(personSkill);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="personSkills"></param>
        public async Task RemoveRangePersonSkillsAsync(List<PersonSkills> personSkills)
        {
            _db.PersonSkills.RemoveRange(personSkills);
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
