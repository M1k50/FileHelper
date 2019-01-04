namespace FileHelper
{
    public interface IFileHelper
    {
        string CounterFormat { get; set; }
        int CounterMax { get; set; }
        bool CreateMissingDirectory { get; set; }
        string FileExtension { get; set; }
        string Filename { get; set; }
        string FullFilename { get; }
        string FullFilepath { get; }
        string FullFilepathCounted { get; }
        string WorkingDirectory { get; set; }

        string GetDirectory(string target);
        string GetDirectory(string target, bool createIfMissing);
        string GetFilename();
        string GetFilename(string filename);
    }
}