using System;
namespace Yatter.Storage.Azure
{
    public class MissingConnectionStringException : Exception
    {
        public MissingConnectionStringException(string message) : base(message) { }
    }
}

