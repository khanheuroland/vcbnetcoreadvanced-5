using cookieauth;

var builder = WebApplication.CreateBuilder(args);

Startup startup = new Startup(builder.Services, builder.Configuration);
startup.ConfigureServices();

var app = builder.Build();

startup.Configure(app, app.Environment);

app.Run();