using System;
namespace Yatter.Storage.Azure.Exceptions
{
    public class ContainerNameNotSpecifiedException : Exception
    {
        public ContainerNameNotSpecifiedException(string message) : base(message) { }
    }
}
