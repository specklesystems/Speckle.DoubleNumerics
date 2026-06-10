// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if NET8_0_OR_GREATER
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

namespace Speckle.DoubleNumerics;

/// <summary>
/// Provides extension methods for converting between different numeric types.
/// </summary>
public static class ConversionExtensions
{
  /// <summary>
  /// Reinterprets a <see cref="Plane"/> as a new <see cref="Vector4"/>.
  /// </summary>
  /// <param name="value">The <see cref="Plane"/> to convert.</param>
  /// <returns>A <see cref="Vector4"/> representation of the <see cref="Plane"/>.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector4 AsVector4(this Plane value) => Unsafe.BitCast<Plane, Vector4>(value);

  /// <summary>
  /// Reinterprets a <see cref="Plane"/> as a new <see cref="Vector256{Double}"/>.
  /// </summary>
  /// <param name="value">The <see cref="Plane"/> to convert.</param>
  /// <returns>A <see cref="Vector256{Double}"/> representation of the <see cref="Plane"/>.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector256<double> AsVector256(this Plane value) => Unsafe.BitCast<Plane, Vector256<double>>(value);

  /// <summary>
  /// Reinterprets a <see cref="Quaternion"/> as a new <see cref="Vector4"/>.
  /// </summary>
  /// <param name="value">The <see cref="Quaternion"/> to convert.</param>
  /// <returns>A <see cref="Vector4"/> representation of the <see cref="Quaternion"/>.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector4 AsVector4(this Quaternion value) => Unsafe.BitCast<Quaternion, Vector4>(value);

  /// <summary>
  /// Reinterprets a <see cref="Quaternion"/> as a new <see cref="Vector256{Double}"/>.
  /// </summary>
  /// <param name="value">The <see cref="Quaternion"/> to convert.</param>
  /// <returns>A <see cref="Vector256{Double}"/> representation of the <see cref="Quaternion"/>.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector256<double> AsVector256(this Quaternion value) =>
    Unsafe.BitCast<Quaternion, Vector256<double>>(value);

  /// <summary>
  /// Reinterprets a <see cref="Vector2"/> as a new <see cref="Vector128{Double}"/>.
  /// </summary>
  /// <param name="value">The <see cref="Vector2"/> to convert.</param>
  /// <returns>A <see cref="Vector128{Double}"/> representation of the <see cref="Vector2"/>.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector128<double> AsVector128(this Vector2 value) => Unsafe.BitCast<Vector2, Vector128<double>>(value);

  /// <summary>
  /// Reinterprets a <see cref="Vector128{Double}"/> as a new <see cref="Vector2"/>.
  /// </summary>
  /// <param name="value">The <see cref="Vector128{Double}"/> to convert.</param>
  /// <returns>A <see cref="Vector2"/> representation of the <see cref="Vector128{Double}"/>.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector2 AsVector2(this Vector128<double> value) => Unsafe.BitCast<Vector128<double>, Vector2>(value);

  /// <summary>
  /// Reinterprets a <see cref="Vector2"/> as a new <see cref="Vector3"/> with the new element zeroed.
  /// </summary>
  /// <param name="value">The <see cref="Vector2"/> to convert.</param>
  /// <returns><paramref name="value"/> reinterpreted as a new <see cref="Vector3"/> with the new element zeroed.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector3 AsVector3(this Vector2 value) => new(value, 0);

  /// <summary>
  /// Reinterprets a <see cref="Vector2"/> as a new <see cref="Vector4"/> with the new elements zeroed.
  /// </summary>
  /// <param name="value">The <see cref="Vector2"/> to convert.</param>
  /// <returns><paramref name="value"/> reinterpreted as a new <see cref="Vector4"/> with the new elements zeroed.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector4 AsVector4(this Vector2 value) => new(value, 0, 0);

  /// <summary>
  /// Reinterprets a <see cref="Vector2"/> as a new <see cref="Vector256{Double}"/>.
  /// </summary>
  /// <param name="value">The <see cref="Vector2"/> to convert.</param>
  /// <returns>A <see cref="Vector256{Double}"/> representation of the <see cref="Vector2"/>.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector256<double> AsVector256(this Vector2 value) => Vector256.Create(value.X, value.Y, 0.0, 0.0);

  /// <summary>
  /// Reinterprets a <see cref="Vector3"/> as a new <see cref="Vector2"/>.
  /// </summary>
  /// <param name="value">The <see cref="Vector3"/> to convert.</param>
  /// <returns><paramref name="value"/> reinterpreted as a new <see cref="Vector2"/>.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector2 AsVector2(this Vector3 value) => value.AsVector256().AsVector2();

