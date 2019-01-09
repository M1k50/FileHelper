namespace FileNameHelper
{
    public interface IFileNameHelper
    {
        string FullFilename { get; set; }

        string Filename { get; set; }
        string Filepath { get; set; }
        string Directory { get; set; }
    }
}