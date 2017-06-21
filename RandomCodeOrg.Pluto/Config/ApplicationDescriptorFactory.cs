﻿using slf4net;
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


        public ApplicationDescriptorType ReadDescriptor(Assembly assembly) {
            var stream = assembly.GetManifestResourceStream(APPDESCRIPTOR_RESOURCE_NAME);
            return ReadDescriptor(stream, false);
        }

        public ApplicationDescriptorType ReadDescriptor(Stream stream, bool leaveOpen = false) {
            ApplicationDescriptorType result = null;
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
                result = new ApplicationDescriptorType();
            ApplyDefaults(result);

            return result;
        }

        protected ApplicationDescriptorType LoadDescriptor(Stream source) {
            ApplicationDescriptorType result = null;
            if (source != null) {
                XmlSerializer serializer = new XmlSerializer(typeof(ApplicationDescriptorType));
                object deserializationResult = serializer.Deserialize(source);
                if (deserializationResult is ApplicationDescriptorType) {
                    result = deserializationResult.As<ApplicationDescriptorType>();
                }
            }

            return result;
        }

        protected virtual void ApplyDefaults(ApplicationDescriptorType descriptor) {

        }


    }
}