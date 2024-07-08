
using Domain.Command;
using Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Acudir.Test.Apis.Controllers
{

    [Route("api/Person")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        public IPersonRepository personRepo { get; set; }

        public PersonController(IPersonRepository personRepo_)
        {
            personRepo = personRepo_;
        }
        //Devolver una lista que retorne Personas
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(string? NombreCompleto, string? Edad, string? Domicilio, string? Profesion)
        {
            var result = personRepo.GetAllAsync(NombreCompleto, Edad, Domicilio, Profesion);
            return result != null
                ? Ok(result)
                : NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(PersonCreateCommand commandCreate)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await personRepo.PostAsync(commandCreate);
            return result?.Succeeded ?? false
                    ? Ok(result)
                    : BadRequest(result?.message);

        }

        [HttpPatch]
        public async Task<IActionResult> UpdateAsync(PersonUpdateCommand commandUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await personRepo.PutAsync(commandUpdate);
            return result?.Succeeded ?? false
                    ? Ok(result)
                    : BadRequest(result?.message);

        }
    }
}
