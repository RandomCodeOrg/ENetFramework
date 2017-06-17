using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.Pluto.Statements {
    public abstract class CompiledToken {

        
        protected CompiledToken() {

        }

        public object Evaluate(IDictionary<string, object> variables) {
            SetVariables(variables);
            return DoEvaluate();
        }

        protected abstract object DoEvaluate();

        protected virtual void SetVariables(IDictionary<string, object> variables) {

        }

    }

    public class CompiledLiteralToken : CompiledToken {

        private readonly object value;

        public CompiledLiteralToken(object value) {
            this.value = value;
        }

        protected override object DoEvaluate() {
            return value;
        }
    }

}
