using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Rfn.App
{
    [DesignerCategory("")]
    public class KeyHandleForm : Form
    {
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);
        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        const int MYACTION_HOTKEY_ID = 1;

        public event EventHandler KeyPressed;

        public KeyHandleForm()
        {
            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;
            Load += OnLoad;
            RegisterHotKey(Handle, MYACTION_HOTKEY_ID, 7, (int)Keys.F12);
        }

        private void OnLoad(object sender, EventArgs e)
        {
            Size = Size.Empty;
        }

        // protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        // {
        //     if (keyData.HasFlag(Keys.F12 | Keys.Control | Keys.Shift | Keys.Alt))
        //             KeyPressed?.Invoke(this, EventArgs.Empty);
        //
        //     return base.ProcessCmdKey(ref msg, keyData);
        // }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0312 && m.WParam.ToInt32() == MYACTION_HOTKEY_ID)
            {
                KeyPressed?.Invoke(this, EventArgs.Empty);
            }
            base.WndProc(ref m);
        }
    }
}