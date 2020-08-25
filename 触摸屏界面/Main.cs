using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.SuperGrid;
using TwAdsClass;
using DevComponents.DotNetBar.Controls;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml;
using System.IO;

namespace 触摸屏界面
{
    
    public partial class Main : Office2007Form
    {
        private static DateTime START_TIME;
        private TwcAds tas;
        PlcVariableClass pvc = new PlcVariableClass();
        Size _beforeDialogSize;
        private string[][] error_hash = new string[6][];
        private double t = 0.00001;
        private double speed = 0;
        private double torque = 0;
        string[] titles = { "位置显示", "速度显示", "力矩显示" };
        MouseHook mh;
        Point downPos, upPos;
        
        private socket2unity mes_socket;
        private byte[] buffer = new byte[1024 * 1024 * 2];
        private showEXE unityEXE;
        private bool unityInited=false;

        private string[] zeroName = new string[3];
        private Single[] motorZero = new Single[] { 0, 0, 0 };
        
        private motorValue motor_value = new motorValue();
        public Main()
        {
            InitializeComponent();
            zeroName[0] = textBox1.Name;
            zeroName[1] = textBox2.Name;
            zeroName[2] = textBox3.Name;
            closePc(true);
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load("PLC.config");
                XmlElement root = doc.DocumentElement;
                XmlNode exeNode = root.SelectSingleNode("/PLC/exe");
                string exe = exeNode.Attributes["address"].Value;

                unityEXE = new showEXE(exe, superTabControlPanel8, 3000);
                //mes_socket = new socket2unity(8888);

                XmlNode tmp = root.SelectSingleNode("/PLC/motor1zero");
                textBox1.Text = tmp.Attributes["value"].Value;
                motorZero[0] = Convert.ToSingle(tmp.Attributes["value"].Value);
                tmp = root.SelectSingleNode("/PLC/motor2zero");
                textBox2.Text = tmp.Attributes["value"].Value;
                motorZero[1] = Convert.ToSingle(tmp.Attributes["value"].Value);
                tmp = root.SelectSingleNode("/PLC/motor3zero");
                textBox3.Text = tmp.Attributes["value"].Value;
                motorZero[2] = Convert.ToSingle(tmp.Attributes["value"].Value);

            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            mh = new MouseHook();
            mh.SetHook();
            mh.MouseDownEvent += mh_MouseDownEvent;
            mh.MouseUpEvent += mh_MouseUpEvent;
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load("PLC.config");
                XmlElement root = doc.DocumentElement;
                XmlNodeList alarmNodes = root.SelectNodes("/PLC/alarm1");

                for (int i = 0; i < 6; i++)
                {
                    XmlNodeList nl = root.SelectNodes("/PLC/alarm" + (i + 1).ToString());
                    error_hash[i] = new string[16];
                    foreach (XmlNode node in nl)
                    {
                        error_hash[i][Convert.ToInt16(node.Attributes["index"].Value)] = node.Attributes["error"].Value;
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message,"配置文件错误");
            }
            START_TIME = DateTime.Now;
        }
        //按下鼠标键触发的事件
        private void mh_MouseDownEvent(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                downPos = e.Location;
            }
            if (e.Button == MouseButtons.Right)
            {

            }
        }
        //松开鼠标键触发的事件
        private void mh_MouseUpEvent(object sender, MouseEventArgs e)
        {
            upPos = e.Location;
            if (e.Button == MouseButtons.Left)
            {
                int detaY = upPos.Y - downPos.Y;
                int detaX = upPos.X - downPos.X;
                if (Math.Abs(detaX) > 200)
                {
                    if (detaX > 0 && Math.Abs(detaY)/Math.Abs(detaX)<0.5)
                    {
                        //superTabControl1.SelectedTabIndex -= superTabControl1.SelectedTabIndex > 0 ? 1 : 0;
                    }
                    if (detaX < 0 && Math.Abs(detaY) / Math.Abs(detaX) < 0.5)
                    {
                        //superTabControl1.SelectedTabIndex += superTabControl1.SelectedTabIndex < 7 ? 1 : 0;
                    }
                }
            }
            if (e.Button == MouseButtons.Right)
            {
              
            }
         
        }

        #region 界面，缩放
        private void superTabItem1_Click(object sender, EventArgs e)
        {
            panelEx2.Text = "自动控制";
        }

        private void superTabItem2_Click(object sender, EventArgs e)
        {
            panelEx2.Text = "手动控制-点动";
        }

        private void superTabItem3_Click(object sender, EventArgs e)
        {
            panelEx2.Text = "阀门操作";
        }

        private void superTabItem4_Click(object sender, EventArgs e)
        {
            panelEx2.Text = "参数设置";
        }

        private void superTabItem5_Click(object sender, EventArgs e)
        {
            panelEx2.Text = "取样队列";
        }

        private void superTabItem6_Click(object sender, EventArgs e)
        {
            panelEx2.Text = "IO点检测输入";
        }

        private void superTabItem7_Click(object sender, EventArgs e)
        {
            panelEx2.Text = "报警查看";
        }

        private void bubbleButton1_Click(object sender, ClickEventArgs e)
        {
            superTabControl1.SelectedTabIndex = 0;
        }

        private void bubbleButton2_Click(object sender, ClickEventArgs e)
        {
            superTabControl1.SelectedTabIndex = 1;
        }

        private void bubbleButton3_Click(object sender, ClickEventArgs e)
        {
            superTabControl1.SelectedTabIndex = 2;
        }
        private void bubbleButton4_Click(object sender, ClickEventArgs e)
        {
            superTabControl1.SelectedTabIndex = 3;
        }

        private void bubbleButton5_Click(object sender, ClickEventArgs e)
        {
            superTabControl1.SelectedTabIndex = 4;
        }

        private void bubbleButton6_Click(object sender, ClickEventArgs e)
        {
            superTabControl1.SelectedTabIndex = 5;
        }
        private void bubbleButton7_Click(object sender, ClickEventArgs e)
        {
            superTabControl1.SelectedTabIndex = 6;
        }
        private void bubbleButton8_Click(object sender, ClickEventArgs e)
        {
            superTabControl1.SelectedTabIndex = 7;
        }


