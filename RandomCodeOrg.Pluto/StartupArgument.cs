using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.Pluto {
    public sealed class StartupArgument {


        public static readonly StartupArgument DISABLE_EXTRACTION = new StartupArgument("DisableExtraction");
        

        private readonly string flagName;

        public string FlagName {
            get {
                return flagName;
            }
        }

        private StartupArgument(string flagName) {
            this.flagName = flagName;
        }

        public static implicit operator string(StartupArgument argument) {
            return argument.flagName;
        }

        

    }
}
