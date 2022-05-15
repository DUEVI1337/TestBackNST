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
        /// Add new <see cref="PersonSkills"/> for <see cref="Person"/>
        /// </summary>
        /// <param name="newPersonSkills">skills person</param>
        /// <param name="idPerson"></param>
        Task AddPersonSkillsAsync(Dictionary<string, byte> newPersonSkills, long idPerson);
    }
}
