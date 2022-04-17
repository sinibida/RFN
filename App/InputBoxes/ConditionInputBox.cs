using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rfn.App.InputBoxes
{
    public abstract class ConditionInputBox : IRfnInputBox
    {
        public double GetProbability(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new InputValueEmptyException();

            return CheckCondition(value) ? 1.0 : 0.0;
        }
        
        public abstract bool CheckCondition(string value);
        public abstract string GetKey();
    }
}
