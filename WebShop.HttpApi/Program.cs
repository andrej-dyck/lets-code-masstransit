using WebShop.HttpApi;
using WebShop.Orders;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddHttpLogging(c => c.Configure());
}

var app = builder.Build();
{
    app.UseHttpLogging();
    app.MapGroup("api").MapOrderEndpoints();
}

app.Run();
