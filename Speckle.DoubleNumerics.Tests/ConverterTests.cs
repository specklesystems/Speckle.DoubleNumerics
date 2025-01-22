// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Json;
using Xunit;

namespace Speckle.DoubleNumerics.Tests;

public class ConverterTests
{
  [Fact]
  public void PlaneConverterTest() => AssertRoundTrip(new Plane(1, 2, 3, 4));

  [Fact]
  public void Vector2ConverterTest() => AssertRoundTrip(new Vector2(1, 2));

  [Fact]
  public void Vector3ConverterTest() => AssertRoundTrip(new Vector3(1, 2, 3));

  [Fact]
  public void Vector4ConverterTest() => AssertRoundTrip(new Vector4(1, 2, 3, 4));

  [Fact]
  public void QuaternionConverterTest() => AssertRoundTrip(new Quaternion(1, 2, 3, 4));

  private static void AssertRoundTrip<T>(T value)
  {
    var json = JsonSerializer.Serialize(value);
    var deserialized = JsonSerializer.Deserialize<T>(json);
    Assert.Equal(value, deserialized);
  }
}
