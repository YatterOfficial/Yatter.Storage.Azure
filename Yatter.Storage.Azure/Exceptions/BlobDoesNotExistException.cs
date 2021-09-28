using System;
namespace Yatter.Storage.Azure.Exceptions
{
    public class BlobDoesNotExistException : Exception
    {
        public BlobDoesNotExistException(string message) : base(message) { }
    }
}

