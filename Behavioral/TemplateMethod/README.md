# Template Method

Sub-variants / Notes:

1. Pure Template Method
2. Hook Methods

This sample shows how a base workflow defines the algorithm skeleton while derived types override only the permitted variation points.

Checklist covered by tests:

- the base class should own the sequence
- subclasses should override only variation points
- hook methods should be optional and safe by default
- tests should confirm the order does not change accidentally
- derived classes should not duplicate the full algorithm
