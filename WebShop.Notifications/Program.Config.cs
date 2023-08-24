using MassTransit;

namespace WebShop.Notifications;

public static class ProgramConfig
{
    public static IRegistrationConfigurator RegisterNotifications(this IRegistrationConfigurator @this)
    {
        @this.AddConsumers(typeof(ProgramConfig).Assembly);
        return @this;
    }
}
