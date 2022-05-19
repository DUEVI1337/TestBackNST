using BackTest.Models;

namespace BackTest.Data.Repository.Interfaces
{
    public interface IPersonSkillsRepository
    {
        /// <summary>
        /// Find all <see cref="PersonSkills"/> for <see cref="Person"/> by him ID
        /// </summary>
        /// <param name="idPerson"></param>
        /// <returns>List <see cref="PersonSkills"/> for person</returns>
        Task<List<PersonSkills>> GetAllPersonSkillsAsync(long idPerson);
        /// <summary>
        /// Find <see cref="PersonSkills"/> for <see cref="Person"/> by ID person and name skill
        /// </summary>
        /// <param name="idPerson"></param>
        /// <param name="skillName"></param>
        /// <returns><see cref="PersonSkills"/> for person</returns>
        Task<PersonSkills> GetPersonSkillAsync(long idPerson, string skillName);
        /// <summary>
        /// Add in database <see cref="PersonSkills"/>
        /// </summary>
        /// <param name="personSkill"></param>
        Task AddPersonSkillAsync(PersonSkills personSkill);
        /// <summary>
        /// Update <see cref="PersonSkills"/> in database
        /// </summary>
        /// <param name="personSkill"></param>
        void UpdatePersonSkill(PersonSkills personSkill);
        /// <summary>
        /// Remove list <see cref="PersonSkills"/> from database
        /// </summary>
        /// <param name="personSkills"></param>
        void RemoveRangePersonSkills(List<PersonSkills> personSkills);
        /// <summary>
        /// Save changes in database
        /// </summary>
        Task SaveAsync();
    }
}
