using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using RandomCodeOrg.ENetFramework.UI;

namespace RandomCodeOrg.Pluto.UI {
    public class InputElementRenderer : FrameworkRendererBase {

        private readonly string inputType;

        protected const string VALUE_ATTRIBUTE = "Value";

        public InputElementRenderer(string elementName, string inputType) : base(elementName) {
            this.inputType = inputType;
        }

        protected override void DoRender(IRenderContext context, XmlDocument doc, XmlElement element) {
            if (!CheckRendered(context, element))
                return;
            XmlElement input = MakeHtmlTag("input", element.ParentNode, element);
            CopyHtmlAttributes(element, input);
            SetAttribute(input, "type", GetInputType());
            SetAttribute(input, "name", context.BuildParmeterName(input, GetAttribute(element, VALUE_ATTRIBUTE)));
            ApplyValue(context, element, input);
            element.ParentNode.RemoveChild(element);
        }

        protected virtual string GetInputType() {
            return inputType;
        }

        protected virtual void ApplyValue(IRenderContext context, XmlElement sourceElement, XmlElement htmlElement) {
            string value = context.Resolve<string>(sourceElement.Attributes["Value"].Value);
            SetAttribute(htmlElement, "value", value);
        }

        protected override void DoPrepare(XmlElement element, IResolutionRegistry registry) {
            if (element.HasAttribute("Value"))
                registry.RequestResolution(element.Attributes["Value"].Value);
        }
    }
}
