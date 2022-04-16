using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rfn.App
{
    public partial class InputForm : Form
    {
        public string Input { get; private set; }

        public InputForm()
        {
            InitializeComponent();

        }

        private void InputForm_Load(object sender, EventArgs e)
        {
#if DEBUG
            Text = "RFN [DEBUG]";
#endif
        }

        private void runButton_Click(object sender, EventArgs e)
        {
            Run();
        }

        private void inputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    Run();
                    break;
                case Keys.Escape:
                    Close();
                    break;
            }
        }

        private void Run()
        {
            Input = inputTextBox.Text;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