        protected override void OnResizeEnd(EventArgs e)
        {

            base.OnResizeEnd(e);
            Size endSize = this.Size;
            float percentWidth = (float)endSize.Width / _beforeDialogSize.Width;
            float percentHeight = (float)endSize.Height / _beforeDialogSize.Height;
            unityEXE.ResizeControl(unityEXE.p);
            foreach (Control control in this.superTabControlPanel1.Controls)
            {
                if (control is DataGridView)
                    continue;
                //按比例改变控件大小
                control.Width = (int)(control.Width * percentWidth);
                control.Height = (int)(control.Height * percentHeight);
                control.Font = new Font(control.Font.Name, control.Font.Size * percentWidth);

                //为了不使控件之间覆盖 位置也要按比例变化
                control.Left = (int)(control.Left * percentWidth);
                control.Top = (int)(control.Top * percentHeight);
            }
            foreach (Control panelex in this.superTabControlPanel2.Controls)
            {
                if (!panelex.Name.StartsWith("panelEx")) continue;
                //panelex.Width = (int)(panelex.Width * percentWidth);
                //panelex.Height = (int)(panelex.Height * percentHeight);

                ////为了不使控件之间覆盖 位置也要按比例变化
                //panelex.Left = (int)(panelex.Left * percentWidth);
                //panelex.Top = (int)(panelex.Top * percentHeight);
                foreach (Control control in panelex.Controls)
                {
                    //按比例改变控件大小
                    control.Width = (int)(control.Width * percentWidth);
                    control.Height = (int)(control.Height * percentHeight);
                    control.Font = new Font(control.Font.Name, control.Font.Size * percentWidth);
                    //为了不使控件之间覆盖 位置也要按比例变化
                    control.Left = (int)(control.Left * percentWidth);
                    control.Top = (int)(control.Top * percentHeight);
                }
                
                
            }
            foreach (Control control in this.superTabControlPanel3.Controls)
            {
                if (control is DataGridView)
                    continue;
                //按比例改变控件大小
                control.Width = (int)(control.Width * percentWidth);
                control.Height = (int)(control.Height * percentHeight);
                control.Font = new Font(control.Font.Name, control.Font.Size * percentWidth);

                //为了不使控件之间覆盖 位置也要按比例变化
                control.Left = (int)(control.Left * percentWidth);
                control.Top = (int)(control.Top * percentHeight);
            }
            foreach (Control control in this.superTabControlPanel4.Controls)
            {
                if (control is DataGridView)
                    continue;
                //按比例改变控件大小
                control.Width = (int)(control.Width * percentWidth);
                control.Height = (int)(control.Height * percentHeight);
                control.Font = new Font(control.Font.Name, control.Font.Size * percentWidth);

                //为了不使控件之间覆盖 位置也要按比例变化
                control.Left = (int)(control.Left * percentWidth);
                control.Top = (int)(control.Top * percentHeight);
            }

            _beforeDialogSize = this.Size;
        }

        #endregion

        #region 功能
        /// <summary>
        /// 计时器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                //日期显示
                lb_data.Text = System.DateTime.Now.ToLongDateString() + " " + System.DateTime.Now.ToLongTimeString();

                //显示连接状态
                if (tas.ConnectFlag)
                {
                    lb_connectflag.Text = "PLC已连接";
                    lb_connectflag.ForeColor = Color.Blue;
                }
                else
                {
                    lb_connectflag.Text = "PLC未连接";
                    lb_connectflag.ForeColor = Color.Red;
                }

