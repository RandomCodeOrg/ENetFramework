using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.Mercury.Data {
    
    public class Visitor {

        [Key]
        public int Id { get; set; }
        
        public DateTime Timestamp { get; set; }

        public int ResourceId { get; set; }

    }
}
