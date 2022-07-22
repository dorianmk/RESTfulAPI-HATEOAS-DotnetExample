using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using WebAPI.ActionFilters;
using WebAPI.Converters;
using WebAPI.Data;
using WebAPI.Dtos.Author;
using WebAPI.Dtos.Book;
using WebAPI.Hateoas;
using WebAPI.Interfaces;
using WebAPI.Models;
using WebAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddControllers(options =>
    {
        options.Filters.Add<ResponseExceptionFilter>();
        options.Filters.Add<HateoasResultFilter>();
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});
builder.Services.AddHttpContextAccessor();

var connectionString = builder.Configuration.GetConnectionString("SQLiteConnection");
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IConverter<Author, AuthorDto>, AuthorConverter>();
builder.Services.AddScoped<IOneWayConverter<Author, AuthorDetailsDto>, AuthorDetailsConverter>();
builder.Services.AddScoped<IOneWayConverter<NewAuthorDto, Author>, NewAuthorConverter>();
builder.Services.AddScoped<IConverter<Book, BookDto>, BookConverter>();
builder.Services.AddScoped<IOneWayConverter<Book, BookDetailsDto>, BookDetailsConverter>();
builder.Services.AddScoped<IOneWayConverter<NewBookDto, Book>, NewBookConverter>();
builder.Services.AddScoped<ILinksFactory, LinksFactory>();
builder.Services.AddSqlite<BookContext>(connectionString);
builder.Services.AddAutoMapper(typeof(MappingProfiles));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.ApplyMigrationsToDb();

app.Run();
