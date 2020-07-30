using System;
using System.Collections;
using System.Threading;
using TwinCAT.Ads;

namespace TwAdsClass
{
    public class TwcAds
    {
        private static TcAdsClient _client = null;
        private int _handBool = 0;
        private ArrayList notificationHandles;
        private bool connectFlag = false;
        private string msg = "";
        delegate void SetTextCallback(string text);
        Thread tr;
        PlcVariableClass pvc;
        private bool isDiffComputer = false;
        private bool isRegistered = false;

        public bool ConnectFlag
        {
            get
            {
                return connectFlag;
            }

            set
            {
                connectFlag = value;
            }
        }

        public string Msg
        {
            get
            {
                return msg;
            }

            set
            {
                msg = value;
            }
        }

        /// <summary>
        /// ADS通讯连接
        /// </summary>
        /// <param name="IpAddress">IP地址</param>
        /// <param name="Port">端口</param>
        /// <param name="Pvc">基础地址数据</param>
        /// <param name="IsDiffComputer">是否不在相同电脑上</param>
        public TwcAds(string IpAddress, int Port, PlcVariableClass Pvc, bool IsDiffComputer)
        {
            try
            {
                pvc = Pvc;
                isDiffComputer = IsDiffComputer;
                _client = new TcAdsClient();
                notificationHandles = new ArrayList();
                _client.AdsNotificationEx += new AdsNotificationExEventHandler(adsClient_AdsNotificationEx);
                if (isDiffComputer)
                    _client.Connect(new AmsNetId(IpAddress), Port);//不同电脑
                else
                    _client.Connect(851);//同一电脑

                _handBool = _client.CreateVariableHandle(pvc.Test);
                //测试PLC连接并注册
                tr = new Thread(() =>
                {
                    while (true)
                    {
                        this.TestConnect();
                        if (ConnectFlag && !isRegistered)
                        {
                            RegNotificationHandles();
                        }
                        Thread.Sleep(10);
                    }
                });
                tr.IsBackground = true;
                tr.Start();
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                ConnectFlag = false;
                return;
            }
        }
        /// <summary>
        /// 注册notificationHandles
        /// </summary>
        private void RegNotificationHandles()
        {

            notificationHandles.Clear();
            try
            {
                isRegistered = true;
                for (int i = 0; i < pvc.Address.Length; i++)
                {
                    string address = pvc.Address[i];
                    if (pvc.Type[i].ToUpper() == "BOOL")
                        pvc.HandId[i] = _client.AddDeviceNotificationEx(address, AdsTransMode.OnChange, 100, 0, this, typeof(Boolean));
                    if (pvc.Type[i].ToUpper() == "BYTE")
                        pvc.HandId[i] = _client.AddDeviceNotificationEx(address, AdsTransMode.OnChange, 100, 0, this, typeof(byte));
                    if (pvc.Type[i].ToUpper() == "INT")
                        pvc.HandId[i] = _client.AddDeviceNotificationEx(address, AdsTransMode.OnChange, 100, 0, this, typeof(short));
                    if (pvc.Type[i].ToUpper() == "ARRAY")
                        pvc.HandId[i] = _client.AddDeviceNotificationEx(address, AdsTransMode.OnChange, 100, 0, this, typeof(short[]), new int[] { 4 });
                    if (pvc.Type[i].ToUpper() == "REAL")
                        pvc.HandId[i] = _client.AddDeviceNotificationEx(address, AdsTransMode.OnChange, 100, 0, this, typeof(double));
                    if (pvc.Type[i] == "String")
                        pvc.HandId[i] = _client.AddDeviceNotificationEx(address, AdsTransMode.OnChange, 100, 0, this, typeof(String), new int[] { 80 });
                    if (pvc.Type[i] == "Struct1")
                        pvc.HandId[i] = _client.AddDeviceNotificationEx(address, AdsTransMode.OnChange, 100, 0, this, typeof(Struct1));
                    if (pvc.Type[i] == "Struct2")
                        pvc.HandId[i] = _client.AddDeviceNotificationEx(address, AdsTransMode.OnChange, 100, 0, this, typeof(Struct2));
                    if (pvc.Type[i] == "Struct3")
                        pvc.HandId[i] = _client.AddDeviceNotificationEx(address, AdsTransMode.OnChange, 100, 0, this, typeof(Struct3));
                    if (pvc.Type[i] == "Struct4")
                        pvc.HandId[i] = _client.AddDeviceNotificationEx(address, AdsTransMode.OnChange, 100, 0, this, typeof(Struct4));
                    if (pvc.Type[i] == "Struct5")
                        pvc.HandId[i] = _client.AddDeviceNotificationEx(address, AdsTransMode.OnChange, 100, 0, this, typeof(Struct5));

                    if (pvc.Type[i].ToUpper() == "WORD")
                        pvc.HandId[i] = _client.AddDeviceNotificationEx(address, AdsTransMode.OnChange, 100, 0, this, typeof(ushort));
                    if (pvc.Type[i].ToUpper() == "ERROR_DATE")
                        pvc.HandId[i] = _client.AddDeviceNotificationEx(address, AdsTransMode.OnChange, 100, 0, this, typeof(Error_data));
                    if (pvc.Type[i].ToUpper() == "TEAM_DATE")
                        pvc.HandId[i] = _client.AddDeviceNotificationEx(address, AdsTransMode.OnChange, 100, 0, this, typeof(team_date));
                    
                    notificationHandles.Add(pvc.HandId[i]);
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message+"..1";
            }
        }
        public bool ConnectClose()
        {
            bool bl = false;
            try
            {
                if (tr != null)
                {
                    tr.Abort();
                }
                try
                {
                    foreach (int handle in notificationHandles)
                        _client.DeleteDeviceNotification(handle);
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                }
                notificationHandles.Clear();
                _client.Disconnect();
                ConnectFlag = false;
                bl = true;
            }
            catch
            {
                bl = false;
            }
            return bl;
        }
        /// <summary>
        /// 析构函数
        /// </summary>
        ~TwcAds()
        {
            try
            {
                foreach (int handle in notificationHandles)
                    _client.DeleteDeviceNotification(handle);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            notificationHandles.Clear();
        }

        /// <summary>
        /// 测试PLC连接
        /// </summary>
        private void TestConnect()
        {
            try
            {
               ConnectFlag = (bool)_client.ReadAny(_handBool, typeof(Boolean));
            }
            catch
            {
                ConnectFlag = false;
            }
        }

        /// <summary>
        /// 读取PLC信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void adsClient_AdsNotificationEx(object sender, AdsNotificationExEventArgs e)
        {
            int i = Array.IndexOf(pvc.HandId, e.NotificationHandle);
            if (i >= 0 && i < pvc.value.Length)
            {
                pvc.value[i] = e.Value;
            }
        }

        /// <summary>
        /// 写入PLC
        /// </summary>
        /// <param name="hand"></param>
        /// <param name="data"></param>
        public void WriterData(string hand, object data)
        {
            try
            {
                int _hand = 0;
                _hand = _client.CreateVariableHandle(hand);
                _client.WriteAny(_hand, data);
            }
            catch (Exception ex)
            {
                msg ="写入数据失败！原因：" +ex.Message;
                return;
            }
        }
    }
}
