# Iterator

Sub-variants / Notes:

1. External Iterator
2. Internal Iterator
3. Fail-fast Iterator

This sample should show how collections are traversed while keeping iteration concerns separated from the data structure.

Checklist covered by tests:

- iterator should work without leaking collection internals
- traversal order should be deterministic and test-covered
- external iterator should preserve state across calls
- fail-fast behavior should be validated under mutation scenarios
- new iterator styles should be pluggable without caller rewrites
