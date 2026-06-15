# Strategy

Sub-variants / Notes:

1. Dynamic Strategy (runtime injection)
2. Static Strategy (compile-time)

This sample shows how interchangeable algorithms can be selected without changing context logic in a .NET Web API structure.

Variant guides:

- `src/01-DynamicStrategy/README.md`
- `src/02-StaticStrategy/README.md`

Checklist coverage in tests:

- All concrete strategies implement a shared strategy contract.
- The context delegates algorithm work to a strategy.
- Strategy behavior is swapped without rewriting context logic.
- New strategies are added without modifying existing context implementation.
- Tests inject multiple strategies to verify context behavior.
