// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Globalization;

namespace Speckle.DoubleNumerics;

/// <summary>
/// A structure encapsulating a four-dimensional vector (x,y,z,w),
/// which is used to efficiently rotate an object about the (x,y,z) vector by the angle theta, where w = cos(theta/2).
/// </summary>
public partial struct Quaternion : IEquatable<Quaternion>
{
  /// <summary>
  /// Specifies the X-value of the vector component of the Quaternion.
  /// </summary>
  public double X;

  /// <summary>
  /// Specifies the Y-value of the vector component of the Quaternion.
  /// </summary>
  public double Y;

  /// <summary>
  /// Specifies the Z-value of the vector component of the Quaternion.
  /// </summary>
  public double Z;

  /// <summary>
  /// Specifies the rotation component of the Quaternion.
  /// </summary>
  public double W;

  /// <summary>
  /// Returns a Quaternion representing no rotation.
  /// </summary>
  public static Quaternion Identity => new(0, 0, 0, 1);

  /// <summary>
  /// Returns whether the Quaternion is the identity Quaternion.
  /// </summary>
  public bool IsIdentity => X == 0 && Y == 0 && Z == 0 && W == 1;

  /// <summary>
  /// Constructs a Quaternion from the given components.
  /// </summary>
  /// <param name="x">The X component of the Quaternion.</param>
  /// <param name="y">The Y component of the Quaternion.</param>
  /// <param name="z">The Z component of the Quaternion.</param>
  /// <param name="w">The W component of the Quaternion.</param>
  public Quaternion(double x, double y, double z, double w)
  {
    X = x;
    Y = y;
    Z = z;
    W = w;
  }

  /// <summary>
  /// Constructs a Quaternion from the given vector and rotation parts.
  /// </summary>
  /// <param name="vectorPart">The vector part of the Quaternion.</param>
  /// <param name="scalarPart">The rotation part of the Quaternion.</param>
  public Quaternion(Vector3 vectorPart, double scalarPart)
  {
    X = vectorPart.X;
    Y = vectorPart.Y;
    Z = vectorPart.Z;
    W = scalarPart;
  }

  /// <summary>
  /// Calculates the length of the Quaternion.
  /// </summary>
  /// <returns>The computed length of the Quaternion.</returns>
  public double Length()
  {
    double ls = X * X + Y * Y + Z * Z + W * W;

    return Math.Sqrt(ls);
  }

  /// <summary>
  /// Calculates the length squared of the Quaternion. This operation is cheaper than Length().
  /// </summary>
  /// <returns>The length squared of the Quaternion.</returns>
  public double LengthSquared() => X * X + Y * Y + Z * Z + W * W;

  /// <summary>
  /// Divides each component of the Quaternion by the length of the Quaternion.
  /// </summary>
  /// <param name="value">The source Quaternion.</param>
  /// <returns>The normalized Quaternion.</returns>
  public static Quaternion Normalize(Quaternion value)
  {
    Quaternion ans;

    double ls = value.X * value.X + value.Y * value.Y + value.Z * value.Z + value.W * value.W;

    double invNorm = 1.0 / Math.Sqrt(ls);

    ans.X = value.X * invNorm;
    ans.Y = value.Y * invNorm;
    ans.Z = value.Z * invNorm;
    ans.W = value.W * invNorm;

    return ans;
  }

  /// <summary>
  /// Creates the conjugate of a specified Quaternion.
  /// </summary>
  /// <param name="value">The Quaternion of which to return the conjugate.</param>
  /// <returns>A new Quaternion that is the conjugate of the specified one.</returns>
  public static Quaternion Conjugate(Quaternion value)
  {
    Quaternion ans;

    ans.X = -value.X;
    ans.Y = -value.Y;
    ans.Z = -value.Z;
    ans.W = value.W;

    return ans;
  }

  /// <summary>
  /// Returns the inverse of a Quaternion.
  /// </summary>
  /// <param name="value">The source Quaternion.</param>
  /// <returns>The inverted Quaternion.</returns>
  public static Quaternion Inverse(Quaternion value)
  {
    //  -1   (       a              -v       )
    // q   = ( -------------   ------------- )
    //       (  a^2 + |v|^2  ,  a^2 + |v|^2  )

    Quaternion ans;

    double ls = value.X * value.X + value.Y * value.Y + value.Z * value.Z + value.W * value.W;
    double invNorm = 1.0 / ls;

    ans.X = -value.X * invNorm;
    ans.Y = -value.Y * invNorm;
    ans.Z = -value.Z * invNorm;
    ans.W = value.W * invNorm;

    return ans;
  }

