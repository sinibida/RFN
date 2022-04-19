using System;

namespace Rfn.App.Commands
{
    public class RfnCommandExecutionException : Exception
    {
        public string UserMessage { get; }
        public string UserCaption { get; }

        public RfnCommandExecutionException(string userMessage) : base(userMessage)
        {
            UserMessage = userMessage;
        }

        public RfnCommandExecutionException(string userMessage, string userCaption) : base(userMessage)
        {
            UserMessage = userMessage;
            UserCaption = userCaption;
        }
    }
}
