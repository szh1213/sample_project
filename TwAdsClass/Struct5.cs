using System;
using System.Runtime.InteropServices;

namespace TwAdsClass
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct Struct5
    {
        [MarshalAs(UnmanagedType.I2)]
        public Int16 nData;
        [MarshalAs(UnmanagedType.I1)]
        public byte byVal;
        [MarshalAs(UnmanagedType.R4)]
        public float fVal;
    }
}
