using Acudir.Test.Apis.Extra.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Acudir.Test.Apis.Extra.Responses
{
    public class CustomValidationResponse : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {

                var error = context.ModelState.Values.Where(v => v.Errors.Count > 0)
                        .First().Errors
                        .First().ErrorMessage;

                throw new BusinessException(error);

            }
        }
    }
}
