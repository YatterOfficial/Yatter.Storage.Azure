using System;
namespace Yatter.Storage.Azure.Exceptions
{
    public class NullRequestObjectException : Exception
    {
        public NullRequestObjectException(string message) : base(message) { }

        public NullRequestObjectException(string message, Exception inner) : base(message, inner) { }
    }
}

