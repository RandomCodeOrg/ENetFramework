using RandomCodeOrg.Pluto.CDI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.Pluto.Sessions {
    public class Session {

        private readonly ManagedContext cdiContext = new ManagedContext();

        public ManagedContext CDIContext {
            get {
                return cdiContext;
            }
        }

        private readonly long id;

        public long Id {
            get {
                return id;
            }
        }

        private readonly DateTime created;

        public DateTime Created {
            get {
                return created;
            }
        }

        public DateTime lastUsage;

        public DateTime LastUsage {
            get {
                return lastUsage;
            }
        }

        public Session(long id) {
            this.id = id;
            created = DateTime.Now;
            lastUsage = created;
        }


        public Session Use() {
            lastUsage = DateTime.Now;
            return this;
        }

    }
}
