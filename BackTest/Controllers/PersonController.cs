using BackTest.Data;
using BackTest.Mappers;
using BackTest.Models;
using BackTest.Repository;
using BackTest.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Log = BackTest.Loggers;

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
        private readonly PersonService _personService;
        private readonly PersonRepository _personRepository;
        public PersonController(PersonService personService, PersonRepository personRepository)
        {
            _personService = personService;
            _personRepository = personRepository;
        }

        /// <summary>
        /// Gets all employees
        /// </summary>
        /// <returns>Json file with a list all employees</returns>
        /// <response code="200">returns json file with employees</response>
        /// <response code="400">failed get persons</response>
        /// <response code="404">employees in company are absent</response>
        [Route("persons")]
        [HttpGet]
        public async Task<ActionResult> GetAllPersonsAsync()
        {
            try
            {
                return Json(await _personService.GetAllPersonsAsync());
            }
            catch (Exception ex)
            {
                await Log.Logger.Log($"Не удалось создать Person, исключение {ex}", Log.TypeLog.Error);
                return BadRequest();
            }
        }


        /// <summary>
        /// Gets a certain worker by his id
        /// </summary>
        /// <param name="id">id employee we want found</param>
        /// <returns>Json file with found employee</returns>
        /// <response code="200">return found employee</response>
        /// <response code="400">failed get person</response>
        /// <response code="404">employee with this id not found</response>
        [Route("person/{id}")]
        [HttpGet]
        public async Task<ActionResult> GetPersonAsync(long id)
        {
            try
            {
                return Json(await _personService.GetPersonAsync(id));
            }
            catch (Exception ex)
            {
                await Log.Logger.Log($"Не удалось создать Person, исключение {ex}", Log.TypeLog.Error);
                return BadRequest();
            }
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
            try
            {
                await _personService.CreatePersonAsync(model);
                return Ok();
            }
            catch (Exception ex)
            {
                await Log.Logger.Log($"Не удалось создать Person, исключение {ex}", Log.TypeLog.Error);
                return BadRequest();
            }
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
            try
            {
                await _personService.UpdatePersonAsync(model, id);
                return Ok();
            }
            catch (Exception ex)
            {
                await Log.Logger.Log($"Не удалось создать Person, исключение {ex}", Log.TypeLog.Error);
                return BadRequest();
            }
        }

        /// <summary>
        /// Delete employee from database
        /// </summary>
        /// <param name="id">id employee we want delete</param>
        /// <returns>status code about the success of deleting an employee</returns>
        /// <remarks code="200">Employee successfully deleted</remarks>
        /// <response code="400">failed delete person</response>
        /// <remarks code="404">Employee not founded</remarks>
        [Route("person/{id}")]
        [HttpDelete]
        public async Task<ActionResult> DeletePersonAsync(long id)
        {
            try
            {
                await _personRepository.DeletePersonAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                await Log.Logger.Log($"Не удалось создать Person, исключение {ex}", Log.TypeLog.Error);
                return BadRequest();
            }
        }
    }
}