  /// <summary>
  /// Reinterprets a <see cref="Vector3"/> as a new <see cref="Vector4"/> with the new element zeroed.
  /// </summary>
  /// <param name="value">The <see cref="Vector3"/> to convert.</param>
  /// <returns><paramref name="value"/> reinterpreted as a new <see cref="Vector4"/> with the new element zeroed.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector4 AsVector4(this Vector3 value) => new(value, 0);

  /// <summary>
  /// Reinterprets a <see cref="Vector3"/> as a new <see cref="Vector256{Double}"/>.
  /// </summary>
  /// <param name="value">The <see cref="Vector3"/> to convert.</param>
  /// <returns>A <see cref="Vector256{Double}"/> representation of the <see cref="Vector3"/>.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector256<double> AsVector256(this Vector3 value) => Vector256.Create(value.X, value.Y, value.Z, 0.0);

  /// <summary>
  /// Reinterprets a <see cref="Vector4"/> as a new <see cref="Quaternion"/>.
  /// </summary>
  /// <param name="value">The <see cref="Vector4"/> to convert.</param>
  /// <returns>A <see cref="Quaternion"/> representation of the <see cref="Vector4"/>.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Quaternion AsQuaternion(this Vector4 value) => Unsafe.BitCast<Vector4, Quaternion>(value);

  /// <summary>
  /// Reinterprets a <see cref="Vector4"/> as a new <see cref="Plane"/>.
  /// </summary>
  /// <param name="value">The <see cref="Vector4"/> to convert.</param>
  /// <returns>A <see cref="Plane"/> representation of the <see cref="Vector4"/>.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Plane AsPlane(this Vector4 value) => Unsafe.BitCast<Vector4, Plane>(value);

  /// <summary>
  /// Reinterprets a <see cref="Vector4"/> as a new <see cref="Vector2"/>.
  /// </summary>
  /// <param name="value">The <see cref="Vector4"/> to convert.</param>
  /// <returns>A <see cref="Vector2"/> representation of the <see cref="Vector4"/>.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector2 AsVector2(this Vector4 value) => value.AsVector256().AsVector2();

  /// <summary>
  /// Reinterprets a <see cref="Vector4"/> as a new <see cref="Vector3"/>.
  /// </summary>
  /// <param name="value">The <see cref="Vector4"/> to convert.</param>
  /// <returns>A <see cref="Vector3"/> representation of the <see cref="Vector4"/>.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector3 AsVector3(this Vector4 value) => value.AsVector256().AsVector3();

  /// <summary>
  /// Reinterprets a <see cref="Vector4"/> as a new <see cref="Vector256{Double}"/>.
  /// </summary>
  /// <param name="value">The <see cref="Vector4"/> to convert.</param>
  /// <returns>A <see cref="Vector256{Double}"/> representation of the <see cref="Vector4"/>.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector256<double> AsVector256(this Vector4 value) => Unsafe.BitCast<Vector4, Vector256<double>>(value);

  /// <summary>
  /// Reinterprets a <see cref="Vector256{Double}"/> as a new <see cref="Plane"/>.
  /// </summary>
  /// <param name="value">The <see cref="Vector256{Double}"/> to convert.</param>
  /// <returns>A <see cref="Plane"/> representation of the <see cref="Vector256{Double}"/>.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Plane AsPlane(this Vector256<double> value) => Unsafe.BitCast<Vector256<double>, Plane>(value);

  /// <summary>
  /// Reinterprets a <see cref="Vector256{Double}"/> as a new <see cref="Quaternion"/>.
  /// </summary>
  /// <param name="value">The <see cref="Vector256{Double}"/> to convert.</param>
  /// <returns>A <see cref="Quaternion"/> representation of the <see cref="Vector256{Double}"/>.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Quaternion AsQuaternion(this Vector256<double> value) =>
    Unsafe.BitCast<Vector256<double>, Quaternion>(value);

  /// <summary>
  /// Reinterprets a <see cref="Vector256{Double}"/> as a new <see cref="Vector2"/>.
  /// </summary>
  /// <param name="value">The <see cref="Vector256{Double}"/> to convert.</param>
  /// <returns>A <see cref="Vector2"/> representation of the <see cref="Vector256{Double}"/>.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector3 AsVector3(this Vector256<double> value) =>
    new(value.GetElement(0), value.GetElement(1), value.GetElement(2));

  /// <summary>
  /// Reinterprets a <see cref="Vector256{Double}"/> as a new <see cref="Vector4"/>.
  /// </summary>
  /// <param name="value">The <see cref="Vector256{Double}"/> to convert.</param>
  /// <returns>A <see cref="Vector4"/> representation of the <see cref="Vector256{Double}"/>.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector4 AsVector4(this Vector256<double> value) => Unsafe.BitCast<Vector256<double>, Vector4>(value);
}
#endif