  /// <summary>
  /// Creates a Quaternion from a normalized vector axis and an angle to rotate about the vector.
  /// </summary>
  /// <param name="axis">The unit vector to rotate around.
  /// This vector must be normalized before calling this function or the resulting Quaternion will be incorrect.</param>
  /// <param name="angle">The angle, in radians, to rotate around the vector.</param>
  /// <returns>The created Quaternion.</returns>
  public static Quaternion CreateFromAxisAngle(Vector3 axis, double angle)
  {
    Quaternion ans;

    double halfAngle = angle * 0.5;
    double s = Math.Sin(halfAngle);
    double c = Math.Cos(halfAngle);

    ans.X = axis.X * s;
    ans.Y = axis.Y * s;
    ans.Z = axis.Z * s;
    ans.W = c;

    return ans;
  }

  /// <summary>
  /// Creates a new Quaternion from the given yaw, pitch, and roll, in radians.
  /// </summary>
  /// <param name="yaw">The yaw angle, in radians, around the Y-axis.</param>
  /// <param name="pitch">The pitch angle, in radians, around the X-axis.</param>
  /// <param name="roll">The roll angle, in radians, around the Z-axis.</param>
  /// <returns></returns>
  public static Quaternion CreateFromYawPitchRoll(double yaw, double pitch, double roll)
  {
    //  Roll first, about axis the object is facing, then
    //  pitch upward, then yaw to face into the new heading
    double sr,
      cr,
      sp,
      cp,
      sy,
      cy;

    double halfRoll = roll * 0.5;
    sr = Math.Sin(halfRoll);
    cr = Math.Cos(halfRoll);

    double halfPitch = pitch * 0.5;
    sp = Math.Sin(halfPitch);
    cp = Math.Cos(halfPitch);

    double halfYaw = yaw * 0.5;
    sy = Math.Sin(halfYaw);
    cy = Math.Cos(halfYaw);

    Quaternion result;

    result.X = cy * sp * cr + sy * cp * sr;
    result.Y = sy * cp * cr - cy * sp * sr;
    result.Z = cy * cp * sr - sy * sp * cr;
    result.W = cy * cp * cr + sy * sp * sr;

    return result;
  }

  /// <summary>
  /// Creates a Quaternion from the given rotation matrix.
  /// </summary>
  /// <param name="matrix">The rotation matrix.</param>
  /// <returns>The created Quaternion.</returns>
  public static Quaternion CreateFromRotationMatrix(Matrix4x4 matrix)
  {
    double trace = matrix.M11 + matrix.M22 + matrix.M33;

    Quaternion q = new();

    if (trace > 0.0)
    {
      double s = Math.Sqrt(trace + 1.0);
      q.W = s * 0.5;
      s = 0.5 / s;
      q.X = (matrix.M23 - matrix.M32) * s;
      q.Y = (matrix.M31 - matrix.M13) * s;
      q.Z = (matrix.M12 - matrix.M21) * s;
    }
    else
    {
      if (matrix.M11 >= matrix.M22 && matrix.M11 >= matrix.M33)
      {
        double s = Math.Sqrt(1.0 + matrix.M11 - matrix.M22 - matrix.M33);
        double invS = 0.5 / s;
        q.X = 0.5 * s;
        q.Y = (matrix.M12 + matrix.M21) * invS;
        q.Z = (matrix.M13 + matrix.M31) * invS;
        q.W = (matrix.M23 - matrix.M32) * invS;
      }
      else if (matrix.M22 > matrix.M33)
      {
        double s = Math.Sqrt(1.0 + matrix.M22 - matrix.M11 - matrix.M33);
        double invS = 0.5 / s;
        q.X = (matrix.M21 + matrix.M12) * invS;
        q.Y = 0.5 * s;
        q.Z = (matrix.M32 + matrix.M23) * invS;
        q.W = (matrix.M31 - matrix.M13) * invS;
      }
      else
      {
        double s = Math.Sqrt(1.0 + matrix.M33 - matrix.M11 - matrix.M22);
        double invS = 0.5 / s;
        q.X = (matrix.M31 + matrix.M13) * invS;
        q.Y = (matrix.M32 + matrix.M23) * invS;
        q.Z = 0.5 * s;
        q.W = (matrix.M12 - matrix.M21) * invS;
      }
    }

    return q;
  }

