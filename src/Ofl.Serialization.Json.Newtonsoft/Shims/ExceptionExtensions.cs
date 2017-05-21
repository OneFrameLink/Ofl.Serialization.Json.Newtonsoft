using System;
using Ofl.Cloning;

namespace Ofl.Serialization.Json.Newtonsoft.Shims
{
    internal static class ExceptionExtensions
    {
        internal static ExceptionShim ToExceptionShim(this Exception exception) =>
            exception?.CloneProperties<Exception, ExceptionShim>();
    }
}
