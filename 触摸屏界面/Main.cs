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

        private double t = 0.00001;
        private double speed = 0;
        private double torque = 0;
        public Main()
        {
            InitializeComponent();
        }
        
        #region 界面
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
            foreach (Control control in this.superTabControlPanel2.Controls)
            {
                if (control is DataGridView)
                    continue;
                //按比例改变控件大小
                control.Width = (int)(control.Width * percentWidth);
                
                //为了不使控件之间覆盖 位置也要按比例变化
                control.Left = (int)(control.Left * percentWidth);
                control.Top = (int)(control.Top * percentHeight);
            }
            foreach (Control cc in groupPanel1.Controls)
                cc.Font = new Font(cc.Font.Name, labelX22.Font.Size * percentWidth);
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
            foreach (Control control in this.groupPanel1.Controls)
            {
                if (control is DataGridView)
                    continue;
                //按比例改变控件大小
                control.Width = (int)(control.Width * percentWidth);
                control.Height = (int)(control.Height * percentHeight);
                Font lastFont = this.sgc1.DefaultVisualStyles.CellStyles.Default.Font;
                Font newFont = new Font(lastFont.Name, lastFont.Size * percentWidth);
                this.sgc1.DefaultVisualStyles.CellStyles.Default.Font = newFont;
                //this.sgc1.DefaultVisualStyles.ColumnHeaderStyles.Default.Font = newFont;
                this.sgc1.DefaultVisualStyles.RowStyles.Default.RowHeaderStyle.Font = newFont;
               
                //为了不使控件之间覆盖 位置也要按比例变化
                control.Left = (int)(control.Left * percentWidth);
                control.Top = (int)(control.Top * percentHeight);
            }
            foreach (Control control in this.groupPanel2.Controls)
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
            foreach (Control control in this.groupPanel3.Controls)
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
            XmlNode ipNode = root.SelectSingleNode("/ip");
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

        #region 方法
        /// <summary>
        /// 地址初始化
        /// </summary>
        private void Init()
        {
            try
            {
                int i=0;
                int I_AddressCount = 300;
                pvc.Test = "MAIN.Test";
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
                        if (!pvc.Address[i].StartsWith("MAIN.")) pvc.Address[i] = "MAIN." + pvc.Address[i];
                        pvc.Type[i] = control.Tag.ToString().Split(';')[1];
                        i++;
                    }
                }
                foreach (Control control in this.groupPanel1.Controls)
                {
                    if (control.Tag != null && !string.IsNullOrEmpty(control.Tag.ToString()))
                    {
                        pvc.ControlsName[i] = control.Name;
                        pvc.Address[i] = control.Tag.ToString().Split(';')[0];
                        if (!pvc.Address[i].StartsWith("MAIN.")) pvc.Address[i] = "MAIN." + pvc.Address[i];
                        pvc.Type[i] = control.Tag.ToString().Split(';')[1];
                        i++;
                    }
                }
                foreach (Control control in this.groupPanel2.Controls)
                {
                    if (control.Tag != null && !string.IsNullOrEmpty(control.Tag.ToString()))
                    {
                        pvc.ControlsName[i] = control.Name;
                        pvc.Address[i] = control.Tag.ToString().Split(';')[0];
                        if (!pvc.Address[i].StartsWith("MAIN.")) pvc.Address[i] = "MAIN." + pvc.Address[i];
                        pvc.Type[i] = control.Tag.ToString().Split(';')[1];
                        i++;
                    }
                }
                foreach (Control control in this.groupPanel3.Controls)
                {
                    if (control.Tag != null && !string.IsNullOrEmpty(control.Tag.ToString()))
                    {
                        pvc.ControlsName[i] = control.Name;
                        pvc.Address[i] = control.Tag.ToString().Split(';')[0];
                        if (!pvc.Address[i].StartsWith("MAIN.")) pvc.Address[i] = "MAIN." + pvc.Address[i];
                        pvc.Type[i] = control.Tag.ToString().Split(';')[1];
                        i++;
                    }
                }
                foreach (Control control in this.superTabControlPanel3.Controls)
                {
                    if (control.Tag != null && !string.IsNullOrEmpty(control.Tag.ToString()))
                    {
                        pvc.ControlsName[i] = control.Name;
                        pvc.Address[i] = control.Tag.ToString().Split(';')[0];
                        if (!pvc.Address[i].StartsWith("MAIN.")) pvc.Address[i] = "MAIN." + pvc.Address[i];
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
                        if (!pvc.Address[i].StartsWith("MAIN.")) pvc.Address[i] = "MAIN." + pvc.Address[i];
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
                        if (!pvc.Address[i].StartsWith("MAIN.")) pvc.Address[i] = "MAIN." + pvc.Address[i];
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
                        if (!pvc.Address[i].StartsWith("MAIN.")) pvc.Address[i] = "MAIN." + pvc.Address[i];
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
                        if (!pvc.Address[i].StartsWith("MAIN.")) pvc.Address[i] = "MAIN." + pvc.Address[i];
                        pvc.Type[i] = control.Tag.ToString().Split(';')[1];
                        i++;
                    }
                }
                //pvc.ControlsName[0]=
                //pvc.Address[0] = "MAIN.Data_Bool1";
                //pvc.Type[0] = "Bool";

                //pvc.Address[1] = "MAIN.Data_Byte";
                //pvc.Type[1] = "Byte";

                //pvc.Address[2] = "MAIN.Data_Int";
                //pvc.Type[2] = "Int";

                //pvc.Address[3] = "MAIN.Data_Array";
                //pvc.Type[3] = "Array";

                //pvc.Address[4] = "MAIN.Data_Real";
                //pvc.Type[4] = "Real";

                //pvc.Address[5] = "MAIN.Data_String";
                //pvc.Type[5] = "String";

                //pvc.Address[6] = "MAIN.Data_Struct";
                //pvc.Type[6] = "Struct1";

                //pvc.Address[7] = "MAIN.Data_Struct";
                //pvc.Type[7] = "Struct2";
            }
            catch
            {
                MessageBoxEx.Show("地址初始化异常！", "异常");
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
                        SetValue(control);
                    }
                    foreach (Control control in this.groupPanel1.Controls)
                    {
                        SetValue(control);
                    }
                    foreach (Control control in this.groupPanel2.Controls)
                    {
                        SetValue(control);
                    }
                    foreach (Control control in this.groupPanel3.Controls)
                    {
                        SetValue(control);
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
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
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

                    //表格
                    if (control is DevComponents.DotNetBar.SuperGrid.SuperGridControl)
                    {

                        Struct4 stru = new Struct4();
                        stru = (Struct4)pvc.value[index];
                        ShowTable(stru);
                    }
                }
            }
            catch(Exception r)
            {
                MessageBox.Show(control.Name );
            }
        }
        /// <summary>
        /// 显示表格
        /// </summary>
        private void ShowTable(Struct4 stru)
        {
            try
            {
                this.sgc1.PrimaryGrid.Rows.Clear();
                
                for (int i = 0; i < stru.fVal.Length; i++)
                {
                    this.sgc1.PrimaryGrid.InsertRow(i);
                    GridRow gr = this.sgc1.PrimaryGrid.Rows[i] as GridRow;
                    //序号
                    gr[0].Value = stru.fVal[i];
                    //日期
                    gr[1].Value = stru.byVal[i];// "2020-" + (i / 2 + 1).ToString("00") + "-01 12:02:01";
                    //批次
                    gr[2].Value = stru.nData[i];
                    //工位
                    gr[3].Value = (i + 1) * 10 - 2;
                    //次数
                    gr[4].Value = (i + 3) * 2 - 3;
                    //优先级
                    gr[5].Value = i / 5;
                    //插队标识
                    if (i == 10)
                        gr[6].Value = 1;
                    else
                        gr[6].Value = 0;
                }
                this.sgc1.Refresh();
            }
            catch(Exception r)
            {
                MessageBox.Show(r.Message);
            }
        }

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
        private void boolChange(Control T,Control F)
        {
            int index = Array.IndexOf(pvc.ControlsName, T.Name);
            tas.WriterData(pvc.Address[index], true);
            index = Array.IndexOf(pvc.ControlsName, F.Name);
            tas.WriterData(pvc.Address[index], false);
        }

        private void buttonX40_Click(object sender, EventArgs e)
        {
            boolChange(buttonX40, buttonX41);
        }

        private void buttonX41_Click(object sender, EventArgs e)
        {
            boolChange(buttonX41, buttonX40);
        }

        private void buttonX43_Click(object sender, EventArgs e)
        {
            boolChange(buttonX43, buttonX42);
        }

        private void buttonX42_Click(object sender, EventArgs e)
        {
            boolChange(buttonX42, buttonX43);
        }

        private void buttonX45_Click(object sender, EventArgs e)
        {
            boolChange(buttonX45, buttonX44);
        }

        private void buttonX44_Click(object sender, EventArgs e)
        {
            boolChange(buttonX44, buttonX45);
        }

        private void labelX17_Click(object sender, EventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, labelX17.Name);
            tas.WriterData(pvc.Address[index], !(bool)(pvc.value[index]));
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            boolChange(buttonX3, buttonX4);
        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
            boolChange(buttonX4, buttonX3);
        }

        private void buttonX5_Click(object sender, EventArgs e)
        {
            boolChange(buttonX5, buttonX6);
        }

        private void buttonX6_Click(object sender, EventArgs e)
        {
            boolChange(buttonX6, buttonX5);
        }

        private void buttonX10_Click(object sender, EventArgs e)
        {
            boolChange(buttonX10, buttonX9);
        }

        private void buttonX9_Click(object sender, EventArgs e)
        {
            boolChange(buttonX9, buttonX10);
        }

        private void buttonX8_Click(object sender, EventArgs e)
        {
            boolChange(buttonX8, buttonX7);
        }

        private void buttonX7_Click(object sender, EventArgs e)
        {
            boolChange(buttonX7, buttonX8);
        }

        private void buttonX14_Click(object sender, EventArgs e)
        {
            boolChange(buttonX14, buttonX13);
        }

        private void buttonX13_Click(object sender, EventArgs e)
        {
            boolChange(buttonX13, buttonX14);
        }

        private void buttonX12_Click(object sender, EventArgs e)
        {
            boolChange(buttonX12, buttonX11);
        }

        private void buttonX11_Click(object sender, EventArgs e)
        {
            boolChange(buttonX11, buttonX12);
        }

        private void buttonX26_Click(object sender, EventArgs e)
        {
            boolChange(buttonX26, buttonX25);
        }

        private void buttonX25_Click(object sender, EventArgs e)
        {
            boolChange(buttonX25, buttonX26);
        }

        private void buttonX24_Click(object sender, EventArgs e)
        {
            boolChange(buttonX24, buttonX23);
        }

        private void buttonX23_Click(object sender, EventArgs e)
        {
            boolChange(buttonX23, buttonX24);
        }

        private void buttonX22_Click(object sender, EventArgs e)
        {
            boolChange(buttonX22, buttonX21);
        }

        private void buttonX21_Click(object sender, EventArgs e)
        {
            boolChange(buttonX21, buttonX22);
        }

        private void buttonX20_Click(object sender, EventArgs e)
        {
            boolChange(buttonX20, buttonX19);
        }

        private void buttonX19_Click(object sender, EventArgs e)
        {
            boolChange(buttonX19, buttonX20);
        }

        private void buttonX18_Click(object sender, EventArgs e)
        {
            boolChange(buttonX18, buttonX17);
        }

        private void buttonX17_Click(object sender, EventArgs e)
        {
            boolChange(buttonX17, buttonX18);
        }

        private void buttonX16_Click(object sender, EventArgs e)
        {
            boolChange(buttonX16, buttonX15);
        }

        private void buttonX15_Click(object sender, EventArgs e)
        {
            boolChange(buttonX15, buttonX16);
        }

        private void buttonX38_Click(object sender, EventArgs e)
        {
            boolChange(buttonX38, buttonX37);
        }

        private void buttonX37_Click(object sender, EventArgs e)
        {
            boolChange(buttonX37, buttonX38);
        }

        private void buttonX36_Click(object sender, EventArgs e)
        {
            boolChange(buttonX36, buttonX35);
        }

        private void buttonX35_Click(object sender, EventArgs e)
        {
            boolChange(buttonX35, buttonX36);
        }

        private void buttonX34_Click(object sender, EventArgs e)
        {
            boolChange(buttonX34, buttonX33);
        }

        private void buttonX33_Click(object sender, EventArgs e)
        {
            boolChange(buttonX33, buttonX34);
        }

        private void buttonX32_Click(object sender, EventArgs e)
        {
            boolChange(buttonX32, buttonX31);
        }

        private void buttonX31_Click(object sender, EventArgs e)
        {
            boolChange(buttonX31, buttonX32);
        }

        private void buttonX28_Click(object sender, EventArgs e)
        {
            boolChange(buttonX28, buttonX27);
        }

        private void buttonX27_Click(object sender, EventArgs e)
        {
            boolChange(buttonX27, buttonX28);
        }

        private void buttonX30_Click(object sender, EventArgs e)
        {
            boolChange(buttonX30, buttonX29);
        }

        private void buttonX29_Click(object sender, EventArgs e)
        {
            boolChange(buttonX29, buttonX30);
        }

        private void labelX26_Click(object sender, EventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, labelX26.Name);
            tas.WriterData(pvc.Address[index], !(bool)(pvc.value[index]));
        }

        private void labelX31_Click(object sender, EventArgs e)
        {
            int index = Array.IndexOf(pvc.ControlsName, labelX31.Name);
            tas.WriterData(pvc.Address[index], !(bool)(pvc.value[index]));
        }
    }
}
