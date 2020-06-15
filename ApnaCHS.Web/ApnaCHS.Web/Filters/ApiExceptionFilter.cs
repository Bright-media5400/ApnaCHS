using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace ApnaCHS.Web.Filters
{
    public class ApiExceptionHandler : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            var ex = context.Exception;
            context.Response = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(ex.Message)
            };
        }
    }
}