// TODO: Add more tests!
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cast.NET.Tests
{
    [TestClass]
    public class CastTests
    {
        [TestMethod]
        public void CastReadWriteTest()
        {
            var dir = Path.Combine(Environment.GetEnvironmentVariable("CAST_TEST_DIR") ?? "tests", "testfiles", "CastReadWriteTests");

            // At the very least, Cast.NET should produce identical output for the given input
            // if a file changes, we've messed something up
            foreach (var file in Directory.EnumerateFiles(dir, "*.cast"))
            {
                var cast = CastReader.Load(file);
                using var memStream = new MemoryStream();
                CastWriter.Save(memStream, cast);
                CollectionAssert.AreEqual(File.ReadAllBytes(file), memStream.ToArray());
            }
        }
    }
}
