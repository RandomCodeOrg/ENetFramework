using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using RandomCodeOrg.ENetFramework.UI;
using RandomCodeOrg.Pluto.Resources;

namespace RandomCodeOrg.Pluto.UI {
    public class CompositionRenderer : FrameworkRendererBase {

        private const string SOURCE_ATTRIBUTE = "Source";

        private readonly ApplicationResourceManager resourceManager;

        public CompositionRenderer(ApplicationResourceManager resourceManager) : base("Composition") {
            this.resourceManager = resourceManager;
        }

        protected override void DoPrepare(XmlElement element, IResolutionRegistry registry) {
            XmlDocument doc = resourceManager.IncludeDocument(element.GetAttribute(SOURCE_ATTRIBUTE));
            if(doc != null) {
                var document = element.OwnerDocument;
                XmlNode toImport = document.ImportNode(doc.DocumentElement, true);
                List<XmlElement> elements = new List<XmlElement>();
                IDictionary<string, XmlElement> substitutionsMap = new Dictionary<string, XmlElement>();
                foreach(XmlNode child in element.ChildNodes)
                    if(child is XmlElement && child.LocalName.Equals("Substitution")) {
                        substitutionsMap[child.As<XmlElement>().GetAttribute("Placeholder")] = child.As<XmlElement>();
                    }
                IDictionary<string, XmlElement> placeholderMap = new Dictionary<string, XmlElement>();
                var xmlnsManager = new XmlNamespaceManager(doc.NameTable);
                xmlnsManager.AddNamespace("net", "http://randomcodeorg.github.com/ENetFramework");
                var placeholders = toImport.SelectNodes("//net:Placeholder[@Name]", xmlnsManager);
                foreach(XmlNode n in placeholders) {
                    if (n is XmlElement)
                        placeholderMap[n.As<XmlElement>().GetAttribute("Name")] = n.As<XmlElement>();
                }
                foreach(string substitution in substitutionsMap.Keys) {
                    if (!placeholderMap.ContainsKey(substitution))
                        continue;
                    var subsElement = substitutionsMap[substitution];
                    var placeholder = placeholderMap[substitution];
                    foreach (XmlNode subsChild in subsElement.ChildNodes) {
                        placeholder.ParentNode.InsertBefore(subsChild.CloneNode(true), placeholder);
                    }
                    placeholder.ParentNode.RemoveChild(placeholder);
                }
                var targetDoc = element.OwnerDocument;
                targetDoc.RemoveAll();
                targetDoc.AppendChild(toImport);
                throw new RecycleException(targetDoc.DocumentElement);
            }
        }

        protected override void DoRender(IRenderContext context, XmlDocument doc, XmlElement element) {
                
        }
    }
}