                //PLC反馈数据显示
                ShowData();

            }
            catch
            {

            }
        }


        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form2_Load(object sender, EventArgs e)
        {

            try
            {
                _beforeDialogSize = this.Size;

                //地址初始化
                Init();

                InitChart();
                //显示曲线

            }
            catch(Exception r)
            {
                MessageBoxEx.Show("窗体加载失败！"+r.Message,"失败");
            }

        }
        private void open_plc_Click(object sender, EventArgs e)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("PLC.config");
            XmlElement root = doc.DocumentElement;
            XmlNode ipNode = root.SelectSingleNode("/PLC/ip");
            string ip = ipNode.Attributes["address"].Value;

            tas = new TwcAds(ip, 851, pvc, true);
            tas.ConnectFlag = false;
            this.open_plc.Enabled = false;
            this.close_plc.Enabled = true;
        }

        private void close_plc_Click(object sender, EventArgs e)
        {
            if (!tas.ConnectClose())
            {
                MessageBoxEx.Show("关闭PLC连接失败！", "通讯失败");
                return;
            }
            this.open_plc.Enabled = true;
            this.close_plc.Enabled = false;
        }

        /// <summary>
        /// 程序关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            try { mes_socket.destory(); }
            catch {  }
            
            if (!Directory.Exists("D:/QY43DATA"))
            {
                Directory.CreateDirectory("D:/QY43DATA");
            }
            string name = START_TIME.ToString("s") + "__" + DateTime.Now.ToString("s");
            name = name.Replace("-", string.Empty);
            name = name.Replace(":", string.Empty);
            try
            {
                motor_value.save("D:/svgdata/"+name + ".csv");
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message,name);
            }
            Application.Exit();
        }

        #endregion

        private void pvcValueInit(int i)
        {
            if (pvc.Type[i].ToUpper() == "BOOL")
                pvc.value[i] = false;
            if (pvc.Type[i].ToUpper() == "BYTE")
                pvc.value[i] = 0;
            if (pvc.Type[i].ToUpper() == "INT")
                pvc.value[i] = 0;
            if (pvc.Type[i].ToUpper() == "ARRAY")
                pvc.value[i] = new int[] { 4 };
            if (pvc.Type[i].ToUpper() == "REAL")
                pvc.value[i] = 0;

            if (pvc.Type[i].ToUpper() == "WORD")
                pvc.value[i] = 0;
            if (pvc.Type[i].ToUpper() == "ERROR_DATA")
            {
                Error_data tmp = new Error_data();
                tmp.alarm = new ushort[6] { 0,0,0,0,0,0};
                pvc.value[i] = tmp;
            }
            if (pvc.Type[i].ToUpper() == "TEAM_DATE")
            {
                team_date tmp = new team_date();
                tmp.DATA_BATCH = new short[20];
                tmp.DATA_NUMBER = new short[20];
                tmp.DATA_STATION = new short[20];
                tmp.DATA_TEAM = new short[20];
                for(int j = 0; j < 20; j++)
                {
                    tmp.DATA_BATCH[j] = 0;
                    tmp.DATA_NUMBER[j] = 0;
                    tmp.DATA_STATION[j] = 0;
                    tmp.DATA_TEAM[j] = 0;
                }
                pvc.value[i] = tmp;
            }

        }
        /// <summary>
        /// 地址初始化
        /// </summary>
        private void Init()
        {
            
            try
            {
                int i=0;
                int I_AddressCount = 300;
                pvc.Test = "HMI.Test";
                pvc.ControlsName = new string[I_AddressCount];
                pvc.Address = new string[I_AddressCount];
                pvc.Type = new string[I_AddressCount];
                pvc.HandId = new int[I_AddressCount];
                pvc.value = new object[I_AddressCount];

                foreach (Control control in this.superTabControlPanel1.Controls)
                {
                    if (control.Tag != null && !string.IsNullOrEmpty(control.Tag.ToString()))
                    {
                        pvc.ControlsName[i] = control.Name;
                        pvc.Address[i] = control.Tag.ToString().Split(';')[0];
                        
                        pvc.Type[i] = control.Tag.ToString().Split(';')[1];pvcValueInit(i);
                        i++;
                    }
                }
                foreach (Control panelex in this.superTabControlPanel2.Controls)
                {
                    if (!panelex.Name.StartsWith("panelEx")) continue;
                    foreach (Control control in panelex.Controls)
                    {
                        if (control.Tag != null && !string.IsNullOrEmpty(control.Tag.ToString()))
                        {

                            pvc.ControlsName[i] = control.Name;
                            pvc.Address[i] = control.Tag.ToString().Split(';')[0];
                            
                            pvc.Type[i] = control.Tag.ToString().Split(';')[1];pvcValueInit(i);
                            i++;
                        }
                    }
                }
                foreach (Control control in this.superTabControlPanel3.Controls)
                {
                    if (control.Tag != null && !string.IsNullOrEmpty(control.Tag.ToString()))
                    {
                        pvc.ControlsName[i] = control.Name;
                        pvc.Address[i] = control.Tag.ToString().Split(';')[0];
                        
                        pvc.Type[i] = control.Tag.ToString().Split(';')[1];pvcValueInit(i);
                        i++;
                    }
                }
                foreach (Control control in this.superTabControlPanel4.Controls)
                {
                    if (control.Tag != null && !string.IsNullOrEmpty(control.Tag.ToString()))
                    {
                        pvc.ControlsName[i] = control.Name;
                        pvc.Address[i] = control.Tag.ToString().Split(';')[0];
                        
                        pvc.Type[i] = control.Tag.ToString().Split(';')[1];pvcValueInit(i);
                        i++;
                    }
                }
                foreach (Control control in this.superTabControlPanel5.Controls)
                {
                    if (control.Tag != null && !string.IsNullOrEmpty(control.Tag.ToString()))
                    {
                        pvc.ControlsName[i] = control.Name;
                        pvc.Address[i] = control.Tag.ToString().Split(';')[0];
                        
                        pvc.Type[i] = control.Tag.ToString().Split(';')[1];pvcValueInit(i);
                        i++;
                    }
                }
                foreach (Control control in this.superTabControlPanel6.Controls)
                {
                    if (control.Tag != null && !string.IsNullOrEmpty(control.Tag.ToString()))
                    {
                        pvc.ControlsName[i] = control.Name;
                        pvc.Address[i] = control.Tag.ToString().Split(';')[0];
                        
                        pvc.Type[i] = control.Tag.ToString().Split(';')[1];pvcValueInit(i);
                        i++;
                    }
                }
                foreach (Control control in this.superTabControlPanel7.Controls)
                {
                    if (control.Tag != null && !string.IsNullOrEmpty(control.Tag.ToString()))
                    {
                        pvc.ControlsName[i] = control.Name;
                        pvc.Address[i] = control.Tag.ToString().Split(';')[0];
                        
                        pvc.Type[i] = control.Tag.ToString().Split(';')[1];pvcValueInit(i);
                        i++;
                    }
                }
                foreach (Control control in this.panelEx2.Controls)
                {
                    if (control.Tag != null && !string.IsNullOrEmpty(control.Tag.ToString()))
                    {
                        pvc.ControlsName[i] = control.Name;
                        pvc.Address[i] = control.Tag.ToString().Split(';')[0];
                        
                        pvc.Type[i] = control.Tag.ToString().Split(';')[1];pvcValueInit(i);
                        i++;
                    }
                }

            }
            catch(Exception rx)
            {
                MessageBoxEx.Show("地址初始化异常！", rx.Message+"异常");
            }
        }

        /// <summary>
        /// PLC反馈数据显示
        /// </summary>
        private void ShowData()
        {
            try
            {
                if (tas.ConnectFlag)
                {
                    foreach (Control control in this.superTabControlPanel1.Controls)
                    {
                        if(control.Name != this.txt_SETP_P.Name)
                        SetValue(control,0);
                    }

                    foreach (Control panelex in this.superTabControlPanel2.Controls)
                    {
                        if (!panelex.Name.StartsWith("panelEx")) continue;
                        foreach (Control control in panelex.Controls)
                        {
                            SetValue(control,0);
                        }
                    }

                    foreach (Control control in this.superTabControlPanel3.Controls)
                    {
                        SetValue(control,0);
                    }
                    foreach (Control control in this.superTabControlPanel4.Controls)
                    {
                        if(Array.IndexOf(zeroName,control.Name)<0)
                        SetValue(control,0);
                    }
                    foreach (Control control in this.superTabControlPanel5.Controls)
                    {
                        SetValue(control,0);
                    }
                    foreach (Control control in this.superTabControlPanel6.Controls)
                    {
                        SetValue(control,0);
                    }
                    foreach (Control control in this.superTabControlPanel7.Controls)
                    {
                        SetValue(control,0);
                    }

                    string[] state = {
                    "准备",
                    "开A6阀",
                    "开A3阀",
                    "空瓶进入",
                    "取出空瓶","闲置空瓶","取样","样瓶抓起","放置样品","送出样品","取样完成","结束","准备"
                    };
                    int index = Array.IndexOf(pvc.ControlsName, this.txt_SETP_P.Name);
                    this.txt_SETP_P.Text = state[Convert.ToInt16(pvc.value[index])];
                    this.textBoxX2.Text = state[Convert.ToInt16(pvc.value[index]) + 1];

                    string[] step = { "运行", "复位", "","停止", "急停","","","","" };
                    index = Array.IndexOf(pvc.ControlsName, this.lb_RUN_SET.Name);
                    this.lb_RUN_SET.Text = step[Convert.ToInt16(pvc.value[index])];
                    this.lb_PA_PC_switch.Text =  (bool) pvc.value[Array.IndexOf(pvc.ControlsName, this.lb_PA_PC_switch.Name)]?"PA":"PC";
                    if (this.lb_PA_PC_switch.Text == "PA")
                    {
                        closePc(true);
                    }else
                    {
                        closePc(false);
                    }
                }
            }
            catch(Exception ex)
            {
                //MessageBox.Show(ex.Message,"读取错误");
            }
        }

        /// <summary>
        /// 是否关闭上位机权限
        /// </summary>
        private void closePc(bool state)
        {
            foreach (Control control in this.superTabControlPanel1.Controls)
            {
                if (control is TextBoxX)
                {
                    (control as TextBoxX).ReadOnly = state;
                }
            }
            foreach (Control panelex in this.superTabControlPanel2.Controls)
            {
                if (!panelex.Name.StartsWith("panelEx")) continue;
                foreach (Control control in panelex.Controls)
                {
                    if (control is TextBoxX)
                    {
                        (control as TextBoxX).ReadOnly = state;
                    }
                    if (control is ButtonX)
                    {
                        (control as ButtonX).Enabled = !state;
                    }
                }
            }
            foreach (Control control in this.superTabControlPanel3.Controls)
            {
                if (control is ButtonX)
                {
                    (control as ButtonX).Enabled = !state;
                }
            }
            foreach (Control control in this.superTabControlPanel4.Controls)
            {
                if (control is TextBoxX)
                {
                    (control as TextBoxX).ReadOnly = state;
                }
            }
            
        }

        private void SetValue(Control control,int deep)
        {
            try
            {
                if (control.Tag != null && !string.IsNullOrEmpty(control.Tag.ToString()))
                {
                    int index = Array.IndexOf(pvc.ControlsName, control.Name);

                    //文本框
                    if (control is DevComponents.DotNetBar.Controls.TextBoxX && !control.Focused)
                    {
                        control.Text = pvc.value[index].ToString();
                    }

                    //指示灯
                    if (control is DevComponents.DotNetBar.Controls.SymbolBox)
                    {

                        bool bl = (bool)pvc.value[index];
                        if (bl)
                            (control as DevComponents.DotNetBar.Controls.SymbolBox).SymbolColor = Color.Lime;
                        else
                            (control as DevComponents.DotNetBar.Controls.SymbolBox).SymbolColor = Color.Red;

                    }

                    //按钮
                    if (control is DevComponents.DotNetBar.ButtonX)
                    {
                        bool bl = (bool)pvc.value[index];
                        if (bl)
                        {
                            control.BackColor = Color.Lime;
                            (control as DevComponents.DotNetBar.ButtonX).ColorTable = eButtonColor.Orange;
                        }
                        else
                        {
                            control.BackColor = Color.Transparent;
                            (control as DevComponents.DotNetBar.ButtonX).ColorTable = eButtonColor.OrangeWithBackground;
                        }
                    }

                    //队列表格
                    if (control is SuperGridControl && control.Name.Contains("1"))
                    {
                        team_date td = new team_date();
                        td = (team_date)pvc.value[index];
                        ShowTable(td);
                    }
                    //报警表格
                    if (control is SuperGridControl && control.Name.Contains("2"))
                    {
                        Error_data ed = new Error_data();

                        ed = (Error_data)pvc.value[index];
                        ShowWarning(ed);
                    }
                }
            }
            catch
            {
            }
        }
        /// <summary>
        /// 显示表格
        /// </summary>
        private void ShowTable(team_date stru)
        {
            try
            {
                this.sgc1.PrimaryGrid.Rows.Clear();
                
                for (int i = 0; i < stru.DATA_TEAM.Length; i++)
                {
                    this.sgc1.PrimaryGrid.InsertRow(i);
                    GridRow gr = this.sgc1.PrimaryGrid.Rows[i] as GridRow;
                    
                    //日期
                    gr[0].Value = stru.DATA_TEAM[i];// "2020-" + (i / 2 + 1).ToString("00") + "-01 12:02:01";
                    //批次
                    gr[1].Value = stru.DATA_BATCH[i];
                    //工位
                    gr[2].Value = stru.DATA_STATION[i];
                    //次数
                    gr[3].Value = stru.DATA_NUMBER[i];
                    //优先级
                    gr[4].Value = i / 5;
                    //插队标识
                    if (i == 10)
                        gr[3].Value = 1;
                    else
                        gr[3].Value = 0;
                }
                this.sgc1.Refresh();
            }
            catch(Exception r)
            {
                //MessageBox.Show(r.Message,"队列读取错误");
            }
        }

        /// <summary>
        /// 显示警报
        /// </summary>
        private void ShowWarning(Error_data stru)
        {
            try
            {
                string errorMsg = "";
                this.sgc2.PrimaryGrid.Rows.Clear();
                for (int i = 0; i < 6; i++)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        if ((stru.alarm[i] & 0x1 << j) != 0)
                        {
                            errorMsg += "\n   " + error_hash[i][j];

                            this.sgc2.PrimaryGrid.InsertRow((int)(this.sgc2.PrimaryGrid.Rows.LongCount()));
                            GridRow gr = this.sgc2.PrimaryGrid.Rows.Last() as GridRow;
                            if(gr[1].Value.ToString()==string.Empty)gr[0].Value = DateTime.Now;
                            gr[1].Value = error_hash[i][j];
                        }
                    }
                }
                
                    this.sgc2.Refresh();
            }
            catch (Exception r)
            {
                //MessageBox.Show(r.Message, "警报读取错误");
            }
        }
        
        private void WriteText(Control control,string aim)
        {
            if (tas.ConnectFlag && control is TextBoxX && control.Tag != null && !string.IsNullOrEmpty(control.Tag.ToString()))
            {
                try
                {
                    int index = Array.IndexOf(pvc.ControlsName, control.Name);
                    if (pvc.Type[index] == "INT")
                        tas.WriterData(pvc.Address[index], (short)Convert.ToInt16(aim));
                    if (pvc.Type[index] == "REAL")
                    {
                        tas.WriterData(pvc.Address[index], (Single)Convert.ToSingle(aim));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "文本内容写入变量错误");
                }
            }else
            {
                int index = Array.IndexOf(zeroName, control.Name);
                motorZero[index] = Convert.ToSingle(aim);
            }
        }
        #region 显示曲线
        /// <summary>
        /// 显示曲线
        /// </summary>

        private void InitChart()
        {
            this.chart_timer.Enabled = true;
            //定义图表区域
            this.chart1.ChartAreas.Clear();
            ChartArea chartArea1 = new ChartArea("C1");
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.ChartAreas[0].BackColor = Color.Black;
            //定义存储和显示点的容器
            this.chart1.Series.Clear();

            Series series1 = new Series("升降");
            series1.ChartArea = "C1";
            this.chart1.Series.Add(series1);
            this.chart1.Series[0].Color = Color.Red;
            this.chart1.Series[0].ChartType = SeriesChartType.StepLine;

            series1 = new Series("旋转");
            series1.ChartArea = "C1";
            this.chart1.Series.Add(series1);
            this.chart1.Series[1].Color = Color.Green;
            this.chart1.Series[1].ChartType = SeriesChartType.StepLine;

            series1 = new Series("收发");
            series1.ChartArea = "C1";
            this.chart1.Series.Add(series1);
            this.chart1.Series[2].Color = Color.Blue;
            this.chart1.Series[2].ChartType = SeriesChartType.StepLine;



            //设置图表显示样式
            //this.chart1.ChartAreas[0].AxisX.ScaleView.Zoom(2, 3);
            this.chart1.ChartAreas[0].CursorX.IsUserEnabled = true;
            this.chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            this.chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            //将滚动内嵌到坐标轴中
            this.chart1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
            
            //设置滚动条的大小
            this.chart1.ChartAreas[0].AxisX.ScrollBar.Size = 10;
            //设置滚动条的按钮的风格，下面代码是将所有滚动条上的按钮都显示出来
            this.chart1.ChartAreas[0].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.All;
            this.chart1.ChartAreas[0].AxisX.ScaleView.SmallScrollMinSize = double.NaN;
            this.chart1.ChartAreas[0].AxisX.ScaleView.SmallScrollMinSize = 2;
            //this.chart1.ChartAreas[0].AxisY.Minimum = -800;
            //this.chart1.ChartAreas[0].AxisY.Maximum = 200;
            //this.chart1.ChartAreas[0].AxisY.Interval = 10;
            this.chart1.ChartAreas[0].AxisX.LineColor = Color.White;
            this.chart1.ChartAreas[0].AxisY.LineColor = Color.White;
            this.chart1.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.White;
            this.chart1.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.White;
            this.chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = System.Drawing.Color.Silver;
            this.chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = System.Drawing.Color.Silver;
            //设置标题
            this.chart1.Titles.Clear();
            this.chart1.Titles.Add("S01");
            this.chart1.Titles[0].Text = "力矩显示";
            this.chart1.Titles[0].ForeColor = Color.RoyalBlue;
            this.chart1.Titles[0].Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);


            //for (int i = 0; i < 3; i++)
            //{
            //    this.chart1.Series[0].Points.AddXY((i + 1), 0);
            //    this.chart1.Series[1].Points.AddXY((i + 1), 0);
            //    this.chart1.Series[2].Points.AddXY((i + 1), 0);
            //}

        }
        /// <summary>
        /// 显示曲线
        /// </summary>
        private void updateChart()
        {
            //DataPoint endp = this.chart1.Series[0].Points.Last();
            //DataPoint startp = this.chart1.Series[0].Points.First();
            if (this.chart1.Series[0].Points.Count >1000)
            {
                this.chart1.Series[0].Points.RemoveAt(0);
                this.chart1.Series[1].Points.RemoveAt(0);
                this.chart1.Series[2].Points.RemoveAt(0);
            }
            
            t = (double)(DateTime.Now.Ticks - START_TIME.Ticks) / 10000/1000;
            speed = 3000 * Math.Sin(t);
            torque = (100 - Math.Max(0, (Math.Abs(speed) - 1440) * 0.02)) * Math.Sign(speed);
            //this.chart1.Series[0].Points.AddXY(t, speed);
            //this.chart1.Series[1].Points.AddXY(t, torque * 10);
            //this.chart1.Series[2].Points.AddXY(t, t*100%3000);
            //this.chart1.ChartAreas[0].AxisX.Maximum = endp.XValue + 50;
            //this.chart1.ChartAreas[0].AxisX.Minimum = startp.XValue - 50;
            TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            try
            {
                motor_value.time.Add(Convert.ToSingle(ts.TotalMilliseconds));
                motor_value.pos[0].Add(Convert.ToSingle(pvc.value[Array.IndexOf(pvc.ControlsName, "txt_AXIS1_REAL_POS")]));
                motor_value.pos[1].Add(Convert.ToSingle(pvc.value[Array.IndexOf(pvc.ControlsName, "txt_AXIS2_REAL_POS")]));
                motor_value.pos[2].Add(Convert.ToSingle(pvc.value[Array.IndexOf(pvc.ControlsName, "txt_AXIS3_REAL_POS")]));
                motor_value.vel[0].Add(Convert.ToSingle(pvc.value[Array.IndexOf(pvc.ControlsName, "txt_AXIS1_REAL_VEL")]));
                motor_value.vel[1].Add(Convert.ToSingle(pvc.value[Array.IndexOf(pvc.ControlsName, "txt_AXIS2_REAL_VEL")]));
                motor_value.vel[2].Add(Convert.ToSingle(pvc.value[Array.IndexOf(pvc.ControlsName, "txt_AXIS3_REAL_VEL")]));
                motor_value.tor[0].Add(Convert.ToSingle(pvc.value[Array.IndexOf(pvc.ControlsName, "txt_AXIS1_REAL_TOR")]));
                motor_value.tor[1].Add(Convert.ToSingle(pvc.value[Array.IndexOf(pvc.ControlsName, "txt_AXIS2_REAL_TOR")]));
                motor_value.tor[2].Add(Convert.ToSingle(pvc.value[Array.IndexOf(pvc.ControlsName, "txt_AXIS3_REAL_TOR")]));

                if (chart1.Titles[0].Text == titles[0])
                {
                    this.chart1.Series[0].Points.AddXY(t, motor_value.pos[0].Last());
                    this.chart1.Series[1].Points.AddXY(t, motor_value.pos[1].Last());
                    this.chart1.Series[2].Points.AddXY(t, motor_value.pos[2].Last());
                }
                if (chart1.Titles[0].Text == titles[1])
                {
                    this.chart1.Series[0].Points.AddXY(t, motor_value.vel[0].Last());
                    this.chart1.Series[1].Points.AddXY(t, motor_value.vel[1].Last());
                    this.chart1.Series[2].Points.AddXY(t, motor_value.vel[2].Last());
                }
                if (chart1.Titles[0].Text == titles[2])
                {
                    this.chart1.Series[0].Points.AddXY(t, motor_value.tor[0].Last());
                    this.chart1.Series[1].Points.AddXY(t, motor_value.tor[1].Last());
                    this.chart1.Series[2].Points.AddXY(t, motor_value.tor[2].Last());
                }
            }
            catch { }
            
        }

        private void chart_timer_Tick(object sender, EventArgs e)
        {
            updateChart();
        }
        #endregion
        
        private void Main_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized|| this.WindowState == FormWindowState.Normal)
            {
                OnResizeEnd(e);
            }
        }
        #region 阀门操作
        /// <summary>
        /// 36个阀门的开关操作，点击事件
        /// </summary>
        private void boolChange(Control T,Control F)
        {
            int index = Array.IndexOf(pvc.ControlsName, T.Name);
            tas.WriterData(pvc.Address[index], true);
            
            index = Array.IndexOf(pvc.ControlsName, F.Name);
            tas.WriterData(pvc.Address[index], false);
            
        }
        

        private void btm_A3_OPEN1_Click(object sender, EventArgs e)
        {
            boolChange(btm_A3_OPEN1, btm_A3_CLOSE1);
        }

        private void btm_A3_CLOSE1_Click(object sender, EventArgs e)
        {
            boolChange(btm_A3_CLOSE1, btm_A3_OPEN1);
        }

        private void btm_A6_OPEN1_Click(object sender, EventArgs e)
        {
            boolChange(btm_A6_OPEN1, btm_A6_CLOSE1);
        }

        private void btm_A6_CLOSE1_Click(object sender, EventArgs e)
        {
            boolChange(btm_A6_CLOSE1, btm_A6_OPEN1);
        }

        private void btm_A3_OPEN2_Click(object sender, EventArgs e)
        {
            boolChange(btm_A3_OPEN2, btm_A3_CLOSE2);
        }

        private void btm_A3_CLOSE2_Click(object sender, EventArgs e)
        {
            boolChange(btm_A3_CLOSE2, btm_A3_OPEN2);
        }

        private void btm_A6_OPEN2_Click(object sender, EventArgs e)
        {
            boolChange(btm_A6_OPEN2, btm_A6_CLOSE2);
        }

        private void btm_A6_CLOSE2_Click(object sender, EventArgs e)
        {
            boolChange(btm_A6_CLOSE2, btm_A6_OPEN2);
        }

        private void btm_A3_OPEN3_Click(object sender, EventArgs e)
        {
            boolChange(btm_A3_OPEN3, btm_A3_CLOSE3);
        }

        private void btm_A3_CLOSE3_Click(object sender, EventArgs e)
        {
            boolChange(btm_A3_CLOSE3, btm_A3_OPEN3);
        }

        private void btm_A6_OPEN3_Click(object sender, EventArgs e)
        {
            boolChange(btm_A6_OPEN3, btm_A6_CLOSE3);
        }

        private void btm_A6_CLOSE3_Click(object sender, EventArgs e)
        {
            boolChange(btm_A6_CLOSE3, btm_A6_OPEN3);
        }

        private void btm_A3_OPEN4_Click(object sender, EventArgs e)
        {
            boolChange(btm_A3_OPEN4, btm_A3_CLOSE4);
        }

        private void btm_A3_CLOSE4_Click(object sender, EventArgs e)
        {
            boolChange(btm_A3_CLOSE4, btm_A3_OPEN4);
        }

        private void btm_A6_OPEN4_Click(object sender, EventArgs e)
        {
            boolChange(btm_A6_OPEN4, btm_A6_CLOSE4);
        }

        private void btm_A6_CLOSE4_Click(object sender, EventArgs e)
        {
            boolChange(btm_A6_CLOSE4, btm_A6_OPEN4);
        }

        private void btm_A3_OPEN5_Click(object sender, EventArgs e)
        {
            boolChange(btm_A3_OPEN5, btm_A3_CLOSE5);
        }

        private void btm_A3_CLOSE5_Click(object sender, EventArgs e)
        {
            boolChange(btm_A3_CLOSE5, btm_A3_OPEN5);
        }

        private void btm_A6_OPEN5_Click(object sender, EventArgs e)
        {
            boolChange(btm_A6_OPEN5, btm_A6_CLOSE5);
        }

        private void btm_A6_CLOSE5_Click(object sender, EventArgs e)
        {
            boolChange(btm_A6_CLOSE5, btm_A6_OPEN5);
        }

        private void btm_A3_OPEN6_Click(object sender, EventArgs e)
        {
            boolChange(btm_A3_OPEN6, btm_A3_CLOSE6);
        }

        private void btm_A3_CLOSE6_Click(object sender, EventArgs e)
        {
            boolChange(btm_A3_CLOSE6, btm_A3_OPEN6);
        }

        private void btm_A6_OPEN6_Click(object sender, EventArgs e)
        {
            boolChange(btm_A6_OPEN6, btm_A6_CLOSE6);
        }

        private void btm_A6_CLOSE6_Click(object sender, EventArgs e)
        {
            boolChange(btm_A6_CLOSE6, btm_A6_OPEN6);
        }

        private void btm_A3_OPEN7_Click(object sender, EventArgs e)
        {
            boolChange(btm_A3_OPEN7, btm_A3_CLOSE7);
        }

        private void btm_A3_CLOSE7_Click(object sender, EventArgs e)
        {
            boolChange(btm_A3_CLOSE7, btm_A3_OPEN7);
        }

        private void btm_A6_OPEN7_Click(object sender, EventArgs e)
        {
            boolChange(btm_A6_OPEN7, btm_A6_CLOSE7);
        }

        private void btm_A6_CLOSE7_Click(object sender, EventArgs e)
        {
            boolChange(btm_A6_CLOSE7, btm_A6_OPEN7);
        }

        private void btm_A3_OPEN8_Click(object sender, EventArgs e)
        {
            boolChange(btm_A3_OPEN8, btm_A3_CLOSE8);
        }

        private void btm_A3_CLOSE8_Click(object sender, EventArgs e)
        {
            boolChange(btm_A3_CLOSE8, btm_A3_OPEN8);
        }

        private void btm_A6_OPEN8_Click(object sender, EventArgs e)
        {
            boolChange(btm_A6_OPEN8, btm_A6_CLOSE8);
        }

        private void btm_A6_CLOSE8_Click(object sender, EventArgs e)
        {
            boolChange(btm_A6_CLOSE8, btm_A6_OPEN8);
        }

        private void btm_A6_OPEN9_Click(object sender, EventArgs e)
        {
            boolChange(btm_A6_OPEN9, btm_A6_CLOSE9);
        }

        private void btm_A6_CLOSE9_Click(object sender, EventArgs e)
        {
            boolChange(btm_A6_CLOSE9, btm_A6_OPEN9);
        }

        private void btm_A3_OPEN9_Click(object sender, EventArgs e)
        {
            boolChange(btm_A3_OPEN9, btm_A3_CLOSE9);
        }

        private void btm_A3_CLOSE9_Click(object sender, EventArgs e)
        {
            boolChange(btm_A3_CLOSE9, btm_A3_OPEN9);
        }
        #endregion


        #region 手动操作
        /// <summary>
        /// 手动操作
        /// </summary>
        private void btm_MOTOR1_BUTT_Click(object sender, EventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, btm_MOTOR1_BUTT.Name);
            tas.WriterData(pvc.Address[index], !(bool)(pvc.value[index]));
        }

        private void btm_MOTOR2_BUTT_Click(object sender, EventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, btm_MOTOR2_BUTT.Name);
            tas.WriterData(pvc.Address[index], !(bool)(pvc.value[index]));
        }

        private void btm_MOTOR3_BUTT_Click(object sender, EventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, btm_MOTOR3_BUTT.Name);
            tas.WriterData(pvc.Address[index], !(bool)(pvc.value[index]));
        }

        private void btm_AXIS_UP_Click(object sender, EventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, btm_AXIS_UP.Name);
            tas.WriterData(pvc.Address[index], !(bool)(pvc.value[index]));
        }
        private void btm_AXIS_DOWN_Click(object sender, EventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, btm_AXIS_DOWN.Name);
            tas.WriterData(pvc.Address[index], !(bool)(pvc.value[index]));
        }
        private void btm_AXIS_DOWN2_Click(object sender, EventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, btm_AXIS_DOWN2.Name);
            tas.WriterData(pvc.Address[index], !(bool)(pvc.value[index]));
        }
        private void btm_VAL_OPEN_Click(object sender, EventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, btm_VAL_OPEN.Name);
            tas.WriterData(pvc.Address[index], !(bool)(pvc.value[index]));
        }
        private void btm_VAL_CLOSE_Click(object sender, EventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, btm_VAL_CLOSE.Name);
            tas.WriterData(pvc.Address[index], !(bool)(pvc.value[index]));
        }
        private void btm_AXIS_OPEN_Click(object sender, EventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, btm_AXIS_OPEN.Name);
            tas.WriterData(pvc.Address[index], !(bool)(pvc.value[index]));
        }
        private void btm_AXIS_CLOSE_Click(object sender, EventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, btm_AXIS_CLOSE.Name);
            tas.WriterData(pvc.Address[index], !(bool)(pvc.value[index]));
        }
        private void btm_AXIS_POS0_Click(object sender, EventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, btm_AXIS_POS0.Name);
            tas.WriterData(pvc.Address[index], !(bool)(pvc.value[index]));
        }
        private void btm_AXIS_POS1_Click(object sender, EventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, btm_AXIS_POS1.Name);
            tas.WriterData(pvc.Address[index], !(bool)(pvc.value[index]));
        }
        private void btm_AXIS_POS2_Click(object sender, EventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, btm_AXIS_POS2.Name);
            tas.WriterData(pvc.Address[index], !(bool)(pvc.value[index]));
        }
        private void btm_AXIS_POS3_Click(object sender, EventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, btm_AXIS_POS3.Name);
            tas.WriterData(pvc.Address[index], !(bool)(pvc.value[index]));
        }
        private void btm_AXIS_POS4_Click(object sender, EventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, btm_AXIS_POS4.Name);
            tas.WriterData(pvc.Address[index], !(bool)(pvc.value[index]));
        }
        private void btm_AXIS_POS5_Click(object sender, EventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, btm_AXIS_POS5.Name);
            tas.WriterData(pvc.Address[index], !(bool)(pvc.value[index]));
        }
        private void btm_AXIS_POS6_Click(object sender, EventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, btm_AXIS_POS6.Name);
            tas.WriterData(pvc.Address[index], !(bool)(pvc.value[index]));
        }
        private void btm_AXIS_POS7_Click(object sender, EventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, btm_AXIS_POS7.Name);
            tas.WriterData(pvc.Address[index], !(bool)(pvc.value[index]));
        }
        private void btm_AXIS_POS8_Click(object sender, EventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, btm_AXIS_POS8.Name);
            tas.WriterData(pvc.Address[index], !(bool)(pvc.value[index]));
        }
        private void btm_AXIS_POS9_Click(object sender, EventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, btm_AXIS_POS9.Name);
            tas.WriterData(pvc.Address[index], !(bool)(pvc.value[index]));
        }

        private void btm_MOTOR_JOG1FOR_Click(object sender, EventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, txt_AXIS1_JOG_VEL.Name);
            tas.WriterData(pvc.Address[index], (Single)Convert.ToSingle(txt_AXIS1_JOG_VEL.Text));
            index = Array.IndexOf(pvc.ControlsName, btm_MOTOR_JOG1FOR.Name);
            tas.WriterData(pvc.Address[index], !(bool)pvc.value[index]);
        }
        
        private void btm_MOTOR_JOG1BACK_Click(object sender, EventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, txt_AXIS1_JOG_VEL.Name);
            tas.WriterData(pvc.Address[index], -(Single)Convert.ToSingle(txt_AXIS1_JOG_VEL.Text));
            index = Array.IndexOf(pvc.ControlsName, btm_MOTOR_JOG1BACK.Name);
            tas.WriterData(pvc.Address[index], !(bool)pvc.value[index]);
        }
        
        private void btm_MOTOR_JOG2FOR_Click(object sender, EventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, txt_AXIS2_JOG_VEL.Name);
            tas.WriterData(pvc.Address[index], (Single)Convert.ToSingle(txt_AXIS2_JOG_VEL.Text));
            index = Array.IndexOf(pvc.ControlsName, btm_MOTOR_JOG2FOR.Name);
            tas.WriterData(pvc.Address[index], !(bool)pvc.value[index]);
        }
        
        private void btm_MOTOR_JOG2BACK_Click(object sender, EventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, txt_AXIS2_JOG_VEL.Name);
            tas.WriterData(pvc.Address[index], -(Single)Convert.ToSingle(txt_AXIS2_JOG_VEL.Text));
            index = Array.IndexOf(pvc.ControlsName, btm_MOTOR_JOG2BACK.Name);
            tas.WriterData(pvc.Address[index], !(bool)pvc.value[index]);
        }
        

        private void btm_MOTOR_JOG3FOR_Click(object sender, EventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, txt_AXIS3_JOG_VEL.Name);
            tas.WriterData(pvc.Address[index], (Single)Convert.ToSingle(txt_AXIS3_JOG_VEL.Text));
            index = Array.IndexOf(pvc.ControlsName, btm_MOTOR_JOG3FOR.Name);
            tas.WriterData(pvc.Address[index], !(bool)pvc.value[index]);
        }
        
        private void btm_MOTOR_JOG3BACK_Click(object sender, EventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, txt_AXIS3_JOG_VEL.Name);
            tas.WriterData(pvc.Address[index], -(Single)Convert.ToSingle(txt_AXIS3_JOG_VEL.Text));
            index = Array.IndexOf(pvc.ControlsName, btm_MOTOR_JOG3BACK.Name);
            tas.WriterData(pvc.Address[index], !(bool)pvc.value[index]);
        }

        #endregion


        #region 文本框变量写入
        /// <summary>
        /// 文本框变量写入
        /// </summary>
        private void txt_enter_write(Control control, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                string aim = control.Text;
                DialogResult dr = MessageBox.Show(aim, "是否写入?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.OK)
                {
                    WriteText(control, aim);
                    control.Text = aim;
                }
            }
        }

        private void txt_AXIS3_ABS_VEL_KeyPress(object sender, KeyPressEventArgs e)
        {
            txt_enter_write(txt_AXIS3_ABS_VEL, e);
        }
        private void txt_AXIS2_ABS_VEL_KeyPress(object sender, KeyPressEventArgs e)
        {
            txt_enter_write(txt_AXIS2_ABS_VEL, e);
        }
        private void txt_AXIS1_ABS_VEL2_KeyPress(object sender, KeyPressEventArgs e)
        {
            txt_enter_write(txt_AXIS1_ABS_VEL2, e);
        }
        private void txt_AXIS1_ABS_VEL_KeyPress(object sender, KeyPressEventArgs e)
        {
            txt_enter_write(txt_AXIS1_ABS_VEL, e);
        }
        private void txt_AXIS1_ABS_VEL1_KeyPress(object sender, KeyPressEventArgs e)
        {
            txt_enter_write(txt_AXIS1_ABS_VEL1, e);
        }
        private void txt_AXIS3_CLOSE_POS_KeyPress(object sender, KeyPressEventArgs e)
        {
            txt_enter_write(txt_AXIS3_CLOSE_POS, e);
        }
        private void txt_AXIS3_OPEN_POS_KeyPress(object sender, KeyPressEventArgs e)
        {
            txt_enter_write(txt_AXIS3_OPEN_POS, e);
        }
        private void txt_AXIS1_SAFE2_POS_KeyPress(object sender, KeyPressEventArgs e)
        {
            txt_enter_write(txt_AXIS1_SAFE2_POS, e);
        }
        private void txt_AXIS2_0_POS_KeyPress(object sender, KeyPressEventArgs e)
        {
            txt_enter_write(txt_AXIS2_0_POS, e);
        }
        private void txt_AXIS2_9_POS_KeyPress(object sender, KeyPressEventArgs e)
        {
            txt_enter_write(txt_AXIS2_9_POS, e);
        }
        private void txt_AXIS2_8_POS_KeyPress(object sender, KeyPressEventArgs e)
        {
            txt_enter_write(txt_AXIS2_8_POS, e);
        }
        private void txt_AXIS2_7_POS_KeyPress(object sender, KeyPressEventArgs e)
        {
            txt_enter_write(txt_AXIS2_7_POS, e);
        }
        private void txt_AXIS2_6_POS_KeyPress(object sender, KeyPressEventArgs e)
        {
            txt_enter_write(txt_AXIS2_6_POS, e);
        }
        private void txt_AXIS1_SAFE1_POS_KeyPress(object sender, KeyPressEventArgs e)
        {
            txt_enter_write(txt_AXIS1_SAFE1_POS, e);
        }
        private void txt_AXIS2_5_POS_KeyPress(object sender, KeyPressEventArgs e)
        {
            txt_enter_write(txt_AXIS2_5_POS, e);
        }
        private void txt_AXIS2_4_POS_KeyPress(object sender, KeyPressEventArgs e)
        {
            txt_enter_write(txt_AXIS2_4_POS, e);
        }
        private void txt_AXIS2_3_POS_KeyPress(object sender, KeyPressEventArgs e)
        {
            txt_enter_write(txt_AXIS2_3_POS, e);
        }
        private void txt_AXIS2_2_POS_KeyPress(object sender, KeyPressEventArgs e)
        {
            txt_enter_write(txt_AXIS2_2_POS, e);
        }
        private void txt_AXIS2_1_POS_KeyPress(object sender, KeyPressEventArgs e)
        {
            txt_enter_write(txt_AXIS2_1_POS, e);
        }
        private void txt_AXIS1_UP_POS_KeyPress(object sender, KeyPressEventArgs e)
        {
            txt_enter_write(txt_AXIS1_UP_POS, e);
        }
        private void txt_AXIS1_DOWN_POS2_KeyPress(object sender, KeyPressEventArgs e)
        {
            txt_enter_write(txt_AXIS1_DOWN_POS2, e);
        }
        private void txt_AXIS1_DOWN_POS_KeyPress(object sender, KeyPressEventArgs e)
        {
            txt_enter_write(txt_AXIS1_DOWN_POS, e);
        }
        private void txt_AXIS1_JOG_VEL_KeyPress(object sender, KeyPressEventArgs e)
        {
            txt_enter_write(txt_AXIS1_JOG_VEL, e);
        }
        private void txt_AXIS2_JOG_VEL_KeyPress(object sender, KeyPressEventArgs e)
        {
            txt_enter_write(txt_AXIS2_JOG_VEL, e);
        }
        private void txt_AXIS3_JOG_VEL_KeyPress(object sender, KeyPressEventArgs e)
        {
            txt_enter_write(txt_AXIS3_JOG_VEL, e);
        }
        private void txt_SCE_VAL_KeyPress(object sender, KeyPressEventArgs e)
        {
            txt_enter_write(txt_SCE_VAL, e);
        }
        private void txt_FIRST_VAL_KeyPress(object sender, KeyPressEventArgs e)
        {
            txt_enter_write(txt_FIRST_VAL, e);
        }
        private void txt_SAMPLING_KeyPress(object sender, KeyPressEventArgs e)
        {
            txt_enter_write(txt_SAMPLING, e);
        }
        #endregion

        private void socket_timer_Tick(object sender, EventArgs e)
        {
            if (unityInited)
            {
                bool tasState=false;
                try
                {
                    tasState = tas.ConnectFlag;
                }
                catch { }
                if (tasState)
                {
                    button2.Text = "PLC";
                    int index;
                    byte[] mes = new byte[24], tmp;
                    index = Array.IndexOf(pvc.ControlsName, txt_AXIS3_REAL_POS.Name);
                    tmp = BitConverter.GetBytes((Single)(pvc.value[index])/1000f-motorZero[2]/1000f);
                    for (int i = 0; i < tmp.Length; i++) mes[i] = tmp[i];
                    index = Array.IndexOf(pvc.ControlsName, txt_AXIS2_REAL_POS.Name);
                    tmp = BitConverter.GetBytes((Single)pvc.value[index] - motorZero[1] / 1000f);
                    for (int i = 0; i < tmp.Length; i++) mes[tmp.Length * 1 + i] = tmp[i];
                    index = Array.IndexOf(pvc.ControlsName, txt_AXIS1_REAL_POS.Name);
                    tmp = BitConverter.GetBytes((Single)pvc.value[index]/1000f - motorZero[0] / 1000f);
                    for (int i = 0; i < tmp.Length; i++) mes[tmp.Length * 2 + i] = tmp[i];

                    index = Array.IndexOf(pvc.ControlsName, btm_VAL_CLOSE.Name);
                    if ((bool)pvc.value[index])
                    {
                        tmp = BitConverter.GetBytes(0.0f);
                        for (int i = 0; i < tmp.Length; i++) mes[tmp.Length * 3 + i] = tmp[i];
                        tmp = BitConverter.GetBytes(0.0f);
                        for (int i = 0; i < tmp.Length; i++) mes[tmp.Length * 4 + i] = tmp[i];
                    }
                    else
                    {
                        tmp = BitConverter.GetBytes(-0.01f);
                        for (int i = 0; i < tmp.Length; i++) mes[tmp.Length * 3 + i] = tmp[i];
                        tmp = BitConverter.GetBytes(0.01f);
                        for (int i = 0; i < tmp.Length; i++) mes[tmp.Length * 4 + i] = tmp[i];
                    }
                    
                    tmp = BitConverter.GetBytes(0);
                    for (int i = 0; i < tmp.Length; i++) mes[tmp.Length * 5 + i] = tmp[i];
                    mes_socket.send(mes);
                }
                else
                {
                    button2.Text = "test";
                    byte[] mes = new byte[24];
                    for (int i = 0; i < 24; i++) mes[i] = mes_socket.testMes[mes_socket.test_index, i];
                    mes_socket.send(mes);
                    mes_socket.test_index += mes_socket.test_d;
                    if (mes_socket.test_index == 599) mes_socket.test_d = -1;
                    if (mes_socket.test_index == 0) mes_socket.test_d = 1;
                }
                button1.Text = mes_socket.alive.ToString() + mes_socket.restart.ToString();
            }
        }

        private void superTabControlPanel2_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized || this.WindowState == FormWindowState.Normal)
            {
                panelEx8.Width = superTabControlPanel2.Width / 2;
                panelEx8.Height = superTabControlPanel2.Height / 2;
                panelEx10.Width = superTabControlPanel2.Width / 2;
                panelEx10.Height = superTabControlPanel2.Height / 2;
                panelEx11.Width = superTabControlPanel2.Width / 2;
                panelEx11.Height = superTabControlPanel2.Height / 4;
                panelEx10.Left = superTabControlPanel2.Width / 2;
                panelEx11.Left = superTabControlPanel2.Width / 2;
                panelEx10.Top = superTabControlPanel2.Height / 4;
                panelEx9.Width = superTabControlPanel2.Width / 4;
                panelEx12.Width = superTabControlPanel2.Width / 4;
                panelEx9.Height = superTabControlPanel2.Height / 4;
                panelEx12.Height = superTabControlPanel2.Height / 4;
                panelEx12.Left = superTabControlPanel2.Width / 4;
                panelEx9.Top = superTabControlPanel2.Height / 2;
                panelEx12.Top = superTabControlPanel2.Height / 2;
                panelEx13.Width = superTabControlPanel2.Width / 3;
                panelEx14.Width = superTabControlPanel2.Width / 3;
                panelEx15.Width = superTabControlPanel2.Width / 3;
                panelEx14.Left = superTabControlPanel2.Width / 3;
                panelEx15.Left = superTabControlPanel2.Width / 3 * 2;
                panelEx13.Top = superTabControlPanel2.Height / 4 * 3;
                panelEx14.Top = superTabControlPanel2.Height / 4 * 3;
                panelEx15.Top = superTabControlPanel2.Height / 4 * 3;
                panelEx13.Height = superTabControlPanel2.Height / 4;
                panelEx14.Height = superTabControlPanel2.Height / 4;
                panelEx15.Height = superTabControlPanel2.Height / 4;
            }
        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            if(unityInited  == false)mes_socket = new socket2unity(8888);
            unityInited = true;
        }

        private void keep_timer_Tick(object sender, EventArgs e)
        {
            if (unityInited)
            {
                if (!mes_socket.alive)
                {
                    mes_socket.init();
                }
            }
        }
        

        private void btm_change_pos_Click(object sender, EventArgs e)
        {
            chart1.Titles[0].Text = titles[0];
            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
            chart1.Series[2].Points.Clear();
        }

        private void btm_change_vel_Click(object sender, EventArgs e)
        {
            chart1.Titles[0].Text = titles[1];
            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
            chart1.Series[2].Points.Clear();

        }

        private void btm_change_tor_Click(object sender, EventArgs e)
        {
            chart1.Titles[0].Text = titles[2];
            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
            chart1.Series[2].Points.Clear();

        }

        private void textBox1_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            txt_enter_write(textBox1,e);
        }

        private void textBox2_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            txt_enter_write(textBox2, e);
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            txt_enter_write(textBox3, e);
        }

        private void txt_SAMPLING2_KeyPress(object sender, KeyPressEventArgs e)
        {
            txt_enter_write(txt_SAMPLING2, e);
        }
    }
}
