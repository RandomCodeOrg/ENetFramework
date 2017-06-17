using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using RandomCodeOrg.ENetFramework.UI;

namespace RandomCodeOrg.Pluto.UI {
    public class IterationRenderer : FrameworkRendererBase {

        public IterationRenderer() : base("Iteration") {

        }



        protected override void DoPrepare(XmlElement element, IResolutionRegistry registry) {
            registry.RequestIterationVariable(element.GetAttribute("Value"), element.GetAttribute("Variable"));

        }

        protected override void DoRender(IRenderContext context, XmlDocument doc, XmlElement element) {
            if (!CheckRendered(context, element))
                return;
            string varName = element.GetAttribute("Variable");
            IEnumerable value = (IEnumerable)context.Resolve<object>(element.GetAttribute("Value"));
            XmlElement parent = element.ParentNode.As<XmlElement>();
            XmlElement clone;
            //XmlNode reference = element;
            List<XmlElement> children = new List<XmlElement>();
            foreach(XmlNode child in element.ChildNodes) {
                if (child is XmlElement)
                    children.Add(child.As<XmlElement>());
            }
            IList<XmlElement> clones = new List<XmlElement>();
            foreach (object entry in value) {
                context.PushVariable(varName, entry);
                foreach (XmlElement child in children) {
                    clone = child.CloneNode(true).As<XmlElement>();
                    parent.InsertBefore(clone, element);
                    //reference = clone;
                    clones.Add(clone);
                }
                foreach(XmlElement toRecurse in clones) {

                    Recurse(context, doc, toRecurse);
                }
                clones.Clear();
            }
            parent.RemoveChild(element);
            context.PopVariable(varName);
        }




    }
}
