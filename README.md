# DotNet Patterns Lab

A .NET 10 Web API learning workspace for design patterns.

## Workspace structure

Creational patterns:

1. Singleton
2. Factory Method
3. Abstract Factory
4. Builder
5. Prototype

Structural patterns:

1. Adapter
2. Decorator
3. Facade
4. Composite
5. Proxy
6. Bridge
7. Flyweight

Behavioral
 
1. Observer
2. Strategy
3. Command
4. Iterator
5. Mediator
6. State
7. Template Method
8. Chain of Responsibility
9. Visitor
10.Interpreter
11.Memento

Each pattern keeps its own solution, source variants, and dedicated NUnit tests under its `tests/` folder. 

# Gang of Four Design Patterns Cheat Sheet

This table summarizes each GoF pattern with its **Problem, Move, Use When, and Participants**.

| **Pattern Name**        | **Problem**                                                                 | **Move**                                                                 | **Use When**                                                                 | **Participants** |
|--------------------------|-----------------------------------------------------------------------------|---------------------------------------------------------------------------|-------------------------------------------------------------------------------|------------------|
| Abstract Factory         | UI, database, or platform variants leak concrete class choices everywhere. | Expose a factory interface per product family; swap the factory to swap the family. | You need consistent product families and want client code to stay platform-neutral. | Client, AbstractFactory, ConcreteFactory, AbstractProduct, Product |
| Builder                  | Constructors become long, fragile, or tied to one representation.          | A director or client calls ordered build steps; the builder assembles the result. | Object creation has many optional parts or multiple output representations. | Director, Builder, ConcreteBuilder, Product |
| Factory Method           | A base workflow knows it needs a product but should not know its concrete class. | Move creation behind an overridable factory method. | A framework or superclass must delegate object choice to extensions. | Creator, ConcreteCreator, Product, ConcreteProduct |
| Prototype                | Creating variants from scratch is expensive or repeats setup logic.        | Clone a prototype, then adjust the few fields that differ. | Runtime configuration or costly initialization makes cloning simpler than construction. | Client, Prototype, ConcretePrototype, Clone |
| Singleton                | Multiple instances would corrupt coordination, caches, or resource ownership. | Control construction and expose one managed instance. | A true process-wide service is needed; avoid it for ordinary dependencies. | Singleton, Client |
| Adapter                  | A useful class cannot be consumed because its API shape does not match.   | Wrap the adaptee and translate calls at the boundary. | Integrating legacy, third-party, or incompatible modules. | Client, Target, Adapter, Adaptee |
| Bridge                   | Class hierarchies multiply across two changing dimensions.                 | Put implementation behind a composed interface instead of subclassing every combination. | You have product × platform, shape × renderer, or API × transport variation. | Abstraction, RefinedAbstraction, Implementor, ConcreteImplementor |
| Composite                | Tree structures force client code to branch between leaf and container logic. | Let leaves and composites implement a common component contract. | Working with menus, scene graphs, folders, UI trees, or nested rules. | Component, Leaf, Composite, Client |
| Decorator                | Subclassing every feature combination creates a class explosion.           | Wrap the component with decorators that implement the same interface. | Features should be stackable, optional, and transparent to clients. | Component, ConcreteComponent, Decorator, ConcreteDecorator |
| Facade                   | Clients coordinate too many subsystem classes and know too much detail.   | Place common workflows behind a higher-level facade. | You need a clean API over build, media, payment, compiler, or service internals. | Facade, Subsystems, Client |
| Flyweight                | Thousands of similar objects repeat identical intrinsic state.             | Share immutable intrinsic state; pass extrinsic state from the context. | Rendering glyphs, particles, map markers, or repeated domain objects at scale. | Flyweight, ConcreteFlyweight, FlyweightFactory, Context |
| Proxy                    | Direct access needs lazy loading, security, caching, remoting, or logging. | Route calls through a proxy that adds the control behavior. | The real subject is expensive, remote, protected, or needs instrumentation. | Subject, Proxy, RealSubject, Client |
| Chain of Responsibility  | Senders know too much about which receiver should process a request.       | Link handlers and let each handle or forward. | Validation, middleware, event handling, or support escalation has ordered fallbacks. | Handler, ConcreteHandler, Client |
| Command                  | Invokers are tightly coupled to receiver actions.                         | Represent the action with execute, and optionally undo, metadata, or history. | Building toolbars, jobs, transactions, macros, or undo stacks. | Command, ConcreteCommand, Invoker, Receiver, Client |
| Interpreter              | A small domain language needs a structured evaluator.                     | Build an expression tree whose nodes interpret themselves against context. | Rules are simple, grammar is stable, and a full parser framework is unnecessary. | AbstractExpression, TerminalExpression, NonterminalExpression, Context |
| Iterator                 | Clients depend on internal arrays, trees, or storage details.              | Expose an iterator with traversal state and next/current operations. | Collections need multiple traversal strategies or representation hiding. | Iterator, ConcreteIterator, Aggregate, ConcreteAggregate |
| Mediator                 | Many-to-many communication tangles components together.                   | Peers notify a mediator; the mediator coordinates responses. | Dialogs, workflow components, or modules need coordinated behavior. | Mediator, ConcreteMediator, Colleague |
| Memento                  | Undo or rollback needs snapshots but should not break encapsulation.       | Originator creates opaque mementos; caretaker stores them. | Editors, transactions, games, or workflows need checkpoints. | Originator, Memento, Caretaker |
| Observer                 | State changes must update multiple subscribers without tight coupling.    | Subjects publish events to observers registered at runtime. | UI binding, event streams, cache invalidation, or domain notifications. | Subject, ConcreteSubject, Observer, ConcreteObserver |
| State                    | Conditionals for mode-specific behavior sprawl across a class.             | Move state-specific behavior into state objects and delegate to the current state. | Workflows, documents, connections, or UI controls have explicit modes. | Context, State, ConcreteState |
| Strategy                 | Conditionals choose between algorithms inside one class.                   | Inject a strategy object and let the context delegate the algorithm. | Sorting, pricing, routing, compression, validation, or scoring varies independently. | Context, Strategy, ConcreteStrategy |
| Template Method          | Workflows share ordering but differ in specific operations.                | Keep the template method fixed; override hooks or primitive operations. | Framework workflows need controlled extension points. | AbstractClass, ConcreteClass |
| Visitor                  | Many operations over a stable object structure clutter the element classes. | Elements accept a visitor; visitors implement operations per element type. | Compilers, ASTs, document models, or reports need new operations frequently. | Visitor, ConcreteVisitor, Element, ConcreteElement, ObjectStructure |

 

