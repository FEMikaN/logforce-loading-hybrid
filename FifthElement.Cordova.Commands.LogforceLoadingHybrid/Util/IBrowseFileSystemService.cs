namespace FifthElement.Cordova.Commands.LogforceLoadingHybrid.Util
{
    public interface IBrowseFileSystemService
    {
        bool OpenFile(string path, string filter, out string fileName);
        bool OpenFiles(string path, string filter, out string[] fileNames);
        bool OpenDirectory(out string pathName);
    }
}
