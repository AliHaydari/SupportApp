using System;
using DNTPersianUtils.Core;

namespace SupportApp.Common.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTimeOffset GetDateTimeOffset(int year, int month, int day)
        {
            var date = $"{year}/{month:00}/{day:00}";
            return date.ToGregorianDateTimeOffset().GetValueOrDefault();
        }

        public static DateTime GetDateTime(int year, int month, int day)
        {
            var date = $"{year}/{month:00}/{day:00}";
            return date.ToGregorianDateTime().GetValueOrDefault();
        }
    }
}
