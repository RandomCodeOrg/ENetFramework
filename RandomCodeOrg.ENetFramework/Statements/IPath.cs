using System;
using System.Collections.Generic;
using System.Text;

namespace RandomCodeOrg.ENetFramework.Statements {


    public interface IPath : IStatement{
        
        IPath Next { get; }

        bool HasNext { get; }
        
        string Component { get; }

        int Index { get; }

        bool HasIndex { get; }

        ICollection<IStatement> Arguments { get; }
        
        bool HasArguments { get; }

    }



}
