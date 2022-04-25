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
using Rfn.App.InputBoxes.Lua;
using Rfn.App.Properties;

namespace Rfn.App
{
    //TODO refactor
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

        private readonly List<string> _inputHistory = new List<string>();

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

        public void Reload()
        {
            LoadConfigs();
            LoadComputer();
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
            LoadConfigs();
            LoadComputer();

            _executor = new RfnExecutor();
        }

        public void LoadConfigs()
        {
            RfnConfig config;
            using (WorkingDirBox.Push(GetAppDataPath()))
            {
                var configText = File.ReadAllText("config.json", Encoding.UTF8);
                config = JObject.Parse(configText).ToObject<RfnConfig>();
            }

            Config = config;
        }

        private void LoadComputer()
        {
            var computer = new RfnComputer();
            using (WorkingDirBox.Push(GetAppDataPath()))
            {
                computer.InputBoxes.Add(new EquationInputBox()); // 5000
                computer.InputBoxes.Add(new UriInputBox()); // 4000
                computer.InputBoxes.Add(new SentenceInputBox()); // 3000
                computer.InputBoxes.Add(new EnglishWordInputBox()); // 2000
                computer.InputBoxes.Add(new KoreanWordInputBox()); // 1000

                var luaInputBoxLoader = new LuaInputBoxJsonLoader();
                var luaBoxes = luaInputBoxLoader.LoadInputBoxes(Config.LuaInputBoxDir);
                computer.InputBoxes.AddRange(luaBoxes);

                var cmdLoader = new CommandJsonLoader(new Dictionary<string, Type>
                {
                    {"openUri", typeof(OpenUriCommand)},
                    {"tryQuit", typeof(TryQuitCommand)},
                    {"reloadConfigs", typeof(ReloadConfigsCommand)},
                });
                computer.Commands = new RfnCommandList(cmdLoader.JsonFileToCommands("commands.json"));
            }

            _computer = computer;
        }

        private void LoadNotifyIcon()
        {
            _notifyIcon = new NotifyIcon();
            _notifyIcon.Text = Resources.NotifyIcon_Text;
#if DEBUG
            _notifyIcon.Text += " [DEBUG]";
#endif
            _notifyIcon.Icon = Resources.TrayIcon;
            _notifyIcon.MouseClick += (o, args) => ShowInputForm();
            _notifyIcon.Visible = true;
        }

        private void LoadKeyHandleForm()
        {
            _keyHandleForm = new KeyHandleForm();
            _keyHandleForm.KeyPressed += (o, args) => ShowInputForm();
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

        private string GetAppDataPath() => 
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RFN");
    }
}