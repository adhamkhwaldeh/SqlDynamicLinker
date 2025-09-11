namespace AlJawad.SqlDynamicLinkerShowCases.Entities
{
    public class Category
    {
        public int Id { get; set; }            // Unique identifier
        public string Name { get; set; }       // Category name (e.g. "Electronics")
        public string Description { get; set; }// Optional details

        // Navigation
        //public ICollection<ProductCategory> ProductCategories { get; set; }
    }
}
