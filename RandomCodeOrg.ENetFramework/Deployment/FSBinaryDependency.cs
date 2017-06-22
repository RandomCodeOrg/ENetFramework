using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.ENetFramework.Deployment {
    public class FSBinaryDependency : IDependency {

        private readonly string path;
        private readonly string name;


        public string Name => name;

     
        public FSBinaryDependency(string filePath) {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("The file could not be found.", filePath);
            path = filePath;
            name = new FileInfo(filePath).Name;
        }


        public Stream GetContent() {
            return new FileStream(path, FileMode.Open, FileAccess.Read);
        }



    }
}
