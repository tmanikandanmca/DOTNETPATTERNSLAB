# State

Sub-variants / Notes:

1. Classic State Objects
2. State Machine (enum + switch)

This sample shows two ways to model changing behavior as state changes.

Variant guides:

- `src/01-ClassicStateObjects/README.md`
- `src/02-StateMachineEnumSwitch/README.md`

Checklist coverage in tests:

- Each state owns only state-specific behavior.
- Transitions are explicit and covered by tests.
- The context does not contain large conditional branches.
- Invalid transitions are prevented or handled clearly.
- State classes are swappable without changing callers.
