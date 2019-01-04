using System;
using System.Activities.Statements;
using System.CodeDom;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Xml;

namespace FileHelper
{
    /// <summary>
    /// 
    /// </summary>
    public class FileHelper : IFileHelper
    {
        /// <summary>
        /// Constructor setting all necessary properties to be able to retrieve a new filename without needing argruments.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="workingDirectory"></param>
        /// <param name="createMissingDirectory"></param>
        /// <param name="counterMax"></param>
        /// <param name="counterFormat"></param>
        public FileHelper(string filename, string workingDirectory, bool createMissingDirectory = true,int counterMax = 1000, string counterFormat="00")
        {
            Filename = filename;
            WorkingDirectory = workingDirectory;
            CreateMissingDirectory = createMissingDirectory;
            CounterMax = counterMax;
            CounterFormat = CounterFormat;
        }

        /// <summary>
        /// Blank Constructor to use class as persistent tool for filenamecreation.
        /// </summary>
        public FileHelper()
        {
            
        }

        /// <summary>
        /// Determines wether a passed directory is created if it does not exist yet.
        /// </summary>
        public bool CreateMissingDirectory { get; set; } = true;

        public string CounterFormat { get; set; } = "00";

        private int Counter { get; set; }

        public int CounterMax { get; set; } = 1000;
        
        /// <summary>
        /// Name of the output filename.
        /// </summary>
        private string _filename= "";
        public string Filename
        {
            get { return _filename; }
            set
            {
                string filename = Path.GetFileNameWithoutExtension(value);
                string extension = Path.GetExtension(value);
                _filename = filename;
                FileExtension = extension;
            }
        }

        /// <summary>
        /// Output filename extension.
        /// e.g. ".csv"
        /// </summary>
        public string FileExtension { get; set; } = ".csv";

        /// <summary>
        /// Output filename and extension.
        /// e.g. "dummy.csv"
        /// </summary>
        public string FullFilename { get { return Filename + FileExtension; } }

        /// <summary>
        /// Full output filepath with filename and extension.
        /// e.g. "c:\temp\dummy.csv"
        /// </summary>
        public string FullFilepath { get { return WorkingDirectory + Filename + FileExtension; } }

        /// <summary>
        /// Full output filepath with filename and extension.
        /// e.g. "c:\temp\dummy.csv"
        /// </summary>
        public string FullFilepathCounted { get { return WorkingDirectory + Filename + string.Format(CounterFormat,Counter)+ FileExtension; } }

        private string _workingDirectory = "./";
        public string WorkingDirectory
        {
            get { return _workingDirectory; }
            set { _workingDirectory = GetDirectory(value, CreateMissingDirectory); }
        }

        /// <summary>
        /// If'target' directory exists, the name gets returned. Otherwise an Argument Exception is thrown.
        /// </summary>
        /// <param name="target">Path to directory to be checked.</param>
        /// <returns></returns>
        public string GetDirectory(string target)
        {
            return GetDirectory(target, CreateMissingDirectory);
        }

        /// <summary>
        /// If'target' directory exists, the name gets returned.
        /// If the createIfMissing argument it true a missing directory will be created.
        /// Otherwise an Argument Exception is thrown.
        /// </summary>
        /// <param name="target">Path to directory to be checked.</param>
        /// <param name="createIfMissing">If true a missing directory is created</param>
        /// <returns></returns>
        public string GetDirectory(string target, bool createIfMissing)
        {
            string output;
            bool dirExists = Directory.Exists(target);
            if (dirExists)
            {
                output = target;
                return output;
            }

            if (createIfMissing)
            {
                Directory.CreateDirectory(target);
                output = target;
                return output;
            }

            throw new ArgumentException("Specified Directory does not exist");
        }

        /// <summary>
        /// Checks if a file with same name as the property 'Filename' with 'Fileextension' exists in the 'WorkingDirectory'.
        /// If no the name is returned, else the name is exanded with a incrementing counter and returned.
        /// e.g. "dummy_01.csv"
        /// </summary>
        /// <returns></returns>
        public string GetFilename()
        {

            return GetFilename(Filename);
        }

        /// <summary>
        /// Checks if the passed 'Filename' exists in the passed 'Directory'.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public string GetFilename(string filename)
        {
            return GetFilename(filename, _workingDirectory);
        }

        /// <summary>
        /// The internal method checks if a filename exists.
        /// If yes, the counter will bei added and incremented until a free name is found, which then is returned.
        /// If the maximum counter value is exceeded and 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="workingDirectory"></param>
        /// <returns></returns>
        private string GetFilename(string filename, string workingDirectory)
        {
            string output;

            output = FullFilepath;
            if (!FileExists(output))
            {
                return output;
            }

            for (int i = 1; i <= CounterMax; i++)
            {
                Counter = i;

                output = FullFilepathCounted;
                if (!FileExists(output))
                {
                    return output;
                }
            }

            throw new Exception(string.Format("Max counter value exceeded {0}", CounterMax));
        }

        private bool FileExists(string filepath)
        {
            return File.Exists(filepath);
        }

        private string IncrementCounter()
        {
            Counter += 1;
            return Counter.ToString(CounterFormat);
        }
    }
}
