using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.Pluto.Statements {
    public class LiteralToken : Token {

        private bool isMasked;

        private readonly StringBuilder sb = new StringBuilder();

        public string Text {
            get {
                return sb.ToString();
            }
        }

        public LiteralToken(RootToken parent) : base(parent) {

        }



        public override Token Append(char c) {
            if (c.Equals(STATEMENT_INDICATOR)) {
                if (isMasked) {
                    isMasked = false;
                    sb.Append(c);
                    return this;
                } else
                    return parent.Append(c);
            }else if (c.Equals(MASK)) {
                if (isMasked) {
                    isMasked = false;
                    sb.Append(c);
                    return this;
                } else {
                    isMasked = true;
                    return this;
                }
            }
            if (isMasked) {
                sb.Append(MASK);
                isMasked = false;
            }
            sb.Append(c);
            return this;
        }



    }
}
