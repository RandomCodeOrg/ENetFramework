using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHttp;

namespace RandomCodeOrg.Pluto.Http.Filters {
    public abstract class RequestFilter {


        public abstract bool Filter(HttpRequestEventArgs e);

    

        public static RequestFilter operator &(RequestFilter a, RequestFilter b) {
            return new AndRequestFilter(a, b);
        }
       


    }
}
