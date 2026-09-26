namespace CastNet.Examples.SEToCast;

/// <summary>
/// The entry point of the example.
/// </summary>
internal static class Program
{
    /// <summary>
    /// Converts each SEModel and SEAnim file passed on the command line to a cast file next to it.
    /// </summary>
    /// <param name="args">The files to convert.</param>
    public static void Main(string[] args)
    {
        foreach (var path in args)
        {
            var root = Path.GetExtension(path).ToLowerInvariant() switch
            {
                ".semodel" => SEModelConverter.Convert(path),
                ".seanim" => SEAnimConverter.Convert(path),
                _ => null,
            };

            if (root is null)
            {
                Console.WriteLine($"Skipped: {path}");
                continue;
            }

            CastWriter.Save(Path.ChangeExtension(path, ".cast"), root);
            Console.WriteLine($"Converted: {path}");
        }
    }
}
