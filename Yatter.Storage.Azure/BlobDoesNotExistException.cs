using System;
namespace Yatter.Storage.Azure
{
    public class BlobDoesNotExistException : Exception
    {
        public BlobDoesNotExistException(string message) : base(message) { }
    }
}

