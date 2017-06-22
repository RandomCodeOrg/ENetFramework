using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.ENetFramework {
    internal class StartupArgument {


        public static readonly StartupArgument CONTAINER = new StartupArgument("Container");
        public static readonly StartupArgument CONTAINER_PATH = new StartupArgument("ContainerPath");
        public static readonly StartupArgument REMOTE_LOADING = new StartupArgument("RemoteLoading");
        public static readonly StartupArgument CRATE_PACKAGE = new StartupArgument("CreatePackage");

        private readonly string flagName;

        public string FlagName {
            get {
                return flagName;
            }
        }

        private StartupArgument(string flagName) {
            this.flagName = flagName;
        }

        
        public static implicit operator string(StartupArgument arg) {
            return arg.flagName;
        }

    }
}
