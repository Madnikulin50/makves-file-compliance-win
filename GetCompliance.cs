using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Management.Automation;
using System.Text.RegularExpressions;
using compliance;
using System.IO;

namespace compliance
{
  
    [Cmdlet(VerbsCommon.Get, "DocumentText")]
    [OutputType(typeof(String))]
    public class GetDocumentText : PSCmdlet
    {
        private string[] filesCollection;
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ValueFromPipeline = true, Position = 0, HelpMessage = "Texts for analysis")]
        public string[] File
        {
            get
            {
                return filesCollection;
            }

            set
            {
                filesCollection = value;
            }
        }

        protected String getText(string filename)
        {

            string text = "";
            try
            {
                try
                {
                    FilterReader reader = new FilterReader(filename);
                    using (reader)
                    {
                        text = reader.ReadToEnd();
                    }
                    if (text == null || text.Length == 0)
                    {
                        TikaReader r = new TikaReader();
                        text = r.getText(filename);
                    }
                }
                catch
                {
                    TikaReader r = new TikaReader();
                    text = r.getText(filename);
                }
            }
            catch
            {
            }
            return text;
        }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }

        protected override void ProcessRecord()
        {
            foreach (string file in filesCollection)
            {
                string text = getText(file);
                WriteObject(text);
            }
        }
    }

    public class Match
    {
        public string Name { get; set; }
        public string Source { get; set; }
        public float Weight { get; set; }
        public string Template { get; set; }
        public string Mask { get; set; }
    }

    [Cmdlet(VerbsCommon.Get, "Compliance")]
    [OutputType(typeof(Match))]
    public class GetCompliance: PSCmdlet
    {
        private string[] textCollection = new string[0];
        private string[] fileCollection = new string[0];
        private string[] kbCollection = new string[0];

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, ValueFromPipeline = true, Position = 0, HelpMessage = "Texts for analysis")]
        public string[] Text
        {
            get
            {
                return textCollection;
            }

            set
            {
                textCollection = value;
            }
        }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, ValueFromPipeline = true, Position = 1, HelpMessage = "Files for analysis")]
        public string[] File
        {
            get
            {
                return fileCollection;
            }

            set
            {
                fileCollection = value;
            }
        }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, ValueFromPipeline = true, Position = 2, HelpMessage = "Knowladge base for analysis")]
        public string[] KB
        {
            get
            {
                return kbCollection;
            }

            set
            {
                kbCollection = value;
            }
        }

        public IList<Standart> Standarts { get; set; }

        protected override void BeginProcessing()
        {
            Standarts = JsonConvert.DeserializeObject<IList<Standart>>(Data.Storage);
            foreach (string k in KB)
            {
                try
                {
                    string s = System.IO.File.ReadAllText(k, Encoding.UTF8);
                    IList<Standart> l = JsonConvert.DeserializeObject<IList<Standart>>(s);
                    foreach (var cur in l)
                    {
                        Standarts.Add(cur);
                    }
                }
                catch {
                }
                
            }
            base.BeginProcessing();
        }

        protected String getText(string filename) {
            
            string text = "";
            try
            {
                try
                {
                    FilterReader reader = new FilterReader(filename);
                    using (reader)
                    {
                        text = reader.ReadToEnd();
                    }
                    if (text == null || text.Length == 0)
                    {
                        TikaReader r = new TikaReader();
                        text = r.getText(filename);
                    }
                }
                catch
                {
                    TikaReader r = new TikaReader();
                    text = r.getText(filename);
                }

                text = text.ToLower();
            }
            catch
            {
            }
            return text;
        }

        protected override void ProcessRecord()
        {
            foreach (string text in Text )
            {
                foreach (Standart cur in Standarts) {
                    string lower = text.ToLower();
                    foreach (Template tcur in cur.Conditions)
                    {
                        
                        if (tcur.Type == "string") {
                            if (lower.Contains(tcur.Mask))
                            {
                                WriteObject(new Match
                                {
                                    Name = cur.Name,
                                    Source = text,
                                    Template = tcur.Descr,
                                    Weight = tcur.Weight,
                                    Mask = tcur.Mask
                                });
                            }
                        } else if (tcur.Type == "regexp")
                        {
                            try
                            {
                                Regex rx = new Regex(tcur.Mask, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                                MatchCollection matches = rx.Matches(text);

                                if (matches.Count > 0)
                                {
                                    WriteObject(new Match
                                    {
                                        Name = cur.Name,
                                        Source = text,
                                        Template = tcur.Descr,
                                        Weight = tcur.Weight,
                                        Mask = tcur.Mask
                                    });
                                }
                            } catch
                            {

                            }
                            
                        }
                    }
                }                
            }

            foreach (string file in File)
            {
                string text = "";
                string ext = Path.GetExtension(file);
                switch (ext)
                {
                    case ".doc":
                    case ".docx":
                    case ".xls":
                    case ".xlsx":
                    case ".pdf":
                    case ".txt":
                    case ".rtf":
                    case ".log":
                        {
                            text = getText(file);
                            break;
                        }
      

                }
                if (text == null || text.Length == 0)
                {
                   
                    continue;
                }
              
            
                string lower = text.ToLower();

                foreach (Standart cur in Standarts)
                {
                    
                    foreach (Template tcur in cur.Conditions)
                    {

                        if (tcur.Type == "string")
                        {
                            /*if (lower.Contains(tcur.Mask))
                            {
                                WriteObject(new Match
                                {
                                    Name = cur.Name,
                                    Source = file,
                                    Template = tcur.Descr,
                                    Weight = tcur.Weight,
                                    Mask = tcur.Mask
                                });
                            }*/
                            try
                            {
                                Regex rx = new Regex("\b" + tcur.Mask, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                                MatchCollection matches = rx.Matches(text);

                                if (matches.Count > 0)
                                {
                                    WriteObject(new Match
                                    {
                                        Name = cur.Name,
                                        Source = file,
                                        Template = tcur.Descr,
                                        Weight = tcur.Weight,
                                        Mask = tcur.Mask
                                    });
                                }
                            }
                            catch
                            {

                            }
                        }
                        else if (tcur.Type == "regexp")
                        {
                            try
                            {
                                Regex rx = new Regex(tcur.Mask, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                                MatchCollection matches = rx.Matches(text);

                                if (matches.Count > 0)
                                {
                                    WriteObject(new Match
                                    {
                                        Name = cur.Name,
                                        Source = file,
                                        Template = tcur.Descr,
                                        Weight = tcur.Weight,
                                        Mask = tcur.Mask
                                    });
                                }
                            }
                            catch
                            {

                            }

                        }
                    }
                }
            }
        }
    }

    
}
