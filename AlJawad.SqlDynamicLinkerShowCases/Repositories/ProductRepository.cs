using AlJawad.SqlDynamicLinkerShowCases.Entities;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace AlJawad.SqlDynamicLinkerShowCases.Repositories
{
    public class ProductRepository
    {
        private readonly IQueryable<Category> _categories;
        private readonly IQueryable<Product> _products;
        private readonly IQueryable<ProductCategory> _productCategories;

        public ProductRepository()
        {
            // Categories
            var _categoryList = new List<Category>
        {
            new Category { Id = 1, Name = "Electronics", Description = "Devices and gadgets" },
            new Category { Id = 2, Name = "Books", Description = "Printed and digital books" },
            new Category { Id = 3, Name = "Clothing", Description = "Apparel and fashion items" },
            new Category { Id = 4, Name = "Home & Kitchen", Description = "Household essentials" },
            new Category { Id = 5, Name = "Sports", Description = "Sporting goods and equipment" },
            new Category { Id = 6, Name = "Toys", Description = "Children’s toys and games" },
            new Category { Id = 7, Name = "Beauty", Description = "Cosmetics and personal care" },
            new Category { Id = 8, Name = "Automotive", Description = "Car accessories and tools" },
            new Category { Id = 9, Name = "Music", Description = "Instruments and audio gear" },
            new Category { Id = 10, Name = "Office Supplies", Description = "Work and study essentials" }
        };
            _categories = _categoryList.AsQueryable();

            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

            // Products
            _products = new List<Product>
        {
            new Product { Id = 1, Name = "Smartphone X", Description = "Latest smartphone with 128GB storage", Price = 699.99m, StockQuantity = 50, SKU = "ELEC-SMART-001", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow,MainCategoryId = _categoryList[0].Id,MainCategory = _categoryList[0], Location = geometryFactory.CreatePoint(new Coordinate(34.0522, -118.2437)) },
            new Product { Id = 2, Name = "Laptop Pro 15\"", Description = "High-performance laptop", Price = 1299.99m, StockQuantity = 20, SKU = "ELEC-LAPTOP-002", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow ,MainCategoryId = _categoryList[0].Id,MainCategory = _categoryList[0], Location = geometryFactory.CreatePoint(new Coordinate(40.7128, -74.0060)) },
            new Product { Id = 3, Name = "Wireless Headphones", Description = "Noise-cancelling headphones", Price = 199.99m, StockQuantity = 75, SKU = "ELEC-AUDIO-003", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow ,MainCategoryId = _categoryList[1].Id,MainCategory = _categoryList[1], Location = geometryFactory.CreatePoint(new Coordinate(48.8566, 2.3522))},
            new Product { Id = 4, Name = "Novel - The Great Story", Description = "Bestselling fiction novel", Price = 14.99m, StockQuantity = 200, SKU = "BOOK-NOVEL-004", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow ,MainCategoryId = _categoryList[1].Id,MainCategory = _categoryList[1], Location = geometryFactory.CreatePoint(new Coordinate(51.5074, -0.1278)) },
            new Product { Id = 5, Name = "Programming in C#", Description = "Technical programming book", Price = 39.99m, StockQuantity = 80, SKU = "BOOK-TECH-005", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow ,MainCategoryId = _categoryList[2].Id,MainCategory = _categoryList[2], Location = geometryFactory.CreatePoint(new Coordinate(35.6895, 139.6917)) },
            new Product { Id = 6, Name = "T-Shirt - Blue", Description = "Cotton T-shirt size M", Price = 19.99m, StockQuantity = 100, SKU = "CLOTH-TSHIRT-006", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow ,MainCategoryId = _categoryList[2].Id,MainCategory = _categoryList[2], Location = geometryFactory.CreatePoint(new Coordinate(-33.8688, 151.2093)) },
            new Product { Id = 7, Name = "Jeans - Regular Fit", Description = "Comfortable denim jeans", Price = 49.99m, StockQuantity = 60, SKU = "CLOTH-JEANS-007", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow ,MainCategoryId = _categoryList[3].Id,MainCategory = _categoryList[3], Location = geometryFactory.CreatePoint(new Coordinate(34.0522, -118.2437)) },
            new Product { Id = 8, Name = "Cooking Pan Set", Description = "Non-stick kitchen pan set", Price = 89.99m, StockQuantity = 40, SKU = "HOME-PAN-008", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow ,MainCategoryId = _categoryList[3].Id,MainCategory = _categoryList[3], Location = geometryFactory.CreatePoint(new Coordinate(40.7128, -74.0060))},
            new Product { Id = 9, Name = "Coffee Maker", Description = "Automatic drip coffee machine", Price = 129.99m, StockQuantity = 30, SKU = "HOME-COFFEE-009", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow ,MainCategoryId = _categoryList[4].Id,MainCategory = _categoryList[4], Location = geometryFactory.CreatePoint(new Coordinate(48.8566, 2.3522))},
            new Product { Id = 10, Name = "Running Shoes", Description = "Lightweight running shoes", Price = 89.99m, StockQuantity = 60, SKU = "SPORT-SHOES-010", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow ,MainCategoryId = _categoryList[4].Id,MainCategory = _categoryList[4], Location = geometryFactory.CreatePoint(new Coordinate(51.5074, -0.1278)) },
            new Product { Id = 11, Name = "Basketball", Description = "Official size basketball", Price = 29.99m, StockQuantity = 150, SKU = "SPORT-BALL-011", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow ,MainCategoryId = _categoryList[5].Id,MainCategory = _categoryList[5], Location = geometryFactory.CreatePoint(new Coordinate(35.6895, 139.6917))},
            new Product { Id = 12, Name = "LEGO Starter Set", Description = "Creative building blocks for kids", Price = 59.99m, StockQuantity = 120, SKU = "TOY-LEGO-012", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow ,MainCategoryId = _categoryList[5].Id,MainCategory = _categoryList[5], Location = geometryFactory.CreatePoint(new Coordinate(-33.8688, 151.2093)) },
            new Product { Id = 13, Name = "Action Figure", Description = "Superhero action figure", Price = 24.99m, StockQuantity = 90, SKU = "TOY-FIGURE-013", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow ,MainCategoryId = _categoryList[6].Id,MainCategory = _categoryList[6], Location = geometryFactory.CreatePoint(new Coordinate(34.0522, -118.2437))},
            new Product { Id = 14, Name = "Lipstick - Red", Description = "Matte finish lipstick", Price = 14.99m, StockQuantity = 140, SKU = "BEAUTY-LIP-014", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow ,MainCategoryId = _categoryList[6].Id,MainCategory = _categoryList[6], Location = geometryFactory.CreatePoint(new Coordinate(40.7128, -74.0060)) },
            new Product { Id = 15, Name = "Shampoo 500ml", Description = "Organic hair care shampoo", Price = 12.99m, StockQuantity = 200, SKU = "BEAUTY-CARE-015", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow ,MainCategoryId = _categoryList[7].Id,MainCategory = _categoryList[7], Location = geometryFactory.CreatePoint(new Coordinate(48.8566, 2.3522)) },
            new Product { Id = 16, Name = "Car Vacuum Cleaner", Description = "Portable auto vacuum cleaner", Price = 49.99m, StockQuantity = 70, SKU = "AUTO-TOOL-016", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow ,MainCategoryId = _categoryList[7].Id,MainCategory = _categoryList[7], Location = geometryFactory.CreatePoint(new Coordinate(51.5074, -0.1278)) },
            new Product { Id = 17, Name = "Car Phone Holder", Description = "Magnetic phone holder for cars", Price = 19.99m, StockQuantity = 110, SKU = "AUTO-HOLDER-017", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow ,MainCategoryId = _categoryList[8].Id,MainCategory = _categoryList[8], Location = geometryFactory.CreatePoint(new Coordinate(35.6895, 139.6917)) },
            new Product { Id = 18, Name = "Acoustic Guitar", Description = "6-string wooden acoustic guitar", Price = 199.99m, StockQuantity = 25, SKU = "MUSIC-GUITAR-018", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow ,MainCategoryId = _categoryList[8].Id,MainCategory = _categoryList[8], Location = geometryFactory.CreatePoint(new Coordinate(-33.8688, 151.2093)) },
            new Product { Id = 19, Name = "Digital Piano", Description = "88-key digital piano", Price = 599.99m, StockQuantity = 15, SKU = "MUSIC-PIANO-019", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow ,MainCategoryId = _categoryList[9].Id,MainCategory = _categoryList[9], Location = geometryFactory.CreatePoint(new Coordinate(34.0522, -118.2437))},
            new Product { Id = 20, Name = "Office Chair", Description = "Ergonomic office chair", Price = 149.99m, StockQuantity = 35, SKU = "OFFICE-CHAIR-020", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow ,MainCategoryId = _categoryList[9].Id,MainCategory = _categoryList[9], Location = geometryFactory.CreatePoint(new Coordinate(40.7128, -74.0060))}

        }.AsQueryable();

            // Relations
            _productCategories = new List<ProductCategory>
        {
            new ProductCategory { ProductId = 1, CategoryId = 1 },
            new ProductCategory { ProductId = 2, CategoryId = 1 },
            new ProductCategory { ProductId = 3, CategoryId = 1 },
            new ProductCategory { ProductId = 4, CategoryId = 2 },
            new ProductCategory { ProductId = 5, CategoryId = 2 },
            new ProductCategory { ProductId = 6, CategoryId = 3 },
            new ProductCategory { ProductId = 7, CategoryId = 3 },
            new ProductCategory { ProductId = 8, CategoryId = 4 },
            new ProductCategory { ProductId = 9, CategoryId = 4 },
            new ProductCategory { ProductId = 10, CategoryId = 5 },
            new ProductCategory { ProductId = 11, CategoryId = 5 },
            new ProductCategory { ProductId = 12, CategoryId = 6 },
            new ProductCategory { ProductId = 13, CategoryId = 6 },
            new ProductCategory { ProductId = 14, CategoryId = 7 },
            new ProductCategory { ProductId = 15, CategoryId = 7 },
            new ProductCategory { ProductId = 16, CategoryId = 8 },
            new ProductCategory { ProductId = 17, CategoryId = 8 },
            new ProductCategory { ProductId = 18, CategoryId = 9 },
            new ProductCategory { ProductId = 19, CategoryId = 9 },
            new ProductCategory { ProductId = 20, CategoryId = 10 },

            // Cross-category examples
            new ProductCategory { ProductId = 2, CategoryId = 5 },  // Laptop in Sports (gaming use)
            new ProductCategory { ProductId = 3, CategoryId = 9 },  // Headphones in Music
            new ProductCategory { ProductId = 8, CategoryId = 10 }  // Cooking pans in Office (for breakroom)
        }.AsQueryable();

            // Hook up navigation properties (simulate EF)
            foreach (var pc in _productCategories)
            {
                var product = _products.First(p => p.Id == pc.ProductId);
                var category = _categories.First(c => c.Id == pc.CategoryId);

                //pc.Product = product;
                pc.Category = category;

                product.ProductCategories ??= new List<ProductCategory>();
                //category.ProductCategories ??= new List<ProductCategory>();

                product.ProductCategories.Add(pc);
                //category.ProductCategories.Add(pc);
            }
        }

        public IQueryable<Product> GetProducts() => _products;
        public IQueryable<Category> GetCategories() => _categories;
        public IQueryable<ProductCategory> GetProductCategories() => _productCategories;

    }

}
