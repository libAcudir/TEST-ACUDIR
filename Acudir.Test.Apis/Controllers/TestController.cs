namespace Acudir.Test.Apis.Controllers
{
    using Acudir.Test.Apis.Common;
    using Microsoft.AspNetCore.Mvc;
    using Services.DTOs;
    using Services.Interfaces;

    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {

        private readonly IPersonService _personService;
        public TestController(IPersonService personService)
        {
            _personService = personService;
        }

        /// <summary>
        /// Retrieves a list of all persons with optional filtering.
        /// </summary>
        /// <param name="filterQueryDTO">The filter criteria to apply.</param>
        /// <returns>A list of persons or a not found response if no persons exist.</returns>
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] FilterQueryDTO filterQueryDTO)
        {
            try
            {
                var result = await _personService.GetAll(filterQueryDTO);
                var response = new ApiResponse();

                if (!result.Any())
                {
                    response.Success = false;
                    response.Message = "No person found.";
                    response.Data = result;

                    return NotFound(response);
                }

                response.Success = true;
                response.Message = "List of persons";
                response.Data = result;

                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while processing the request",
                    Errors = new List<string> { ex.Message }
                };

                return BadRequest(errorResponse);
            }
        }

        /// <summary>
        /// Adds one or more persons to the database.
        /// </summary>
        /// <param name="personDTOs">The list of persons to add.</param>
        /// <returns>A response indicating success or failure.</returns>
        [HttpPost("AddPerson")]
        public async Task<IActionResult> AddPerson([FromBody] List<PersonDTO> personDTOs)
        {

            foreach (var person in personDTOs)
            {
                if (!TryValidateModel(person))
                {
                    var errorResponse = new ApiResponse
                    {
                        Success = false,
                        Message = "Validation errors occurred.",
                        Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                    };
                    return BadRequest(errorResponse);
                }
            }


            try
            {
                var result = await _personService.AddPerson(personDTOs);
                var response = new ApiResponse();

                response.Success = true;
                response.Message = "Persons added successfully.";
                response.Data = result;

                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while processing the request.",
                    Errors = new List<string> { ex.Message }
                };

                return BadRequest(errorResponse);
            }
        }

        /// <summary>
        /// Updates the data of an existing person.
        /// </summary>
        /// <param name="namePersonToUpdate">The name of the person to update.</param>
        /// <param name="personUpdate">The new data for the person.</param>
        /// <returns>A response indicating success or a not found response if the person does not exist.</returns>
        [HttpPut("UpdatePerson")]
        public async Task<IActionResult> UpdatePerson(string namePersonToUpdate, [FromQuery] PersonDTO personUpdate)
        {
            try
            {
                var result = await _personService.UpdatePerson(namePersonToUpdate, personUpdate);
                var response = new ApiResponse();

                response.Success = true;
                response.Message = "Person data updated";
                response.Data = result;

                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                var errorResponse = new ApiResponse
                {
                    Success = false,
                    Message = ex.Message,
                    Errors = new List<string> { ex.Message }
                };

                return NotFound(errorResponse);
            }
            catch (Exception ex)
            {
                var errorResponse = new ApiResponse
                {
                    Success = false,
                    Message = "OAn error occurred while processing the request.",
                    Errors = new List<string> { ex.Message }
                };

                return BadRequest(errorResponse);
            }
        }

    }
}
