#if NET6_0_OR_GREATER
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Speckle.DoubleNumerics;

[JsonConverter(typeof(Vector2JsonConverter))]
public partial struct Vector2;

/// <summary>
/// Converter for <see cref="Vector2"/> to and from JSON.
/// </summary>
public class Vector2JsonConverter : JsonConverter<Vector2>
{
  public override Vector2 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    if (reader.TokenType != JsonTokenType.StartObject)
    {
      throw new JsonException();
    }

    double x = 0;
    double y = 0;

    while (reader.Read())
    {
      if (reader.TokenType == JsonTokenType.EndObject)
      {
        return new Vector2(x, y);
      }

      if (reader.TokenType != JsonTokenType.PropertyName)
      {
        throw new JsonException();
      }

      var propertyName = reader.GetString();

      reader.Read();

      var comparison = options.PropertyNameCaseInsensitive
        ? StringComparison.OrdinalIgnoreCase
        : StringComparison.Ordinal;

      if (string.Equals(propertyName, nameof(Vector2.X), comparison))
      {
        x = reader.GetDouble();
      }
      else if (string.Equals(propertyName, nameof(Vector2.Y), comparison))
      {
        y = reader.GetDouble();
      }
      else
      {
        reader.Skip();
      }
    }

    throw new JsonException();
  }

  public override void Write(Utf8JsonWriter writer, Vector2 value, JsonSerializerOptions options)
  {
    writer.WriteStartObject();
    writer.WriteNumber(ConvertName(nameof(value.X), options), value.X);
    writer.WriteNumber(ConvertName(nameof(value.Y), options), value.Y);
    writer.WriteEndObject();
  }

  private static string ConvertName(string name, JsonSerializerOptions options) =>
    options.PropertyNamingPolicy?.ConvertName(name) ?? name;
}
#endif
