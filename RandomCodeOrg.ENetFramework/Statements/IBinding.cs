using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.ENetFramework.Statements {
    public interface IBinding {

        string Name { get; }
        string Identifier { get; }
        object GetValue();
        void SetValue(object value);

    }
}
