using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Net;

namespace Api.Controllers
{
    [AttributeUsage(AttributeTargets.Method)]
    public class MapExceptionAttribute : ExceptionFilterAttribute
    {
        private readonly Type[] _exceptionTypes;
        private readonly HttpStatusCode[] _statusCodes;

        public MapExceptionAttribute(Type[] exceptionTypes, HttpStatusCode[] statusCodes)
        {
            if (exceptionTypes != null && statusCodes != null && exceptionTypes.Length == statusCodes.Length)
            {
                //Intended usage: exceptionType[i] should correspond to statusCode[i]
                _exceptionTypes = exceptionTypes;
                _statusCodes = statusCodes;
            }
            else
            {
                //Prevent errors from occuring in OnException(...)
                _exceptionTypes = Array.Empty<Type>();
                _statusCodes = Array.Empty<HttpStatusCode>();
            }
        }

        public override void OnException(ExceptionContext context)
        {
            for (int i = 0; i < _exceptionTypes.Length; i++)
            {
                if (context.Exception.GetType() == _exceptionTypes[i])
                {
                    var errorType = context.Exception.GetType().ToString().Split(".").Last();
                    context.HttpContext.Response.StatusCode = (int)_statusCodes[i];
                    context.Result = new JsonResult(new
                    {
                        errorType = errorType,
                        errorMessage = context.Exception.Message
                    });

                    context.ExceptionHandled = true;

                    break;
                }
            }
        }
    }
}