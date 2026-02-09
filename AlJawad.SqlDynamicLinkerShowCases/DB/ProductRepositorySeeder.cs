namespace AlJawad.SqlDynamicLinkerShowCases.DB;

using AlJawad.SqlDynamicLinkerShowCases.Entities;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Geometries;

public static class ProductRepositorySeeder
{
    public static void Seed(AppDbContext db)
    {
        db.Categories.ExecuteDelete();

        db.SaveChanges();
        if (db.Categories.Any() || db.Products.Any() || db.ProductCategories.Any())
            return; // Prevent duplicate seeding

        var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

        // Points for MultiPoint
        Point p1 = new Point(11.567104, 48.1460224) { SRID = 4326 };
        Point p2 = new Point(11.567104, 48.1460224) { SRID = 4326 };
        Point p3 = new Point(11.567104, 48.1460224) { SRID = 4326 };
        var points = new List<Point> { p1, p2, p3 };

        // Categories
        var categories = new List<Category>
        {
            new Category { 
              //  Id = 1,
                Name = "Electronics", Description = "Devices and gadgets", Locations = new MultiPoint(points.ToArray()) { SRID = 4326 }
            },
            new Category {
              //  Id = 2,
                Name = "Books", Description = "Printed and digital books", Locations = new MultiPoint(points.ToArray()) { SRID = 4326 }
            },
            new Category { 
              //  Id = 3,
                Name = "Clothing", Description = "Apparel and fashion items", Locations = new MultiPoint(points.ToArray()) { SRID = 4326 }
            },
            new Category { 
             //   Id = 4, 
                Name = "Home & Kitchen", Description = "Household essentials", Locations = new MultiPoint(points.ToArray()) { SRID = 4326 }
            },
            new Category { 
             //   Id = 5, 
                Name = "Sports", Description = "Sporting goods and equipment", Locations = new MultiPoint(points.ToArray()) { SRID = 4326 }
            },
            new Category { 
               // Id = 6, 
                Name = "Toys", Description = "Children’s toys and games", Locations = new MultiPoint(points.ToArray()) { SRID = 4326 }
            },
            new Category { 
              //  Id = 7, 
                Name = "Beauty", Description = "Cosmetics and personal care", Locations = new MultiPoint(points.ToArray()) { SRID = 4326 }
            },
            new Category { 
                //Id = 8, 
                Name = "Automotive", Description = "Car accessories and tools", Locations = new MultiPoint(points.ToArray()) { SRID = 4326 }
            },
            new Category { 
              //  Id = 9, 
                Name = "Music", Description = "Instruments and audio gear", Locations = new MultiPoint(points.ToArray()) { SRID = 4326 }
            },
            new Category { 
            //    Id = 10, 
                Name = "Office Supplies", Description = "Work and study essentials", Locations = new MultiPoint(points.ToArray()) { SRID = 4326 }
            }
        };
        db.Categories.AddRange(categories);

        // Products
        var products = new List<Product>
        {
            new Product { Id = 1, Name = "Smartphone X", Description = "Latest smartphone with 128GB storage", Price = 699.99m, StockQuantity = 50, SKU = "ELEC-SMART-001", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, MainCategoryId = 1, Location = geometryFactory.CreatePoint(new Coordinate(-118.2437, 34.0522)) },
            new Product { Id = 2, Name = "Laptop Pro 15\"", Description = "High-performance laptop", Price = 1299.99m, StockQuantity = 20, SKU = "ELEC-LAPTOP-002", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, MainCategoryId = 1, Location = geometryFactory.CreatePoint(new Coordinate(40.7128, 74.0060)) },
            new Product { Id = 3, Name = "Wireless Headphones", Description = "Noise-cancelling headphones", Price = 199.99m, StockQuantity = 75, SKU = "ELEC-AUDIO-003", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, MainCategoryId = 2, Location = geometryFactory.CreatePoint(new Coordinate(48.8566, 2.3522)) },
            new Product { Id = 4, Name = "Novel - The Great Story", Description = "Bestselling fiction novel", Price = 14.99m, StockQuantity = 200, SKU = "BOOK-NOVEL-004", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, MainCategoryId = 2, Location = geometryFactory.CreatePoint(new Coordinate(51.5074, 0.1278)) },
            new Product { Id = 5, Name = "Programming in C#", Description = "Technical programming book", Price = 39.99m, StockQuantity = 80, SKU = "BOOK-TECH-005", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, MainCategoryId = 3, Location = geometryFactory.CreatePoint(new Coordinate(35.6895, 139.6917)) },
            new Product { Id = 6, Name = "T-Shirt - Blue", Description = "Cotton T-shirt size M", Price = 19.99m, StockQuantity = 100, SKU = "CLOTH-TSHIRT-006", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, MainCategoryId = 3, Location = geometryFactory.CreatePoint(new Coordinate(33.8688, 151.2093)) },
            new Product { Id = 7, Name = "Jeans - Regular Fit", Description = "Comfortable denim jeans", Price = 49.99m, StockQuantity = 60, SKU = "CLOTH-JEANS-007", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, MainCategoryId = 4, Location = geometryFactory.CreatePoint(new Coordinate(-118.2437, 34.0522)) },
            new Product { Id = 8, Name = "Cooking Pan Set", Description = "Non-stick kitchen pan set", Price = 89.99m, StockQuantity = 40, SKU = "HOME-PAN-008", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, MainCategoryId = 4, Location = geometryFactory.CreatePoint(new Coordinate(40.7128, 74.0060)) },
            new Product { Id = 9, Name = "Coffee Maker", Description = "Automatic drip coffee machine", Price = 129.99m, StockQuantity = 30, SKU = "HOME-COFFEE-009", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, MainCategoryId = 5, Location = geometryFactory.CreatePoint(new Coordinate(48.8566, 2.3522)) },
            new Product { Id = 10, Name = "Running Shoes", Description = "Lightweight running shoes", Price = 89.99m, StockQuantity = 60, SKU = "SPORT-SHOES-010", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, MainCategoryId = 5, Location = geometryFactory.CreatePoint(new Coordinate(51.5074, 0.1278)) },
            new Product { Id = 11, Name = "Basketball", Description = "Official size basketball", Price = 29.99m, StockQuantity = 150, SKU = "SPORT-BALL-011", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, MainCategoryId = 6, Location = geometryFactory.CreatePoint(new Coordinate(35.6895, 139.6917)) },
            new Product { Id = 12, Name = "LEGO Starter Set", Description = "Creative building blocks for kids", Price = 59.99m, StockQuantity = 120, SKU = "TOY-LEGO-012", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, MainCategoryId = 6, Location = geometryFactory.CreatePoint(new Coordinate(33.8688, 151.2093)) },
            new Product { Id = 13, Name = "Action Figure", Description = "Superhero action figure", Price = 24.99m, StockQuantity = 90, SKU = "TOY-FIGURE-013", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, MainCategoryId = 7, Location = geometryFactory.CreatePoint(new Coordinate(-118.2437, 34.0522)) },
            new Product { Id = 14, Name = "Lipstick - Red", Description = "Matte finish lipstick", Price = 14.99m, StockQuantity = 140, SKU = "BEAUTY-LIP-014", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, MainCategoryId = 7, Location = geometryFactory.CreatePoint(new Coordinate(40.7128, 74.0060)) },
            new Product { Id = 15, Name = "Shampoo 500ml", Description = "Organic hair care shampoo", Price = 12.99m, StockQuantity = 200, SKU = "BEAUTY-CARE-015", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, MainCategoryId = 8, Location = geometryFactory.CreatePoint(new Coordinate(48.8566, 2.3522)) },
            new Product { Id = 16, Name = "Car Vacuum Cleaner", Description = "Portable auto vacuum cleaner", Price = 49.99m, StockQuantity = 70, SKU = "AUTO-TOOL-016", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, MainCategoryId = 8, Location = geometryFactory.CreatePoint(new Coordinate(51.5074, 0.1278)) },
            new Product { Id = 17, Name = "Car Phone Holder", Description = "Magnetic phone holder for cars", Price = 19.99m, StockQuantity = 110, SKU = "AUTO-HOLDER-017", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, MainCategoryId = 9, Location = geometryFactory.CreatePoint(new Coordinate(35.6895, 139.6917)) },
            new Product { Id = 18, Name = "Acoustic Guitar", Description = "6-string wooden acoustic guitar", Price = 199.99m, StockQuantity = 25, SKU = "MUSIC-GUITAR-018", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, MainCategoryId = 9, Location = geometryFactory.CreatePoint(new Coordinate(33.8688, 151.2093)) },
            new Product { Id = 19, Name = "Digital Piano", Description = "88-key digital piano", Price = 599.99m, StockQuantity = 15, SKU = "MUSIC-PIANO-019", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, MainCategoryId = 10, Location = geometryFactory.CreatePoint(new Coordinate(-118.2437, 34.0522)) },
            new Product { Id = 20, Name = "Office Chair", Description = "Ergonomic office chair", Price = 149.99m, StockQuantity = 35, SKU = "OFFICE-CHAIR-020", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, MainCategoryId = 10, Location = geometryFactory.CreatePoint(new Coordinate(40.7128, 74.0060)) }
        };
        //db.Products.AddRange(products);

        // ProductCategories
        var productCategories = new List<ProductCategory>
        {
            new ProductCategory { Id=1,ProductId = 1, CategoryId = 1 },
            new ProductCategory { Id=2,ProductId = 2, CategoryId = 1 },
            new ProductCategory { Id=3,ProductId = 3, CategoryId = 1 },
            new ProductCategory { Id=4,ProductId = 4, CategoryId = 2 },
            new ProductCategory { Id=5,ProductId = 5, CategoryId = 2 },
            new ProductCategory { Id=6,ProductId = 6, CategoryId = 3 },
            new ProductCategory { Id=7,ProductId = 7, CategoryId = 3 },
            new ProductCategory { Id=8,ProductId = 8, CategoryId = 4 },
            new ProductCategory { Id=9,ProductId = 9, CategoryId = 4 },
            new ProductCategory { Id=10,ProductId = 10, CategoryId = 5 },
            new ProductCategory { Id=11,ProductId = 11, CategoryId = 5 },
            new ProductCategory { Id=12,ProductId = 12, CategoryId = 6 },
            new ProductCategory { Id=13,ProductId = 13, CategoryId = 6 },
            new ProductCategory { Id=14,ProductId = 14, CategoryId = 7 },
            new ProductCategory { Id=15,ProductId = 15, CategoryId = 7 },
            new ProductCategory { Id=16,ProductId = 16, CategoryId = 8 },
            new ProductCategory { Id=17,ProductId = 17, CategoryId = 8 },
            new ProductCategory { Id=18,ProductId = 18, CategoryId = 9 },
            new ProductCategory { Id=19,ProductId = 19, CategoryId = 9 },
            new ProductCategory { Id=20,ProductId = 20, CategoryId = 10 },
            new ProductCategory { Id=21,ProductId = 2, CategoryId = 5 },
            new ProductCategory { Id=22,ProductId = 3, CategoryId = 9 },
            new ProductCategory { Id=23,ProductId = 8, CategoryId = 10 }
        };
        //db.ProductCategories.AddRange(productCategories);

        db.SaveChanges();
    }
}
