using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using RandomCodeOrg.ENetFramework;
using slf4net;
using NHttp;


namespace RandomCodeOrg.Pluto {
    public class PlutoApplicationContainer : IApplicationContainer {

        private readonly ILogger logger = LoggerFactory.GetLogger(typeof(PlutoApplicationContainer));

        private readonly ISet<PlutoApplicationContext> applications = new HashSet<PlutoApplicationContext>();

        private readonly Http.RequestHandlerChain requestHandler = new Http.RequestHandlerChain();

        private HttpServer server;

        private readonly string homePath;

        public string HomePath {
            get {
                return homePath;
            }
        }

        public Http.RequestHandlerChain HandlerChain {
            get {
                return requestHandler;
            }
        }

        public PlutoApplicationContainer() {
            homePath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "PlutoApplicationContainer");
        }

        public void Start() {
            server = new HttpServer();
            server.EndPoint = new System.Net.IPEndPoint(System.Net.IPAddress.Any, 8080);
            server.RequestReceived += (s, e) => requestHandler.HandleRequest(e);
            server.Start();
        }
        
        public void Deploy(Assembly assembly) {
            PlutoApplicationContext application = new PlutoApplicationContext(this, assembly);
            applications.Add(application);
            application.Start();
        }


        public void Shutdown() {
            logger.Info("Shutting down the application container...");
            server.Stop();

            foreach(PlutoApplicationContext context in applications) {
                context.Stop();
            }
            logger.Info("Shutdown completed.");
        }
       

    }
}
