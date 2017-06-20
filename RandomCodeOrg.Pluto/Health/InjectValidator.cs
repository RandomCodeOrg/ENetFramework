using RandomCodeOrg.ENetFramework.Container;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.Pluto.Health {
    class InjectValidator : IAttributeValidator {


        public Type AttributeType => typeof(InjectAttribute);

        public void Validate(Type t, MemberInfo member, Attribute attribute, ValidationReport report) {
            Type typeToInject = null;
            if(member is PropertyInfo) {
                typeToInject = member.As<PropertyInfo>().PropertyType;
            }else if(member is FieldInfo) {
                typeToInject = member.As<FieldInfo>().FieldType;
            }
            if (typeToInject == null)
                return;
            if (typeToInject.IsInterface)
                return;
            if (typeToInject.IsClass) {
                ManagedAttribute managedAttr = typeToInject.GetCustomAttribute<ManagedAttribute>();
                if (managedAttr == null)
                    report.Register(new ValidationEntry(ValidationEntryType.Error, member, "Can't inject instances of '{0}' becuase it is not a managed resource.", typeToInject.FullName));
                ScopeAttribute scopeAttr = typeToInject.GetCustomAttribute<ScopeAttribute>();
                if(scopeAttr == null || scopeAttr.Scope != Lifetime.ApplicationScoped) {
                    report.Register(new ValidationEntry(ValidationEntryType.Error, member, "Can't inject instances of '{0}' because the resource is not application scoped. Consider injecting it by an interface implemented by the required resource or change its scope.", typeToInject.FullName));
                } else {
                    if (typeToInject.IsAbstract) {
                        report.Register(new ValidationEntry(ValidationEntryType.Error, member, "Can't inject instances of '{0}' because it is abstract.", typeToInject.FullName));
                    } else if(typeToInject.GetConstructor(new Type[0]) == null) {
                        report.Register(new ValidationEntry(ValidationEntryType.Error, member, "Can't inject instances of '{0}' because the class does not provide an accessible default constructor."));
                    }
                }
            }
        }
    }
}
