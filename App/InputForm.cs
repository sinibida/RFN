using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Rfn.App.Properties;

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
            Text = Resources.InputForm_Title;
            enterCommandLabel.Text = Resources.InputForm_EnterCommand;
            runButton.Text = Resources.InputForm_Run;
#if DEBUG
            Text += " [DEBUG]";
#endif
            Activate();
            inputTextBox.Focus();
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
