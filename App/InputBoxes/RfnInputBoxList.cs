using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Rfn.App.InputBoxes
{
    public class RfnInputBoxList : List<IRfnInputBox>
    {
        public IRfnInputBox SelectBoxFromInput(string body)
        {
            IRfnInputBox foundBox = null;
            var prob = -1.0;
            foreach (var box in this)
            {
                var p = box.GetProbability(body);
                if (p < 0 ||
                    p <= prob ||
                    foundBox != null && p == prob && foundBox.GetOrder() > box.GetOrder())
                    continue;

                foundBox = box;
                prob = p;
            }

            return foundBox;
        }
    }
}