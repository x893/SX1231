namespace SemtechLib.Devices.SX1231.Forms
{
    using SemtechLib.Controls;
    using SemtechLib.Devices.SX1231;
    using SemtechLib.Devices.SX1231.Controls;
    using SemtechLib.Devices.SX1231.General;
    using SemtechLib.General;
    using SemtechLib.General.Events;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;
    using ZedGraph;

    public class RssiAnalyserForm : Form
    {
        private ApplicationSettings appSettings;
        private Button btnLogBrowseFile;
        private CheckBox cBtnLogOnOff;
        private IContainer components;
        private RssiGraphControl graph;
        private GroupBoxEx groupBox5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label55;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblCommandsLogMaxSamples;
        private DataLog log = new DataLog();
        private NumericUpDownEx nudRssiThresh;
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private ProgressBar pBarLog;
        private string previousValue;
        private RadioButton rBtnRssiAutoThreshOff;
        private RadioButton rBtnRssiAutoThreshOn;
        private SaveFileDialog sfLogSaveFileDlg;
        private SemtechLib.Devices.SX1231.SX1231 sx1231;
        private TableLayoutPanel tableLayoutPanel3;
        private TextBox tBoxLogMaxSamples;
        private int tickStart = Environment.TickCount;
        private double time;

        public RssiAnalyserForm()
        {
            this.InitializeComponent();
            this.graph.MouseWheel += new MouseEventHandler(this.graph_MouseWheel);
            this.log.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(this.log_PropertyChanged);
            this.log.Stoped += new EventHandler(this.log_Stoped);
            this.log.ProgressChanged += new ProgressEventHandler(this.log_ProgressChanged);
        }

        private void btnLogBrowseFile_Click(object sender, EventArgs e)
        {
            this.OnError(0, "-");
            try
            {
                this.sfLogSaveFileDlg.InitialDirectory = this.log.Path;
                this.sfLogSaveFileDlg.FileName = this.log.FileName;
                if (this.sfLogSaveFileDlg.ShowDialog() == DialogResult.OK)
                {
                    string[] strArray = this.sfLogSaveFileDlg.FileName.Split(new char[] { '\\' });
                    this.log.FileName = strArray[strArray.Length - 1];
                    this.log.Path = "";
                    int index = 0;
                    while (index < (strArray.Length - 2))
                    {
                        this.log.Path = this.log.Path + strArray[index] + @"\";
                        index++;
                    }
                    this.log.Path = this.log.Path + strArray[index];
                }
            }
            catch (Exception exception)
            {
                this.OnError(1, exception.Message);
            }
        }

        private void cBtnLogOnOff_CheckedChanged(object sender, EventArgs e)
        {
            this.OnError(0, "-");
            try
            {
                if (this.cBtnLogOnOff.Checked)
                {
                    this.cBtnLogOnOff.Text = "Stop";
                    this.tBoxLogMaxSamples.Enabled = false;
                    this.btnLogBrowseFile.Enabled = false;
                    this.log.Start();
                }
                else
                {
                    this.cBtnLogOnOff.Text = "Start";
                    this.tBoxLogMaxSamples.Enabled = true;
                    this.btnLogBrowseFile.Enabled = true;
                    this.log.Stop();
                }
            }
            catch (Exception exception)
            {
                this.cBtnLogOnOff.Checked = false;
                this.cBtnLogOnOff.Text = "Start";
                this.tBoxLogMaxSamples.Enabled = true;
                this.btnLogBrowseFile.Enabled = true;
                this.log.Stop();
                this.OnError(1, exception.Message);
            }
            finally
            {
                this.UpdateProgressBarStyle();
            }
        }

        private void CreateThreshold()
        {
            GraphPane pane = this.graph.PaneList[0];
            double num = 0.0;
            LineObj item = new LineObj(Color.Green, 0.0, num, 1.0, num);
            item.Location.CoordinateFrame = CoordType.XChartFractionYScale;
            item.IsVisible = true;
            pane.GraphObjList.Add(item);
            pane.AxisChange();
            this.graph.Invalidate();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void graph_MouseWheel(object sender, MouseEventArgs e)
        {
            this.UpdateThreshold((double) this.nudRssiThresh.Value);
        }

        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(RssiAnalyserForm));
            this.panel1 = new Panel();
            this.groupBox5 = new GroupBoxEx();
            this.btnLogBrowseFile = new Button();
            this.pBarLog = new ProgressBar();
            this.tableLayoutPanel3 = new TableLayoutPanel();
            this.tBoxLogMaxSamples = new TextBox();
            this.lblCommandsLogMaxSamples = new System.Windows.Forms.Label();
            this.cBtnLogOnOff = new CheckBox();
            this.panel3 = new Panel();
            this.rBtnRssiAutoThreshOff = new RadioButton();
            this.rBtnRssiAutoThreshOn = new RadioButton();
            this.label6 = new System.Windows.Forms.Label();
            this.nudRssiThresh = new NumericUpDownEx();
            this.label55 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.panel2 = new Panel();
            this.graph = new RssiGraphControl();
            this.sfLogSaveFileDlg = new SaveFileDialog();
            this.panel1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.panel3.SuspendLayout();
            this.nudRssiThresh.BeginInit();
            this.panel2.SuspendLayout();
            base.SuspendLayout();
            this.panel1.Anchor = AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Top;
            this.panel1.BackColor = Color.Black;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.groupBox5);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.nudRssiThresh);
            this.panel1.Controls.Add(this.label55);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Location = new Point(0x229, 0);
            this.panel1.Margin = new Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0xdf, 0x16e);
            this.panel1.TabIndex = 1;
            this.groupBox5.BackColor = Color.Transparent;
            this.groupBox5.Controls.Add(this.btnLogBrowseFile);
            this.groupBox5.Controls.Add(this.pBarLog);
            this.groupBox5.Controls.Add(this.tableLayoutPanel3);
            this.groupBox5.Controls.Add(this.cBtnLogOnOff);
            this.groupBox5.ForeColor = Color.Gray;
            this.groupBox5.Location = new Point(9, 0xc4);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new Size(0xd1, 0x67);
            this.groupBox5.TabIndex = 8;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Log control";
            this.btnLogBrowseFile.BackColor = SystemColors.Control;
            this.btnLogBrowseFile.ForeColor = SystemColors.ControlText;
            this.btnLogBrowseFile.Location = new Point(15, 70);
            this.btnLogBrowseFile.Name = "btnLogBrowseFile";
            this.btnLogBrowseFile.Size = new Size(0x4b, 0x17);
            this.btnLogBrowseFile.TabIndex = 2;
            this.btnLogBrowseFile.Text = "Browse...";
            this.btnLogBrowseFile.UseVisualStyleBackColor = false;
            this.btnLogBrowseFile.Click += new EventHandler(this.btnLogBrowseFile_Click);
            this.pBarLog.Location = new Point(15, 0x33);
            this.pBarLog.Name = "pBarLog";
            this.pBarLog.Size = new Size(0xb3, 13);
            this.pBarLog.Step = 1;
            this.pBarLog.Style = ProgressBarStyle.Continuous;
            this.pBarLog.TabIndex = 1;
            this.tableLayoutPanel3.AutoSize = true;
            this.tableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle());
            this.tableLayoutPanel3.Controls.Add(this.tBoxLogMaxSamples, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.lblCommandsLogMaxSamples, 0, 0);
            this.tableLayoutPanel3.Location = new Point(15, 0x13);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new RowStyle());
            this.tableLayoutPanel3.Size = new Size(0xb3, 0x1a);
            this.tableLayoutPanel3.TabIndex = 0;
            this.tBoxLogMaxSamples.Location = new Point(0x5e, 3);
            this.tBoxLogMaxSamples.Name = "tBoxLogMaxSamples";
            this.tBoxLogMaxSamples.Size = new Size(0x52, 20);
            this.tBoxLogMaxSamples.TabIndex = 1;
            this.tBoxLogMaxSamples.Text = "1000";
            this.tBoxLogMaxSamples.TextAlign = HorizontalAlignment.Center;
            this.tBoxLogMaxSamples.Enter += new EventHandler(this.tBoxLogMaxSamples_Enter);
            this.tBoxLogMaxSamples.Validating += new CancelEventHandler(this.tBoxLogMaxSamples_Validating);
            this.tBoxLogMaxSamples.Validated += new EventHandler(this.tBoxLogMaxSamples_Validated);
            this.lblCommandsLogMaxSamples.ForeColor = Color.Gray;
            this.lblCommandsLogMaxSamples.Location = new Point(3, 0);
            this.lblCommandsLogMaxSamples.Name = "lblCommandsLogMaxSamples";
            this.lblCommandsLogMaxSamples.Size = new Size(0x55, 0x17);
            this.lblCommandsLogMaxSamples.TabIndex = 0;
            this.lblCommandsLogMaxSamples.Text = "Max samples:";
            this.lblCommandsLogMaxSamples.TextAlign = ContentAlignment.MiddleLeft;
            this.cBtnLogOnOff.Appearance = Appearance.Button;
            this.cBtnLogOnOff.BackColor = SystemColors.Control;
            this.cBtnLogOnOff.CheckAlign = ContentAlignment.MiddleCenter;
            this.cBtnLogOnOff.ForeColor = SystemColors.ControlText;
            this.cBtnLogOnOff.Location = new Point(0x77, 70);
            this.cBtnLogOnOff.Name = "cBtnLogOnOff";
            this.cBtnLogOnOff.Size = new Size(0x4b, 0x17);
            this.cBtnLogOnOff.TabIndex = 3;
            this.cBtnLogOnOff.Text = "Start";
            this.cBtnLogOnOff.TextAlign = ContentAlignment.MiddleCenter;
            this.cBtnLogOnOff.UseVisualStyleBackColor = false;
            this.cBtnLogOnOff.CheckedChanged += new EventHandler(this.cBtnLogOnOff_CheckedChanged);
            this.panel3.Anchor = AnchorStyles.None;
            this.panel3.AutoSize = true;
            this.panel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel3.Controls.Add(this.rBtnRssiAutoThreshOff);
            this.panel3.Controls.Add(this.rBtnRssiAutoThreshOn);
            this.panel3.ForeColor = Color.Gray;
            this.panel3.Location = new Point(0x75, 0x91);
            this.panel3.Name = "panel3";
            this.panel3.Size = new Size(0x62, 0x17);
            this.panel3.TabIndex = 5;
            this.rBtnRssiAutoThreshOff.AutoSize = true;
            this.rBtnRssiAutoThreshOff.Location = new Point(50, 3);
            this.rBtnRssiAutoThreshOff.Name = "rBtnRssiAutoThreshOff";
            this.rBtnRssiAutoThreshOff.Size = new Size(0x2d, 0x11);
            this.rBtnRssiAutoThreshOff.TabIndex = 1;
            this.rBtnRssiAutoThreshOff.Text = "OFF";
            this.rBtnRssiAutoThreshOff.UseVisualStyleBackColor = true;
            this.rBtnRssiAutoThreshOff.CheckedChanged += new EventHandler(this.rBtnRssiAutoThreshOff_CheckedChanged);
            this.rBtnRssiAutoThreshOn.AutoSize = true;
            this.rBtnRssiAutoThreshOn.Checked = true;
            this.rBtnRssiAutoThreshOn.Location = new Point(3, 3);
            this.rBtnRssiAutoThreshOn.Name = "rBtnRssiAutoThreshOn";
            this.rBtnRssiAutoThreshOn.Size = new Size(0x29, 0x11);
            this.rBtnRssiAutoThreshOn.TabIndex = 0;
            this.rBtnRssiAutoThreshOn.TabStop = true;
            this.rBtnRssiAutoThreshOn.Text = "ON";
            this.rBtnRssiAutoThreshOn.UseVisualStyleBackColor = true;
            this.rBtnRssiAutoThreshOn.CheckedChanged += new EventHandler(this.rBtnRssiAutoThreshOn_CheckedChanged);
            this.label6.Anchor = AnchorStyles.None;
            this.label6.AutoSize = true;
            this.label6.ForeColor = Color.Gray;
            this.label6.Location = new Point(6, 150);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x69, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "RSSI auto threshold:";
            this.nudRssiThresh.Anchor = AnchorStyles.None;
            this.nudRssiThresh.DecimalPlaces = 1;
            this.nudRssiThresh.Enabled = false;
            int[] bits = new int[4];
            bits[0] = 5;
            bits[3] = 0x10000;
            this.nudRssiThresh.Increment = new decimal(bits);
            this.nudRssiThresh.Location = new Point(0x75, 0xab);
            this.nudRssiThresh.Margin = new Padding(0);
            int[] numArray2 = new int[4];
            this.nudRssiThresh.Maximum = new decimal(numArray2);
            int[] numArray3 = new int[4];
            numArray3[0] = 0x4fb;
            numArray3[3] = -2147418112;
            this.nudRssiThresh.Minimum = new decimal(numArray3);
            this.nudRssiThresh.Name = "nudRssiThresh";
            this.nudRssiThresh.Size = new Size(60, 20);
            this.nudRssiThresh.TabIndex = 7;
            this.nudRssiThresh.ThousandsSeparator = true;
            int[] numArray4 = new int[4];
            numArray4[0] = 0x72;
            numArray4[3] = -2147483648;
            this.nudRssiThresh.Value = new decimal(numArray4);
            this.nudRssiThresh.ValueChanged += new EventHandler(this.nudRssiThresh_ValueChanged);
            this.label55.Anchor = AnchorStyles.None;
            this.label55.AutoSize = true;
            this.label55.BackColor = Color.Transparent;
            this.label55.ForeColor = Color.Gray;
            this.label55.Location = new Point(6, 0xab);
            this.label55.Margin = new Padding(0);
            this.label55.Name = "label55";
            this.label55.Size = new Size(0x36, 13);
            this.label55.TabIndex = 6;
            this.label55.Text = "Threshold";
            this.label55.TextAlign = ContentAlignment.MiddleCenter;
            this.label3.Anchor = AnchorStyles.None;
            this.label3.AutoSize = true;
            this.label3.ForeColor = Color.Green;
            this.label3.Location = new Point(6, 90);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x52, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "RSSI Threshold";
            this.label1.Anchor = AnchorStyles.None;
            this.label1.BackColor = Color.Green;
            this.label1.Location = new Point(0x75, 0x5f);
            this.label1.Margin = new Padding(3, 3, 0, 3);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x19, 2);
            this.label1.TabIndex = 3;
            this.label1.Text = "label7";
            this.label9.Anchor = AnchorStyles.None;
            this.label9.AutoSize = true;
            this.label9.ForeColor = Color.Aqua;
            this.label9.Location = new Point(6, 0x1d);
            this.label9.Margin = new Padding(3);
            this.label9.Name = "label9";
            this.label9.Size = new Size(0x45, 13);
            this.label9.TabIndex = 0;
            this.label9.Text = "RF_PA RSSI";
            this.label9.Visible = false;
            this.label7.Anchor = AnchorStyles.None;
            this.label7.BackColor = Color.Aqua;
            this.label7.Location = new Point(0x75, 0x22);
            this.label7.Margin = new Padding(3);
            this.label7.Name = "label7";
            this.label7.Size = new Size(0x19, 2);
            this.label7.TabIndex = 1;
            this.label7.Text = "label7";
            this.label7.Visible = false;
            this.label5.Anchor = AnchorStyles.None;
            this.label5.AutoSize = true;
            this.label5.ForeColor = Color.Yellow;
            this.label5.Location = new Point(6, 0x30);
            this.label5.Margin = new Padding(3);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x42, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "RF_IO RSSI";
            this.label5.Visible = false;
            this.label4.Anchor = AnchorStyles.None;
            this.label4.BackColor = Color.Yellow;
            this.label4.Location = new Point(0x75, 0x35);
            this.label4.Margin = new Padding(3);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x19, 2);
            this.label4.TabIndex = 1;
            this.label4.Text = "label7";
            this.label4.Visible = false;
            this.label2.Anchor = AnchorStyles.None;
            this.label2.AutoSize = true;
            this.label2.ForeColor = Color.Red;
            this.label2.Location = new Point(6, 0x43);
            this.label2.Margin = new Padding(3);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x20, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "RSSI";
            this.label8.Anchor = AnchorStyles.None;
            this.label8.BackColor = Color.Red;
            this.label8.Location = new Point(0x75, 0x48);
            this.label8.Margin = new Padding(3);
            this.label8.Name = "label8";
            this.label8.Size = new Size(0x19, 2);
            this.label8.TabIndex = 1;
            this.label8.Text = "label7";
            this.panel2.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.panel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.graph);
            this.panel2.Location = new Point(0, 0);
            this.panel2.Margin = new Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(0x229, 0x16e);
            this.panel2.TabIndex = 0;
            this.graph.Dock = DockStyle.Fill;
            this.graph.Location = new Point(0, 0);
            this.graph.Name = "graph";
            this.graph.Size = new Size(0x227, 0x16c);
            this.graph.TabIndex = 0;
            this.sfLogSaveFileDlg.DefaultExt = "*.log";
            this.sfLogSaveFileDlg.Filter = "Log Files(*.log)|*.log|AllFiles(*.*)|*.*";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0x308, 0x16e);
            base.Controls.Add(this.panel2);
            base.Controls.Add(this.panel1);
            base.Icon = (Icon) resources.GetObject("$this.Icon");
            base.Name = "RssiAnalyserForm";
            this.Text = "Rssi analyser";
            base.FormClosed += new FormClosedEventHandler(this.RssiAnalyserForm_FormClosed);
            base.Load += new EventHandler(this.RssiAnalyserForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.nudRssiThresh.EndInit();
            this.panel2.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private bool IsFormLocatedInScreen(Form frm, Screen[] screens)
        {
            int upperBound = screens.GetUpperBound(0);
            bool flag = false;
            for (int i = 0; i <= upperBound; i++)
            {
                if (((frm.Left < screens[i].WorkingArea.Left) || (frm.Top < screens[i].WorkingArea.Top)) || ((frm.Left > screens[i].WorkingArea.Right) || (frm.Top > screens[i].WorkingArea.Bottom)))
                {
                    flag = false;
                }
                else
                {
                    return true;
                }
            }
            return flag;
        }

        private void log_ProgressChanged(object sender, ProgressEventArg e)
        {
            MethodInvoker method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        this.pBarLog.Value = (int) e.Progress;
                    };
                }
                base.BeginInvoke(method, null);
            }
            else
            {
                this.pBarLog.Value = (int) e.Progress;
            }
        }

        private void log_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string str;
            if (((((str = e.PropertyName) != null) && (str != "Path")) && (str != "FileName")) && (str == "MaxSamples"))
            {
                this.tBoxLogMaxSamples.Text = this.log.MaxSamples.ToString();
            }
        }

        private void log_Stoped(object sender, EventArgs e)
        {
            MethodInvoker method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        this.cBtnLogOnOff.Checked = false;
                        this.cBtnLogOnOff.Text = "Start";
                        this.tBoxLogMaxSamples.Enabled = true;
                        this.btnLogBrowseFile.Enabled = true;
                        this.log.Stop();
                        this.UpdateProgressBarStyle();
                    };
                }
                base.BeginInvoke(method, null);
            }
            else
            {
                this.cBtnLogOnOff.Checked = false;
                this.cBtnLogOnOff.Text = "Start";
                this.tBoxLogMaxSamples.Enabled = true;
                this.btnLogBrowseFile.Enabled = true;
                this.log.Stop();
                this.UpdateProgressBarStyle();
            }
        }

        private void nudRssiThresh_ValueChanged(object sender, EventArgs e)
        {
            this.sx1231.SetRssiThresh(this.nudRssiThresh.Value);
        }

        private void OnError(byte status, string message)
        {
            this.Refresh();
        }

        private void OnSX1231PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string propertyName = e.PropertyName;
            if (propertyName != null)
            {
                if (!(propertyName == "RssiAutoThresh"))
                {
                    if (propertyName == "RssiValue")
                    {
                        if (this.sx1231.RfPaSwitchEnabled == 0)
                        {
                            this.time = ((double) (Environment.TickCount - this.tickStart)) / 1000.0;
                            this.graph.UpdateLineGraph(this.time, (double) this.sx1231.RssiValue);
                        }
                    }
                    else if (propertyName == "RfPaSwitchEnabled")
                    {
                        this.label9.Visible = this.sx1231.RfPaSwitchEnabled != 0;
                        this.label7.Visible = this.sx1231.RfPaSwitchEnabled != 0;
                        this.label5.Visible = this.sx1231.RfPaSwitchEnabled != 0;
                        this.label4.Visible = this.sx1231.RfPaSwitchEnabled != 0;
                        this.label2.Visible = this.sx1231.RfPaSwitchEnabled == 0;
                        this.label8.Visible = this.sx1231.RfPaSwitchEnabled == 0;
                    }
                    else if (propertyName == "RfPaRssiValue")
                    {
                        if (this.sx1231.RfPaSwitchEnabled == 1)
                        {
                            this.time = ((double) (Environment.TickCount - this.tickStart)) / 1000.0;
                            this.graph.UpdateLineGraph(1, this.time, (double) this.sx1231.RfPaRssiValue);
                            this.graph.UpdateLineGraph(2, this.time, (double) this.sx1231.RfIoRssiValue);
                        }
                    }
                    else if (propertyName == "RfIoRssiValue")
                    {
                        if (this.sx1231.RfPaSwitchEnabled != 0)
                        {
                            this.time = ((double) (Environment.TickCount - this.tickStart)) / 1000.0;
                            this.graph.UpdateLineGraph(1, this.time, (double) this.sx1231.RfPaRssiValue);
                            this.graph.UpdateLineGraph(2, this.time, (double) this.sx1231.RfIoRssiValue);
                        }
                    }
                    else if (propertyName == "RssiThresh")
                    {
                        if (!this.sx1231.RssiAutoThresh)
                        {
                            this.nudRssiThresh.Value = this.sx1231.RssiThresh;
                        }
                        else if (this.sx1231.AgcReference < -127)
                        {
                            this.nudRssiThresh.Value = -127.5M;
                        }
                        else
                        {
                            this.nudRssiThresh.Value = this.sx1231.AgcReference - this.sx1231.AgcSnrMargin;
                        }
                    }
                }
                else
                {
                    this.rBtnRssiAutoThreshOn.Checked = this.sx1231.RssiAutoThresh;
                    this.rBtnRssiAutoThreshOff.Checked = !this.sx1231.RssiAutoThresh;
                    this.nudRssiThresh.Enabled = !this.sx1231.RssiAutoThresh;
                }
            }
            this.UpdateThreshold((double) this.nudRssiThresh.Value);
        }

        private void rBtnRssiAutoThreshOff_CheckedChanged(object sender, EventArgs e)
        {
            this.sx1231.RssiAutoThresh = this.rBtnRssiAutoThreshOn.Checked;
        }

        private void rBtnRssiAutoThreshOn_CheckedChanged(object sender, EventArgs e)
        {
            this.sx1231.RssiAutoThresh = this.rBtnRssiAutoThreshOn.Checked;
        }

        private void RssiAnalyserForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                this.appSettings.SetValue("RssiAnalyserTop", base.Top.ToString());
                this.appSettings.SetValue("RssiAnalyserLeft", base.Left.ToString());
                this.appSettings.SetValue("LogPath", this.log.Path);
                this.appSettings.SetValue("LogFileName", this.log.FileName);
                this.appSettings.SetValue("LogMaxSamples", this.log.MaxSamples.ToString());
            }
            catch (Exception)
            {
            }
        }

        private void RssiAnalyserForm_Load(object sender, EventArgs e)
        {
            try
            {
                string s = this.appSettings.GetValue("RssiAnalyserTop");
                if (s != null)
                {
                    try
                    {
                        base.Top = int.Parse(s);
                    }
                    catch
                    {
                        MessageBox.Show(this, "Error getting Top value.");
                    }
                }
                s = this.appSettings.GetValue("RssiAnalyserLeft");
                if (s != null)
                {
                    try
                    {
                        base.Left = int.Parse(s);
                    }
                    catch
                    {
                        MessageBox.Show(this, "Error getting Left value.");
                    }
                }
                Screen[] allScreens = Screen.AllScreens;
                if (!this.IsFormLocatedInScreen(this, allScreens))
                {
                    base.Top = allScreens[0].WorkingArea.Top;
                    base.Left = allScreens[0].WorkingArea.Left;
                }
                s = this.appSettings.GetValue("LogPath");
                if (s == null)
                {
                    s = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    this.appSettings.SetValue("LogPath", s);
                }
                this.log.Path = s;
                s = this.appSettings.GetValue("LogFileName");
                if (s == null)
                {
                    s = "sx1231-Rssi.log";
                    this.appSettings.SetValue("LogFileName", s);
                }
                this.log.FileName = s;
                s = this.appSettings.GetValue("LogMaxSamples");
                if (s == null)
                {
                    s = "1000";
                    this.appSettings.SetValue("LogMaxSamples", s);
                }
                this.log.MaxSamples = ulong.Parse(s);
            }
            catch (Exception exception)
            {
                this.OnError(1, exception.Message);
            }
        }

        private void sx1231_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (base.InvokeRequired)
            {
                base.BeginInvoke(new SX1231DataChangedDelegate(this.OnSX1231PropertyChanged), new object[] { sender, e });
            }
            else
            {
                this.OnSX1231PropertyChanged(sender, e);
            }
        }

        private void tBoxLogMaxSamples_Enter(object sender, EventArgs e)
        {
            this.previousValue = this.tBoxLogMaxSamples.Text;
        }

        private void tBoxLogMaxSamples_Validated(object sender, EventArgs e)
        {
            this.log.MaxSamples = ulong.Parse(this.tBoxLogMaxSamples.Text);
        }

        private void tBoxLogMaxSamples_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                Convert.ToUInt64(this.tBoxLogMaxSamples.Text);
            }
            catch (Exception exception)
            {
                object[] objArray = new object[] { exception.Message, "\rInput Format: ", (ulong) 0L, " - ", ulong.MaxValue.ToString() };
                MessageBox.Show(string.Concat(objArray), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.tBoxLogMaxSamples.Text = this.previousValue;
            }
        }

        private void UpdateProgressBarStyle()
        {
            if ((this.log.MaxSamples == 0L) && this.cBtnLogOnOff.Checked)
            {
                this.pBarLog.Style = ProgressBarStyle.Marquee;
            }
            else
            {
                this.pBarLog.Style = ProgressBarStyle.Continuous;
            }
        }

        public void UpdateThreshold(double thres)
        {
            GraphPane pane = this.graph.PaneList[0];
            (pane.GraphObjList[0] as LineObj).Location.Y = thres;
            (pane.GraphObjList[0] as LineObj).Location.Y1 = thres;
            if ((thres < pane.YAxis.Scale.Max) && (thres > pane.YAxis.Scale.Min))
            {
                (pane.GraphObjList[0] as LineObj).IsVisible = true;
            }
            else
            {
                (pane.GraphObjList[0] as LineObj).IsVisible = false;
            }
            pane.AxisChange();
            this.graph.Invalidate();
        }

        public ApplicationSettings AppSettings
        {
            get
            {
                return this.appSettings;
            }
            set
            {
                this.appSettings = value;
            }
        }

        public DataLog Log
        {
            get
            {
                return this.log;
            }
        }

        public SemtechLib.Devices.SX1231.SX1231 SX1231
        {
            set
            {
                if (this.sx1231 != value)
                {
                    this.sx1231 = value;
                    this.Log.SX1231 = value;
                    this.sx1231.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(this.sx1231_PropertyChanged);
                    this.CreateThreshold();
                    if (!this.sx1231.RssiAutoThresh)
                    {
                        this.nudRssiThresh.Value = this.sx1231.RssiThresh;
                    }
                    else if (this.sx1231.AgcReference < -127)
                    {
                        this.nudRssiThresh.Value = -127.5M;
                    }
                    else
                    {
                        this.nudRssiThresh.Value = this.sx1231.AgcReference - this.sx1231.AgcSnrMargin;
                    }
                    this.UpdateThreshold((double) this.nudRssiThresh.Value);
                }
            }
        }

        private delegate void SX1231DataChangedDelegate(object sender, PropertyChangedEventArgs e);
    }
}

