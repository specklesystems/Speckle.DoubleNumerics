// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
#if NET8_0_OR_GREATER
using System.Runtime.Intrinsics;
#endif

namespace Speckle.DoubleNumerics;

/// <summary>
/// A structure encapsulating three single precision floating point values and provides hardware accelerated methods.
/// </summary>
public partial struct Vector3 : IEquatable<Vector3>, IFormattable
{
  #region Public Static Properties
  /// <summary>
  /// Returns the vector (0,0,0).
  /// </summary>
  public static Vector3 Zero => new();

  /// <summary>
  /// Returns the vector (1,1,1).
  /// </summary>
  public static Vector3 One => new(1.0, 1.0, 1.0);

  /// <summary>
  /// Returns the vector (1,0,0).
  /// </summary>
  public static Vector3 UnitX => new(1.0, 0.0, 0.0);

  /// <summary>
  /// Returns the vector (0,1,0).
  /// </summary>
  public static Vector3 UnitY => new(0.0, 1.0, 0.0);

  /// <summary>
  /// Returns the vector (0,0,1).
  /// </summary>
  public static Vector3 UnitZ => new(0.0, 0.0, 1.0);

  #endregion Public Static Properties

  #region Public Instance Methods

  /// <summary>
  /// Returns the hash code for this instance.
  /// </summary>
  /// <returns>The hash code.</returns>
  public override int GetHashCode()
  {
    int hash = X.GetHashCode();
    hash = HashHelpers.Combine(hash, Y.GetHashCode());
    hash = HashHelpers.Combine(hash, Z.GetHashCode());
    return hash;
  }

  /// <summary>
  /// Returns a boolean indicating whether the given Object is equal to this Vector3 instance.
  /// </summary>
  /// <param name="obj">The Object to compare against.</param>
  /// <returns>True if the Object is equal to this Vector3; False otherwise.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public override bool Equals(object? obj)
  {
    if (!(obj is Vector3 vector3))
      return false;
    return Equals(vector3);
  }

  /// <summary>
  /// Returns a String representing this Vector3 instance.
  /// </summary>
  /// <returns>The string representation.</returns>
  public override string ToString() => ToString("G", CultureInfo.CurrentCulture);

  /// <summary>
  /// Returns a String representing this Vector3 instance, using the specified format to format individual elements.
  /// </summary>
  /// <param name="format">The format of individual elements.</param>
  /// <returns>The string representation.</returns>
  public string ToString(string format) => ToString(format, CultureInfo.CurrentCulture);

  /// <summary>
  /// Returns a String representing this Vector3 instance, using the specified format to format individual elements
  /// and the given IFormatProvider.
  /// </summary>
  /// <param name="format">The format of individual elements.</param>
  /// <param name="formatProvider">The format provider to use when formatting elements.</param>
  /// <returns>The string representation.</returns>
  public string ToString(string? format, IFormatProvider? formatProvider)
  {
    StringBuilder sb = new();
    string separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
    sb.Append('<');
    sb.Append(X.ToString(format, formatProvider));
    sb.Append(separator);
    sb.Append(' ');
    sb.Append(Y.ToString(format, formatProvider));
    sb.Append(separator);
    sb.Append(' ');
    sb.Append(Z.ToString(format, formatProvider));
    sb.Append('>');
    return sb.ToString();
  }

  /// <summary>
  /// Returns the length of the vector.
  /// </summary>
  /// <returns>The vector's length.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public double Length() => Math.Sqrt(LengthSquared());

  /// <summary>
  /// Returns the length of the vector squared. This operation is cheaper than Length().
  /// </summary>
  /// <returns>The vector's length squared.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public double LengthSquared() => Dot(this, this);

  #endregion Public Instance Methods

  #region Public Static Methods
  /// <summary>
  /// Returns the Euclidean distance between the two given points.
  /// </summary>
  /// <param name="value1">The first point.</param>
  /// <param name="value2">The second point.</param>
  /// <returns>The distance.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static double Distance(Vector3 value1, Vector3 value2) => Math.Sqrt(DistanceSquared(value1, value2));

  /// <summary>
  /// Returns the Euclidean distance squared between the two given points.
  /// </summary>
  /// <param name="value1">The first point.</param>
  /// <param name="value2">The second point.</param>
  /// <returns>The distance squared.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static double DistanceSquared(Vector3 value1, Vector3 value2) => (value1 - value2).LengthSquared();

  /// <summary>
  /// Returns a vector with the same direction as the given vector, but with a length of 1.
  /// </summary>
  /// <param name="value">The vector to normalize.</param>
  /// <returns>The normalized vector.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector3 Normalize(Vector3 value)
  {
#if NET8_0_OR_GREATER
    return (value.AsVector256() / value.Length()).AsVector3();
#else
    double ls = value.X * value.X + value.Y * value.Y + value.Z * value.Z;
    double length = Math.Sqrt(ls);
    return new Vector3(value.X / length, value.Y / length, value.Z / length);
#endif
  }

