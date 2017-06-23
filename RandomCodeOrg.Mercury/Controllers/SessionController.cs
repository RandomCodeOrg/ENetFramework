using RandomCodeOrg.ENetFramework.Container;
using RandomCodeOrg.Mercury.Data;
using slf4net;
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

        private static volatile int idCounter;

        private readonly int id;

        public SessionController() {
            id = idCounter++;
        }

        [Inject]
        private IMercuryData data;

        [Inject]
        private HomeController homeController;

        [Inject]
        private ILogger logger;

        public string WelcomeMessage {
            get {
                string yourName = "Guest";
                if (!string.IsNullOrEmpty(Name)) {
                    yourName = Name;
                }
                return string.Format("Welcome '{0}'! Version: {1}", yourName, homeController.Constructed);
            }
        }


        [PostConstruct]
        public void Init() {
            logger.Info("Started session: {0}", id);
            data.AddVisitor(id);
        }

    }
}
