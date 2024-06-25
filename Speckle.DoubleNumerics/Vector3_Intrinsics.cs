// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.CompilerServices;

namespace Speckle.DoubleNumerics;

// This file contains the definitions for all of the JIT intrinsic methods and properties that are recognized by the current x64 JIT compiler.
// The implementation defined here is used in any circumstance where the JIT fails to recognize these members as intrinsic.
// The JIT recognizes these methods and properties by name and signature: if either is changed, the JIT will no longer recognize the member.
// Some methods declared here are not strictly intrinsic, but delegate to an intrinsic method. For example, only one overload of CopyTo()
// is actually recognized by the JIT, but both are here for simplicity.

public partial struct Vector3
{
  /// <summary>
  /// The X component of the vector.
  /// </summary>
  public Double X;

  /// <summary>
  /// The Y component of the vector.
  /// </summary>
  public Double Y;

  /// <summary>
  /// The Z component of the vector.
  /// </summary>
  public Double Z;

  #region Constructors
  /// <summary>
  /// Constructs a vector whose elements are all the single specified value.
  /// </summary>
  /// <param name="value">The element to fill the vector with.</param>
  public Vector3(Double value)
    : this(value, value, value) { }

  /// <summary>
  /// Constructs a Vector3 from the given Vector2 and a third value.
  /// </summary>
  /// <param name="value">The Vector to extract X and Y components from.</param>
  /// <param name="z">The Z component.</param>
  public Vector3(Vector2 value, double z)
    : this(value.X, value.Y, z) { }

  /// <summary>
  /// Constructs a vector with the given individual elements.
  /// </summary>
  /// <param name="x">The X component.</param>
  /// <param name="y">The Y component.</param>
  /// <param name="z">The Z component.</param>
  public Vector3(Double x, Double y, Double z)
  {
    X = x;
    Y = y;
    Z = z;
  }
  #endregion Constructors

  #region Public Instance Methods
  /// <summary>
  /// Copies the contents of the vector into the given array.
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void CopyTo(Double[] array) => CopyTo(array, 0);

  /// <summary>
  /// Copies the contents of the vector into the given array, starting from index.
  /// </summary>
  /// <exception cref="ArgumentNullException">If array is null.</exception>
  /// <exception cref="RankException">If array is multidimensional.</exception>
  /// <exception cref="ArgumentOutOfRangeException">If index is greater than end of the array or index is less than zero.</exception>
  /// <exception cref="ArgumentException">If number of elements in source vector is greater than those available in destination array.</exception>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void CopyTo(Double[]? array, int index)
  {
    if (array == null)
    {
      // Match the JIT's exception type here. For perf, a NullReference is thrown instead of an ArgumentNull.
      throw new NullReferenceException(Strings.Arg_NullArgumentNullRef);
    }
    if (index < 0 || index >= array.Length)
    {
      throw new ArgumentOutOfRangeException(
        nameof(index),
        String.Format(Strings.Arg_ArgumentOutOfRangeException, index)
      );
    }
    if ((array.Length - index) < 3)
    {
      throw new ArgumentException(String.Format(Strings.Arg_ElementsInSourceIsGreaterThanDestination, index));
    }
    array[index] = X;
    array[index + 1] = Y;
    array[index + 2] = Z;
  }

  /// <summary>
  /// Returns a boolean indicating whether the given Vector3 is equal to this Vector3 instance.
  /// </summary>
  /// <param name="other">The Vector3 to compare this instance to.</param>
  /// <returns>True if the other Vector3 is equal to this instance; False otherwise.</returns>
  public bool Equals(Vector3 other) => X == other.X && Y == other.Y && Z == other.Z;

  #endregion Public Instance Methods

  #region Public Static Methods
  /// <summary>
  /// Returns the dot product of two vectors.
  /// </summary>
  /// <param name="vector1">The first vector.</param>
  /// <param name="vector2">The second vector.</param>
  /// <returns>The dot product.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static double Dot(Vector3 vector1, Vector3 vector2) =>
    vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z;

  /// <summary>
  /// Returns a vector whose elements are the minimum of each of the pairs of elements in the two source vectors.
  /// </summary>
  /// <param name="value1">The first source vector.</param>
  /// <param name="value2">The second source vector.</param>
  /// <returns>The minimized vector.</returns>
  public static Vector3 Min(Vector3 value1, Vector3 value2) =>
    new(
      (value1.X < value2.X) ? value1.X : value2.X,
      (value1.Y < value2.Y) ? value1.Y : value2.Y,
      (value1.Z < value2.Z) ? value1.Z : value2.Z
    );

