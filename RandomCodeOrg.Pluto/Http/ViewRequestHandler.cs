using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NHttp;
using RandomCodeOrg.Pluto.Resources;
using System.Xml;
using RandomCodeOrg.ENetFramework.UI;
using RandomCodeOrg.Pluto.Statements;

namespace RandomCodeOrg.Pluto.Http {
    public class ViewRequestHandler : IRequestHandler {

        private readonly ApplicationResourceManager resourceManager;

        private readonly Filters.RequestFilter requestFilter;
        

        private readonly PlutoStatementParser statementParser;

        private readonly CDI.ContextStateManager stateManager;

        private readonly Sessions.SessionManager sessionManager;

        public ViewRequestHandler(Filters.RequestFilter requestFilter, ApplicationResourceManager resourceManager, PlutoStatementParser statementParser, CDI.ContextStateManager stateManager, Sessions.SessionManager sessionManager) {
            this.resourceManager = resourceManager;
            this.stateManager = stateManager;
            this.requestFilter = requestFilter;
            this.statementParser = statementParser;
            this.sessionManager = sessionManager;
        }


        protected bool GetSessionId(HttpRequest request, out long id) {
            if (request.Cookies.AllKeys.Contains("session")) {
                string value = request.Cookies["session"].Value;
                if (long.TryParse(value, out id)) {
                    return true;
                }
            }
            
            id = 0;
            return false;
        }

        public bool Handle(HttpRequestEventArgs e) {
            if (!requestFilter.Filter(e))
                return false;
            string localPath = e.Request.Path;
            if (!resourceManager.HasView(localPath)) {
                string current = FindIndex(localPath);
                if (current == null)
                    return false;
                localPath = current;
            }
            Sessions.Session session = null;
            try {
                stateManager.BeginRequest();
                sessionManager.Clean();
                long sessionId;
                if (GetSessionId(e.Request, out sessionId)) {
                    session = sessionManager.GetSession(sessionId);
                }
                if (session != null)
                    stateManager.ContinueSession(session.CDIContext);
               
                ViewSource view = resourceManager.GetView(localPath);
                var response = e.Response;

                var doc = (XmlDocument)view.Document.Clone();

                UI.PlutoRenderContext renderContext = new UI.PlutoRenderContext(statementParser, e.Request, resourceManager);

                renderContext.Render(doc);

                renderContext.Render(doc, doc.DocumentElement);


                response.ContentType = "text/html";

                using (Stream output = response.OutputStream) {
                    doc.Save(output);
                }

                if (sessionManager.HasNewSession) {
                    session = sessionManager.PopNewSession();
                    response.Cookies.Add(new HttpCookie("session", string.Format("{0}", session.Id)) {
                        HttpOnly = true
                    });
                }

                response.StatusCode = 200;

            } finally {
                if (session != null) stateManager.SuspendSession();
                stateManager.CompleteRequest();
            }

            return true;
        }

        protected string FindIndex(string localPath) {
            if (!localPath.EndsWith("/"))
                localPath += "/";
            string current = localPath + "index.html";
            if (resourceManager.HasView(current))
                return current;
            current = localPath + "Index.html";
            if (resourceManager.HasView(current))
                return current;
            current = localPath + "index.xml";
            if (resourceManager.HasView(current))
                return current;
            current = localPath + "Index.xml";
            if (resourceManager.HasView(current))
                return current;
            return null;
        }
        

    }
}
