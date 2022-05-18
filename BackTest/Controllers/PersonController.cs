using BackTest.Models;
using BackTest.Services.Interface;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IPersonService _personService;
        private readonly IPersonSkillsService _personSkillsService;
        private readonly ISkillService _skillService;
        public PersonController(IPersonService personService, IPersonSkillsService personSkillsService, ISkillService skillService)
        {
            _personService = personService;
            _skillService = skillService;
            _personSkillsService = personSkillsService;
        }

        /// <summary>
        /// Gets all employees
        /// </summary>
        /// <returns>Json file with a list all employees</returns>
        /// <response code="200">returns json file with employees</response>
        /// <response code="400">failed get persons</response>
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
                await Log.Logger.LogFile($"Не удалось получить всех Person, исключение {ex}", Log.TypeLog.Error);
                return BadRequest();
            }
        }


        /// <summary>
        /// Gets a certain worker by his id
        /// </summary>
        /// <param name="id">id employee we want found</param>
        /// <returns>Json file with found employee or null</returns>
        /// <response code="200">return found employee</response>
        /// <response code="400">failed get person</response>
        /// <response code="404">employee with this id not found</response>
        [Route("person/{id}")]
        [HttpGet]
        public async Task<ActionResult> GetPersonAsync(long id)
        {
            try
            {
                var personDto = await _personService.GetPersonAsync(id);
                if (personDto == null)
                {
                    return NotFound();
                }
                return Json(personDto);
            }
            catch (Exception ex)
            {
                await Log.Logger.LogFile($"Не удалось получить определенного Person, исключение {ex}", Log.TypeLog.Error);
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
                var person = await _personService.CreatePersonAsync(model);
                await _skillService.CheckContainsSkillDbAsync(model.PersonSkills.Select(x => x.Key).ToList());
                await _personSkillsService.AddPersonSkillsAsync(model.PersonSkills, person.Id);
                return Ok();
            }
            catch (Exception ex)
            {
                await Log.Logger.LogFile($"Не удалось создать Person, исключение {ex}", Log.TypeLog.Error);
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
                var result = await _personService.UpdatePersonAsync(model, id);
                if (result == true)
                {
                    await _skillService.CheckContainsSkillDbAsync(model.PersonSkills.Select(x => x.Key).ToList());
                    await _personSkillsService.ChangePersonSkillsAsync(model.PersonSkills, id);
                    return Ok();
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                await Log.Logger.LogFile($"Не удалось обновить Person, исключение {ex}", Log.TypeLog.Error);
                return BadRequest();
            }
        }

        /// <summary>
        /// Delete employee from database
        /// </summary>
        /// <param name="id">id employee we want delete</param>
        /// <returns>status code about the success of deleting an employee</returns>
        /// <response code="200">Employee successfully deleted</response>
        /// <response code="400">failed delete person</response>
        /// <response code="404">Employee not founded</response>
        [Route("person/{id}")]
        [HttpDelete]
        public async Task<ActionResult> DeletePersonAsync(long id)
        {
            try
            {
                var result = await _personService.DeletePersonAsync(id);
                if (result == true)
                {
                    return Ok();
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                await Log.Logger.LogFile($"Не удалось удалить Person, исключение {ex}", Log.TypeLog.Error);
                return BadRequest();
            }
        }
    }
}
