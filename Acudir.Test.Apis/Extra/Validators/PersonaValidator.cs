using Acudir.Test.Apis.Dominio.DTOs;
using FluentValidation;

namespace Acudir.Test.Apis.Extra.Validators
{
    public class PersonaValidator : AbstractValidator<PersonaDTO_Post>
    {
        public PersonaValidator()
        {
            RuleFor(p => p.Edad)
                .GreaterThan(0).WithMessage("La edad debe ser mayor que cero.");
        }
    }

    public class PersonaListValidator : AbstractValidator<List<PersonaDTO_Post>>
    {
        public PersonaListValidator()
        {
            RuleForEach(persona => persona)
                .SetValidator(new PersonaValidator());

        }
    }
}
