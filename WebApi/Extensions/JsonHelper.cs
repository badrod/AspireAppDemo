using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Encodings.Web;
using NJsonSchema;

public static class JsonHelper
{
    /// <summary>
    /// Generate a JSON Schema definition from a POCO type.
    /// </summary>
    public static string GetSchema<T>()
    {
        var schema = JsonSchema.FromType<T>();
        return schema.ToJson();
    }

    /// <summary>
    /// Serialize any object to JSON using centralized options.
    /// </summary>
    public static string Serialize(this object o)
    {
        return JsonSerializer.Serialize(o, Options);
    }

    /// <summary>
    /// Deserialize JSON into a strongly typed object using centralized options.
    /// </summary>
    public static T? Deserialize<T>(string json, JsonSerializerOptions? options=null)
    {
        try
        {
            return JsonSerializer.Deserialize<T>(json,options?? Options);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Deserialization failed", ex);
        }
    }

    /// <summary>
    /// Centralized serializer options with snake_case support.
    /// </summary>
    public static JsonSerializerOptions Options { get; } = new JsonSerializerOptions
    {
        WriteIndented = true,
        PropertyNamingPolicy = new SnakeCaseNamingPolicy(),
        PropertyNameCaseInsensitive = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        NumberHandling = JsonNumberHandling.AllowReadingFromString
    };
}

/// <summary>
/// Custom naming policy to convert PascalCase → snake_case.
/// </summary>
public class SnakeCaseNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        if (string.IsNullOrEmpty(name)) return name;

        var sb = new StringBuilder();
        for (int i = 0; i < name.Length; i++)
        {
            var c = name[i];
            if (char.IsUpper(c))
            {
                if (i > 0 && !char.IsUpper(name[i - 1]))
                    sb.Append('_');
                sb.Append(char.ToLower(c));
            }
            else
            {
                sb.Append(c);
            }
        }
        return sb.ToString();
    }
}