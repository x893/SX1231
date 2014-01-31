using SemtechLib.Controls;
using SemtechLib.Devices.SX1231.Enumerations;
using SemtechLib.Devices.SX1231.Forms;
using SemtechLib.General.Events;
using SemtechLib.General.Interfaces;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace SemtechLib.Devices.SX1231.Controls
{
	public class TemperatureViewControl : UserControl, INotifyDocumentationChanged
	{
		private Button btnCalibrate;
		private IContainer components;
		private ErrorProvider errorProvider;
		private Label label3;
		private Label lblMeasuring;
		private Led ledTempMeasRunning;
		private OperatingModeEnum _mode = OperatingModeEnum.Stdby;
		private Panel panel1;
		private Panel pnlAdcLowPower;
		private RadioButton rBtnAdcLowPowerOff;
		private RadioButton rBtnAdcLowPowerOn;
		private bool _tempCalDone;
		private decimal tempValueRoom = 25.0M;
		private TempCtrl thermometerCtrl;
		private System.Windows.Forms.Timer tmrTempMeasStart;

		public event BooleanEventHandler AdcLowPowerOnChanged;
		public event DocumentationChangedEventHandler DocumentationChanged;
		public event DecimalEventHandler TempCalibrateChanged;

		public TemperatureViewControl()
		{
			InitializeComponent();
		}

		private void btnTempCalibrate_Click(object sender, EventArgs e)
		{
			TemperatureCalibrationForm form = new TemperatureCalibrationForm();
			form.TempValueRoom = TempValueRoom;
			if (form.ShowDialog() == DialogResult.OK)
			{
				try
				{
					Cursor = Cursors.WaitCursor;
					TempValueRoom = form.TempValueRoom;
					OnTempCalibrateChanged(TempValueRoom);
				}
				catch
				{
				}
				finally
				{
					Cursor = Cursors.Default;
				}
			}
		}

		private void control_MouseEnter(object sender, EventArgs e)
		{
			if (sender == thermometerCtrl)
				OnDocumentationChanged(new DocumentationChangedEventArgs("Temperature", "Thermometer"));
			else if (sender == btnCalibrate)
				OnDocumentationChanged(new DocumentationChangedEventArgs("Temperature", "Calibrate"));
			else if ((sender == ledTempMeasRunning) || (sender == lblMeasuring))
				OnDocumentationChanged(new DocumentationChangedEventArgs("Temperature", "Measure running"));
			else if (((sender == panel1) || (sender == rBtnAdcLowPowerOn)) || (sender == rBtnAdcLowPowerOff))
				OnDocumentationChanged(new DocumentationChangedEventArgs("Temperature", "Adc low power"));
		}

		private void control_MouseLeave(object sender, EventArgs e)
		{
			OnDocumentationChanged(new DocumentationChangedEventArgs(".", "Overview"));
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			components = new Container();
			errorProvider = new ErrorProvider(components);
			btnCalibrate = new Button();
			lblMeasuring = new Label();
			tmrTempMeasStart = new System.Windows.Forms.Timer(components);
			pnlAdcLowPower = new Panel();
			rBtnAdcLowPowerOff = new RadioButton();
			rBtnAdcLowPowerOn = new RadioButton();
			label3 = new Label();
			panel1 = new Panel();
			thermometerCtrl = new TempCtrl();
			ledTempMeasRunning = new Led();
			((ISupportInitialize)errorProvider).BeginInit();
			pnlAdcLowPower.SuspendLayout();
			panel1.SuspendLayout();
			base.SuspendLayout();
			errorProvider.ContainerControl = this;
			btnCalibrate.Location = new Point(0x25, 3);
			btnCalibrate.Name = "btnCalibrate";
			btnCalibrate.Size = new Size(0x4b, 0x17);
			btnCalibrate.TabIndex = 0;
			btnCalibrate.Text = "Calibrate";
			btnCalibrate.UseVisualStyleBackColor = true;
			btnCalibrate.MouseLeave += new EventHandler(control_MouseLeave);
			btnCalibrate.Click += new EventHandler(btnTempCalibrate_Click);
			btnCalibrate.MouseEnter += new EventHandler(control_MouseEnter);
			lblMeasuring.AutoSize = true;
			lblMeasuring.Location = new Point(0x22, 0x1d);
			lblMeasuring.Name = "lblMeasuring";
			lblMeasuring.Size = new Size(0x3b, 13);
			lblMeasuring.TabIndex = 1;
			lblMeasuring.Text = "Measuring:";
			lblMeasuring.TextAlign = ContentAlignment.MiddleLeft;
			lblMeasuring.MouseLeave += new EventHandler(control_MouseLeave);
			lblMeasuring.MouseEnter += new EventHandler(control_MouseEnter);
			tmrTempMeasStart.Interval = 0x3e8;
			pnlAdcLowPower.AutoSize = true;
			pnlAdcLowPower.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			pnlAdcLowPower.Controls.Add(rBtnAdcLowPowerOff);
			pnlAdcLowPower.Controls.Add(rBtnAdcLowPowerOn);
			pnlAdcLowPower.Location = new Point(0x2b6, 0x1d3);
			pnlAdcLowPower.Name = "pnlAdcLowPower";
			pnlAdcLowPower.Size = new Size(0x66, 0x17);
			pnlAdcLowPower.TabIndex = 3;
			pnlAdcLowPower.Visible = false;
			pnlAdcLowPower.MouseLeave += new EventHandler(control_MouseLeave);
			pnlAdcLowPower.MouseEnter += new EventHandler(control_MouseEnter);
			rBtnAdcLowPowerOff.AutoSize = true;
			rBtnAdcLowPowerOff.Location = new Point(0x36, 3);
			rBtnAdcLowPowerOff.Name = "rBtnAdcLowPowerOff";
			rBtnAdcLowPowerOff.Size = new Size(0x2d, 0x11);
			rBtnAdcLowPowerOff.TabIndex = 1;
			rBtnAdcLowPowerOff.Text = "OFF";
			rBtnAdcLowPowerOff.UseVisualStyleBackColor = true;
			rBtnAdcLowPowerOff.MouseLeave += new EventHandler(control_MouseLeave);
			rBtnAdcLowPowerOff.MouseEnter += new EventHandler(control_MouseEnter);
			rBtnAdcLowPowerOff.CheckedChanged += new EventHandler(rBtnAdcLowPower_CheckedChanged);
			rBtnAdcLowPowerOn.AutoSize = true;
			rBtnAdcLowPowerOn.Checked = true;
			rBtnAdcLowPowerOn.Location = new Point(3, 3);
			rBtnAdcLowPowerOn.Name = "rBtnAdcLowPowerOn";
			rBtnAdcLowPowerOn.Size = new Size(0x29, 0x11);
			rBtnAdcLowPowerOn.TabIndex = 0;
			rBtnAdcLowPowerOn.TabStop = true;
			rBtnAdcLowPowerOn.Text = "ON";
			rBtnAdcLowPowerOn.UseVisualStyleBackColor = true;
			rBtnAdcLowPowerOn.MouseLeave += new EventHandler(control_MouseLeave);
			rBtnAdcLowPowerOn.MouseEnter += new EventHandler(control_MouseEnter);
			rBtnAdcLowPowerOn.CheckedChanged += new EventHandler(rBtnAdcLowPower_CheckedChanged);
			label3.AutoSize = true;
			label3.Location = new Point(0x25a, 0x1d8);
			label3.Name = "label3";
			label3.Size = new Size(0x53, 13);
			label3.TabIndex = 2;
			label3.Text = "ADC low power:";
			label3.TextAlign = ContentAlignment.MiddleLeft;
			label3.Visible = false;
			label3.MouseLeave += new EventHandler(control_MouseLeave);
			label3.MouseEnter += new EventHandler(control_MouseEnter);
			panel1.Controls.Add(btnCalibrate);
			panel1.Controls.Add(lblMeasuring);
			panel1.Controls.Add(ledTempMeasRunning);
			panel1.Location = new Point(0x145, 0x1bb);
			panel1.Name = "panel1";
			panel1.Size = new Size(0x94, 0x2f);
			panel1.TabIndex = 1;
			panel1.MouseLeave += new EventHandler(control_MouseLeave);
			panel1.MouseEnter += new EventHandler(control_MouseEnter);
			thermometerCtrl.BackColor = Color.Transparent;
			thermometerCtrl.DrawTics = true;
			thermometerCtrl.Enabled = false;
			thermometerCtrl.ForeColor = Color.Red;
			thermometerCtrl.LargeTicFreq = 10;
			thermometerCtrl.Location = new Point(0x145, 3);
			thermometerCtrl.Name = "thermometerCtrl";
			thermometerCtrl.Range.Max = 90.0;
			thermometerCtrl.Range.Min = -40.0;
			thermometerCtrl.Size = new Size(0x94, 0x1b2);
			thermometerCtrl.SmallTicFreq = 5;
			thermometerCtrl.TabIndex = 0;
			thermometerCtrl.Text = "Thermometer";
			thermometerCtrl.Value = 25.0;
			thermometerCtrl.MouseLeave += new EventHandler(control_MouseLeave);
			thermometerCtrl.MouseEnter += new EventHandler(control_MouseEnter);
			ledTempMeasRunning.BackColor = Color.Transparent;
			ledTempMeasRunning.LedColor = Color.Green;
			ledTempMeasRunning.LedSize = new Size(11, 11);
			ledTempMeasRunning.Location = new Point(0x63, 0x1d);
			ledTempMeasRunning.Name = "ledTempMeasRunning";
			ledTempMeasRunning.Size = new Size(15, 15);
			ledTempMeasRunning.TabIndex = 2;
			ledTempMeasRunning.Text = "Measuring";
			ledTempMeasRunning.MouseLeave += new EventHandler(control_MouseLeave);
			ledTempMeasRunning.MouseEnter += new EventHandler(control_MouseEnter);
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(thermometerCtrl);
			base.Controls.Add(pnlAdcLowPower);
			base.Controls.Add(panel1);
			base.Controls.Add(label3);
			base.Name = "TemperatureViewControl";
			base.Size = new Size(0x31f, 0x1ed);
			((ISupportInitialize)errorProvider).EndInit();
			pnlAdcLowPower.ResumeLayout(false);
			pnlAdcLowPower.PerformLayout();
			panel1.ResumeLayout(false);
			panel1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		private void OnAdcLowPowerOnChanged(bool value)
		{
			if (AdcLowPowerOnChanged != null)
			{
				AdcLowPowerOnChanged(this, new BooleanEventArg(value));
			}
		}

		private void OnDocumentationChanged(DocumentationChangedEventArgs e)
		{
			if (DocumentationChanged != null)
			{
				DocumentationChanged(this, e);
			}
		}

		private void OnTempCalibrateChanged(decimal value)
		{
			if (TempCalibrateChanged != null)
			{
				TempCalibrateChanged(this, new DecimalEventArg(value));
			}
		}

		private void rBtnAdcLowPower_CheckedChanged(object sender, EventArgs e)
		{
			AdcLowPowerOn = rBtnAdcLowPowerOn.Checked;
			OnAdcLowPowerOnChanged(AdcLowPowerOn);
		}

		public bool AdcLowPowerOn
		{
			get
			{
				return rBtnAdcLowPowerOn.Checked;
			}
			set
			{
				rBtnAdcLowPowerOn.CheckedChanged -= new EventHandler(rBtnAdcLowPower_CheckedChanged);
				rBtnAdcLowPowerOff.CheckedChanged -= new EventHandler(rBtnAdcLowPower_CheckedChanged);
				if (value)
				{
					rBtnAdcLowPowerOn.Checked = true;
					rBtnAdcLowPowerOff.Checked = false;
				}
				else
				{
					rBtnAdcLowPowerOn.Checked = false;
					rBtnAdcLowPowerOff.Checked = true;
				}
				rBtnAdcLowPowerOn.CheckedChanged += new EventHandler(rBtnAdcLowPower_CheckedChanged);
				rBtnAdcLowPowerOff.CheckedChanged += new EventHandler(rBtnAdcLowPower_CheckedChanged);
			}
		}

		public OperatingModeEnum Mode
		{
			get { return _mode; }
			set
			{
				_mode = value;
				switch (_mode)
				{
					case OperatingModeEnum.Sleep:
					case OperatingModeEnum.Tx:
					case OperatingModeEnum.Rx:
						panel1.Enabled = false;
						thermometerCtrl.Enabled = false;
						break;

					case OperatingModeEnum.Stdby:
					case OperatingModeEnum.Fs:
						panel1.Enabled = true;
						if (!TempCalDone)
						{
							break;
						}
						thermometerCtrl.Enabled = true;
						return;

					default:
						return;
				}
			}
		}

		public bool TempCalDone
		{
			get { return _tempCalDone; }
			set
			{
				_tempCalDone = value;
				thermometerCtrl.Enabled = value;
			}
		}

		public bool TempMeasRunning
		{
			get { return ledTempMeasRunning.Checked; }
			set { ledTempMeasRunning.Checked = value; }
		}

		public decimal TempValue
		{
			get { return (decimal)thermometerCtrl.Value; }
			set { thermometerCtrl.Value = (double)value; }
		}

		public decimal TempValueRoom
		{
			get { return tempValueRoom; }
			set { tempValueRoom = value; }
		}
	}
}
