using System;

namespace Rfn.App.InputBoxes
{
    public class InputBoxException : Exception
    {
        public string UserMessage { get; }
        public string UserCaption { get; }

        public InputBoxException(string userMessage) : base(userMessage)
        {
            UserMessage = userMessage;
        }

        public InputBoxException(string userMessage, string userCaption) : base(userMessage)
        {
            UserMessage = userMessage;
            UserCaption = userCaption;
        }
    }
}