  /// <summary>
  /// Calculates the dot product of two Quaternions.
  /// </summary>
  /// <param name="quaternion1">The first source Quaternion.</param>
  /// <param name="quaternion2">The second source Quaternion.</param>
  /// <returns>The dot product of the Quaternions.</returns>
  public static double Dot(Quaternion quaternion1, Quaternion quaternion2) =>
    quaternion1.X * quaternion2.X
    + quaternion1.Y * quaternion2.Y
    + quaternion1.Z * quaternion2.Z
    + quaternion1.W * quaternion2.W;

  /// <summary>
  /// Interpolates between two quaternions, using spherical linear interpolation.
  /// </summary>
  /// <param name="quaternion1">The first source Quaternion.</param>
  /// <param name="quaternion2">The second source Quaternion.</param>
  /// <param name="amount">The relative weight of the second source Quaternion in the interpolation.</param>
  /// <returns>The interpolated Quaternion.</returns>
  public static Quaternion Slerp(Quaternion quaternion1, Quaternion quaternion2, double amount)
  {
    const double epsilon = 1e-6;

    double t = amount;

    double cosOmega =
      quaternion1.X * quaternion2.X
      + quaternion1.Y * quaternion2.Y
      + quaternion1.Z * quaternion2.Z
      + quaternion1.W * quaternion2.W;

    bool flip = false;

    if (cosOmega < 0.0)
    {
      flip = true;
      cosOmega = -cosOmega;
    }

    double s1,
      s2;

    if (cosOmega > (1.0 - epsilon))
    {
      // Too close, do straight linear interpolation.
      s1 = 1.0 - t;
      s2 = (flip) ? -t : t;
    }
    else
    {
      double omega = Math.Acos(cosOmega);
      double invSinOmega = 1 / Math.Sin(omega);

      s1 = Math.Sin((1.0 - t) * omega) * invSinOmega;
      s2 = (flip) ? -Math.Sin(t * omega) * invSinOmega : Math.Sin(t * omega) * invSinOmega;
    }

    Quaternion ans;

    ans.X = s1 * quaternion1.X + s2 * quaternion2.X;
    ans.Y = s1 * quaternion1.Y + s2 * quaternion2.Y;
    ans.Z = s1 * quaternion1.Z + s2 * quaternion2.Z;
    ans.W = s1 * quaternion1.W + s2 * quaternion2.W;

    return ans;
  }

  /// <summary>
  ///  Linearly interpolates between two quaternions.
  /// </summary>
  /// <param name="quaternion1">The first source Quaternion.</param>
  /// <param name="quaternion2">The second source Quaternion.</param>
  /// <param name="amount">The relative weight of the second source Quaternion in the interpolation.</param>
  /// <returns>The interpolated Quaternion.</returns>
  public static Quaternion Lerp(Quaternion quaternion1, Quaternion quaternion2, double amount)
  {
    double t = amount;
    double t1 = 1.0 - t;

    Quaternion r = new();

    double dot =
      quaternion1.X * quaternion2.X
      + quaternion1.Y * quaternion2.Y
      + quaternion1.Z * quaternion2.Z
      + quaternion1.W * quaternion2.W;

    if (dot >= 0.0)
    {
      r.X = t1 * quaternion1.X + t * quaternion2.X;
      r.Y = t1 * quaternion1.Y + t * quaternion2.Y;
      r.Z = t1 * quaternion1.Z + t * quaternion2.Z;
      r.W = t1 * quaternion1.W + t * quaternion2.W;
    }
    else
    {
      r.X = t1 * quaternion1.X - t * quaternion2.X;
      r.Y = t1 * quaternion1.Y - t * quaternion2.Y;
      r.Z = t1 * quaternion1.Z - t * quaternion2.Z;
      r.W = t1 * quaternion1.W - t * quaternion2.W;
    }

    // Normalize it.
    double ls = r.X * r.X + r.Y * r.Y + r.Z * r.Z + r.W * r.W;
    double invNorm = 1.0 / Math.Sqrt(ls);

    r.X *= invNorm;
    r.Y *= invNorm;
    r.Z *= invNorm;
    r.W *= invNorm;

    return r;
  }

