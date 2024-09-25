using basicauthentication;

var builder = WebApplication.CreateBuilder(args);

Startup startup = new Startup(builder.Services);
startup.ConfigureServices();

var app = builder.Build();

startup.Configure(app, app.Environment);

app.Run();
