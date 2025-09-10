namespace AlJawad.SqlDynamicLinkerShowCases.Entities
{
    public class Product
    {
        public int Id { get; set; }            // Unique identifier
        public string Name { get; set; }       // Product name
        public string Description { get; set; }// Detailed info
        public decimal Price { get; set; }     // Selling price
        public int StockQuantity { get; set; } // Available units
        public string SKU { get; set; }        // Stock keeping unit (unique code)
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation
        public ICollection<ProductCategory> ProductCategories { get; set; }
    }
}
