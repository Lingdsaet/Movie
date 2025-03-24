using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Movie.Models;
using Movie.Repository;

var builder = WebApplication.CreateBuilder(args);

var allowSpecificOrigins = "allowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyHeader();
                          policy.AllowAnyMethod();
                          policy.AllowAnyOrigin();
                      });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Movie API", Version = "v1" });

    // H? tr? upload file trong Swagger
    c.OperationFilter<FileUploadOperation>();
});
IConfigurationRoot cf = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();
builder.Services.AddDbContext<movieDB>(opt => opt.UseSqlServer(cf.GetConnectionString("cnn")));

builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<IMovieCategoryRepository<MovieCategory>, MovieCategoryRepository>();
builder.Services.AddScoped<IMovieActorRepository<MovieActor>, MovieActorRepository>();
builder.Services.AddScoped<IMovieHome, MovieHomeRepository>();
builder.Services.AddScoped<IActorRepository, ActorRepository>();
builder.Services.AddScoped<IDirectorsRepository, DirectorRepository>();
builder.Services.AddScoped<ISeriesRepository, SeriesRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ??m b?o th? m?c Assets ?ã ???c thi?t l?p
//var folderPath = Path.Combine(app.Environment.WebRootPath, "Assets");
//if (!Directory.Exists(folderPath))
//{
//    Directory.CreateDirectory(folderPath);
//}
app.UseAuthorization();

app.MapControllers();

app.Run();
