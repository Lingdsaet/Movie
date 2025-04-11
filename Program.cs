using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Movie.Models;
using Movie.Repository;
using Movie.Service;
using Newtonsoft.Json; // Add this using directive
using Microsoft.AspNetCore.Mvc.NewtonsoftJson; // Add this using directive

var builder = WebApplication.CreateBuilder(args);

// Th�m d?ch v? v�o container.
builder.Services.AddControllers();

// Th�m Swagger cho OpenAPI
builder.Services.AddScoped<JwtService>(); // Register JwtService as Scoped
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

// Th�m h? tr? file upload trong Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Movie API", Version = "v1" });

    // Th�m ph?n h? tr? file upload
    c.OperationFilter<FileUploadOperation>();
});

// ??c c?u h�nh t? appsettings.json
IConfigurationRoot configuration = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

// C?u h�nh DbContext v?i SQL Server
builder.Services.AddDbContext<movieDB>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("cnn")));

builder.Services.AddMvc(options =>
{
    options.EnableEndpointRouting = false;
}).AddNewtonsoftJson(opt =>
{
    opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

// Th�m c�c repository v�o container
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<IMovieCategoryRepository<MovieCategories>, MovieCategoryRepository>();
builder.Services.AddScoped<IMovieActorRepository<MovieActors>, MovieActorRepository>();
builder.Services.AddScoped<IMovieHome, MovieHomeRepository>();
builder.Services.AddScoped<IActorRepository, ActorRepository>();
builder.Services.AddScoped<IDirectorsRepository, DirectorRepository>();
builder.Services.AddScoped<ISeriesRepository, SeriesRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IUserUserRepository, UserUserRepository>();
builder.Services.AddScoped<ContentRepository>();


var app = builder.Build();

// C?u h�nh pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAllOrigins"); // Enable CORS policy
app.UseAuthorization();
app.MapControllers();

app.Run();
