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
            

            var applicationContainer = new PlutoApplicationContainer();
            applicationContainer.Start();

            ENetFrameworkContext.Instance.Register(applicationContainer);

        }


        
    }


    
}
