# 03 - Dynamic vs Static Decoration

## What this variant demonstrates

This variant shows two different **timing strategies** for applying decorators:

- **Static Decoration**: Decorators are composed at compile-time or object creation time. The structure is fixed and known ahead of time.
- **Dynamic Decoration**: Decorators are applied at runtime, often driven by configuration or runtime decisions. The structure can change based on conditions.

Both approaches use the same decorator classes but apply them at different points in the execution flow.

### Code focus

```csharp
// STATIC: Composition is decided at creation, structure is fixed
var staticDecorated = new StarDecorator(new UpperDecorator(new BaseContent("compile time")));
Console.WriteLine(staticDecorated.Value());  // *COMPILE TIME*

// DYNAMIC: Composition is decided at runtime based on logic
IContent dynamicDecorated = new BaseContent("runtime");
var decorators = new Func<IContent, IContent>[]
{
    c => new UpperDecorator(c),
    c => new StarDecorator(c)
};

foreach (var decorate in decorators)
{
    dynamicDecorated = decorate(dynamicDecorated);
}
Console.WriteLine(dynamicDecorated.Value());  // *RUNTIME*
```

In this variant:
- **Static** uses fixed type composition at known time.
- **Dynamic** uses lambdas or factory patterns to vary decorator chains at runtime.
- Both result in the same interface and behavior pattern.
- Dynamic allows configuration-driven composition or condition-based decoration.
- Static provides compile-time safety and predictable performance.

## How it differs from other Decorator variants

- Compared to `01-TransparentDecorator`: Both use transparent interfaces, but this variant focuses on *when* decoration occurs, not *how*.
- Compared to `02-SemiTransparentDecorator`: This variant is about temporal composition strategies, while Semi-Transparent is about decorator visibility.

## UML

```mermaid
classDiagram
    class IContent {
        <<interface>>
        +Value() string
    }

    class BaseContent {
        -value: string
        +BaseContent(value: string)
        +Value() string
    }

    class UpperDecorator {
        -inner: IContent
        +UpperDecorator(inner: IContent)
        +Value() string
    }

    class StarDecorator {
        -inner: IContent
        +StarDecorator(inner: IContent)
        +Value() string
    }

    class Client {
        +StaticDecoration()
        +DynamicDecoration()
    }

    IContent <|.. BaseContent
    IContent <|.. UpperDecorator
    IContent <|.. StarDecorator
    
    UpperDecorator --> IContent : wraps
    StarDecorator --> IContent : wraps
    
    Client --> IContent : creates
    Client ..>|new UpperDecorator(...)| UpperDecorator : static
    Client ..>|loop in lambda| StarDecorator : dynamic
```

## Comparison Table

| Aspect | Static | Dynamic |
|--------|--------|---------|
| **Timing** | Compile-time or initialization | Runtime |
| **Flexibility** | Fixed structure | Runtime-configurable |
| **Performance** | Predictable, optimized | Slight overhead from configuration |
| **Type Safety** | Full compile-time checks | May use reflection or lambdas |
| **Testing** | Test fixed structures | Test configuration variations |
| **Readability** | Explicit nesting | Loop or conditional logic |
| **Use Case** | Standard configurations | Variable requirements |

## Key Implementation Points

1. **Static Approach**: Direct type composition `new StarDecorator(new UpperDecorator(new BaseContent(...)))`.
2. **Dynamic Approach**: Factory functions or lambdas `Func<IContent, IContent>[]` applied in loops.
3. **Same Interface**: Both produce objects implementing the same `IContent` interface.
4. **Interchangeable**: The client code using the result doesn't know which approach was used.
5. **Chainable**: Both support decorator chaining through composition.

## Real-World Example

```csharp
// STATIC: You know you always need compression, then encryption
var pipeline = new EncryptionDecorator(new CompressionDecorator(new FileContent("data.txt")));

// DYNAMIC: Based on configuration, apply different decorators
IContent content = new FileContent("data.txt");

var decoratorConfig = config.GetSection("Decorators");
if (decoratorConfig.GetValue<bool>("UseCompression"))
{
    content = new CompressionDecorator(content);
}
if (decoratorConfig.GetValue<bool>("UseEncryption"))
{
    content = new EncryptionDecorator(content);
}
if (decoratorConfig.GetValue<bool>("UseValidation"))
{
    content = new ValidationDecorator(content);
}

return content;
```

## When to Use Each

### Use Static When:
- ✅ The decorator chain is known and fixed
- ✅ You want maximum performance
- ✅ You want compile-time verification
- ✅ The combination is rarely changing

### Use Dynamic When:
- ✅ The decorator chain varies by configuration
- ✅ The composition is feature-flag controlled
- ✅ Different environments need different decorators
- ✅ You're building a plugin or extension system
- ✅ Runtime performance is less critical than flexibility

## Validation Checklist

For both static and dynamic approaches, verify:
- ✅ Both produce the same functional result
- ✅ The decorator chain is traversed in the correct order
- ✅ All decorators maintain the interface contract
- ✅ Multiple chains can coexist without interference
- ✅ Adding a new decorator doesn't require client changes
