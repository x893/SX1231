using SemtechLib.Controls;
using SemtechLib.Devices.SX1231.Enumerations;
using SemtechLib.Devices.SX1231.Events;
using SemtechLib.General.Events;
using SemtechLib.General.Interfaces;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace SemtechLib.Devices.SX1231.Controls
{
	public class TransmitterViewControl : UserControl, INotifyDocumentationChanged
	{
		private ComboBox cBoxPaRamp;
		private IContainer components;
		private ErrorProvider errorProvider;
		private GroupBoxEx gBoxOutputPower;
		private GroupBoxEx gBoxOverloadCurrentProtection;
		private GroupBoxEx gBoxPowerAmplifier;
		private Label label3;
		private Label label5;
		private NumericUpDownEx nudOcpTrim;
		private NumericUpDownEx nudOutputPower;
		private decimal ocpTrim = 100M;
		private Panel panel1;
		private Panel panel4;
		private RadioButton rBtnOcpOff;
		private RadioButton rBtnOcpOn;
		private RadioButton rBtnPaControlPa0;
		private RadioButton rBtnPaControlPa1;
		private RadioButton rBtnPaControlPa1Pa2;
		private Label suffixOCPtrim;
		private Label suffixOutputPower;
		private Label suffixPAramp;

		public event DocumentationChangedEventHandler DocumentationChanged;
		public event BooleanEventHandler OcpOnChanged;
		public event DecimalEventHandler OcpTrimChanged;
		public event DecimalEventHandler OutputPowerChanged;
		public event PaModeEventHandler PaModeChanged;
		public event PaRampEventHandler PaRampChanged;

		public TransmitterViewControl()
		{
			InitializeComponent();
		}

		private void cBoxPaRamp_SelectedIndexChanged(object sender, EventArgs e)
		{
			PaRamp = (PaRampEnum)cBoxPaRamp.SelectedIndex;
			OnPaRampChanged(PaRamp);
		}

		private void control_MouseEnter(object sender, EventArgs e)
		{
			if (sender == gBoxPowerAmplifier)
				OnDocumentationChanged(new DocumentationChangedEventArgs("Transmitter", "Power amplifier"));
			else if (sender == gBoxOutputPower)
				OnDocumentationChanged(new DocumentationChangedEventArgs("Transmitter", "Output power"));
			else if (sender == gBoxOverloadCurrentProtection)
				OnDocumentationChanged(new DocumentationChangedEventArgs("Transmitter", "Overload current protection"));
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
			gBoxOverloadCurrentProtection = new GroupBoxEx();
			panel4 = new Panel();
			rBtnOcpOff = new RadioButton();
			rBtnOcpOn = new RadioButton();
			label5 = new Label();
			nudOcpTrim = new NumericUpDownEx();
			suffixOCPtrim = new Label();
			gBoxOutputPower = new GroupBoxEx();
			nudOutputPower = new NumericUpDownEx();
			suffixOutputPower = new Label();
			gBoxPowerAmplifier = new GroupBoxEx();
			cBoxPaRamp = new ComboBox();
			panel1 = new Panel();
			rBtnPaControlPa1Pa2 = new RadioButton();
			rBtnPaControlPa1 = new RadioButton();
			rBtnPaControlPa0 = new RadioButton();
			suffixPAramp = new Label();
			label3 = new Label();
			((ISupportInitialize)errorProvider).BeginInit();
			gBoxOverloadCurrentProtection.SuspendLayout();
			panel4.SuspendLayout();
			nudOcpTrim.BeginInit();
			gBoxOutputPower.SuspendLayout();
			nudOutputPower.BeginInit();
			gBoxPowerAmplifier.SuspendLayout();
			panel1.SuspendLayout();
			base.SuspendLayout();
			errorProvider.ContainerControl = this;
			gBoxOverloadCurrentProtection.Controls.Add(panel4);
			gBoxOverloadCurrentProtection.Controls.Add(label5);
			gBoxOverloadCurrentProtection.Controls.Add(nudOcpTrim);
			gBoxOverloadCurrentProtection.Controls.Add(suffixOCPtrim);
			gBoxOverloadCurrentProtection.Location = new Point(0xd9, 0x12b);
			gBoxOverloadCurrentProtection.Name = "gBoxOverloadCurrentProtection";
			gBoxOverloadCurrentProtection.Size = new Size(0x16c, 0x45);
			gBoxOverloadCurrentProtection.TabIndex = 2;
			gBoxOverloadCurrentProtection.TabStop = false;
			gBoxOverloadCurrentProtection.Text = "Overload current protection";
			gBoxOverloadCurrentProtection.MouseEnter += new EventHandler(control_MouseEnter);
			gBoxOverloadCurrentProtection.MouseLeave += new EventHandler(control_MouseLeave);
			panel4.AutoSize = true;
			panel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			panel4.Controls.Add(rBtnOcpOff);
			panel4.Controls.Add(rBtnOcpOn);
			panel4.Location = new Point(0xc0, 0x13);
			panel4.Name = "panel4";
			panel4.Size = new Size(0x66, 20);
			panel4.TabIndex = 0;
			rBtnOcpOff.AutoSize = true;
			rBtnOcpOff.Location = new Point(0x36, 3);
			rBtnOcpOff.Margin = new Padding(3, 0, 3, 0);
			rBtnOcpOff.Name = "rBtnOcpOff";
			rBtnOcpOff.Size = new Size(0x2d, 0x11);
			rBtnOcpOff.TabIndex = 1;
			rBtnOcpOff.Text = "OFF";
			rBtnOcpOff.UseVisualStyleBackColor = true;
			rBtnOcpOff.CheckedChanged += new EventHandler(rBtnOcpOn_CheckedChanged);
			rBtnOcpOn.AutoSize = true;
			rBtnOcpOn.Checked = true;
			rBtnOcpOn.Location = new Point(3, 3);
			rBtnOcpOn.Margin = new Padding(3, 0, 3, 0);
			rBtnOcpOn.Name = "rBtnOcpOn";
			rBtnOcpOn.Size = new Size(0x29, 0x11);
			rBtnOcpOn.TabIndex = 0;
			rBtnOcpOn.TabStop = true;
			rBtnOcpOn.Text = "ON";
			rBtnOcpOn.UseVisualStyleBackColor = true;
			rBtnOcpOn.CheckedChanged += new EventHandler(rBtnOcpOn_CheckedChanged);
			label5.AutoSize = true;
			label5.Location = new Point(6, 0x31);
			label5.Name = "label5";
			label5.Size = new Size(0x34, 13);
			label5.TabIndex = 1;
			label5.Text = "Trimming:";
			nudOcpTrim.Location = new Point(0xc0, 0x2d);
			int[] bits = new int[4];
			bits[0] = 120;
			nudOcpTrim.Maximum = new decimal(bits);
			int[] numArray2 = new int[4];
			numArray2[0] = 0x2d;
			nudOcpTrim.Minimum = new decimal(numArray2);
			nudOcpTrim.Name = "nudOcpTrim";
			nudOcpTrim.Size = new Size(0x7c, 20);
			nudOcpTrim.TabIndex = 2;
			nudOcpTrim.ThousandsSeparator = true;
			int[] numArray3 = new int[4];
			numArray3[0] = 100;
			nudOcpTrim.Value = new decimal(numArray3);
			nudOcpTrim.ValueChanged += new EventHandler(nudOcpTrim_ValueChanged);
			suffixOCPtrim.AutoSize = true;
			suffixOCPtrim.Location = new Point(0x142, 0x31);
			suffixOCPtrim.Name = "suffixOCPtrim";
			suffixOCPtrim.Size = new Size(0x16, 13);
			suffixOCPtrim.TabIndex = 3;
			suffixOCPtrim.Text = "mA";
			gBoxOutputPower.Controls.Add(nudOutputPower);
			gBoxOutputPower.Controls.Add(suffixOutputPower);
			gBoxOutputPower.Location = new Point(0xd9, 0xf9);
			gBoxOutputPower.Name = "gBoxOutputPower";
			gBoxOutputPower.Size = new Size(0x16c, 0x2c);
			gBoxOutputPower.TabIndex = 1;
			gBoxOutputPower.TabStop = false;
			gBoxOutputPower.Text = "Output power";
			gBoxOutputPower.MouseEnter += new EventHandler(control_MouseEnter);
			gBoxOutputPower.MouseLeave += new EventHandler(control_MouseLeave);
			nudOutputPower.Location = new Point(0xc0, 0x13);
			int[] numArray4 = new int[4];
			numArray4[0] = 13;
			nudOutputPower.Maximum = new decimal(numArray4);
			int[] numArray5 = new int[4];
			numArray5[0] = 0x12;
			numArray5[3] = -2147483648;
			nudOutputPower.Minimum = new decimal(numArray5);
			nudOutputPower.Name = "nudOutputPower";
			nudOutputPower.Size = new Size(0x7c, 20);
			nudOutputPower.TabIndex = 0;
			nudOutputPower.ThousandsSeparator = true;
			int[] numArray6 = new int[4];
			numArray6[0] = 13;
			nudOutputPower.Value = new decimal(numArray6);
			nudOutputPower.ValueChanged += new EventHandler(nudOutputPower_ValueChanged);
			suffixOutputPower.AutoSize = true;
			suffixOutputPower.Location = new Point(0x142, 0x17);
			suffixOutputPower.Name = "suffixOutputPower";
			suffixOutputPower.Size = new Size(0x1c, 13);
			suffixOutputPower.TabIndex = 1;
			suffixOutputPower.Text = "dBm";
			gBoxPowerAmplifier.Controls.Add(cBoxPaRamp);
			gBoxPowerAmplifier.Controls.Add(panel1);
			gBoxPowerAmplifier.Controls.Add(suffixPAramp);
			gBoxPowerAmplifier.Controls.Add(label3);
			gBoxPowerAmplifier.Location = new Point(0xd9, 0x7c);
			gBoxPowerAmplifier.Name = "gBoxPowerAmplifier";
			gBoxPowerAmplifier.Size = new Size(0x16c, 0x77);
			gBoxPowerAmplifier.TabIndex = 0;
			gBoxPowerAmplifier.TabStop = false;
			gBoxPowerAmplifier.Text = "Power Amplifier";
			gBoxPowerAmplifier.MouseEnter += new EventHandler(control_MouseEnter);
			gBoxPowerAmplifier.MouseLeave += new EventHandler(control_MouseLeave);
			cBoxPaRamp.Items.AddRange(new object[] { "3400", "2000", "1000", "500", "250", "125", "100", "62", "50", "40", "31", "25", "20", "15", "12", "10" });
			cBoxPaRamp.Location = new Point(0xc0, 0x5e);
			cBoxPaRamp.Name = "cBoxPaRamp";
			cBoxPaRamp.Size = new Size(0x7c, 0x15);
			cBoxPaRamp.TabIndex = 2;
			cBoxPaRamp.SelectedIndexChanged += new EventHandler(cBoxPaRamp_SelectedIndexChanged);
			panel1.AutoSize = true;
			panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			panel1.Controls.Add(rBtnPaControlPa1Pa2);
			panel1.Controls.Add(rBtnPaControlPa1);
			panel1.Controls.Add(rBtnPaControlPa0);
			panel1.Location = new Point(0x41, 0x13);
			panel1.Name = "panel1";
			panel1.Size = new Size(0xeb, 0x45);
			panel1.TabIndex = 0;
			rBtnPaControlPa1Pa2.AutoSize = true;
			rBtnPaControlPa1Pa2.Location = new Point(3, 0x31);
			rBtnPaControlPa1Pa2.Name = "rBtnPaControlPa1Pa2";
			rBtnPaControlPa1Pa2.Size = new Size(0xe5, 0x11);
			rBtnPaControlPa1Pa2.TabIndex = 2;
			rBtnPaControlPa1Pa2.Text = "PA1 + PA2 -> Transmits on pin PA_BOOST";
			rBtnPaControlPa1Pa2.UseVisualStyleBackColor = true;
			rBtnPaControlPa1Pa2.CheckedChanged += new EventHandler(rBtnPaControl_CheckedChanged);
			rBtnPaControlPa1.AutoSize = true;
			rBtnPaControlPa1.Location = new Point(3, 0x1a);
			rBtnPaControlPa1.Name = "rBtnPaControlPa1";
			rBtnPaControlPa1.Size = new Size(0xc5, 0x11);
			rBtnPaControlPa1.TabIndex = 1;
			rBtnPaControlPa1.Text = "PA1 -> Transmits on pin PA_BOOST";
			rBtnPaControlPa1.UseVisualStyleBackColor = true;
			rBtnPaControlPa1.CheckedChanged += new EventHandler(rBtnPaControl_CheckedChanged);
			rBtnPaControlPa0.AutoSize = true;
			rBtnPaControlPa0.Checked = true;
			rBtnPaControlPa0.Location = new Point(3, 3);
			rBtnPaControlPa0.Name = "rBtnPaControlPa0";
			rBtnPaControlPa0.Size = new Size(0xa5, 0x11);
			rBtnPaControlPa0.TabIndex = 0;
			rBtnPaControlPa0.TabStop = true;
			rBtnPaControlPa0.Text = "PA0 -> Transmits on pin RFIO";
			rBtnPaControlPa0.UseVisualStyleBackColor = true;
			rBtnPaControlPa0.CheckedChanged += new EventHandler(rBtnPaControl_CheckedChanged);
			suffixPAramp.AutoSize = true;
			suffixPAramp.Location = new Point(0x142, 0x62);
			suffixPAramp.Name = "suffixPAramp";
			suffixPAramp.Size = new Size(0x12, 13);
			suffixPAramp.TabIndex = 3;
			suffixPAramp.Text = "\x00b5s";
			label3.AutoSize = true;
			label3.Location = new Point(6, 0x62);
			label3.Name = "label3";
			label3.Size = new Size(50, 13);
			label3.TabIndex = 1;
			label3.Text = "PA ramp:";
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(gBoxOverloadCurrentProtection);
			base.Controls.Add(gBoxOutputPower);
			base.Controls.Add(gBoxPowerAmplifier);
			base.Name = "TransmitterViewControl";
			base.Size = new Size(0x31f, 0x1ed);
			((ISupportInitialize)errorProvider).EndInit();
			gBoxOverloadCurrentProtection.ResumeLayout(false);
			gBoxOverloadCurrentProtection.PerformLayout();
			panel4.ResumeLayout(false);
			panel4.PerformLayout();
			nudOcpTrim.EndInit();
			gBoxOutputPower.ResumeLayout(false);
			gBoxOutputPower.PerformLayout();
			nudOutputPower.EndInit();
			gBoxPowerAmplifier.ResumeLayout(false);
			gBoxPowerAmplifier.PerformLayout();
			panel1.ResumeLayout(false);
			panel1.PerformLayout();
			base.ResumeLayout(false);
		}

		private void nudOcpTrim_ValueChanged(object sender, EventArgs e)
		{
			int num = (int)Math.Round((decimal)((OcpTrim - 45.0M) / 5.0M), MidpointRounding.AwayFromZero);
			int num2 = (int)Math.Round((decimal)((nudOcpTrim.Value - 45.0M) / 5.0M), MidpointRounding.AwayFromZero);
			int num3 = (int)(nudOcpTrim.Value - OcpTrim);
			if (((num3 >= -1) && (num3 <= 1)) && (num == num2))
			{
				nudOcpTrim.ValueChanged -= new EventHandler(nudOcpTrim_ValueChanged);
				nudOcpTrim.Value = 45.0M + (5.0M * (num2 + num3));
				nudOcpTrim.ValueChanged += new EventHandler(nudOcpTrim_ValueChanged);
			}
			OcpTrim = nudOcpTrim.Value;
			OnOcpTrimChanged(OcpTrim);
		}

		private void nudOutputPower_ValueChanged(object sender, EventArgs e)
		{
			OutputPower = nudOutputPower.Value;
			OnOutputPowerChanged(OutputPower);
		}

		private void OnDocumentationChanged(DocumentationChangedEventArgs e)
		{
			if (DocumentationChanged != null)
				DocumentationChanged(this, e);
		}

		private void OnOcpOnChanged(bool value)
		{
			if (OcpOnChanged != null)
				OcpOnChanged(this, new BooleanEventArg(value));
		}

		private void OnOcpTrimChanged(decimal value)
		{
			if (OcpTrimChanged != null)
				OcpTrimChanged(this, new DecimalEventArg(value));
		}

		private void OnOutputPowerChanged(decimal value)
		{
			if (OutputPowerChanged != null)
				OutputPowerChanged(this, new DecimalEventArg(value));
		}

		private void OnPaModeChanged(PaModeEnum value)
		{
			if (PaModeChanged != null)
			{
				PaModeChanged(this, new PaModeEventArg(value));
			}
		}

		private void OnPaRampChanged(PaRampEnum value)
		{
			if (PaRampChanged != null)
				PaRampChanged(this, new PaRampEventArg(value));
		}

		private void rBtnOcpOn_CheckedChanged(object sender, EventArgs e)
		{
			OcpOn = rBtnOcpOn.Checked;
			OnOcpOnChanged(OcpOn);
		}

		private void rBtnPaControl_CheckedChanged(object sender, EventArgs e)
		{
			if (rBtnPaControlPa0.Checked)
				PaMode = PaModeEnum.PA0;
			else if (rBtnPaControlPa1.Checked)
				PaMode = PaModeEnum.PA1;
			else if (rBtnPaControlPa1Pa2.Checked)
				PaMode = PaModeEnum.PA1_PA2;
			OnPaModeChanged(PaMode);
		}

		public bool OcpOn
		{
			get { return rBtnOcpOn.Checked; }
			set
			{
				rBtnOcpOn.CheckedChanged -= new EventHandler(rBtnOcpOn_CheckedChanged);
				rBtnOcpOff.CheckedChanged -= new EventHandler(rBtnOcpOn_CheckedChanged);
				if (value)
				{
					rBtnOcpOn.Checked = true;
					rBtnOcpOff.Checked = false;
				}
				else
				{
					rBtnOcpOn.Checked = false;
					rBtnOcpOff.Checked = true;
				}
				rBtnOcpOn.CheckedChanged += new EventHandler(rBtnOcpOn_CheckedChanged);
				rBtnOcpOff.CheckedChanged += new EventHandler(rBtnOcpOn_CheckedChanged);
			}
		}

		public decimal OcpTrim
		{
			get { return ocpTrim; }
			set
			{
				try
				{
					nudOcpTrim.ValueChanged -= new EventHandler(nudOcpTrim_ValueChanged);
					ushort num = (ushort)Math.Round((decimal)((value - 45.0M) / 5.0M), MidpointRounding.AwayFromZero);
					ocpTrim = 45.0M + (5.0M * num);
					nudOcpTrim.Value = ocpTrim;
					nudOcpTrim.ValueChanged += new EventHandler(nudOcpTrim_ValueChanged);
				}
				catch (Exception)
				{
					nudOcpTrim.BackColor = ControlPaint.LightLight(Color.Red);
					nudOcpTrim.ValueChanged += new EventHandler(nudOcpTrim_ValueChanged);
				}
			}
		}

		public decimal OutputPower
		{
			get { return nudOutputPower.Value; }
			set
			{
				try
				{
					nudOutputPower.ValueChanged -= new EventHandler(nudOutputPower_ValueChanged);
					if (PaMode == PaModeEnum.PA1_PA2)
					{
						nudOutputPower.Maximum = 17M;
						nudOutputPower.Minimum = -14M;
					}
					else
					{
						nudOutputPower.Maximum = 13M;
						nudOutputPower.Minimum = -18M;
					}
					nudOutputPower.Value = value;
					nudOutputPower.ValueChanged += new EventHandler(nudOutputPower_ValueChanged);
				}
				catch (Exception)
				{
					nudOutputPower.BackColor = ControlPaint.LightLight(Color.Red);
					nudOutputPower.ValueChanged += new EventHandler(nudOutputPower_ValueChanged);
				}
			}
		}

		public PaModeEnum PaMode
		{
			get
			{
				if (!rBtnPaControlPa0.Checked)
				{
					if (rBtnPaControlPa1.Checked)
						return PaModeEnum.PA1;
					if (rBtnPaControlPa1Pa2.Checked)
						return PaModeEnum.PA1_PA2;
				}
				return PaModeEnum.PA0;
			}
			set
			{
				rBtnPaControlPa0.CheckedChanged -= new EventHandler(rBtnPaControl_CheckedChanged);
				rBtnPaControlPa1.CheckedChanged -= new EventHandler(rBtnPaControl_CheckedChanged);
				rBtnPaControlPa1Pa2.CheckedChanged -= new EventHandler(rBtnPaControl_CheckedChanged);
				switch (value)
				{
					case PaModeEnum.PA0:
						rBtnPaControlPa0.Checked = true;
						rBtnPaControlPa1.Checked = false;
						rBtnPaControlPa1Pa2.Checked = false;
						break;

					case PaModeEnum.PA1:
						rBtnPaControlPa0.Checked = false;
						rBtnPaControlPa1.Checked = true;
						rBtnPaControlPa1Pa2.Checked = false;
						break;

					case PaModeEnum.PA1_PA2:
						rBtnPaControlPa0.Checked = false;
						rBtnPaControlPa1.Checked = false;
						rBtnPaControlPa1Pa2.Checked = true;
						break;
				}
				rBtnPaControlPa0.CheckedChanged += new EventHandler(rBtnPaControl_CheckedChanged);
				rBtnPaControlPa1.CheckedChanged += new EventHandler(rBtnPaControl_CheckedChanged);
				rBtnPaControlPa1Pa2.CheckedChanged += new EventHandler(rBtnPaControl_CheckedChanged);
			}
		}

		public PaRampEnum PaRamp
		{
			get { return (PaRampEnum)cBoxPaRamp.SelectedIndex; }
			set
			{
				cBoxPaRamp.SelectedIndexChanged -= new EventHandler(cBoxPaRamp_SelectedIndexChanged);
				cBoxPaRamp.SelectedIndex = (int)value;
				cBoxPaRamp.SelectedIndexChanged += new EventHandler(cBoxPaRamp_SelectedIndexChanged);
			}
		}
	}
}
