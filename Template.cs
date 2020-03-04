using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace compliance
{
    public class Template
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("mask")]
        public string Mask { get; set; }

        [JsonProperty("weight")]
        public int Weight { get; set; }

        [JsonProperty("descr")]
        public string Descr { get; set; }
    }
}
