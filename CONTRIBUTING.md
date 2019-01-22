# Contribution Guidelines
Thank you for showing interest in this project. This document lists some of the guidelines I set out when I started or accumulated over the development process. I recommend that you read them if you are confused about the design philosophy of the project.

 - [Issues](#issues)
 - [Pull requests](#pull-requests)
 - [Code Style](#code-style)
    - [General](#general)
    - [XML documentation](#xml-documentation)
    - [Contract validation](#contract-validation)
    - [Naming conventions](#naming-conventions)

## Issues

> **TODO** |
> --- |
> 
> TBD ;)

## Pull requests
If you would like to make contributions to the code, consider this section carefully.

All contributions should follow the [Code style](#code-style) that is laid out in this document. In addition, all contributions should provide appropriate tests in the shared test project. If you add functionality or types that are part of the broader interface, other documentation besides XML comments might be in order. Be sure to coordinate wiki contributions with me as well.

> **TODO** |
> --- |
> 
> TBD ;)

## Code Style
I strive to keep a consistent style of code in the codebase of the **Whetstone.Core** project. While some arbitrary choices have to be made where debating would be fruitless, others are open for change.

### General
Here are some general rules that apply everywhere.

* Annotate as much as possible.

   Use the appropriate annotations from the `System` assemblies and those provided by the `JetBrains.Annotations` package to annotate all code elements. Express invariants using them, and ensure correct and sensible behavior when debugging or analyzing the project.

* Prefer expression bodies.

   Whenever possible, and where their length does not exceed one line unless they are a single binary/ternary operator or call, expression bodies shall be used. This includes properties, property accessors and methods.

* Prefer `readonly` fields and value types.

   Wherever possible, ensure that fields and value types are immutable.

* Prefer readonly references (`in`) for parameters that might assume a value type (includes generics).

* Use verbatim strings for untranslated messages.

   To indicate that strings are messages that serve an indicative or development purpose, like exception and assertions messages, shall be provided as verbatim strings.

* Prefer string interpolation.

   Where format expressions are used, use interpolated string expressions (`$"..."`) whenever possible. If the previous rule cannot be fulfilled this way, use a string constant instead.

* Prefer extension methods.

   If methods can be provided on top of existing interfaces or types, and do not conflict with existing names or overloads, use extension methods instead. Avoid them however if a certain method lacks complexity but has a high call frequency.

### XML documentation
In order of significance, the following rules for what must be documented apply.

1. All `public`, `protected`, `private protected` and `internal` types and members require documentation. Only `private` elements are exempt, unless required by another rule.

2. All methods require complete exception documentation. This includes `private` methods, which therefore need to be documented if they can throw exceptions.

3. Assertions that validate contracts that are not obvious from the signature and annotations (like `[NotNull]`) need to be documented in the `<remarks>` section. They may only be present in internals, and they can also warrant `private` methods to be documented.

### Contract validation
In general, every method should perform contract validation. This includes the parameters, but also the state of the objects it operates on, unless it is unspecific in these aspects.

There are two recognized ways to check contracts:

1. `Require.`\*, `ThrowIf`\*

   The static methods provided by `Require` perform contract validation based on exceptions, while the `ThrowIf`\* instance methods perform state validation also based on exceptions.

   Use this kind of validation on all public interfaces, for all values and states that can potentially be manipulated by outside (different assembly) callers.

2. `Ensure.`\*, `Assert`\*

   The static methods provided by `Ensure` mirror those in `Require`, as the `Assert`\* instance methods do the `ThrowIf`\* methods. They perform assertion-based validation that is always `[Conditional]`.

   Use this kind of validation for everything else, and only if there is no way to trigger them from outside code.

### Naming conventions
The names of all code elements shall adhere to the following rules.

Code element                    | Rule
---                             | ---
Namespace                       | `UpperCamelCase`
Class, Struct                   | `UpperCamelCase`
Interface                       | `IUpperCamelCase`
 | |
Method                          | `UpperCamelCase`
Property                        | `UpperCamelCase`
Event                           | `UpperCamelCase`
Field (`private`)               | `FUpperCamelCase`
Field (`static`, `private`)     | `_FUpperCamelCase`
Constant                        | `C_UpperCamelCase`
Enum member                     | `UpperCamelCase`
 | |
Parameter                       | `AUpperCamelCase`, `BUpperCamelCase`
Type parameter                  | `TUpperCamelCase`
Lambda parameter                | `X`, `Y`, Parameter
 | |
Local variable                  | `lowerCamelCase`
Local constant                  | `C_UpperCamelCase`
Local function                  | `UpperCamelCase`
