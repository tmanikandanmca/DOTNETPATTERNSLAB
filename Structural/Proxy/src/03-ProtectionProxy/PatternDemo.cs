namespace Structural.Proxy.ProtectionProxy.Api;

public interface IConfidentialReport
{
    string Read();
}

public sealed class ConfidentialReport : IConfidentialReport
{
    public string Read() => "Quarterly margin: 27.3%";
}

public sealed class ProtectionProxyReport : IConfidentialReport
{
    private readonly IConfidentialReport _inner;
    private readonly string _role;

    public ProtectionProxyReport(IConfidentialReport inner, string role)
    {
        _inner = inner;
        _role = role;
    }

    public string Read() => _role == "admin" ? _inner.Read() : "Access denied";
}

public static class PatternDemo
{
    public static object Create()
    {
        var report = new ConfidentialReport();
        IConfidentialReport adminView = new ProtectionProxyReport(report, "admin");
        IConfidentialReport guestView = new ProtectionProxyReport(report, "guest");

        return new
        {
            Pattern = "Proxy",
            Variant = "Protection Proxy",
            Admin = adminView.Read(),
            Guest = guestView.Read()
        };
    }
}
