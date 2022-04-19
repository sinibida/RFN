namespace Rfn.App.InputBoxes
{
    public abstract class WordInputBox : IRfnInputBox
    {
        public virtual double GetProbability(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new InputValueEmptyException();

            var all = 0.0;
            var pass = 0.0;
            foreach (var c in value)
            {
                if (char.IsWhiteSpace(c))
                    return 0.0;

                pass += CheckChar(c);
                all += 1.0;
            }

            return pass / all;
        }

        public abstract double CheckChar(char c);

        public abstract string GetKey();
        public abstract int GetOrder();
    }
}