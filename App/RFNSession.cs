using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rfn.App.Commands;
using Rfn.App.InputBoxes;
using Rfn.App.Properties;

namespace Rfn.App
{
    public class RfnSession : IDisposable
    {
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        public static RfnSession Instance { get; private set; }

        public RfnConfig Config { get; private set; }

        private KeyHandleForm _keyHandleForm;
        private NotifyIcon _notifyIcon;
        private RfnComputer _computer;
        private RfnExecutor _executor;

        private List<string> _inputHistory = new List<string>();

        public RfnSession()
        {
            if (Instance == null)
                Instance = this;
            else
                throw new Exception("RfnSession is singleton class.");
        }

        public void Dispose()
        {
            _notifyIcon?.Dispose();
            Instance = null;
        }

        public void Begin()
        {
            Load();
            _keyHandleForm.ShowDialog();
        }

        public void TryQuit()
        {
            if (!AskClose()) return;

            _notifyIcon?.Dispose();
            _keyHandleForm.Close();
        }

        private static bool AskClose()
        {
            var response = MessageBox.Show(
                Resources.MsgBox_Stop_RFN_Text,
                Resources.MsgBox_Stop_RFN_Caption,
                MessageBoxButtons.YesNo);
            return response == DialogResult.Yes;
        }

        private void Load()
        {
            LoadNotifyIcon();
            LoadKeyHandleForm();
            LoadComputer();
            LoadConfigs();
            //TODO load config.json

            _executor = new RfnExecutor();
        }

        public void LoadConfigs()
        {
            var cmdText = File.ReadAllText("commands.json", Encoding.UTF8);
            var loader = new CommandJsonLoader(new Dictionary<string, Type>()
            {
                {"openUri", typeof(OpenUriCommand)},
                {"tryQuit", typeof(TryQuitCommand)},
                {"reloadConfigs", typeof(ReloadConfigsCommand)},
            });
            _computer.Commands = new RfnCommandList(loader.JsonStringToCommands(cmdText));
            var configText = File.ReadAllText("config.json", Encoding.UTF8);
            Config = JObject.Parse(configText).ToObject<RfnConfig>();
        }

        private void LoadComputer()
        {
            _computer = new RfnComputer();
            _computer.InputBoxes.Add(new EquationInputBox());
            _computer.InputBoxes.Add(new UriInputBox());
            _computer.InputBoxes.Add(new SentenceInputBox());
            _computer.InputBoxes.Add(new EnglishWordInputBox());
            _computer.InputBoxes.Add(new KoreanWordInputBox());
        }

        private void LoadNotifyIcon()
        {
            _notifyIcon = new NotifyIcon();
            _notifyIcon.Text = Resources.NotifyIcon_Text;
#if DEBUG
            _notifyIcon.Text += " [DEBUG]";
#endif
            _notifyIcon.Icon = Resources.TrayIcon;
            _notifyIcon.MouseClick += NotifyIcon_MouseClick;
            _notifyIcon.Visible = true;
        }

        private void LoadKeyHandleForm()
        {
            _keyHandleForm = new KeyHandleForm();
            _keyHandleForm.KeyPressed += KeyHandleForm_KeyPressed;
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
            var process = Process.GetCurrentProcess();
            SetForegroundWindow(process.MainWindowHandle);
            var inputForm = new InputForm
            {
                InputHistory = _inputHistory
            };
            var result = inputForm.ShowDialog();

            if (result != DialogResult.OK) return;

            var input = inputForm.Input;
            if (!string.IsNullOrEmpty(input))
                _inputHistory.Add(input);

            RfnExecuteData data;
            try
            {
                data = _computer.Compute(input);
            }
            catch (Exception e)
            {
                MessageBox.Show(string.Format(Resources.ComputingException_MsgBox_Text, e.Message),
                    Resources.ComputingException_MsgBox_Caption);
                return;
            }
            _executor.Run(data);
        }
    }
}