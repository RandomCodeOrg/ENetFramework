using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHttp;
using slf4net;

namespace RandomCodeOrg.Pluto.Http {
    public class RequestHandlerChain : IRequestHandler{

        private readonly IList<IRequestHandler> handler = new List<IRequestHandler>();

        private readonly ILogger logger = LoggerFactory.GetLogger(typeof(RequestHandlerChain));

        public RequestHandlerChain() {

        }

        public bool Handle(HttpRequestEventArgs e) {
            foreach (IRequestHandler h in handler) {
                if (h.Handle(e)) {
                    logger.Trace("Request for '{0}' was handled and returned HTTP status code {1}", e.Request.Url, e.Response.StatusCode);
                    return true;
                }
            }
            return false;
        }

        public void HandleRequest(HttpRequestEventArgs e) {
            logger.Trace("Client requested: {0}", e.Request.Url);
            if (Handle(e))
                return;
          
            e.Response.Status = "Not Found";
            e.Response.StatusCode = 404;
        }


        public void Register(IRequestHandler handler) {
            if (!this.handler.Contains(handler)) {
                this.handler.Add(handler);
            }
        }

        public void Remove(IRequestHandler handler) {
            if (this.handler.Contains(handler)) {
                this.handler.Remove(handler);
            }
        }

    }
}
