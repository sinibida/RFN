using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Rfn.App.Properties;

namespace Rfn.App.Commands
{
    public class OpenExplorerCommand : RfnCommand
    {
        public OpenExplorerCommandProperties Properties { get; set; }

        public override void Execute(string[] args)
        {
            var path = Properties.Path == "" ? Directory.GetCurrentDirectory() : Properties.Path;
            path = Path.GetFullPath(Environment.ExpandEnvironmentVariables(path));
            var attributes = File.GetAttributes(path);
            var procArgs = (attributes.HasFlag(FileAttributes.Directory) ? $"/open," : "/select,") + $" \"{path}\"";
            Process.Start("explorer.exe", procArgs);
        }

        public override Type GetPropertyType()
        {
            return typeof(OpenExplorerCommandProperties);
        }

        public override void SetProperty(object prop)
        {
            Properties = (OpenExplorerCommandProperties) prop;
        }
    }
}