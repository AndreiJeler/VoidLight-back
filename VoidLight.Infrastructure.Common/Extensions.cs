using System;
using System.Collections.Generic;
using System.Text;

namespace VoidLight.Infrastructure.Common
{
    public static class Extensions
    {
        public static bool IsSystemException(this Exception exception)
        {
            return (exception.GetType().FullName.StartsWith("System."));
        }
    }
}
