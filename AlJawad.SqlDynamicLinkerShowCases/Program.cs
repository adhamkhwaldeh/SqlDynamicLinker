using AlJawad.SqlDynamicLinkerShowCases.Repositories;
using AlJawad.SqlDynamicLinker.ModelBinder;
using System.Reflection;
using AlJawad.SqlDynamicLinkerShowCases.DB;
using Microsoft.EntityFrameworkCore;
using AlJawad.SqlDynamicLinker;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Infrastructure;
using AlJawad.SqlDynamicLinker.Extensions;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Register AutoMapper scanning the current assembly (or specify types)
builder.Services.AddAutoMapper(typeof(Program));

// Register controllers
builder.Services.AddControllers().AddJsonOptions(
                x =>
                {
                    x.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    //x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                })
     .AddNewtonsoftJson(options =>
     {
         // Handle cycles safely (optional)
         options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
         options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
     }).ConfigureApiBehaviorOptions(options => { options.SuppressModelStateInvalidFilter = true; });

builder.Services.InitializerSqlDynamicLinker();

// Register your repository as singleton (so dummy data persists)
builder.Services.AddSingleton<ProductRepository>();

// Register AppDbContext
builder.Services.AddDbContext<AppDbContext>(options =>
{

    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlServerOptionsAction =>
        {
            sqlServerOptionsAction.CommandTimeout(1200);
            sqlServerOptionsAction.UseNetTopologySuite();
        }).UseSpatialExtensions();
}
    );


// Add Swagger

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenNewtonsoftSupport();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGenNewtonsoftSupport();

// Add services to the container.
builder.Services.AddRazorPages();


builder.Services.AddMvc()
         .AddJsonOptions(
         x =>
         {
             x.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
             //x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
         })
         .AddNewtonsoftJson(options =>
         {
             options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
             options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
             //options.SerializerSettings.Converters.Add(new InvalidDateToNullConverter());
         })
         .ConfigureApiBehaviorOptions(options =>
         {
             options.SuppressModelStateInvalidFilter = true;
         });


builder.Services.AddControllersWithViews(); // add MVC



var app = builder.Build();

// Enable Swagger middleware (only in dev for best practice)
if (app.Environment.IsDevelopment())
{
    //builder.Services.AddSwaggerGen(options =>
    //{
    //    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    //    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    //});
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Product API V1");
        options.RoutePrefix = string.Empty; // Swagger UI at root "/"
    });
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.MapControllers();


app.UseRouting();

app.UseAuthorization();

// enable static files, routing, etc.
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated(); // or Migrate()
    ProductRepositorySeeder.Seed(db);
}

foreach (var sd in builder.Services.Where(s => s.ServiceType == typeof(IConventionSetPlugin)))
{
    Console.WriteLine("Registered plugin: " + sd.ImplementationType?.FullName);
}

app.Run();
