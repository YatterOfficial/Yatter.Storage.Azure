using System;
namespace Yatter.Storage.Azure.Exceptions
{
    public class BlobExistsException : Exception
    {
        public BlobExistsException(string message) : base(message) { }
    }
}

