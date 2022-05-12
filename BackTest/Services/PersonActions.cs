using BackTest.Data;
using BackTest.Models;
using Microsoft.EntityFrameworkCore;

namespace BackTest.Services
{
    public class PersonActions
    {
        private readonly DataContext _db;
        public PersonActions(DataContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Create new person and add in database
        /// </summary>
        /// <param name="name">name employee</param>
        /// <param name="displayName">display name employee</param>
        /// <returns>returns created <seealso cref="Person"/></returns>
        public async Task<Person> NewPersonAsync(string name, string displayName)
        {
            Person person = new Person { Name = name, DisplayName = displayName };
            await _db.Persons.AddAsync(person);
            await _db.SaveChangesAsync();
            return person;
        }

        /// <summary>
        /// Create new skill and add in database
        /// </summary>
        /// <param name="name">name skill</param>
        public async Task NewSkillAsync(string name)
        {
            Skill skill = new Skill { Name = name };
            await _db.Skills.AddAsync(skill);
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Add new skill for person and save in database
        /// </summary>
        /// <param name="id">id employee whom we want add skills</param>
        /// <param name="skillName">name skill</param>
        /// <param name="levelSkill">level skill</param>
        public async Task NewPersonSkillsAsync(long id, string skillName, byte levelSkill)
        {
            PersonSkills personSkills = new PersonSkills();
            personSkills.PersonId = id;
            personSkills.SkillName = skillName;
            personSkills.Level = levelSkill;
            await _db.PersonSkills.AddAsync(personSkills);
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Update level skill certain employee
        /// </summary>
        /// <param name="idPerson">id employee whom we want update level skill</param>
        /// <param name="skillName">name skill</param>
        /// <param name="levelSkill">level skill</param>
        public async Task UpdatePersonSkillAsync(long idPerson, string skillName, byte levelSkill)
        {
            PersonSkills personSkills = await _db.PersonSkills.FirstOrDefaultAsync(x => x.PersonId == idPerson && x.SkillName == skillName);
            if (levelSkill != personSkills.Level)
            {
                personSkills.Level = levelSkill;
                _db.PersonSkills.Update(personSkills);
                await _db.SaveChangesAsync();
            }

        }

        /// <summary>
        /// Add or/and update skills for person
        /// </summary>
        /// <param name="personSkills">update skill for person</param>
        /// <param name="person">/param>
        /// <returns>created or/and update skills for person</returns>
        public async Task ChangePersonSkillsAsync(Dictionary<string, byte> personSkills, Person person)
        {
            for (int i = 0; i < personSkills.Count; i++)
            {
                var skillPerson = personSkills.ElementAt(i);
                if (!_db.Skills.Select(x => x.Name).Contains(skillPerson.Key))
                {
                    await NewSkillAsync(skillPerson.Key);
                }
                else if (person.PersonSkills.Select(x => x.SkillName).Contains(skillPerson.Key))
                {
                    await UpdatePersonSkillAsync(person.Id, skillPerson.Key, skillPerson.Value);
                    continue;
                }
                await NewPersonSkillsAsync(person.Id, skillPerson.Key, skillPerson.Value);
            }
        }

        /// <summary>
        /// Add new skills for person
        /// </summary>
        /// <param name="personSkills">skills person</param>
        /// <param name="person"></param>
        /// <returns>created skills for person</returns>
        public async Task AddPersonSkillsAsync(Dictionary<string, byte> personSkills, Person person)
        {
            for (int i = 0; i < personSkills.Count; i++)
            {
                var skillPerson = personSkills.ElementAt(i);
                if (!_db.Skills.Select(x => x.Name).Contains(skillPerson.Key))
                {
                    await NewSkillAsync(skillPerson.Key);
                }
                await NewPersonSkillsAsync(person.Id, skillPerson.Key, skillPerson.Value);
            }
        }
    }
}
