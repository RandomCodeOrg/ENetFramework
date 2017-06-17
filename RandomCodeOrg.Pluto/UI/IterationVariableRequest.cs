using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.Pluto.UI {
    internal class IterationVariableRequest {

        private readonly string variableName;
        private readonly string sourceStatement;

        public string VariableName {
            get {
                return variableName;
            }
        }

        public string SourceStatement {
            get {
                return sourceStatement;
            }
        }

        public IterationVariableRequest(string sourceStatement, string variableName) {
            this.variableName = variableName;
            this.sourceStatement = sourceStatement;
        }

    }
}
