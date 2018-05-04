using System;
using System.Collections.Generic;
using System.Text;
using RepetierHostExtender.interfaces;
using RepetierHostExtender.geom;
using RepetierHostExtender.basic;
using System.Windows.Forms;
using System.IO.Ports;
using System.Timers;
using d3plugin.Properties;

namespace d3plugin
{
    public partial class handler : UserControl, IHostComponent
    {
        private System.ComponentModel.IContainer components;
        public SerialPort serialPort1;
        private ComboBox comboBox1;
        private ComboBox comboBox2;
        private Label label1;
        private Button button1;
        private Label label2;
        public TextBox textBox1;
        private Button button2;
        public TextBox textBox2;
        private Button button3;
        private IHost host;
        public DateTime startTime, endTime;
        //public ProgressType progress;
        public string stage;
        

        #region 跨线程操作代理
        private delegate void setCallback(string text); 

        private void setTextBox1(string text)
        {
            // InvokeRequired需要比较调用线程ID和创建线程ID
            // 如果它们不相同则返回true
            if (this.textBox1.InvokeRequired)
            {
                setCallback d = new setCallback(setTextBox1);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.textBox1.Text = text;
            }
        }

        private void setTextBox2(string text)
        {
            if (this.textBox2.InvokeRequired)
            {
                setCallback d = new setCallback(setTextBox2);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.textBox2.Text = text;
            }
        }

