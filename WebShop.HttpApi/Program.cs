using MassTransit;
using WebShop.HttpApi;
using WebShop.Notifications;
using WebShop.Orders;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddHttpLogging(c => c.Configure());
    builder.Services.AddMassTransit(
        c =>
        {
            c.SetKebabCaseEndpointNameFormatter();
            c.RegisterNotifications();
            c.UsingInMemory((context, cfg) => cfg.ConfigureEndpoints(context));
        }
    );
    builder.Services.AddOrdersFeature();
}

var app = builder.Build();
{
    app.UseHttpLogging();
    app.MapGroup("api").MapOrderEndpoints();
}

app.Run();
