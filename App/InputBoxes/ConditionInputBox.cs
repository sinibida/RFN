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
                return OnNullOrEmpty(value);

            return CheckCondition(value) ? 1.0 : 0.0;
        }

        protected virtual double OnNullOrEmpty(string value) => 0.0;

        public abstract bool CheckCondition(string value);
        public abstract string GetKey();
        public abstract int GetOrder();
    }
}
