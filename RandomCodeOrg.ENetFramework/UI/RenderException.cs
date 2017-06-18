using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.ENetFramework.UI {
    public class RenderException : Exception {
        public RenderException() {
        }

        public RenderException(string message) : base(message) {
        }

        public RenderException(string message, Exception innerException) : base(message, innerException) {
        }

        protected RenderException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}
