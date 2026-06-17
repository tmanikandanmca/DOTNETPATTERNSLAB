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

Each pattern keeps its own solution, source variants, and dedicated NUnit tests under its `tests/` folder.

flowchart TD
    A[Gang of Four Patterns] --> B[Creational]
    A --> C[Structural]
    A --> D[Behavioral]

    %% Creational Patterns
    B --> B1[Abstract Factory\nProblem: Variants leak concrete classes\nMove: Factory interface\nUse: Platform-neutral families\nParticipants: Client, AbstractFactory, ConcreteFactory, Product]
    B --> B2[Builder\nProblem: Fragile constructors\nMove: Step-by-step build\nUse: Complex objects with options\nParticipants: Director, Builder, ConcreteBuilder, Product]
    B --> B3[Factory Method\nProblem: Base workflow shouldn’t know concrete class\nMove: Overridable factory method\nUse: Framework delegates choice\nParticipants: Creator, ConcreteCreator, Product]
    B --> B4[Prototype\nProblem: Expensive setup\nMove: Clone prototype\nUse: Runtime config, costly init\nParticipants: Client, Prototype, ConcretePrototype]
    B --> B5[Singleton\nProblem: Multiple instances corrupt state\nMove: One shared instance\nUse: Process-wide service\nParticipants: Singleton, Client]

    %% Structural Patterns
    C --> C1[Adapter\nProblem: API mismatch\nMove: Wrap and translate\nUse: Legacy/3rd-party integration\nParticipants: Client, Target, Adapter, Adaptee]
    C --> C2[Bridge\nProblem: Hierarchies multiply\nMove: Composition over subclassing\nUse: Product × Platform variation\nParticipants: Abstraction, Implementor]
    C --> C3[Composite\nProblem: Tree branching logic\nMove: Common component contract\nUse: Menus, UI trees\nParticipants: Component, Leaf, Composite]
    C --> C4[Decorator\nProblem: Class explosion\nMove: Wrap with decorators\nUse: Stackable features\nParticipants: Component, Decorator]
    C --> C5[Facade\nProblem: Clients know too much detail\nMove: Higher-level facade\nUse: Clean API over subsystems\nParticipants: Facade, Subsystems, Client]
    C --> C6[Flyweight\nProblem: Memory footprint\nMove: Share intrinsic state\nUse: Glyphs, particles, markers\nParticipants: Flyweight, Factory, Context]
    C --> C7[Proxy\nProblem: Direct access costly/insecure\nMove: Stand-in proxy\nUse: Lazy load, caching, security\nParticipants: Subject, Proxy, RealSubject]

    %% Behavioral Patterns
    D --> D1[Chain of Responsibility\nProblem: Sender knows too much\nMove: Link handlers\nUse: Validation, middleware\nParticipants: Handler, ConcreteHandler, Client]
    D --> D2[Command\nProblem: Coupled invoker/receiver\nMove: Request as object\nUse: Undo, jobs, macros\nParticipants: Command, Invoker, Receiver]
    D --> D3[Interpreter\nProblem: Need evaluator for small language\nMove: Expression tree\nUse: Simple rules, stable grammar\nParticipants: Expression, Context]
    D --> D4[Iterator\nProblem: Clients depend on representation\nMove: Iterator abstraction\nUse: Multiple traversal strategies\nParticipants: Iterator, Aggregate]
    D --> D5[Mediator\nProblem: Many-to-many communication\nMove: Central mediator\nUse: Dialogs, workflows\nParticipants: Mediator, Colleague]
    D --> D6[Memento\nProblem: Undo needs snapshots\nMove: Opaque memento\nUse: Editors, transactions\nParticipants: Originator, Memento, Caretaker]
    D --> D7[Observer\nProblem: State changes need updates\nMove: Publish-subscribe\nUse: UI binding, events\nParticipants: Subject, Observer]
    D --> D8[State\nProblem: Conditionals sprawl\nMove: State objects\nUse: Workflows, modes\nParticipants: Context, State]
    D --> D9[Strategy\nProblem: Algorithm conditionals\nMove: Inject strategy\nUse: Sorting, pricing, routing\nParticipants: Context, Strategy]
    D --> D10[Template Method\nProblem: Shared workflow order\nMove: Skeleton + hooks\nUse: Framework extension points\nParticipants: AbstractClass, ConcreteClass]
    D --> D11[Visitor\nProblem: Many operations clutter classes\nMove: Visitor per element\nUse: Compilers, ASTs, reports\nParticipants: Visitor, Element]

