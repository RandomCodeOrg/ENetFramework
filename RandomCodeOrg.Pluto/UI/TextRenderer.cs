using RandomCodeOrg.ENetFramework.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RandomCodeOrg.Pluto.UI {
    public class TextRenderer : FrameworkRendererBase {

        public TextRenderer() : base("Text") {

        }

        protected override void DoPrepare(XmlElement element, IResolutionRegistry registry) {
            if (element.HasAttribute("Content"))
                registry.RequestResolution(element.Attributes["Content"].Value);
        }

        protected override void DoRender(IRenderContext context, XmlDocument doc, XmlElement element) {
            if (!CheckRendered(context, element))
                return;
            XmlText label = MakeHtmlText(context.Resolve<string>(element.Attributes["Content"].Value), element.ParentNode, element);
            element.ParentNode.RemoveChild(element);
            
        }
    }
}
