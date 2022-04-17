using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using Rfn.App.Properties;

namespace Rfn.App
{
    public class RfnExecutor
    {
        public void Run(RfnExecuteData dat)
        {
            try
            {
                dat.Command.Execute(dat.Args);
            }
            catch (RfnCommandExecutionException e)
            {
                MessageBox.Show(e.UserMessage,
                    e.UserCaption,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
