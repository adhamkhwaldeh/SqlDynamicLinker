namespace AlJawad.SqlDynamicLinkerShowCases.Entities
{
    public class ProductCategory
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int CategoryId { get; set; }

        // Navigation
        //public Product Product { get; set; }
        public Category Category { get; set; }
    }
}
