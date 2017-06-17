using RandomCodeOrg.ENetFramework.Container;
using slf4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.Mercury.Controllers {

    [Managed]
    [ApplicationScoped]
    public class HomeController {

        [Inject]
        public IRequestController RequestController { get; set; }

        [Inject]
        private ILogger logger;

        public string WelcomeMessage() {
            logger.Trace("Backing method was called...");
            int number = RequestController.RequestNumber;
            return string.Format("Welcome '{0}' - Connection number #{1}", RequestController.Name, number);
        }


        [PostConstruct]
        public void Initialize() {
            logger.Trace("Controller constructed...");
        }


        [PostConstruct]
        public void InvalidInitialize(object smth) {
            logger.Info("ahll");
        }

    }
}
