using RandomCodeOrg.ENetFramework.Container;
using RandomCodeOrg.ENetFramework.Data;
using slf4net;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.Mercury.Data {
    
    public interface IMercuryData {

        Visitor AddVisitor(int id);
        
    }


    [PersistenceProvider]
    public class MercuryDataContext : DbContext, IMercuryData {

        public virtual DbSet<Visitor> Visitors { get; set; }

        [Inject]
        private readonly ILogger logger;

        [PostConstruct]
        public void Initialized() {
            logger.Info("Data constructed.");
        }

        public Visitor AddVisitor(int id) {
            Visitor visitor = new Visitor() { Timestamp = DateTime.Now, ResourceId = id };
            Visitors.Add(visitor);
            return visitor;
        }
        
    }
}
