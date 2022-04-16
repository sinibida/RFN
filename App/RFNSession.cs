using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Rfn.App.Properties;

namespace Rfn.App
{
    public class RfnSession : IDisposable
    {
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        private KeyHandleForm _keyHandleForm;
        private NotifyIcon _notifyIcon;
        private RfnComputer _computer;
        private RfnExecutor _executor;

        public void Run()
        {
            Load();
            _keyHandleForm.ShowDialog();
        }

        private void Load()
        {
            LoadNotifyIcon();
            LoadKeyHandleForm();
            _computer = new RfnComputer();
            _executor = new RfnExecutor();
        }

        private void LoadNotifyIcon()
        {
            _notifyIcon = new NotifyIcon();
            _notifyIcon.Text = Resources.Program_Main_Notification_Text;
            _notifyIcon.Icon = Resources.TrayIcon;
            _notifyIcon.MouseClick += NotifyIcon_MouseClick;
            _notifyIcon.Visible = true;
        }

        private void LoadKeyHandleForm()
        {
            _keyHandleForm = new KeyHandleForm();
            _keyHandleForm.KeyPressed += KeyHandleForm_KeyPressed;
        }

        public void Dispose()
        {
            _notifyIcon?.Dispose();
        }

        private void KeyHandleForm_KeyPressed(object sender, EventArgs e)
        {
            ShowInputForm();
        }

        private void NotifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            ShowInputForm();
        }

        private void ShowInputForm()
        {
            Process process = Process.GetCurrentProcess();
            SetForegroundWindow(process.MainWindowHandle);
            var inputForm = new InputForm();
            var result = inputForm.ShowDialog();

            if (result == DialogResult.OK)
            {
                var data = _computer.Compute(inputForm.Input);
                _executor.Run(data);
            }
        }
    }
}
