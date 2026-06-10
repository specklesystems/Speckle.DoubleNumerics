using BenchmarkDotNet.Attributes;
using Speckle.DoubleNumerics;

namespace Speckle.DoubleNumerics.Benchmarks;

/// <summary>
/// Each benchmark performs <see cref="N" /> operations over pre-generated data,
/// so reported times are per-1000-operations. Results are written to heap arrays
/// (or accumulated) to prevent dead-code elimination.
/// </summary>
public class SimdBenchmarks
{
  private const int N = 1000;

  private readonly Vector2[] _v2A = new Vector2[N];
  private readonly Vector2[] _v2B = new Vector2[N];
  private readonly Vector2[] _v2R = new Vector2[N];

  private readonly Vector3[] _v3A = new Vector3[N];
  private readonly Vector3[] _v3B = new Vector3[N];
  private readonly Vector3[] _v3R = new Vector3[N];

  private readonly Vector4[] _v4A = new Vector4[N];
  private readonly Vector4[] _v4B = new Vector4[N];
  private readonly Vector4[] _v4R = new Vector4[N];

  private readonly Quaternion[] _qA = new Quaternion[N];
  private readonly Quaternion[] _qB = new Quaternion[N];
  private readonly Quaternion[] _qR = new Quaternion[N];

  private readonly Plane[] _pA = new Plane[N];
  private readonly Plane[] _pR = new Plane[N];

  private Matrix4x4 _m4;
  private Matrix3x2 _m3;

  [GlobalSetup]
  public void Setup()
  {
    var rng = new Random(42);
    double D() => rng.NextDouble() * 2.0 - 1.0;

    for (int i = 0; i < N; i++)
    {
      _v2A[i] = new Vector2(D(), D());
      _v2B[i] = new Vector2(D(), D());
      _v3A[i] = new Vector3(D(), D(), D());
      _v3B[i] = new Vector3(D(), D(), D());
      _v4A[i] = new Vector4(D(), D(), D(), D());
      _v4B[i] = new Vector4(D(), D(), D(), D());
      _qA[i] = Quaternion.CreateFromYawPitchRoll(D(), D(), D());
      _qB[i] = Quaternion.CreateFromYawPitchRoll(D(), D(), D());
      _pA[i] = new Plane(D() + 2.0, D(), D(), D());
    }

    _m4 = Matrix4x4.CreateFromYawPitchRoll(0.5, 0.25, 0.125) * Matrix4x4.CreateTranslation(1, 2, 3);
    _m3 = Matrix3x2.CreateRotation(0.5) * Matrix3x2.CreateTranslation(1, 2);
  }

  // ---- Vector4 ----

  [Benchmark]
  public void Vector4_Add()
  {
    for (int i = 0; i < N; i++)
      _v4R[i] = _v4A[i] + _v4B[i];
  }

  [Benchmark]
  public void Vector4_Multiply()
  {
    for (int i = 0; i < N; i++)
      _v4R[i] = _v4A[i] * _v4B[i];
  }

  [Benchmark]
  public double Vector4_Dot()
  {
    double sum = 0;
    for (int i = 0; i < N; i++)
      sum += Vector4.Dot(_v4A[i], _v4B[i]);
    return sum;
  }

  [Benchmark]
  public double Vector4_Length()
  {
    double sum = 0;
    for (int i = 0; i < N; i++)
      sum += _v4A[i].Length();
    return sum;
  }

  [Benchmark]
  public void Vector4_Normalize()
  {
    for (int i = 0; i < N; i++)
      _v4R[i] = Vector4.Normalize(_v4A[i]);
  }

  [Benchmark]
  public double Vector4_Distance()
  {
    double sum = 0;
    for (int i = 0; i < N; i++)
      sum += Vector4.Distance(_v4A[i], _v4B[i]);
    return sum;
  }

  [Benchmark]
  public void Vector4_Min()
  {
    for (int i = 0; i < N; i++)
      _v4R[i] = Vector4.Min(_v4A[i], _v4B[i]);
  }

  [Benchmark]
  public void Vector4_SquareRoot()
  {
    for (int i = 0; i < N; i++)
      _v4R[i] = Vector4.SquareRoot(Vector4.Abs(_v4A[i]));
  }

  [Benchmark]
  public void Vector4_Lerp()
  {
    for (int i = 0; i < N; i++)
      _v4R[i] = Vector4.Lerp(_v4A[i], _v4B[i], 0.5);
  }

  [Benchmark]
  public void Vector4_Clamp()
  {
    for (int i = 0; i < N; i++)
      _v4R[i] = Vector4.Clamp(_v4A[i], _v4B[i], _v4B[i]);
  }

