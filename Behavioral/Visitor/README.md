# Visitor

Sub-variants / Notes:

1. Classic Visitor
2. Acyclic Visitor (avoids dependency cycles)

This sample should show how new operations are added without changing the visited element classes.

Checklist covered by tests:

- operations should be addable without changing every element class
- each element should expose a clear accept method
- visitors should stay focused on one operation
- tests should verify traversal and dispatch behavior
- the hierarchy should be stable enough to justify the pattern
