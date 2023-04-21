using IdentityService.Exceptions;
using IdentityService.Repository;
using IdentityService.Repository.Connection;
using IdentityService.Services;
using IdentityService.Services.Keys;
using Bugsnag;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Bugsnag
builder.Services.AddSingleton<IClient>(_ => new Client(builder.Configuration["Bugsnag:ApiKey"]));

// Repositories
builder.Services.AddSingleton(_ => new UserRepository(new ConnectionFactory(builder.Configuration["ConnectionStrings:Users"])));
builder.Services.AddSingleton(_ => new KeyRepository(new ConnectionFactory(builder.Configuration["ConnectionStrings:Keys"])));

// Services
builder.Services.AddTransient(s => new UserService(s.GetRequiredService<UserRepository>()));
builder.Services.AddTransient(s => new KeyService(s.GetRequiredService<KeyRepository>(), s.GetRequiredService<UserService>()));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<HttpExceptionMiddleware>();
app.UseCors(options =>
{
    options.AllowAnyMethod()
        .AllowAnyHeader()
        .SetIsOriginAllowed(origin => true)
        .AllowCredentials();
});
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