        private void setButtonText(string text)
        {
            if (this.button2.InvokeRequired)
            {
                setCallback d = new setCallback(setButtonText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.button2.Text = text;
            }
        }

        #endregion

        public handler()
        {
            InitializeComponent();
        }

        #region 插件代码
        public void Connect(IHost _host)
        {
            host = _host;
            checkPort();
            hostEventHandler();
            stage = "stopped";
        }
        public ThreeDView Associated3DView
        {
            get
            {
                return null;
            }
        }
        public Control ComponentControl
        {
            get
            {
                return this;
            }
        }
        public string ComponentDescription
        {
            get
            {
                return ("D3plugin");
            }
        }
        public string ComponentName
        {
            get
            {
                return ("D3plugin");
            }
        }
        public int ComponentOrder
        {
            get
            {
                return 8000;
            }
        }
        public PreferredComponentPositions PreferredPosition
        {
            get
            {
                return PreferredComponentPositions.SIDEBAR;
            }
        }
        public void ComponentActivated()
        {

        }
        #endregion

        #region 窗体代码
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // serialPort1
            // 
            this.serialPort1.DtrEnable = true;
            this.serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort1_DataReceived);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(96, 44);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 20);
            this.comboBox1.TabIndex = 0;
            // 
            // comboBox2
            // 
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "9600",
            "19200",
            "38400",
            "57600",
            "115200"});
            this.comboBox2.Location = new System.Drawing.Point(246, 44);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(121, 20);
            this.comboBox2.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "编码器串口";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(111, 99);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "连接设备";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 152);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "工作状态";
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(81, 149);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.textBox1.Size = new System.Drawing.Size(246, 84);
            this.textBox1.TabIndex = 5;
            this.textBox1.Text = "未工作";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(167, 253);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "开始";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(218, 99);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 7;
            this.button3.Text = "刷新串口";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(12, 296);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.textBox2.Size = new System.Drawing.Size(406, 474);
            this.textBox2.TabIndex = 8;
            this.textBox2.Text = "debug";
            this.textBox2.Click += new System.EventHandler(this.textBox2_Click);
            this.textBox2.MouseClick += new System.Windows.Forms.MouseEventHandler(this.textBox2_MouseClick);
            // 
            // handler
            // 
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.comboBox1);
            this.Name = "handler";
            this.Size = new System.Drawing.Size(437, 795);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "连接设备")
            {
                if (serialPort1.IsOpen == true)
                {
                    serialPort1.Close();
                }
                try
                {
                    serialPort1.PortName = comboBox1.Text;
                    serialPort1.BaudRate = Convert.ToInt32(comboBox2.Text);
                    serialPort1.Open();
                    serialPort1.DiscardInBuffer(); //清除接收缓冲区
                    serialPort1.DiscardOutBuffer();//清除发送缓冲区
                    MessageBox.Show("开启编码器串口成功", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    button1.Text = "关闭串口";
                    button3.Enabled = false;
                    comboBox1.Enabled = false;
                    comboBox2.Enabled = false;
                }
                catch
                {
                    MessageBox.Show("开启编码器串口失败", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                if (serialPort1.IsOpen == true)
                {
                    serialPort1.Close();
                }
                button1.Text = "连接设备";
                button3.Enabled = true;
                comboBox1.Enabled = true;
                comboBox2.Enabled = true;
            }
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (stage == "started")
            {
                setError();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            checkPort();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Text == "开始")
            {
                setStart();
            }
            else 
            {
                setStop();
            }
        }

        #region get serialPort
        public void checkPort()
        {
            serialPort1.Encoding = Encoding.ASCII;
            string[] names = SerialPort.GetPortNames();
            for (byte i = 0; i < names.Length; i++)
            {
                serialPort1.PortName = names[i];

                if (serialPort1.IsOpen == false)
                {
                    try
                    {
                        serialPort1.Open();
                    }
                    catch
                    {
                        continue;
                    }
                }

                if (serialPort1.IsOpen == true)
                {
                    comboBox1.Items.AddRange(new object[] { serialPort1.PortName });
                    serialPort1.Close();
                }
            }
            if (comboBox1.Items == null)
            {
                MessageBox.Show("没有发现可用串口或者被占用!", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        #endregion

        #region hook event handle
        public void hostEventHandler()
        {
            try
            {
                host.Connection.eventConnectionChange += onEventConnectionChange;
                host.ProgressChanged += onProgressChangedEvent;
                host.JobFinishedEvent += onJobFinishedEvent;
                host.JobStoppedEvent += onJobStoppedEvent;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        #endregion

        private void onEventConnectionChange(string msg)
        {
            setTextBox2(textBox2.Text + "\r\nConnectionChangemsg  " + msg);
        }

        private void onProgressChangedEvent(ProgressType _progress, double arg)
        {
            Printjob _printjob = new Printjob(host.Connection, host);
            //TemperatureEntry _TemperatureEntry = new TemperatureEntry
            if ( stage == "stopped")
            {
                stage = "heatingbed";
            }
            else if( stage == "heatingbed" )
            {
                stage = "dojob";
            }
            else if( stage != "started" )
            {
                setTextBox2(textBox2.Text + "\r\n ETA:  " + _printjob.ETA);
                setStart();
                setTextBox2(textBox2.Text + "\r\nprogress  " + _progress.ToString() + "    " + arg.ToString());
            }
        }

        private void onJobFinishedEvent()
        {
            setStop();
            setTextBox2(textBox2.Text + "\r\nJobFinished  ");
        }

        private void onJobStoppedEvent()
        {
            setStop();
            setTextBox2(textBox2.Text + "\r\nJobStopped  ");
        }


        private void setError()
        {
            endTime = DateTime.Now;
            setTextBox1("出错\r\n开始时间：" + startTime.ToString() + "\r\n结束时间：" + endTime.ToString() + "\r\n历时：" + (endTime - startTime).ToString());
            setButtonText("重置");
            host.Connection.injectManualCommandFirst("@pause"); 
            //host.Connection.injectManualCommandFirst("M1");
            //host.SetJobStopped();
            host.SetPrinterAction("pause");
            host.FireNamedEvent("core:printjobPaused", "pause");
            stage = "error";
            MessageBox.Show("打印出错", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void setStop()
        {
            endTime = DateTime.Now;
            setTextBox1("未工作\r\n开始时间：" + startTime.ToString() + "\r\n结束时间：" + endTime.ToString() + "\r\n历时：" + (endTime - startTime).ToString());
            setTextBox2(textBox2.Text + "\r\n" + host.Connection.connector.IsConnected().ToString() + host.IsJobRunning.ToString());  // to remove
            setButtonText("开始");
            serialPort1.WriteLine("0");
            stage = "stopped";
        }

        private void setStart()
        {
            if (serialPort1.IsOpen)
            {
                startTime = DateTime.Now;
                setTextBox1("正在工作\r\n开始时间:  " + startTime.ToString());
                setTextBox2(textBox2.Text + "\r\nconnetion  " + host.Connection.connector.IsConnected().ToString() + "  jobrunning  " + host.IsJobRunning.ToString());  // to remove
                setButtonText("停止");
                serialPort1.WriteLine("1");
                stage = "started";
            }
            else
            {
               // MessageBox.Show("编码器串口未开启", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            
        }

        private void textBox2_MouseClick(object sender, MouseEventArgs e)
        {
//            setError();
            setTextBox2(null);
        }

        public static bool Delay(int delayTime)
        {
            DateTime now = DateTime.Now;
            int s;
            do
            {
                TimeSpan spand = DateTime.Now - now;
                s = spand.Seconds;
                Application.DoEvents();
            }
            while (s < delayTime);
            return true;
        }
    }
}
