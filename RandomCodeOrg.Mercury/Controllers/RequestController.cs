using RandomCodeOrg.ENetFramework.Container;
using slf4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.Mercury.Controllers {
    [Managed]
    [RequestScoped]
    internal class RequestController : IRequestController {


        [Inject]
        private ILogger logger;

        private int number;

        public int RequestNumber {
            get {
                logger.Debug("Result should be: {0}", number);
                return number;
            }
        }


        private string name;

        public string Name {
            get {
                return name;
            }
            set {
                name = value;
            }
        }

        private static int instances = 0;

        public RequestController() {
            number = instances++;

        }

        [PostConstruct]
        public void Init() {
            logger.Info("New RequestController...");
        }
        

        [OnDispose]
        private void Destroy() {
            logger.Info(">> Destroyed!");
        }

        
    }
}
