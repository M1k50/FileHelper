using Xunit;


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

        public IFileNameHelper TestFileHelper()
        {
            string filename = "Testname.txt";
            string dir = "c:/temp/";
            bool createDir = false;
            int counterMax = 999;
            string counterFormat = "000";

            FileNameHelper nameHelper = new FileNameHelper(filename, dir, createDir, counterMax, counterFormat);

            return nameHelper;
        }

        [Fact]
        public void ConstructorWithArguments_FilenameOk()
        {
            IFileNameHelper helper = TestFileHelper();

            //Arrange
            string expectedFilename = "Testname.txt";

            //Act
            string actualFilename = helper.Filename;

            //Assert
            Assert.Equal(expectedFilename, actualFilename);
        }

        [Fact]
        public void ConstructorWithArguments_Filepath()
        {
            IFileNameHelper helper = TestFileHelper();

            //Arrange
            string expectedFilepath = "c:/temp/Testname.txt";

            //Act
            string actualFilepath = helper.Filepath;

            //Assert
            Assert.Equal(expectedFilepath, actualFilepath);
        }

    }
}
