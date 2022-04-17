﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using Rfn.App.Properties;

namespace Rfn.App
{
    [DesignerCategory("")]
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            if (Environment.OSVersion.Version.Major >= 6)
                SetProcessDPIAware();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var session = new RfnSession();
            session.Begin();
        }

        [DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();
    }
}
