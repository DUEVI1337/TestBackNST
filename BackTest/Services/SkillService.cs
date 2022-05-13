using BackTest.Data.Repository.Interfaces;
using BackTest.Models;
using BackTest.Services.Interface;

namespace BackTest.Services
{
    /// <summary>
    /// Contain mehods for work with <see cref="Skill"/>
    /// </summary>
    public class SkillService : ISkillService
    {
        private readonly ISkillRepository _skillRepository;
        public SkillService(ISkillRepository skillRepository)
        {
            _skillRepository = skillRepository;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="nameSkills"></param>
        /// <returns></returns>
        public async Task CheckContainsSkillDbAsync(List<string> nameSkills)
        {
            var allSkills = await _skillRepository.GetAllSkillsAsync();
            for(int i = 0; i < nameSkills.Count; i++)
            {
                if (!allSkills.Select(x => x.Name).Contains(nameSkills[i]))
                {
                    await NewSkillAsync(nameSkills[i]);
                }
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="name"><inheritdoc/></param>
        public async Task NewSkillAsync(string name)
        {
            var skill = new Skill { Name = name };
            await _skillRepository.AddSkillAsync(skill);
            await _skillRepository.SaveAsync();
        }
    }
}
