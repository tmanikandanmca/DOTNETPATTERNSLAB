namespace Singleton.UnitTests;

public class ProgramEndpointContractTests
{
    [TestCase("Creational/Singleton/src/01-BasicSingleton/Program.cs")]
    [TestCase("Creational/Singleton/src/02-ThreadSafeLock/Program.cs")]
    [TestCase("Creational/Singleton/src/03-DoubleCheckedLocking/Program.cs")]
    [TestCase("Creational/Singleton/src/04-LazyInitialization/Program.cs")]
    [TestCase("Creational/Singleton/src/05-EagerInitialization/Program.cs")]
    public void Program_DefinesRootAndExplainEndpoints_WithRequiredFields(string relativePath)
    {
        var source = File.ReadAllText(Path.Combine(GetRepoRoot(), relativePath));

        Assert.Multiple(() =>
        {
            Assert.That(source, Does.Contain("app.MapGet(\"/\","));
            Assert.That(source, Does.Contain("app.MapGet(\"/explain\","));
            Assert.That(source, Does.Contain("pattern = "));
            Assert.That(source, Does.Contain("variant = "));
            Assert.That(source, Does.Contain("summary = "));
        });
    }

    private static string GetRepoRoot()
    {
        var dir = new DirectoryInfo(TestContext.CurrentContext.TestDirectory);
        while (dir is not null && !Directory.Exists(Path.Combine(dir.FullName, ".git")))
        {
            dir = dir.Parent;
        }

        return dir?.FullName ?? throw new DirectoryNotFoundException("Repository root not found.");
    }
}
