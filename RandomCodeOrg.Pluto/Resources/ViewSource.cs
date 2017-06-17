using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace RandomCodeOrg.Pluto.Resources {
    public class ViewSource {

        private readonly XmlDocument document;

        public XmlDocument Document {
            get {
                return document;
            }
        }

        public ViewSource(Stream stream) {
            document = new XmlDocument();
            document.Load(stream);
        }

        

    }
}
