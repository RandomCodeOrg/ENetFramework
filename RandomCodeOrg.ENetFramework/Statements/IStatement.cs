using System;
using System.Collections.Generic;
using System.Text;

namespace RandomCodeOrg.ENetFramework.Statements {
    public interface IStatement {

        T Evaluate<T>();

    }
}
