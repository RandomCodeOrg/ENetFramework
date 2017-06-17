using RandomCodeOrg.ENetFramework.UI;
using RandomCodeOrg.Pluto.Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.Pluto.UI {
    class PlutoResolutionRegistry : IResolutionRegistry {

        

        private readonly ISet<string> statementsToResolve;
        private readonly PlutoStatementParser parser;
        private readonly IList<IterationVariableRequest> iterationVariableRequests;

        public PlutoResolutionRegistry(PlutoStatementParser parser, ISet<string> statementsToResolve, IList<IterationVariableRequest> iterationVariableRequests) {
            this.statementsToResolve = statementsToResolve;
            this.parser = parser;
            this.iterationVariableRequests = iterationVariableRequests;
        }

        public void RequestIterationVariable(string typeSource, string name) {
            iterationVariableRequests.Add(new IterationVariableRequest(typeSource, name));
            RequestResolution(typeSource);
        }

        public bool RequestResolution(string statement) {
            if (statementsToResolve.Contains(statement))
                return true;
            if (parser.IsStatement(statement)) {
                statementsToResolve.Add(statement);
                return true;
            }
            return false;
        }



    }
}
