using CqrsMediatrNotesAPI.Contexts;
using CqrsMediatrNotesAPI.Interfaces;
using CqrsMediatrNotesAPI.Models;
using CqrsMediatrNotesAPI.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<Context<Read,Note>>( options => 
    options.UseSqlite($"Data Source=notesRead.sqlite")
);

builder.Services.AddDbContext<Context<Write,Note>>( options => 
    options.UseSqlite($"Data Source=notesWrite.sqlite")
);

builder.Services.AddScoped<IReadRepository<Note>, ReadRepository<Note>>();
builder.Services.AddScoped<IWriteRepository<Note>, WriteRepository<Note>>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

var app = builder.Build();

using (var scope = app.Services.CreateScope()) {
    var readNotesContext = scope.ServiceProvider.GetRequiredService<Context<Read,Note>>();
    var writeNotesContext = scope.ServiceProvider.GetRequiredService<Context<Write,Note>>();

    readNotesContext.Database.EnsureCreated();
    writeNotesContext.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();