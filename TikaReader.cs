using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TikaOnDotNet.TextExtraction;

namespace compliance
{
    class TikaReader
    {

        public TikaReader()
        {
            
        }
        public string getText(string fileName)
        {
            try
            {
                TextExtractor textExtractor = new TextExtractor();
                string result = textExtractor.Extract(fileName).Text;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
    }
}
