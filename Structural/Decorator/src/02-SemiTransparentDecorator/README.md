# 02 - Semi-Transparent Decorator

## What this variant demonstrates

A **Semi-Transparent Decorator** maintains the component's interface but also exposes additional methods or properties specific to the decorator. While still preserving polymorphic behavior, it allows clients who need decorator-specific functionality to access it directly.

This is a pragmatic middle-ground: most clients treat it as a normal component, but some clients can take advantage of the decorator's unique capabilities.

### Code focus

```csharp
// The decorator implements IMessage interface
IMessage message = new TracingDecorator(new SimpleMessage("order updated"));

// Clients can use it through the interface (transparent)
string rendered = message.Render();

// But clients CAN also access decorator-specific state if they know it's there
var tracer = message as TracingDecorator;
if (tracer != null)
{
    Console.WriteLine($"Trace ID: {tracer.TraceId}");  // Decorator-specific feature
}
```

In this variant:
- The decorator implements the same interface as the component.
- The decorator exposes additional methods or properties beyond the interface.
- Generic clients work with the interface and remain unaware of decoration.
- Specific clients can cast to the concrete decorator type to access extra functionality.
- Perfect for adding observability, debugging, or instrumentation features.

## How it differs from other Decorator variants

- Compared to `01-TransparentDecorator`: Transparent decorators hide everything, while Semi-Transparent decorators expose their specialized behavior if needed.
- Compared to `03-DynamicVsStaticDecoration`: Semi-Transparent focuses on the visibility of the decorator's features, while Dynamic/Static focuses on when decoration occurs.

## UML

```mermaid
classDiagram
    class IMessage {
        <<interface>>
        +Render() string
    }

    class SimpleMessage {
        -content: string
        +SimpleMessage(content: string)
        +Render() string
    }

    class TracingDecorator {
        -inner: IMessage
        +TracingDecorator(inner: IMessage)
        +Render() string
        +TraceId: string
    }

    class CachingDecorator {
        -inner: IMessage
        -cache: Dictionary
        +CachingDecorator(inner: IMessage)
        +Render() string
        +GetCacheHitCount() int
        +ClearCache() void
    }

    class Client

    IMessage <|.. SimpleMessage
    IMessage <|.. TracingDecorator
    IMessage <|.. CachingDecorator
    
    TracingDecorator --> IMessage : wraps
    CachingDecorator --> IMessage : wraps
    
    Client --> IMessage : uses
    Client -.->|optional cast| TracingDecorator
    Client -.->|optional cast| CachingDecorator
```

## Key Implementation Points

1. **Dual Interface**: The decorator implements the base interface and adds extra public members.
2. **Casting Pattern**: Clients who know the concrete type can cast to access decorator-specific features.
3. **Backward Compatible**: Clients using only the interface continue to work unchanged.
4. **Information Hiding**: Decorator-specific features are available but not forced on all clients.
5. **Metadata Access**: Perfect for accessing internal state like trace IDs, hit counts, or performance metrics.

## Real-World Example

```csharp
// Generic client—works with any IMessage implementation
public void ProcessMessage(IMessage msg)
{
    var rendered = msg.Render();
    // Does the heavy lifting
}

// Specific client that wants tracing info
public void DebugProcessMessage(IMessage msg)
{
    var rendered = msg.Render();
    
    // Optional: if we know it's traceable, use the extra feature
    if (msg is ITraceable traceable)
    {
        Console.WriteLine($"Trace: {traceable.TraceId}");
    }
}
```

## When to Use Semi-Transparent Decorator

- ✅ You want to preserve polymorphism for most clients
- ✅ The decorator adds useful debugging or instrumentation info
- ✅ Some clients need decorator-specific state or operations
- ✅ You're adding features like caching, tracing, or performance monitoring
- ✅ You want a "backdoor" to access decorator internals without breaking the interface
