using System.IO;

namespace FifthElement.LogforceLoadingHybrid.Core.Util
{
    public class TempFileStream : FileStream
    {
        public TempFileStream()
            : base(Path.GetTempFileName(), FileMode.Create, FileAccess.ReadWrite, FileShare.Read, 4096, FileOptions.DeleteOnClose) { }
        public TempFileStream(FileAccess access)
            : base(Path.GetTempFileName(), FileMode.Create, access, FileShare.Read, 4096, FileOptions.DeleteOnClose) { }
        public TempFileStream(FileAccess access, FileShare share)
            : base(Path.GetTempFileName(), FileMode.Create, access, share, 4096, FileOptions.DeleteOnClose) { }
        public TempFileStream(FileAccess access, FileShare share, int bufferSize)
            : base(Path.GetTempFileName(), FileMode.Create, access, share, bufferSize, FileOptions.DeleteOnClose) { }
    }

    public class PricingtableFileStream : FileStream
    {
        public PricingtableFileStream()
            : base(Path.Combine(AppConfiguration.ConfigurationsPath, Constants.PricingTableFilename), FileMode.Create, FileAccess.ReadWrite, FileShare.Read, 4096) { }

        public PricingtableFileStream(FileAccess fileAccess)
            : base(Path.Combine(AppConfiguration.ConfigurationsPath, Constants.PricingTableFilename), FileMode.Create, fileAccess, FileShare.Read, 4096) { }

        public string Filename { get { return Path.Combine(AppConfiguration.ConfigurationsPath, Constants.PricingTableFilename); } }
    }

    public class ParametertableFileStream : FileStream
    {
        public ParametertableFileStream(FileAccess fileAccess)
            : base(Path.Combine(AppConfiguration.ConfigurationsPath, Constants.CodeTableFilename), FileMode.Create, fileAccess, FileShare.Read, 4096) { }

        public string Filename { get { return Path.Combine(AppConfiguration.ConfigurationsPath, Constants.CodeTableFilename); } }
    }

}
