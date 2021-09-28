using System;
namespace Yatter.Storage.Azure.Exceptions
{
    public class BlobPathNotSpecifiedException : Exception
    {
        public BlobPathNotSpecifiedException(string message) : base(message) { }
    }
}
