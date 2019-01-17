using System;
using System.IO.Abstractions;

namespace FileNameHelper
{
    /// <summary>
    /// 
    /// </summary>
    public class FileNameHelper : IFileNameHelper
    {
        private int _counter;
        private string _counterFormat = "D2";
        private int _counterMax = 1000;
        /// <summary>
        /// Determines wether a passed directory is created if it does not exist yet.
        /// </summary>
        private bool _createMissingDirectory = false;

        /// <summary>
        /// Output filename extension.
        /// e.g. ".csv"
        /// </summary>
        private string _fileExtension = ".csv";

        /// <summary>
        /// Name of the output filename.
        /// </summary>
        private string _filename = "output";

        /// <summary>
        /// Current workingdirectory.
        /// </summary>
        private string _workingDirectory="./";

        private IFileSystem _fileSystem;


        /// <summary>
        /// Constructor setting all necessary properties to be able to retrieve a new filename without needing argruments.
        /// </summary>
        /// <param name="fullFilename"></param>
        /// <param name="workingDirectory"></param>
        /// <param name="createMissingDirectory"></param>
        /// <param name="counterMax"></param>
        /// <param name="counterFormat"></param>
        /// <param name="fileSystem"></param>
        public FileNameHelper(string fullFilename ="output.csv",
            string directory ="./",
            bool createMissingDirectory = false,
            int counterMax = 1000,
            string counterFormat="D2",
            IFileSystem fileSystem = null)
        {
            SetFileSystem(fileSystem);

            SetCounter(createMissingDirectory, counterMax, counterFormat);

            FullFilename = fullFilename;
            Directory = directory;

        }

        private void SetCounter(bool createMissingDirectory, int counterMax, string counterFormat)
        {
            _createMissingDirectory = createMissingDirectory;
            _counterMax = counterMax;
            _counterFormat = counterFormat;
        }

        private void SetFileSystem(IFileSystem fileSystem)
        {
            if (fileSystem == null)
            {
                _fileSystem = new FileSystem();
            }
            else
            {
                _fileSystem = fileSystem;
            }
        }

        /// <summary>
        /// Blank Constructor to use class as persistent tool for filenamecreation.
        /// </summary>
        public FileNameHelper()
        {
            _fileSystem = new FileSystem();
        }

        #region Interface


        public string FullFilename
        {
            get
            {
                SetAvailableFilename();

                string output = AssembleFullFilename();
                return output;
            }
            set
            {
                //string filename = _fileSystem.Path.GetFileNameWithoutExtension(value);
                SetFilename(value);

                string extension = _fileSystem.Path.GetExtension(value);
                bool extensionFound = (extension != "");
                if (extensionFound)
                {
                    _fileExtension = extension;
                }
            }
        }

        public string Filename
        { 
            get
            {
                SetAvailableFilename();

                return AssembleFilename();
            }
            set
            {
                SetFilename(value);
            }
        }

        private void SetFilename(string value)
        {
            _filename = _fileSystem.Path.GetFileNameWithoutExtension(value);
        }

        private string _filePath;

        /// <summary>
        /// Full output filepath with filename and extension.
        /// e.g. "c:\temp\dummy.csv"
        /// </summary>
        public string Filepath
        {
            set
            {
                _filePath = value;
                string directory = _fileSystem.Path.GetDirectoryName(value);
                _workingDirectory = directory;
                FullFilename = value;
            }

            get
            {
                SetAvailableFilename();

                string output = AssembleFilepath();
                return output;
            }
        }

        public string Directory
        {
            get
            {
                return _workingDirectory;
            }
            set
            {
                _workingDirectory = GetDirectory(value, _createMissingDirectory);
            }

        }
        #endregion



        #region Calculations

        private string AssembleFilepath()
        {
            string output= _fileSystem.Path.Combine(_workingDirectory, AssembleFullFilename());
            return output;
        }
        private string AssembleFilename()
        {
            string output;
            if (_counter == 0)
            {
                output = _filename;
                return output;
            }
            output = $"{_filename}_" + _counter.ToString(_counterFormat);
            return output;
        }

        private string AssembleFullFilename()
        {
            string output;
            output = AssembleFilename() + _fileExtension;
            return output;
        }

        //private bool FileExists(string filepath)
        //{
        //    return _fileSystem.File.Exists(filepath);
        //}

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
                output = AssembleFilepath();
                bool fileExists = _fileSystem.File.Exists(output);
                if (!fileExists)
                {
                    return output;
                }

                _counter++;
                bool counterIsEnd = _counter > _counterMax;
                bool startedCounterFromZero = counterStart == 0;
                bool incrLoopCounter = counterIsEnd && startedCounterFromZero;

                if (incrLoopCounter)
                {
                    loopTerminator ++;
                }

                if (_counter > _counterMax)
                {
                    _counter = 1;
                }


            }
            throw new Exception(string.Format("Max counter value exceeded {0}, no free filename found.", _counterMax));

        }

        #endregion
    }
}
