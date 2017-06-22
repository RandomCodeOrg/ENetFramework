using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
using RandomCodeOrg.Pluto.Resources;
using NHttp;
using RandomCodeOrg.Pluto.Http.Mapping;

namespace RandomCodeOrg.Pluto.Http {
    public class ResourceRequestHandler : IRequestHandler {

        private readonly Filters.RequestFilter requestFilter;
        private readonly Mapping.PathMapping mapping;
        private readonly ApplicationResourceManager resourceManager;
        

        public ResourceRequestHandler(Filters.RequestFilter requestFilter, PathMapping mapping, ApplicationResourceManager resourceManager) {
            this.requestFilter = requestFilter;
            this.resourceManager = resourceManager;
            this.mapping = mapping;
        }


        public bool Handle(HttpRequestEventArgs e) {
            if (!requestFilter.Filter(e))
                return false;

            string path = mapping[e.Request.Url];
            if (path == null)
                return false;
            if (!resourceManager.HasStaticResource(path))
                return false;

            var response = e.Response;
            string mimeType = mapping.GetContentType(path);
            if (mimeType != null)
                response.ContentType = mimeType;

            resourceManager.StaticResource(path, response.OutputStream);

            /*
            using (FileStream input = new FileStream(path, FileMode.Open, FileAccess.Read)) {
                input.CopyTo(response.OutputStream);
            }*/

            int timeout = 2; //TODO: move to config

            response.CacheControl = string.Format("public, max-age={0}", (timeout) * 60);

            response.StatusCode = 200;
            response.Status = "OK";
            return true;
        }


    }
}
