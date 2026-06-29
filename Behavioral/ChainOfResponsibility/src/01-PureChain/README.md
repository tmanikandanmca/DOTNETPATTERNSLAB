# Pure Chain

Pure chain models the version of Chain of Responsibility where the request stops at the first handler that can process it.

## Code Walkthrough

- `SupportRequest` carries the request data that flows through the chain.
- `IRequestHandler` is the abstraction every handler implements, so the chain depends on the interface instead of concrete classes.
- `ValidationHandler` returns `null` when it cannot process a request and a response when it can.
- `PureChain` iterates through handlers and stops immediately after the first non-null response.
- `SingleHandler` shows a concrete handler that can still be tested independently.

## How It Differs

- Pure chain has early exit behavior.
- Only one handler owns the final response.
- Later handlers are skipped once the request is handled.

## UML

```mermaid
classDiagram
    direction LR

    class SupportRequest {
        +string Category
        +string Message
    }

    class IRequestHandler {
        <<interface>>
        +Handle(request) string?
    }

    class ValidationHandler {
        +Handle(request) string?
    }

    class SingleHandler {
        +Handle(request) string?
    }

    class PureChain {
        +Handle(request) string
    }

    IRequestHandler <|.. ValidationHandler
    IRequestHandler <|.. SingleHandler
    PureChain o-- IRequestHandler
    PureChain ..> SupportRequest
```

## Example Flow

```mermaid
sequenceDiagram
    participant Client
    participant Chain as PureChain
    participant Validator as ValidationHandler
    participant Escalation as SingleHandler

    Client->>Chain: Handle(request)
    Chain->>Validator: Handle(request)
    Validator-->>Chain: "Validated Billing"
    Chain-->>Client: first handled result
```