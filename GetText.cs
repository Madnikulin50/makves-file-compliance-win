using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using compliance;

namespace compliance
{
    public class ComplienceResult
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int DeviceId { get; set; }
        public string Manufacturer { get; set; }
        public string NetConnectionId { get; set; }
        public bool PhysicalAdapter { get; set; }
    }

    [Cmdlet(VerbsCommon.Get, "Document-Text")]
    class GetDocumentText : PSCmdlet
    {
        private string[] filesCollection;
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ValueFromPipeline = true, Position = 0, HelpMessage = "Texts for analysis")]
        public string[] Files
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
        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }

        protected override void ProcessRecord()
        {

            foreach (string file in Files)
            {
                WriteObject("Start extruct " + file);
                FilterReader reader = new FilterReader(file);
                using (reader)
                {
                    string text = reader.ReadToEnd();
                    WriteObject("Start extruct " + file);
                }
            }
        }
    }
}
