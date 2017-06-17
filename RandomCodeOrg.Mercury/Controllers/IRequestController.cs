using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.Mercury.Controllers {
    public interface IRequestController {



        string Name { get; set; }

        int RequestNumber { get; }

    }
}
