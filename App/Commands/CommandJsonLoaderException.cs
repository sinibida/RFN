using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Rfn.App.Commands
{
    public class CommandJsonLoaderException : Exception
    {
        public CommandJsonLoaderException()
        {
        }

        public CommandJsonLoaderException(string message) : base(message)
        {
        }

        public CommandJsonLoaderException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public CommandJsonLoaderException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
