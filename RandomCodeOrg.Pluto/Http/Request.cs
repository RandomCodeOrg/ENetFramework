using NHttp;
using RandomCodeOrg.ENetFramework.Http;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.Pluto.Http {
    public class Request : IRequest {


        private readonly HttpRequest request;

        public string HttpMethod {
            get {
                return request.HttpMethod;
            }
        }

        public string Path {
            get {
                return request.Path;
            }
        }
        
        public NameValueCollection QueryString {
            get {
                return request.QueryString;
            }
        }
        
        public NameValueCollection Headers {
            get {
                return request.Headers;
            }
        }

        public string ContentType {
            get {
                return request.ContentType;
            }
        }

        public int ContentLength {
            get {
                return request.ContentLength;
            }
        }

        public Encoding ContentEncoding {
            get {
                return request.ContentEncoding;
            }
        }

        public string ClientAddress {
            get {
                return request.UserHostAddress;
            }
        }
        
        public string UserAgent {
            get {
                return request.UserAgent;
            }
        }

      

        public Request(HttpRequest request) {
            this.request = request;
        }
        
    }
}
