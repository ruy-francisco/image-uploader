using System;

namespace Core.Exceptions
{
    public class CustomUploadException : Exception
    {
        public CustomUploadException(string message)
            : base(message)
        {
            
        }
    }
}