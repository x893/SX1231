namespace SemtechLib.Devices.SX1231.Forms
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class TemperatureCalibrationForm : Form
    {
        private Button btnOk;
        private IContainer components;
        private Label label1;
        private Label label2;
        private Label label3;
        private NumericUpDown nudTempRoom;

        public TemperatureCalibrationForm()
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(TemperatureCalibrationForm));
            this.label1 = new Label();
            this.nudTempRoom = new NumericUpDown();
            this.label2 = new Label();
            this.label3 = new Label();
            this.btnOk = new Button();
            this.nudTempRoom.BeginInit();
            base.SuspendLayout();
            this.label1.AutoSize = true;
            this.label1.Location = new Point(12, 0x4d);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x7d, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Actual room temperature:";
            this.nudTempRoom.Location = new Point(0x8f, 0x49);
            int[] bits = new int[4];
            bits[0] = 0x55;
            this.nudTempRoom.Maximum = new decimal(bits);
            int[] numArray2 = new int[4];
            numArray2[0] = 40;
            numArray2[3] = -2147483648;
            this.nudTempRoom.Minimum = new decimal(numArray2);
            this.nudTempRoom.Name = "nudTempRoom";
            this.nudTempRoom.Size = new Size(0x27, 20);
            this.nudTempRoom.TabIndex = 2;
            int[] numArray3 = new int[4];
            numArray3[0] = 0x19;
            this.nudTempRoom.Value = new decimal(numArray3);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0xbc, 0x4d);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x12, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "\x00b0C";
            this.label3.AutoSize = true;
            this.label3.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label3.Location = new Point(13, 10);
            this.label3.MaximumSize = new Size(0xee, 0);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0xda, 0x33);
            this.label3.TabIndex = 0;
            this.label3.Text = "Please enter the actual room temperature measured on an auxiliary thermometer!";
            this.label3.TextAlign = ContentAlignment.MiddleCenter;
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new Point(0x55, 0x63);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new Size(0x4b, 0x17);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0xf4, 0x85);
            base.Controls.Add(this.btnOk);
            base.Controls.Add(this.nudTempRoom);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.label3);
            this.DoubleBuffered = true;
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            base.Icon = (Icon) resources.GetObject("$this.Icon");
            base.KeyPreview = true;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "TemperatureCalibrationForm";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Temperature Calibration";
            this.nudTempRoom.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public decimal TempValueRoom
        {
            get
            {
                return this.nudTempRoom.Value;
            }
            set
            {
                this.nudTempRoom.Value = value;
            }
        }
    }
}

