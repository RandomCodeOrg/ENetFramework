using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.Pluto.Statements {
    public class CompiledToken {

        private readonly Func<object> function;
        

        public CompiledToken(Func<object> function) {
            this.function = function;
        }

        protected CompiledToken() {

        }

        public virtual object Evaluate() {
            return function();
        }

    }
}
