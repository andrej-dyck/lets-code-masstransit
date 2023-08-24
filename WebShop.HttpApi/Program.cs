using WebShop.HttpApi;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddHttpLogging(c => c.Configure());
}

var app = builder.Build();
{
    app.UseHttpLogging();
}

app.Run();
