using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Management.Automation;
using System.Text.RegularExpressions;
using compliance;

namespace compliance
{
    public class Match
    {
        public string Name { get; set; }
        public string Source { get; set; }
        public float Width { get; set; }
        public string Template { get; set; }
    }

    [Cmdlet(VerbsCommon.Get, "Compliance")]
    [OutputType(typeof(Match))]
    public class GetCompliance: PSCmdlet
    {
        private string[] textCollection;

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
                return textCollection;
            }

            set
            {
                textCollection = value;
            }
        }

        public IList<Standart> Standarts { get; set; }

        protected override void BeginProcessing()
        {
            Standarts = JsonConvert.DeserializeObject<IList<Standart>>(Data.Storage);
            base.BeginProcessing();
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
                                    Width = tcur.Width
                                }); ;
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
                                        Width = tcur.Width
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
                FilterReader reader = new FilterReader(file);
                using (reader)
                {
                    text = reader.ReadToEnd();
                }
                string lower = text.ToLower();

                foreach (Standart cur in Standarts)
                {
                    
                    foreach (Template tcur in cur.Conditions)
                    {

                        if (tcur.Type == "string")
                        {
                            if (lower.Contains(tcur.Mask))
                            {
                                WriteObject(new Match
                                {
                                    Name = cur.Name,
                                    Source = file,
                                    Template = tcur.Descr,
                                    Width = tcur.Width
                                });
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
                                        Width = tcur.Width
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
