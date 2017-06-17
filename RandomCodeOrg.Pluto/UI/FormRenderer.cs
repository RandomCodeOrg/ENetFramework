using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using RandomCodeOrg.ENetFramework.UI;

namespace RandomCodeOrg.Pluto.UI {
    public class FormRenderer : FrameworkRendererBase {

        public FormRenderer() : base("Form") {

        }

        protected const string METHOD_ATTRIBUTE = "Method";
        protected const string HTML_METHOD_ATTRIBUTE = "method";

        protected override void DoPrepare(XmlElement element, IResolutionRegistry registry) {

        }

        protected override void DoRender(IRenderContext context, XmlDocument doc, XmlElement element) {
            if (!CheckRendered(context, element))
                return;

            var form = MakeHtmlTag("form", element.ParentNode, element);
            string method = "post";
            if (element.HasAttribute(METHOD_ATTRIBUTE)) {
                method = element.GetAttribute(METHOD_ATTRIBUTE);
                method = method.ToLower();
                if (string.IsNullOrWhiteSpace(method))
                    method = "post";
            }
            CopyHtmlAttributes(element, form);
            CopyChildren(element, form);
            SetAttribute(form, HTML_METHOD_ATTRIBUTE, method);

            Recurse(context, doc, element);

            Remove(element);

        }


    }
}
