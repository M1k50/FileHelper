using Xunit;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;

// http://www.borismod.net/2015/08/how-to-mock-file-system-in-tests.html

namespace FileNameHelper.Tests
{
    public class FileNameHelperTests
    {
        [Fact]
        public void ConstructorNoArguments_InvocesStandard()
        {
            //Arrange
            IFileNameHelper helper =new FileNameHelper();
            string expectedFilename = "output.csv";
            string expectedFilepath = "./output.csv";
            string expectedWorkingDir = "./";

            //Act
            string actualFilename = helper.Filename;
            string actualFilepath = helper.Filepath;
            string actualWorkingDir = helper.WorkingDirectory;

            //Assert
            Assert.Equal(expectedFilename,actualFilename);
            Assert.Equal(expectedFilepath,actualFilepath);
            Assert.Equal(expectedWorkingDir,actualWorkingDir);
        }

        [Fact]
        public void ConstructorWithArguments_FilenameOk()
        {
            //Arrange
            string name = "Testname.txt";
            IFileNameHelper helper = new FileNameHelper(filename: name);
            string expectedFilename = "Testname.txt";

            //Act
            string actualFilename = helper.Filename;

            //Assert
            Assert.Equal(expectedFilename, actualFilename);
        }

        [Fact]
        public void ConstructorWithArguments_FilepathOK()
        {
            //Arrange
            IFileSystem mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>()
            {
                {@"c:\temp\",new MockDirectoryData()},
                {@"c:\temp\Testname.txt",new MockFileData("dummy") }
            });

            string name = "Testname.txt";
            string path = "c:/temp/";
            IFileNameHelper helper = new FileNameHelper(filename: name, workingDirectory:path,createMissingDirectory:false,fileSystem: mockFileSystem);


            string expectedFilepath = "c:/temp/Testname.txt";

            //Act
            string actualFilepath = helper.Filepath;

            //Assert
            Assert.Equal(expectedFilepath, actualFilepath);
        }

    }
}
