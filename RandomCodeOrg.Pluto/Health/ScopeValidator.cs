using RandomCodeOrg.ENetFramework.Container;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace RandomCodeOrg.Pluto.Health {
    class ScopeValidator : IAttributeValidator {


        public Type AttributeType => typeof(ScopeAttribute);
        
        public void Validate(Type t, MemberInfo member, Attribute attribute, ValidationReport report) {
            ScopeAttribute scopeAttr = attribute.As<ScopeAttribute>();
            if(t.GetCustomAttribute<ManagedAttribute>() == null) {
                report.Register(new ValidationEntry(ValidationEntryType.Warning, t, "The class uses a scope attribute but is not managed. Consider adding [Managed] or remove the scope attribute."));
            }
        }
    }
}