  /// <summary>
  /// Concatenates two Quaternions; the result represents the value1 rotation followed by the value2 rotation.
  /// </summary>
  /// <param name="value1">The first Quaternion rotation in the series.</param>
  /// <param name="value2">The second Quaternion rotation in the series.</param>
  /// <returns>A new Quaternion representing the concatenation of the value1 rotation followed by the value2 rotation.</returns>
  public static Quaternion Concatenate(Quaternion value1, Quaternion value2)
  {
    Quaternion ans;

    // Concatenate rotation is actually q2 * q1 instead of q1 * q2.
    // So that's why value2 goes q1 and value1 goes q2.
    double q1x = value2.X;
    double q1y = value2.Y;
    double q1z = value2.Z;
    double q1w = value2.W;

    double q2x = value1.X;
    double q2y = value1.Y;
    double q2z = value1.Z;
    double q2w = value1.W;

    // cross(av, bv)
    double cx = q1y * q2z - q1z * q2y;
    double cy = q1z * q2x - q1x * q2z;
    double cz = q1x * q2y - q1y * q2x;

    double dot = q1x * q2x + q1y * q2y + q1z * q2z;

    ans.X = q1x * q2w + q2x * q1w + cx;
    ans.Y = q1y * q2w + q2y * q1w + cy;
    ans.Z = q1z * q2w + q2z * q1w + cz;
    ans.W = q1w * q2w - dot;

    return ans;
  }

  /// <summary>
  /// Flips the sign of each component of the quaternion.
  /// </summary>
  /// <param name="value">The source Quaternion.</param>
  /// <returns>The negated Quaternion.</returns>
  public static Quaternion Negate(Quaternion value)
  {
    Quaternion ans;

    ans.X = -value.X;
    ans.Y = -value.Y;
    ans.Z = -value.Z;
    ans.W = -value.W;

    return ans;
  }

  /// <summary>
  /// Adds two Quaternions element-by-element.
  /// </summary>
  /// <param name="value1">The first source Quaternion.</param>
  /// <param name="value2">The second source Quaternion.</param>
  /// <returns>The result of adding the Quaternions.</returns>
  public static Quaternion Add(Quaternion value1, Quaternion value2)
  {
    Quaternion ans;

    ans.X = value1.X + value2.X;
    ans.Y = value1.Y + value2.Y;
    ans.Z = value1.Z + value2.Z;
    ans.W = value1.W + value2.W;

    return ans;
  }

  /// <summary>
  /// Subtracts one Quaternion from another.
  /// </summary>
  /// <param name="value1">The first source Quaternion.</param>
  /// <param name="value2">The second Quaternion, to be subtracted from the first.</param>
  /// <returns>The result of the subtraction.</returns>
  public static Quaternion Subtract(Quaternion value1, Quaternion value2)
  {
    Quaternion ans;

    ans.X = value1.X - value2.X;
    ans.Y = value1.Y - value2.Y;
    ans.Z = value1.Z - value2.Z;
    ans.W = value1.W - value2.W;

    return ans;
  }

  /// <summary>
  /// Multiplies two Quaternions together.
  /// </summary>
  /// <param name="value1">The Quaternion on the left side of the multiplication.</param>
  /// <param name="value2">The Quaternion on the right side of the multiplication.</param>
  /// <returns>The result of the multiplication.</returns>
  public static Quaternion Multiply(Quaternion value1, Quaternion value2)
  {
    Quaternion ans;

    double q1x = value1.X;
    double q1y = value1.Y;
    double q1z = value1.Z;
    double q1w = value1.W;

    double q2x = value2.X;
    double q2y = value2.Y;
    double q2z = value2.Z;
    double q2w = value2.W;

    // cross(av, bv)
    double cx = q1y * q2z - q1z * q2y;
    double cy = q1z * q2x - q1x * q2z;
    double cz = q1x * q2y - q1y * q2x;

    double dot = q1x * q2x + q1y * q2y + q1z * q2z;

    ans.X = q1x * q2w + q2x * q1w + cx;
    ans.Y = q1y * q2w + q2y * q1w + cy;
    ans.Z = q1z * q2w + q2z * q1w + cz;
    ans.W = q1w * q2w - dot;

    return ans;
  }

  /// <summary>
  /// Multiplies a Quaternion by a scalar value.
  /// </summary>
  /// <param name="value1">The source Quaternion.</param>
  /// <param name="value2">The scalar value.</param>
  /// <returns>The result of the multiplication.</returns>
  public static Quaternion Multiply(Quaternion value1, double value2)
  {
    Quaternion ans;

    ans.X = value1.X * value2;
    ans.Y = value1.Y * value2;
    ans.Z = value1.Z * value2;
    ans.W = value1.W * value2;

    return ans;
  }

