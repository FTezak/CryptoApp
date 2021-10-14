using System;

namespace CryptoAPI.Extensions
{
    public static class RoundDateTime
    {
        public static DateTime Round(this DateTime value, int rountTo)
        {
            var ticksInMins = TimeSpan.FromMinutes(rountTo).Ticks;

            return (value.Ticks % ticksInMins == 0) ? new DateTime(value.Ticks + ticksInMins) : new DateTime((value.Ticks / ticksInMins + 1) * ticksInMins);
        }
    }
}
