using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.Pluto.Statements {
    public class CompiledStatementCollection : CompiledToken {


        private readonly IEnumerable<CompiledToken> enumerable;

        public CompiledStatementCollection(IEnumerable<CompiledToken> tokens) {
            this.enumerable = tokens;
        }

        public override object Evaluate() {
            StringBuilder sb = new StringBuilder();
            foreach(CompiledToken ct in enumerable) {
                sb.AppendFormat("{0}", Evaluate(ct));
            }
            return sb.ToString();
        }

        protected object Evaluate(CompiledToken compiledToken) {
            try {
                return compiledToken.Evaluate();
            }catch(NoResultException) {
                return string.Empty;
            }
        }

    }
}
