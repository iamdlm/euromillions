using System;
using System.Collections.Generic;
using System.Text;

namespace Euromillions.Configurations
{
    class AppSettings
    {
        public string FolderPath { get; set; }

        public string FilePath { get; set; }

        public NunofcAPIConfiguration NunofcAPIConfiguration { get; set; }
    }    
}
