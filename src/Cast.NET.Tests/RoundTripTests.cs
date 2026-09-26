namespace CastNet.Tests;

[TestClass]
public sealed class RoundTripTests
{
    [TestMethod]
    public void SavingLoadedFilesProducesIdenticalBytes()
    {
        var directory = Environment.GetEnvironmentVariable("CAST_TEST_DIR");

        for (var search = new DirectoryInfo(AppContext.BaseDirectory); directory is null && search is not null; search = search.Parent)
        {
            if (Directory.Exists(Path.Combine(search.FullName, "testfiles")))
                directory = Path.Combine(search.FullName, "testfiles");
        }

        var paths = directory is null ? [] : Directory.GetFiles(directory, "*.cast");

        if (paths.Length == 0)
            Assert.Inconclusive("No test files found. Place .cast files in a 'testfiles' folder at the repository root or set CAST_TEST_DIR.");

        foreach (var path in paths)
        {
            var expected = File.ReadAllBytes(path);
            using var stream = new MemoryStream();

            CastWriter.Save(stream, CastReader.Load(new MemoryStream(expected)));

            CollectionAssert.AreEqual(expected, stream.ToArray(), Path.GetFileName(path));
        }
    }
}
