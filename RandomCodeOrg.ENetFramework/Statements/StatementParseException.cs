using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.ENetFramework.Statements {
    public class StatementParseException : Exception {
        public StatementParseException() {

        }

        public StatementParseException(string message) : base(message) {

        }

        public StatementParseException(string message, Exception innerException) : base(message, innerException) {

        }

        protected StatementParseException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}
