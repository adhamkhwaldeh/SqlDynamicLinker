# AlJawad.SqlDynamicLinker

**AlJawad.SqlDynamicLinker** is a .NET library that empowers developers to build dynamic, client-driven queries for Entity Framework Core. It allows frontend applications to specify filtering, sorting, and related-entity inclusion (eager loading) through a flexible JSON structure, which is then translated into efficient IQueryable expressions.

This library is perfect for building APIs that need to support complex data grids, search pages, or any scenario where the client needs to control the data shaping.

## Features

- **Dynamic Filtering**: Apply complex filters with various operators.
- **Dynamic Sorting**: Sort by multiple fields and in ascending or descending order.
- **Dynamic Eager Loading**: Include related entities to avoid N+1 query problems.
- **Spatial Queries**: Filter data based on geographic location (latitude, longitude, and radius).
- **Easy to Use**: A simple `.Filter()` extension method is all you need to apply the dynamic query.
- **JSON-Based**: Uses a clear and intuitive JSON structure for defining queries.

## Installation

You can install the package via NuGet Package Manager or the .NET CLI.

### .NET CLI

```bash
dotnet add package AlJawad.SqlDynamicLinker
```

### Package Manager Console

```powershell
Install-Package AlJawad.SqlDynamicLinker
```

## Getting Started

### 1. Configure Services in `Program.cs`

To use **AlJawad.SqlDynamicLinker**, you need to configure services in your `Program.cs` (or `Startup.cs`).

#### For Deserializing from Request Body (`[FromBody]`)

If you are passing the filter object in the request body (e.g., in a POST request), you need to add the custom `JsonConverter`:

```csharp
using AlJawad.SqlDynamicLinker.Converters;
using AlJawad.SqlDynamicLinker.ModelBinder;

builder.Services
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.Converters.Add(new EntityBaseFilterConverter());
    });
```

#### For Model Binding from Query String (`[FromQuery]`)

If you are passing the filter parameters in the query string (e.g., in a GET request), you need to add the custom `ModelBinderProvider`s:

```csharp
using AlJawad.SqlDynamicLinker.ModelBinder;

builder.Services.AddControllers(options =>
{
    options.ModelBinderProviders.Add(new BaseQueryableFilterBinderProvider());
    options.ModelBinderProviders.Add(new BasePagingFilterBinderProvider());
});
```

You may need to add both configurations if you plan to support both scenarios.

### 2. Controller Endpoint

In your API controller, you can now accept a `BaseQueryableFilter` object in your action methods, either from the request body (`[FromBody]`) or from the query string (`[FromQuery]`), depending on your configuration.

Here's an example of a controller that uses the library to filter a list of products:

```csharp
using AlJawad.SqlDynamicLinker.DynamicFilter;
using AlJawad.SqlDynamicLinker.Extensions;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IQueryable<Product> _products; // Your IQueryable source

    public ProductController(YourDbContext dbContext)
    {
        _products = dbContext.Products;
    }

    [HttpPost("search")]
    public IActionResult SearchProducts([FromBody] BaseQueryableFilter filter)
    {
        var filteredProducts = _products.Filter(filter);
        return Ok(filteredProducts.ToList());
    }
}
```

## The `BaseQueryableFilter` Object

The `BaseQueryableFilter` is the main object that you'll use to define your queries. It has the following properties:

- `Filters` (array): An array of filter objects.
- `DynamicSorting` (array): An array of sorting objects.
- `IncludeProperties` (array): An array of objects to specify which related entities to include.

Here is a full example of a `BaseQueryableFilter` object:

```json
{
  "IncludeProperties": [
    { "PropertyName": "MainCategory" },
    { "PropertyName": "ProductCategories.Category" }
  ],
  "Filters": [
    {
      "logic": "and",
      "dataName": "Name",
      "operator": "Contains",
      "value": "phone"
    },
    {
      "logic": "and",
      "dataName": "Price",
      "operator": "gte",
      "value": 100
    }
  ],
  "DynamicSorting": [
    {
      "sortIndex": 0,
      "dataName": "Price",
      "IsAscending": false
    }
  ]
}
```

## Filtering

The `Filters` array contains one or more filter objects. Each object defines a condition to apply to the query.

### Simple Filters

A simple filter object has the following properties:

- `logic` (string): The logical operator to use when combining this filter with the previous one. Can be `"and"` or `"or"`. The first filter's logic is ignored.
- `dataName` (string): The name of the property to filter on. You can use dot notation for nested properties (e.g., `"MainCategory.Name"`).
- `operator` (string): The comparison operator. See the list of supported operators below.
- `value` (any): The value to compare against.

**Example: Find all products with the name "Laptop"**

```json
{
  "Filters": [
    {
      "dataName": "Name",
      "operator": "eq",
      "value": "Laptop"
    }
  ]
}
```

### Supported Filter Operators

| Operator | Description              |
|----------|--------------------------|
| `eq`     | Equal                    |
| `neq`    | Not Equal                |
| `gt`     | Greater Than             |
| `gte`    | Greater Than or Equal    |
| `lt`     | Less Than                |
| `lte`    | Less Than or Equal       |
| `StartsWith` | The string starts with the value |
| `EndsWith`   | The string ends with the value   |
| `Contains`   | The string contains the value    |

### Spatial (Geometry) Filters

The library supports filtering based on geographic location. A geometry filter object has the following properties:

- `DataName` (string): The name of the property that stores the geometry data (must be a `Point` type in the database).
- `Operator` (string): Must be `"within"`.
- `Latitude` (number): The latitude of the center of the circle.
- `Longitude` (number): The longitude of the center of the circle.
- `Radius` (number): The radius of the circle in meters.

**Example: Find all stores within a 1500-meter radius of a point**

```json
{
  "Filters": [
    {
      "DataName": "Location",
      "Operator": "within",
      "Latitude": 34.0522,
      "Longitude": -118.2437,
      "Radius": 1500
    }
  ]
}
```

## Sorting

The `DynamicSorting` array allows you to sort the results by one or more fields. Each sorting object has the following properties:

- `sortIndex` (number): The order in which to apply the sorting.
- `dataName` (string): The name of the property to sort by.
- `IsAscending` (boolean): `true` for ascending order, `false` for descending.

**Example: Sort products by price (descending) and then by name (ascending)**

```json
{
  "DynamicSorting": [
    {
      "sortIndex": 0,
      "dataName": "Price",
      "IsAscending": false
    },
    {
      "sortIndex": 1,
      "dataName": "Name",
      "IsAscending": true
    }
  ]
}
```

## Including Related Entities

The `IncludeProperties` array allows you to specify which related entities to eager load. This is equivalent to using `.Include()` in Entity Framework. Each object in the array has one property:

- `PropertyName` (string): The name of the navigation property to include. You can use dot notation for nested includes (e.g., `"Order.OrderItems"`).

**Example: Include the main category and the product's categories**

```json
{
  "IncludeProperties": [
    { "PropertyName": "MainCategory" },
    { "PropertyName": "ProductCategories.Category" }
  ]
}
```

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
