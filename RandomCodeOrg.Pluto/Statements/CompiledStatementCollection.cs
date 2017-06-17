using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.Pluto.Statements {
    public class CompiledStatementCollection : CompiledToken {


        private readonly IEnumerable<CompiledToken> enumerable;

        private IDictionary<string, object> variables;

        public CompiledStatementCollection(IEnumerable<CompiledToken> tokens) {
            this.enumerable = tokens;
        }

        protected override object DoEvaluate() {
            StringBuilder sb = new StringBuilder();
            foreach(CompiledToken ct in enumerable) {
                sb.AppendFormat("{0}", Evaluate(ct));
            }
            return sb.ToString();
        }

        protected object Evaluate(CompiledToken compiledToken) {
            try {
                return compiledToken.Evaluate(variables);
            }catch(NoResultException) {
                return string.Empty;
            }
        }


        protected override void SetVariables(IDictionary<string, object> variables) {
            base.SetVariables(variables);
            this.variables = variables;
        }

    }
}
