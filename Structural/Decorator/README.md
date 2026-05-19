# Decorator

This solution demonstrates how to **attach additional responsibilities** to an object **dynamically**, without altering its structure. Decorators provide a flexible alternative to subclassing for extending functionality.

---

## 1. Intro

The **Decorator** pattern is a **structural design pattern** that lets you attach new behaviors to objects by placing these objects inside special wrapper objects that contain the behaviors.

## Definition

**Decorator** lets you compose objects with behaviors dynamically. It provides a way to extend an object's functionality at runtime by wrapping it in a decorator that has the same interface and delegates to the original object while adding extra behavior.

This is especially useful when you have a base component and want to add various optional responsibilities without creating an explosion of subclasses.

---

## 2. Core Components of the Design Pattern

| Pattern Role | Responsibility | Example from this solution |
| --- | --- | --- |
| **Component** | The original interface that the decorator must maintain | `ILogger`, `IDataProcessor` |
| **Concrete Component** | The original object being decorated | `ConsoleLogger`, `BasicDataProcessor` |
| **Decorator** | Maintains a reference to the component and defines the same interface | `LoggerDecorator`, `ProcessorDecorator` |
| **Concrete Decorator** | Adds specific behavior before or after forwarding calls | `TimestampLoggerDecorator`, `JsonFormatterDecorator` |
| **Client** | Uses the decorated component through its common interface | Demo methods in each `PatternDemo.cs` |

### How the current code implements this pattern

- The decorator wraps a component instance and maintains its interface.
- The decorator forwards calls to the wrapped component.
- The decorator adds behavior before or after forwarding.
- Multiple decorators can be chained together.
- The client cannot distinguish between a bare component and a decorated one (transparent decorator).

---

## 3. Sub Types of Decorator in this Solution

### 01 - Transparent Decorator

The decorator is completely transparent to the client. It maintains the exact interface of the component and adds behavior without revealing itself.

**Use when**: You want clients to be completely unaware they're using a decorator. Perfect for adding cross-cutting concerns like logging or caching.

### 02 - Semi-Transparent Decorator

The decorator exposes additional methods or properties beyond the component's interface. Clients can access decorator-specific functionality if they need it.

**Use when**: You want decorators to optionally expose additional behavior. Useful when the decorator adds meaningful operations clients might want to leverage.

### 03 - Dynamic vs Static Decoration

- **Dynamic**: Decorators are applied at runtime. Objects can be wrapped and unwrapped dynamically, even during execution.
- **Static**: Decorators are applied at compile-time through inheritance or generic composition.

**Use when**: Dynamic when you need flexibility and runtime configuration. Static when you want compile-time safety and performance.

---

## 4. Validation Checklist

Use this checklist to confirm your Decorator implementation is working correctly:

- ✅ **The wrapper still behaves like the component** — A decorated object responds to the same interface calls as the original.
- ✅ **The original object receives the call when expected** — When the decorator doesn't override the call, it forwards to the wrapped component.
- ✅ **Added behavior runs before or after forwarding in the correct order** — Decorators execute their logic at appropriate points.
- ✅ **Swapping one decorator chain for another does not require client changes** — Clients work with the interface, not concrete types.
