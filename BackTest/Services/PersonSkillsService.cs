using BackTest.Data.Repository.Interfaces;
using BackTest.Models;
using BackTest.Services.Interface;

namespace BackTest.Services
{
    /// <summary>
    /// Contain mehods for work with <see cref="PersonSkills"/>
    /// </summary>
    public class PersonSkillsService : IPersonSkillsService
    {
        private readonly IPersonSkillsRepository _personSkillsRepository;
        public PersonSkillsService(IPersonSkillsRepository personSkillsRepository)
        {
            _personSkillsRepository = personSkillsRepository;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="newPersonSkills"></param>
        /// <param name="idPerson"></param>
        public async Task ChangePersonSkillsAsync(Dictionary<string, byte> newPersonSkills, long idPerson)
        {
            var personSkills = await _personSkillsRepository.GetAllPersonSkillsAsync(idPerson);
            for (int i = 0; i < newPersonSkills.Count; i++)
            {
                var newSkillPerson = newPersonSkills.ElementAt(i);
                if (personSkills.Select(x => x.SkillName).Contains(newSkillPerson.Key))
                {
                    await UpdatePersonSkillAsync(idPerson, newSkillPerson.Key, newSkillPerson.Value);
                    personSkills = DeletePersonSkill(newSkillPerson.Key, personSkills);
                    continue;
                }
                await NewPersonSkillsAsync(idPerson, newSkillPerson.Key, newSkillPerson.Value);
            }
            await _personSkillsRepository.RemoveRangePersonSkillsAsync(personSkills);
            await _personSkillsRepository.SaveAsync();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="newPersonSkills">skills person</param>
        /// <param name="idPerson"></param>
        public async Task AddPersonSkillsAsync(Dictionary<string, byte> newPersonSkills, long idPerson)
        {
            for (int i = 0; i < newPersonSkills.Count; i++)
            {
                var newSkillPerson = newPersonSkills.ElementAt(i);
                await NewPersonSkillsAsync(idPerson, newSkillPerson.Key, newSkillPerson.Value);
            }
        }

        /// <summary>
        /// Add new <see cref="Skill"/> for <see cref="Person"/> and save in database
        /// </summary>
        /// <param name="idPerson">id employee whom we want add skills</param>
        /// <param name="skillName">name skill</param>
        /// <param name="levelSkill">level skill</param>
        private async Task NewPersonSkillsAsync(long idPerson, string skillName, byte levelSkill)
        {
            var personSkill = new PersonSkills()
            {
                PersonId = idPerson,
                SkillName = skillName,
                Level = levelSkill
            };
            await _personSkillsRepository.AddPersonSkillAsync(personSkill);
            await _personSkillsRepository.SaveAsync();
        }

        /// <summary>
        /// Update <see cref="PersonSkills"/> for <see cref="Person"/> by his ID
        /// </summary>
        /// <param name="idPerson">id employee whom we want update level skill</param>
        /// <param name="skillName">name skill</param>
        /// <param name="levelSkill">level skill</param>
        private async Task UpdatePersonSkillAsync(long idPerson, string skillName, byte levelSkill)
        {
            PersonSkills personSkills = await _personSkillsRepository.GetPersonSkillAsync(idPerson, skillName);
            if (levelSkill != personSkills.Level)
            {
                personSkills.Level = levelSkill;
                await _personSkillsRepository.UpdatePersonSkillAsync(personSkills);
                await _personSkillsRepository.SaveAsync();
            }

        }

        /// <summary>
        /// Remove <see cref="Skill"/> from database by skill name
        /// </summary>
        /// <param name="nameSkill"></param>
        /// <param name="personSkills"></param>
        private List<PersonSkills> DeletePersonSkill(string nameSkill, List<PersonSkills> personSkills)
        {
            PersonSkills containsSkillPerson = personSkills.FirstOrDefault(x => x.SkillName == nameSkill);
            if (containsSkillPerson != null)
            {
                personSkills.Remove(containsSkillPerson);
            }
            return personSkills;
        }

    }
}
