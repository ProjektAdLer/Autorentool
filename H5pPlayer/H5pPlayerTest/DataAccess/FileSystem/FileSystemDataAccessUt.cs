using System.IO.Abstractions;
using H5pPlayer.DataAccess.FileSystem;
using NSubstitute;

namespace H5pPlayerTest.DataAccess.FileSystem;



[TestFixture]
public class FileSystemDataAccessUt
{


    
    [Test]
    public void DeleteDirectoryRecursively_DeleteAllFilesAndDirectoriesInDirectory()
    {        
        var directoryPath = @"C:\TestDirectory";
        var mockFileSystem = FakeFileSystemWithFilesAndTwoDirectoryDepths(directoryPath);
        FakeFileSystemConfiguration(mockFileSystem, directoryPath);
        var systemUnderTest = CreateTestableFileSystemDataAccess(mockFileSystem);
        
        systemUnderTest.DeleteDirectoryRecursively(directoryPath);

        Assert.That(mockFileSystem.Directory.GetFiles(directoryPath), Is.Empty);
        Assert.That(mockFileSystem.Directory.GetDirectories(directoryPath), Is.Empty);
    }




    [Test]
    public void DeleteDirectoryRecursively_ThrowsExceptionIfDirectoryDoesNotExist()
    {
        var directoryPath = @"C:\NonExistentDirectory";
        var mockFileSystem = Substitute.For<IFileSystem>();
        mockFileSystem.Directory.Exists(directoryPath).Returns(false);

        var systemUnderTest = CreateTestableFileSystemDataAccess(mockFileSystem);

        Assert.Throws<DirectoryNotFoundException>(() => systemUnderTest.DeleteDirectoryRecursively(directoryPath));
    }


    [Test]
    public void DeleteDirectoryRecursively_DeletesEmptyDirectory()
    {
        var emptyDirectory = @"C:\EmptyDirectory";
        var mockFileSystem = Substitute.For<IFileSystem>();
        mockFileSystem.Directory.Exists(emptyDirectory).Returns(true);
        mockFileSystem.Directory.GetFiles(emptyDirectory).Returns(Array.Empty<string>());
        mockFileSystem.Directory.GetDirectories(emptyDirectory).Returns(Array.Empty<string>());

        var systemUnderTest = CreateTestableFileSystemDataAccess(mockFileSystem);
    
        systemUnderTest.DeleteDirectoryRecursively(emptyDirectory);

        mockFileSystem.Directory.Received(1).Delete(emptyDirectory, true);
    }
    
    
    [Test]
    public void DeleteDirectoryRecursively_ThrowsExceptionIfAccessDenied()
    {
        var restrictedDirectory = @"C:\RestrictedDirectory";
        var mockFileSystem = Substitute.For<IFileSystem>();
        mockFileSystem.Directory.Exists(restrictedDirectory).Returns(true);
        mockFileSystem.Directory.When(d => d.Delete(restrictedDirectory, true))
            .Do(x => throw new UnauthorizedAccessException());

        var systemUnderTest = CreateTestableFileSystemDataAccess(mockFileSystem);

        Assert.Throws<UnauthorizedAccessException>(() => systemUnderTest.DeleteDirectoryRecursively(restrictedDirectory));
    }
    
  
    
    [Test]
    public void DeleteDirectoryRecursively_ThrowsExceptionIfPathIsNull()
    {
        var systemUnderTest = CreateTestableFileSystemDataAccess();

        Assert.Throws<ArgumentNullException>(() => systemUnderTest.DeleteDirectoryRecursively(null));
    }
    
     
    [Test]
    public void DeleteDirectoryRecursively_ThrowsExceptionIfPathIsEmpty()
    {
        var systemUnderTest = CreateTestableFileSystemDataAccess();

        Assert.Throws<ArgumentException>(() => systemUnderTest.DeleteDirectoryRecursively(string.Empty));
    }
    

    
    private static FileSystemDataAccess CreateTestableFileSystemDataAccess(IFileSystem mockFileSystem = null)
    {
        mockFileSystem ??= Substitute.For<IFileSystem>();
        return new FileSystemDataAccess(mockFileSystem);
    }
    
    
    public IFileSystem FakeFileSystemWithFilesAndTwoDirectoryDepths(string baseDirectory)
    {
        var fakeFileSystem = Substitute.For<IFileSystem>();
        
        fakeFileSystem.Directory.CreateDirectory(baseDirectory);
        fakeFileSystem.File.WriteAllText(Path.Combine(baseDirectory, "file1.txt"), "Text-Datei-Inhalt");
        fakeFileSystem.File.WriteAllText(Path.Combine(baseDirectory, "file2.jpg"), "JPG-Datei-Inhalt");
        fakeFileSystem.File.WriteAllText(Path.Combine(baseDirectory, "file3.pdf"), "PDF-Datei-Inhalt");

        var subDirectory1 = Path.Combine(baseDirectory, "SubDirectory1");
        fakeFileSystem.Directory.CreateDirectory(subDirectory1);
        fakeFileSystem.File.WriteAllText(Path.Combine(subDirectory1, "subFile1.docx"), "DOCX-Datei-Inhalt");
        fakeFileSystem.File.WriteAllText(Path.Combine(subDirectory1, "subFile2.mp3"), "MP3-Datei-Inhalt");
        
        var subDirectory2 = Path.Combine(baseDirectory, "SubDirectory2");
        fakeFileSystem.Directory.CreateDirectory(subDirectory2);
        fakeFileSystem.File.WriteAllText(Path.Combine(subDirectory2, "subFile3.png"), "PNG-Datei-Inhalt");
        fakeFileSystem.File.WriteAllText(Path.Combine(subDirectory2, "subFile4.xlsx"), "XLSX-Datei-Inhalt");

        return fakeFileSystem;
    }
    
    private static void FakeFileSystemConfiguration(IFileSystem mockFileSystem, string directoryPath)
    {
        mockFileSystem.Directory.Exists(directoryPath).Returns(true);
    }
    
}