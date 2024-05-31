using System.Globalization;

public static class UriHelper
{
    public static string ToQueryString(this object obj, string prefix = null)
    {
        if (obj == null)
        {
            return null;
        }

        var properties = obj.GetType().GetProperties()
            .Where(x => x.CanRead)
            .Where(x => x.GetValue(obj, null) != null)
            .ToDictionary(x => x.Name, x => x.GetValue(obj, null));

        return string.Join("&",
            properties
            .Select(kvp => ConvertPropertyToQueryString(kvp, prefix)));
    }

    private static string ConvertPropertyToQueryString(KeyValuePair<string, object> kvp, string prefix)
    {
        if (kvp.Value.GetType() == typeof(DateTime))
        {
            // Convert Datetime to ISO 8601 compatible format (YYYY-MM-DD)
            var value = !(kvp.Value is IFormattable formattable) ? kvp.Value.ToString() : formattable.ToString("o", CultureInfo.InvariantCulture);

            return $"{GetParameterNamePrefix(prefix)}{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(value)}";
        }
        else if (kvp.Value.GetType().IsValueType || kvp.Value.GetType() == typeof(string))
        {
            return $"{GetParameterNamePrefix(prefix)}{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value.ToString())}";
        }
        else
        {
            if (string.IsNullOrWhiteSpace(prefix))
            {
                return kvp.Value.ToQueryString(kvp.Key);
            }
            else
            {
                return kvp.Value.ToQueryString(prefix + "." + kvp.Key);
            }
        }
    }

    private static string GetParameterNamePrefix(string prefix)
    {
        return !string.IsNullOrWhiteSpace(prefix) ? $"{prefix}." : string.Empty;
    }
}
