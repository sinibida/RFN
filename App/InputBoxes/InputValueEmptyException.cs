using System;

namespace Rfn.App.InputBoxes
{
    public class InputValueEmptyException : Exception
    {
        public InputValueEmptyException(string msg) : base(msg)
        {
        }

        public InputValueEmptyException() :
            this("This input box does not allow empty value.")
        {
        }
    }
}