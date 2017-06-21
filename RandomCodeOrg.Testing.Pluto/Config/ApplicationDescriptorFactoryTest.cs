using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RandomCodeOrg.Pluto.Config;
using System.Reflection;
using static NUnit.Framework.Assert;
using static RandomCodeOrg.Testing.Pluto.TestHelper;


namespace RandomCodeOrg.Testing.Pluto.Config {
    [TestFixture]
    public class ApplicationDescriptorFactoryTest {


        private ApplicationDescriptorFactory CreateFactory() {
            return new ApplicationDescriptorFactory();
        }


        [Test]
        public void TestLoadDefaultForMissingDescriptor() {
            ApplicationDescriptorFactory factory = CreateFactory();
            var currentAssembly = Assembly.GetExecutingAssembly();
            Assume.That(currentAssembly != null);
            var result = factory.ReadDescriptor(currentAssembly);
            IsNotNull(result);
        }


        [Test]
        public void TestLoadStream() {
            var factory = CreateFactory();
            var input = ReadTestResource("Config/application.xml");
            IsNotNull(input);
            var appDescriptor = factory.ReadDescriptor(input);
            IsNotNull(appDescriptor);
        }

    }
}
