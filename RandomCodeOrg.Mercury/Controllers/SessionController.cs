using RandomCodeOrg.ENetFramework.Container;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.Mercury.Controllers {

    public interface ISessionController {
    

        string Name { get; set; }

        bool IsAuthenticated { get; }

    }


    [Managed]
    [SessionScoped]
    internal class SessionController : ISessionController {


        public string Name { get; set; }

        public bool IsAuthenticated {
            get {
                return !string.IsNullOrWhiteSpace(Name);
            }
        }
        

    }
}
