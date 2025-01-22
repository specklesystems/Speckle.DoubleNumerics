using System.Runtime.Intrinsics;
using Xunit;

namespace Speckle.DoubleNumerics.Tests;

public class ConversionExtensionsTests
{
  [Fact]
  public void PlaneAsVector4Test()
  {
    var plane = new Plane(1, 2, 3, 4);
    var vector4 = plane.AsVector4();
    Assert.Equal(plane.Normal.X, vector4.X);
    Assert.Equal(plane.Normal.Y, vector4.Y);
    Assert.Equal(plane.Normal.Z, vector4.Z);
    Assert.Equal(plane.D, vector4.W);
  }

  [Fact]
  public void PlaneAsVector256Test()
  {
    var plane = new Plane(1, 2, 3, 4);
    var vector256 = plane.AsVector256();
    Assert.Equal(plane.Normal.X, vector256[0]);
    Assert.Equal(plane.Normal.Y, vector256[1]);
    Assert.Equal(plane.Normal.Z, vector256[2]);
    Assert.Equal(plane.D, vector256[3]);
  }

  [Fact]
  public void QuaternionAsVector4Test()
  {
    var quaternion = new Quaternion(1, 2, 3, 4);
    var vector4 = quaternion.AsVector4();
    Assert.Equal(quaternion.X, vector4.X);
    Assert.Equal(quaternion.Y, vector4.Y);
    Assert.Equal(quaternion.Z, vector4.Z);
    Assert.Equal(quaternion.W, vector4.W);
  }

  [Fact]
  public void QuaternionAsVector256Test()
  {
    var quaternion = new Quaternion(1, 2, 3, 4);
    var vector256 = quaternion.AsVector256();
    Assert.Equal(quaternion.X, vector256[0]);
    Assert.Equal(quaternion.Y, vector256[1]);
    Assert.Equal(quaternion.Z, vector256[2]);
    Assert.Equal(quaternion.W, vector256[3]);
  }

  [Fact]
  public void Vector2AsVector3Test()
  {
    var vector2 = new Vector2(1, 2);
    var vector3 = vector2.AsVector3();
    Assert.Equal(vector2.X, vector3.X);
    Assert.Equal(vector2.Y, vector3.Y);
    Assert.Equal(0, vector3.Z);
  }

  [Fact]
  public void Vector2AsVector4Test()
  {
    var vector2 = new Vector2(1, 2);
    var vector4 = vector2.AsVector4();
    Assert.Equal(vector2.X, vector4.X);
    Assert.Equal(vector2.Y, vector4.Y);
    Assert.Equal(0, vector4.Z);
    Assert.Equal(0, vector4.W);
  }

  [Fact]
  public void Vector2AsVector256Test()
  {
    var vector2 = new Vector2(1, 2);
    var vector256 = vector2.AsVector256();
    Assert.Equal(vector2.X, vector256[0]);
    Assert.Equal(vector2.Y, vector256[1]);
    Assert.Equal(0, vector256[2]);
    Assert.Equal(0, vector256[3]);
  }

  [Fact]
  public void Vector3AsVector2Test()
  {
    var vector3 = new Vector3(1, 2, 3);
    var vector2 = vector3.AsVector2();
    Assert.Equal(vector3.X, vector2.X);
    Assert.Equal(vector3.Y, vector2.Y);
  }

  [Fact]
  public void Vector3AsVector4Test()
  {
    var vector3 = new Vector3(1, 2, 3);
    var vector4 = vector3.AsVector4();
    Assert.Equal(vector3.X, vector4.X);
    Assert.Equal(vector3.Y, vector4.Y);
    Assert.Equal(vector3.Z, vector4.Z);
    Assert.Equal(0, vector4.W);
  }

  [Fact]
  public void Vector3AsVector256Test()
  {
    var vector3 = new Vector3(1, 2, 3);
    var vector256 = vector3.AsVector256();
    Assert.Equal(vector3.X, vector256[0]);
    Assert.Equal(vector3.Y, vector256[1]);
    Assert.Equal(vector3.Z, vector256[2]);
    Assert.Equal(0, vector256[3]);
  }

