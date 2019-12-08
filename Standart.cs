using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace compliance
{
    public class Standart
    {
        [JsonProperty("category")]
        public string Name { get; set; }

        [JsonProperty("masks")]
        public IList<Template> Conditions { get; set; }
    }
}
