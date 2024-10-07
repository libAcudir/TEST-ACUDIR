using Microsoft.AspNetCore.Mvc;
using Acudir.Test.Apis.Models;
using NombreDelProyecto.Interfaces;

namespace Acudir.Test.Apis.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonaController : ControllerBase
    {
        private readonly IPersonaRepository _personaRepository;

        public PersonaController(IPersonaRepository personaRepository)
        {
            _personaRepository = personaRepository;
        }

        // Endpoint GET para obtener todas las personas, con filtros opcionales
        [HttpGet("GetAll")]
        public IActionResult GetAll([FromQuery] string nombre, [FromQuery] int? edad)
        {
            var personas = _personaRepository.GetAll(nombre, edad);
            return Ok(personas);
        }

        // Endpoint POST para agregar una nueva persona
        [HttpPost]
        public IActionResult Add([FromBody] Persona nuevaPersona)
        {
            _personaRepository.Add(nuevaPersona);
            return Ok();
        }

        // Endpoint PUT para modificar una persona existente
        [HttpPut]
        public IActionResult Update([FromBody] Persona personaModificada)
        {
            _personaRepository.Update(personaModificada);
            return Ok();
        }
    }
}