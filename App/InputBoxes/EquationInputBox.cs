using System.Linq;

namespace Rfn.App.InputBoxes
{
    public class EquationInputBox : IRfnInputBox
    {
        private static readonly string[] WolframAlphaIgnore =
        {
            "derivative of",
            "derivative",
            "derivation",
            "d/dx",
            "arcsin",
            "arccos",
            "arctan",
            "sinh",
            "cosh",
            "tanh",
            "sin",
            "cos",
            "tan",
            "x",
            "e",
        };
        
        public bool WolframAlpha { get; set; }

        public EquationInputBox(bool wolframAlpha)
        {
            WolframAlpha = wolframAlpha;
        }

        public EquationInputBox() : this(true)
        {

        }

        public double GetProbability(string value)
        {
            var formattedInput = TidyStringForProbFunction(value);

            if (formattedInput.Length == 0)
                return 1;

            string ignoredInput;
            var add = 0;

            if (WolframAlpha)
            {
                ignoredInput = value.ToLower();
                foreach (var s in WolframAlphaIgnore)
                {
                    ignoredInput = ignoredInput.Replace(s, "");
                }
                add = value.Length - ignoredInput.Length;
                ignoredInput = TidyStringForProbFunction(ignoredInput);
            }
            else
            {
                ignoredInput = formattedInput;
            }

            return (double)(ignoredInput.Count(c =>
                '0' <= c && c <= '9' ||
                c == '+' || c == '-' ||
                c == '*' || c == '/' ||
                c == '^' ||
                c == '(' || c == ')'
            ) + add) / formattedInput.Length;
        }

        private static string TidyStringForProbFunction(string input)
        {
            return new string(input.ToLower().Where(c => !char.IsWhiteSpace(c)).ToArray());
        }

        public string GetKey()
        {
            return "__eq";
        }
    }
}