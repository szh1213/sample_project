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

namespace 触摸屏界面
{
    public partial class Main : Office2007Form
    {
        private TwcAds tas;
        PlcVariableClass pvc = new PlcVariableClass();
        Size _beforeDialogSize;
        private string[][] error_hash = new string[6][];

        private double t = 0.00001;
        private double speed = 0;
        private double torque = 0;
        public Main()
        {
            InitializeComponent();
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

        
        protected override void OnResizeEnd(EventArgs e)
        {

            base.OnResizeEnd(e);
            Size endSize = this.Size;
            float percentWidth = (float)endSize.Width / _beforeDialogSize.Width;
            float percentHeight = (float)endSize.Height / _beforeDialogSize.Height;

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
                panelex.Width = (int)(panelex.Width * percentWidth);
                panelex.Height = (int)(panelex.Height * percentHeight);

                //为了不使控件之间覆盖 位置也要按比例变化
                panelex.Left = (int)(panelex.Left * percentWidth);
                panelex.Top = (int)(panelex.Top * percentHeight);
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
        private void buttonX1_Click(object sender, EventArgs e)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("PLC.config");
            XmlElement root = doc.DocumentElement;
            XmlNode ipNode = root.SelectSingleNode("/PLC/ip");
            string ip = ipNode.Attributes["address"].Value;

            tas = new TwcAds(ip, 851, pvc, true);
            this.buttonX1.Enabled = false;
            this.buttonX2.Enabled = true;
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            if (!tas.ConnectClose())
            {
                MessageBoxEx.Show("关闭PLC连接失败！", "通讯失败");
                return;
            }
            this.buttonX1.Enabled = true;
            this.buttonX2.Enabled = false;
        }

        /// <summary>
        /// 程序关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        #endregion

        
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
                        if (!pvc.Address[i].Contains(".")) pvc.Address[i] = "MAIN." + pvc.Address[i];
                        pvc.Type[i] = control.Tag.ToString().Split(';')[1];
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
                            if (!pvc.Address[i].Contains(".")) pvc.Address[i] = "MAIN." + pvc.Address[i];
                            pvc.Type[i] = control.Tag.ToString().Split(';')[1];
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
                        if (!pvc.Address[i].Contains(".")) pvc.Address[i] = "MAIN." + pvc.Address[i];
                        pvc.Type[i] = control.Tag.ToString().Split(';')[1];
                        i++;
                    }
                }
                foreach (Control control in this.superTabControlPanel4.Controls)
                {
                    if (control.Tag != null && !string.IsNullOrEmpty(control.Tag.ToString()))
                    {
                        pvc.ControlsName[i] = control.Name;
                        pvc.Address[i] = control.Tag.ToString().Split(';')[0];
                        if (!pvc.Address[i].Contains(".")) pvc.Address[i] = "MAIN." + pvc.Address[i];
                        pvc.Type[i] = control.Tag.ToString().Split(';')[1];
                        i++;
                    }
                }
                foreach (Control control in this.superTabControlPanel5.Controls)
                {
                    if (control.Tag != null && !string.IsNullOrEmpty(control.Tag.ToString()))
                    {
                        pvc.ControlsName[i] = control.Name;
                        pvc.Address[i] = control.Tag.ToString().Split(';')[0];
                        if (!pvc.Address[i].Contains(".")) pvc.Address[i] = "MAIN." + pvc.Address[i];
                        pvc.Type[i] = control.Tag.ToString().Split(';')[1];
                        i++;
                    }
                }
                foreach (Control control in this.superTabControlPanel6.Controls)
                {
                    if (control.Tag != null && !string.IsNullOrEmpty(control.Tag.ToString()))
                    {
                        pvc.ControlsName[i] = control.Name;
                        pvc.Address[i] = control.Tag.ToString().Split(';')[0];
                        if (!pvc.Address[i].Contains(".")) pvc.Address[i] = "MAIN." + pvc.Address[i];
                        pvc.Type[i] = control.Tag.ToString().Split(';')[1];
                        i++;
                    }
                }
                foreach (Control control in this.superTabControlPanel7.Controls)
                {
                    if (control.Tag != null && !string.IsNullOrEmpty(control.Tag.ToString()))
                    {
                        pvc.ControlsName[i] = control.Name;
                        pvc.Address[i] = control.Tag.ToString().Split(';')[0];
                        if (!pvc.Address[i].Contains(".")) pvc.Address[i] = "MAIN." + pvc.Address[i];
                        pvc.Type[i] = control.Tag.ToString().Split(';')[1];
                        i++;
                    }
                }
                foreach (Control control in this.panelEx2.Controls)
                {
                    if (control.Tag != null && !string.IsNullOrEmpty(control.Tag.ToString()))
                    {
                        pvc.ControlsName[i] = control.Name;
                        pvc.Address[i] = control.Tag.ToString().Split(';')[0];
                        if (!pvc.Address[i].Contains(".")) pvc.Address[i] = "MAIN." + pvc.Address[i];
                        pvc.Type[i] = control.Tag.ToString().Split(';')[1];
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
                        if(control is SymbolBox)
                        SetValue(control);
                    }
                    foreach (Control panelex in this.superTabControlPanel2.Controls)
                    {
                        if (!panelex.Name.StartsWith("panelEx")) continue;
                        foreach (Control control in panelex.Controls)
                        {
                            if(control.Tag!=null && !control.Tag.ToString().Contains("JOG_VEL"))
                            SetValue(control);
                        }
                    }
                    foreach (Control control in this.superTabControlPanel3.Controls)
                    {
                        SetValue(control);
                    }
                    foreach (Control control in this.superTabControlPanel5.Controls)
                    {
                        SetValue(control);
                    }
                    foreach (Control control in this.superTabControlPanel6.Controls)
                    {
                        SetValue(control);
                    }
                    foreach (Control control in this.superTabControlPanel7.Controls)
                    {
                        SetValue(control);
                    }
                    string[] tmp = { "运行", "复位", "","停止", "急停" };
                    int index = Array.IndexOf(pvc.ControlsName, this.labelX8.Name);
                    this.labelX8.Text = tmp[Convert.ToInt16(pvc.value[index])/2];
                    this.labelX9.Text =  (bool) pvc.value[Array.IndexOf(pvc.ControlsName, this.labelX9.Name)]?"PA":"PC";
                    
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message,"读取错误");
            }
        }

        private void SetValue(Control control)
        {
            try
            {
                if (control.Tag != null && !string.IsNullOrEmpty(control.Tag.ToString()))
                {
                    int index = Array.IndexOf(pvc.ControlsName, control.Name);

                    //文本框
                    if (control is DevComponents.DotNetBar.Controls.TextBoxX)
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
            catch(Exception r)
            {
                MessageBox.Show(r.Message,control.Name);
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
                MessageBox.Show(r.Message,"队列读取错误");
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
                            gr[0].Value = DateTime.Now;
                            gr[1].Value = error_hash[i][j];
                        }
                    }
                }
                
                    this.sgc2.Refresh();
            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message, "警报读取错误");
            }
        }

        private void WriteText(Control control)
        {
            if (control is TextBoxX && control.Tag != null && !string.IsNullOrEmpty(control.Tag.ToString()))
            {
                try
                {
                    int index = Array.IndexOf(pvc.ControlsName, control.Name);
                    if (pvc.Type[index] == "INT")
                        tas.WriterData(pvc.Address[index], (short)Convert.ToInt16(control.Text));
                    if (pvc.Type[index] == "REAL")
                    {
                        tas.WriterData(pvc.Address[index], (double)Convert.ToDouble(control.Text));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message,"文本内容写入变量错误");
                }
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

            Series series1 = new Series("转速(转/分钟)");
            series1.ChartArea = "C1";
            this.chart1.Series.Add(series1);
            this.chart1.Series[0].Color = Color.Green;
            this.chart1.Series[0].ChartType = SeriesChartType.StepLine;

            series1 = new Series("扭矩(‰)");
            series1.ChartArea = "C1";
            this.chart1.Series.Add(series1);
            this.chart1.Series[1].Color = Color.Red;
            this.chart1.Series[1].ChartType = SeriesChartType.StepLine;



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
            this.chart1.Titles[0].Text = "数据显示";
            this.chart1.Titles[0].ForeColor = Color.RoyalBlue;
            this.chart1.Titles[0].Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);


            for (int i = 0; i < 3; i++)
            {
                this.chart1.Series[0].Points.AddXY((i + 1), 0);
                this.chart1.Series[1].Points.AddXY((i + 1), 0);
            }

        }
        /// <summary>
        /// 显示曲线
        /// </summary>
        private void updateChart()
        {
            DataPoint endp = this.chart1.Series[0].Points.Last();
            DataPoint startp = this.chart1.Series[0].Points.First();
            if (this.chart1.Series[0].Points.Count > 500)
            {
                this.chart1.Series[0].Points.RemoveAt(0);
                this.chart1.Series[1].Points.RemoveAt(0);
            }

            t += 0.01;
            speed = 3000 * Math.Sin(t);
            torque = (100 - Math.Max(0, (Math.Abs(speed) - 1440) * 0.02)) * Math.Sign(speed);
            this.chart1.Series[0].Points.AddXY(endp.XValue + 1, speed);
            this.chart1.Series[1].Points.AddXY(endp.XValue + 1, torque * 10);
            this.chart1.ChartAreas[0].AxisX.Maximum = endp.XValue + 50;
            this.chart1.ChartAreas[0].AxisX.Minimum = startp.XValue - 50;


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
        

        private void btn_A3_OPEN1_Click(object sender, EventArgs e)
        {
            boolChange(btn_A3_OPEN1, btn_A3_CLOSE1);
            int index = Array.IndexOf(pvc.ControlsName, btn_A3_OPEN1.Name);
            MessageBox.Show(pvc.value[index].ToString());
        }

        private void btn_A3_CLOSE1_Click(object sender, EventArgs e)
        {
            boolChange(btn_A3_CLOSE1, btn_A3_OPEN1);
            int index = Array.IndexOf(pvc.ControlsName, btn_A3_OPEN1.Name);
            MessageBox.Show(pvc.value[index].ToString());
        }

        private void btn_A6_OPEN1_Click(object sender, EventArgs e)
        {
            boolChange(btn_A6_OPEN1, btn_A6_CLOSE1);
        }

        private void btn_A6_CLOSE1_Click(object sender, EventArgs e)
        {
            boolChange(btn_A6_CLOSE1, btn_A6_OPEN1);
        }

        private void btn_A3_OPEN2_Click(object sender, EventArgs e)
        {
            boolChange(btn_A3_OPEN2, btn_A3_CLOSE2);
        }

        private void btn_A3_CLOSE2_Click(object sender, EventArgs e)
        {
            boolChange(btn_A3_CLOSE2, btn_A3_OPEN2);
        }

        private void btn_A6_OPEN2_Click(object sender, EventArgs e)
        {
            boolChange(btn_A6_OPEN2, btn_A6_CLOSE2);
        }

        private void btn_A6_CLOSE2_Click(object sender, EventArgs e)
        {
            boolChange(btn_A6_CLOSE2, btn_A6_OPEN2);
        }

        private void btn_A3_OPEN3_Click(object sender, EventArgs e)
        {
            boolChange(btn_A3_OPEN3, btn_A3_CLOSE3);
        }

        private void btn_A3_CLOSE3_Click(object sender, EventArgs e)
        {
            boolChange(btn_A3_CLOSE3, btn_A3_OPEN3);
        }

        private void btn_A6_OPEN3_Click(object sender, EventArgs e)
        {
            boolChange(btn_A6_OPEN3, btn_A6_CLOSE3);
        }

        private void btn_A6_CLOSE3_Click(object sender, EventArgs e)
        {
            boolChange(btn_A6_CLOSE3, btn_A6_OPEN3);
        }

        private void btn_A3_OPEN4_Click(object sender, EventArgs e)
        {
            boolChange(btn_A3_OPEN4, btn_A3_CLOSE4);
        }

        private void btn_A3_CLOSE4_Click(object sender, EventArgs e)
        {
            boolChange(btn_A3_CLOSE4, btn_A3_OPEN4);
        }

        private void btn_A6_OPEN4_Click(object sender, EventArgs e)
        {
            boolChange(btn_A6_OPEN4, btn_A6_CLOSE4);
        }

        private void btn_A6_CLOSE4_Click(object sender, EventArgs e)
        {
            boolChange(btn_A6_CLOSE4, btn_A6_OPEN4);
        }

        private void btn_A3_OPEN5_Click(object sender, EventArgs e)
        {
            boolChange(btn_A3_OPEN5, btn_A3_CLOSE5);
        }

        private void btn_A3_CLOSE5_Click(object sender, EventArgs e)
        {
            boolChange(btn_A3_CLOSE5, btn_A3_OPEN5);
        }

        private void btn_A6_OPEN5_Click(object sender, EventArgs e)
        {
            boolChange(btn_A6_OPEN5, btn_A6_CLOSE5);
        }

        private void btn_A6_CLOSE5_Click(object sender, EventArgs e)
        {
            boolChange(btn_A6_CLOSE5, btn_A6_OPEN5);
        }

        private void btn_A3_OPEN6_Click(object sender, EventArgs e)
        {
            boolChange(btn_A3_OPEN6, btn_A3_CLOSE6);
        }

        private void btn_A3_CLOSE6_Click(object sender, EventArgs e)
        {
            boolChange(btn_A3_CLOSE6, btn_A3_OPEN6);
        }

        private void btn_A6_OPEN6_Click(object sender, EventArgs e)
        {
            boolChange(btn_A6_OPEN6, btn_A6_CLOSE6);
        }

        private void btn_A6_CLOSE6_Click(object sender, EventArgs e)
        {
            boolChange(btn_A6_CLOSE6, btn_A6_OPEN6);
        }

        private void btn_A3_OPEN7_Click(object sender, EventArgs e)
        {
            boolChange(btn_A3_OPEN7, btn_A3_CLOSE7);
        }

        private void btn_A3_CLOSE7_Click(object sender, EventArgs e)
        {
            boolChange(btn_A3_CLOSE7, btn_A3_OPEN7);
        }

        private void btn_A6_OPEN7_Click(object sender, EventArgs e)
        {
            boolChange(btn_A6_OPEN7, btn_A6_CLOSE7);
        }

        private void btn_A6_CLOSE7_Click(object sender, EventArgs e)
        {
            boolChange(btn_A6_CLOSE7, btn_A6_OPEN7);
        }

        private void btn_A3_OPEN8_Click(object sender, EventArgs e)
        {
            boolChange(btn_A3_OPEN8, btn_A3_CLOSE8);
        }

        private void btn_A3_CLOSE8_Click(object sender, EventArgs e)
        {
            boolChange(btn_A3_CLOSE8, btn_A3_OPEN8);
        }

        private void btn_A6_OPEN8_Click(object sender, EventArgs e)
        {
            boolChange(btn_A6_OPEN8, btn_A6_CLOSE8);
        }

        private void btn_A6_CLOSE8_Click(object sender, EventArgs e)
        {
            boolChange(btn_A6_CLOSE8, btn_A6_OPEN8);
        }

        private void btn_A6_OPEN9_Click(object sender, EventArgs e)
        {
            boolChange(btn_A6_OPEN9, btn_A6_CLOSE9);
        }

        private void btn_A6_CLOSE9_Click(object sender, EventArgs e)
        {
            boolChange(btn_A6_CLOSE9, btn_A6_OPEN9);
        }

        private void btn_A3_OPEN9_Click(object sender, EventArgs e)
        {
            boolChange(btn_A3_OPEN9, btn_A3_CLOSE9);
        }

        private void btn_A3_CLOSE9_Click(object sender, EventArgs e)
        {
            boolChange(btn_A3_CLOSE9, btn_A3_OPEN9);
        }
        #endregion


        #region 手动操作
        /// <summary>
        /// 手动操作
        /// </summary>
        private void buttonX40_Click(object sender, EventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, buttonX40.Name);
            tas.WriterData(pvc.Address[index], !(bool)(pvc.value[index]));
        }

        private void buttonX41_Click(object sender, EventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, buttonX41.Name);
            tas.WriterData(pvc.Address[index], !(bool)(pvc.value[index]));
        }

        private void buttonX42_Click(object sender, EventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, buttonX42.Name);
            tas.WriterData(pvc.Address[index], !(bool)(pvc.value[index]));
        }

        private void buttonX56_Click(object sender, EventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, buttonX56.Name);
            tas.WriterData(pvc.Address[index], !(bool)(pvc.value[index]));
        }
        private void buttonX55_Click(object sender, EventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, buttonX55.Name);
            tas.WriterData(pvc.Address[index], !(bool)(pvc.value[index]));
        }
        private void buttonX57_Click(object sender, EventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, buttonX57.Name);
            tas.WriterData(pvc.Address[index], !(bool)(pvc.value[index]));
        }
        private void buttonX43_Click(object sender, EventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, buttonX43.Name);
            tas.WriterData(pvc.Address[index], !(bool)(pvc.value[index]));
        }
        private void buttonX44_Click(object sender, EventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, buttonX44.Name);
            tas.WriterData(pvc.Address[index], !(bool)(pvc.value[index]));
        }
        private void buttonX59_Click(object sender, EventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, buttonX59.Name);
            tas.WriterData(pvc.Address[index], !(bool)(pvc.value[index]));
        }
        private void buttonX58_Click(object sender, EventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, buttonX58.Name);
            tas.WriterData(pvc.Address[index], !(bool)(pvc.value[index]));
        }
        private void buttonX46_Click(object sender, EventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, buttonX46.Name);
            tas.WriterData(pvc.Address[index], !(bool)(pvc.value[index]));
        }
        private void buttonX45_Click(object sender, EventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, buttonX45.Name);
            tas.WriterData(pvc.Address[index], !(bool)(pvc.value[index]));
        }
        private void buttonX47_Click(object sender, EventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, buttonX47.Name);
            tas.WriterData(pvc.Address[index], !(bool)(pvc.value[index]));
        }
        private void buttonX48_Click(object sender, EventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, buttonX48.Name);
            tas.WriterData(pvc.Address[index], !(bool)(pvc.value[index]));
        }
        private void buttonX49_Click(object sender, EventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, buttonX49.Name);
            tas.WriterData(pvc.Address[index], !(bool)(pvc.value[index]));
        }
        private void buttonX50_Click(object sender, EventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, buttonX50.Name);
            tas.WriterData(pvc.Address[index], !(bool)(pvc.value[index]));
        }
        private void buttonX51_Click(object sender, EventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, buttonX51.Name);
            tas.WriterData(pvc.Address[index], !(bool)(pvc.value[index]));
        }
        private void buttonX52_Click(object sender, EventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, buttonX52.Name);
            tas.WriterData(pvc.Address[index], !(bool)(pvc.value[index]));
        }
        private void buttonX53_Click(object sender, EventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, buttonX53.Name);
            tas.WriterData(pvc.Address[index], !(bool)(pvc.value[index]));
        }
        private void buttonX54_Click(object sender, EventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, buttonX54.Name);
            tas.WriterData(pvc.Address[index], !(bool)(pvc.value[index]));
        }

        private void buttonX60_MouseDown(object sender, MouseEventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, tbx_AXIS1_JOG_VEL.Name);
            tas.WriterData(pvc.Address[index], (double)Convert.ToDouble(tbx_AXIS1_JOG_VEL.Text));
            index = Array.IndexOf(pvc.ControlsName, buttonX60.Name);
            tas.WriterData(pvc.Address[index], true);
        }

        private void buttonX60_MouseUp(object sender, MouseEventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, tbx_AXIS1_JOG_VEL.Name);
            tas.WriterData(pvc.Address[index], 0);
            index = Array.IndexOf(pvc.ControlsName, buttonX60.Name);
            tas.WriterData(pvc.Address[index], false);
        }
        private void buttonX61_MouseDown(object sender, MouseEventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, tbx_AXIS1_JOG_VEL.Name);
            tas.WriterData(pvc.Address[index], -(double)Convert.ToDouble(tbx_AXIS1_JOG_VEL.Text));
            index = Array.IndexOf(pvc.ControlsName, buttonX61.Name);
            tas.WriterData(pvc.Address[index], true);
        }

        private void buttonX61_MouseUp(object sender, MouseEventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, tbx_AXIS1_JOG_VEL.Name);
            tas.WriterData(pvc.Address[index], 0);
            index = Array.IndexOf(pvc.ControlsName, buttonX61.Name);
            tas.WriterData(pvc.Address[index], false);
        }
        private void buttonX62_MouseDown(object sender, MouseEventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, tbx_AXIS2_JOG_VEL.Name);
            tas.WriterData(pvc.Address[index], (double)Convert.ToDouble(tbx_AXIS2_JOG_VEL.Text));
            index = Array.IndexOf(pvc.ControlsName, buttonX62.Name);
            tas.WriterData(pvc.Address[index], true);
        }

        private void buttonX62_MouseUp(object sender, MouseEventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, tbx_AXIS2_JOG_VEL.Name);
            tas.WriterData(pvc.Address[index], 0);
            index = Array.IndexOf(pvc.ControlsName, buttonX62.Name);
            tas.WriterData(pvc.Address[index], false);
        }
        private void buttonX63_MouseDown(object sender, MouseEventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, tbx_AXIS2_JOG_VEL.Name);
            tas.WriterData(pvc.Address[index], -(double)Convert.ToDouble(tbx_AXIS1_JOG_VEL.Text));
            index = Array.IndexOf(pvc.ControlsName, buttonX63.Name);
            tas.WriterData(pvc.Address[index], true);
        }

        private void buttonX63_MouseUp(object sender, MouseEventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, tbx_AXIS2_JOG_VEL.Name);
            tas.WriterData(pvc.Address[index], 0);
            index = Array.IndexOf(pvc.ControlsName, buttonX63.Name);
            tas.WriterData(pvc.Address[index], false);
        }

        private void buttonX64_MouseDown(object sender, MouseEventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, tbx_AXIS3_JOG_VEL.Name);
            tas.WriterData(pvc.Address[index], (double)Convert.ToDouble(tbx_AXIS2_JOG_VEL.Text));
            index = Array.IndexOf(pvc.ControlsName, buttonX64.Name);
            tas.WriterData(pvc.Address[index], true);
        }

        private void buttonX64_MouseUp(object sender, MouseEventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, tbx_AXIS3_JOG_VEL.Name);
            tas.WriterData(pvc.Address[index], 0);
            index = Array.IndexOf(pvc.ControlsName, buttonX64.Name);
            tas.WriterData(pvc.Address[index], false);
        }
        private void buttonX65_MouseDown(object sender, MouseEventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, tbx_AXIS3_JOG_VEL.Name);
            tas.WriterData(pvc.Address[index], -(double)Convert.ToDouble(tbx_AXIS1_JOG_VEL.Text));
            index = Array.IndexOf(pvc.ControlsName, buttonX65.Name);
            tas.WriterData(pvc.Address[index], true);
        }

        private void buttonX65_MouseUp(object sender, MouseEventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, tbx_AXIS3_JOG_VEL.Name);
            tas.WriterData(pvc.Address[index], 0);
            index = Array.IndexOf(pvc.ControlsName, buttonX65.Name);
            tas.WriterData(pvc.Address[index], false);
        }
        #endregion

        #region  自动操作
        /// <summary>
        /// 自动操作，点击读取，点击写入
        /// </summary>
        private void buttonX67_Click(object sender, EventArgs e)
        {
            foreach (Control control in this.superTabControlPanel1.Controls)
            {
                if (control.Tag != null && !control.Tag.ToString().Contains("WORD"))
                    SetValue(control);
            }
            string[] tmp = {
                "准备",
                "开A6阀",
                "开A3阀",
                "空瓶进入",
                "取出空瓶","闲置空瓶","取样","样瓶抓起","放置样品","送出样品","取样完成","结束",""
            };
            int index = Array.IndexOf(pvc.ControlsName, this.tbx_SETP_P.Name);
            this.tbx_SETP_P.Text = tmp[Convert.ToInt16(pvc.value[index])];
            this.textBoxX2.Text = tmp[Convert.ToInt16(pvc.value[index])+1];
            
        }

        private void buttonX68_Click(object sender, EventArgs e)
        {
            foreach (Control control in this.superTabControlPanel1.Controls)
            {
                WriteText(control);
            }
        }
        #endregion

        #region  参数设置
        /// <summary>
        /// 参数设置，点击读取，点击写入
        /// </summary>
        private void buttonX39_Click(object sender, EventArgs e)
        {
            foreach (Control control in this.superTabControlPanel4.Controls)
            {
                SetValue(control);
            }
        }

        private void buttonX66_Click(object sender, EventArgs e)
        {
            foreach (Control control in this.superTabControlPanel4.Controls)
            {
                WriteText(control);
            }
        }
        #endregion
    }
}
