using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.ENetFramework.UI {
    public interface IResolutionRegistry {

        bool RequestResolution(string statement);


        void RequestIterationVariable(string typeSource, string name);

    }
}
