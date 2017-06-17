using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using RandomCodeOrg.ENetFramework.UI;
using RandomCodeOrg.Pluto.Resources;

namespace RandomCodeOrg.Pluto.UI {
    public class IncludeRenderer : FrameworkRendererBase {

        private readonly ApplicationResourceManager resourceManager;

        public IncludeRenderer(ApplicationResourceManager resourceManager) : base("Include") {
            this.resourceManager = resourceManager;
        }

        protected override void DoPrepare(XmlElement element, IResolutionRegistry registry) {
            XmlDocument doc = resourceManager.IncludeDocument(element.GetAttribute("Source"));
            if (doc != null) {
                var parent = element.ParentNode;
                XmlNode toImport = element.OwnerDocument.ImportNode(doc.DocumentElement, true);
                element.ParentNode.InsertBefore(toImport, element);
                element.ParentNode.RemoveChild(element);
            }
        }

        protected override void DoRender(IRenderContext context, XmlDocument doc, XmlElement element) {

        }
    }
}
