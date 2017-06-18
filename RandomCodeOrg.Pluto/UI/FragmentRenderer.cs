using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using RandomCodeOrg.ENetFramework.UI;

namespace RandomCodeOrg.Pluto.UI {
    public class FragmentRenderer : FrameworkRendererBase {

        public FragmentRenderer() : base("Fragment", "Placeholder") {

        }

        protected override void DoPrepare(XmlElement element, IResolutionRegistry registry) {

        }

        protected override void DoRender(IRenderContext context, XmlDocument doc, XmlElement element) {
            if (!CheckRendered(context, element))
                return;

            RecurseChildren(context, doc, element);
            ReplaceWithChildren(element);
        }


    }
}
