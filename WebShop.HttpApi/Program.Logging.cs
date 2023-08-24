using Microsoft.AspNetCore.HttpLogging;

namespace WebShop.HttpApi;

using static HttpLoggingFields;

internal static class ProgramExtensions
{
    public static void Configure(this HttpLoggingOptions @this)
    {
        @this.LoggingFields = RequestPath | RequestHeaders | RequestBody | ResponseStatusCode | ResponseBody;
        @this.RequestBodyLogLimit = 4096;
    }
}