  [Fact]
  public void Vector4AsQuaternionTest()
  {
    var vector4 = new Vector4(1, 2, 3, 4);
    var quaternion = vector4.AsQuaternion();
    Assert.Equal(vector4.X, quaternion.X);
    Assert.Equal(vector4.Y, quaternion.Y);
    Assert.Equal(vector4.Z, quaternion.Z);
    Assert.Equal(vector4.W, quaternion.W);
  }

  [Fact]
  public void Vector4AsPlaneTest()
  {
    var vector4 = new Vector4(1, 2, 3, 4);
    var plane = vector4.AsPlane();
    Assert.Equal(vector4.X, plane.Normal.X);
    Assert.Equal(vector4.Y, plane.Normal.Y);
    Assert.Equal(vector4.Z, plane.Normal.Z);
    Assert.Equal(vector4.W, plane.D);
  }

  [Fact]
  public void Vector4AsVector2Test()
  {
    var vector4 = new Vector4(1, 2, 3, 4);
    var vector2 = vector4.AsVector2();
    Assert.Equal(vector4.X, vector2.X);
    Assert.Equal(vector4.Y, vector2.Y);
  }

  [Fact]
  public void Vector4AsVector3Test()
  {
    var vector4 = new Vector4(1, 2, 3, 4);
    var vector3 = vector4.AsVector3();
    Assert.Equal(vector4.X, vector3.X);
    Assert.Equal(vector4.Y, vector3.Y);
    Assert.Equal(vector4.Z, vector3.Z);
  }

  [Fact]
  public void Vector4AsVector256Test()
  {
    var vector4 = new Vector4(1, 2, 3, 4);
    var vector256 = vector4.AsVector256();
    Assert.Equal(vector4.X, vector256[0]);
    Assert.Equal(vector4.Y, vector256[1]);
    Assert.Equal(vector4.Z, vector256[2]);
    Assert.Equal(vector4.W, vector256[3]);
  }

  [Fact]
  public void Vector256AsPlaneTest()
  {
    var vector256 = Vector256.Create(1d, 2d, 3d, 4d);
    var plane = vector256.AsPlane();
    Assert.Equal(vector256[0], plane.Normal.X);
    Assert.Equal(vector256[1], plane.Normal.Y);
    Assert.Equal(vector256[2], plane.Normal.Z);
    Assert.Equal(vector256[3], plane.D);
  }

  [Fact]
  public void Vector256AsQuaternionTest()
  {
    var vector256 = Vector256.Create(1d, 2d, 3d, 4d);
    var quaternion = vector256.AsQuaternion();
    Assert.Equal(vector256[0], quaternion.X);
    Assert.Equal(vector256[1], quaternion.Y);
    Assert.Equal(vector256[2], quaternion.Z);
    Assert.Equal(vector256[3], quaternion.W);
  }

  [Fact]
  public void Vector256AsVector2Test()
  {
    var vector256 = Vector256.Create(1d, 2d, 0d, 0d);
    var vector2 = vector256.AsVector2();
    Assert.Equal(vector256[0], vector2.X);
    Assert.Equal(vector256[1], vector2.Y);
  }

  [Fact]
  public void Vector256AsVector3Test()
  {
    var vector256 = Vector256.Create(1d, 2d, 3d, 0d);
    var vector3 = vector256.AsVector3();
    Assert.Equal(vector256[0], vector3.X);
    Assert.Equal(vector256[1], vector3.Y);
    Assert.Equal(vector256[2], vector3.Z);
  }

  [Fact]
  public void Vector256AsVector4Test()
  {
    var vector256 = Vector256.Create(1d, 2d, 3d, 4d);
    var vector4 = vector256.AsVector4();
    Assert.Equal(vector256[0], vector4.X);
    Assert.Equal(vector256[1], vector4.Y);
    Assert.Equal(vector256[2], vector4.Z);
    Assert.Equal(vector256[3], vector4.W);
  }
}
