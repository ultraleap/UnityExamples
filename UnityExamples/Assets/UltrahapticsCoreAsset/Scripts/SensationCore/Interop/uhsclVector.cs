﻿using System.Runtime.InteropServices;
using UnityEngine;

namespace UltrahapticsCoreAsset
{
    [StructLayout(LayoutKind.Sequential)]
    public struct uhsclVector3_t
    {
        public double X;
        public double Y;
        public double Z;

        public uhsclVector3_t(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public UnityEngine.Vector3 toVector3()
        {
            return new UnityEngine.Vector3((float)X, (float)Y, (float)Z);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct uhsclVector4_t
    {
        public double X;
        public double Y;
        public double Z;
        public double W;

        public uhsclVector4_t(double x, double y, double z, double w)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }

        public UnityEngine.Vector4 toVector4()
        {
            return new UnityEngine.Vector4((float)X, (float)Y, (float)Z, (float)W);
        }
    }
}
