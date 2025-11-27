using System;
using NodaTime;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime ToLocal(this DateTime dateTime, string timeZone)
        {
            var zone = DateTimeZoneProviders.Tzdb[timeZone];
            return Instant.FromDateTimeUtc(DateTime.SpecifyKind(dateTime, DateTimeKind.Utc))
                .InZone(zone)
                .ToDateTimeUnspecified();
        }

        public static DateTime? ToLocal(this DateTime? dateTime, string timeZone)
        {
            if (!dateTime.HasValue)
            {
                return null;
            }

            var zone = DateTimeZoneProviders.Tzdb[timeZone];
            return Instant.FromDateTimeUtc(DateTime.SpecifyKind(dateTime.Value, DateTimeKind.Utc))
                .InZone(zone)
                .ToDateTimeUnspecified();
        }
    }
}