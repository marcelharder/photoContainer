using photoContainer.data.seed;

var builder = WebApplication.CreateBuilder(args);

//Initialize AppSettings
AppSettings.Initialize(builder.Configuration);

// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddCors();
builder.Services.Configure<ComSettings>(builder.Configuration.GetSection("ComSettings"));

builder.Services.AddControllers().AddJsonOptions(options => { options.JsonSerializerOptions.PropertyNamingPolicy = null; });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// initialize the database ...

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<ApplicationDbContext>();
    var dapper = services.GetRequiredService<DapperContext>();
    var image = services.GetRequiredService<IImage>();
    var category = services.GetRequiredService<ICategory>();
    await context.Database.EnsureCreatedAsync();
    await Seed.SeedCategories(context);
    await Seed.SeedImages(context, image, category);
    await Seed.AddMainPhotoToCategories(context);

}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during migration");
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    /* {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "photoContainer API V1");
        c.RoutePrefix = string.Empty;
    }); */
}

//app.UseHttpsRedirection();
/* app.UseStaticFiles(new StaticFileOptions{
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "MyStaticFiles")),
    RequestPath = "/StaticFiles"
});
 */
app.UseCors(x => x.AllowAnyHeader()
           .AllowAnyMethod()
           .AllowCredentials()
           .WithOrigins("http://localhost:4200"));

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();
//app.MapFallbackToController("Index", "Fallback");

app.Run();
