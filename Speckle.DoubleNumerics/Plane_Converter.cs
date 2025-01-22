#if NET6_0_OR_GREATER
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Speckle.DoubleNumerics;

[JsonConverter(typeof(PlaneJsonConverter))]
public partial struct Plane;

/// <summary>
/// Converter for <see cref="Plane"/> to and from JSON.
/// </summary>
public class PlaneJsonConverter : JsonConverter<Plane>
{
  public override Plane Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    if (reader.TokenType != JsonTokenType.StartObject)
    {
      throw new JsonException();
    }

    Vector3 normal = default;
    double d = 0;

    while (reader.Read())
    {
      if (reader.TokenType == JsonTokenType.EndObject)
      {
        return new Plane(normal, d);
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

      if (string.Equals(propertyName, nameof(Plane.Normal), comparison))
      {
        var vectorConverter = (JsonConverter<Vector3>)options.GetConverter(typeof(Vector3));
        normal = vectorConverter.Read(ref reader, typeof(Vector3), options);
      }
      else if (string.Equals(propertyName, nameof(Plane.D), comparison))
      {
        d = reader.GetDouble();
      }
      else
      {
        reader.Skip();
      }
    }

    throw new JsonException();
  }

  public override void Write(Utf8JsonWriter writer, Plane value, JsonSerializerOptions options)
  {
    writer.WriteStartObject();
    writer.WritePropertyName(ConvertName(nameof(value.Normal), options));
    var vectorConverter = (JsonConverter<Vector3>)options.GetConverter(typeof(Vector3));
    vectorConverter.Write(writer, value.Normal, options);
    writer.WriteNumber(ConvertName(nameof(value.D), options), value.D);
    writer.WriteEndObject();
  }

  private static string ConvertName(string name, JsonSerializerOptions options) =>
    options.PropertyNamingPolicy?.ConvertName(name) ?? name;
}
#endif