  /// <summary>
  /// Computes the cross product of two vectors.
  /// </summary>
  /// <param name="vector1">The first vector.</param>
  /// <param name="vector2">The second vector.</param>
  /// <returns>The cross product.</returns>
  // Deliberately scalar: the cross-lane shuffles (vpermpd) plus Vector3 packing cost more
  // than the six multiplies and three subtractions they replace.
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector3 Cross(Vector3 vector1, Vector3 vector2) =>
    new(
      vector1.Y * vector2.Z - vector1.Z * vector2.Y,
      vector1.Z * vector2.X - vector1.X * vector2.Z,
      vector1.X * vector2.Y - vector1.Y * vector2.X
    );

  /// <summary>
  /// Returns the reflection of a vector off a surface that has the specified normal.
  /// </summary>
  /// <param name="vector">The source vector.</param>
  /// <param name="normal">The normal of the surface being reflected off.</param>
  /// <returns>The reflected vector.</returns>
  // Deliberately scalar: see Cross. Benchmarks showed the SIMD version 2x slower.
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector3 Reflect(Vector3 vector, Vector3 normal)
  {
    double dot = vector.X * normal.X + vector.Y * normal.Y + vector.Z * normal.Z;
    double tempX = normal.X * dot * 2;
    double tempY = normal.Y * dot * 2;
    double tempZ = normal.Z * dot * 2;
    return new Vector3(vector.X - tempX, vector.Y - tempY, vector.Z - tempZ);
  }

  /// <summary>
  /// Restricts a vector between a min and max value.
  /// </summary>
  /// <param name="value1">The source vector.</param>
  /// <param name="min">The minimum value.</param>
  /// <param name="max">The maximum value.</param>
  /// <returns>The restricted vector.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector3 Clamp(Vector3 value1, Vector3 min, Vector3 max)
  {
    // This compare order is very important!!!
    // We must follow HLSL behavior in the case user specified min value is bigger than max value.

#if NET8_0_OR_GREATER
    Vector256<double> result = value1.AsVector256();
    result = Vector256.ConditionalSelect(Vector256.GreaterThan(result, max.AsVector256()), max.AsVector256(), result);
    result = Vector256.ConditionalSelect(Vector256.LessThan(result, min.AsVector256()), min.AsVector256(), result);
    return result.AsVector3();
#else
    double x = value1.X;
    x = (x > max.X) ? max.X : x;
    x = (x < min.X) ? min.X : x;

    double y = value1.Y;
    y = (y > max.Y) ? max.Y : y;
    y = (y < min.Y) ? min.Y : y;

    double z = value1.Z;
    z = (z > max.Z) ? max.Z : z;
    z = (z < min.Z) ? min.Z : z;

    return new Vector3(x, y, z);
#endif
  }

  /// <summary>
  /// Linearly interpolates between two vectors based on the given weighting.
  /// </summary>
  /// <param name="value1">The first source vector.</param>
  /// <param name="value2">The second source vector.</param>
  /// <param name="amount">Value between 0 and 1 indicating the weight of the second source vector.</param>
  /// <returns>The interpolated vector.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector3 Lerp(Vector3 value1, Vector3 value2, double amount) => value1 + (value2 - value1) * amount;

  /// <summary>
  /// Transforms a vector by the given matrix.
  /// </summary>
  /// <param name="position">The source vector.</param>
  /// <param name="matrix">The transformation matrix.</param>
  /// <returns>The transformed vector.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector3 Transform(Vector3 position, Matrix4x4 matrix)
  {
#if NET8_0_OR_GREATER
    Vector256<double> result = Vector256.Create(position.X) * Vector256.LoadUnsafe(ref matrix.M11);
    result += Vector256.Create(position.Y) * Vector256.LoadUnsafe(ref matrix.M21);
    result += Vector256.Create(position.Z) * Vector256.LoadUnsafe(ref matrix.M31);
    result += Vector256.LoadUnsafe(ref matrix.M41);
    return result.AsVector3();
#else
    return new(
      position.X * matrix.M11 + position.Y * matrix.M21 + position.Z * matrix.M31 + matrix.M41,
      position.X * matrix.M12 + position.Y * matrix.M22 + position.Z * matrix.M32 + matrix.M42,
      position.X * matrix.M13 + position.Y * matrix.M23 + position.Z * matrix.M33 + matrix.M43
    );
#endif
  }

