using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RandomCodeOrg.ENetFramework;


namespace RandomCodeOrg.Mercury {
    class Program : ENetFrameworkApplication {
        static void Main(string[] args) {
            new Program().Start(
                new string[] {
                    "-Container", "RandomCodeOrg.Pluto",
                    "-ContainerPath", @"..\..\..\RandomCodeOrg.Pluto\bin\Debug",
                    "-RemoteLoading",
                    "-CreatePackage"
                });
        }
    }
}
