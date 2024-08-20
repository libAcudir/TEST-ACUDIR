namespace Acudir.Test.Apis.Controllers
{
    using Acudir.Test.Apis.Dominio.DTOs;
    using Acudir.Test.Apis.Extra.Exceptions;
    using Acudir.Test.Apis.Extra.Extensions;
    using Acudir.Test.Apis.Extra.QueryFilters;
    using Acudir.Test.Apis.Extra.Responses;
    using Acudir.Test.Apis.Interfaces.IRepositorys;
    using Acudir.Test.Apis.Models.DTOs;
    using Acudir.Test.Apis.Models.Entities;
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using System.Net;

    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IPersonaRepository _personaRepository;
        private readonly IMapper _mapper;

        public TestController(IPersonaRepository personaRepository, IMapper mapper)
        {
            _personaRepository = personaRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Devuelve un listado de personas con o sin filtros, no tiene en cuenta si es mayúscula o minúscula
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        [HttpGet(nameof(GetAll))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<PersonaDTO>>))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(CustomExceptionResponse))]
        public IActionResult GetAll([FromQuery] PersonaQueryFilter filters)
        {
            var personas = _personaRepository.GetAll();

            if (!string.IsNullOrEmpty(filters.NombreCompleto))
            {
                personas = personas.Where(p => p.NombreCompleto.IndexOf(filters.NombreCompleto, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            if (filters.Edad.HasValue)
            {
                personas = personas.Where(p => p.Edad == filters.Edad.Value);
            }

            if (!string.IsNullOrEmpty(filters.Domicilio))
            {
                personas = personas.Where(p => p.Domicilio.IndexOf(filters.Domicilio, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            if (!string.IsNullOrEmpty(filters.Telefono))
            {
                personas = personas.Where(p => p.Telefono.IndexOf(filters.Telefono, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            if (!string.IsNullOrEmpty(filters.Profesion))
            {
                personas = personas.Where(p => p.Profesion.IndexOf(filters.Profesion, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            IEnumerable<PersonaDTO> personaDTO_List = _mapper.Map<IEnumerable<PersonaDTO>>(personas);

            ApiResponse<IEnumerable<PersonaDTO>> response = new ApiResponse<IEnumerable<PersonaDTO>>(personaDTO_List);

            return Ok(response);
        }

        /// <summary>
        /// Devuelve una persona por id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet(nameof(GetById))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<PersonaDTO>))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(CustomExceptionResponse))]
        public IActionResult GetById(int id)
        {

            if (id <= 0)
                throw new BusinessException("Id inválido.");

            Persona persona = _personaRepository.GetById(id);

            if (persona == null)
                throw new BusinessException("La persona no existe.");

            PersonaDTO personaDTO = _mapper.Map<PersonaDTO>(persona);

            ApiResponse<PersonaDTO> response = new ApiResponse<PersonaDTO>(personaDTO);

            return Ok(response);
        }

        /// <summary>
        /// Guardar una o más personas, si una persona con las mismas props ya existe no la guarda, la lista no puede ser nula o sin registros y la edad debe ser mayor que 0
        /// </summary>
        /// <param name="personaDTO_List"></param>
        /// <returns></returns>
        [HttpPost(nameof(Post))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<bool>))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(CustomExceptionResponse))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(CustomExceptionResponse))]
        public IActionResult Post([FromBody] IEnumerable<PersonaDTO_Post> personaDTO_List)
        {

            if (personaDTO_List == null || personaDTO_List.Count() == 0)
                throw new BusinessException("Debe ingresar al menos una persona.");

            IEnumerable<Persona> personaDB_List = _mapper.Map<IEnumerable<Persona>>(personaDTO_List);

            IEnumerable<Persona> existingPersonas = _personaRepository.GetAll();

            IEnumerable<Persona> personasToInsert = personaDB_List
                .Where(newPersona => !existingPersonas.Any(existingPersona =>
                    existingPersona.NombreCompleto.Equals(newPersona.NombreCompleto, StringComparison.OrdinalIgnoreCase) &&
                    existingPersona.Edad == newPersona.Edad &&
                    existingPersona.Domicilio.Equals(newPersona.Domicilio, StringComparison.OrdinalIgnoreCase) &&
                    existingPersona.Telefono.Equals(newPersona.Telefono, StringComparison.OrdinalIgnoreCase) &&
                    existingPersona.Profesion.Equals(newPersona.Profesion, StringComparison.OrdinalIgnoreCase)))
                .ToList();

            int maxId = existingPersonas.Any() ? existingPersonas.Max(p => p.Id) : 0;

            if (personasToInsert.Count() > 0)
            {
                foreach (var persona in personasToInsert)
                {
                    persona.Id = ++maxId;
                }

                _personaRepository.AddRange(personasToInsert);
            }

            ApiResponse<bool> response = new ApiResponse<bool>(true);

            return Ok(response);
        }       

        /// <summary>
        /// Modifica un registro en caso de existir, según el objeto enviado
        /// </summary>
        /// <param name="id"></param>
        /// <param name="personaDTO"></param>
        /// <returns></returns>
        [HttpPut(nameof(Put))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<bool>))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(CustomExceptionResponse))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(CustomExceptionResponse))]
        public IActionResult Put(int id, [FromBody] PersonaDTO personaDTO)
        {
            Persona Updatedperson = _mapper.Map<Persona>(personaDTO);

            if (id == 0 || Updatedperson.Id == 0)
                throw new BusinessException("Id inválido.");

            if (id != Updatedperson.Id)
                throw new BusinessException("Los ids no coinciden.");

            var existingPersona = _personaRepository.GetById(id);

            if (existingPersona == null)
                throw new BusinessException("La persona que se desea modificar no existe.");

            existingPersona.UpdatePropertiesFrom(Updatedperson);

            _personaRepository.Update(Updatedperson);

            ApiResponse<bool> response = new ApiResponse<bool>(true);

            return Ok(response);
        }

        /// <summary>
        /// Elimina una persona según el id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete(nameof(Delete))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<bool>))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(CustomExceptionResponse))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(CustomExceptionResponse))]
        public IActionResult Delete(int id)
        {
            if (id <= 0)
                throw new BusinessException("Id inválido.");

            Persona existingPersona = _personaRepository.GetById(id);

            if (existingPersona == null)
                throw new BusinessException($"La persona con el id '{id}' no existe.");

            _personaRepository.Delete(existingPersona);

            ApiResponse<bool> response = new ApiResponse<bool>(true);

            return Ok(response);
        }
    }
}
