using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.IO;

namespace RandomCodeOrg.Testing.Pluto {
    public static class TestHelper {
        
        public static string GetTestResource(string testResource) {
            string path = testResource.Replace('/', Path.DirectorySeparatorChar);
            return Path.Combine(TestContext.CurrentContext.TestDirectory, "Resources", path);
        }


        public static Stream ReadTestResource(string testResource) {
            return new FileStream(GetTestResource(testResource), FileMode.Open);
        }

    }
}
