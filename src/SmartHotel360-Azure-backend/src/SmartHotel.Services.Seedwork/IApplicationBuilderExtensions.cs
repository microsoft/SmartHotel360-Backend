using Microsoft.AspNetCore.Builder;
using SmartHotel.Services.Seedwork;
using System;

namespace Microsoft.AspNetCore.Builder
{
    public static class IApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseByPassAuth(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ByPassAuthMiddleware>();
        }

    }
}
