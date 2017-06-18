using RandomCodeOrg.ENetFramework.Container;
using slf4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.Mercury.Controllers {

    public interface IRequestController {
        
        int RequestNumber { get; }

    }

    [Managed]
    [RequestScoped]
    internal class RequestController : IRequestController {

        private static int requestCounter = 0;

        [Inject]
        private ILogger logger;
        
        private int number;

        public int RequestNumber {
            get {
                return number;
            }
        }
        
        [PostConstruct]
        public void Init() {
            number = requestCounter++;
            logger.Debug("This is the controller for request #{0} speaking...", number);
        }


        [OnDispose]
        private void Destroy() {
            logger.Debug("... request controller #{0}: over and out!");
        }


    }

}
