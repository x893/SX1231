namespace SemtechLib.Devices.SX1231.Controls
{
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

	public class CommonViewControl : UserControl, INotifyDocumentationChanged
	{
		private decimal bitRate = 4800M;
		private Button btnListenModeAbort;
		private Button btnRcCalibration;
		private ComboBox cBoxListenEnd;
		private ComboBox cBoxListenResolIdle;
		private ComboBox cBoxListenResolRx;
		private ComboBox cBoxLowBatTrim;
		private IContainer components;
		private ErrorProvider errorProvider;
		private GroupBoxEx gBoxBatteryManagement;
		private GroupBoxEx gBoxBitSyncDataMode;
		private GroupBoxEx gBoxGeneral;
		private GroupBoxEx gBoxListenMode;
		private GroupBoxEx gBoxModulation;
		private GroupBoxEx gBoxOscillators;
		private Label label1;
		private Label label10;
		private Label label11;
		private Label label12;
		private Label label13;
		private Label label14;
		private Label label15;
		private Label label16;
		private Label label17;
		private Label label18;
		private Label label19;
		private Label label20;
		private Label label21;
		private Label label22;
		private Label label23;
		private Label label24;
		private Label label25;
		private Label label26;
		private Label label27;
		private Label label28;
		private Label label30;
		private Label label5;
		private Label label6;
		private Label label7;
		private Label label8;
		private Label label9;
		private Label lblListenResolRx;
		private Label lblRcOscillatorCal;
		private Label lblRcOscillatorCalStat;
		private Led ledLowBatMonitor;
		private Led ledRcCalibration;
		private NumericUpDownEx nudBitRate;
		private NumericUpDownEx nudFdev;
		private NumericUpDownEx nudFrequencyRf;
		private NumericUpDownEx nudFrequencyXo;
		private NumericUpDownEx nudListenCoefIdle;
		private NumericUpDownEx nudListenCoefRx;
		private Panel panel1;
		private Panel panel2;
		private Panel panel3;
		private Panel panel4;
		private Panel panel5;
		private Panel panel6;
		private Panel panel7;
		private RadioButton rBtnContinousBitSyncOff;
		private RadioButton rBtnContinousBitSyncOn;
		private RadioButton rBtnListenCriteria0;
		private RadioButton rBtnListenCriteria1;
		private RadioButton rBtnListenModeOff;
		private RadioButton rBtnListenModeOn;
		private RadioButton rBtnLowBatOff;
		private RadioButton rBtnLowBatOn;
		private RadioButton rBtnModulationShaping01;
		private RadioButton rBtnModulationShaping10;
		private RadioButton rBtnModulationShaping11;
		private RadioButton rBtnModulationShapingOff;
		private RadioButton rBtnModulationTypeFsk;
		private RadioButton rBtnModulationTypeOok;
		private RadioButton rBtnPacketHandler;
		private RadioButton rBtnSequencerOff;
		private RadioButton rBtnSequencerOn;
		private string version = "2.3";

		public event DecimalEventHandler BitRateChanged;

		public event DataModeEventHandler DataModeChanged;

		public event DocumentationChangedEventHandler DocumentationChanged;

		public event DecimalEventHandler FdevChanged;

		public event DecimalEventHandler FrequencyRfChanged;

		public event DecimalEventHandler FrequencyXoChanged;

		public event DecimalEventHandler ListenCoefIdleChanged;

		public event DecimalEventHandler ListenCoefRxChanged;

		public event ListenCriteriaEventHandler ListenCriteriaChanged;

		public event ListenEndEventHandler ListenEndChanged;

		public event EventHandler ListenModeAbortChanged;

		public event BooleanEventHandler ListenModeChanged;

		public event ListenResolEventHandler ListenResolIdleChanged;

		public event ListenResolEventHandler ListenResolRxChanged;

		public event BooleanEventHandler LowBatOnChanged;

		public event LowBatTrimEventHandler LowBatTrimChanged;

		public event ByteEventHandler ModulationShapingChanged;

		public event ModulationTypeEventHandler ModulationTypeChanged;

		public event EventHandler RcCalibrationChanged;

		public event BooleanEventHandler SequencerChanged;

		public CommonViewControl()
		{
			InitializeComponent();
		}

		private void btnListenModeAbort_Click(object sender, EventArgs e)
		{
			OnListenModeAbortChanged();
		}

		private void btnRcCalibration_Click(object sender, EventArgs e)
		{
			OnRcCalibrationChanged();
		}

		private void cBoxListenEnd_SelectedIndexChanged(object sender, EventArgs e)
		{
			ListenEnd = (ListenEndEnum)cBoxListenEnd.SelectedIndex;
			OnListenEndChanged(ListenEnd);
		}

		private void cBoxListenResolIdle_SelectedIndexChanged(object sender, EventArgs e)
		{
			ListenResolIdle = (ListenResolEnum)cBoxListenResolIdle.SelectedIndex;
			OnListenResolIdleChanged(ListenResolIdle);
		}

		private void cBoxListenResolRx_SelectedIndexChanged(object sender, EventArgs e)
		{
			ListenResolRx = (ListenResolEnum)cBoxListenResolRx.SelectedIndex;
			OnListenResolRxChanged(ListenResolRx);
		}

		private void cBoxLowBatTrim_SelectedIndexChanged(object sender, EventArgs e)
		{
			LowBatTrim = (LowBatTrimEnum)cBoxLowBatTrim.SelectedIndex;
			OnLowBatTrimChanged(LowBatTrim);
		}

		private void control_MouseEnter(object sender, EventArgs e)
		{
			if (sender == gBoxGeneral)
			{
				OnDocumentationChanged(new DocumentationChangedEventArgs("Common", "General"));
			}
			else if (sender == gBoxBitSyncDataMode)
			{
				OnDocumentationChanged(new DocumentationChangedEventArgs("Common", "Bit synchronizer Data mode"));
			}
			else if (sender == gBoxModulation)
			{
				OnDocumentationChanged(new DocumentationChangedEventArgs("Common", "Modulation"));
			}
			else if (sender == gBoxOscillators)
			{
				OnDocumentationChanged(new DocumentationChangedEventArgs("Common", "Oscillators"));
			}
			else if (sender == gBoxListenMode)
			{
				OnDocumentationChanged(new DocumentationChangedEventArgs("Common", "Listen mode"));
			}
			else if (sender == gBoxBatteryManagement)
			{
				OnDocumentationChanged(new DocumentationChangedEventArgs("Common", "Battery management"));
			}
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
			this.components = new System.ComponentModel.Container();
			this.btnRcCalibration = new System.Windows.Forms.Button();
			this.cBoxLowBatTrim = new System.Windows.Forms.ComboBox();
			this.panel4 = new System.Windows.Forms.Panel();
			this.rBtnLowBatOff = new System.Windows.Forms.RadioButton();
			this.rBtnLowBatOn = new System.Windows.Forms.RadioButton();
			this.label1 = new System.Windows.Forms.Label();
			this.panel2 = new System.Windows.Forms.Panel();
			this.rBtnModulationTypeOok = new System.Windows.Forms.RadioButton();
			this.rBtnModulationTypeFsk = new System.Windows.Forms.RadioButton();
			this.panel3 = new System.Windows.Forms.Panel();
			this.rBtnModulationShaping11 = new System.Windows.Forms.RadioButton();
			this.rBtnModulationShaping10 = new System.Windows.Forms.RadioButton();
			this.rBtnModulationShaping01 = new System.Windows.Forms.RadioButton();
			this.rBtnModulationShapingOff = new System.Windows.Forms.RadioButton();
			this.label5 = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.rBtnContinousBitSyncOff = new System.Windows.Forms.RadioButton();
			this.rBtnContinousBitSyncOn = new System.Windows.Forms.RadioButton();
			this.rBtnPacketHandler = new System.Windows.Forms.RadioButton();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.label16 = new System.Windows.Forms.Label();
			this.lblRcOscillatorCalStat = new System.Windows.Forms.Label();
			this.lblRcOscillatorCal = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.label17 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.label18 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.rBtnSequencerOff = new System.Windows.Forms.RadioButton();
			this.rBtnSequencerOn = new System.Windows.Forms.RadioButton();
			this.label19 = new System.Windows.Forms.Label();
			this.panel5 = new System.Windows.Forms.Panel();
			this.label20 = new System.Windows.Forms.Label();
			this.panel6 = new System.Windows.Forms.Panel();
			this.rBtnListenModeOff = new System.Windows.Forms.RadioButton();
			this.rBtnListenModeOn = new System.Windows.Forms.RadioButton();
			this.btnListenModeAbort = new System.Windows.Forms.Button();
			this.label21 = new System.Windows.Forms.Label();
			this.cBoxListenResolIdle = new System.Windows.Forms.ComboBox();
			this.label22 = new System.Windows.Forms.Label();
			this.panel7 = new System.Windows.Forms.Panel();
			this.rBtnListenCriteria1 = new System.Windows.Forms.RadioButton();
			this.rBtnListenCriteria0 = new System.Windows.Forms.RadioButton();
			this.label23 = new System.Windows.Forms.Label();
			this.label24 = new System.Windows.Forms.Label();
			this.cBoxListenEnd = new System.Windows.Forms.ComboBox();
			this.label25 = new System.Windows.Forms.Label();
			this.label26 = new System.Windows.Forms.Label();
			this.label27 = new System.Windows.Forms.Label();
			this.label28 = new System.Windows.Forms.Label();
			this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
			this.nudBitRate = new SemtechLib.Controls.NumericUpDownEx();
			this.nudFdev = new SemtechLib.Controls.NumericUpDownEx();
			this.nudFrequencyRf = new SemtechLib.Controls.NumericUpDownEx();
			this.lblListenResolRx = new System.Windows.Forms.Label();
			this.label30 = new System.Windows.Forms.Label();
			this.cBoxListenResolRx = new System.Windows.Forms.ComboBox();
			this.gBoxGeneral = new SemtechLib.Controls.GroupBoxEx();
			this.gBoxBitSyncDataMode = new SemtechLib.Controls.GroupBoxEx();
			this.gBoxModulation = new SemtechLib.Controls.GroupBoxEx();
			this.gBoxOscillators = new SemtechLib.Controls.GroupBoxEx();
			this.nudFrequencyXo = new SemtechLib.Controls.NumericUpDownEx();
			this.ledRcCalibration = new SemtechLib.Controls.Led();
			this.gBoxBatteryManagement = new SemtechLib.Controls.GroupBoxEx();
			this.ledLowBatMonitor = new SemtechLib.Controls.Led();
			this.gBoxListenMode = new SemtechLib.Controls.GroupBoxEx();
			this.nudListenCoefRx = new SemtechLib.Controls.NumericUpDownEx();
			this.nudListenCoefIdle = new SemtechLib.Controls.NumericUpDownEx();
			this.panel4.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel3.SuspendLayout();
			this.panel1.SuspendLayout();
			this.panel5.SuspendLayout();
			this.panel6.SuspendLayout();
			this.panel7.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudBitRate)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudFdev)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudFrequencyRf)).BeginInit();
			this.gBoxGeneral.SuspendLayout();
			this.gBoxBitSyncDataMode.SuspendLayout();
			this.gBoxModulation.SuspendLayout();
			this.gBoxOscillators.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudFrequencyXo)).BeginInit();
			this.gBoxBatteryManagement.SuspendLayout();
			this.gBoxListenMode.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudListenCoefRx)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudListenCoefIdle)).BeginInit();
			this.SuspendLayout();
			// 
			// btnRcCalibration
			// 
			this.btnRcCalibration.Location = new System.Drawing.Point(164, 51);
			this.btnRcCalibration.Name = "btnRcCalibration";
			this.btnRcCalibration.Size = new System.Drawing.Size(75, 23);
			this.btnRcCalibration.TabIndex = 4;
			this.btnRcCalibration.Text = "Calibrate";
			this.btnRcCalibration.UseVisualStyleBackColor = true;
			this.btnRcCalibration.Click += new System.EventHandler(this.btnRcCalibration_Click);
			// 
			// cBoxLowBatTrim
			// 
			this.cBoxLowBatTrim.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cBoxLowBatTrim.FormattingEnabled = true;
			this.cBoxLowBatTrim.Items.AddRange(new object[] {
            "1.695",
            "1.764",
            "1.835",
            "1.905",
            "1.976",
            "2.045",
            "2.116",
            "2.185"});
			this.cBoxLowBatTrim.Location = new System.Drawing.Point(166, 45);
			this.cBoxLowBatTrim.Name = "cBoxLowBatTrim";
			this.cBoxLowBatTrim.Size = new System.Drawing.Size(124, 21);
			this.cBoxLowBatTrim.TabIndex = 3;
			this.cBoxLowBatTrim.SelectedIndexChanged += new System.EventHandler(this.cBoxLowBatTrim_SelectedIndexChanged);
			// 
			// panel4
			// 
			this.panel4.AutoSize = true;
			this.panel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.panel4.Controls.Add(this.rBtnLowBatOff);
			this.panel4.Controls.Add(this.rBtnLowBatOn);
			this.panel4.Location = new System.Drawing.Point(166, 19);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(102, 20);
			this.panel4.TabIndex = 1;
			// 
			// rBtnLowBatOff
			// 
			this.rBtnLowBatOff.AutoSize = true;
			this.rBtnLowBatOff.Location = new System.Drawing.Point(54, 3);
			this.rBtnLowBatOff.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
			this.rBtnLowBatOff.Name = "rBtnLowBatOff";
			this.rBtnLowBatOff.Size = new System.Drawing.Size(45, 17);
			this.rBtnLowBatOff.TabIndex = 1;
			this.rBtnLowBatOff.Text = "OFF";
			this.rBtnLowBatOff.UseVisualStyleBackColor = true;
			// 
			// rBtnLowBatOn
			// 
			this.rBtnLowBatOn.AutoSize = true;
			this.rBtnLowBatOn.Checked = true;
			this.rBtnLowBatOn.Location = new System.Drawing.Point(3, 3);
			this.rBtnLowBatOn.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
			this.rBtnLowBatOn.Name = "rBtnLowBatOn";
			this.rBtnLowBatOn.Size = new System.Drawing.Size(41, 17);
			this.rBtnLowBatOn.TabIndex = 0;
			this.rBtnLowBatOn.TabStop = true;
			this.rBtnLowBatOn.Text = "ON";
			this.rBtnLowBatOn.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 23);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(78, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "XO Frequency:";
			// 
			// panel2
			// 
			this.panel2.AutoSize = true;
			this.panel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.panel2.Controls.Add(this.rBtnModulationTypeOok);
			this.panel2.Controls.Add(this.rBtnModulationTypeFsk);
			this.panel2.Location = new System.Drawing.Point(164, 19);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(105, 23);
			this.panel2.TabIndex = 1;
			// 
			// rBtnModulationTypeOok
			// 
			this.rBtnModulationTypeOok.AutoSize = true;
			this.rBtnModulationTypeOok.Location = new System.Drawing.Point(54, 3);
			this.rBtnModulationTypeOok.Name = "rBtnModulationTypeOok";
			this.rBtnModulationTypeOok.Size = new System.Drawing.Size(48, 17);
			this.rBtnModulationTypeOok.TabIndex = 1;
			this.rBtnModulationTypeOok.Text = "OOK";
			this.rBtnModulationTypeOok.UseVisualStyleBackColor = true;
			this.rBtnModulationTypeOok.CheckedChanged += new System.EventHandler(this.rBtnModulationType_CheckedChanged);
			// 
			// rBtnModulationTypeFsk
			// 
			this.rBtnModulationTypeFsk.AutoSize = true;
			this.rBtnModulationTypeFsk.Checked = true;
			this.rBtnModulationTypeFsk.Location = new System.Drawing.Point(3, 3);
			this.rBtnModulationTypeFsk.Name = "rBtnModulationTypeFsk";
			this.rBtnModulationTypeFsk.Size = new System.Drawing.Size(45, 17);
			this.rBtnModulationTypeFsk.TabIndex = 0;
			this.rBtnModulationTypeFsk.TabStop = true;
			this.rBtnModulationTypeFsk.Text = "FSK";
			this.rBtnModulationTypeFsk.UseVisualStyleBackColor = true;
			this.rBtnModulationTypeFsk.CheckedChanged += new System.EventHandler(this.rBtnModulationType_CheckedChanged);
			// 
			// panel3
			// 
			this.panel3.AutoSize = true;
			this.panel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.panel3.Controls.Add(this.rBtnModulationShaping11);
			this.panel3.Controls.Add(this.rBtnModulationShaping10);
			this.panel3.Controls.Add(this.rBtnModulationShaping01);
			this.panel3.Controls.Add(this.rBtnModulationShapingOff);
			this.panel3.Location = new System.Drawing.Point(164, 48);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(144, 92);
			this.panel3.TabIndex = 3;
			// 
			// rBtnModulationShaping11
			// 
			this.rBtnModulationShaping11.AutoSize = true;
			this.rBtnModulationShaping11.Location = new System.Drawing.Point(3, 72);
			this.rBtnModulationShaping11.Name = "rBtnModulationShaping11";
			this.rBtnModulationShaping11.Size = new System.Drawing.Size(138, 17);
			this.rBtnModulationShaping11.TabIndex = 3;
			this.rBtnModulationShaping11.Text = "Gaussian filter, BT = 0.3";
			this.rBtnModulationShaping11.UseVisualStyleBackColor = true;
			this.rBtnModulationShaping11.CheckedChanged += new System.EventHandler(this.rBtnModulationShaping_CheckedChanged);
			// 
			// rBtnModulationShaping10
			// 
			this.rBtnModulationShaping10.AutoSize = true;
			this.rBtnModulationShaping10.Location = new System.Drawing.Point(3, 49);
			this.rBtnModulationShaping10.Name = "rBtnModulationShaping10";
			this.rBtnModulationShaping10.Size = new System.Drawing.Size(138, 17);
			this.rBtnModulationShaping10.TabIndex = 2;
			this.rBtnModulationShaping10.Text = "Gaussian filter, BT = 0.5";
			this.rBtnModulationShaping10.UseVisualStyleBackColor = true;
			this.rBtnModulationShaping10.CheckedChanged += new System.EventHandler(this.rBtnModulationShaping_CheckedChanged);
			// 
			// rBtnModulationShaping01
			// 
			this.rBtnModulationShaping01.AutoSize = true;
			this.rBtnModulationShaping01.Location = new System.Drawing.Point(3, 26);
			this.rBtnModulationShaping01.Name = "rBtnModulationShaping01";
			this.rBtnModulationShaping01.Size = new System.Drawing.Size(138, 17);
			this.rBtnModulationShaping01.TabIndex = 1;
			this.rBtnModulationShaping01.Text = "Gaussian filter, BT = 1.0";
			this.rBtnModulationShaping01.UseVisualStyleBackColor = true;
			this.rBtnModulationShaping01.CheckedChanged += new System.EventHandler(this.rBtnModulationShaping_CheckedChanged);
			// 
			// rBtnModulationShapingOff
			// 
			this.rBtnModulationShapingOff.AutoSize = true;
			this.rBtnModulationShapingOff.Checked = true;
			this.rBtnModulationShapingOff.Location = new System.Drawing.Point(3, 3);
			this.rBtnModulationShapingOff.Name = "rBtnModulationShapingOff";
			this.rBtnModulationShapingOff.Size = new System.Drawing.Size(45, 17);
			this.rBtnModulationShapingOff.TabIndex = 0;
			this.rBtnModulationShapingOff.TabStop = true;
			this.rBtnModulationShapingOff.Text = "OFF";
			this.rBtnModulationShapingOff.UseVisualStyleBackColor = true;
			this.rBtnModulationShapingOff.CheckedChanged += new System.EventHandler(this.rBtnModulationShaping_CheckedChanged);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(6, 24);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(62, 13);
			this.label5.TabIndex = 0;
			this.label5.Text = "Modulation:";
			// 
			// panel1
			// 
			this.panel1.AutoSize = true;
			this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.panel1.Controls.Add(this.rBtnContinousBitSyncOff);
			this.panel1.Controls.Add(this.rBtnContinousBitSyncOn);
			this.panel1.Controls.Add(this.rBtnPacketHandler);
			this.panel1.Location = new System.Drawing.Point(164, 19);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(128, 69);
			this.panel1.TabIndex = 0;
			// 
			// rBtnContinousBitSyncOff
			// 
			this.rBtnContinousBitSyncOff.AutoSize = true;
			this.rBtnContinousBitSyncOff.Location = new System.Drawing.Point(3, 49);
			this.rBtnContinousBitSyncOff.Name = "rBtnContinousBitSyncOff";
			this.rBtnContinousBitSyncOff.Size = new System.Drawing.Size(98, 17);
			this.rBtnContinousBitSyncOff.TabIndex = 2;
			this.rBtnContinousBitSyncOff.Text = "OFF- Continous";
			this.rBtnContinousBitSyncOff.UseVisualStyleBackColor = true;
			this.rBtnContinousBitSyncOff.CheckedChanged += new System.EventHandler(this.rBtnDataMode_CheckedChanged);
			// 
			// rBtnContinousBitSyncOn
			// 
			this.rBtnContinousBitSyncOn.AutoSize = true;
			this.rBtnContinousBitSyncOn.Location = new System.Drawing.Point(3, 26);
			this.rBtnContinousBitSyncOn.Name = "rBtnContinousBitSyncOn";
			this.rBtnContinousBitSyncOn.Size = new System.Drawing.Size(97, 17);
			this.rBtnContinousBitSyncOn.TabIndex = 1;
			this.rBtnContinousBitSyncOn.Text = "ON - Continous";
			this.rBtnContinousBitSyncOn.UseVisualStyleBackColor = true;
			this.rBtnContinousBitSyncOn.CheckedChanged += new System.EventHandler(this.rBtnDataMode_CheckedChanged);
			// 
			// rBtnPacketHandler
			// 
			this.rBtnPacketHandler.AutoSize = true;
			this.rBtnPacketHandler.Checked = true;
			this.rBtnPacketHandler.Location = new System.Drawing.Point(3, 3);
			this.rBtnPacketHandler.Name = "rBtnPacketHandler";
			this.rBtnPacketHandler.Size = new System.Drawing.Size(122, 17);
			this.rBtnPacketHandler.TabIndex = 0;
			this.rBtnPacketHandler.TabStop = true;
			this.rBtnPacketHandler.Text = "ON - Packet handler";
			this.rBtnPacketHandler.UseVisualStyleBackColor = true;
			this.rBtnPacketHandler.CheckedChanged += new System.EventHandler(this.rBtnDataMode_CheckedChanged);
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(6, 49);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(40, 13);
			this.label7.TabIndex = 3;
			this.label7.Text = "Bitrate:";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(6, 53);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(102, 13);
			this.label6.TabIndex = 2;
			this.label6.Text = "Modulation shaping:";
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(6, 75);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(34, 13);
			this.label10.TabIndex = 6;
			this.label10.Text = "Fdev:";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(294, 23);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(20, 13);
			this.label9.TabIndex = 2;
			this.label9.Text = "Hz";
			// 
			// label14
			// 
			this.label14.AutoSize = true;
			this.label14.Location = new System.Drawing.Point(6, 23);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(74, 13);
			this.label14.TabIndex = 0;
			this.label14.Text = "RF frequency:";
			// 
			// label16
			// 
			this.label16.AutoSize = true;
			this.label16.Location = new System.Drawing.Point(296, 49);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(14, 13);
			this.label16.TabIndex = 4;
			this.label16.Text = "V";
			// 
			// lblRcOscillatorCalStat
			// 
			this.lblRcOscillatorCalStat.AutoSize = true;
			this.lblRcOscillatorCalStat.Location = new System.Drawing.Point(6, 81);
			this.lblRcOscillatorCalStat.Name = "lblRcOscillatorCalStat";
			this.lblRcOscillatorCalStat.Size = new System.Drawing.Size(151, 13);
			this.lblRcOscillatorCalStat.TabIndex = 5;
			this.lblRcOscillatorCalStat.Text = "RC oscillator calibration status:";
			// 
			// lblRcOscillatorCal
			// 
			this.lblRcOscillatorCal.AutoSize = true;
			this.lblRcOscillatorCal.Location = new System.Drawing.Point(6, 56);
			this.lblRcOscillatorCal.Name = "lblRcOscillatorCal";
			this.lblRcOscillatorCal.Size = new System.Drawing.Size(120, 13);
			this.lblRcOscillatorCal.TabIndex = 3;
			this.lblRcOscillatorCal.Text = "RC oscillator calibration:";
			// 
			// label15
			// 
			this.label15.AutoSize = true;
			this.label15.Location = new System.Drawing.Point(8, 24);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(107, 13);
			this.label15.TabIndex = 0;
			this.label15.Text = "Low battery detector:";
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.label13.Location = new System.Drawing.Point(294, 23);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(20, 13);
			this.label13.TabIndex = 2;
			this.label13.Text = "Hz";
			// 
			// label17
			// 
			this.label17.AutoSize = true;
			this.label17.Location = new System.Drawing.Point(8, 48);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(130, 13);
			this.label17.TabIndex = 2;
			this.label17.Text = "Low battery threshold trim:";
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(294, 75);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(20, 13);
			this.label11.TabIndex = 9;
			this.label11.Text = "Hz";
			// 
			// label18
			// 
			this.label18.AutoSize = true;
			this.label18.Location = new System.Drawing.Point(8, 73);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(108, 13);
			this.label18.TabIndex = 5;
			this.label18.Text = "Low battery indicator:";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(294, 49);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(24, 13);
			this.label8.TabIndex = 5;
			this.label8.Text = "bps";
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Location = new System.Drawing.Point(137, 75);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(21, 13);
			this.label12.TabIndex = 7;
			this.label12.Text = "+/-";
			// 
			// rBtnSequencerOff
			// 
			this.rBtnSequencerOff.AutoSize = true;
			this.rBtnSequencerOff.Location = new System.Drawing.Point(50, 0);
			this.rBtnSequencerOff.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
			this.rBtnSequencerOff.Name = "rBtnSequencerOff";
			this.rBtnSequencerOff.Size = new System.Drawing.Size(45, 17);
			this.rBtnSequencerOff.TabIndex = 1;
			this.rBtnSequencerOff.TabStop = true;
			this.rBtnSequencerOff.Text = "OFF";
			this.rBtnSequencerOff.UseVisualStyleBackColor = true;
			this.rBtnSequencerOff.CheckedChanged += new System.EventHandler(this.rBtnSequencer_CheckedChanged);
			// 
			// rBtnSequencerOn
			// 
			this.rBtnSequencerOn.AutoSize = true;
			this.rBtnSequencerOn.Location = new System.Drawing.Point(3, 0);
			this.rBtnSequencerOn.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
			this.rBtnSequencerOn.Name = "rBtnSequencerOn";
			this.rBtnSequencerOn.Size = new System.Drawing.Size(41, 17);
			this.rBtnSequencerOn.TabIndex = 0;
			this.rBtnSequencerOn.TabStop = true;
			this.rBtnSequencerOn.Text = "ON";
			this.rBtnSequencerOn.UseVisualStyleBackColor = true;
			this.rBtnSequencerOn.CheckedChanged += new System.EventHandler(this.rBtnSequencer_CheckedChanged);
			// 
			// label19
			// 
			this.label19.AutoSize = true;
			this.label19.Location = new System.Drawing.Point(7, 99);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(62, 13);
			this.label19.TabIndex = 10;
			this.label19.Text = "Sequencer:";
			// 
			// panel5
			// 
			this.panel5.AutoSize = true;
			this.panel5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.panel5.Controls.Add(this.rBtnSequencerOff);
			this.panel5.Controls.Add(this.rBtnSequencerOn);
			this.panel5.Location = new System.Drawing.Point(164, 97);
			this.panel5.Name = "panel5";
			this.panel5.Size = new System.Drawing.Size(98, 17);
			this.panel5.TabIndex = 11;
			// 
			// label20
			// 
			this.label20.AutoSize = true;
			this.label20.Location = new System.Drawing.Point(8, 87);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(67, 13);
			this.label20.TabIndex = 0;
			this.label20.Text = "Listen mode:";
			// 
			// panel6
			// 
			this.panel6.AutoSize = true;
			this.panel6.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.panel6.Controls.Add(this.rBtnListenModeOff);
			this.panel6.Controls.Add(this.rBtnListenModeOn);
			this.panel6.Location = new System.Drawing.Point(165, 85);
			this.panel6.Name = "panel6";
			this.panel6.Size = new System.Drawing.Size(98, 17);
			this.panel6.TabIndex = 1;
			// 
			// rBtnListenModeOff
			// 
			this.rBtnListenModeOff.AutoSize = true;
			this.rBtnListenModeOff.Location = new System.Drawing.Point(50, 0);
			this.rBtnListenModeOff.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
			this.rBtnListenModeOff.Name = "rBtnListenModeOff";
			this.rBtnListenModeOff.Size = new System.Drawing.Size(45, 17);
			this.rBtnListenModeOff.TabIndex = 1;
			this.rBtnListenModeOff.TabStop = true;
			this.rBtnListenModeOff.Text = "OFF";
			this.rBtnListenModeOff.UseVisualStyleBackColor = true;
			this.rBtnListenModeOff.CheckedChanged += new System.EventHandler(this.rBtnListenMode_CheckedChanged);
			// 
			// rBtnListenModeOn
			// 
			this.rBtnListenModeOn.AutoSize = true;
			this.rBtnListenModeOn.Location = new System.Drawing.Point(3, 0);
			this.rBtnListenModeOn.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
			this.rBtnListenModeOn.Name = "rBtnListenModeOn";
			this.rBtnListenModeOn.Size = new System.Drawing.Size(41, 17);
			this.rBtnListenModeOn.TabIndex = 0;
			this.rBtnListenModeOn.TabStop = true;
			this.rBtnListenModeOn.Text = "ON";
			this.rBtnListenModeOn.UseVisualStyleBackColor = true;
			this.rBtnListenModeOn.CheckedChanged += new System.EventHandler(this.rBtnListenMode_CheckedChanged);
			// 
			// btnListenModeAbort
			// 
			this.btnListenModeAbort.Enabled = false;
			this.btnListenModeAbort.Location = new System.Drawing.Point(269, 82);
			this.btnListenModeAbort.Name = "btnListenModeAbort";
			this.btnListenModeAbort.Size = new System.Drawing.Size(75, 23);
			this.btnListenModeAbort.TabIndex = 2;
			this.btnListenModeAbort.Text = "Abort";
			this.btnListenModeAbort.UseVisualStyleBackColor = true;
			this.btnListenModeAbort.Visible = false;
			this.btnListenModeAbort.Click += new System.EventHandler(this.btnListenModeAbort_Click);
			// 
			// label21
			// 
			this.label21.AutoSize = true;
			this.label21.Location = new System.Drawing.Point(8, 114);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(105, 13);
			this.label21.TabIndex = 3;
			this.label21.Text = "Listen resolution idle:";
			// 
			// cBoxListenResolIdle
			// 
			this.cBoxListenResolIdle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cBoxListenResolIdle.FormattingEnabled = true;
			this.cBoxListenResolIdle.Items.AddRange(new object[] {
            "64",
            "4\'100",
            "262\'000"});
			this.cBoxListenResolIdle.Location = new System.Drawing.Point(165, 108);
			this.cBoxListenResolIdle.Name = "cBoxListenResolIdle";
			this.cBoxListenResolIdle.Size = new System.Drawing.Size(124, 21);
			this.cBoxListenResolIdle.TabIndex = 4;
			this.cBoxListenResolIdle.SelectedIndexChanged += new System.EventHandler(this.cBoxListenResolIdle_SelectedIndexChanged);
			// 
			// label22
			// 
			this.label22.AutoSize = true;
			this.label22.Location = new System.Drawing.Point(295, 112);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(18, 13);
			this.label22.TabIndex = 5;
			this.label22.Text = "µs";
			// 
			// panel7
			// 
			this.panel7.AutoSize = true;
			this.panel7.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.panel7.BackColor = System.Drawing.Color.Transparent;
			this.panel7.Controls.Add(this.rBtnListenCriteria1);
			this.panel7.Controls.Add(this.rBtnListenCriteria0);
			this.panel7.Location = new System.Drawing.Point(165, 162);
			this.panel7.Name = "panel7";
			this.panel7.Size = new System.Drawing.Size(226, 46);
			this.panel7.TabIndex = 10;
			// 
			// rBtnListenCriteria1
			// 
			this.rBtnListenCriteria1.AutoSize = true;
			this.rBtnListenCriteria1.Location = new System.Drawing.Point(3, 26);
			this.rBtnListenCriteria1.Name = "rBtnListenCriteria1";
			this.rBtnListenCriteria1.Size = new System.Drawing.Size(220, 17);
			this.rBtnListenCriteria1.TabIndex = 1;
			this.rBtnListenCriteria1.Text = "> RssiThreshold && SyncAddress detected";
			this.rBtnListenCriteria1.UseVisualStyleBackColor = true;
			this.rBtnListenCriteria1.CheckedChanged += new System.EventHandler(this.rBtnListenCriteria_CheckedChanged);
			// 
			// rBtnListenCriteria0
			// 
			this.rBtnListenCriteria0.AutoSize = true;
			this.rBtnListenCriteria0.Checked = true;
			this.rBtnListenCriteria0.Location = new System.Drawing.Point(3, 3);
			this.rBtnListenCriteria0.Name = "rBtnListenCriteria0";
			this.rBtnListenCriteria0.Size = new System.Drawing.Size(101, 17);
			this.rBtnListenCriteria0.TabIndex = 0;
			this.rBtnListenCriteria0.TabStop = true;
			this.rBtnListenCriteria0.Text = "> RssiThreshold";
			this.rBtnListenCriteria0.UseVisualStyleBackColor = true;
			this.rBtnListenCriteria0.CheckedChanged += new System.EventHandler(this.rBtnListenCriteria_CheckedChanged);
			// 
			// label23
			// 
			this.label23.AutoSize = true;
			this.label23.Location = new System.Drawing.Point(8, 167);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(72, 13);
			this.label23.TabIndex = 9;
			this.label23.Text = "Listen criteria:";
			// 
			// label24
			// 
			this.label24.AutoSize = true;
			this.label24.Location = new System.Drawing.Point(8, 217);
			this.label24.Name = "label24";
			this.label24.Size = new System.Drawing.Size(59, 13);
			this.label24.TabIndex = 11;
			this.label24.Text = "Listen end:";
			// 
			// cBoxListenEnd
			// 
			this.cBoxListenEnd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cBoxListenEnd.FormattingEnabled = true;
			this.cBoxListenEnd.Items.AddRange(new object[] {
            "Rx",
            "Rx & Mode after IRQ",
            "Rx & Idle after IRQ"});
			this.cBoxListenEnd.Location = new System.Drawing.Point(165, 214);
			this.cBoxListenEnd.Name = "cBoxListenEnd";
			this.cBoxListenEnd.Size = new System.Drawing.Size(124, 21);
			this.cBoxListenEnd.TabIndex = 12;
			this.cBoxListenEnd.SelectedIndexChanged += new System.EventHandler(this.cBoxListenEnd_SelectedIndexChanged);
			// 
			// label25
			// 
			this.label25.AutoSize = true;
			this.label25.Location = new System.Drawing.Point(295, 245);
			this.label25.Name = "label25";
			this.label25.Size = new System.Drawing.Size(20, 13);
			this.label25.TabIndex = 15;
			this.label25.Text = "ms";
			// 
			// label26
			// 
			this.label26.AutoSize = true;
			this.label26.Location = new System.Drawing.Point(295, 271);
			this.label26.Name = "label26";
			this.label26.Size = new System.Drawing.Size(20, 13);
			this.label26.TabIndex = 18;
			this.label26.Text = "ms";
			// 
			// label27
			// 
			this.label27.AutoSize = true;
			this.label27.Location = new System.Drawing.Point(8, 245);
			this.label27.Name = "label27";
			this.label27.Size = new System.Drawing.Size(79, 13);
			this.label27.TabIndex = 13;
			this.label27.Text = "Listen idle time:";
			// 
			// label28
			// 
			this.label28.AutoSize = true;
			this.label28.Location = new System.Drawing.Point(8, 270);
			this.label28.Name = "label28";
			this.label28.Size = new System.Drawing.Size(76, 13);
			this.label28.TabIndex = 16;
			this.label28.Text = "Listen Rx time:";
			// 
			// errorProvider
			// 
			this.errorProvider.ContainerControl = this;
			// 
			// nudBitRate
			// 
			this.errorProvider.SetIconPadding(this.nudBitRate, 30);
			this.nudBitRate.Location = new System.Drawing.Point(164, 45);
			this.nudBitRate.Maximum = new decimal(new int[] {
            603774,
            0,
            0,
            0});
			this.nudBitRate.Minimum = new decimal(new int[] {
            600,
            0,
            0,
            0});
			this.nudBitRate.Name = "nudBitRate";
			this.nudBitRate.Size = new System.Drawing.Size(124, 20);
			this.nudBitRate.TabIndex = 4;
			this.nudBitRate.ThousandsSeparator = true;
			this.nudBitRate.Value = new decimal(new int[] {
            4800,
            0,
            0,
            0});
			this.nudBitRate.ValueChanged += new System.EventHandler(this.nudBitRate_ValueChanged);
			// 
			// nudFdev
			// 
			this.errorProvider.SetIconPadding(this.nudFdev, 30);
			this.nudFdev.Increment = new decimal(new int[] {
            61,
            0,
            0,
            0});
			this.nudFdev.Location = new System.Drawing.Point(164, 71);
			this.nudFdev.Maximum = new decimal(new int[] {
            300000,
            0,
            0,
            0});
			this.nudFdev.Name = "nudFdev";
			this.nudFdev.Size = new System.Drawing.Size(124, 20);
			this.nudFdev.TabIndex = 8;
			this.nudFdev.ThousandsSeparator = true;
			this.nudFdev.Value = new decimal(new int[] {
            5005,
            0,
            0,
            0});
			this.nudFdev.ValueChanged += new System.EventHandler(this.nudFdev_ValueChanged);
			// 
			// nudFrequencyRf
			// 
			this.errorProvider.SetIconPadding(this.nudFrequencyRf, 30);
			this.nudFrequencyRf.Increment = new decimal(new int[] {
            61,
            0,
            0,
            0});
			this.nudFrequencyRf.Location = new System.Drawing.Point(164, 19);
			this.nudFrequencyRf.Maximum = new decimal(new int[] {
            1020000000,
            0,
            0,
            0});
			this.nudFrequencyRf.Minimum = new decimal(new int[] {
            290000000,
            0,
            0,
            0});
			this.nudFrequencyRf.Name = "nudFrequencyRf";
			this.nudFrequencyRf.Size = new System.Drawing.Size(124, 20);
			this.nudFrequencyRf.TabIndex = 1;
			this.nudFrequencyRf.ThousandsSeparator = true;
			this.nudFrequencyRf.Value = new decimal(new int[] {
            915000000,
            0,
            0,
            0});
			this.nudFrequencyRf.ValueChanged += new System.EventHandler(this.nudFrequencyRf_ValueChanged);
			// 
			// lblListenResolRx
			// 
			this.lblListenResolRx.AutoSize = true;
			this.lblListenResolRx.Location = new System.Drawing.Point(8, 141);
			this.lblListenResolRx.Name = "lblListenResolRx";
			this.lblListenResolRx.Size = new System.Drawing.Size(102, 13);
			this.lblListenResolRx.TabIndex = 6;
			this.lblListenResolRx.Text = "Listen resolution Rx:";
			// 
			// label30
			// 
			this.label30.AutoSize = true;
			this.label30.Location = new System.Drawing.Point(295, 139);
			this.label30.Name = "label30";
			this.label30.Size = new System.Drawing.Size(18, 13);
			this.label30.TabIndex = 8;
			this.label30.Text = "µs";
			// 
			// cBoxListenResolRx
			// 
			this.cBoxListenResolRx.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cBoxListenResolRx.FormattingEnabled = true;
			this.cBoxListenResolRx.Items.AddRange(new object[] {
            "64",
            "4\'100",
            "262\'000"});
			this.cBoxListenResolRx.Location = new System.Drawing.Point(165, 135);
			this.cBoxListenResolRx.Name = "cBoxListenResolRx";
			this.cBoxListenResolRx.Size = new System.Drawing.Size(124, 21);
			this.cBoxListenResolRx.TabIndex = 7;
			this.cBoxListenResolRx.SelectedIndexChanged += new System.EventHandler(this.cBoxListenResolRx_SelectedIndexChanged);
			// 
			// gBoxGeneral
			// 
			this.gBoxGeneral.Controls.Add(this.nudBitRate);
			this.gBoxGeneral.Controls.Add(this.label12);
			this.gBoxGeneral.Controls.Add(this.label8);
			this.gBoxGeneral.Controls.Add(this.label11);
			this.gBoxGeneral.Controls.Add(this.label13);
			this.gBoxGeneral.Controls.Add(this.label14);
			this.gBoxGeneral.Controls.Add(this.panel5);
			this.gBoxGeneral.Controls.Add(this.label19);
			this.gBoxGeneral.Controls.Add(this.label10);
			this.gBoxGeneral.Controls.Add(this.label7);
			this.gBoxGeneral.Controls.Add(this.nudFdev);
			this.gBoxGeneral.Controls.Add(this.nudFrequencyRf);
			this.gBoxGeneral.Location = new System.Drawing.Point(16, 9);
			this.gBoxGeneral.Name = "gBoxGeneral";
			this.gBoxGeneral.Size = new System.Drawing.Size(355, 122);
			this.gBoxGeneral.TabIndex = 0;
			this.gBoxGeneral.TabStop = false;
			this.gBoxGeneral.Text = "General";
			this.gBoxGeneral.MouseEnter += new System.EventHandler(this.control_MouseEnter);
			this.gBoxGeneral.MouseLeave += new System.EventHandler(this.control_MouseLeave);
			// 
			// gBoxBitSyncDataMode
			// 
			this.gBoxBitSyncDataMode.Controls.Add(this.panel1);
			this.gBoxBitSyncDataMode.Location = new System.Drawing.Point(16, 137);
			this.gBoxBitSyncDataMode.Name = "gBoxBitSyncDataMode";
			this.gBoxBitSyncDataMode.Size = new System.Drawing.Size(355, 91);
			this.gBoxBitSyncDataMode.TabIndex = 1;
			this.gBoxBitSyncDataMode.TabStop = false;
			this.gBoxBitSyncDataMode.Text = "Bit synchronizer / data mode";
			this.gBoxBitSyncDataMode.MouseEnter += new System.EventHandler(this.control_MouseEnter);
			this.gBoxBitSyncDataMode.MouseLeave += new System.EventHandler(this.control_MouseLeave);
			// 
			// gBoxModulation
			// 
			this.gBoxModulation.Controls.Add(this.panel2);
			this.gBoxModulation.Controls.Add(this.label6);
			this.gBoxModulation.Controls.Add(this.label5);
			this.gBoxModulation.Controls.Add(this.panel3);
			this.gBoxModulation.Location = new System.Drawing.Point(16, 234);
			this.gBoxModulation.Name = "gBoxModulation";
			this.gBoxModulation.Size = new System.Drawing.Size(355, 143);
			this.gBoxModulation.TabIndex = 2;
			this.gBoxModulation.TabStop = false;
			this.gBoxModulation.Text = "Modulation";
			this.gBoxModulation.MouseEnter += new System.EventHandler(this.control_MouseEnter);
			this.gBoxModulation.MouseLeave += new System.EventHandler(this.control_MouseLeave);
			// 
			// gBoxOscillators
			// 
			this.gBoxOscillators.Controls.Add(this.nudFrequencyXo);
			this.gBoxOscillators.Controls.Add(this.label9);
			this.gBoxOscillators.Controls.Add(this.btnRcCalibration);
			this.gBoxOscillators.Controls.Add(this.label1);
			this.gBoxOscillators.Controls.Add(this.lblRcOscillatorCal);
			this.gBoxOscillators.Controls.Add(this.lblRcOscillatorCalStat);
			this.gBoxOscillators.Controls.Add(this.ledRcCalibration);
			this.gBoxOscillators.Location = new System.Drawing.Point(16, 383);
			this.gBoxOscillators.Name = "gBoxOscillators";
			this.gBoxOscillators.Size = new System.Drawing.Size(355, 100);
			this.gBoxOscillators.TabIndex = 3;
			this.gBoxOscillators.TabStop = false;
			this.gBoxOscillators.Text = "Oscillators";
			this.gBoxOscillators.MouseEnter += new System.EventHandler(this.control_MouseEnter);
			this.gBoxOscillators.MouseLeave += new System.EventHandler(this.control_MouseLeave);
			// 
			// nudFrequencyXo
			// 
			this.nudFrequencyXo.Location = new System.Drawing.Point(164, 19);
			this.nudFrequencyXo.Maximum = new decimal(new int[] {
            32000000,
            0,
            0,
            0});
			this.nudFrequencyXo.Minimum = new decimal(new int[] {
            26000000,
            0,
            0,
            0});
			this.nudFrequencyXo.Name = "nudFrequencyXo";
			this.nudFrequencyXo.Size = new System.Drawing.Size(124, 20);
			this.nudFrequencyXo.TabIndex = 1;
			this.nudFrequencyXo.ThousandsSeparator = true;
			this.nudFrequencyXo.Value = new decimal(new int[] {
            32000000,
            0,
            0,
            0});
			this.nudFrequencyXo.ValueChanged += new System.EventHandler(this.nudFrequencyXo_ValueChanged);
			// 
			// ledRcCalibration
			// 
			this.ledRcCalibration.BackColor = System.Drawing.Color.Transparent;
			this.ledRcCalibration.LedColor = System.Drawing.Color.Green;
			this.ledRcCalibration.LedSize = new System.Drawing.Size(11, 11);
			this.ledRcCalibration.Location = new System.Drawing.Point(164, 80);
			this.ledRcCalibration.Name = "ledRcCalibration";
			this.ledRcCalibration.Size = new System.Drawing.Size(15, 15);
			this.ledRcCalibration.TabIndex = 6;
			this.ledRcCalibration.Text = "led1";
			// 
			// gBoxBatteryManagement
			// 
			this.gBoxBatteryManagement.Controls.Add(this.panel4);
			this.gBoxBatteryManagement.Controls.Add(this.label18);
			this.gBoxBatteryManagement.Controls.Add(this.label17);
			this.gBoxBatteryManagement.Controls.Add(this.label15);
			this.gBoxBatteryManagement.Controls.Add(this.label16);
			this.gBoxBatteryManagement.Controls.Add(this.cBoxLowBatTrim);
			this.gBoxBatteryManagement.Controls.Add(this.ledLowBatMonitor);
			this.gBoxBatteryManagement.Location = new System.Drawing.Point(377, 383);
			this.gBoxBatteryManagement.Name = "gBoxBatteryManagement";
			this.gBoxBatteryManagement.Size = new System.Drawing.Size(405, 100);
			this.gBoxBatteryManagement.TabIndex = 5;
			this.gBoxBatteryManagement.TabStop = false;
			this.gBoxBatteryManagement.Text = "Battery management";
			this.gBoxBatteryManagement.MouseEnter += new System.EventHandler(this.control_MouseEnter);
			this.gBoxBatteryManagement.MouseLeave += new System.EventHandler(this.control_MouseLeave);
			// 
			// ledLowBatMonitor
			// 
			this.ledLowBatMonitor.BackColor = System.Drawing.Color.Transparent;
			this.ledLowBatMonitor.LedColor = System.Drawing.Color.Green;
			this.ledLowBatMonitor.LedSize = new System.Drawing.Size(11, 11);
			this.ledLowBatMonitor.Location = new System.Drawing.Point(166, 72);
			this.ledLowBatMonitor.Name = "ledLowBatMonitor";
			this.ledLowBatMonitor.Size = new System.Drawing.Size(15, 15);
			this.ledLowBatMonitor.TabIndex = 6;
			this.ledLowBatMonitor.Text = "Low battery";
			// 
			// gBoxListenMode
			// 
			this.gBoxListenMode.Controls.Add(this.panel6);
			this.gBoxListenMode.Controls.Add(this.label20);
			this.gBoxListenMode.Controls.Add(this.label21);
			this.gBoxListenMode.Controls.Add(this.label23);
			this.gBoxListenMode.Controls.Add(this.lblListenResolRx);
			this.gBoxListenMode.Controls.Add(this.label24);
			this.gBoxListenMode.Controls.Add(this.label27);
			this.gBoxListenMode.Controls.Add(this.label28);
			this.gBoxListenMode.Controls.Add(this.btnListenModeAbort);
			this.gBoxListenMode.Controls.Add(this.label22);
			this.gBoxListenMode.Controls.Add(this.cBoxListenEnd);
			this.gBoxListenMode.Controls.Add(this.label25);
			this.gBoxListenMode.Controls.Add(this.cBoxListenResolRx);
			this.gBoxListenMode.Controls.Add(this.label30);
			this.gBoxListenMode.Controls.Add(this.cBoxListenResolIdle);
			this.gBoxListenMode.Controls.Add(this.label26);
			this.gBoxListenMode.Controls.Add(this.nudListenCoefRx);
			this.gBoxListenMode.Controls.Add(this.panel7);
			this.gBoxListenMode.Controls.Add(this.nudListenCoefIdle);
			this.gBoxListenMode.Location = new System.Drawing.Point(377, 9);
			this.gBoxListenMode.Name = "gBoxListenMode";
			this.gBoxListenMode.Size = new System.Drawing.Size(405, 368);
			this.gBoxListenMode.TabIndex = 4;
			this.gBoxListenMode.TabStop = false;
			this.gBoxListenMode.Text = "Listen mode";
			this.gBoxListenMode.MouseEnter += new System.EventHandler(this.control_MouseEnter);
			this.gBoxListenMode.MouseLeave += new System.EventHandler(this.control_MouseLeave);
			// 
			// nudListenCoefRx
			// 
			this.nudListenCoefRx.DecimalPlaces = 3;
			this.nudListenCoefRx.Increment = new decimal(new int[] {
            41,
            0,
            0,
            65536});
			this.nudListenCoefRx.Location = new System.Drawing.Point(165, 267);
			this.nudListenCoefRx.Maximum = new decimal(new int[] {
            10455,
            0,
            0,
            65536});
			this.nudListenCoefRx.Name = "nudListenCoefRx";
			this.nudListenCoefRx.Size = new System.Drawing.Size(124, 20);
			this.nudListenCoefRx.TabIndex = 17;
			this.nudListenCoefRx.ThousandsSeparator = true;
			this.nudListenCoefRx.Value = new decimal(new int[] {
            1312,
            0,
            0,
            65536});
			this.nudListenCoefRx.ValueChanged += new System.EventHandler(this.nudListenCoefRx_ValueChanged);
			// 
			// nudListenCoefIdle
			// 
			this.nudListenCoefIdle.DecimalPlaces = 3;
			this.nudListenCoefIdle.Increment = new decimal(new int[] {
            41,
            0,
            0,
            65536});
			this.nudListenCoefIdle.Location = new System.Drawing.Point(165, 241);
			this.nudListenCoefIdle.Maximum = new decimal(new int[] {
            10455,
            0,
            0,
            65536});
			this.nudListenCoefIdle.Name = "nudListenCoefIdle";
			this.nudListenCoefIdle.Size = new System.Drawing.Size(124, 20);
			this.nudListenCoefIdle.TabIndex = 14;
			this.nudListenCoefIdle.ThousandsSeparator = true;
			this.nudListenCoefIdle.Value = new decimal(new int[] {
            10045,
            0,
            0,
            65536});
			this.nudListenCoefIdle.ValueChanged += new System.EventHandler(this.nudListenCoefIdle_ValueChanged);
			// 
			// CommonViewControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.gBoxListenMode);
			this.Controls.Add(this.gBoxBatteryManagement);
			this.Controls.Add(this.gBoxOscillators);
			this.Controls.Add(this.gBoxModulation);
			this.Controls.Add(this.gBoxBitSyncDataMode);
			this.Controls.Add(this.gBoxGeneral);
			this.Name = "CommonViewControl";
			this.Size = new System.Drawing.Size(799, 493);
			this.panel4.ResumeLayout(false);
			this.panel4.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.panel3.ResumeLayout(false);
			this.panel3.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.panel5.ResumeLayout(false);
			this.panel5.PerformLayout();
			this.panel6.ResumeLayout(false);
			this.panel6.PerformLayout();
			this.panel7.ResumeLayout(false);
			this.panel7.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudBitRate)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudFdev)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudFrequencyRf)).EndInit();
			this.gBoxGeneral.ResumeLayout(false);
			this.gBoxGeneral.PerformLayout();
			this.gBoxBitSyncDataMode.ResumeLayout(false);
			this.gBoxBitSyncDataMode.PerformLayout();
			this.gBoxModulation.ResumeLayout(false);
			this.gBoxModulation.PerformLayout();
			this.gBoxOscillators.ResumeLayout(false);
			this.gBoxOscillators.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudFrequencyXo)).EndInit();
			this.gBoxBatteryManagement.ResumeLayout(false);
			this.gBoxBatteryManagement.PerformLayout();
			this.gBoxListenMode.ResumeLayout(false);
			this.gBoxListenMode.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudListenCoefRx)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudListenCoefIdle)).EndInit();
			this.ResumeLayout(false);

		}

		private void nudBitRate_ValueChanged(object sender, EventArgs e)
		{
			int num1 = (int)Math.Round((decimal)(FrequencyXo / BitRate), MidpointRounding.AwayFromZero);
			int num = (int)Math.Round((decimal)(FrequencyXo / nudBitRate.Value), MidpointRounding.AwayFromZero);
			int num2 = (int)(nudBitRate.Value - BitRate);
			nudBitRate.ValueChanged -= new EventHandler(nudBitRate_ValueChanged);
			if ((num2 >= -1) && (num2 <= 1))
			{
				nudBitRate.Value = Math.Round((decimal)(FrequencyXo / (num - num2)), MidpointRounding.AwayFromZero);
			}
			else
			{
				nudBitRate.Value = Math.Round((decimal)(FrequencyXo / num), MidpointRounding.AwayFromZero);
			}
			nudBitRate.ValueChanged += new EventHandler(nudBitRate_ValueChanged);
			BitRate = nudBitRate.Value;
			OnBitRateChanged(BitRate);
		}

		private void nudFdev_ValueChanged(object sender, EventArgs e)
		{
			Fdev = nudFdev.Value;
			OnFdevChanged(Fdev);
		}

		private void nudFrequencyRf_ValueChanged(object sender, EventArgs e)
		{
			FrequencyRf = nudFrequencyRf.Value;
			OnFrequencyRfChanged(FrequencyRf);
		}

		private void nudFrequencyXo_ValueChanged(object sender, EventArgs e)
		{
			FrequencyXo = nudFrequencyXo.Value;
			OnFrequencyXoChanged(FrequencyXo);
		}

		private void nudListenCoefIdle_ValueChanged(object sender, EventArgs e)
		{
			ListenCoefIdle = nudListenCoefIdle.Value;
			OnListenCoefIdleChanged(ListenCoefIdle);
		}

		private void nudListenCoefRx_ValueChanged(object sender, EventArgs e)
		{
			ListenCoefRx = nudListenCoefRx.Value;
			OnListenCoefRxChanged(ListenCoefRx);
		}

		private void OnBitRateChanged(decimal value)
		{
			if (BitRateChanged != null)
				BitRateChanged(this, new DecimalEventArg(value));
		}

		private void OnDataModeChanged(DataModeEnum value)
		{
			if (DataModeChanged != null)
				DataModeChanged(this, new DataModeEventArg(value));
		}

		private void OnDocumentationChanged(DocumentationChangedEventArgs e)
		{
			if (DocumentationChanged != null)
				DocumentationChanged(this, e);
		}

		private void OnFdevChanged(decimal value)
		{
			if (FdevChanged != null)
				FdevChanged(this, new DecimalEventArg(value));
		}

		private void OnFrequencyRfChanged(decimal value)
		{
			if (FrequencyRfChanged != null)
				FrequencyRfChanged(this, new DecimalEventArg(value));
		}

		private void OnFrequencyXoChanged(decimal value)
		{
			if (FrequencyXoChanged != null)
				FrequencyXoChanged(this, new DecimalEventArg(value));
		}

		private void OnListenCoefIdleChanged(decimal value)
		{
			if (ListenCoefIdleChanged != null)
				ListenCoefIdleChanged(this, new DecimalEventArg(value));
		}

		private void OnListenCoefRxChanged(decimal value)
		{
			if (ListenCoefRxChanged != null)
				ListenCoefRxChanged(this, new DecimalEventArg(value));
		}

		private void OnListenCriteriaChanged(ListenCriteriaEnum value)
		{
			if (ListenCriteriaChanged != null)
				ListenCriteriaChanged(this, new ListenCriteriaEventArg(value));
		}

		private void OnListenEndChanged(ListenEndEnum value)
		{
			if (ListenEndChanged != null)
				ListenEndChanged(this, new ListenEndEventArg(value));
		}

		private void OnListenModeAbortChanged()
		{
			if (ListenModeAbortChanged != null)
				ListenModeAbortChanged(this, EventArgs.Empty);
		}

		private void OnListenModeChanged(bool value)
		{
			if (ListenModeChanged != null)
				ListenModeChanged(this, new BooleanEventArg(value));
		}

		private void OnListenResolIdleChanged(ListenResolEnum value)
		{
			if (ListenResolIdleChanged != null)
				ListenResolIdleChanged(this, new ListenResolEventArg(value));
		}

		private void OnListenResolRxChanged(ListenResolEnum value)
		{
			if (ListenResolRxChanged != null)
				ListenResolRxChanged(this, new ListenResolEventArg(value));
		}

		private void OnLowBatOnChanged(bool value)
		{
			if (LowBatOnChanged != null)
				LowBatOnChanged(this, new BooleanEventArg(value));
		}

		private void OnLowBatTrimChanged(LowBatTrimEnum value)
		{
			if (LowBatTrimChanged != null)
				LowBatTrimChanged(this, new LowBatTrimEventArg(value));
		}

		private void OnModulationShapingChanged(byte value)
		{
			if (ModulationShapingChanged != null)
				ModulationShapingChanged(this, new ByteEventArg(value));
		}

		private void OnModulationTypeChanged(ModulationTypeEnum value)
		{
			if (ModulationTypeChanged != null)
				ModulationTypeChanged(this, new ModulationTypeEventArg(value));
		}

		private void OnRcCalibrationChanged()
		{
			if (RcCalibrationChanged != null)
				RcCalibrationChanged(this, EventArgs.Empty);
		}

		private void OnSequencerChanged(bool value)
		{
			if (SequencerChanged != null)
				SequencerChanged(this, new BooleanEventArg(value));
		}

		private void rBtnDataMode_CheckedChanged(object sender, EventArgs e)
		{
			if (rBtnPacketHandler.Checked)
				DataMode = DataModeEnum.Packet;
			else if (rBtnContinousBitSyncOn.Checked)
				DataMode = DataModeEnum.ContinuousBitSync;
			else if (rBtnContinousBitSyncOff.Checked)
				DataMode = DataModeEnum.Continuous;
			else
				DataMode = DataModeEnum.Reserved;
			OnDataModeChanged(DataMode);
		}

		private void rBtnListenCriteria_CheckedChanged(object sender, EventArgs e)
		{
			ListenCriteria = rBtnListenCriteria0.Checked ? ListenCriteriaEnum.RssiThresh : ListenCriteriaEnum.RssiThreshSyncAddress;
			OnListenCriteriaChanged(ListenCriteria);
		}

		private void rBtnListenMode_CheckedChanged(object sender, EventArgs e)
		{
			ListenMode = rBtnListenModeOn.Checked;
			OnListenModeChanged(ListenMode);
		}

		private void rBtnLowBatOn_CheckedChanged(object sender, EventArgs e)
		{
			LowBatOn = rBtnLowBatOn.Checked;
			OnLowBatOnChanged(LowBatOn);
		}

		private void rBtnModulationShaping_CheckedChanged(object sender, EventArgs e)
		{
			if (rBtnModulationShapingOff.Checked)
				ModulationShaping = 0;
			else if (rBtnModulationShaping01.Checked)
				ModulationShaping = 1;
			else if (rBtnModulationShaping10.Checked)
				ModulationShaping = 2;
			else if (rBtnModulationShaping11.Checked)
				ModulationShaping = 3;
			OnModulationShapingChanged(ModulationShaping);
		}

		private void rBtnModulationType_CheckedChanged(object sender, EventArgs e)
		{
			if (rBtnModulationTypeFsk.Checked)
				ModulationType = ModulationTypeEnum.FSK;
			else if (rBtnModulationTypeOok.Checked)
				ModulationType = ModulationTypeEnum.OOK;
			else
				ModulationType = ModulationTypeEnum.Reserved;
			OnModulationTypeChanged(ModulationType);
		}

		private void rBtnSequencer_CheckedChanged(object sender, EventArgs e)
		{
			Sequencer = rBtnSequencerOn.Checked;
			OnSequencerChanged(Sequencer);
		}

		public void UpdateBitRateLimits(LimitCheckStatusEnum status, string message)
		{
			switch (status)
			{
				case LimitCheckStatusEnum.OK:
					nudBitRate.BackColor = SystemColors.Window;
					break;

				case LimitCheckStatusEnum.OUT_OF_RANGE:
					nudBitRate.BackColor = ControlPaint.LightLight(Color.Orange);
					break;

				case LimitCheckStatusEnum.ERROR:
					nudBitRate.BackColor = ControlPaint.LightLight(Color.Red);
					break;
			}
			errorProvider.SetError(nudBitRate, message);
		}

		public void UpdateFdevLimits(LimitCheckStatusEnum status, string message)
		{
			switch (status)
			{
				case LimitCheckStatusEnum.OK:
					nudFdev.BackColor = SystemColors.Window;
					break;

				case LimitCheckStatusEnum.OUT_OF_RANGE:
					nudFdev.BackColor = ControlPaint.LightLight(Color.Orange);
					break;

				case LimitCheckStatusEnum.ERROR:
					nudFdev.BackColor = ControlPaint.LightLight(Color.Red);
					break;
			}
			errorProvider.SetError(nudFdev, message);
		}

		public void UpdateFrequencyRfLimits(LimitCheckStatusEnum status, string message)
		{
			switch (status)
			{
				case LimitCheckStatusEnum.OK:
					nudFrequencyRf.BackColor = SystemColors.Window;
					break;

				case LimitCheckStatusEnum.OUT_OF_RANGE:
					nudFrequencyRf.BackColor = ControlPaint.LightLight(Color.Orange);
					break;

				case LimitCheckStatusEnum.ERROR:
					nudFrequencyRf.BackColor = ControlPaint.LightLight(Color.Red);
					break;
			}
			errorProvider.SetError(nudFrequencyRf, message);
		}

		public decimal BitRate
		{
			get
			{
				return bitRate;
			}
			set
			{
				try
				{
					nudBitRate.ValueChanged -= new EventHandler(nudBitRate_ValueChanged);
					ushort num = (ushort)Math.Round((decimal)(FrequencyXo / value), MidpointRounding.AwayFromZero);
					bitRate = Math.Round((decimal)(FrequencyXo / num), MidpointRounding.AwayFromZero);
					nudBitRate.Value = bitRate;
				}
				catch (Exception)
				{
					nudBitRate.BackColor = ControlPaint.LightLight(Color.Red);
				}
				finally
				{
					nudBitRate.ValueChanged += new EventHandler(nudBitRate_ValueChanged);
				}
			}
		}

		public DataModeEnum DataMode
		{
			get
			{
				if (rBtnPacketHandler.Checked)
					return DataModeEnum.Packet;
				if (rBtnContinousBitSyncOn.Checked)
					return DataModeEnum.ContinuousBitSync;
				if (rBtnContinousBitSyncOff.Checked)
					return DataModeEnum.Continuous;
				return DataModeEnum.Reserved;
			}
			set
			{
				rBtnPacketHandler.CheckedChanged -= new EventHandler(rBtnDataMode_CheckedChanged);
				rBtnContinousBitSyncOn.CheckedChanged -= new EventHandler(rBtnDataMode_CheckedChanged);
				rBtnContinousBitSyncOff.CheckedChanged -= new EventHandler(rBtnDataMode_CheckedChanged);
				switch (value)
				{
					case DataModeEnum.Packet:
						rBtnPacketHandler.Checked = true;
						rBtnContinousBitSyncOn.Checked = false;
						rBtnContinousBitSyncOff.Checked = false;
						break;

					case DataModeEnum.ContinuousBitSync:
						rBtnPacketHandler.Checked = false;
						rBtnContinousBitSyncOn.Checked = true;
						rBtnContinousBitSyncOff.Checked = false;
						break;

					case DataModeEnum.Continuous:
						rBtnPacketHandler.Checked = false;
						rBtnContinousBitSyncOn.Checked = false;
						rBtnContinousBitSyncOff.Checked = true;
						break;
				}
				rBtnPacketHandler.CheckedChanged += new EventHandler(rBtnDataMode_CheckedChanged);
				rBtnContinousBitSyncOn.CheckedChanged += new EventHandler(rBtnDataMode_CheckedChanged);
				rBtnContinousBitSyncOff.CheckedChanged += new EventHandler(rBtnDataMode_CheckedChanged);
			}
		}

		public decimal Fdev
		{
			get
			{
				return nudFdev.Value;
			}
			set
			{
				try
				{
					nudFdev.ValueChanged -= new EventHandler(nudFdev_ValueChanged);
					ushort num = (ushort)Math.Round((decimal)(value / FrequencyStep), MidpointRounding.AwayFromZero);
					nudFdev.Value = num * FrequencyStep;
				}
				catch (Exception)
				{
					nudFdev.BackColor = ControlPaint.LightLight(Color.Red);
				}
				finally
				{
					nudFdev.ValueChanged += new EventHandler(nudFdev_ValueChanged);
				}
			}
		}

		public decimal FrequencyRf
		{
			get
			{
				return nudFrequencyRf.Value;
			}
			set
			{
				try
				{
					nudFrequencyRf.ValueChanged -= new EventHandler(nudFrequencyRf_ValueChanged);
					uint num = (uint)Math.Round((value / FrequencyStep), MidpointRounding.AwayFromZero);
					nudFrequencyRf.Value = num * FrequencyStep;
				}
				catch (Exception)
				{
					nudFrequencyRf.BackColor = ControlPaint.LightLight(Color.Red);
				}
				finally
				{
					nudFrequencyRf.ValueChanged += new EventHandler(nudFrequencyRf_ValueChanged);
				}
			}
		}

		public decimal FrequencyStep
		{
			get
			{
				return nudFrequencyRf.Increment;
			}
			set
			{
				nudFrequencyRf.Increment = value;
				nudFdev.Increment = value;
			}
		}

		public decimal FrequencyXo
		{
			get
			{
				return nudFrequencyXo.Value;
			}
			set
			{
				nudFrequencyXo.ValueChanged -= new EventHandler(nudFrequencyXo_ValueChanged);
				nudFrequencyXo.Value = value;
				nudFrequencyXo.ValueChanged += new EventHandler(nudFrequencyXo_ValueChanged);
			}
		}

		public decimal ListenCoefIdle
		{
			get
			{
				return nudListenCoefIdle.Value;
			}
			set
			{
				try
				{
					nudListenCoefIdle.ValueChanged -= new EventHandler(nudListenCoefIdle_ValueChanged);
					nudListenCoefIdle.Value = value;
				}
				catch { }
				finally
				{
					nudListenCoefIdle.ValueChanged += new EventHandler(nudListenCoefIdle_ValueChanged);
				}
			}
		}

		public decimal ListenCoefRx
		{
			get
			{
				return nudListenCoefRx.Value;
			}
			set
			{
				try
				{
					nudListenCoefRx.ValueChanged -= new EventHandler(nudListenCoefRx_ValueChanged);
					nudListenCoefRx.Value = value;
				}
				catch { }
				finally
				{
					nudListenCoefRx.ValueChanged += new EventHandler(nudListenCoefRx_ValueChanged);
				}
			}
		}

		public ListenCriteriaEnum ListenCriteria
		{
			get
			{
				if (rBtnListenCriteria0.Checked)
					return ListenCriteriaEnum.RssiThresh;
				return ListenCriteriaEnum.RssiThreshSyncAddress;
			}
			set
			{
				rBtnListenCriteria0.CheckedChanged -= new EventHandler(rBtnListenCriteria_CheckedChanged);
				rBtnListenCriteria1.CheckedChanged -= new EventHandler(rBtnListenCriteria_CheckedChanged);
				switch (value)
				{
					case ListenCriteriaEnum.RssiThresh:
						rBtnListenCriteria0.Checked = true;
						rBtnListenCriteria1.Checked = false;
						break;

					case ListenCriteriaEnum.RssiThreshSyncAddress:
						rBtnListenCriteria0.Checked = false;
						rBtnListenCriteria1.Checked = true;
						break;
				}
				rBtnListenCriteria0.CheckedChanged += new EventHandler(rBtnListenCriteria_CheckedChanged);
				rBtnListenCriteria1.CheckedChanged += new EventHandler(rBtnListenCriteria_CheckedChanged);
			}
		}

		public ListenEndEnum ListenEnd
		{
			get
			{
				return (ListenEndEnum)cBoxListenEnd.SelectedIndex;
			}
			set
			{
				try
				{
					cBoxListenEnd.SelectedIndexChanged -= new EventHandler(cBoxListenEnd_SelectedIndexChanged);
					if (value == ListenEndEnum.Reserved)
						cBoxListenEnd.SelectedIndex = -1;
					else
						cBoxListenEnd.SelectedIndex = (int)value;
				}
				catch (Exception) { }
				finally
				{
					cBoxListenEnd.SelectedIndexChanged += new EventHandler(cBoxListenEnd_SelectedIndexChanged);
				}
			}
		}

		public bool ListenMode
		{
			get
			{
				return rBtnListenModeOn.Checked;
			}
			set
			{
				rBtnListenModeOn.CheckedChanged -= new EventHandler(rBtnListenMode_CheckedChanged);
				rBtnListenModeOff.CheckedChanged -= new EventHandler(rBtnListenMode_CheckedChanged);
				if (value)
				{
					btnListenModeAbort.Enabled = true;
					rBtnListenModeOn.Checked = true;
					rBtnListenModeOff.Checked = false;
				}
				else
				{
					btnListenModeAbort.Enabled = false;
					rBtnListenModeOn.Checked = false;
					rBtnListenModeOff.Checked = true;
				}
				rBtnListenModeOn.CheckedChanged += new EventHandler(rBtnListenMode_CheckedChanged);
				rBtnListenModeOff.CheckedChanged += new EventHandler(rBtnListenMode_CheckedChanged);
			}
		}

		public ListenResolEnum ListenResolIdle
		{
			get
			{
				return (ListenResolEnum)cBoxListenResolIdle.SelectedIndex;
			}
			set
			{
				cBoxListenResolIdle.SelectedIndexChanged -= new EventHandler(cBoxListenResolIdle_SelectedIndexChanged);
				cBoxListenResolIdle.SelectedIndex = (int)value;
				switch (value)
				{
					case ListenResolEnum.Res000064:
						nudListenCoefIdle.ValueChanged -= new EventHandler(nudListenCoefIdle_ValueChanged);
						nudListenCoefIdle.Maximum = 16.320M;
						nudListenCoefIdle.Increment = 0.064M;
						nudListenCoefIdle.ValueChanged += new EventHandler(nudListenCoefIdle_ValueChanged);
						break;

					case ListenResolEnum.Res004100:
						nudListenCoefIdle.ValueChanged -= new EventHandler(nudListenCoefIdle_ValueChanged);
						nudListenCoefIdle.Maximum = 1045.5M;
						nudListenCoefIdle.Increment = 4.1M;
						nudListenCoefIdle.ValueChanged += new EventHandler(nudListenCoefIdle_ValueChanged);
						break;

					case ListenResolEnum.Res262000:
						nudListenCoefIdle.ValueChanged -= new EventHandler(nudListenCoefIdle_ValueChanged);
						nudListenCoefIdle.Maximum = 66810M;
						nudListenCoefIdle.Increment = 262M;
						nudListenCoefIdle.ValueChanged += new EventHandler(nudListenCoefIdle_ValueChanged);
						break;
				}
				cBoxListenResolIdle.SelectedIndexChanged += new EventHandler(cBoxListenResolIdle_SelectedIndexChanged);
			}
		}

		public ListenResolEnum ListenResolRx
		{
			get
			{
				return (ListenResolEnum)cBoxListenResolRx.SelectedIndex;
			}
			set
			{
				try
				{
					cBoxListenResolRx.SelectedIndexChanged -= new EventHandler(cBoxListenResolRx_SelectedIndexChanged);
					cBoxListenResolRx.SelectedIndex = (int)value;
					switch (value)
					{
						case ListenResolEnum.Res000064:
							nudListenCoefRx.ValueChanged -= new EventHandler(nudListenCoefRx_ValueChanged);
							nudListenCoefRx.Maximum = 16.320M;
							nudListenCoefRx.Increment = 0.064M;
							nudListenCoefRx.ValueChanged += new EventHandler(nudListenCoefRx_ValueChanged);
							return;

						case ListenResolEnum.Res004100:
							nudListenCoefRx.ValueChanged -= new EventHandler(nudListenCoefRx_ValueChanged);
							nudListenCoefRx.Maximum = 1045.5M;
							nudListenCoefRx.Increment = 4.1M;
							nudListenCoefRx.ValueChanged += new EventHandler(nudListenCoefRx_ValueChanged);
							return;

						case ListenResolEnum.Res262000:
							nudListenCoefRx.ValueChanged -= new EventHandler(nudListenCoefRx_ValueChanged);
							nudListenCoefRx.Maximum = 66810M;
							nudListenCoefRx.Increment = 262M;
							nudListenCoefRx.ValueChanged += new EventHandler(nudListenCoefRx_ValueChanged);
							return;
					}
				}
				catch (Exception)
				{
				}
				finally
				{
					cBoxListenResolRx.SelectedIndexChanged += new EventHandler(cBoxListenResolRx_SelectedIndexChanged);
				}
			}
		}

		public bool LowBatMonitor
		{
			get { return ledLowBatMonitor.Checked; }
			set { ledLowBatMonitor.Checked = value; }
		}

		public bool LowBatOn
		{
			get
			{
				return rBtnLowBatOn.Checked;
			}
			set
			{
				rBtnLowBatOn.CheckedChanged -= new EventHandler(rBtnLowBatOn_CheckedChanged);
				rBtnLowBatOff.CheckedChanged -= new EventHandler(rBtnLowBatOn_CheckedChanged);
				if (value)
				{
					rBtnLowBatOn.Checked = true;
					rBtnLowBatOff.Checked = false;
				}
				else
				{
					rBtnLowBatOn.Checked = false;
					rBtnLowBatOff.Checked = true;
				}
				rBtnLowBatOn.CheckedChanged += new EventHandler(rBtnLowBatOn_CheckedChanged);
				rBtnLowBatOff.CheckedChanged += new EventHandler(rBtnLowBatOn_CheckedChanged);
			}
		}

		public LowBatTrimEnum LowBatTrim
		{
			get
			{
				return (LowBatTrimEnum)cBoxLowBatTrim.SelectedIndex;
			}
			set
			{
				cBoxLowBatTrim.SelectedIndexChanged -= new EventHandler(cBoxLowBatTrim_SelectedIndexChanged);
				cBoxLowBatTrim.SelectedIndex = (int)value;
				cBoxLowBatTrim.SelectedIndexChanged += new EventHandler(cBoxLowBatTrim_SelectedIndexChanged);
			}
		}

		public byte ModulationShaping
		{
			get
			{
				if (rBtnModulationShapingOff.Checked)
					return 0;
				if (rBtnModulationShaping01.Checked)
					return 1;
				if (rBtnModulationShaping10.Checked)
					return 2;
				return 3;
			}
			set
			{
				rBtnModulationShapingOff.CheckedChanged -= new EventHandler(rBtnModulationShaping_CheckedChanged);
				rBtnModulationShaping01.CheckedChanged -= new EventHandler(rBtnModulationShaping_CheckedChanged);
				rBtnModulationShaping10.CheckedChanged -= new EventHandler(rBtnModulationShaping_CheckedChanged);
				rBtnModulationShaping11.CheckedChanged -= new EventHandler(rBtnModulationShaping_CheckedChanged);
				switch (value)
				{
					case 0:
						rBtnModulationShapingOff.Checked = true;
						rBtnModulationShaping01.Checked = false;
						rBtnModulationShaping10.Checked = false;
						rBtnModulationShaping11.Checked = false;
						break;

					case 1:
						rBtnModulationShapingOff.Checked = false;
						rBtnModulationShaping01.Checked = true;
						rBtnModulationShaping10.Checked = false;
						rBtnModulationShaping11.Checked = false;
						break;

					case 2:
						rBtnModulationShapingOff.Checked = false;
						rBtnModulationShaping01.Checked = false;
						rBtnModulationShaping10.Checked = true;
						rBtnModulationShaping11.Checked = false;
						break;

					case 3:
						rBtnModulationShapingOff.Checked = false;
						rBtnModulationShaping01.Checked = false;
						rBtnModulationShaping10.Checked = false;
						rBtnModulationShaping11.Checked = true;
						break;
				}
				rBtnModulationShapingOff.CheckedChanged += new EventHandler(rBtnModulationShaping_CheckedChanged);
				rBtnModulationShaping01.CheckedChanged += new EventHandler(rBtnModulationShaping_CheckedChanged);
				rBtnModulationShaping10.CheckedChanged += new EventHandler(rBtnModulationShaping_CheckedChanged);
				rBtnModulationShaping11.CheckedChanged += new EventHandler(rBtnModulationShaping_CheckedChanged);
			}
		}

		public ModulationTypeEnum ModulationType
		{
			get
			{
				if (!rBtnModulationTypeFsk.Checked && rBtnModulationTypeOok.Checked)
					return ModulationTypeEnum.OOK;
				return ModulationTypeEnum.FSK;
			}
			set
			{
				rBtnModulationTypeFsk.CheckedChanged -= new EventHandler(rBtnModulationType_CheckedChanged);
				rBtnModulationTypeOok.CheckedChanged -= new EventHandler(rBtnModulationType_CheckedChanged);
				switch (value)
				{
					case ModulationTypeEnum.FSK:
						rBtnModulationTypeFsk.Checked = true;
						rBtnModulationTypeOok.Checked = false;
						rBtnModulationShapingOff.Text = "OFF";
						rBtnModulationShaping01.Text = "Gaussian filter, BT = 1.0";
						rBtnModulationShaping10.Text = "Gaussian filter, BT = 0.5";
						rBtnModulationShaping11.Text = "Gaussian filter, BT = 0.3";
						rBtnModulationShaping11.Visible = true;
						break;

					case ModulationTypeEnum.OOK:
						rBtnModulationTypeFsk.Checked = false;
						rBtnModulationTypeOok.Checked = true;
						rBtnModulationShapingOff.Text = "OFF";
						rBtnModulationShaping01.Text = "Filtering with fCutOff = BR";
						rBtnModulationShaping10.Text = "Filtering with fCutOff = 2 * BR";
						rBtnModulationShaping11.Text = "Unused";
						rBtnModulationShaping11.Visible = false;
						break;
				}
				rBtnModulationTypeFsk.CheckedChanged += new EventHandler(rBtnModulationType_CheckedChanged);
				rBtnModulationTypeOok.CheckedChanged += new EventHandler(rBtnModulationType_CheckedChanged);
			}
		}

		public bool RcCalDone
		{
			get { return ledRcCalibration.Checked; }
			set { ledRcCalibration.Checked = value; }
		}

		public bool Sequencer
		{
			get
			{
				return rBtnSequencerOn.Checked;
			}
			set
			{
				rBtnSequencerOn.CheckedChanged -= new EventHandler(rBtnSequencer_CheckedChanged);
				rBtnSequencerOff.CheckedChanged -= new EventHandler(rBtnSequencer_CheckedChanged);
				if (value)
				{
					rBtnSequencerOn.Checked = true;
					rBtnSequencerOff.Checked = false;
				}
				else
				{
					rBtnSequencerOn.Checked = false;
					rBtnSequencerOff.Checked = true;
				}
				rBtnSequencerOn.CheckedChanged += new EventHandler(rBtnSequencer_CheckedChanged);
				rBtnSequencerOff.CheckedChanged += new EventHandler(rBtnSequencer_CheckedChanged);
			}
		}

		public string Version
		{
			get { return version; }
			set
			{
				version = value;
				if (value.ToString() == "2.1")
					cBoxListenResolRx.Enabled = false;
				else
					cBoxListenResolRx.Enabled = true;
			}
		}
	}
}