using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Rfn.App.Commands;
using Rfn.App.InputBoxes;
using Rfn.App.Properties;

namespace Rfn.App
{
    public class RfnComputer
    {
        public RfnInputBoxList InputBoxes { get; set; }
        public RfnCommandList Commands { get; set; }

        public RfnComputer()
        {
            InputBoxes = new RfnInputBoxList();
        }

        public RfnExecuteData Compute(string input)
        {
            RfnExecuteData dat;
            if (input.StartsWith(":"))
            {
                dat = RfnExecuteDataFromCommand(input);
                if (dat != null)
                    return dat;
            }

            if (!input.Contains(';')) return RfnExecuteDataFromBody(input);

            dat = RfnExecuteDataFromTagAndBody(input);
            return dat ?? RfnExecuteDataFromBody(input);
        }

        public RfnExecuteData RfnExecuteDataFromTagAndBody(string input)
        {
            var split = input.Split(';');

            string[] args;

            var cmd = GetCommandFromAlias(split.First());
            if (cmd != null)
            {
                args = split.Length == 2 && string.IsNullOrEmpty(split[1])
                    ? Array.Empty<string>()
                    : string.Join(";", split.Skip(1)).Split(' ');
                return new RfnExecuteData(cmd, args);
            }

            cmd = GetCommandFromAlias(split.Last());
            if (cmd == null) return null;

            args = split.Length == 2 && string.IsNullOrEmpty(split[0])
                ? Array.Empty<string>()
                : string.Join(";", split.Take(split.Length - 1)).Split(' ');
            return new RfnExecuteData(cmd, args);
        }

        public RfnExecuteData RfnExecuteDataFromBody(string input)
        {
            var cmdAlias = GetTagFromBody(input);
            var cmd = GetCommandFromAlias(cmdAlias);
            if (cmd == null)
                throw new ArgumentException($"The command with alias {cmdAlias} does not exist.");
            return new RfnExecuteData(cmd, input.Split(' '));
        }

        public RfnExecuteData RfnExecuteDataFromCommand(string input)
        {
            var split = input.Substring(1).Split(' ');
            if (split.Length == 0)
                return null;
            var args = split.Skip(1).ToArray();
            var cmdAlias = split[0].ToLower();

            var cmd = GetCommandFromAlias(cmdAlias);
            if (cmd == null)
                throw new ArgumentException($"The command with alias {cmdAlias} does not exist.");

            return new RfnExecuteData(cmd, args);
        }

        public RfnCommand GetCommandFromAlias(string cmdAlias)
        {
            return Commands.GetCommandFromAlias(cmdAlias);
        }

        public string GetTagFromBody(string body)
        {
            try
            {
                return InputBoxes.SelectBoxFromInput(body).GetKey();
            }
            catch (InputBoxException e)
            {
                MessageBox.Show(
                    string.Format(Resources.RfnComputer_InputBoxException_Text,
                        e.Box.GetType().Name,
                        e.UserMessage),
                    e.UserCaption,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return null;
            }
        }
    }
}