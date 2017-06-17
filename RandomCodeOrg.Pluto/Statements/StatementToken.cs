using RandomCodeOrg.ENetFramework.Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.Pluto.Statements {
    public class StatementToken : Token {

        private readonly StringBuilder sb = new StringBuilder();

        private bool isMasked = false;

        public string Statement {
            get {
                return sb.ToString();
            }
        }

        public StatementToken(RootToken parent) : base(parent) {
        }

        public override Token Append(char c) {
            if (c.Equals(STATEMENT_INDICATOR)) {
                if (isMasked) {
                    sb.Append(c);
                    isMasked = false;
                    return this;
                } else {
                    return parent;
                }
            }else if (c.Equals(MASK)) {
                if (isMasked) {
                    sb.Append(MASK);
                    isMasked = false;
                    return this;
                } else {
                    isMasked = true;
                    return this;
                }
            }else if (c.Equals(SINGLE_QUOTE)) {
                if (isMasked) {
                    sb.Append(SINGLE_QUOTE);
                    isMasked = false;
                    return this;
                }else {
                    sb.Append(QUOTE);
                    return this;
                }
            }
            if (isMasked) {
                isMasked = false;
                sb.Append(MASK);
            }
            sb.Append(c);
            return this;
        }
    }
}
