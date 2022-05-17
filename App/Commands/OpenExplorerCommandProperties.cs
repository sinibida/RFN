using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;
using Newtonsoft.Json;

namespace Rfn.App.Commands
{
    public struct OpenExplorerCommandProperties
    {
        [DefaultValue("")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public string Path;
    }
}