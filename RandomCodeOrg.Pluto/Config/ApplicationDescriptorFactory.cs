using slf4net;
using System;
using System.Collections.Generic;
using System.IO;
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


        public ApplicationDescriptor ReadDescriptor(Assembly assembly) {
            var stream = assembly.GetManifestResourceStream(APPDESCRIPTOR_RESOURCE_NAME);
            return ReadDescriptor(stream, false);
        }

        public ApplicationDescriptor ReadDescriptor(Stream stream, bool leaveOpen = false) {
            ApplicationDescriptor result = null;
            if (stream != null) {
                if (!leaveOpen) {
                    using (stream) {
                        result = LoadDescriptor(stream);
                    }
                } else {
                    result = LoadDescriptor(stream);
                }
            }
            if (result == null) // If deserialization failed or there is no app descriptor
                result = new ApplicationDescriptor();
            ApplyDefaults(result);

            return result;
        }

        protected ApplicationDescriptor LoadDescriptor(Stream source) {
            ApplicationDescriptor result = null;
            if (source != null) {
                XmlSerializer serializer = new XmlSerializer(typeof(ApplicationDescriptor));
                object deserializationResult = serializer.Deserialize(source);
                if (deserializationResult is ApplicationDescriptor) {
                    result = deserializationResult.As<ApplicationDescriptor>();
                }
            }

            return result;
        }

        protected virtual void ApplyDefaults(ApplicationDescriptor descriptor) {

        }


    }
}
