using Rfn.App.Properties;
using System;
using System.Collections.Generic;
using System.Media;
using System.Windows.Forms;

namespace Rfn.App
{
    public partial class InputForm : Form
    {
        public string Input { get; private set; }
        public List<string> InputHistory { get; set; }

        private int _historyIndex = 0;

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

            _historyIndex = InputHistory.Count;

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
                case Keys.Escape:
                    Close();
                    break;
                case Keys.Up:
                    if (_historyIndex > 0)
                        _historyIndex--;
                    else
                        SystemSounds.Exclamation.Play();
                    inputTextBox.Text = LoadHistory(_historyIndex);
                    break;
                case Keys.Down:
                    if (_historyIndex < InputHistory.Count)
                        _historyIndex++;
                    else
                        SystemSounds.Exclamation.Play();
                    inputTextBox.Text = LoadHistory(_historyIndex);
                    break;
            }
        }

        private string LoadHistory(int index)
        {
            return index == InputHistory.Count ? string.Empty : InputHistory[index];
        }

        private void Run()
        {
            Input = inputTextBox.Text;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
