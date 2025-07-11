namespace OPMLCore.NET;

using System;
using System.Globalization;

public static class CommonUtils
{
    public static readonly CultureInfo MyCultureInfo = new("en-US");


    public static DateTime? ParseDateTime(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return null;
        if (DateTime.TryParse(value, MyCultureInfo, DateTimeStyles.None, out var result))
            return result;
        return null;
    }
}
