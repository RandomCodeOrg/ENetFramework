using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.Pluto.Statements {
    public class CompiledStatementToken : CompiledToken {


        private CompiledStatement compiledStatement;
        private readonly CDI.CDIContainer container;

        private bool injected = false;

        private IDictionary<string, FieldInfo> variableFields = new Dictionary<string, FieldInfo>();

        public CompiledStatementToken(CDI.CDIContainer container, Type t) {
            compiledStatement = (CompiledStatement) t.GetConstructor(new Type[0]).Invoke(new object[0]);
            this.container = container;
        }


        protected override object DoEvaluate() {
            if (!injected) {
                container.Inject(compiledStatement);
                injected = true;
            }

            return compiledStatement.Evaluate();
        }

        protected override void SetVariables(IDictionary<string, object> variables) {
            base.SetVariables(variables);
            foreach(string name in variables.Keys) {
                if (!variableFields.ContainsKey(name)) {
                    variableFields[name] = compiledStatement.GetType().GetField(name);
                }
                variableFields[name].SetValue(compiledStatement, variables[name]);
            }
        }

    }
}
