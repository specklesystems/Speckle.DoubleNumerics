﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Speckle.DoubleNumerics;

internal static class HashHelpers
{
  public static int Combine(int h1, int h2)
  {
    // The jit optimizes this to use the ROL instruction on x86
    // Related GitHub pull request: dotnet/coreclr#1830
    uint shift5 = ((uint)h1 << 5) | ((uint)h1 >> 27);
    return ((int)shift5 + h1) ^ h2;
  }
}
