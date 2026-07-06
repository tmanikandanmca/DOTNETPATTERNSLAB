# Hook Methods

Hook Methods demonstrate the flexible Template Method variant where the base class still owns the sequence, but optional extension points can add behavior safely.

## Code Walkthrough

- `Pipeline.Run()` is the template method and remains the only place that controls execution order.
- `Start`, `Execute`, and `Finish` are required steps every subclass must provide.
- `BeforeExecute` and `AfterExecute` are hook methods with default no-op implementations.
- `AuditPipeline` overrides `AfterExecute` to add extra behavior without copying the full algorithm.

```csharp
public abstract class Pipeline
{
    public string Run()
    {
        Start(steps);
        BeforeExecute(steps);
        Execute(steps);
        AfterExecute(steps);
        Finish(steps);
    }

    protected virtual void BeforeExecute(IList<string> steps) { }
    protected virtual void AfterExecute(IList<string> steps) { }
}
```

## How It Differs

- Compared to `01-PureTemplateMethod`, this variant adds optional hooks for extension.
- Default hook behavior is safe because doing nothing still preserves a valid algorithm.
- Subclasses can inject extra behavior without taking ownership of the full workflow.

## UML

```mermaid
classDiagram
    class Pipeline {
      +Run() string
      #Start(steps IList~string~)
      #BeforeExecute(steps IList~string~)
      #Execute(steps IList~string~)
      #AfterExecute(steps IList~string~)
      #Finish(steps IList~string~)
    }

    class AuditPipeline {
      #Start(steps IList~string~)
      #Execute(steps IList~string~)
      #AfterExecute(steps IList~string~)
      #Finish(steps IList~string~)
    }

    Pipeline <|-- AuditPipeline
```

## Example Flow

```mermaid
sequenceDiagram
    participant Client
    participant Template as Pipeline
    participant Audit as AuditPipeline

    Client->>Template: Run()
    Template->>Audit: Start()
    Template->>Audit: BeforeExecute()
    Template->>Audit: Execute()
    Template->>Audit: AfterExecute()
    Template->>Audit: Finish()
    Template-->>Client: ordered pipeline output
```
