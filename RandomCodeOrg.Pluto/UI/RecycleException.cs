using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RandomCodeOrg.Pluto.UI {
    public class RecycleException : Exception {

        private readonly XmlElement element;

        public XmlElement Element {
            get {
               return element;
            }
        }

        public RecycleException(XmlElement element) {
            this.element = element;
        }

    }
}
