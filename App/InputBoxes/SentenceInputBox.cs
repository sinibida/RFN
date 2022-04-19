using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rfn.App.InputBoxes
{
    public class SentenceInputBox : ConditionInputBox
    {
        public override bool CheckCondition(string value)
        {
            return value.Any(char.IsWhiteSpace);
        }

        public override string GetKey()
        {
            return "__sentence";
        }

        public override int GetOrder()
        {
            return 3000;
        }
    }
}
