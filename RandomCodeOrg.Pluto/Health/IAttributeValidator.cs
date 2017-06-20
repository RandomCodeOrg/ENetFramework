using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace RandomCodeOrg.Pluto.Health {

    interface IAttributeValidator {

        Type AttributeType { get; }

        void Validate(Type t, MemberInfo member, Attribute attribute, ValidationReport report);

    }

   
}
