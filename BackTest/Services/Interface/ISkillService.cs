using BackTest.Models;

namespace BackTest.Services.Interface
{
    public interface ISkillService
    {
        /// <summary>
        /// Сhecks if there is such a <see cref="Skill"/> with such name in database
        /// </summary>
        /// <param name="nameSkills"></param>
        /// <returns></returns>
        Task CheckContainsSkillDbAsync(List<string> nameSkills);
    }
}
