using BackTest.Models;

namespace BackTest.Services.Interface
{
    public interface IPersonSkillsService
    {
        /// <summary>
        /// Change <see cref="PersonSkills"/> for <see cref="Person"/>
        /// </summary>
        /// <param name="newPersonSkills">update skill for person</param>
        /// <param name="idPerson"></param>
        Task ChangePersonSkillsAsync(Dictionary<string, byte> newPersonSkills, long idPerson);
        /// <summary>
        /// Update <see cref="PersonSkills"/> for <see cref="Person"/> by his ID
        /// </summary>
        /// <param name="idPerson">id employee whom we want update level skill</param>
        /// <param name="skillName">name skill</param>
        /// <param name="levelSkill">level skill</param>
        Task UpdatePersonSkillAsync(long idPerson, string skillName, byte levelSkill);
        /// <summary>
        /// Remove <see cref="Skill"/> from database by skill name
        /// </summary>
        /// <param name="nameSkill"></param>
        /// <param name="personSkills"></param>
        List<PersonSkills> DeletePersonSkill(string nameSkill, List<PersonSkills> personSkills);
        /// <summary>
        /// Add new <see cref="Skill"/> for <see cref="Person"/> and save in database
        /// </summary>
        /// <param name="idPerson">id employee whom we want add skills</param>
        /// <param name="skillName">name skill</param>
        /// <param name="levelSkill">level skill</param>
        Task NewPersonSkillsAsync(long idPerson, string skillName, byte levelSkill);
        /// <summary>
        /// Add new <see cref="PersonSkills"/> for <see cref="Person"/>
        /// </summary>
        /// <param name="newPersonSkills">skills person</param>
        /// <param name="idPerson"></param>
        Task AddPersonSkillsAsync(Dictionary<string, byte> newPersonSkills, long idPerson);
    }
}
