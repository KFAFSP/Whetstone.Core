# Whetstone.Core
A .NET Standard library aiming to provide reliable extensions and alternatives to built-ins.

## Libraries
The **Whetstone.Core** project is divided into smaller libraries that each serve a specific purpose. Most of them depend on the **Contracts** library, which could also be used stand-alone.

### **Whetstone.Core.Contracts**
The most basic library is the **Contracts** module, providing a set of types used in method and other interface contracts as well as contract validation. Whilst providing little functionality, it serves to simplify the instanciation of standard patterns, such as argument validation and result packing.

### **Whetstone.Core.Tasks**
The **Tasks** module is designed to augment the **TPL** by providing implementations of universally used primitives for synchronization and coordination. It allows for first-class use of asynchronous operations in place of traditional synchronization primitives, allowing for cancelation and more.

## Project structure
Each library has its own root. Most libraries are designed around each other, and should always be on the same version. All tests are present in a single testing project included in this repository.

Path                        | Contents
---                         | ---
`T4/`                       | Includes for **T4** templates.
`Whetstone.Core.Contracts/` | The **Contracts** module project.
`Whetstone.Core.Tasks/`     | The **Tasks** module project.
`Whetstone.Core.Tests/`     | The **NUnit 3** test project.
`LICENSE`                   | The **MIT** license document.
`README.md`                 | This readme file.

## Third-party dependencies

* **ReSharper** Annotations package (nuget)
* **NUnit 3** Test framework (nuget) (tests only)