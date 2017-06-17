using RandomCodeOrg.ENetFramework.Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.Pluto.Statements {
    public abstract class CompiledStatement {


        public void Initialize() {
            DoInitialize();

        }

        protected virtual void DoInitialize() {

        }

        protected abstract object DoEvaluate();

        public object Evaluate() {
            return DoEvaluate();
        }


        protected IBinding Binding(object instance, string name) {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));
            Type instanceType = instance.GetType();
            PropertyInfo propInfo = instanceType.GetProperty(name);
            if (propInfo != null)
                return new PlutoBinding(instance, name, string.Format("param{0}", name.GetHashCode()), propInfo);
            FieldInfo fieldInfo = instanceType.GetField(name);
            if (fieldInfo != null)
                return new PlutoBinding(instance, name, string.Format("param{0}", name.GetHashCode()), fieldInfo);
            throw new ArgumentException("No property or field found for the given name.");
        }
        

    }
}
