
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Utils.Exception
{
    public class ExceptionHandlerFilter : IExceptionFilter, IFilterMetadata
    {

        public void OnException(ExceptionContext context)
        {
            //Message Result
            var Result = ExceptionHandler.CreateErrorResult(context.Exception);
            if (context.HttpContext.Response.StatusCode >= 400)
                Result.tittle = "Aplication Error";
           
            Result.status = context.HttpContext.Response.StatusCode.ToString();
            context.Result = new ObjectResult(Result)
                                            {
                                              StatusCode = (int)HttpStatusCode.BadRequest
                                            };
           
        }
    }
}
