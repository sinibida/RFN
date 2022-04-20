using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Rfn.App.Properties;

namespace Rfn.App.Commands
{
    public class OpenUriCommand : RfnCommand
    {
        //https://stackoverflow.com/questions/4989106/string-format-count-the-number-of-expected-args
        public static int GetFormatArgCount(string format)
        {
            var matches = Regex.Matches(format, @"(?<!\{)\{([0-9]+).*?\}(?!})");
            if (matches.Count > 0)
                return matches.Cast<Match>().Max(m => int.Parse(m.Groups[1].Value)) + 1;
            return 0;
        }

        public static string[] PopulateArgs(string[] args, int count)
        {
            string[] realArgs;
            if (count == 0)
                return new string[] { };

            if (args.Length < count)
                throw new RfnCommandExecutionException(
                    string.Format(Resources.UriCommand_Exception_NotEnoughArgs_Text, count, count == 1 ? "" : "s"),
                    Resources.UriCommand_Exception_NotEnoughArgs_Caption);

            if (args.Length > count)
            {
                realArgs = new string[count];
                var lastArg = "";
                for (var i = 0; i < args.Length; i++)
                {
                    if (i < count - 1)
                        realArgs[i] = args[i];
                    else
                    {
                        if (lastArg.Length != 0)
                            lastArg += " ";

                        lastArg += args[i];
                    }
                }

                realArgs[count - 1] = lastArg;
            }
            else
            {
                realArgs = args;
            }

            return realArgs;
        }

        public OpenUriCommandProperties Properties { get; set; }

        public override void Execute(string[] args)
        {
            var count = Properties.UriFormatArgCount < 0
                ? GetFormatArgCount(Properties.UriFormat)
                : Properties.UriFormatArgCount;
            var realArgs = PopulateArgs(args, count);
            realArgs = realArgs.Select(x => HttpUtility.UrlEncode(x)).ToArray();
            var uri = string.Format(Properties.UriFormat, realArgs);
            var procPath = RfnSession.Instance.Config.WebBrowserPath;
            Process.Start(procPath, $"\"{uri}\"");
        }

        public override Type GetPropertyType()
        {
            return typeof(OpenUriCommandProperties);
        }

        public override void SetProperty(object prop)
        {
            Properties = (OpenUriCommandProperties) prop;
        }
    }
}