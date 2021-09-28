using System;
namespace Yatter.Storage.Azure.Exceptions
{
    public class BlobUploadException : Exception
    {
        public BlobUploadException(string message) : base(message) { }
    }
}
