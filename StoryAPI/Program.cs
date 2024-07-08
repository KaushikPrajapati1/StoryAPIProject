using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
//using StoryAPI.Repository;
using HackerStoryBusinessLayer.Repository;
using Microsoft.AspNetCore.Diagnostics;
using StoryAPI;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
       policy =>
       {
           policy.WithOrigins("http://localhost:4200")
                  .WithHeaders(HeaderNames.ContentType, "x-custom-header");
       });
});
builder.Services.AddTransient<IHackerStoryRepository, HackerStoryRepository>();
builder.Services.AddControllers();

builder.Services.AddMemoryCache();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Api Versioning

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthorization();

app.MapControllers();

app.Run();
