using Azure.Storage.Blobs;
using HaviraApi.Entities;
using HaviraApi.Middleware;
using HaviraApi.Repositories;
using HaviraApi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//SERVICES
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<IDishService, DishService>();

//REPOS
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<IDishRepository, DishRepository>();


builder.Services.AddControllers();

//MIDDLEWARE
builder.Services.AddScoped<ErrorHandlingMiddleware>();

//MAPPER
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//SQL STORAGE
builder.Services.AddDbContext<HaviraDbContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("HaviraDbConnection")
        ));

//BLOB STORAGE
builder.Services.AddSingleton(x =>
    new BlobServiceClient(
        builder.Configuration.GetConnectionString("HaviraBlobStorage")
        ));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

