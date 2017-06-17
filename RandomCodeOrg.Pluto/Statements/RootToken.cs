using RandomCodeOrg.Pluto.Statements.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.Pluto.Statements {
    public class RootToken : Token {

        

        public RootToken() : base(null) {

        }

        public override Token Append(char c) {
            if (c.Equals(STATEMENT_INDICATOR)) {
                return new StatementToken(this);
            } else {
                return new LiteralToken(this).Append(c);
            }
        }

        public CompiledToken Compile(IDictionary<string, Type> variables, PlutoStatementCompiler compiler) {
            if (children.Count == 0)
                return new CompiledLiteralToken(string.Empty);
            if(children.Count == 1) {
                var token = children[0];
                return Compile(compiler, variables, token);
            }
            return new CompiledStatementCollection(children.Select(child => Compile(compiler, variables, child)));
        }

        protected CompiledToken Compile(PlutoStatementCompiler compiler, IDictionary<string, Type> variables, Token token) {
            if (token is StatementToken) {
                Type t = compiler.Compile(variables, token.As<StatementToken>().Statement);
                return new CompiledStatementToken(compiler.CDI, t);
            } else {
                return new CompiledLiteralToken(token.As<LiteralToken>().Text);
            }
        }

    }
}
