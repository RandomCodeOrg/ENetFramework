using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using RandomCodeOrg.ENetFramework;
using slf4net;
using NHttp;
using RandomCodeOrg.Pluto.Health;
using RandomCodeOrg.ENetFramework.Deployment;

namespace RandomCodeOrg.Pluto {
    public class PlutoApplicationContainer : IApplicationContainer {

        private readonly ILogger logger = LoggerFactory.GetLogger(typeof(PlutoApplicationContainer));

        private readonly ISet<PlutoApplicationContext> applications = new HashSet<PlutoApplicationContext>();

        private readonly Http.RequestHandlerChain requestHandler = new Http.RequestHandlerChain();

        private HttpServer server;

        private readonly string homePath;

        private readonly ApplicationValidator validator = new ApplicationValidator();

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

        public bool DisableExtraction { get; set; } = false;

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
            Deploy(new AssemblydefinedApplication(assembly));
        }

        

        public void Shutdown() {
            logger.Info("Shutting down the application container...");
            server.Stop();

            foreach(PlutoApplicationContext context in applications) {
                context.Stop();
            }
            logger.Info("Shutdown completed.");
        }

        public void Deploy(IDeploymentPackage deploymentPackage) {
            
            throw new NotImplementedException(); // TODO: Implement
        }


        protected void Deploy(IApplicationHandle appHandle) {
            logger.Info("Validating {0}...", appHandle.FriendlyName);
            var report = validator.Validate(appHandle.DefiningAssembly);
            string message = report.GetPrintableString();
            if (report.HasErrors) {
                logger.Error(message);
            } else if (report.HasWarnings) {
                logger.Warn(message);
            } else {
                logger.Info(message);
            }
            if (report.HasErrors)
                throw new ApplicationValidationException(string.Format("The application failed validation.\n\n{0}", message));
            

            PlutoApplicationContext application = new PlutoApplicationContext(this, appHandle);
            applications.Add(application);
            application.Start();
        }

    }
}
