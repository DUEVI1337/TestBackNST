using BackTest.Data;
using BackTest.Mappers;
using BackTest.Models;
using BackTest.Services;
using Microsoft.EntityFrameworkCore;

namespace BackTest.Repository
{
    /// <summary>
    /// This class contains methods for working with data about a person 
    /// </summary>
    public class PersonRepository
    {
        private readonly DataContext _db;
        public PersonRepository(DataContext db)
        {
            _db = db;
        }

        public async Task<List<Person>> FindAllPersonsAsync()
        {
            var persons = await _db.Persons.Include(x => x.PersonSkills).ToListAsync(); //include for optimization 
            return persons;
        }

        public async Task<Person> FindPersonByIdAsync(long id)
        {
            var person = await _db.Persons.FindAsync(id);
            return person;
        }

        public async Task<List<PersonSkills>> FindPersonSkillsById(long id)
        {
            var personSkills = await _db.PersonSkills.Where(x => x.PersonId == id).ToListAsync();
            return personSkills;
        }

        public async Task InsertPersonAsync(Person person)
        {
            await _db.Persons.AddAsync(person);
            await _db.SaveChangesAsync();
        }

        public async Task UpdatePersonAsync(Person person)
        {
            _db.Persons.Update(person);
            await _db.SaveChangesAsync();
        }

        public async Task DeletePersonAsync(long id)
        {
            var person = await _db.Persons.FindAsync(id);
            if (person == null)
            {
                return;
            }
            _db.Persons.Remove(person);
            await _db.SaveChangesAsync();
        }

        public async Task InsertPersonSkillAsync(PersonSkills personSkill)
        {
            await _db.PersonSkills.AddAsync(personSkill);
            await _db.SaveChangesAsync();
        }
    }
}
