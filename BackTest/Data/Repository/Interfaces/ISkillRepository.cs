using BackTest.Models;

namespace BackTest.Data.Repository.Interfaces
{
    public interface ISkillRepository
    {
        /// <summary>
        /// Find in database all <see cref="Skill"/>
        /// </summary>
        /// <returns>List <see cref="Skill"/></returns>
        Task<List<Skill>> GetAllSkillsAsync();
        /// <summary>
        /// Add in database <see cref="Skill"/>
        /// </summary>
        /// <param name="skill"></param>
        Task AddSkillAsync(Skill skill);
        /// <summary>
        /// Save changes in database
        /// </summary>
        Task SaveAsync();
    }
}
