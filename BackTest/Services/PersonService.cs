using BackTest.Mappers;
using BackTest.Models;
using BackTest.Repository;
using Microsoft.EntityFrameworkCore;

namespace BackTest.Services
{
    public class PersonService
    {
        private readonly PersonRepository _personRepository;
        private readonly SkillRepository _skillRepository;
        public PersonService(PersonRepository personRepository, SkillRepository skillRepository)
        {
            _personRepository = personRepository;
            _skillRepository = skillRepository;
        }

        public async Task<List<PersonDto>> GetAllPersonsAsync()
        {
            var personsListDto = new List<PersonDto>(Mapper.MapperListPersons(await _personRepository.FindAllPersonsAsync()));
            return personsListDto;
        }


        /// <summary>
        /// Gets a certain worker by his id
        /// </summary>
        /// <param name="id">id employee we want found</param>
        /// <returns>Json file with found employee</returns>
        /// <response code="200">return found employee</response>
        /// <response code="400">failed get person</response>
        /// <response code="404">employee with this id not found</response>
        public async Task<PersonDto> GetPersonAsync(long id)
        {
            var personDto = Mapper.MapperPerson(await _personRepository.FindPersonByIdAsync(id));
            return personDto;
        }

        /// <summary>
        /// Create new person with certain skills
        /// </summary>
        /// <param name="model">Model contain: name employee, display name employee and skills person</param>
        /// <returns>status code about the success of creating an employee</returns>
        /// <response code="200">Employee successfully created</response>
        /// <response code="400">Employee not created</response>
        public async Task CreatePersonAsync(CreatePersonDto model)
        {
            var person = new Person() { Name = model.Name, DisplayName = model.DisplayName};
            await _personRepository.InsertPersonAsync(person);
            await AddPersonSkillsAsync(model.PersonSkills, person);
        }

        /// <summary>
        /// Updating info about existing employee
        /// </summary>
        /// <param name="model">Model contain: name employee, display name employee and skills person</param>
        /// <param name="id">id employee about which we want update info</param>
        /// <returns>status code about the success of updating an employee</returns>
        /// <response code="200">Employee successfully updated</response>
        /// <response code="400">Employee not updated</response>
        /// <response code="404">Employee not found</response>
        public async Task UpdatePersonAsync(UpdatePersonDto model, long id)
        {
            var person = await _personRepository.FindPersonByIdAsync(id);
            if (person == null)
            {
                return;
            }
            person.Name = model.Name ?? person.Name;
            person.DisplayName = model.DisplayName ?? person.DisplayName;
            await _personRepository.UpdatePersonAsync(person);
            //await ChangePersonSkillsAsync();
        }

        /// <summary>
        /// Delete employee from database
        /// </summary>
        /// <param name="id">id employee we want delete</param>
        /// <returns>status code about the success of deleting an employee</returns>
        /// <remarks code="200">Employee successfully deleted</remarks>
        /// <response code="400">failed delete person</response>
        /// <remarks code="404">Employee not founded</remarks>
        public async Task DeletePersonAsync(long id)
        {
            await _personRepository.DeletePersonAsync(id);

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
            await _personRepository.InsertPersonAsync(person);
            return person;
        }

        /// <summary>
        /// Create new skill and add in database
        /// </summary>
        /// <param name="name">name skill</param>
        public async Task NewSkillAsync(string name)
        {
            var skill = new Skill { Name = name };
            await _skillRepository.InsertSkillAsync(skill);
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
            await _personRepository.InsertPersonSkillAsync(personSkills);
        }

        /// <summary>
        /// Update level skill certain employee
        /// </summary>
        /// <param name="idPerson">id employee whom we want update level skill</param>
        /// <param name="skillName">name skill</param>
        /// <param name="levelSkill">level skill</param>
        public async Task UpdatePersonSkillAsync(long idPerson, string skillName, byte levelSkill)
        {
            //PersonSkills personSkills = await _db.PersonSkills.FirstOrDefaultAsync(x => x.PersonId == idPerson && x.SkillName == skillName);
            //if (levelSkill != personSkills.Level)
            //{
            //    personSkills.Level = levelSkill;
            //    _db.PersonSkills.Update(personSkills);
            //    await _db.SaveChangesAsync();
            //}

        }

        /// <summary>
        /// Update skills for person
        /// </summary>
        /// <param name="newPersonSkills">update skill for person</param>
        /// <param name="person"></param>
        public async Task ChangePersonSkillsAsync(Dictionary<string, byte> newPersonSkills, Person person)
        {
            //List<PersonSkills> personSkills = await _personRepository.FindPersonSkillsById(person.Id);
            //for (int i = 0; i < newPersonSkills.Count; i++)
            //{
            //    var newSkillPerson = newPersonSkills.ElementAt(i);
            //    if (!_skillRepository.FindAllSkillsAsync().Select().Contains(newSkillPerson.Key))
            //    {
            //        await NewSkillAsync(newSkillPerson.Key);
            //    }
            //    else if (person.PersonSkills.Select(x => x.SkillName).Contains(newSkillPerson.Key))
            //    {
            //        await UpdatePersonSkillAsync(person.Id, newSkillPerson.Key, newSkillPerson.Value);
            //        continue;
            //    }
            //    await NewPersonSkillsAsync(person.Id, newSkillPerson.Key, newSkillPerson.Value);
            //    DeletePersonSkill(newSkillPerson.Key, personSkills);
            //}
            //_db.PersonSkills.RemoveRange(personSkills);
            //await _db.SaveChangesAsync();
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
            var allSkills = await _skillRepository.FindAllSkillsAsync();
            for (int i = 0; i < newPersonSkills.Count; i++)
            {
                var newSkillPerson = newPersonSkills.ElementAt(i);
                if (!allSkills.Select(x => x.Name).Contains(newSkillPerson.Key))
                {
                    await NewSkillAsync(newSkillPerson.Key);
                }
                await NewPersonSkillsAsync(person.Id, newSkillPerson.Key, newSkillPerson.Value);
            }
        }
    }
}
