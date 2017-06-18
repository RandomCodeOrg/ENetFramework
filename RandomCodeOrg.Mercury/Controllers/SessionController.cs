using RandomCodeOrg.ENetFramework.Container;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.Mercury.Controllers {

    public interface ISessionController {
    

        string Name { get; set; }
        
        string WelcomeMessage { get; }

    }


    [Managed]
    [SessionScoped]
    internal class SessionController : ISessionController {


        public string Name { get; set; }

        [Inject]
        private HomeController homeController;

        public string WelcomeMessage {
            get {
                string yourName = "Guest";
                if (!string.IsNullOrEmpty(Name)) {
                    yourName = Name;
                }
                return string.Format("Welcome '{0}'! Version: {1}", yourName, homeController.Constructed);
            }
        }

    }
}
