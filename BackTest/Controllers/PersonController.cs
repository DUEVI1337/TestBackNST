using BackTest.Data;
using BackTest.Mappers;
using BackTest.Models;
using BackTest.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackTest.Controllers
{
    /// <summary>
    /// This controller is designed to work
    /// with employees
    /// </summary>
    [Route("api/v1")]
    [ApiController]
    public class PersonController : Controller
    {
        private readonly DataContext _db;
        private readonly PersonActions _personActions;
        public PersonController(DataContext db, PersonActions personActions)
        {
            _db = db;
            _personActions = personActions;
        }

        /// <summary>
        /// Gets all employees
        /// </summary>
        /// <returns>Json file with a list all employees</returns>
        /// <response code="200">returns json file with employees</response>
        /// <response code="404">employees in company are absent</response>
        [Route("persons")]
        [HttpGet]
        public async Task<ActionResult> GetAllPersonsAsync()
        {
            List<Person> persons = await _db.Persons.ToListAsync();
            var presonsListDto = new List<PersonDto>(Mapper.MapperListPersons(persons));
            return Json(presonsListDto);
        }


        /// <summary>
        /// Gets a certain worker by his id
        /// </summary>
        /// <param name="id">id employee we want found</param>
        /// <returns>Json file with found employee</returns>
        /// <response code="200">return found employee</response>
        /// <response code="404">employee with this id not found</response>
        [Route("person/{id}")]
        [HttpGet]
        public async Task<ActionResult> GetPersonAsync(long id)
        {
            Person person = await _db.Persons.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            PersonDto personDto = Mapper.MapperPerson(person);
            return Json(personDto);
        }

        /// <summary>
        /// Create new person with certain skills
        /// </summary>
        /// <param name="model">Model contain: name employee, display name employee and skills person</param>
        /// <returns>status code about the success of creating an employee</returns>
        /// <response code="200">Employee successfully created</response>
        /// <response code="400">Employee not created</response>
        [Route("person")]
        [HttpPost]
        public async Task<ActionResult> CreatePersonAsync([FromBody] CreatePersonDto model)
        {
            if (ModelState.IsValid)
            {
                Person person = await _personActions.NewPersonAsync(model.Name, model.DisplayName);
                await _personActions.AddPersonSkillsAsync(model.PersonSkills, person);
                return Ok();
            }
            return BadRequest();
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
        [Route("person/{id}")]
        [HttpPut]
        public async Task<ActionResult> UpdatePersonAsync([FromBody] UpdatePersonDto model, long id)
        {
            Person person = await _db.Persons.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            person.Name = model.Name == null ? person.Name : model.Name;
            person.DisplayName = model.DisplayName == null ? person.DisplayName : model.DisplayName;
            _db.Persons.Update(person);
            await _db.SaveChangesAsync();
            await _personActions.ChangePersonSkillsAsync(model.PersonSkills, person);
            return Ok();
        }

        /// <summary>
        /// Delete employee from database
        /// </summary>
        /// <param name="id">id employee we want delete</param>
        /// <returns>status code about the success of deleting an employee</returns>
        /// <remarks code="200">Employee successfully deleted</remarks>
        /// <remarks code="404">Employee not founded</remarks>
        [Route("person/{id}")]
        [HttpDelete]
        public async Task<ActionResult> DeletePersonAsync(long id)
        {
            Person person = await _db.Persons.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            _db.Persons.Remove(person);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}
