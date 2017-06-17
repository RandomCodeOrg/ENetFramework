using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.ENetFramework.Statements {
    public class StatementResolutionException : Exception {
        public StatementResolutionException() {
        }

        public StatementResolutionException(string message) : base(message) {
        }

        public StatementResolutionException(string message, Exception innerException) : base(message, innerException) {
        }

        protected StatementResolutionException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}