  /// <summary>
  /// Transforms a vector normal by the given matrix.
  /// </summary>
  /// <param name="normal">The source vector.</param>
  /// <param name="matrix">The transformation matrix.</param>
  /// <returns>The transformed vector.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector3 TransformNormal(Vector3 normal, Matrix4x4 matrix)
  {
#if NET8_0_OR_GREATER
    Vector256<double> result = Vector256.Create(normal.X) * Vector256.LoadUnsafe(ref matrix.M11);
    result += Vector256.Create(normal.Y) * Vector256.LoadUnsafe(ref matrix.M21);
    result += Vector256.Create(normal.Z) * Vector256.LoadUnsafe(ref matrix.M31);
    return result.AsVector3();
#else
    return new(
      normal.X * matrix.M11 + normal.Y * matrix.M21 + normal.Z * matrix.M31,
      normal.X * matrix.M12 + normal.Y * matrix.M22 + normal.Z * matrix.M32,
      normal.X * matrix.M13 + normal.Y * matrix.M23 + normal.Z * matrix.M33
    );
#endif
  }

  /// <summary>
  /// Transforms a vector by the given Quaternion rotation value.
  /// </summary>
  /// <param name="value">The source vector to be rotated.</param>
  /// <param name="rotation">The rotation to apply.</param>
  /// <returns>The transformed vector.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector3 Transform(Vector3 value, Quaternion rotation)
  {
    double x2 = rotation.X + rotation.X;
    double y2 = rotation.Y + rotation.Y;
    double z2 = rotation.Z + rotation.Z;

    double wx2 = rotation.W * x2;
    double wy2 = rotation.W * y2;
    double wz2 = rotation.W * z2;
    double xx2 = rotation.X * x2;
    double xy2 = rotation.X * y2;
    double xz2 = rotation.X * z2;
    double yy2 = rotation.Y * y2;
    double yz2 = rotation.Y * z2;
    double zz2 = rotation.Z * z2;

    return new Vector3(
      value.X * (1.0 - yy2 - zz2) + value.Y * (xy2 - wz2) + value.Z * (xz2 + wy2),
      value.X * (xy2 + wz2) + value.Y * (1.0 - xx2 - zz2) + value.Z * (yz2 - wx2),
      value.X * (xz2 - wy2) + value.Y * (yz2 + wx2) + value.Z * (1.0 - xx2 - yy2)
    );
  }
  #endregion Public Static Methods

  #region Public operator methods

  // All these methods should be inlined as they are implemented
  // over JIT intrinsics

  /// <summary>
  /// Adds two vectors together.
  /// </summary>
  /// <param name="left">The first source vector.</param>
  /// <param name="right">The second source vector.</param>
  /// <returns>The summed vector.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector3 Add(Vector3 left, Vector3 right) => left + right;

  /// <summary>
  /// Subtracts the second vector from the first.
  /// </summary>
  /// <param name="left">The first source vector.</param>
  /// <param name="right">The second source vector.</param>
  /// <returns>The difference vector.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector3 Subtract(Vector3 left, Vector3 right) => left - right;

  /// <summary>
  /// Multiplies two vectors together.
  /// </summary>
  /// <param name="left">The first source vector.</param>
  /// <param name="right">The second source vector.</param>
  /// <returns>The product vector.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector3 Multiply(Vector3 left, Vector3 right) => left * right;

  /// <summary>
  /// Multiplies a vector by the given scalar.
  /// </summary>
  /// <param name="left">The source vector.</param>
  /// <param name="right">The scalar value.</param>
  /// <returns>The scaled vector.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector3 Multiply(Vector3 left, Double right) => left * right;

  /// <summary>
  /// Multiplies a vector by the given scalar.
  /// </summary>
  /// <param name="left">The scalar value.</param>
  /// <param name="right">The source vector.</param>
  /// <returns>The scaled vector.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector3 Multiply(Double left, Vector3 right) => left * right;

  /// <summary>
  /// Divides the first vector by the second.
  /// </summary>
  /// <param name="left">The first source vector.</param>
  /// <param name="right">The second source vector.</param>
  /// <returns>The vector resulting from the division.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector3 Divide(Vector3 left, Vector3 right) => left / right;

  /// <summary>
  /// Divides the vector by the given scalar.
  /// </summary>
  /// <param name="left">The source vector.</param>
  /// <param name="divisor">The scalar value.</param>
  /// <returns>The result of the division.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector3 Divide(Vector3 left, Double divisor) => left / divisor;

  /// <summary>
  /// Negates a given vector.
  /// </summary>
  /// <param name="value">The source vector.</param>
  /// <returns>The negated vector.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector3 Negate(Vector3 value) => -value;

  #endregion Public operator methods
}
