using System;
namespace Yatter.Storage.Azure
{
    public class ContainerDoesNotExistException : Exception
    {
        public ContainerDoesNotExistException(string message) : base(message) { }
    }
}

