using System;
namespace Yatter.Storage.Azure.Exceptions
{
    public class ContainerDoesNotExistException : Exception
    {
        public ContainerDoesNotExistException(string message) : base(message) { }
    }
}

