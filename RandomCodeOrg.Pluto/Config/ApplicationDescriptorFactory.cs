using slf4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RandomCodeOrg.Pluto.Config {
    public class ApplicationDescriptorFactory {


        private const string APPDESCRIPTOR_RESOURCE_NAME = "application.xml";

        private readonly ILogger logger;

        public ApplicationDescriptorFactory() {

        }


        public ApplicationDescriptorType ReadDescriptor(Assembly assembly) {
            ApplicationDescriptorType result = null;

            var stream = assembly.GetManifestResourceStream(APPDESCRIPTOR_RESOURCE_NAME);
            if (stream != null) {
                using (stream) {
                    XmlSerializer serializer = new XmlSerializer(typeof(ApplicationDescriptorType));
                    object deserializationResult = serializer.Deserialize(stream);
                    if (deserializationResult is ApplicationDescriptorType) {
                        result = deserializationResult.As<ApplicationDescriptorType>();
                    }
                }
            }
            if (result == null) // If deserialization failed or there is no app descriptor
                result = new ApplicationDescriptorType();
            ApplyDefaults(result);

            return result;
        }

        protected virtual void ApplyDefaults(ApplicationDescriptorType descriptor) {
           
        }


    }
}
