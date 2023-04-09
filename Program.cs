using Bugsnag;
using ContactService.Exceptions;
using ContactService.Repository;
using ContactService.Repository.Connection;
using ContactService.Services;
using ContactService.Services.Keys;

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
builder.Services.AddTransient<IUserService>(s => new UserService(s.GetRequiredService<UserRepository>()));
builder.Services.AddTransient<IKeyService>(s => new KeyService(s.GetRequiredService<KeyRepository>(), s.GetRequiredService<IUserService>()));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<HttpExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
