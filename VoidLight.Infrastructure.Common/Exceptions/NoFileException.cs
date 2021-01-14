﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace VoidLight.Infrastructure.Common.Exceptions
{
    public class NoFileException : ApiExceptionBase
    {
        public NoFileException() : base(HttpStatusCode.BadRequest, Constants.AUTHENTICATION_EXCEPTION)
        {
        }

        public NoFileException(string message) : base(HttpStatusCode.BadRequest, message, Constants.AUTHENTICATION_EXCEPTION)
        {
        }

        public NoFileException(string message, Exception innerException) : base(HttpStatusCode.BadRequest, message, innerException, Constants.AUTHENTICATION_EXCEPTION)
        {
        }
    }
}
