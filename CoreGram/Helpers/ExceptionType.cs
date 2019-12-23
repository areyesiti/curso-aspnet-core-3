using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreGram.Helpers
{
    /// <summary>
    /// Excepciones personalizadas utilizadas en el middleware de excepciones
    /// </summary>
    public class BadRequestException : Exception
    {
        public BadRequestException(string msg) : base(msg) { }
    }

    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string msg) : base(msg) { }
    }

    public class NotFoundException : Exception
    {
        public NotFoundException(string msg) : base(msg) { }
    }

    public class NotAlloedException : Exception
    {
        public NotAlloedException(string msg) : base(msg) { }
    }

    public class UnprocessableEntityException : Exception
    {
        public UnprocessableEntityException(string msg) : base(msg) { }
    }
}
