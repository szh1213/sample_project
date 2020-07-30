using System;
using System.Runtime.InteropServices;

namespace TwAdsClass
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct Error_data
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public ushort[] alarm;
    }
    //DATA_ALARM.alarm1.0	BOOL 旋转电机报警
    //DATA_ALARM.alarm1.1	BOOL 升降电机报警
    //DATA_ALARM.alarm1.2	BOOL 收发电机报警
    //DATA_ALARM.alarm1.3	BOOL 空压报警
    //DATA_ALARM.alarm1.4	BOOL
    //DATA_ALARM.alarm1.5	BOOL
    //DATA_ALARM.alarm1.6	BOOL 旋转电机编码器报警
    //DATA_ALARM.alarm1.7	BOOL 升降电机编码器报警
    //DATA_ALARM.alarm1.8	BOOL 收发电机编码器报警
    //DATA_ALARM.alarm1.9	BOOL
    //DATA_ALARM.alarm1.10	BOOL
    //DATA_ALARM.alarm1.11	BOOL
    //DATA_ALARM.alarm1.12	BOOL
    //DATA_ALARM.alarm1.13	BOOL
    //DATA_ALARM.alarm1.14	BOOL
    //DATA_ALARM.alarm1.15	BOOL
    //DATA_ALARM.alarm2.0	BOOL    1号位A3阀打开超时报警
    //DATA_ALARM.alarm2.1	BOOL    2号位A3阀打开超时报警
    //DATA_ALARM.alarm2.2	BOOL    3号位A3阀打开超时报警
    //DATA_ALARM.alarm2.3	BOOL    4号位A3阀打开超时报警
    //DATA_ALARM.alarm2.4	BOOL    5号位A3阀打开超时报警
    //DATA_ALARM.alarm2.5	BOOL    6号位A3阀打开超时报警
    //DATA_ALARM.alarm2.6	BOOL    7号位A3阀打开超时报警
    //DATA_ALARM.alarm2.7	BOOL    8号位A3阀打开超时报警
    //DATA_ALARM.alarm2.8	BOOL    9号位A3阀打开超时报警
    //DATA_ALARM.alarm2.9	BOOL    1号位A3阀关闭超时报警
    //DATA_ALARM.alarm2.10	BOOL    2号位A3阀关闭超时报警
    //DATA_ALARM.alarm2.11	BOOL    3号位A3阀关闭超时报警
    //DATA_ALARM.alarm2.12	BOOL    4号位A3阀关闭超时报警
    //DATA_ALARM.alarm2.13	BOOL    5号位A3阀关闭超时报警
    //DATA_ALARM.alarm2.14	BOOL    6号位A3阀关闭超时报警
    //DATA_ALARM.alarm2.15	BOOL    7号位A3阀关闭超时报警
    //DATA_ALARM.alarm3.0	BOOL    8号位A3阀关闭超时报警
    //DATA_ALARM.alarm3.1	BOOL    9号位A3阀关闭超时报警
    //DATA_ALARM.alarm3.2	BOOL    1号位A6阀打开超时报警
    //DATA_ALARM.alarm3.3	BOOL    2号位A6阀打开超时报警
    //DATA_ALARM.alarm3.4	BOOL    3号位A6阀打开超时报警
    //DATA_ALARM.alarm3.5	BOOL    4号位A6阀打开超时报警
    //DATA_ALARM.alarm3.6	BOOL    5号位A6阀打开超时报警
    //DATA_ALARM.alarm3.7	BOOL    6号位A6阀打开超时报警
    //DATA_ALARM.alarm3.8	BOOL    7号位A6阀打开超时报警
    //DATA_ALARM.alarm3.9	BOOL    8号位A6阀打开超时报警
    //DATA_ALARM.alarm3.10	BOOL    9号位A6阀打开超时报警
    //DATA_ALARM.alarm3.11	BOOL    1号位A6阀关闭超时报警
    //DATA_ALARM.alarm3.12	BOOL    2号位A6阀关闭超时报警
    //DATA_ALARM.alarm3.13	BOOL    3号位A6阀关闭超时报警
    //DATA_ALARM.alarm3.14	BOOL    4号位A6阀关闭超时报警
    //DATA_ALARM.alarm3.15	BOOL    5号位A6阀关闭超时报警
    //DATA_ALARM.alarm4.0	BOOL    6号位A6阀关闭超时报警
    //DATA_ALARM.alarm4.1	BOOL    7号位A6阀关闭超时报警
    //DATA_ALARM.alarm4.2	BOOL    8号位A6阀关闭超时报警
    //DATA_ALARM.alarm4.3	BOOL    9号位A6阀关闭超时报警
    //DATA_ALARM.alarm4.4	BOOL
    //DATA_ALARM.alarm4.5	BOOL 程序报错
    //DATA_ALARM.alarm4.6	BOOL
    //DATA_ALARM.alarm4.7	BOOL
    //DATA_ALARM.alarm4.8	BOOL 升降传感器故障
    //DATA_ALARM.alarm4.9	BOOL
    //DATA_ALARM.alarm4.10	BOOL 舱门传感器故障
    //DATA_ALARM.alarm4.11	BOOL
    //DATA_ALARM.alarm4.12	BOOL 旋转位传感器故障
    //DATA_ALARM.alarm4.13	BOOL 夹爪开传感器故障
    //DATA_ALARM.alarm4.14	BOOL 夹爪关传感器故障
    //DATA_ALARM.alarm4.15	BOOL
    //DATA_ALARM.alarm4.0	BOOL 请先升起夹爪
    //DATA_ALARM.alarm4.1	BOOL 请先运行密封舱
    //DATA_ALARM.alarm4.2	BOOL 请先旋转到工位
    //DATA_ALARM.alarm4.3	BOOL 请确认瓶子到岗
    //DATA_ALARM.alarm4.4	BOOL
    //DATA_ALARM.alarm4.5	BOOL
    //DATA_ALARM.alarm4.6	BOOL
    //DATA_ALARM.alarm4.7	BOOL
    //DATA_ALARM.alarm4.8	BOOL 收发电机位置超限
    //DATA_ALARM.alarm4.9	BOOL 收发电机运行位置超限
    //DATA_ALARM.alarm4.10	BOOL 保护位置起点设置错误
    //DATA_ALARM.alarm4.11	BOOL 保护位置始点设置错误
    //DATA_ALARM.alarm4.12	BOOL 密封舱位置设置错误
    //DATA_ALARM.alarm4.13	BOOL
    //DATA_ALARM.alarm4.14	BOOL
    //DATA_ALARM.alarm4.15	BOOL
    //DATA_ALARM.alarm6.0	BOOL 通讯报警1备用
    //DATA_ALARM.alarm6.1	BOOL 通讯报警2备用
    //DATA_ALARM.alarm6.2	BOOL 通讯报警3备用
    //DATA_ALARM.alarm6.3	BOOL 通讯报警4备用
    //DATA_ALARM.alarm6.4	BOOL 通讯报警5备用
    //DATA_ALARM.alarm6.5	BOOL 通讯报警6备用
    //DATA_ALARM.alarm6.6	BOOL 通讯报警7备用
    //DATA_ALARM.alarm6.7	BOOL 通讯报警8备用
    //DATA_ALARM.alarm6.8	BOOL 通讯报警9备用
    //DATA_ALARM.alarm6.9	BOOL 通讯报警10备用
    //DATA_ALARM.alarm6.10	BOOL 通讯报警11备用
    //DATA_ALARM.alarm6.11	BOOL 通讯报警12备用
    //DATA_ALARM.alarm6.12	BOOL 通讯报警13备用
    //DATA_ALARM.alarm6.13	BOOL 通讯报警14备用
    //DATA_ALARM.alarm6.14	BOOL 通讯报警15备用
    //DATA_ALARM.alarm6.15	BOOL 通讯报警16备用

}
