using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Threading.Tasks;
using slf4net;
using RandomCodeOrg.Pluto.Http;
using RandomCodeOrg.Pluto.Statements;

namespace RandomCodeOrg.Pluto {
    public class PlutoApplicationContext {

        private readonly Assembly assembly;

        private readonly ILogger logger = LoggerFactory.GetLogger(typeof(PlutoApplicationContext));
        private readonly string applicationName;
        private readonly PlutoApplicationContainer container;
        private Http.Filters.RequestFilter requestFilter;

        private readonly ResourceRequestHandler resourceHandler;
        private readonly ViewRequestHandler viewHandler;

        private readonly Http.Mapping.PathMapping mapping;
        private readonly Resources.ApplicationResourceManager resourcesManager;


        private Statements.Compiler.PlutoStatementCompiler statementCompiler;
        private readonly CDI.CDIContainer cdiContainer;
        private readonly PlutoStatementParser statementParser;
        private readonly Sessions.SessionManager sessionManager = new Sessions.SessionManager();
        private readonly CDI.ContextStateManager contextManager;


        public PlutoApplicationContext(PlutoApplicationContainer container, Assembly assembly) {
            this.assembly = assembly;
            contextManager = new CDI.ContextStateManager(sessionManager);
            cdiContainer = new CDI.CDIContainer(contextManager).Use(new CDI.LoggerInjector());
            statementCompiler = new Statements.Compiler.PlutoStatementCompiler(cdiContainer, assembly.EntryPoint.DeclaringType.Namespace);
            statementCompiler.Reference(assembly);
            statementParser = new PlutoStatementParser(statementCompiler);
            this.container = container;
            applicationName = assembly.GetName().Name;
            requestFilter = new Http.Filters.ConstantRequestFilter(true);
            mapping = new Http.Mapping.PathMapping();
            resourceHandler = new ResourceRequestHandler(requestFilter & new Http.Filters.PathRequestFilter("/resources/.*"), mapping);
            resourcesManager = new Resources.ApplicationResourceManager(container.HomePath, applicationName);
            viewHandler = new ViewRequestHandler(requestFilter, resourcesManager, statementParser, contextManager, sessionManager);
        }

        public void Start() {
            logger.Info("Starting application '{0}' v{1}...", applicationName, assembly.GetName().Version);
            logger.Debug("Assembly location is: {0}", assembly.Location);
            logger.Debug("Application directory is: {0}", resourcesManager.HomePath);

            mapping.Map("/resources/(.*)", resourcesManager.ResourcesPath + @"\{0}");

            resourcesManager.Load(assembly);

            logger.Info("Starting CDI container...");
            cdiContainer.Load(assembly);

            logger.Info("Registering request handlers...");
            container.HandlerChain.Register(resourceHandler);
            container.HandlerChain.Register(viewHandler);
        }

        public string BuildResourcePath(string basePath, string prefix, string resourceName) {
            string suffix = resourceName.Substring(prefix.Length);
            string result = basePath;
            string[] parts = suffix.Split('.');
            for (int i = 0; i < parts.Length; i++) {
                if (i == parts.Length - 2) {
                    result = Path.Combine(result, parts[i] + "." + parts[i + 1]);
                    break;
                } else {
                    result = Path.Combine(result, parts[i]);
                }
            }
            return result;
        }

        public void Stop() {
            logger.Info("Stopping application '{0}'...", applicationName);

            logger.Trace("Releasing resources.");

            container.HandlerChain.Remove(resourceHandler);
            container.HandlerChain.Remove(viewHandler);

            resourcesManager.Unload();
        }



    }
}
