// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if NET6_0_OR_GREATER
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

namespace Speckle.DoubleNumerics;

/// <summary>
/// Provides extension methods for converting between different numeric types.
/// </summary>
public static class ConversionExtensions
{
  // TODO: When targeting .NET 8 and above, we can use the new Unsafe.BitCast method to reinterpret types.

  /// <summary>
  /// Reinterprets a <see cref="Plane"/> as a new <see cref="Vector4"/>.
  /// </summary>
  /// <param name="value">The <see cref="Plane"/> to convert.</param>
  /// <returns>A <see cref="Vector4"/> representation of the <see cref="Plane"/>.</returns>
  public static Vector4 AsVector4(this Plane value) => Unsafe.As<Plane, Vector4>(ref value);

  /// <summary>
  /// Reinterprets a <see cref="Plane"/> as a new <see cref="Vector256{Double}"/>.
  /// </summary>
  /// <param name="value">The <see cref="Plane"/> to convert.</param>
  /// <returns>A <see cref="Vector256{Double}"/> representation of the <see cref="Plane"/>.</returns>
  public static Vector256<double> AsVector256(this Plane value) => Unsafe.As<Plane, Vector256<double>>(ref value);

  /// <summary>
  /// Reinterprets a <see cref="Quaternion"/> as a new <see cref="Vector4"/>.
  /// </summary>
  /// <param name="value">The <see cref="Quaternion"/> to convert.</param>
  /// <returns>A <see cref="Vector4"/> representation of the <see cref="Quaternion"/>.</returns>
  public static Vector4 AsVector4(this Quaternion value) => Unsafe.As<Quaternion, Vector4>(ref value);

  /// <summary>
  /// Reinterprets a <see cref="Quaternion"/> as a new <see cref="Vector256{Double}"/>.
  /// </summary>
  /// <param name="value">The <see cref="Quaternion"/> to convert.</param>
  /// <returns>A <see cref="Vector256{Double}"/> representation of the <see cref="Quaternion"/>.</returns>
  public static Vector256<double> AsVector256(this Quaternion value) => Unsafe.As<Quaternion, Vector256<double>>(ref value);

  /// <summary>
  /// Reinterprets a <see cref="Vector2"/> as a new <see cref="Vector3"/> with the new element zeroed.
  /// </summary>
  /// <param name="value">The <see cref="Vector2"/> to convert.</param>
  /// <returns><paramref name="value"/> reinterpreted as a new <see cref="Vector3"/> with the new element zeroed.</returns>
  public static Vector3 AsVector3(this Vector2 value) => new(value, 0);

  /// <summary>
  /// Reinterprets a <see cref="Vector2"/> as a new <see cref="Vector4"/> with the new elements zeroed.
  /// </summary>
  /// <param name="value">The <see cref="Vector2"/> to convert.</param>
  /// <returns><paramref name="value"/> reinterpreted as a new <see cref="Vector4"/> with the new elements zeroed.</returns>
  public static Vector4 AsVector4(this Vector2 value) => new(value, 0, 0);

  /// <summary>
  /// Reinterprets a <see cref="Vector2"/> as a new <see cref="Vector256{Double}"/>.
  /// </summary>
  /// <param name="value">The <see cref="Vector2"/> to convert.</param>
  /// <returns>A <see cref="Vector256{Double}"/> representation of the <see cref="Vector2"/>.</returns>
  public static Vector256<double> AsVector256(this Vector2 value) => value.AsVector4().AsVector256();

  /// <summary>
  /// Reinterprets a <see cref="Vector3"/> as a new <see cref="Vector2"/>.
  /// </summary>
  /// <param name="value">The <see cref="Vector3"/> to convert.</param>
  /// <returns><paramref name="value"/> reinterpreted as a new <see cref="Vector2"/>.</returns>
  public static Vector2 AsVector2(this Vector3 value) => value.AsVector256().AsVector2();

  /// <summary>
  /// Reinterprets a <see cref="Vector3"/> as a new <see cref="Vector4"/> with the new element zeroed.
  /// </summary>
  /// <param name="value">The <see cref="Vector3"/> to convert.</param>
  /// <returns><paramref name="value"/> reinterpreted as a new <see cref="Vector4"/> with the new element zeroed.</returns>
  public static Vector4 AsVector4(this Vector3 value) => new(value, 0);

  /// <summary>
  /// Reinterprets a <see cref="Vector3"/> as a new <see cref="Vector256{Double}"/>.
  /// </summary>
  /// <param name="value">The <see cref="Vector3"/> to convert.</param>
  /// <returns>A <see cref="Vector256{Double}"/> representation of the <see cref="Vector3"/>.</returns>
  public static Vector256<double> AsVector256(this Vector3 value) => value.AsVector4().AsVector256();

