﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Rfn.App.Properties;

namespace Rfn.App.Commands
{
    public class ReloadConfigsCommand : RfnCommand
    {
        public ReloadConfigsCommandSettings Properties { get; set; }

        public override void Execute(string[] args)
        {
            RfnSession.Instance.Reload();
            MessageBox.Show(Resources.ReloadConfigsCommand_MsgBox_Text, Resources.ReloadConfigsCommand_MsgBox_Caption);
        }

        public override Type GetPropertyType()
        {
            return typeof(ReloadConfigsCommandSettings);
        }

        public override void SetProperty(object prop)
        {
            Properties = (ReloadConfigsCommandSettings)prop;
        }
    }
}
