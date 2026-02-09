using AlJawad.SqlDynamicLinkerShowCases.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using AlJawad.SqlDynamicLinker.Extensions;

namespace AlJawad.SqlDynamicLinkerShowCases.DB
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
       : base(options)
        {
        }
        //  private readonly string _connectionString;

        //  public AppDbContext(IConfiguration configuration)
        //  {
        //      _connectionString = configuration.GetConnectionString("DefaultConnection");
        //  }


        //  protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlServer(_connectionString, sqlServerOptionsAction =>
        //{
        //    //sqlServerOptionsAction.CommandTimeout(30);
        //    //sqlServerOptionsAction.EnableRetryOnFailure(3);
        //    //TODO need to be reviewed
        //    //sqlServerOptionsAction.MigrationsAssembly("Recta.Infra.Migration");
        //    sqlServerOptionsAction.CommandTimeout(1200);
        //    sqlServerOptionsAction.UseNetTopologySuite();
        //}).UseSpatialExtensions();

        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //    => options.UseInMemoryDatabase("TestDb");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Register();

            modelBuilder.Entity<ProductCategory>()
                .HasOne(pc => pc.Category)
                .WithMany()
                .HasForeignKey(pc => pc.CategoryId)
                .OnDelete(DeleteBehavior.Restrict); // or DeleteBehavior.NoAction

            modelBuilder.Entity<ProductCategory>()
                .HasOne<Product>() // If you have navigation, use .HasOne(pc => pc.Product)
                .WithMany()
                .HasForeignKey(pc => pc.ProductId)
                .OnDelete(DeleteBehavior.Cascade); // Only one can be Cascade
        }
    }
}
