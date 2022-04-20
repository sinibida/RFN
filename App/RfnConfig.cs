using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;

namespace Rfn.App
{
    public struct RfnConfig
    {
        public string WebBrowserPath;
        public string LuaInputBoxDir;
        public string LuaCommandDir;
    }
}
