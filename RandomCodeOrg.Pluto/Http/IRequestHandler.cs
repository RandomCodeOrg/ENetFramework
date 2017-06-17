using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHttp;
namespace RandomCodeOrg.Pluto.Http {
    public interface IRequestHandler {



        bool Handle(HttpRequestEventArgs e);


    }
}
