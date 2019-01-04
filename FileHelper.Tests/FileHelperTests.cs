using System;
using FileHelper;
using Xunit;

namespace FileHelper.Tests
{
    public class FileHelperTests
    {
        [Fact]
        public void ConstructorNoArguments_InvocesStandard()
        {
            //Arrange
            FileHelper helper = new FileHelper();
            bool expectedCreateDir = true;
            string expectedFilename = "";
            string expectedDir = "./";

            //Act
            bool actualCreateDir = helper.CreateMissingDirectory;
            string actualFilename = helper.Filename;
            string actualDir = helper.WorkingDirectory;

            //Assert
            Assert.Equal(expectedCreateDir,actualCreateDir);
            Assert.Equal(expectedFilename,actualFilename);
            Assert.Equal(expectedDir,actualDir);
        }

        [Fact]
        public void ConstructorWithArguments_InvocesCustom()
        {
            //Arrange
            string fullFilename = "Testname.txt";
            string filename = "Testname";
            string extension = ".txt";
            string dir = "c:/";
            bool createBool = true;

            FileHelper helper = new FileHelper(fullFilename, dir, createBool);
            string expectedFilename = filename;
            string expectedExtension = extension;
            string expectedFullFilename = fullFilename;

            //Act
            string actualFilename = helper.Filename;
            string actualExtension = helper.FileExtension;
            string actualFullFilename = helper.FullFilename;

            //Assert
            Assert.Equal(expectedFilename, actualFilename);
            Assert.Equal(expectedExtension, actualExtension);
            Assert.Equal(expectedFullFilename, actualFullFilename);
        }

        //Arrange

        //Act

        //Assert
    }
}
