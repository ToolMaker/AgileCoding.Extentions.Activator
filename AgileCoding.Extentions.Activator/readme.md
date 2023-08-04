AgileCoding.Extensions.Activator
================================

Overview
--------

The AgileCoding.Extensions.Activator is a .NET library that provides extension methods for the `System.Type` class and the `System.Exception` class. These extension methods are mainly designed to assist with creating instances of types using the `Activator.CreateInstance` method and handling exceptions thrown by the .NET Reflection API.

Installation
------------

This library is available as a NuGet package. You can install it using the NuGet package manager console:

bashCopy code

`Install-Package AgileCoding.Extensions.Activator -Version 2.0.5`

Features
--------

The library provides two main features:

1.  Creating instances of types with detailed error logging: The `CreateInstanceWithLogging` extension method can be used to create an instance of a type. If there are any issues during the creation of the instance, the method logs detailed error information.

2.  Creating instances of types without detailed error logging: The `CreateInstanceWithoutLogging` extension method can be used to create an instance of a type without logging detailed error information.

3.  Handling Reflection API Exceptions: The `ToStringAll` extension method can be used to convert an exception to a string representation. This is particularly useful for handling exceptions thrown by the .NET Reflection API.

Usage
-----

Here's a brief overview of how you can use these features in your code:

```csharp

using System;
using AgileCoding.Extentions.Activators;
using AgileCoding.Library.Interfaces.Logging;

// Suppose 'logger' is an instance of a class implementing the ILogger interface.

Type myType = typeof(MyClass);
var instance = myType.CreateInstanceWithLogging<IMyInterface>(logger, constructorParameter1, constructorParameter2);

// ...

try
{
    // Some code that can throw exceptions.
}
catch (Exception ex)
{
    string errorDetails = ex.ToStringAll();
    logger.WriteError(errorDetails);
}
```

Documentation
-------------

For more detailed information about the usage of this library, please refer to the [official documentation](https://github.com/ToolMaker/AgileCoding.Extentions.Activator/wiki).

License
-------

This project is licensed under the terms of the MIT license. For more information, see the [LICENSE](https://github.com/ToolMaker/AgileCoding.Extentions.Activator/blob/main/LICENSE) file.

Contributing
------------

Contributions are welcome! Please see our [contributing guidelines](https://github.com/ToolMaker/AgileCoding.Extentions.Activator/blob/main/CONTRIBUTING.md) for more details.