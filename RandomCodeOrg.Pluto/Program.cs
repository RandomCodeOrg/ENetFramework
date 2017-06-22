using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RandomCodeOrg.ENetFramework;
using RandomCodeOrg.Pluto.CDI;

using NHttp;
using slf4net;
using log4net;
using slf4net.log4net;
using System.Reflection;

namespace RandomCodeOrg.Pluto {
    class Program {
        static void Main(string[] args) {
            ApplicationStartupConfig startupConfig = new ApplicationStartupConfig(args);
            
            var applicationContainer = new PlutoApplicationContainer();

            if (startupConfig.HasFlag(StartupArgument.DISABLE_EXTRACTION))
                applicationContainer.DisableExtraction = true;

            applicationContainer.Start();

            ENetFrameworkContext.Instance.Register(applicationContainer);

        }


        
    }


    
}
