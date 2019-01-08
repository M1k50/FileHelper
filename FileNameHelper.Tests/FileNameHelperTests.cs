using Xunit;
using System;
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
            string actualFilename = helper.FullFilename;
            string actualFilepath = helper.Filepath;
            string actualWorkingDir = helper.Directory;

            //Assert
            Assert.Equal(expectedFilename,actualFilename);
            Assert.Equal(expectedFilepath,actualFilepath);
            Assert.Equal(expectedWorkingDir,actualWorkingDir);
        }

        [Fact]
        public void ConstructorFullFilenameSet_Ok()
        {
            //Arrange
            string path = @".\Testname.txt";
            IFileNameHelper helper = new FileNameHelper(filepath: path);
            string expectedFilename = "Testname.txt";

            //Act
            string actualFilename = helper.FullFilename;

            //Assert
            Assert.Equal(expectedFilename, actualFilename);
        }

        [Fact]
        public void FullFilenameSet_MissingExtensionDefaultsToStandard()
        {
            //Arrange
            IFileNameHelper helper = new FileNameHelper();
            helper.FullFilename = @".\Testname";
            string expectedFullFilename = "Testname.csv";

            //Act
            string actualFilename = helper.FullFilename;

            //Assert
            Assert.Equal(expectedFullFilename, actualFilename);
        }

        [Fact]
        public void FullFilenameSet_WrongExtensionDefaultsToStandard()
        {
            //Arrange
            IFileNameHelper helper = new FileNameHelper();
            helper.FullFilename = @".\Testname.";
            string expectedFullFilename = "Testname.csv";

            //Act
            string actualFilename = helper.FullFilename;

            //Assert
            Assert.Equal(expectedFullFilename, actualFilename);
        }

        [Fact]
        public void ConstructorFilenameSet_Ok()
        {
            //Arrange
            string path = @".\Testname.txt";
            IFileNameHelper helper = new FileNameHelper(filepath: path);
            string expectedFilename = "Testname";

            //Act
            string actualFilename1 = helper.Filename;
            helper.Filename = "Testname.";
            string actualFilename2 = helper.Filename;


            //Assert
            Assert.Equal(expectedFilename, actualFilename1);
            Assert.Equal(expectedFilename, actualFilename2);

        }

        [Fact]
        public void ConstructorFilepathSet_Ok()
        {
            //Arrange
            IFileSystem mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>()
            {
                {@"c:\temp\",new MockDirectoryData()},
            });
            string path = @"c:\temp\Testname.txt";
            IFileNameHelper helper = new FileNameHelper(filepath: path, createMissingDirectory:false,fileSystem: mockFileSystem);

            string expectedFilepath = @"c:\temp\Testname.txt";

            //Act
            string actualFilepath = helper.Filepath;

            //Assert
            Assert.Equal(expectedFilepath, actualFilepath);
        }

        [Theory]
        [InlineData(@"c:\temp\Testname_01.txt","D2")]
        [InlineData(@"c:\temp\Testname_0001.txt", "D4")]

        public void FilnameSet_ExistingFilenameOk(string expectedFilepath,string format)
        {
            //Arrange
            IFileSystem mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>()
            {
                {@"c:\temp\",new MockDirectoryData()},
                {@"c:\temp\Testname.txt",new MockFileData("dummy") }
            });
            string path = @"c:\temp\Testname.txt";
            IFileNameHelper helper = new FileNameHelper(filepath: path, createMissingDirectory: false, fileSystem: mockFileSystem,counterFormat: format);

            //Act
            string actualFilepath = helper.Filepath;

            //Assert
            Assert.Equal(expectedFilepath, actualFilepath);
        }

        [Fact]
        public void FilepathSet_NoAvailableName_ThrowsException()
        {
            //Arrange
            IFileSystem mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>()
            {
                {@"c:\temp\",new MockDirectoryData()},
                {@"c:\temp\Testname.txt",new MockFileData("dummy") },
                {@"c:\temp\Testname_01.txt",new MockFileData("dummy") },
                {@"c:\temp\Testname_02.txt",new MockFileData("dummy") },
                {@"c:\temp\Testname_03.txt",new MockFileData("dummy") }
            });
            string path = @"c:\temp\Testname.txt";
            int maxCounter = 3;
            IFileNameHelper helper = new FileNameHelper(filepath: path,
                fileSystem: mockFileSystem,
                counterMax: maxCounter);


            //Assert
            Assert.Throws<Exception>(() => helper.Filename);
        }

        [Fact]
        public void FilepathSet_CounterCylcesThrowFullRange()
        {
            //Arrange
            IFileSystem mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>()
            {
                {@"c:\temp\",new MockDirectoryData()},
                {@"c:\temp\Testname.txt",new MockFileData("dummy") },
                {@"c:\temp\Testname_01.txt",new MockFileData("dummy") },
                {@"c:\temp\Testname_02.txt",new MockFileData("dummy") },
                {@"c:\temp\Testname_03.txt",new MockFileData("dummy") }
            });
            string path = @"c:\temp\Testname.txt";

            IFileNameHelper helper = new FileNameHelper(filepath: path,fileSystem: mockFileSystem);

            string expectedFilepath1 = @"c:\temp\Testname_04.txt";
            string expectedFilepath2 = @"c:\temp\Testname_05.txt";

            //Act
            string actualFilepath1 = helper.Filepath;
            mockFileSystem.File.Create(@"c:\temp\Testname_04.txt");
            mockFileSystem.File.Delete(@"c:\temp\Testname_02.txt");
            string actualFilepath2 = helper.Filepath;


            //Assert
            Assert.Equal(expectedFilepath1, actualFilepath1);
            Assert.Equal(expectedFilepath2, actualFilepath2);

        }

    }
}
