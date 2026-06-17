namespace Structural.Adapter.Tests;

public class ProgramEndpointContractTests
{
    [TestCase("Structural/Adapter/src/01-ClassAdapter/Program.cs")]
    [TestCase("Structural/Adapter/src/02-ObjectAdapter/Program.cs")]
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
