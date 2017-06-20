using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.ENetFramework.Http {
    public interface IRequest {

        string HttpMethod {
            get;
        }

        string Path {
            get;
        }

        NameValueCollection QueryString {
            get;
        }

        NameValueCollection Headers {
            get;
        }

        string ContentType {
            get;
        }

        int ContentLength {
            get;
        }


        Encoding ContentEncoding {
            get;
        }


        string ClientAddress {
            get;
        }

        string UserAgent {
            get;
        }
        


    }
}