  /// <summary>
  /// Reinterprets a <see cref="Vector4"/> as a new <see cref="Quaternion"/>.
  /// </summary>
  /// <param name="value">The <see cref="Vector4"/> to convert.</param>
  /// <returns>A <see cref="Quaternion"/> representation of the <see cref="Vector4"/>.</returns>
  public static Quaternion AsQuaternion(this Vector4 value) => Unsafe.As<Vector4, Quaternion>(ref value);

  /// <summary>
  /// Reinterprets a <see cref="Vector4"/> as a new <see cref="Plane"/>.
  /// </summary>
  /// <param name="value">The <see cref="Vector4"/> to convert.</param>
  /// <returns>A <see cref="Plane"/> representation of the <see cref="Vector4"/>.</returns>
  public static Plane AsPlane(this Vector4 value) => Unsafe.As<Vector4, Plane>(ref value);

  /// <summary>
  /// Reinterprets a <see cref="Vector4"/> as a new <see cref="Vector2"/>.
  /// </summary>
  /// <param name="value">The <see cref="Vector4"/> to convert.</param>
  /// <returns>A <see cref="Vector2"/> representation of the <see cref="Vector4"/>.</returns>
  public static Vector2 AsVector2(this Vector4 value) => value.AsVector256().AsVector2();

  /// <summary>
  /// Reinterprets a <see cref="Vector4"/> as a new <see cref="Vector3"/>.
  /// </summary>
  /// <param name="value">The <see cref="Vector4"/> to convert.</param>
  /// <returns>A <see cref="Vector3"/> representation of the <see cref="Vector4"/>.</returns>
  public static Vector3 AsVector3(this Vector4 value) => value.AsVector256().AsVector3();

  /// <summary>
  /// Reinterprets a <see cref="Vector4"/> as a new <see cref="Vector256{Double}"/>.
  /// </summary>
  /// <param name="value">The <see cref="Vector4"/> to convert.</param>
  /// <returns>A <see cref="Vector256{Double}"/> representation of the <see cref="Vector4"/>.</returns>
  public static Vector256<double> AsVector256(this Vector4 value) => Unsafe.As<Vector4, Vector256<double>>(ref value);

  /// <summary>
  /// Reinterprets a <see cref="Vector256{Double}"/> as a new <see cref="Plane"/>.
  /// </summary>
  /// <param name="value">The <see cref="Vector256{Double}"/> to convert.</param>
  /// <returns>A <see cref="Plane"/> representation of the <see cref="Vector256{Double}"/>.</returns>
  public static Plane AsPlane(this Vector256<double> value) => Unsafe.As<Vector256<double>, Plane>(ref value);

  /// <summary>
  /// Reinterprets a <see cref="Vector256{Double}"/> as a new <see cref="Quaternion"/>.
  /// </summary>
  /// <param name="value">The <see cref="Vector256{Double}"/> to convert.</param>
  /// <returns>A <see cref="Quaternion"/> representation of the <see cref="Vector256{Double}"/>.</returns>
  public static Quaternion AsQuaternion(this Vector256<double> value) => Unsafe.As<Vector256<double>, Quaternion>(ref value);

  /// <summary>
  /// Reinterprets a <see cref="Vector256{Double}"/> as a new <see cref="Vector2"/>.
  /// </summary>
  /// <param name="value">The <see cref="Vector256{Double}"/> to convert.</param>
  /// <returns>A <see cref="Vector2"/> representation of the <see cref="Vector256{Double}"/>.</returns>
  public static Vector2 AsVector2(this Vector256<double> value)
  {
    ref byte address = ref Unsafe.As<Vector256<double>, byte>(ref value);
    return Unsafe.ReadUnaligned<Vector2>(ref address);
  }

  /// <summary>
  /// Reinterprets a <see cref="Vector256{Double}"/> as a new <see cref="Vector3"/>.
  /// </summary>
  /// <param name="value">The <see cref="Vector256{Double}"/> to convert.</param>
  /// <returns>A <see cref="Vector3"/> representation of the <see cref="Vector256{Double}"/>.</returns>
  public static Vector3 AsVector3(this Vector256<double> value)
  {
    ref byte address = ref Unsafe.As<Vector256<double>, byte>(ref value);
    return Unsafe.ReadUnaligned<Vector3>(ref address);
  }

  /// <summary>
  /// Reinterprets a <see cref="Vector256{Double}"/> as a new <see cref="Vector4"/>.
  /// </summary>
  /// <param name="value">The <see cref="Vector256{Double}"/> to convert.</param>
  /// <returns>A <see cref="Vector4"/> representation of the <see cref="Vector256{Double}"/>.</returns>
  public static Vector4 AsVector4(this Vector256<double> value) => Unsafe.As<Vector256<double>, Vector4>(ref value);
}
#endif
