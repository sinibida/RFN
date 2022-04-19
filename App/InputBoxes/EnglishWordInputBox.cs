using System.Windows.Forms;

namespace Rfn.App.InputBoxes
{
    public class EnglishWordInputBox : WordInputBox
    {
        public override double CheckChar(char c)
        {
            if ('a' <= c && c <= 'z' || 'A' <= c && c <= 'Z') 
            {
                return 1.0;
            }

            return 0.0;
        }

        public override string GetKey()
        {
            return "__eword";
        }

        public override int GetOrder()
        {
            return 2000;
        }
    }
}