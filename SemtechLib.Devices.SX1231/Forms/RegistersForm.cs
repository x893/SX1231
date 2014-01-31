namespace SemtechLib.Devices.SX1231.Forms
{
    using SemtechLib.Devices.SX1231;
    using SemtechLib.Devices.SX1231.Controls;
    using SemtechLib.General;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;

    public class RegistersForm : Form, INotifyPropertyChanged
    {
        private ApplicationSettings appSettings;
        private IContainer components;
        private Panel panel1;
        private bool registersFormEnabled;
        private RegisterTableControl registerTableControl1;
        private ToolStripStatusLabel ssLblStatus;
        private StatusStrip statusStrip1;
        private SemtechLib.Devices.SX1231.SX1231 sx1231;

        public event PropertyChangedEventHandler PropertyChanged;

        public RegistersForm()
        {
            this.InitializeComponent();
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(RegistersForm));
            this.statusStrip1 = new StatusStrip();
            this.ssLblStatus = new ToolStripStatusLabel();
            this.panel1 = new Panel();
            this.registerTableControl1 = new RegisterTableControl();
            this.statusStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            base.SuspendLayout();
            this.statusStrip1.Items.AddRange(new ToolStripItem[] { this.ssLblStatus });
            this.statusStrip1.Location = new Point(0, 0xf4);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new Size(0x124, 0x16);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            this.ssLblStatus.Name = "ssLblStatus";
            this.ssLblStatus.Size = new Size(11, 0x11);
            this.ssLblStatus.Text = "-";
            this.panel1.AutoSize = true;
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.Controls.Add(this.registerTableControl1);
            this.panel1.Dock = DockStyle.Fill;
            this.panel1.Location = new Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x124, 0xf4);
            this.panel1.TabIndex = 0;
            this.registerTableControl1.AutoSize = true;
            this.registerTableControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.registerTableControl1.Location = new Point(3, 3);
            this.registerTableControl1.Name = "registerTableControl1";
            this.registerTableControl1.Size = new Size(0xd0, 0x19);
            this.registerTableControl1.Split = 4;
            this.registerTableControl1.TabIndex = 0;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            base.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            base.ClientSize = new Size(0x124, 0x10a);
            base.Controls.Add(this.panel1);
            base.Controls.Add(this.statusStrip1);
            this.DoubleBuffered = true;
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            base.Icon = (Icon) resources.GetObject("$this.Icon");
            base.KeyPreview = true;
            base.MaximizeBox = false;
            base.Name = "RegistersForm";
            this.Text = "SX1231 Registers display";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
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

        private void OnError(byte status, string message)
        {
            if (status != 0)
            {
                this.ssLblStatus.Text = "ERROR: " + message;
            }
            else
            {
                this.ssLblStatus.Text = message;
            }
            this.Refresh();
        }

        private void OnPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        private void RegistersForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                this.appSettings.SetValue("RegistersTop", base.Top.ToString());
                this.appSettings.SetValue("RegistersLeft", base.Left.ToString());
            }
            catch (Exception)
            {
            }
        }

        private void RegistersForm_Load(object sender, EventArgs e)
        {
            string s = this.appSettings.GetValue("RegistersTop");
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
            s = this.appSettings.GetValue("RegistersLeft");
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
        }

        private void SX1231_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string str;
            if (((str = e.PropertyName) != null) && (str == "Version"))
            {
                this.registerTableControl1.Registers = this.sx1231.Registers;
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

        public bool RegistersFormEnabled
        {
            get
            {
                return this.registersFormEnabled;
            }
            set
            {
                this.registersFormEnabled = value;
                this.panel1.Enabled = value;
                this.OnPropertyChanged("RegistersFormEnabled");
            }
        }

        public SemtechLib.Devices.SX1231.SX1231 SX1231
        {
            set
            {
                try
                {
                    this.sx1231 = value;
                    this.sx1231.PropertyChanged += new PropertyChangedEventHandler(this.SX1231_PropertyChanged);
                    this.registerTableControl1.Registers = this.sx1231.Registers;
                    this.sx1231.ReadRegisters();
                }
                catch (Exception exception)
                {
                    this.OnError(1, exception.Message);
                }
            }
        }
    }
}

