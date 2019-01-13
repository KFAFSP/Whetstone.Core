# Whetstone.Core.Tests
**NUnit 3** test project for all **Whetstone.Core** libraries.

## Project structure
Each library gets its own folder for its root namespace, as library namespaces may not overlap. Common helpers are stored within the root folder. The base namespace of the test project is **Whetstone.Core**, therefore contained sources will be within the same namespace. In addition to this, all modules list this assembly as `InternalsVisibleTo` to allow for whitebox testing. All test classes must be suffixed with `Tests` to resolve the resulting name conflicts.

Path                        | Contents
---                         | ---
`Contracts/`                | Tests for the **Contracts** module.
`Tasks/`                    | Tests for the **Tasks** module.
`TaskAssert`                | Helper for performing assertions on `Task`s.
`TestCaseDataExtensions`    | Extensions for **NUnits**'s `TestCaseData` class.
`README.md`                 | This readme file.

## Big issues

### Asynchronicity and assertions
Performing asserts on asynchronous `Task` operations is difficult, although for different reasons to `Thread`. In any case, the testing code is unable to enforce the activation/scheduling of a `Task`. Asserting a state transition in a `Task` usually requires execution, which cannot be forced because of the scheduling problem.

Fortunately, most problems can be avoided reliably. In most cases, the library will prefer to return `Task`s in final states, if that can be inferred synchronously. In all other cases, tests need to assert transitions on a best-effort basis, and invalidate their results should the `Task` not have been scheduled. In these cases, a small timeout is used, and only a single test shall be ran at a time, making scheduling much more likely.

Sometimes assertions for 'blocking' or 'does not complete' are necessary, in which case the test author is responsible for using a sufficiently large timeout that also does not impact the overall test time too much.

### `Debug.Assert`
The **Whetstone.Core** project relies on using debug-only assertions in all places where invariants need to be upheld that are theoretically inviolable or not testable for in other ways. Assertions in **NUnit** tests result in strange behavior and/or abortion of the tests.

As they should never be encountered anyway, and are not to be tested by design, this is not considered a problem. If it arises, however, the assertions that lead to the crash can only be traced by running the tests with an attached debugger.

### GC-aware tests
For some instances, the behavior of finalizers and other GC related processes needs to be tested for certain classes. In these cases, **NUnit** must be forced to do these processes manually.

It should also be noted, that any test that might have side-effects on garbage collection (i.e. assertions in finalizers) can cause erratic behavior. In these cases, the enclosing test class must define a `TearDown` that forces the GC processes, otherwise not even the assertion debugging method mentioned above can be used. By doing do, **NUnit** will not yield to the framework, which is excluded from debugging, before the panic happens, allowing the user to witness it.