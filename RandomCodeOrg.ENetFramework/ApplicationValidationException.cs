using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.ENetFramework {
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
