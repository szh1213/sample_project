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
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ip">连接服务器的IP</param>
        /// <param name="port">连接服务器的端口</param>
        public socket2unity(string ip, int port)
        {
            this._ip = ip;
            this._port = port;
            init();
        }
        public socket2unity(int port)
        {
            this._ip = "127.0.0.1";
            this._port = port;
            init();
        }

        /// <summary>
        /// 开启服务,连接服务端
        /// </summary>
        private void init()
        {
            try
            {
                //等待服务端加载 
                Thread.Sleep(2000);
                //1.0 实例化套接字(IP4寻址地址,流式传输,TCP协议)
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //2.0 创建IP对象
                IPAddress address = IPAddress.Parse(_ip);
                //3.0 创建网络端口包括ip和端口
                IPEndPoint endPoint = new IPEndPoint(address, _port);
                //4.0 建立连接
                _socket.Connect(endPoint);
                MessageBox.Show("连接服务器成功");
                //5.0 接收数据
                //int length = _socket.Receive(buffer);
                //Console.WriteLine("接收服务器{0},消息:{1}", _socket.RemoteEndPoint.ToString(), Encoding.UTF8.GetString(buffer, 0, length));
                //6.0 像服务器发送消息
                alive = true;
                
            }
            catch (Exception ex)
            {
                if(_socket.Connected)
                    _socket.Shutdown(SocketShutdown.Both);
                _socket.Close();
                alive = false;
                MessageBox.Show(ex.Message);
            }
        }
        public void send(byte[] sendMessage)
        {
            if(alive)
            _socket.Send(sendMessage);
        }
        public void destory()
        {
            if (_socket.Connected)
                _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
            alive = false;
        }
    }
}
