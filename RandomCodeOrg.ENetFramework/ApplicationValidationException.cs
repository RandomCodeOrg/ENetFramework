using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.ENetFramework {

    /// <summary>
    /// This exception will be thrown by the application container, if an application fails validation.
    /// 
    /// The application container may validate an application during the deployment process. More information about the causing problems(s) might be present in the exception's message or the server log/output.
    /// </summary>
    public class ApplicationValidationException : Exception {
        

        public ApplicationValidationException() {

        }

        public ApplicationValidationException(string message) : base(message) {
        }



        public ApplicationValidationException(string message, Exception innerException) : base(message, innerException) {
        }

        protected ApplicationValidationException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
        

    }
}
