using BackTest.Data;
using BackTest.Models;
using Microsoft.EntityFrameworkCore;

namespace BackTest.Services
{
    /// <summary>
    /// This class contains methods for working with data about a person 
    /// </summary>
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
            var person = new Person { Name = name, DisplayName = displayName };
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
            var skill = new Skill { Name = name };
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
            var personSkills = new PersonSkills()
            {
                PersonId = id,
                SkillName = skillName,
                Level = levelSkill
            };
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
        /// Update skills for person
        /// </summary>
        /// <param name="newPersonSkills">update skill for person</param>
        /// <param name="person"></param>
        public async Task ChangePersonSkillsAsync(Dictionary<string, byte> newPersonSkills, Person person)
        {
            List<PersonSkills> personSkills = await _db.PersonSkills.Where(x => x.PersonId == person.Id).ToListAsync();
            for (int i = 0; i < newPersonSkills.Count; i++)
            {
                var newSkillPerson = newPersonSkills.ElementAt(i);
                if (!_db.Skills.Select(x => x.Name).Contains(newSkillPerson.Key))
                {
                    await NewSkillAsync(newSkillPerson.Key);
                }
                else if(person.PersonSkills.Select(x => x.SkillName).Contains(newSkillPerson.Key))
                {
                    await UpdatePersonSkillAsync(person.Id, newSkillPerson.Key, newSkillPerson.Value);
                    continue;
                }
                await NewPersonSkillsAsync(person.Id, newSkillPerson.Key, newSkillPerson.Value);
                DeletePersonSkill(newSkillPerson.Key, personSkills);
            }
            _db.PersonSkills.RemoveRange(personSkills);
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Remove skill from person by skill name
        /// </summary>
        /// <param name="nameSkill"></param>
        /// <param name="personSkills"></param>
        public void DeletePersonSkill(string nameSkill, List<PersonSkills> personSkills)
        {
            PersonSkills containsSkillPerson = personSkills.FirstOrDefault(x => x.SkillName == nameSkill);
            if (containsSkillPerson != default)
            {
                personSkills.Remove(containsSkillPerson);
            }
        }

        /// <summary>
        /// Add new skills for person
        /// </summary>
        /// <param name="newPersonSkills">skills person</param>
        /// <param name="person"></param>
        public async Task AddPersonSkillsAsync(Dictionary<string, byte> newPersonSkills, Person person)
        {
            for (int i = 0; i < newPersonSkills.Count; i++)
            {
                var newSkillPerson = newPersonSkills.ElementAt(i);
                if (!_db.Skills.Select(x => x.Name).Contains(newSkillPerson.Key))
                {
                    await NewSkillAsync(newSkillPerson.Key);
                }
                await NewPersonSkillsAsync(person.Id, newSkillPerson.Key, newSkillPerson.Value);
            }
        }
    }
}
