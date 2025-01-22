// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if NET6_0_OR_GREATER
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Speckle.DoubleNumerics;

[JsonConverter(typeof(QuaternionJsonConverter))]
public partial struct Quaternion;

/// <summary>
/// Converter for <see cref="Quaternion"/> to and from JSON.
/// </summary>
public class QuaternionJsonConverter : JsonConverter<Quaternion>
{
  public override Quaternion Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    if (reader.TokenType != JsonTokenType.StartObject)
    {
      throw new JsonException();
    }

    double x = 0;
    double y = 0;
    double z = 0;
    double w = 0;

    while (reader.Read())
    {
      if (reader.TokenType == JsonTokenType.EndObject)
      {
        return new Quaternion(x, y, z, w);
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

      if (string.Equals(propertyName, nameof(Quaternion.X), comparison))
      {
        x = reader.GetDouble();
      }
      else if (string.Equals(propertyName, nameof(Quaternion.Y), comparison))
      {
        y = reader.GetDouble();
      }
      else if (string.Equals(propertyName, nameof(Quaternion.Z), comparison))
      {
        z = reader.GetDouble();
      }
      else if (string.Equals(propertyName, nameof(Quaternion.W), comparison))
      {
        w = reader.GetDouble();
      }
      else
      {
        reader.Skip();
      }
    }

    throw new JsonException();
  }

  public override void Write(Utf8JsonWriter writer, Quaternion value, JsonSerializerOptions options)
  {
    writer.WriteStartObject();
    writer.WriteNumber(ConvertName(nameof(value.X), options), value.X);
    writer.WriteNumber(ConvertName(nameof(value.Y), options), value.Y);
    writer.WriteNumber(ConvertName(nameof(value.Z), options), value.Z);
    writer.WriteNumber(ConvertName(nameof(value.W), options), value.W);
    writer.WriteEndObject();
  }

  private static string ConvertName(string name, JsonSerializerOptions options) =>
    options.PropertyNamingPolicy?.ConvertName(name) ?? name;
}
#endif
