using RandomCodeOrg.ENetFramework.Container;
using RandomCodeOrg.ENetFramework.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.Mercury.Data {
    
    public interface IMercuryData {

        void AddVisitor(int id);
        
    }


    [PersistenceProvider]
    public class MercuryDataContext : DbContext, IMercuryData {

        public virtual DbSet<Visitor> Visitors { get; set; }


        public MercuryDataContext()  {
            
        }

        public void AddVisitor(int id) {
            Visitor visitor = new Visitor() { Timestamp = DateTime.Now, ResourceId = id };
            Visitors.Add(visitor);
        }


    }
}
