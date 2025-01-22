#if NET6_0_OR_GREATER
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Speckle.DoubleNumerics;

[JsonConverter(typeof(Vector3JsonConverter))]
public partial struct Vector3;

/// <summary>
/// Converter for <see cref="Vector3"/> to and from JSON.
/// </summary>
public class Vector3JsonConverter : JsonConverter<Vector3>
{
  public override Vector3 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    if (reader.TokenType != JsonTokenType.StartObject)
    {
      throw new JsonException();
    }

    double x = 0;
    double y = 0;
    double z = 0;

    while (reader.Read())
    {
      if (reader.TokenType == JsonTokenType.EndObject)
      {
        return new Vector3(x, y, z);
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

      if (string.Equals(propertyName, nameof(Vector3.X), comparison))
      {
        x = reader.GetDouble();
      }
      else if (string.Equals(propertyName, nameof(Vector3.Y), comparison))
      {
        y = reader.GetDouble();
      }
      else if (string.Equals(propertyName, nameof(Vector3.Z), comparison))
      {
        z = reader.GetDouble();
      }
      else
      {
        reader.Skip();
      }
    }

    throw new JsonException();
  }

  public override void Write(Utf8JsonWriter writer, Vector3 value, JsonSerializerOptions options)
  {
    writer.WriteStartObject();
    writer.WriteNumber(ConvertName(nameof(value.X), options), value.X);
    writer.WriteNumber(ConvertName(nameof(value.Y), options), value.Y);
    writer.WriteNumber(ConvertName(nameof(value.Z), options), value.Z);
    writer.WriteEndObject();
  }

  private static string ConvertName(string name, JsonSerializerOptions options) =>
    options.PropertyNamingPolicy?.ConvertName(name) ?? name;
}
#endif
