using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.Pluto.Statements {
    public abstract class Token {

        public const char STATEMENT_INDICATOR = '$';
        public const char MASK = '\\';
        public const char NEW_LINE = 'n';
        public const char TAB = 't';
        public const char QUOTE = '"';
        public const char SINGLE_QUOTE = '\'';
        public const char SEPARATOR = '.';
        public const char ARGUMENT_START = '(';
        public const char ARGUMENT_END = ')';
        public const char ARG_SEPARATOR = ',';
        public const char SPACE = ' ';

        protected readonly IList<Token> children = new List<Token>();

        public IList<Token> Children {
            get {
                return children;
            }
        }
        

        protected readonly Token parent;

        public Token Parent {
            get {
                return parent;
            }
        }

        protected Token(Token parent) {
            this.parent = parent;
            if (parent != null)
                parent.children.Add(this);
        }

        public abstract Token Append(char c);
        
        
    }
}
