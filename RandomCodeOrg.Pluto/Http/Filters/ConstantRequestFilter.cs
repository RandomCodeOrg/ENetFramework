using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHttp;

namespace RandomCodeOrg.Pluto.Http.Filters {
    public class ConstantRequestFilter : RequestFilter {


        private bool constant;

        public ConstantRequestFilter(bool constant) {
            this.constant = constant;
        }

        public override bool Filter(HttpRequestEventArgs e) {
            return constant;
        }
    }
}
