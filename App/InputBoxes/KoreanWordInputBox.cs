using System.Windows.Forms;

namespace Rfn.App.InputBoxes
{
    public class KoreanWordInputBox : WordInputBox
    {
        public override double CheckChar(char c)
        {
            if ('가' <= c && c <= '힣')
            {
                return 1.0;
            }

            return 0.0;
        }

        public override string GetKey()
        {
            return "__kword";
        }
    }
}