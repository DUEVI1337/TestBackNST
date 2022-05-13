using BackTest.Models;

namespace BackTest.Data.Repository.Interfaces
{
    public interface IPersonRepository
    {
        /// <summary>
        /// Find all <see cref="Person"/> in database
        /// </summary>
        /// <returns>List <see cref="Person"/></returns>
        Task<List<Person>> GetAllPersonsAsync();
        /// <summary>
        /// Find <see cref="Person"/> by him ID
        /// </summary>
        /// <param name="idPerson"></param>
        /// <returns>Find <see cref="Person"/></returns>
        Task<Person> GetPersonByIdAsync(long idPerson);
        /// <summary>
        /// Add in database <see cref="Person"/>
        /// </summary>
        /// <param name="person"></param>
        Task AddPersonAsync(Person person);
        /// <summary>
        /// Update in database <see cref="Person"/>
        /// </summary>
        /// <param name="person"></param>
        Task UpdatePersonAsync(Person person);
        /// <summary>
        /// Remove from database <see cref="Person"/> by him ID
        /// </summary>
        /// <param name="idPerson"></param>
        Task RemovePersonAsync(Person person);
        /// <summary>
        /// Save changes in database
        /// </summary>
        Task SaveAsync();

    }
}
