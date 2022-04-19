using System;
using System.Collections.Generic;

namespace Rfn.App.InputBoxes
{
    public class RfnInputBoxList : List<IRfnInputBox>
    {
        public IRfnInputBox SelectBoxFromInput(string body)
        {
            try
            {

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            IRfnInputBox foundBox = null;
            var prob = -1.0;
            foreach (var box in this)
            {
                var p = box.GetProbability(body);
                if (p < 0 || p <= prob) continue;

                foundBox = box;
                prob = p;
            }

            return foundBox;
        }
    }
}