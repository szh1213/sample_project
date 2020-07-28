using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace TwAdsClass
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct team_date
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public short[] DATA_TEAM;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public short[] DATA_BATCH;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public short[] DATA_STATION;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public short[] DATA_NUMBER;
    }
    //DATA_TEAM: ARRAY[0..19] OF INT;	//日期
    //DATA_BATCH: ARRAY[0..19] OF INT;	//批次
    //DATA_STATION: ARRAY[0..19] OF INT;	//工位
    //DATA_NUMBER: ARRAY[0..19] OF INT;	//次数

}
