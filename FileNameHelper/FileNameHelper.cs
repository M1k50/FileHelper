using System;
using System.IO.Abstractions;

namespace FileNameHelper
{
    /// <summary>
    /// 
    /// </summary>
    public class FileNameHelper : IFileNameHelper
    {
        private IFileSystem _fileSystem;

        private int _counter;
        private string _counterFormat="00";
        private int _counterMax=1000;

        /// <summary>
        /// Determines wether a passed directory is created if it does not exist yet.
        /// </summary>
        private bool _createMissingDirectory=false;

        /// <summary>
        /// Output filename extension.
        /// e.g. ".csv"
        /// </summary>
        private string _fileExtension=".csv";

        /// <summary>
        /// Name of the output filename.
        /// </summary>
        private string _filename="output";

        private string _workingDirectory="./";

        /// <summary>
        /// Constructor setting all necessary properties to be able to retrieve a new filename without needing argruments.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="workingDirectory"></param>
        /// <param name="createMissingDirectory"></param>
        /// <param name="counterMax"></param>
        /// <param name="counterFormat"></param>
        /// <param name="fileSystem"></param>
        public FileNameHelper(string filename ="output.csv", string workingDirectory="./",
            bool createMissingDirectory = false,
            int counterMax = 1000,
            string counterFormat="00",
            IFileSystem fileSystem = null)
        {
            if (fileSystem == null)
            {
                _fileSystem = new FileSystem();
            }
            else
            {
                _fileSystem = fileSystem;
            }

            _createMissingDirectory = createMissingDirectory;
            _counterMax = counterMax;
            _counterFormat = counterFormat;

            WorkingDirectory = workingDirectory;
            Filename = filename;
            
        }

        /// <summary>
        /// Blank Constructor to use class as persistent tool for filenamecreation.
        /// </summary>
        public FileNameHelper()
        {
            _fileSystem = new FileSystem();
        }

        #region Inteface

        /// <summary>
        /// The internal method checks if a filename exists.
        /// If yes, the counter will bei added and incremented until a free name is found, which then is returned.
        /// If the maximum counter value is exceeded and 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="workingDirectory"></param>
        /// <returns></returns>
        public string Filename
        {
            get
            {
                SetAvailableFilename();

                string output = AssembleFilename();
                return output;
            }
            set
            {
                string filename = _fileSystem.Path.GetFileNameWithoutExtension(value);
                string extension = _fileSystem.Path.GetExtension(value);

                _filename = filename;
                _fileExtension = extension;
            }
        }

        /// <summary>
        /// Full output filepath with filename and extension.
        /// e.g. "c:\temp\dummy.csv"
        /// </summary>
        public string Filepath
        {
            get
            {
                string output = AssembleFilepath();
                return output;
            }
        }

        public string WorkingDirectory
        {
            get { return _workingDirectory; }
            set { _workingDirectory = GetDirectory(value, _createMissingDirectory); }
        }

        #endregion

        #region Calculations

        private string AssembleFilepath()
        {
            string output = _workingDirectory + AssembleFilename();
            return output;

        }

        private string AssembleFilename()
        {
            string output;
            if (_counter == 0)
            {
                output = _filename + _fileExtension;
                return output;
            }
            output = $"{_filename}_" + string.Format(_counterFormat, _counter) + _fileExtension;
            return output;
        }

        private bool FileExists(string filepath)
        {
            return _fileSystem.File.Exists(filepath);
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
            bool dirExists = _fileSystem.Directory.Exists(target);
            if (dirExists)
            {
                output = target;
                return output;
            }

            if (createIfMissing)
            {
                _fileSystem.Directory.CreateDirectory(target);
                output = target;
                return output;
            }

            throw new ArgumentException("Specified directory does not exist");
        }

        private string SetAvailableFilename()
        {
            string output;
            int counterStart = _counter;
            int loopTerminator = 0;

            while (loopTerminator == 0)
            {
                output = Filepath;
                if (!FileExists(output))
                {
                    return output;
                }
                _counter++;

                if (_counter == _counterMax)
                {
                    _counter = 1;
                }

                if (counterStart == _counter)
                {
                    loopTerminator ++;
                }
            }
            throw new Exception(string.Format("Max counter value exceeded {0}, no free filename found.", _counterMax));

        }

        #endregion
    }
}
