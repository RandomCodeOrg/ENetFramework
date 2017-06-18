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
        private ILogger logger;
        
        private DateTime constructed;

        public DateTime Constructed {
            get {
                return constructed;
            }
        }
        
        [PostConstruct]
        public void Initialize() {
            logger.Trace("Controller constructed...");
            constructed = DateTime.Now;
        }
        

    }
}
