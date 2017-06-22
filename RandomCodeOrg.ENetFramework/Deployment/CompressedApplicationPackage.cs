using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;


namespace RandomCodeOrg.ENetFramework.Deployment {

    //TODO: Implement interface
    public class CompressedApplicationPackage : IDeploymentPackage {



        public IEnumerable<IDependency> Dependencies => null;

        public string AssemblyFileName => throw new NotImplementedException();

        private readonly ZipArchive archive;


        public CompressedApplicationPackage(string filePath) {
            archive = ZipFile.OpenRead(filePath);

        }


        public Stream GetAssemblyContent() {
            return null;
        }

        public Stream GetDescriptorContent() {
            return null;
        }

        public static CompressedApplicationPackage Create(string target, IDeploymentPackage source, CompressionLevel compressionLevel = CompressionLevel.Optimal) {
            using (FileStream fs = new FileStream(target, FileMode.Create)) {
                using (ZipArchive zipArchive = new ZipArchive(fs, ZipArchiveMode.Create)) {
                    ZipArchiveEntry entry = zipArchive.CreateEntry(source.AssemblyFileName, compressionLevel);
                    Stream sourceStream = source.GetAssemblyContent(); // Compresses the assembly
                    using (sourceStream) {
                        CopyTo(sourceStream, entry);
                    }
                    sourceStream = source.GetDescriptorContent();
                    if (sourceStream != null) { // Compresses the application descriptor (if provided)
                        entry = zipArchive.CreateEntry(FrameworkConstants.APP_DESCRIPTOR_FILE_NAME, compressionLevel);
                        using (sourceStream) {
                            CopyTo(sourceStream, entry);
                        }
                    }
                    string targetName;
                    foreach (IDependency dependency in source.Dependencies) { // Compresses the required dependencies
                        sourceStream = dependency.GetContent();
                        if (sourceStream != null) {
                            targetName = string.Format("Bin/{0}", dependency.Name);
                            entry = zipArchive.CreateEntry(targetName, compressionLevel);
                            using (sourceStream) {
                                CopyTo(sourceStream, entry);
                            }
                        }
                    }
                }
            }
            return new CompressedApplicationPackage(target);
        }


        private static void CopyTo(Stream sourceStream, ZipArchiveEntry target) {
            using (Stream targetStream = target.Open()) {
                sourceStream.CopyTo(targetStream);
            }
        }



    }
}
