namespace FileNameHelper
{
    public interface IFileNameHelper
    {
        string Filename { get; set; }
        string Filepath { get; }
        string WorkingDirectory { get; set; }
    }
}