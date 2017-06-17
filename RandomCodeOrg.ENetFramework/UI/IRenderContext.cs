using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RandomCodeOrg.ENetFramework.UI {
    public interface IRenderContext {

        void Render(XmlDocument document);

        void Render(XmlDocument document, XmlElement element);
        
        T Resolve<T>(string statement);

        string BuildParmeterName(XmlElement element, string parameterStatement);



    }
}
