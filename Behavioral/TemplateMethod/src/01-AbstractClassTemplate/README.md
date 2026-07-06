# Pure Template Method

Pure Template Method demonstrates the strict form of the pattern: the base class owns the entire algorithm sequence, and subclasses only supply the required step implementations.

## Code Walkthrough

- `ReportTemplate.Generate()` is the template method. It fixes the execution order as header, body, then footer.
- `BuildHeader`, `BuildBody`, and `BuildFooter` are the only variation points.
- `SalesReportTemplate` provides concrete step content without changing the overall workflow.

```csharp
public abstract class ReportTemplate
{
    public string Generate() => BuildHeader() + BuildBody() + BuildFooter();

    protected abstract string BuildHeader();
    protected abstract string BuildBody();
    protected abstract string BuildFooter();
}
```

## How It Differs

- Compared to `02-HookMethods`, this variant has no optional extension points.
- The algorithm is fully deterministic because derived types can only replace required steps.
- This is the best fit when every execution must follow the exact same shape.

## UML

```mermaid
classDiagram
    class ReportTemplate {
      +Generate() string
      #BuildHeader() string
      #BuildBody() string
      #BuildFooter() string
    }

    class SalesReportTemplate {
      #BuildHeader() string
      #BuildBody() string
      #BuildFooter() string
    }

    ReportTemplate <|-- SalesReportTemplate
```

## Example Flow

```mermaid
sequenceDiagram
    participant Client
    participant Template as ReportTemplate
    participant Sales as SalesReportTemplate

    Client->>Template: Generate()
    Template->>Sales: BuildHeader()
    Template->>Sales: BuildBody()
    Template->>Sales: BuildFooter()
    Template-->>Client: final report text
```
