using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VoidLight.Infrastructure.Common.Exceptions;

namespace VoidLight.Web.Infrastructure
{
    public class ExceptionProblemDetails: ProblemDetails
    {
        public ExceptionProblemDetails(ApiExceptionBase apiExceptionBase)
        {
            Detail = apiExceptionBase.Message;
            Status = (int)apiExceptionBase.Code;
            Title = apiExceptionBase.Title;
        }
    }
}
