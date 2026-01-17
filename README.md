# Jacobus.NaturalSort
[![NuGet](https://img.shields.io/nuget/v/Jacobus.NaturalSort)](https://www.nuget.org/packages/Jacobus.NaturalSort/)

Provides extremely performant natural/alphanumeric comparison of strings in C#.
It sorts strings in **natural order**, so `file2` comes before `file10`.

## Installation
Install via NuGet:
```bash
dotnet add package Jacobus.NaturalSort
```
Or via the Package Manager Console:
```powershell
Install-Package Jacobus.NaturalSort
```

## Usage
Because `NaturalStringComparer` is an `IComparer<string>` implementation, you can use the normal C# sorting and ordering methods. 

```csharp
using Jacobus.NaturalSort;

// You can sort collections of strings.
string[] files = [ "file10", "file2" ];
files.Sort(new NaturalStringComparer());
var ordered1 = files.OrderBy(f => f, new NaturalStringComparer());
var ordered2 = files.OrderByDescending(f => f, new NaturalStringComparer());
var ordered3 = files.Order(new NaturalStringComparer());
var ordered4 = files.OrderDescending(new NaturalStringComparer());

// You can sort collections of instances.
record Person(string Name);
List<Person> persons = [ new Person("John"), new Person("Jack") ];
persons.Sort((a, b) => _comparer.Compare(a.Name, b.Name));
var ordered5 = persons.OrderBy(p => p.Name, new NaturalStringComparer());
var ordered6 = persons.OrderByDescending(p => p.Name, new NaturalStringComparer());
```

> [!IMPORTANT]
> Does not support decimal fractions, only full numbers are compared.<br/>
> Does not support negative numbers, so '-' and '+' will be compared just like any other non-digit character.<br/>
> Does not support comparing scientific number representations like 1e3.


## Benchmarks
Soon™
