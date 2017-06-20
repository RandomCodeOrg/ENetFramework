using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.ENetFramework.Http {

    /// <summary>
    /// A context bundling relevant information for a given request. 
    /// </summary>
    public interface IRequestContext {

        /// <summary>
        /// The received request of this request context.
        /// </summary>
        IRequest Request { get; }

        /// <summary>
        /// Terminates the request processing. The client request will be discarded without sending a response. 
        /// </summary>
        void TerminateRequest();

        /// <summary>
        /// Ends the request processing. In contrast to <see cref="TerminateRequest"/> a response will be sent to the client.
        /// </summary>
        /// <param name="statusCode"></param>
        void EndRequest(int statusCode);

    }
}
