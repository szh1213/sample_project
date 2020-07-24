using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace TwAdsClass
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct Struct2
    {
        [MarshalAs(UnmanagedType.U2)]
        public UInt16 DriverIO;
        [MarshalAs(UnmanagedType.U2)]
        public UInt16 DriverErrorID;

        [MarshalAs(UnmanagedType.I1)]
        public bool Q_Brake;
        [MarshalAs(UnmanagedType.I1)]
        public bool AxisCamDog1;
        [MarshalAs(UnmanagedType.I1)]
        public bool AxisCamDog2;
        [MarshalAs(UnmanagedType.I1)]
        public bool LimitP;
        [MarshalAs(UnmanagedType.I1)]
        public bool LimitN;
        [MarshalAs(UnmanagedType.I1)]
        public bool HOME;
        [MarshalAs(UnmanagedType.I1)]
        public bool HomingDirection;
        [MarshalAs(UnmanagedType.I1)]
        public bool EXT1;
        [MarshalAs(UnmanagedType.U4)]
        public UInt32 ToDriverIO;

        [MarshalAs(UnmanagedType.I1)]
        public bool ServoOn_HMI;
        [MarshalAs(UnmanagedType.I1)]
        public bool ServoOnInt;
        [MarshalAs(UnmanagedType.I1)]
        public bool Homing_HMI;
        [MarshalAs(UnmanagedType.I1)]
        public bool HomingInt;
        [MarshalAs(UnmanagedType.I1)]
        public bool BrakeBtn_HMI;
        [MarshalAs(UnmanagedType.I1)]
        public bool Inch_HMI;
        [MarshalAs(UnmanagedType.I1)]
        public bool JogF_HMI;
        [MarshalAs(UnmanagedType.I1)]
        public bool JogB_HMI;
        [MarshalAs(UnmanagedType.I1)]
        public bool StationIni;
        [MarshalAs(UnmanagedType.I1)]
        public bool AutoPStart_HMI;
        [MarshalAs(UnmanagedType.I1)]
        public bool StopCond;

        [MarshalAs(UnmanagedType.I1)]
        public bool ServoOn_L;
        [MarshalAs(UnmanagedType.I1)]
        public bool OriginOk_L;
        [MarshalAs(UnmanagedType.I1)]
        public bool SingleCycle;
        [MarshalAs(UnmanagedType.I1)]
        public bool Busy;
        [MarshalAs(UnmanagedType.R8)]
        public double ActPos;
        [MarshalAs(UnmanagedType.R8)]
        public double AutoPosition;

        [MarshalAs(UnmanagedType.U4)]
        public UInt32 Alarm1;
        [MarshalAs(UnmanagedType.U4)]
        public UInt32 ErrorID;

        [MarshalAs(UnmanagedType.I1)]
        public bool ActionEnable;   //轴可动作
        [MarshalAs(UnmanagedType.I1)]
        public bool ActionM;   //轴手动基本条件
        [MarshalAs(UnmanagedType.I1)]
        public bool PosEnable;
        [MarshalAs(UnmanagedType.I1)]
        public bool PosEnableM;
        [MarshalAs(UnmanagedType.I1)]
        public bool BackAutoPosEN;
        [MarshalAs(UnmanagedType.I1)]
        public bool Homing;   //轴回原点
        [MarshalAs(UnmanagedType.I1)]
        public bool MotionMethod;   //轴初始化回原点
        [MarshalAs(UnmanagedType.I1)]
        public bool ServoOn_M;   //伺服ON
        [MarshalAs(UnmanagedType.I1)]
        public bool Inch_M;   //寸动
        [MarshalAs(UnmanagedType.I1)]
        public bool JogF_M;   //正向微动
        [MarshalAs(UnmanagedType.I1)]
        public bool JogB_M;   //反向微动
        [MarshalAs(UnmanagedType.I1)]
        public bool AutoPStart_M;   //轴自动停位启动
        [MarshalAs(UnmanagedType.I1)]
        public bool BrakeBtn_M;   //刹车

        [MarshalAs(UnmanagedType.I1)]
        public bool GrabSafetyPos;
        [MarshalAs(UnmanagedType.I1)]
        public bool PlacedSafetyPos;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] PosHandStart;   //轴手动启动 
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] PosHandStartM;   //轴手动启动
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] PosAutoStart;   //轴自动启动
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] PosStart_M;   //轴启动	
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] Position_L;   //轴位指示

        [MarshalAs(UnmanagedType.I1)]
        public bool PositionDone;   //轴动作完成

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] PosHandEnable;   //轴手动条件

        [MarshalAs(UnmanagedType.R8)]
        public double VelocityTotal;  //速度汇总 
        [MarshalAs(UnmanagedType.R8)]
        public double OriginCompensation;  //原点补偿

        [MarshalAs(UnmanagedType.R8)]
        public double InchingLength;  //寸动长度
        [MarshalAs(UnmanagedType.R8)]
        public double Override;  //速度倍率
        [MarshalAs(UnmanagedType.R8)]
        public double JogVelocity;  //微动速度
        [MarshalAs(UnmanagedType.R8)]
        public double VelocityManual;  //轴手动速度 
        [MarshalAs(UnmanagedType.R8)]
        public double VelocityAuto;  //轴自动速度	
        [MarshalAs(UnmanagedType.R8)]
        public double VelocityAutoM;  //轴自动速度M
        [MarshalAs(UnmanagedType.R8)]
        public double VelocityDebug;  //调试速度

        [MarshalAs(UnmanagedType.R8)]
        public double HomingVelocity;  //原点补偿与寸动速度
        [MarshalAs(UnmanagedType.R8)]
        public double ErrorRangeSet;  //轴误差范围设定 
        [MarshalAs(UnmanagedType.I4)]
        public Int32 AlarmCount;   //报警计数

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public double[] PositionSet;

        [MarshalAs(UnmanagedType.R8)]
        public double DTerminalVelocity;  //对端子速度   	    
        [MarshalAs(UnmanagedType.R8)]
        public double TerminalvelocityManual;  //对端子手动速度
        [MarshalAs(UnmanagedType.R8)]
        public double TerminalvelocityAuto;    //对端子自动速度
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct Plc_StAxisInfo
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public Struct2[] Axis;
    }
}
