using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Net;

namespace Api.Controllers
{
    public class MapExceptionAttribute : ExceptionFilterAttribute
    {
        private readonly Type _exceptionType;
        private readonly HttpStatusCode _statusCode;

        public MapExceptionAttribute(Type exceptionType, HttpStatusCode statusCode)
        {
            _exceptionType = exceptionType;
            _statusCode = statusCode;
        }

        public override void OnException(ExceptionContext context)
        {
            if (context.Exception.GetType() == _exceptionType)
            {
                var errorType = context.Exception.GetType();

                context.Result = new JsonResult(new
                {
                    errorType = errorType.ToString().Split(".").Last(),
                    errorMessage = context.Exception.Message
                });

                context.HttpContext.Response.StatusCode = (int)_statusCode;
                context.ExceptionHandled = true;
            }
        }
    }
}