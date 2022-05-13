using BackTest.Models;

namespace BackTest.Services.Interface
{
    /// <summary>
    /// Contain mehods for work with <see cref="Person"/>
    /// </summary>
    public interface IPersonService
    {
        /// <summary>
        /// Gets all <see cref="Person"/> and mapping in <see cref="PersonDto"/>
        /// </summary>
        /// <returns>List <see cref="PersonDto"/></returns>
        Task<List<PersonDto>> GetAllPersonsAsync();
        /// <summary>
        /// Gets <see cref="Person"/> by his id and mapping in <see cref="PersonDto"/>
        /// </summary>
        /// <param name="idPerson">id employee we want found</param>
        /// <returns><see cref="PersonDto"/></returns>
        Task<PersonDto> GetPersonAsync(long idPerson);
        /// <summary>
        /// Create new <see cref="Person"/> with certain <see cref="PersonSkills"/>
        /// </summary>
        /// <param name="model">Model contain: name employee, display name employee and skills person</param>
        /// <returns>created <see cref="Person"/></returns>
        Task<Person> CreatePersonAsync(CreatePersonDto model);
        /// <summary>
        /// Updating info about existing employee
        /// </summary>
        /// <param name="model">Model contain: name employee, display name employee and skills person</param>
        /// <param name="idPerson">id employee about which we want update info</param>
        /// <returns>result <see cref="bool"/> updated <see cref="Person"/></returns>
        Task<bool> UpdatePersonAsync(UpdatePersonDto model, long idPerson);
        /// <summary>
        /// Delete <see cref="Person"/> from database by his ID
        /// </summary>
        /// <param name="idPerson">id employee we want delete</param>
        /// <returns>result <see cref="bool"/> delete <see cref="Person"/></returns>
        Task<bool> DeletePersonAsync(long idPerson);
    }
}
