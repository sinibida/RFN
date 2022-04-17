using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rfn.App.InputBoxes
{
    public interface IRfnInputBox
    {
        /// <summary>
        /// Returns the probability that the value stored in this box is in correct type.
        /// </summary>
        /// <returns>the probability that the value is in the input type. (0.0 ~ 1.0)</returns>
        double GetProbability(string value);

        /// <summary>
        /// Returns the key of the type. Used as a key of the command.
        /// </summary>
        /// <returns>The key</returns>
        string GetKey();
    }
}
