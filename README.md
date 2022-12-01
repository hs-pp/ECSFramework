# ECSFramework
flecs wrapper for C#

This is mostly a wrapper exposing functionality from flecs with some quality of life improvements.
Generated the flecs binding using the steps found here: https://github.com/flecs-hub/flecs-cs

Features include:
- World, Entity, Component, System,
- Tags are differentiated from Components.
- RuntimeTags are tags that can be defined at runtime.
- Three versions of Pairs: (Tag, Tag), (Tag, Component), (Tag, Entity)
- Queries, Filters, Observers
- A universal Iterator
- A new concept called the ManagedComponent which can attach a managed object as a Component to an Entity. (Useful for attaching MonoBehaviours)
- Clean way to enable RestAPI/Flecs Explorer
- Comprehensive unit tests
- A custom impl of System phases that works outside of Flecs. This enables the flexibility to define an ordering but also define a tickrate for each phase.

The code still uses a Unity namespace for some unsafe utilities.
Namely, the Unity.Collections.LowLevel.Unsafe has some cool pure functions for working with unsafe code.
It should be very easy to replace those so there are no more Unity dependencies but I don't plan on ever using this outside of a Unity context so whatever.
