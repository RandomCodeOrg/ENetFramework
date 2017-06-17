using RandomCodeOrg.ENetFramework.Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.Pluto.Statements {
    public class PlutoBinding : IBinding {

        

        private readonly PropertyInfo property;

        private readonly FieldInfo field;

        private readonly object instance;

        private readonly string name;

        private readonly string identifier;

        protected PlutoBinding(object instance, string name, string identifier, PropertyInfo property, FieldInfo field) {
            this.property = property;
            this.field = field;
            this.instance = instance;
            this.name = name;
            this.identifier = identifier;
        }

        public PlutoBinding(object instance, string name, string identifier, PropertyInfo property) : this(instance, name, identifier, property, null) {

        }

        public PlutoBinding(object instance, string name, string identifier, FieldInfo field) : this(instance, name, identifier, null, field) {

        }

        public string Name => name;

        public string Identifier => identifier;

        public object GetValue() {
            if (property != null)
                return property.GetValue(instance);
            return field.GetValue(instance);
        }

        public void SetValue(object value) {
            if (property != null)
                property.SetValue(instance, value);
            else
                field.SetValue(instance, value);
        }
    }
}
