using System;
namespace Yatter.Storage.Azure.Exceptions
{
    public class MissingConnectionStringException : Exception
    {
        public MissingConnectionStringException(string message) : base(message) { }
    }
}

