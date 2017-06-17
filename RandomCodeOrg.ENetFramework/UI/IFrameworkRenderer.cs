using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RandomCodeOrg.ENetFramework.UI {
    public interface IFrameworkRenderer {

        ISet<string> SupportedElements { get; }

        void Render(IRenderContext context, XmlDocument doc, XmlElement element);

        void Prepare(XmlElement element, IResolutionRegistry registry);

    }
}
