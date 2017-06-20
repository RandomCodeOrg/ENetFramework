using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.ENetFramework.Http {

    /// <summary>
    /// A listener that listenes for new client requests.
    /// </summary>
    public interface IRequestListener {

        /// <summary>
        /// Is called when a new request is received.
        /// </summary>
        /// <param name="request">The context created for the received request.</param>
        void OnNewRequest(IRequestContext request);
        


    }
}
