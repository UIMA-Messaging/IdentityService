using IdentityService.Exceptions;
using IdentityService.Repositories.Connection;
using IdentityService.Services;
using IdentityService.Services.Keys;
using IdentityService.Contracts;
using IdentityService.RabbitMQ.Connection;
using IdentityService.RabbitMQ;
using Bugsnag;
using IdentityService.Repositories.Users;
using IdentityService.Repositories.Keys;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Bugsnag
builder.Services.AddSingleton<IClient>(_ => new Client(builder.Configuration["Bugsnag:ApiKey"]));

// Repositories
builder.Services.AddSingleton<IUserRepository>(_ => new UserRepository(new ConnectionFactory(builder.Configuration["ConnectionStrings:Users"])));
builder.Services.AddSingleton<IKeyRepository>(_ => new KeyRepository(new ConnectionFactory(builder.Configuration["ConnectionStrings:Keys"])));

// RabbitMQ
builder.Services.AddSingleton<IRabbitMQConnection>(_ => new RabbitMQConnection(builder.Configuration["RabbitMQ:Host"], builder.Configuration["RabbitMQ:Username"], builder.Configuration["RabbitMQ:Password"]));
builder.Services.AddSingleton<IRabbitMQListener<ExchangeKeys>>(s => new RabbitMQListener<ExchangeKeys>(s.GetRequiredService<IRabbitMQConnection>(), "identity.users.keys", builder.Configuration["RabbitMQ:UserRegistrations:Exchange"], builder.Configuration["RabbitMQ:UserRegistrations:RoutingKey"]));

// Services
builder.Services.AddTransient(s => new UserService(s.GetRequiredService<IUserRepository>()));
builder.Services.AddSingleton(s => new KeyService(s.GetRequiredService<IKeyRepository>(), s.GetRequiredService<IUserRepository>(), s.GetRequiredService<IRabbitMQListener<ExchangeKeys>>()));

var app = builder.Build();

// Singleton instantiations
app.Services.GetRequiredService<KeyService>();

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
