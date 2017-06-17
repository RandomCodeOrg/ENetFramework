using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.Pluto.Statements {
    public class CompiledStatementToken : CompiledToken {


        private CompiledStatement compiledStatement;
        private readonly CDI.CDIContainer container;

        private bool injected = false;

        public CompiledStatementToken(CDI.CDIContainer container, Type t) {
            compiledStatement = (CompiledStatement) t.GetConstructor(new Type[0]).Invoke(new object[0]);
            this.container = container;
        }


        public override object Evaluate() {
            if (!injected) {
                container.Inject(compiledStatement);
                injected = true;
            }
            return compiledStatement.Evaluate();
        }


    }
}