  /// <summary>
  /// Divides a Quaternion by another Quaternion.
  /// </summary>
  /// <param name="value1">The source Quaternion.</param>
  /// <param name="value2">The divisor.</param>
  /// <returns>The result of the division.</returns>
  public static Quaternion Divide(Quaternion value1, Quaternion value2)
  {
    Quaternion ans;

    double q1x = value1.X;
    double q1y = value1.Y;
    double q1z = value1.Z;
    double q1w = value1.W;

    //-------------------------------------
    // Inverse part.
    double ls = value2.X * value2.X + value2.Y * value2.Y + value2.Z * value2.Z + value2.W * value2.W;
    double invNorm = 1.0 / ls;

    double q2x = -value2.X * invNorm;
    double q2y = -value2.Y * invNorm;
    double q2z = -value2.Z * invNorm;
    double q2w = value2.W * invNorm;

    //-------------------------------------
    // Multiply part.

    // cross(av, bv)
    double cx = q1y * q2z - q1z * q2y;
    double cy = q1z * q2x - q1x * q2z;
    double cz = q1x * q2y - q1y * q2x;

    double dot = q1x * q2x + q1y * q2y + q1z * q2z;

    ans.X = q1x * q2w + q2x * q1w + cx;
    ans.Y = q1y * q2w + q2y * q1w + cy;
    ans.Z = q1z * q2w + q2z * q1w + cz;
    ans.W = q1w * q2w - dot;

    return ans;
  }

  /// <summary>
  /// Flips the sign of each component of the quaternion.
  /// </summary>
  /// <param name="value">The source Quaternion.</param>
  /// <returns>The negated Quaternion.</returns>
  public static Quaternion operator -(Quaternion value)
  {
    Quaternion ans;

    ans.X = -value.X;
    ans.Y = -value.Y;
    ans.Z = -value.Z;
    ans.W = -value.W;

    return ans;
  }

  /// <summary>
  /// Adds two Quaternions element-by-element.
  /// </summary>
  /// <param name="value1">The first source Quaternion.</param>
  /// <param name="value2">The second source Quaternion.</param>
  /// <returns>The result of adding the Quaternions.</returns>
  public static Quaternion operator +(Quaternion value1, Quaternion value2)
  {
    Quaternion ans;

    ans.X = value1.X + value2.X;
    ans.Y = value1.Y + value2.Y;
    ans.Z = value1.Z + value2.Z;
    ans.W = value1.W + value2.W;

    return ans;
  }

  /// <summary>
  /// Subtracts one Quaternion from another.
  /// </summary>
  /// <param name="value1">The first source Quaternion.</param>
  /// <param name="value2">The second Quaternion, to be subtracted from the first.</param>
  /// <returns>The result of the subtraction.</returns>
  public static Quaternion operator -(Quaternion value1, Quaternion value2)
  {
    Quaternion ans;

    ans.X = value1.X - value2.X;
    ans.Y = value1.Y - value2.Y;
    ans.Z = value1.Z - value2.Z;
    ans.W = value1.W - value2.W;

    return ans;
  }

  /// <summary>
  /// Multiplies two Quaternions together.
  /// </summary>
  /// <param name="value1">The Quaternion on the left side of the multiplication.</param>
  /// <param name="value2">The Quaternion on the right side of the multiplication.</param>
  /// <returns>The result of the multiplication.</returns>
  public static Quaternion operator *(Quaternion value1, Quaternion value2)
  {
    Quaternion ans;

    double q1x = value1.X;
    double q1y = value1.Y;
    double q1z = value1.Z;
    double q1w = value1.W;

    double q2x = value2.X;
    double q2y = value2.Y;
    double q2z = value2.Z;
    double q2w = value2.W;

    // cross(av, bv)
    double cx = q1y * q2z - q1z * q2y;
    double cy = q1z * q2x - q1x * q2z;
    double cz = q1x * q2y - q1y * q2x;

    double dot = q1x * q2x + q1y * q2y + q1z * q2z;

    ans.X = q1x * q2w + q2x * q1w + cx;
    ans.Y = q1y * q2w + q2y * q1w + cy;
    ans.Z = q1z * q2w + q2z * q1w + cz;
    ans.W = q1w * q2w - dot;

    return ans;
  }