  [Benchmark]
  public void Vector4_Transform()
  {
    for (int i = 0; i < N; i++)
      _v4R[i] = Vector4.Transform(_v4A[i], _m4);
  }

  // ---- Vector3 ----

  [Benchmark]
  public void Vector3_Add()
  {
    for (int i = 0; i < N; i++)
      _v3R[i] = _v3A[i] + _v3B[i];
  }

  [Benchmark]
  public double Vector3_Dot()
  {
    double sum = 0;
    for (int i = 0; i < N; i++)
      sum += Vector3.Dot(_v3A[i], _v3B[i]);
    return sum;
  }

  [Benchmark]
  public void Vector3_Cross()
  {
    for (int i = 0; i < N; i++)
      _v3R[i] = Vector3.Cross(_v3A[i], _v3B[i]);
  }

  [Benchmark]
  public void Vector3_Normalize()
  {
    for (int i = 0; i < N; i++)
      _v3R[i] = Vector3.Normalize(_v3A[i]);
  }

  [Benchmark]
  public void Vector3_Reflect()
  {
    for (int i = 0; i < N; i++)
      _v3R[i] = Vector3.Reflect(_v3A[i], _v3B[i]);
  }

  [Benchmark]
  public void Vector3_Transform()
  {
    for (int i = 0; i < N; i++)
      _v3R[i] = Vector3.Transform(_v3A[i], _m4);
  }

  [Benchmark]
  public void Vector3_Min()
  {
    for (int i = 0; i < N; i++)
      _v3R[i] = Vector3.Min(_v3A[i], _v3B[i]);
  }

  [Benchmark]
  public void Vector3_Clamp()
  {
    for (int i = 0; i < N; i++)
      _v3R[i] = Vector3.Clamp(_v3A[i], _v3B[i], _v3B[i]);
  }

  [Benchmark]
  public void Vector3_Lerp()
  {
    for (int i = 0; i < N; i++)
      _v3R[i] = Vector3.Lerp(_v3A[i], _v3B[i], 0.5);
  }

  [Benchmark]
  public void Vector3_SquareRoot()
  {
    for (int i = 0; i < N; i++)
      _v3R[i] = Vector3.SquareRoot(Vector3.Abs(_v3A[i]));
  }

  [Benchmark]
  public void Vector3_Multiply()
  {
    for (int i = 0; i < N; i++)
      _v3R[i] = _v3A[i] * _v3B[i];
  }

  // ---- Vector2 ----

  [Benchmark]
  public void Vector2_Add()
  {
    for (int i = 0; i < N; i++)
      _v2R[i] = _v2A[i] + _v2B[i];
  }

  [Benchmark]
  public double Vector2_Dot()
  {
    double sum = 0;
    for (int i = 0; i < N; i++)
      sum += Vector2.Dot(_v2A[i], _v2B[i]);
    return sum;
  }

  [Benchmark]
  public void Vector2_Transform()
  {
    for (int i = 0; i < N; i++)
      _v2R[i] = Vector2.Transform(_v2A[i], _m3);
  }

  [Benchmark]
  public void Vector2_Lerp()
  {
    for (int i = 0; i < N; i++)
      _v2R[i] = Vector2.Lerp(_v2A[i], _v2B[i], 0.5);
  }

  [Benchmark]
  public void Vector2_Min()
  {
    for (int i = 0; i < N; i++)
      _v2R[i] = Vector2.Min(_v2A[i], _v2B[i]);
  }

  // ---- Quaternion ----

  [Benchmark]
  public void Quaternion_Multiply()
  {
    for (int i = 0; i < N; i++)
      _qR[i] = _qA[i] * _qB[i];
  }

  [Benchmark]
  public void Quaternion_Normalize()
  {
    for (int i = 0; i < N; i++)
      _qR[i] = Quaternion.Normalize(_qA[i]);
  }

  [Benchmark]
  public void Quaternion_Lerp()
  {
    for (int i = 0; i < N; i++)
      _qR[i] = Quaternion.Lerp(_qA[i], _qB[i], 0.5);
  }

  // ---- Plane ----

  [Benchmark]
  public void Plane_Normalize()
  {
    for (int i = 0; i < N; i++)
      _pR[i] = Plane.Normalize(_pA[i]);
  }

  [Benchmark]
  public double Plane_DotCoordinate()
  {
    double sum = 0;
    for (int i = 0; i < N; i++)
      sum += Plane.DotCoordinate(_pA[i], _v3A[i]);
    return sum;
  }
}
