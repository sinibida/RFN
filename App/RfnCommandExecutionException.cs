using System;
using System.Runtime.Serialization;

namespace Rfn.App
{
    public class RfnCommandExecutionException : Exception
    {
        public string UserMessage { get; }
        public string UserCaption { get; }

        public RfnCommandExecutionException(string userMessage)
        {
            UserMessage = userMessage;
        }

        public RfnCommandExecutionException(string userMessage, string userCaption)
        {
            UserMessage = userMessage;
            UserCaption = userCaption;
        }
    }
}
