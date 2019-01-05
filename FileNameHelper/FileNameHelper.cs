using System;
using System.IO;

namespace FileNameHelper
{
    /// <summary>
    /// 
    /// </summary>
    public class FileNameHelper
    {
        private int _counter;
        private string _counterFormat = "00";
        private int _counterMax = 1000;

        /// <summary>
        /// Output filename extension.
        /// e.g. ".csv"
        /// </summary>
        private string _fileExtension = ".csv";

        /// <summary>
        /// Name of the output filename.
        /// </summary>
        private string _filename = "output";

        private string _workingDirectory = "./";

        /// <summary>
        /// Constructor setting all necessary properties to be able to retrieve a new filename without needing argruments.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="workingDirectory"></param>
        /// <param name="createMissingDirectory"></param>
        /// <param name="counterMax"></param>
        /// <param name="counterFormat"></param>
        public FileNameHelper(string filename ="output.csv", string workingDirectory="./", bool createMissingDirectory = true,int counterMax = 1000, string counterFormat="00")
        {
            SetFilename(filename);
            WorkingDirectory = workingDirectory;
            _createMissingDirectory = createMissingDirectory;
            _counterMax = counterMax;
            _counterFormat = counterFormat;

            GetFilename();
        }

        /// <summary>
        /// Blank Constructor to use class as persistent tool for filenamecreation.
        /// </summary>
        public FileNameHelper()
        {
            
        }

        public string WorkingDirectory
        {
            get { return _workingDirectory; }
            set { _workingDirectory = GetDirectory(value, _createMissingDirectory); }
        }

        /// <summary>
        /// Determines wether a passed directory is created if it does not exist yet.
        /// </summary>
        private bool _createMissingDirectory { get; set; } = true;

        /// <summary>
        /// The internal method checks if a filename exists.
        /// If yes, the counter will bei added and incremented until a free name is found, which then is returned.
        /// If the maximum counter value is exceeded and 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="workingDirectory"></param>
        /// <returns></returns>
        public string GetFilename()
        {
            SetAvailableFilename();

            string output = Filename();
            return output;
        }

        public string GetFilepath()
        {
            SetAvailableFilename();

            string output = Filepath();
            return output;
        }

        public void SetFilename(string name)
        {
            string filename = Path.GetFileNameWithoutExtension(name);
            string extension = Path.GetExtension(name);

            _filename = filename;
            _fileExtension = extension;

        }

        private bool FileExists(string filepath)
        {
            return File.Exists(filepath);
        }

        private string Filename()
        {
            if (_counter == 0)
            {
                return _filename + _fileExtension;
            }
            return _filename + string.Format(_counterFormat, _counter) + _fileExtension;
        }

        /// <summary>
        /// Full output filepath with filename and extension.
        /// e.g. "c:\temp\dummy.csv"
        /// </summary>
        private string Filepath()
        {
            string filename = Filename();
            return _workingDirectory + filename;
        }

        /// <summary>
        /// If'target' directory exists, the name gets returned.
        /// If the createIfMissing argument it true a missing directory will be created.
        /// Otherwise an Argument Exception is thrown.
        /// </summary>
        /// <param name="target">Path to directory to be checked.</param>
        /// <param name="createIfMissing">If true a missing directory is created</param>
        /// <returns></returns>
        private string GetDirectory(string target, bool createIfMissing)
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

        private void SetAvailableFilename()
        {
            string filepath;
            int counterStart = _counter;
            int loopTerminator = 0;

            while (loopTerminator == 0)
            {
                filepath = Filepath();
                if (!FileExists(filepath))
                {
                    break;
                }
                _counter++;

                if (_counter == _counterMax)
                {
                    _counter = 1;
                }

                if (counterStart == _counter)
                {
                    throw new Exception(string.Format("Max counter value exceeded {0}, no free filename found.", _counterMax));
                }
            }
        }
    }
}
