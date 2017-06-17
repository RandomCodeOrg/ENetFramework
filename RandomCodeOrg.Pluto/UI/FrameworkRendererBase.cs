using RandomCodeOrg.ENetFramework.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RandomCodeOrg.Pluto.UI {
    public abstract class FrameworkRendererBase : IFrameworkRenderer {

        private readonly ISet<string> supportedElements = new HashSet<string>();

        public ISet<string> SupportedElements {
            get {
                return supportedElements;
            }
        }

        protected FrameworkRendererBase(params string[] supportedElements) {
            foreach (string supported in supportedElements) {
                this.supportedElements.Add(supported);
            }
        }


        public void Render(IRenderContext context, XmlDocument doc, XmlElement element) {
            if (!SupportedElements.Contains(element.LocalName))
                return;

            DoRender(context, doc, element);
        }

        protected abstract void DoRender(IRenderContext context, XmlDocument doc, XmlElement element);

        protected bool CheckRendered(IRenderContext context, XmlElement element) {
            if (!element.HasAttribute("Rendered"))
                return true;
            bool rendered = context.Resolve<bool>(element.Attributes["Rendered"].Value);
            if (!rendered) {
                element.ParentNode.RemoveChild(element);
            }
            return rendered;
        }


        protected bool RecurseChildren(IRenderContext context, XmlDocument doc, XmlElement element) {
            bool result = false;
            List<XmlElement> children = new List<XmlElement>();

            foreach (XmlNode node in element.ChildNodes) {
                if (node is XmlElement)
                    children.Add(node.As<XmlElement>());
            }

            foreach (XmlElement child in children) {
                result = result | Recurse(context, doc, child);
            }

            return result;
        }

        protected bool Recurse(IRenderContext context, XmlDocument doc, XmlElement element) {
            bool result = false;

            if ("http://randomcodeorg.github.com/ENetFramework".Equals(element.NamespaceURI)) {
                result = true;
                context.Render(doc, element);
            }

            return result | RecurseChildren(context, doc, element);
        }

        public void ReplaceWithChildren(XmlElement element) {
            List<XmlNode> children = new List<XmlNode>();
            foreach (XmlNode child in element.ChildNodes) {
                children.Add(child);
            }
            foreach (XmlNode child in children) {
                element.ParentNode.InsertBefore(child, element);
            }
            element.ParentNode.RemoveChild(element);
        }


        protected XmlElement MakeHtmlTag(string tag, XmlNode parent, XmlElement current = null) {
            XmlElement element = parent.OwnerDocument.CreateElement(tag, "http://www.w3.org/1999/xhtml");
            if (current == null) {
                parent.AppendChild(element);
            } else {
                parent.InsertAfter(element, current);
            }
            return element;
        }

        protected void CopyChildren(XmlElement source, XmlElement target) {
            List<XmlNode> children = new List<XmlNode>();
            foreach (XmlNode child in source.ChildNodes) {
                children.Add(child);
            }
            foreach (XmlNode child in children) {
                target.AppendChild(child);
            }
        }


        protected XmlText MakeHtmlText(string content, XmlNode parent, XmlElement current = null) {
            XmlText element = parent.OwnerDocument.CreateTextNode(content);
            if (current == null) {
                parent.AppendChild(element);
            } else {
                parent.InsertAfter(element, current);
            }
            return element;
        }

        protected void CopyAttributes(XmlElement source, XmlElement target, params string[] attributeNames) {
            foreach (string attr in attributeNames) {
                if (source.HasAttribute(attr)) {
                    target.Attributes.Append(source.Attributes[attr]);
                }
            }

        }

        protected void SetAttribute(XmlElement element, string key, string value) {
            if (element.HasAttribute("key")) {
                element.Attributes[key].Value = value;
                return;
            }
            var attr = element.OwnerDocument.CreateAttribute(key);
            attr.Value = value;
            element.Attributes.Append(attr);
        }

        protected void CopyHtmlAttributes(XmlElement source, XmlElement target) {
            CopyAttributes(source, target, "id", "class", "style", "title");
        }

        public void Remove(XmlElement element) {
            element.ParentNode.RemoveChild(element);
        }


        public void Prepare(XmlElement element, IResolutionRegistry registry) {
            if (element.HasAttribute("Rendered")) {
                registry.RequestResolution(element.Attributes["Rendered"].Value);
            }
            DoPrepare(element, registry);
        }

        protected abstract void DoPrepare(XmlElement element, IResolutionRegistry registry);

        protected string GetAttribute(XmlElement element, string attribute) {
            if (!element.HasAttribute(attribute))
                return null;
            return element.Attributes[attribute].Value;
        }

    }
}
