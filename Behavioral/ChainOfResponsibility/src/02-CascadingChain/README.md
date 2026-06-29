# Cascading Chain

Cascading chain models the version of Chain of Responsibility where every handler runs in sequence and each step feeds the next one.

## Code Walkthrough

- `IHandler` is the shared abstraction for transformation steps.
- `TrimHandler`, `UpperCaseHandler`, and `SuffixHandler` each transform the input independently.
- `CascadingChain` applies every handler in order and returns the final transformed value.
- `CascadingChainDemo` captures the trace so the sequence is easy to inspect.

## How It Differs

- Cascading chain has no early exit.
- Every handler contributes to the final output.
- Order matters because each step receives the previous step's result.

## UML

```mermaid
classDiagram
    direction LR

    class IHandler {
        <<interface>>
        +Handle(input) string
    }

    class TrimHandler {
        +Handle(input) string
    }

    class UpperCaseHandler {
        +Handle(input) string
    }

    class SuffixHandler {
        +Handle(input) string
    }

    class CascadingChain {
        +Handle(input, trace) string
    }

    IHandler <|.. TrimHandler
    IHandler <|.. UpperCaseHandler
    IHandler <|.. SuffixHandler
    CascadingChain o-- IHandler
```

## Example Flow

```mermaid
sequenceDiagram
    participant Client
    participant Chain as CascadingChain
    participant Trim as TrimHandler
    participant Upper as UpperCaseHandler
    participant Suffix as SuffixHandler

    Client->>Chain: Handle("  request  ")
    Chain->>Trim: Handle(input)
    Trim-->>Chain: "request"
    Chain->>Upper: Handle("request")
    Upper-->>Chain: "REQUEST"
    Chain->>Suffix: Handle("REQUEST")
    Suffix-->>Chain: "REQUEST_DONE"
    Chain-->>Client: final result
```