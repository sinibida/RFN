using System;

namespace Rfn.App.InputBoxes
{
    public class InputBoxException : Exception
    {
        public IRfnInputBox Box { get; }
        public string UserMessage { get; }
        public string UserCaption { get; }

        public InputBoxException(IRfnInputBox box, string userMessage) : base(userMessage)
        {
            Box = box;
            UserMessage = userMessage;
        }

        public InputBoxException(IRfnInputBox box, string userMessage, string userCaption) : base(userMessage)
        {
            Box = box;
            UserMessage = userMessage;
            UserCaption = userCaption;
        }
    }
}
