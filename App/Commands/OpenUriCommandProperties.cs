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
    public struct OpenUriCommandProperties
    {
        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public int UriFormatArgCount;

        [DefaultValue("https://www.google.com/")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public string UriFormat;
    }
}