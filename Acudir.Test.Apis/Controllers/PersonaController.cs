using Acudir.Test.Apis.Domain;
using Acudir.Test.Apis.DTOs;
using Acudir.Test.Apis.Helpers;
using Acudir.Test.Apis.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Acudir.Test.Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonaController : ControllerBase
    {
        private readonly IPersonaRepository _repository;

        public PersonaController(IPersonaRepository repository)
        {
            _repository = repository;
        }

        // Get All
        [HttpGet("GetAll")]
        public async Task<ActionResult<List<PersonaDTO>>> GetAll([FromQuery] PersonaFilters filters)
        {
            var response = await _repository.GetAllPersonas(filters);
            if (response.Any())
            {
                return Ok(response);
            }
            return NotFound();
        }

        // Post
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] List<PersonaForAddDTO> personasDto)
        {
            if (personasDto == null || !personasDto.Any())
            {
                return BadRequest("La lista de personas no puede ser nula o estar vacía.");
            }

            var response = await _repository.AddPersonas(personasDto);
            if (response.Any())
            {
                return Ok(response);
            }
            return StatusCode(500, "Error al agregar personas.");
        }

        // Put
        [HttpPut("{nombreCompleto}")]
        public async Task<IActionResult> Put(string nombreCompleto, [FromBody] PersonaForEditDTO persona)
        {
            if (string.IsNullOrEmpty(nombreCompleto))
            {
                return BadRequest("El parámetro 'nombreCompleto' no puede estar vacío.");
            }

            if (persona == null)
            {
                return BadRequest("La persona no puede ser nula.");
            }

            try
            {
                var response = await _repository.EditPersona(nombreCompleto, persona);
                if (response != null)
                {
                    return Ok(response);
                }
                return NotFound($"Persona '{nombreCompleto}' no encontrada.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno al actualizar la persona.");
            }
        }

        // Delete
        [HttpDelete("{nombreCompleto}")]
        public async Task<IActionResult> Delete(string nombreCompleto)
        {
            if (string.IsNullOrEmpty(nombreCompleto))
            {
                return BadRequest("El parámetro 'nombreCompleto' no puede estar vacío.");
            }

            try
            {
                var response = await _repository.DeletePersona(nombreCompleto);
                if (response != null)
                {
                    return Ok(response);
                }
                return NotFound("Persona no encontrada.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno al eliminar la persona.");
            }
        }
    }
}
