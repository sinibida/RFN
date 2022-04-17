using System;
using System.Runtime.Serialization;

namespace Rfn.App
{
    public class RfnCommandExecutionException : Exception
    {
        public string UserMessage { get; }

        public RfnCommandExecutionException(string userMessage)
        {
            UserMessage = userMessage;
        }
    }
}
