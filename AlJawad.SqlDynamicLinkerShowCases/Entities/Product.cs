using NetTopologySuite.Geometries;
using Newtonsoft.Json;

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

        [JsonIgnore]
        public Point Location { get; set; }

        // Custom properties for serialization
        [JsonProperty("latitude")]
        public double Latitude
        {
            get => Location.Y;    // or use double if you prefer
            set => Location = new Point(Location.X, value);
        }

        [JsonProperty("longitude")]
        public double Longitude
        {
            get => Location.X;
            set => Location = new Point(value, Location.Y);
        }

        public int MainCategoryId { get; set; }

        public Category MainCategory { get; set; }

        // Navigation
        public ICollection<ProductCategory> ProductCategories { get; set; }
    }
}
