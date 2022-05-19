using BackTest.Data.Repository.Interfaces;
using BackTest.Mappers;
using BackTest.Models;
using BackTest.Services.Interface;

namespace BackTest.Services
{
    /// <summary>
    /// Contain mehods for work with <see cref="Person"/>
    /// </summary>
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;
        public PersonService(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns><inheritdoc/></returns>
        public async Task<List<PersonDto>> GetAllPersonsAsync()
        {
            var personsListDto = MapperPerson.MapListPersons(await _personRepository.GetAllPersonsAsync());
            return personsListDto;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="idPerson"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public async Task<PersonDto> GetPersonAsync(long idPerson)
        {
            var person = await _personRepository.GetPersonByIdAsync(idPerson);
            if (person == null)
            {
                return null;
            }
            return MapperPerson.MapPerson(person);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="model"><inheritdoc/></param>
        public async Task<Person> CreatePersonAsync(CreatePersonDto model)
        {
            var person = new Person() { Name = model.Name, DisplayName = model.DisplayName };
            await _personRepository.AddPersonAsync(person);
            await _personRepository.SaveAsync();
            return person;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="model"><inheritdoc/></param>
        /// <param name="idPerson"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public async Task<bool> UpdatePersonAsync(UpdatePersonDto model, long idPerson)
        {
            var person = await _personRepository.GetPersonByIdAsync(idPerson);
            if (person == null)
            {
                return false;
            }
            person.Name = model.Name ?? person.Name;
            person.DisplayName = model.DisplayName ?? person.DisplayName;
            _personRepository.UpdatePerson(person);
            await _personRepository.SaveAsync();
            return true;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="idPerson"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public async Task<bool> DeletePersonAsync(long idPerson)
        {
            var person = await _personRepository.GetPersonByIdAsync(idPerson);
            if (person != null)
            {
                _personRepository.RemovePerson(person);
                await _personRepository.SaveAsync();
                return true;
            }
            return false;
        }
    }
}
