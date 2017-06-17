using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using RandomCodeOrg.ENetFramework.UI;

namespace RandomCodeOrg.Pluto.UI {
    public class LabelRenderer : FrameworkRendererBase {

        public LabelRenderer() : base("Label") {

        }

        protected override void DoPrepare(XmlElement element, IResolutionRegistry registry) {
            if (element.HasAttribute("Content"))
                registry.RequestResolution(element.Attributes["Content"].Value);
        }

        protected override void DoRender(IRenderContext context, XmlDocument doc, XmlElement element) {
            if (!CheckRendered(context, element))
                return;
            XmlElement label = MakeHtmlTag("label", element.ParentNode, element);
            label.InnerText = context.Resolve<string>(element.Attributes["Content"].Value);
            CopyHtmlAttributes(element, label);
            element.ParentNode.RemoveChild(element);
        }
        
    }
}
