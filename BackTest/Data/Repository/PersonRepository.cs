using BackTest.Data.Repository.Interfaces;
using BackTest.Models;
using Microsoft.EntityFrameworkCore;

namespace BackTest.Data.Repository
{
    /// <summary>
    /// This class contains methods for working with data about a person in database
    /// </summary>
    public class PersonRepository : IPersonRepository
    {
        private readonly DataContext _db;
        public PersonRepository(DataContext db)
        {
            _db = db;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns><inheritdoc/></returns>
        public async Task<List<Person>> GetAllPersonsAsync()
        {
            var persons = await _db.Persons.Include(x => x.PersonSkills).ToListAsync(); //include for optimization 
            return persons;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="idPerson"></param>
        /// <returns><inheritdoc/></returns>
        public async Task<Person> GetPersonByIdAsync(long idPerson)
        {
            var person = await _db.Persons.Include(x=>x.PersonSkills).FirstOrDefaultAsync(x=>x.Id==idPerson);
            return person;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="person"></param>
        public async Task AddPersonAsync(Person person)
        {
            await _db.Persons.AddAsync(person);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="person"></param>
        public void UpdatePerson(Person person)
        {
            _db.Persons.Update(person);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="person"></param> 
        public void RemovePerson(Person person)
        {
            _db.Persons.Remove(person);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

    }
}
