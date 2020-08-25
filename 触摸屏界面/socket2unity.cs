using System;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace 触摸屏界面
{
    class socket2unity
    {
        private string _ip = string.Empty;
        private int _port = 0;
        private Socket _socket = null;
        private byte[] buffer = new byte[1024 * 1024 * 2];
        public bool alive = false;
        public Byte[,] testMes=new Byte[600,24];
        public int test_index = 0;
        public int test_d = 1;
        public int restart = 0;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ip">连接服务器的IP</param>
        /// <param name="port">连接服务器的端口</param>
        public socket2unity(string ip, int port)
        {
            this._ip = ip;
            this._port = port;
            Thread initThread = new Thread(init);
            initThread.Start();
        }
        public socket2unity(int port)
        {
            this._ip = "127.0.0.1";
            this._port = port;
            Thread initThread = new Thread(init);
            initThread.Start();
        }

        /// <summary>
        /// 开启服务,连接服务端
        /// </summary>
        public void init()
        {
            try
            {
                //1.0 实例化套接字(IP4寻址地址,流式传输,TCP协议)
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //2.0 创建IP对象
                IPAddress address = IPAddress.Parse(_ip);
                //3.0 创建网络端口包括ip和端口
                IPEndPoint endPoint = new IPEndPoint(address, _port);
                //4.0 建立连接
                _socket.Connect(endPoint);
                //5.0 接收数据
                //int length = _socket.Receive(buffer);
                //Console.WriteLine("接收服务器{0},消息:{1}", _socket.RemoteEndPoint.ToString(), Encoding.UTF8.GetString(buffer, 0, length));
                //等待服务端加载 
                Thread.Sleep(2000);
                alive = true;
                restart += 1;

            }
            catch (Exception ex)
            {
                destory();
                //MessageBox.Show(ex.Message);
            }
            for (int i = 0; i < 100; i++)
            {
                Single[] data = new Single[6];
                data[0] = -0.05f + 0.0005f * (i + 1);
                data[1] = 1.8f * (i + 1);
                data[2] = (i + 1) * 0.001f;
                data[3] = -0.01f;
                data[4] = +0.01f;
                data[5] = 0.01f;
                for (int ii = 0; ii < 6; ii++)
                {
                    byte[] tmp = BitConverter.GetBytes(data[ii]);
                    for (int j = 0; j < 4; j++)
                    {
                        testMes[i,ii * 4 + j] = tmp[j];
                    }
                }
            }
            for (int i = 0; i < 100; i++)
            {
                Single[] data = new Single[6];
                data[0] = 0f;
                data[1] = 180;
                data[2] = 0.1f - (i + 1) * 0.001f;
                data[3] = -0.01f;
                data[4] = +0.01f;
                data[5] = 0.01f;
                for (int ii = 0; ii < 6; ii++)
                {
                    byte[] tmp = BitConverter.GetBytes(data[ii]);
                    for (int j = 0; j < 4; j++)
                    {
                        testMes[i + 100, ii * 4 + j] = tmp[j];
                    }
                }
            }
            for (int i = 0; i < 100; i++)
            {
                Single[] data = new Single[6];
                data[0] = 0f;
                data[1] = 180;
                data[2] = (i + 1) * 0.001f;
                data[3] = 0f;
                data[4] = 0f;
                data[5] = 0f;
                for (int ii = 0; ii < 6; ii++)
                {
                    byte[] tmp = BitConverter.GetBytes(data[ii]);
                    for (int j = 0; j < 4; j++)
                    {
                        testMes[i + 200, ii * 4 + j] = tmp[j];
                    }
                }
            }
            for (int i = 0; i < 100; i++)
            {
                Single[] data = new Single[6];
                data[0] = 0f;
                data[1] = 180 - 1.8f * (i + 1);
                data[2] = 0.1f;
                data[3] = 0f;
                data[4] = 0f;
                data[5] = 0f;
                for (int ii = 0; ii < 6; ii++)
                {
                    byte[] tmp = BitConverter.GetBytes(data[ii]);
                    for (int j = 0; j < 4; j++)
                    {
                        testMes[i+300, ii * 4 + j] = tmp[j];
                    }
                }
            }
            for (int i = 0; i < 100; i++)
            {
                Single[] data = new Single[6];
                data[0] = 0f;
                data[1] = 0f;
                data[2] = 0.1f - (i + 1) * 0.001f;
                data[3] = 0f;
                data[4] = 0f;
                data[5] = 0f;
                for (int ii = 0; ii < 6; ii++)
                {
                    byte[] tmp = BitConverter.GetBytes(data[ii]);
                    for (int j = 0; j < 4; j++)
                    {
                        testMes[i+400, ii * 4 + j] = tmp[j];
                    }
                }
            }
            for (int i = 0; i < 100; i++)
            {
                Single[] data = new Single[6];
                data[0] = 0f;
                data[1] = 0f;
                data[2] = (i + 1) * 0.001f;
                data[3] = -0.01f;
                data[4] = 0.01f;
                data[5] = 0f;
                for (int ii = 0; ii < 6; ii++)
                {
                    byte[] tmp = BitConverter.GetBytes(data[ii]);
                    for (int j = 0; j < 4; j++)
                    {
                        testMes[i + 500, ii * 4 + j] = tmp[j];
                    }
                }
            }
        }
        public void send(byte[] sendMessage)
        {
            if (alive)
            {
                try { _socket.Send(sendMessage); }
                catch { destory(); }
            }
        }
        public void destory()
        {
                _socket.Close();
                alive = false;
        }
    }
}
