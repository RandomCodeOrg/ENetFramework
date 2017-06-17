using RandomCodeOrg.ENetFramework.Statements;
using RandomCodeOrg.Pluto.Statements.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RandomCodeOrg.Pluto.Statements {
    public class PlutoStatementParser : IStatementParser {

        private readonly PlutoStatementCompiler compiler;

        public PlutoStatementParser(PlutoStatementCompiler compiler) {
            this.compiler = compiler;
        }

        public bool IsStatement(string value) {
            return Parse(value).Children.Any(t => t is StatementToken);
        }

        
        public CompiledToken Compile(IDictionary<string, Type> variables, string value) {
           
            return Parse(value).Compile(variables, compiler);
        }

        public RootToken Parse(string value) {
            RootToken root = new RootToken();
            Token current = root;
            int i = 0;
            foreach (char c in value.ToCharArray()) {
                try {
                    current = current.Append(c);
                } catch (StatementParseException e) {
                    throw new StatementParseException(
                        string.Format(
                            "Invalid statement. Refer to the inner exception for more details. Error at index: {0}",
                            i
                        ),
                        e
                    );
                }
                i++;
            }
            return root;
        }

        
    }
}
