using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHttp;

namespace RandomCodeOrg.Pluto.Http.Filters {
    public class AndRequestFilter : RequestFilter {


        private readonly RequestFilter a;
        private readonly RequestFilter b;

        public AndRequestFilter(RequestFilter a, RequestFilter b) {
            this.a = a;
            this.b = b;
        }

        public override bool Filter(HttpRequestEventArgs e) {
            return a.Filter(e) && b.Filter(e);
        }
    }
}
