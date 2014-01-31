namespace SemtechLib.Devices.SX1231.Forms
{
    using SemtechLib.Devices.SX1231;
    using SemtechLib.Devices.SX1231.General;
    using SemtechLib.General;
    using SemtechLib.General.Events;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class PacketLogForm : Form
    {
        private ApplicationSettings appSettings;
        private Button btnClose;
        private Button btnLogBrowseFile;
        private CheckBox cBtnLogOnOff;
        private IContainer components;
        private GroupBox groupBox5;
        private Label lblCommandsLogMaxSamples;
        private PacketLog log = new PacketLog();
        private ProgressBar pBarLog;
        private string previousValue;
        private SaveFileDialog sfLogSaveFileDlg;
        private SemtechLib.Devices.SX1231.SX1231 sx1231;
        private TableLayoutPanel tableLayoutPanel3;
        private TextBox tBoxLogMaxSamples;
        private int tickStart = Environment.TickCount;

        public PacketLogForm()
        {
            this.InitializeComponent();
            this.log.PropertyChanged += new PropertyChangedEventHandler(this.log_PropertyChanged);
            this.log.Stoped += new EventHandler(this.log_Stoped);
            this.log.ProgressChanged += new ProgressEventHandler(this.log_ProgressChanged);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            base.Close();
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

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(PacketLogForm));
            this.groupBox5 = new GroupBox();
            this.btnLogBrowseFile = new Button();
            this.pBarLog = new ProgressBar();
            this.tableLayoutPanel3 = new TableLayoutPanel();
            this.tBoxLogMaxSamples = new TextBox();
            this.lblCommandsLogMaxSamples = new Label();
            this.cBtnLogOnOff = new CheckBox();
            this.btnClose = new Button();
            this.sfLogSaveFileDlg = new SaveFileDialog();
            this.groupBox5.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            base.SuspendLayout();
            this.groupBox5.Controls.Add(this.btnLogBrowseFile);
            this.groupBox5.Controls.Add(this.pBarLog);
            this.groupBox5.Controls.Add(this.tableLayoutPanel3);
            this.groupBox5.Controls.Add(this.cBtnLogOnOff);
            this.groupBox5.Location = new Point(12, 12);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new Size(0xd1, 0x67);
            this.groupBox5.TabIndex = 4;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Log control";
            this.btnLogBrowseFile.Location = new Point(15, 70);
            this.btnLogBrowseFile.Name = "btnLogBrowseFile";
            this.btnLogBrowseFile.Size = new Size(0x4b, 0x17);
            this.btnLogBrowseFile.TabIndex = 2;
            this.btnLogBrowseFile.Text = "Browse...";
            this.btnLogBrowseFile.UseVisualStyleBackColor = true;
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
            this.lblCommandsLogMaxSamples.Location = new Point(3, 0);
            this.lblCommandsLogMaxSamples.Name = "lblCommandsLogMaxSamples";
            this.lblCommandsLogMaxSamples.Size = new Size(0x55, 0x17);
            this.lblCommandsLogMaxSamples.TabIndex = 0;
            this.lblCommandsLogMaxSamples.Text = "Max samples:";
            this.lblCommandsLogMaxSamples.TextAlign = ContentAlignment.MiddleLeft;
            this.cBtnLogOnOff.Appearance = Appearance.Button;
            this.cBtnLogOnOff.CheckAlign = ContentAlignment.MiddleCenter;
            this.cBtnLogOnOff.Location = new Point(0x77, 70);
            this.cBtnLogOnOff.Name = "cBtnLogOnOff";
            this.cBtnLogOnOff.Size = new Size(0x4b, 0x17);
            this.cBtnLogOnOff.TabIndex = 3;
            this.cBtnLogOnOff.Text = "Start";
            this.cBtnLogOnOff.TextAlign = ContentAlignment.MiddleCenter;
            this.cBtnLogOnOff.UseVisualStyleBackColor = true;
            this.cBtnLogOnOff.CheckedChanged += new EventHandler(this.cBtnLogOnOff_CheckedChanged);
            this.btnClose.Location = new Point(0x4f, 0x79);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new Size(0x4b, 0x17);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new EventHandler(this.btnClose_Click);
            this.sfLogSaveFileDlg.DefaultExt = "*.log";
            this.sfLogSaveFileDlg.Filter = "Log Files(*.log)|*.log|AllFiles(*.*)|*.*";
            base.AcceptButton = this.btnClose;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0xe9, 0x9b);
            base.Controls.Add(this.btnClose);
            base.Controls.Add(this.groupBox5);
            this.DoubleBuffered = true;
            base.Icon = (Icon) resources.GetObject("$this.Icon");
            base.KeyPreview = true;
            base.MaximizeBox = false;
            base.Name = "PacketLogForm";
            base.Opacity = 0.9;
            this.Text = "Packet Log";
            base.FormClosed += new FormClosedEventHandler(this.PacketLogForm_FormClosed);
            base.Load += new EventHandler(this.PacketLogForm_Load);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
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

        private void OnError(byte status, string message)
        {
            this.Refresh();
        }

        private void PacketLogForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                this.appSettings.SetValue("PacketLogTop", base.Top.ToString());
                this.appSettings.SetValue("PacketLogLeft", base.Left.ToString());
                this.appSettings.SetValue("PacketLogPath", this.log.Path);
                this.appSettings.SetValue("PacketLogFileName", this.log.FileName);
                this.appSettings.SetValue("PacketLogMaxSamples", this.log.MaxSamples.ToString());
            }
            catch (Exception)
            {
            }
        }

        private void PacketLogForm_Load(object sender, EventArgs e)
        {
            try
            {
                string s = this.appSettings.GetValue("PacketLogTop");
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
                s = this.appSettings.GetValue("PacketLogLeft");
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
                s = this.appSettings.GetValue("PacketLogPath");
                if (s == null)
                {
                    s = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    this.appSettings.SetValue("PacketLogPath", s);
                }
                this.log.Path = s;
                s = this.appSettings.GetValue("PacketLogFileName");
                if (s == null)
                {
                    s = "sx1231-pkt.log";
                    this.appSettings.SetValue("PacketLogFileName", s);
                }
                this.log.FileName = s;
                s = this.appSettings.GetValue("PacketLogMaxSamples");
                if (s == null)
                {
                    s = "1000";
                    this.appSettings.SetValue("PacketLogMaxSamples", s);
                }
                this.log.MaxSamples = ulong.Parse(s);
            }
            catch (Exception exception)
            {
                this.OnError(1, exception.Message);
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

        public PacketLog Log
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
                }
            }
        }

        private delegate void SX1231DataChangedDelegate(object sender, PropertyChangedEventArgs e);
    }
}

