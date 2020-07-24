using System;
using System.Runtime.InteropServices;

namespace TwAdsClass
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct Struct4
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public Int16[] nData;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] byVal;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public float[] fVal;
    }
}