  /// <summary>
  /// Returns a vector whose elements are the maximum of each of the pairs of elements in the two source vectors.
  /// </summary>
  /// <param name="value1">The first source vector.</param>
  /// <param name="value2">The second source vector.</param>
  /// <returns>The maximized vector.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector3 Max(Vector3 value1, Vector3 value2) =>
    new(
      (value1.X > value2.X) ? value1.X : value2.X,
      (value1.Y > value2.Y) ? value1.Y : value2.Y,
      (value1.Z > value2.Z) ? value1.Z : value2.Z
    );

  /// <summary>
  /// Returns a vector whose elements are the absolute values of each of the source vector's elements.
  /// </summary>
  /// <param name="value">The source vector.</param>
  /// <returns>The absolute value vector.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector3 Abs(Vector3 value) => new(Math.Abs(value.X), Math.Abs(value.Y), Math.Abs(value.Z));

  /// <summary>
  /// Returns a vector whose elements are the square root of each of the source vector's elements.
  /// </summary>
  /// <param name="value">The source vector.</param>
  /// <returns>The square root vector.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector3 SquareRoot(Vector3 value) => new(Math.Sqrt(value.X), Math.Sqrt(value.Y), Math.Sqrt(value.Z));

  #endregion Public Static Methods

  #region Public Static Operators
  /// <summary>
  /// Adds two vectors together.
  /// </summary>
  /// <param name="left">The first source vector.</param>
  /// <param name="right">The second source vector.</param>
  /// <returns>The summed vector.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector3 operator +(Vector3 left, Vector3 right) =>
    new(left.X + right.X, left.Y + right.Y, left.Z + right.Z);

  /// <summary>
  /// Subtracts the second vector from the first.
  /// </summary>
  /// <param name="left">The first source vector.</param>
  /// <param name="right">The second source vector.</param>
  /// <returns>The difference vector.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector3 operator -(Vector3 left, Vector3 right) =>
    new(left.X - right.X, left.Y - right.Y, left.Z - right.Z);

  /// <summary>
  /// Multiplies two vectors together.
  /// </summary>
  /// <param name="left">The first source vector.</param>
  /// <param name="right">The second source vector.</param>
  /// <returns>The product vector.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector3 operator *(Vector3 left, Vector3 right) =>
    new(left.X * right.X, left.Y * right.Y, left.Z * right.Z);

  /// <summary>
  /// Multiplies a vector by the given scalar.
  /// </summary>
  /// <param name="left">The source vector.</param>
  /// <param name="right">The scalar value.</param>
  /// <returns>The scaled vector.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector3 operator *(Vector3 left, Double right) => left * new Vector3(right);

  /// <summary>
  /// Multiplies a vector by the given scalar.
  /// </summary>
  /// <param name="left">The scalar value.</param>
  /// <param name="right">The source vector.</param>
  /// <returns>The scaled vector.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector3 operator *(Double left, Vector3 right) => new Vector3(left) * right;

  /// <summary>
  /// Divides the first vector by the second.
  /// </summary>
  /// <param name="left">The first source vector.</param>
  /// <param name="right">The second source vector.</param>
  /// <returns>The vector resulting from the division.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector3 operator /(Vector3 left, Vector3 right) =>
    new(left.X / right.X, left.Y / right.Y, left.Z / right.Z);

  /// <summary>
  /// Divides the vector by the given scalar.
  /// </summary>
  /// <param name="value1">The source vector.</param>
  /// <param name="value2">The scalar value.</param>
  /// <returns>The result of the division.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector3 operator /(Vector3 value1, double value2)
  {
    double invDiv = 1.0 / value2;

    return new Vector3(value1.X * invDiv, value1.Y * invDiv, value1.Z * invDiv);
  }

  /// <summary>
  /// Negates a given vector.
  /// </summary>
  /// <param name="value">The source vector.</param>
  /// <returns>The negated vector.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector3 operator -(Vector3 value) => Zero - value;

  /// <summary>
  /// Returns a boolean indicating whether the two given vectors are equal.
  /// </summary>
  /// <param name="left">The first vector to compare.</param>
  /// <param name="right">The second vector to compare.</param>
  /// <returns>True if the vectors are equal; False otherwise.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool operator ==(Vector3 left, Vector3 right) =>
    (left.X == right.X && left.Y == right.Y && left.Z == right.Z);

  /// <summary>
  /// Returns a boolean indicating whether the two given vectors are not equal.
  /// </summary>
  /// <param name="left">The first vector to compare.</param>
  /// <param name="right">The second vector to compare.</param>
  /// <returns>True if the vectors are not equal; False if they are equal.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool operator !=(Vector3 left, Vector3 right) =>
    (left.X != right.X || left.Y != right.Y || left.Z != right.Z);

  #endregion Public Static Operators
}
