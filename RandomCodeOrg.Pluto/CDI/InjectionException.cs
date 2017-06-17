using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.Pluto.CDI {
    public class InjectionException : Exception {

        public InjectionException(string message) : base(message){

        }

    }
}
