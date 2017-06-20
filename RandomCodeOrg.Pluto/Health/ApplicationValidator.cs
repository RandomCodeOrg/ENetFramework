using RandomCodeOrg.ENetFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.Pluto.Health {
    public class ApplicationValidator {

        private readonly ISet<IAttributeValidator> validators = new HashSet<IAttributeValidator>();

        

        public ApplicationValidator() {
            Register(new ScopeValidator());
            Register(new InjectValidator());
        }

        void Register(IAttributeValidator validator) {
            validators.Add(validator);
        }
        
        public ValidationReport Validate(Assembly assembly) {
            ValidationReport report = new ValidationReport();
            Validate(report, assembly);
            return report;
        }

        protected void Validate(ValidationReport report, Assembly assembly) {
            foreach(Type type in assembly.GetTypes()) {
                Validate(report, type);
            }
        }

        public ValidationReport Validate(Type type) {
            ValidationReport report = new ValidationReport();
            Validate(report, type);
            return report;
        }

        protected void Validate(ValidationReport report, Type type) {
            Type attributeType;
            foreach (object attribute in type.GetCustomAttributes(true)) {
                attributeType = attribute.GetType();
                foreach(IAttributeValidator attrValidator in validators) {
                    if (attrValidator.AttributeType.IsAssignableFrom(attributeType)) {
                        attrValidator.Validate(type, null, (Attribute)attribute, report);
                    }
                }
            }
            BindingFlags searchFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static;
            foreach (MemberInfo memberInfo in type.GetMembers(searchFlags)) {
                Validate(report, memberInfo);
            }
        }

        protected void Validate(ValidationReport report, MemberInfo member) {
            Type attributeType;
            foreach (object attribute in member.GetCustomAttributes(true)) {
                attributeType = attribute.GetType();
                foreach (IAttributeValidator attrValidator in validators) {
                    if (attrValidator.AttributeType.IsAssignableFrom(attributeType)) {
                        attrValidator.Validate(member.DeclaringType, member, (Attribute)attribute, report);
                    }
                }
            }
        }

        

    }
}