  /// <summary>
  /// Multiplies a Quaternion by a scalar value.
  /// </summary>
  /// <param name="value1">The source Quaternion.</param>
  /// <param name="value2">The scalar value.</param>
  /// <returns>The result of the multiplication.</returns>
  public static Quaternion operator *(Quaternion value1, double value2)
  {
    Quaternion ans;

    ans.X = value1.X * value2;
    ans.Y = value1.Y * value2;
    ans.Z = value1.Z * value2;
    ans.W = value1.W * value2;

    return ans;
  }

  /// <summary>
  /// Divides a Quaternion by another Quaternion.
  /// </summary>
  /// <param name="value1">The source Quaternion.</param>
  /// <param name="value2">The divisor.</param>
  /// <returns>The result of the division.</returns>
  public static Quaternion operator /(Quaternion value1, Quaternion value2)
  {
    Quaternion ans;

    double q1x = value1.X;
    double q1y = value1.Y;
    double q1z = value1.Z;
    double q1w = value1.W;

    //-------------------------------------
    // Inverse part.
    double ls = value2.X * value2.X + value2.Y * value2.Y + value2.Z * value2.Z + value2.W * value2.W;
    double invNorm = 1.0 / ls;

    double q2x = -value2.X * invNorm;
    double q2y = -value2.Y * invNorm;
    double q2z = -value2.Z * invNorm;
    double q2w = value2.W * invNorm;

    //-------------------------------------
    // Multiply part.

    // cross(av, bv)
    double cx = q1y * q2z - q1z * q2y;
    double cy = q1z * q2x - q1x * q2z;
    double cz = q1x * q2y - q1y * q2x;

    double dot = q1x * q2x + q1y * q2y + q1z * q2z;

    ans.X = q1x * q2w + q2x * q1w + cx;
    ans.Y = q1y * q2w + q2y * q1w + cy;
    ans.Z = q1z * q2w + q2z * q1w + cz;
    ans.W = q1w * q2w - dot;

    return ans;
  }

  /// <summary>
  /// Returns a boolean indicating whether the two given Quaternions are equal.
  /// </summary>
  /// <param name="value1">The first Quaternion to compare.</param>
  /// <param name="value2">The second Quaternion to compare.</param>
  /// <returns>True if the Quaternions are equal; False otherwise.</returns>
  public static bool operator ==(Quaternion value1, Quaternion value2) =>
    (value1.X == value2.X && value1.Y == value2.Y && value1.Z == value2.Z && value1.W == value2.W);

  /// <summary>
  /// Returns a boolean indicating whether the two given Quaternions are not equal.
  /// </summary>
  /// <param name="value1">The first Quaternion to compare.</param>
  /// <param name="value2">The second Quaternion to compare.</param>
  /// <returns>True if the Quaternions are not equal; False if they are equal.</returns>
  public static bool operator !=(Quaternion value1, Quaternion value2) =>
    (value1.X != value2.X || value1.Y != value2.Y || value1.Z != value2.Z || value1.W != value2.W);

  /// <summary>
  /// Returns a boolean indicating whether the given Quaternion is equal to this Quaternion instance.
  /// </summary>
  /// <param name="other">The Quaternion to compare this instance to.</param>
  /// <returns>True if the other Quaternion is equal to this instance; False otherwise.</returns>
  public bool Equals(Quaternion other) => (X == other.X && Y == other.Y && Z == other.Z && W == other.W);

  /// <summary>
  /// Returns a boolean indicating whether the given Object is equal to this Quaternion instance.
  /// </summary>
  /// <param name="obj">The Object to compare against.</param>
  /// <returns>True if the Object is equal to this Quaternion; False otherwise.</returns>
  public override bool Equals(object? obj)
  {
    if (obj is Quaternion quaternion)
    {
      return Equals(quaternion);
    }

    return false;
  }

  /// <summary>
  /// Returns a String representing this Quaternion instance.
  /// </summary>
  /// <returns>The string representation.</returns>
  public override string ToString()
  {
    CultureInfo ci = CultureInfo.CurrentCulture;

    return String.Format(
      ci,
      "{{X:{0} Y:{1} Z:{2} W:{3}}}",
      X.ToString(ci),
      Y.ToString(ci),
      Z.ToString(ci),
      W.ToString(ci)
    );
  }

  /// <summary>
  /// Returns the hash code for this instance.
  /// </summary>
  /// <returns>The hash code.</returns>
  public override int GetHashCode() => X.GetHashCode() + Y.GetHashCode() + Z.GetHashCode() + W.GetHashCode();
}
