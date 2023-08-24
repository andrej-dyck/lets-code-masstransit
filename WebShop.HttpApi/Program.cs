using DotNet.Extensions;
using MassTransit;
using WebShop.HttpApi;
using WebShop.Notifications;
using WebShop.Orders;

var builder = WebApplication.CreateBuilder(args);
{
    // logging
    builder.Services.AddHttpLogging(c => c.Configure());

    // message bus config
    builder.Services.AddMassTransit(
        c =>
        {
            if (builder.Environment.IsDevelopment())
                c.UsingInMemory((context, cfg) => cfg.ConfigureEndpoints(context));
            else
                throw new Todo("use a message broker for production"); // TODO

            // so that the queues are named order-placed and not OrderPlaced 
            c.SetKebabCaseEndpointNameFormatter();

            // register consumers, sagas, etc.
            c.RegisterNotifications();
        }
    );

    // services
    builder.Services.AddSingleton<UtcNow>(_ => () => DateTimeOffset.UtcNow);
    builder.Services.AddOrdersFeature();
}

var app = builder.Build();
{
    app.UseHttpLogging();
    app.MapGroup("api").MapOrderEndpoints();
}

app.Run();
