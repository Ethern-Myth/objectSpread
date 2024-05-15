# ObjectSpread

`ObjectSpread` is a C# utility library that provides an efficient way to merge properties from multiple objects into a new object. It uses reflection and caching to enhance performance, making it a useful tool for scenarios where object merging is required.

![NuGet Version](https://img.shields.io/nuget/v/objectSpread)

![NuGet Downloads](https://img.shields.io/nuget/dt/objectSpread)

## Features

- **Merge Multiple Objects:** Combine properties from multiple source objects into a new instance of a target type.
- **Reflection and Caching:** Utilizes reflection to dynamically access properties and caching for improved performance.
- **Type Safety:** Ensures that properties are only merged if they are compatible and can be written to the target object.

## Installation

To include `ObjectSpread` in your project, you can add the source files directly or, if available, install via a package manager like NuGet.

### NuGet

To install via NuGet, run the following command in the NuGet Package Manager Console:

```bash
Install-Package ObjectSpread
```

Or via .NET CLI:

```bash
dotnet add package efetch
```

### Direct Download

Alternatively, you can download the source code and include it in your project manually.

## Usage

Here's a basic example demonstrating how to use the `Spread` method from the `ObjectExtensions` class to merge properties from multiple objects:

```csharp
using objectSpread;

class Program
{
    static void Main(string[] args)
    {
        var obj1 = new { Name = "Alice", Age = 25 };
        var obj2 = new { Age = 30, Country = "Wonderland" };
        
        var merged = ObjectExtensions.Spread<Person>(obj1, obj2);

        Console.WriteLine($"Name: {merged.Name}, Age: {merged.Age}, Country: {merged.Country}");
    }
}

public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string Country { get; set; }
}
```

### Output

```
Name: Alice, Age: 30, Country: Wonderland
```

In this example, properties from `obj1` and `obj2` are merged into a new `Person` object. The `Age` property from `obj2` overwrites the `Age` property from `obj1`.

## API Reference

### ObjectExtensions

#### Spread<T>(params object[] sources) where T : class, new()

Merges the properties of multiple source objects into a new instance of the target type `T`.

- **T**: The type of the target object.
- **sources**: An array of source objects whose properties will be merged.

**Returns**: A new instance of type `T` with merged properties.

### Author

**objectSpread** is developed and maintained by [Ethern Myth](https://github.com/Ethern-Myth).


## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

---

For more details and updates, please visit the [GitHub repository](https://github.com/Ethern-Myth/objectSpread).