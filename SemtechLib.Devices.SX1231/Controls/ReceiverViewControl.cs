using SemtechLib.Controls;
using SemtechLib.Devices.SX1231;
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
	public class ReceiverViewControl : UserControl, INotifyDocumentationChanged
	{
		private decimal afcDccFreq = 497M;
		private bool afcDone;
		private decimal afcRxBw = 50000M;
		private decimal afcValue = 0.0M;
		private int agcReference = -80;
		private int agcThresh1;
		private int agcThresh2;
		private int agcThresh3;
		private int agcThresh4;
		private int agcThresh5;
		private decimal bitRate = 4800M;
		private Button btnAfcClear;
		private Button btnAfcStart;
		private Button btnFeiRead;
		private Button btnRestartRx;
		private Button btnRssiRead;
		private ComboBox cBoxOokAverageThreshFilt;
		private ComboBox cBoxOokPeakThreshDec;
		private ComboBox cBoxOokThreshType;
		private IContainer components;
		private DataModeEnum dataMode;
		private decimal dccFreq = 414M;
		private ErrorProvider errorProvider;
		private bool feiDone;
		private decimal feiValue = 0.0M;
		private decimal frequencyXo = 32000000M;
		private GroupBoxEx gBoxAfcBw;
		private GroupBoxEx gBoxAfcFei;
		private GroupBoxEx gBoxAgc;
		private GroupBoxEx gBoxDagc;
		private GroupBoxEx gBoxLna;
		private GroupBoxEx gBoxLnaSensitivity;
		private GroupBoxEx gBoxOok;
		private GroupBoxEx gBoxRssi;
		private GroupBoxEx gBoxRxBw;
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
		private Label label2;
		private Label label20;
		private Label label21;
		private Label label22;
		private Label label23;
		private Label label24;
		private Label label25;
		private Label label26;
		private Label label27;
		private Label label28;
		private Label label29;
		private Label label3;
		private Label label30;
		private Label label31;
		private Label label32;
		private Label label33;
		private Label label34;
		private Label label4;
		private Label label47;
		private Label label48;
		private Label label49;
		private Label label5;
		private Label label50;
		private Label label51;
		private Label label52;
		private Label label53;
		private Label label54;
		private Label label55;
		private Label label56;
		private Label label6;
		private Label label7;
		private Label label8;
		private Label label9;
		private Label lblAfcDcc;
		private Label lblAfcLowBeta;
		private Label lblAfcRxBw;
		private Label lblAfcValue;
		private Label lblAGC;
		private Label lblAgcReference;
		private Label lblAgcThresh1;
		private Label lblAgcThresh2;
		private Label lblAgcThresh3;
		private Label lblAgcThresh4;
		private Label lblAgcThresh5;
		private Label lblDcc;
		private Label lblFeiValue;
		private Label lblLnaGain1;
		private Label lblLnaGain2;
		private Label lblLnaGain3;
		private Label lblLnaGain4;
		private Label lblLnaGain5;
		private Label lblLnaGain6;
		private Label lblLowBetaAfcOffset;
		private Label lblLowBetaAfcOfssetUnit;
		private Label lblOokCutoff;
		private Label lblOokDec;
		private Label lblOokFixed;
		private Label lblOokStep;
		private Label lblOokType;
		private Label lblRssiValue;
		private Label lblRxBw;
		private Label lblSensitivityBoost;
		private Led ledAfcDone;
		private Led ledFeiDone;
		private Led ledRssiDone;
		private decimal lowBetaAfcOffset;
		private ModulationTypeEnum modulationType;
		private NumericUpDownEx nudAfcDccFreq;
		private NumericUpDownEx nudAgcRefLevel;
		private NumericUpDownEx nudAgcSnrMargin;
		private NumericUpDown nudAgcStep1;
		private NumericUpDown nudAgcStep2;
		private NumericUpDown nudAgcStep3;
		private NumericUpDown nudAgcStep4;
		private NumericUpDown nudAgcStep5;
		private NumericUpDownEx nudDccFreq;
		private NumericUpDownEx nudLowBetaAfcOffset;
		private NumericUpDownEx nudOokFixedThresh;
		private NumericUpDownEx nudOokPeakThreshStep;
		private NumericUpDownEx nudRssiThresh;
		private NumericUpDownEx nudRxFilterBw;
		private NumericUpDownEx nudRxFilterBwAfc;
		private NumericUpDownEx nudTimeoutRssiThresh;
		private NumericUpDownEx nudTimeoutRxStart;
		private decimal ookPeakThreshStep = 0.5M;
		private Panel panel1;
		private Panel panel11;
		private Panel panel2;
		private Panel panel3;
		private Panel panel4;
		private Panel panel5;
		private Panel panel6;
		private Panel panel7;
		private Panel panel8;
		private Panel panel9;
		private Panel pnlAfcLowBeta;
		private Panel pnlRssiPhase;
		private Panel pnlSensitivityBoost;
		private RadioButton rBtnAfcAutoClearOff;
		private RadioButton rBtnAfcAutoClearOn;
		private RadioButton rBtnAfcAutoOff;
		private RadioButton rBtnAfcAutoOn;
		private RadioButton rBtnAfcLowBetaOff;
		private RadioButton rBtnAfcLowBetaOn;
		private RadioButton rBtnAgcAutoRefOff;
		private RadioButton rBtnAgcAutoRefOn;
		private RadioButton rBtnDagcOff;
		private RadioButton rBtnDagcOn;
		private RadioButton rBtnFastRxOff;
		private RadioButton rBtnFastRxOn;
		private RadioButton rBtnLnaGain1;
		private RadioButton rBtnLnaGain2;
		private RadioButton rBtnLnaGain3;
		private RadioButton rBtnLnaGain4;
		private RadioButton rBtnLnaGain5;
		private RadioButton rBtnLnaGain6;
		private RadioButton rBtnLnaGainAutoOff;
		private RadioButton rBtnLnaGainAutoOn;
		private RadioButton rBtnLnaLowPowerOff;
		private RadioButton rBtnLnaLowPowerOn;
		private RadioButton rBtnLnaZin200;
		private RadioButton rBtnLnaZin50;
		private RadioButton rBtnRssiAutoThreshOff;
		private RadioButton rBtnRssiAutoThreshOn;
		private RadioButton rBtnRssiPhaseAuto;
		private RadioButton rBtnRssiPhaseManual;
		private RadioButton rBtnSensitivityBoostOff;
		private RadioButton rBtnSensitivityBoostOn;
		private bool rssiAutoThresh = true;
		private bool rssiDone;
		private decimal rssiValue = -127.5M;
		private decimal rxBw = 10417M;
		private Label suffixAFCDCC;
		private Label suffixAFCRxBw;
		private Label suffixDCC;
		private Label suffixOOKfixed;
		private Label suffixOOKstep;
		private Label suffixRxBw;
		private string version = "2.3";

		public event BooleanEventHandler AfcAutoClearOnChanged;
		public event BooleanEventHandler AfcAutoOnChanged;
		public event EventHandler AfcClearChanged;
		public event DecimalEventHandler AfcDccFreqChanged;
		public event BooleanEventHandler AfcLowBetaOnChanged;
		public event DecimalEventHandler AfcRxBwChanged;
		public event EventHandler AfcStartChanged;
		public event BooleanEventHandler AgcAutoRefChanged;
		public event Int32EventHandler AgcRefLevelChanged;
		public event ByteEventHandler AgcSnrMarginChanged;
		public event AgcStepEventHandler AgcStepChanged;
		public event BooleanEventHandler AutoRxRestartOnChanged;
		public event BooleanEventHandler DagcOnChanged;
		public event DecimalEventHandler DccFreqChanged;
		public event DocumentationChangedEventHandler DocumentationChanged;
		public event BooleanEventHandler FastRxChanged;
		public event EventHandler FeiStartChanged;
		public event LnaGainEventHandler LnaGainChanged;
		public event BooleanEventHandler LnaLowPowerOnChanged;
		public event LnaZinEventHandler LnaZinChanged;
		public event DecimalEventHandler LowBetaAfcOffsetChanged;
		public event OokAverageThreshFiltEventHandler OokAverageThreshFiltChanged;
		public event ByteEventHandler OokFixedThreshChanged;
		public event OokPeakThreshDecEventHandler OokPeakThreshDecChanged;
		public event DecimalEventHandler OokPeakThreshStepChanged;
		public event OokThreshTypeEventHandler OokThreshTypeChanged;
		public event EventHandler RestartRxChanged;
		public event BooleanEventHandler RssiAutoThreshChanged;
		public event EventHandler RssiStartChanged;
		public event DecimalEventHandler RssiThreshChanged;
		public event DecimalEventHandler RxBwChanged;
		public event BooleanEventHandler SensitivityBoostOnChanged;
		public event DecimalEventHandler TimeoutRssiThreshChanged;
		public event DecimalEventHandler TimeoutRxStartChanged;
		public ReceiverViewControl()
		{
			InitializeComponent();
		}

		private void btnAfcClear_Click(object sender, EventArgs e)
		{
			OnAfcClearChanged();
		}

		private void btnAfcStart_Click(object sender, EventArgs e)
		{
			OnAfcStartChanged();
		}

		private void btnFeiStart_Click(object sender, EventArgs e)
		{
			OnFeiStartChanged();
		}

		private void btnRestartRx_Click(object sender, EventArgs e)
		{
			OnRestartRxChanged();
		}

		private void btnRssiStart_Click(object sender, EventArgs e)
		{
			OnRssiStartChanged();
		}

		private void cBoxOokAverageThreshFilt_SelectedIndexChanged(object sender, EventArgs e)
		{
			OokAverageThreshFilt = (OokAverageThreshFiltEnum)cBoxOokAverageThreshFilt.SelectedIndex;
			OnOokAverageThreshFiltChanged(OokAverageThreshFilt);
		}

		private void cBoxOokPeakThreshDec_SelectedIndexChanged(object sender, EventArgs e)
		{
			OokPeakThreshDec = (OokPeakThreshDecEnum)cBoxOokPeakThreshDec.SelectedIndex;
			OnOokPeakThreshDecChanged(OokPeakThreshDec);
		}

		private void cBoxOokThreshType_SelectedIndexChanged(object sender, EventArgs e)
		{
			OokThreshType = (OokThreshTypeEnum)cBoxOokThreshType.SelectedIndex;
			OnOokThreshTypeChanged(OokThreshType);
		}

		private void control_MouseEnter(object sender, EventArgs e)
		{
			if (sender == gBoxRxBw)
				OnDocumentationChanged(new DocumentationChangedEventArgs("Receiver", "Rx bandwidth"));
			else if (sender == gBoxAfcBw)
				OnDocumentationChanged(new DocumentationChangedEventArgs("Receiver", "Afc bandwidth"));
			else if (sender == gBoxOok)
				OnDocumentationChanged(new DocumentationChangedEventArgs("Receiver", "Ook"));
			else if (sender == gBoxAfcFei)
				OnDocumentationChanged(new DocumentationChangedEventArgs("Receiver", "Afc Fei"));
			else if (sender == gBoxRssi)
				OnDocumentationChanged(new DocumentationChangedEventArgs("Receiver", "Rssi"));
			else if (sender == gBoxLna)
				OnDocumentationChanged(new DocumentationChangedEventArgs("Receiver", "Lna"));
			else if (sender == gBoxLnaSensitivity)
				OnDocumentationChanged(new DocumentationChangedEventArgs("Receiver", "Lna sensitivity"));
			else if (sender == gBoxAgc)
				OnDocumentationChanged(new DocumentationChangedEventArgs("Receiver", "Agc"));
			else if (sender == gBoxDagc)
				OnDocumentationChanged(new DocumentationChangedEventArgs("Receiver", "Dagc"));
		}

		private void control_MouseLeave(object sender, EventArgs e)
		{
			OnDocumentationChanged(new DocumentationChangedEventArgs(".", "Overview"));
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
				components.Dispose();
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			components = new Container();
			errorProvider = new ErrorProvider(components);
			label4 = new Label();
			lblAGC = new Label();
			panel3 = new Panel();
			rBtnLnaZin200 = new RadioButton();
			rBtnLnaZin50 = new RadioButton();
			panel4 = new Panel();
			rBtnLnaLowPowerOff = new RadioButton();
			rBtnLnaLowPowerOn = new RadioButton();
			label7 = new Label();
			lblSensitivityBoost = new Label();
			pnlSensitivityBoost = new Panel();
			rBtnSensitivityBoostOff = new RadioButton();
			rBtnSensitivityBoostOn = new RadioButton();
			gBoxLnaSensitivity = new GroupBoxEx();
			gBoxAgc = new GroupBoxEx();
			panel2 = new Panel();
			rBtnAgcAutoRefOff = new RadioButton();
			rBtnAgcAutoRefOn = new RadioButton();
			label5 = new Label();
			label8 = new Label();
			label24 = new Label();
			label25 = new Label();
			label26 = new Label();
			label27 = new Label();
			label28 = new Label();
			label1 = new Label();
			label2 = new Label();
			label3 = new Label();
			label29 = new Label();
			label30 = new Label();
			label31 = new Label();
			label32 = new Label();
			label33 = new Label();
			nudAgcStep5 = new NumericUpDown();
			nudAgcSnrMargin = new NumericUpDownEx();
			nudAgcStep4 = new NumericUpDown();
			nudAgcRefLevel = new NumericUpDownEx();
			nudAgcStep3 = new NumericUpDown();
			nudAgcStep1 = new NumericUpDown();
			nudAgcStep2 = new NumericUpDown();
			gBoxRssi = new GroupBoxEx();
			pnlRssiPhase = new Panel();
			rBtnRssiPhaseManual = new RadioButton();
			rBtnRssiPhaseAuto = new RadioButton();
			label23 = new Label();
			btnRestartRx = new Button();
			panel7 = new Panel();
			rBtnFastRxOff = new RadioButton();
			rBtnFastRxOn = new RadioButton();
			label21 = new Label();
			btnRssiRead = new Button();
			label17 = new Label();
			label54 = new Label();
			label55 = new Label();
			label56 = new Label();
			lblRssiValue = new Label();
			nudRssiThresh = new NumericUpDownEx();
			ledRssiDone = new Led();
			panel1 = new Panel();
			rBtnRssiAutoThreshOff = new RadioButton();
			rBtnRssiAutoThreshOn = new RadioButton();
			label6 = new Label();
			nudTimeoutRxStart = new NumericUpDownEx();
			label9 = new Label();
			label14 = new Label();
			label11 = new Label();
			label15 = new Label();
			nudTimeoutRssiThresh = new NumericUpDownEx();
			gBoxAfcFei = new GroupBoxEx();
			nudLowBetaAfcOffset = new NumericUpDownEx();
			lblLowBetaAfcOffset = new Label();
			lblAfcLowBeta = new Label();
			label19 = new Label();
			lblLowBetaAfcOfssetUnit = new Label();
			label20 = new Label();
			pnlAfcLowBeta = new Panel();
			rBtnAfcLowBetaOff = new RadioButton();
			rBtnAfcLowBetaOn = new RadioButton();
			btnFeiRead = new Button();
			panel8 = new Panel();
			rBtnAfcAutoClearOff = new RadioButton();
			rBtnAfcAutoClearOn = new RadioButton();
			ledFeiDone = new Led();
			panel9 = new Panel();
			rBtnAfcAutoOff = new RadioButton();
			rBtnAfcAutoOn = new RadioButton();
			lblFeiValue = new Label();
			label12 = new Label();
			label18 = new Label();
			label10 = new Label();
			btnAfcClear = new Button();
			btnAfcStart = new Button();
			ledAfcDone = new Led();
			lblAfcValue = new Label();
			label22 = new Label();
			gBoxOok = new GroupBoxEx();
			cBoxOokThreshType = new ComboBox();
			lblOokType = new Label();
			lblOokStep = new Label();
			lblOokDec = new Label();
			lblOokCutoff = new Label();
			lblOokFixed = new Label();
			suffixOOKstep = new Label();
			suffixOOKfixed = new Label();
			nudOokPeakThreshStep = new NumericUpDownEx();
			nudOokFixedThresh = new NumericUpDownEx();
			cBoxOokPeakThreshDec = new ComboBox();
			cBoxOokAverageThreshFilt = new ComboBox();
			gBoxAfcBw = new GroupBoxEx();
			nudAfcDccFreq = new NumericUpDownEx();
			lblAfcDcc = new Label();
			lblAfcRxBw = new Label();
			suffixAFCDCC = new Label();
			suffixAFCRxBw = new Label();
			nudRxFilterBwAfc = new NumericUpDownEx();
			gBoxRxBw = new GroupBoxEx();
			nudDccFreq = new NumericUpDownEx();
			lblDcc = new Label();
			lblRxBw = new Label();
			suffixDCC = new Label();
			suffixRxBw = new Label();
			nudRxFilterBw = new NumericUpDownEx();
			gBoxLna = new GroupBoxEx();
			panel5 = new Panel();
			rBtnLnaGainAutoOff = new RadioButton();
			rBtnLnaGainAutoOn = new RadioButton();
			label13 = new Label();
			label16 = new Label();
			lblAgcReference = new Label();
			label48 = new Label();
			label49 = new Label();
			label50 = new Label();
			label51 = new Label();
			label52 = new Label();
			lblLnaGain1 = new Label();
			label53 = new Label();
			panel6 = new Panel();
			rBtnLnaGain1 = new RadioButton();
			rBtnLnaGain2 = new RadioButton();
			rBtnLnaGain3 = new RadioButton();
			rBtnLnaGain4 = new RadioButton();
			rBtnLnaGain5 = new RadioButton();
			rBtnLnaGain6 = new RadioButton();
			lblLnaGain2 = new Label();
			lblLnaGain3 = new Label();
			lblLnaGain4 = new Label();
			lblLnaGain5 = new Label();
			lblLnaGain6 = new Label();
			lblAgcThresh1 = new Label();
			lblAgcThresh2 = new Label();
			lblAgcThresh3 = new Label();
			lblAgcThresh4 = new Label();
			lblAgcThresh5 = new Label();
			label47 = new Label();
			gBoxDagc = new GroupBoxEx();
			label34 = new Label();
			panel11 = new Panel();
			rBtnDagcOff = new RadioButton();
			rBtnDagcOn = new RadioButton();
			((ISupportInitialize)errorProvider).BeginInit();
			panel3.SuspendLayout();
			panel4.SuspendLayout();
			pnlSensitivityBoost.SuspendLayout();
			gBoxLnaSensitivity.SuspendLayout();
			gBoxAgc.SuspendLayout();
			panel2.SuspendLayout();
			nudAgcStep5.BeginInit();
			nudAgcSnrMargin.BeginInit();
			nudAgcStep4.BeginInit();
			nudAgcRefLevel.BeginInit();
			nudAgcStep3.BeginInit();
			nudAgcStep1.BeginInit();
			nudAgcStep2.BeginInit();
			gBoxRssi.SuspendLayout();
			pnlRssiPhase.SuspendLayout();
			panel7.SuspendLayout();
			nudRssiThresh.BeginInit();
			panel1.SuspendLayout();
			nudTimeoutRxStart.BeginInit();
			nudTimeoutRssiThresh.BeginInit();
			gBoxAfcFei.SuspendLayout();
			nudLowBetaAfcOffset.BeginInit();
			pnlAfcLowBeta.SuspendLayout();
			panel8.SuspendLayout();
			panel9.SuspendLayout();
			gBoxOok.SuspendLayout();
			nudOokPeakThreshStep.BeginInit();
			nudOokFixedThresh.BeginInit();
			gBoxAfcBw.SuspendLayout();
			nudAfcDccFreq.BeginInit();
			nudRxFilterBwAfc.BeginInit();
			gBoxRxBw.SuspendLayout();
			nudDccFreq.BeginInit();
			nudRxFilterBw.BeginInit();
			gBoxLna.SuspendLayout();
			panel5.SuspendLayout();
			panel6.SuspendLayout();
			gBoxDagc.SuspendLayout();
			panel11.SuspendLayout();
			base.SuspendLayout();
			errorProvider.ContainerControl = this;
			label4.AutoSize = true;
			label4.Location = new Point(11, 0x56);
			label4.Name = "label4";
			label4.Size = new Size(0x56, 13);
			label4.TabIndex = 6;
			label4.Text = "Mixer low-power:";
			lblAGC.AutoSize = true;
			lblAGC.Location = new Point(11, 30);
			lblAGC.Name = "lblAGC";
			lblAGC.Size = new Size(0x59, 13);
			lblAGC.TabIndex = 0;
			lblAGC.Text = "Input impedance:";
			panel3.AutoSize = true;
			panel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			panel3.Controls.Add(rBtnLnaZin200);
			panel3.Controls.Add(rBtnLnaZin50);
			panel3.Location = new Point(0x6a, 0x13);
			panel3.Name = "panel3";
			panel3.Size = new Size(0x31, 0x22);
			panel3.TabIndex = 1;
			rBtnLnaZin200.AutoSize = true;
			rBtnLnaZin200.Location = new Point(3, 0x11);
			rBtnLnaZin200.Margin = new Padding(3, 0, 3, 0);
			rBtnLnaZin200.Name = "rBtnLnaZin200";
			rBtnLnaZin200.Size = new Size(0x2b, 0x11);
			rBtnLnaZin200.TabIndex = 1;
			rBtnLnaZin200.Text = "200";
			rBtnLnaZin200.UseVisualStyleBackColor = true;
			rBtnLnaZin200.CheckedChanged += new EventHandler(rBtnLnaZin_CheckedChanged);
			rBtnLnaZin50.AutoSize = true;
			rBtnLnaZin50.Checked = true;
			rBtnLnaZin50.Location = new Point(3, 0);
			rBtnLnaZin50.Margin = new Padding(3, 0, 3, 0);
			rBtnLnaZin50.Name = "rBtnLnaZin50";
			rBtnLnaZin50.Size = new Size(0x25, 0x11);
			rBtnLnaZin50.TabIndex = 0;
			rBtnLnaZin50.TabStop = true;
			rBtnLnaZin50.Text = "50";
			rBtnLnaZin50.UseVisualStyleBackColor = true;
			rBtnLnaZin50.CheckedChanged += new EventHandler(rBtnLnaZin_CheckedChanged);
			panel4.AutoSize = true;
			panel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			panel4.Controls.Add(rBtnLnaLowPowerOff);
			panel4.Controls.Add(rBtnLnaLowPowerOn);
			panel4.Location = new Point(0x6a, 0x54);
			panel4.Name = "panel4";
			panel4.Size = new Size(0x5d, 0x11);
			panel4.TabIndex = 5;
			rBtnLnaLowPowerOff.AutoSize = true;
			rBtnLnaLowPowerOff.BackColor = Color.Transparent;
			rBtnLnaLowPowerOff.Location = new Point(0x2d, 0);
			rBtnLnaLowPowerOff.Margin = new Padding(3, 0, 3, 0);
			rBtnLnaLowPowerOff.Name = "rBtnLnaLowPowerOff";
			rBtnLnaLowPowerOff.Size = new Size(0x2d, 0x11);
			rBtnLnaLowPowerOff.TabIndex = 1;
			rBtnLnaLowPowerOff.Text = "OFF";
			rBtnLnaLowPowerOff.UseVisualStyleBackColor = false;
			rBtnLnaLowPowerOff.CheckedChanged += new EventHandler(rBtnLnaLowPower_CheckedChanged);
			rBtnLnaLowPowerOn.AutoSize = true;
			rBtnLnaLowPowerOn.BackColor = Color.Transparent;
			rBtnLnaLowPowerOn.Checked = true;
			rBtnLnaLowPowerOn.Location = new Point(3, 0);
			rBtnLnaLowPowerOn.Margin = new Padding(3, 0, 3, 0);
			rBtnLnaLowPowerOn.Name = "rBtnLnaLowPowerOn";
			rBtnLnaLowPowerOn.Size = new Size(0x29, 0x11);
			rBtnLnaLowPowerOn.TabIndex = 0;
			rBtnLnaLowPowerOn.TabStop = true;
			rBtnLnaLowPowerOn.Text = "ON";
			rBtnLnaLowPowerOn.UseVisualStyleBackColor = false;
			rBtnLnaLowPowerOn.CheckedChanged += new EventHandler(rBtnLnaLowPower_CheckedChanged);
			label7.AutoSize = true;
			label7.BackColor = Color.Transparent;
			label7.Location = new Point(0xa1, 30);
			label7.Name = "label7";
			label7.Size = new Size(0x20, 13);
			label7.TabIndex = 2;
			label7.Text = "ohms";
			lblSensitivityBoost.AutoSize = true;
			lblSensitivityBoost.Location = new Point(11, 0x3d);
			lblSensitivityBoost.Name = "lblSensitivityBoost";
			lblSensitivityBoost.Size = new Size(0x56, 13);
			lblSensitivityBoost.TabIndex = 3;
			lblSensitivityBoost.Text = "Sensitivity boost:";
			pnlSensitivityBoost.AutoSize = true;
			pnlSensitivityBoost.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			pnlSensitivityBoost.Controls.Add(rBtnSensitivityBoostOff);
			pnlSensitivityBoost.Controls.Add(rBtnSensitivityBoostOn);
			pnlSensitivityBoost.Location = new Point(0x6a, 0x3b);
			pnlSensitivityBoost.Name = "pnlSensitivityBoost";
			pnlSensitivityBoost.Size = new Size(0x5d, 0x11);
			pnlSensitivityBoost.TabIndex = 4;
			rBtnSensitivityBoostOff.AutoSize = true;
			rBtnSensitivityBoostOff.Location = new Point(0x2d, 0);
			rBtnSensitivityBoostOff.Margin = new Padding(3, 0, 3, 0);
			rBtnSensitivityBoostOff.Name = "rBtnSensitivityBoostOff";
			rBtnSensitivityBoostOff.Size = new Size(0x2d, 0x11);
			rBtnSensitivityBoostOff.TabIndex = 1;
			rBtnSensitivityBoostOff.Text = "OFF";
			rBtnSensitivityBoostOff.UseVisualStyleBackColor = true;
			rBtnSensitivityBoostOff.CheckedChanged += new EventHandler(rBtnSensitivityBoost_CheckedChanged);
			rBtnSensitivityBoostOn.AutoSize = true;
			rBtnSensitivityBoostOn.Checked = true;
			rBtnSensitivityBoostOn.Location = new Point(3, 0);
			rBtnSensitivityBoostOn.Margin = new Padding(3, 0, 3, 0);
			rBtnSensitivityBoostOn.Name = "rBtnSensitivityBoostOn";
			rBtnSensitivityBoostOn.Size = new Size(0x29, 0x11);
			rBtnSensitivityBoostOn.TabIndex = 0;
			rBtnSensitivityBoostOn.TabStop = true;
			rBtnSensitivityBoostOn.Text = "ON";
			rBtnSensitivityBoostOn.UseVisualStyleBackColor = true;
			rBtnSensitivityBoostOn.CheckedChanged += new EventHandler(rBtnSensitivityBoost_CheckedChanged);
			gBoxLnaSensitivity.Controls.Add(panel3);
			gBoxLnaSensitivity.Controls.Add(lblSensitivityBoost);
			gBoxLnaSensitivity.Controls.Add(lblAGC);
			gBoxLnaSensitivity.Controls.Add(pnlSensitivityBoost);
			gBoxLnaSensitivity.Controls.Add(label4);
			gBoxLnaSensitivity.Controls.Add(label7);
			gBoxLnaSensitivity.Controls.Add(panel4);
			gBoxLnaSensitivity.Location = new Point(0x249, 3);
			gBoxLnaSensitivity.Name = "gBoxLnaSensitivity";
			gBoxLnaSensitivity.Size = new Size(0xd3, 0x70);
			gBoxLnaSensitivity.TabIndex = 5;
			gBoxLnaSensitivity.TabStop = false;
			gBoxLnaSensitivity.Text = "Lna sensitivity";
			gBoxLnaSensitivity.MouseLeave += new EventHandler(control_MouseLeave);
			gBoxLnaSensitivity.MouseEnter += new EventHandler(control_MouseEnter);
			gBoxAgc.Controls.Add(panel2);
			gBoxAgc.Controls.Add(label5);
			gBoxAgc.Controls.Add(label8);
			gBoxAgc.Controls.Add(label24);
			gBoxAgc.Controls.Add(label25);
			gBoxAgc.Controls.Add(label26);
			gBoxAgc.Controls.Add(label27);
			gBoxAgc.Controls.Add(label28);
			gBoxAgc.Controls.Add(label1);
			gBoxAgc.Controls.Add(label2);
			gBoxAgc.Controls.Add(label3);
			gBoxAgc.Controls.Add(label29);
			gBoxAgc.Controls.Add(label30);
			gBoxAgc.Controls.Add(label31);
			gBoxAgc.Controls.Add(label32);
			gBoxAgc.Controls.Add(label33);
			gBoxAgc.Controls.Add(nudAgcStep5);
			gBoxAgc.Controls.Add(nudAgcSnrMargin);
			gBoxAgc.Controls.Add(nudAgcStep4);
			gBoxAgc.Controls.Add(nudAgcRefLevel);
			gBoxAgc.Controls.Add(nudAgcStep3);
			gBoxAgc.Controls.Add(nudAgcStep1);
			gBoxAgc.Controls.Add(nudAgcStep2);
			gBoxAgc.Location = new Point(0x249, 0x79);
			gBoxAgc.Name = "gBoxAgc";
			gBoxAgc.Size = new Size(0xd3, 0xfb);
			gBoxAgc.TabIndex = 6;
			gBoxAgc.TabStop = false;
			gBoxAgc.Text = "AGC";
			gBoxAgc.MouseLeave += new EventHandler(control_MouseLeave);
			gBoxAgc.MouseEnter += new EventHandler(control_MouseEnter);
			panel2.AutoSize = true;
			panel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			panel2.Controls.Add(rBtnAgcAutoRefOff);
			panel2.Controls.Add(rBtnAgcAutoRefOn);
			panel2.Location = new Point(110, 0x17);
			panel2.Name = "panel2";
			panel2.Size = new Size(0x33, 0x22);
			panel2.TabIndex = 1;
			rBtnAgcAutoRefOff.AutoSize = true;
			rBtnAgcAutoRefOff.Location = new Point(3, 0x11);
			rBtnAgcAutoRefOff.Margin = new Padding(3, 0, 3, 0);
			rBtnAgcAutoRefOff.Name = "rBtnAgcAutoRefOff";
			rBtnAgcAutoRefOff.Size = new Size(0x2d, 0x11);
			rBtnAgcAutoRefOff.TabIndex = 1;
			rBtnAgcAutoRefOff.Text = "OFF";
			rBtnAgcAutoRefOff.UseVisualStyleBackColor = true;
			rBtnAgcAutoRefOff.CheckedChanged += new EventHandler(rBtnAgcAutoRef_CheckedChanged);
			rBtnAgcAutoRefOn.AutoSize = true;
			rBtnAgcAutoRefOn.Checked = true;
			rBtnAgcAutoRefOn.Location = new Point(3, 0);
			rBtnAgcAutoRefOn.Margin = new Padding(3, 0, 3, 0);
			rBtnAgcAutoRefOn.Name = "rBtnAgcAutoRefOn";
			rBtnAgcAutoRefOn.Size = new Size(0x29, 0x11);
			rBtnAgcAutoRefOn.TabIndex = 0;
			rBtnAgcAutoRefOn.TabStop = true;
			rBtnAgcAutoRefOn.Text = "ON";
			rBtnAgcAutoRefOn.UseVisualStyleBackColor = true;
			rBtnAgcAutoRefOn.CheckedChanged += new EventHandler(rBtnAgcAutoRef_CheckedChanged);
			label5.AutoSize = true;
			label5.Location = new Point(15, 0x22);
			label5.Name = "label5";
			label5.Size = new Size(80, 13);
			label5.TabIndex = 0;
			label5.Text = "Auto reference:";
			label8.AutoSize = true;
			label8.BackColor = Color.Transparent;
			label8.Location = new Point(15, 0x41);
			label8.Name = "label8";
			label8.Size = new Size(0x59, 13);
			label8.TabIndex = 2;
			label8.Text = "Reference Level:";
			label24.AutoSize = true;
			label24.BackColor = Color.Transparent;
			label24.Location = new Point(15, 0x75);
			label24.Name = "label24";
			label24.Size = new Size(0x59, 13);
			label24.TabIndex = 8;
			label24.Text = "Threshold step 1:";
			label25.AutoSize = true;
			label25.BackColor = Color.Transparent;
			label25.Location = new Point(15, 0x8f);
			label25.Name = "label25";
			label25.Size = new Size(0x59, 13);
			label25.TabIndex = 11;
			label25.Text = "Threshold step 2:";
			label26.AutoSize = true;
			label26.BackColor = Color.Transparent;
			label26.Location = new Point(15, 0xa9);
			label26.Name = "label26";
			label26.Size = new Size(0x59, 13);
			label26.TabIndex = 14;
			label26.Text = "Threshold step 3:";
			label27.AutoSize = true;
			label27.BackColor = Color.Transparent;
			label27.Location = new Point(15, 0xc3);
			label27.Name = "label27";
			label27.Size = new Size(0x59, 13);
			label27.TabIndex = 0x11;
			label27.Text = "Threshold step 4:";
			label28.AutoSize = true;
			label28.BackColor = Color.Transparent;
			label28.Location = new Point(15, 0xdd);
			label28.Name = "label28";
			label28.Size = new Size(0x59, 13);
			label28.TabIndex = 20;
			label28.Text = "Threshold step 5:";
			label1.AutoSize = true;
			label1.BackColor = Color.Transparent;
			label1.Location = new Point(0xa7, 0x41);
			label1.Name = "label1";
			label1.Size = new Size(0x1c, 13);
			label1.TabIndex = 4;
			label1.Text = "dBm";
			label2.AutoSize = true;
			label2.Location = new Point(15, 0x5d);
			label2.Name = "label2";
			label2.Size = new Size(0x43, 13);
			label2.TabIndex = 5;
			label2.Text = "SNR margin:";
			label3.AutoSize = true;
			label3.BackColor = Color.Transparent;
			label3.Location = new Point(0xa7, 0x5c);
			label3.Name = "label3";
			label3.Size = new Size(20, 13);
			label3.TabIndex = 7;
			label3.Text = "dB";
			label29.AutoSize = true;
			label29.BackColor = Color.Transparent;
			label29.Location = new Point(0xa7, 0x76);
			label29.Name = "label29";
			label29.Size = new Size(20, 13);
			label29.TabIndex = 10;
			label29.Text = "dB";
			label30.AutoSize = true;
			label30.BackColor = Color.Transparent;
			label30.Location = new Point(0xa7, 0x90);
			label30.Name = "label30";
			label30.Size = new Size(20, 13);
			label30.TabIndex = 13;
			label30.Text = "dB";
			label31.AutoSize = true;
			label31.BackColor = Color.Transparent;
			label31.Location = new Point(0xa7, 170);
			label31.Name = "label31";
			label31.Size = new Size(20, 13);
			label31.TabIndex = 0x10;
			label31.Text = "dB";
			label32.AutoSize = true;
			label32.BackColor = Color.Transparent;
			label32.Location = new Point(0xa7, 0xc4);
			label32.Name = "label32";
			label32.Size = new Size(20, 13);
			label32.TabIndex = 0x13;
			label32.Text = "dB";
			label33.AutoSize = true;
			label33.BackColor = Color.Transparent;
			label33.Location = new Point(0xa7, 0xde);
			label33.Name = "label33";
			label33.Size = new Size(20, 13);
			label33.TabIndex = 0x16;
			label33.Text = "dB";
			nudAgcStep5.Location = new Point(110, 0xdb);
			int[] bits = new int[4];
			bits[0] = 15;
			nudAgcStep5.Maximum = new decimal(bits);
			nudAgcStep5.Name = "nudAgcStep5";
			nudAgcStep5.Size = new Size(0x33, 20);
			nudAgcStep5.TabIndex = 0x15;
			int[] numArray2 = new int[4];
			numArray2[0] = 11;
			nudAgcStep5.Value = new decimal(numArray2);
			nudAgcStep5.ValueChanged += new EventHandler(nudAgcStep_ValueChanged);
			nudAgcSnrMargin.Location = new Point(110, 0x59);
			int[] numArray3 = new int[4];
			numArray3[0] = 7;
			nudAgcSnrMargin.Maximum = new decimal(numArray3);
			nudAgcSnrMargin.Name = "nudAgcSnrMargin";
			nudAgcSnrMargin.Size = new Size(0x33, 20);
			nudAgcSnrMargin.TabIndex = 6;
			nudAgcSnrMargin.ThousandsSeparator = true;
			int[] numArray4 = new int[4];
			numArray4[0] = 5;
			nudAgcSnrMargin.Value = new decimal(numArray4);
			nudAgcSnrMargin.ValueChanged += new EventHandler(nudAgcSnrMargin_ValueChanged);
			nudAgcStep4.Location = new Point(110, 0xc1);
			int[] numArray5 = new int[4];
			numArray5[0] = 15;
			nudAgcStep4.Maximum = new decimal(numArray5);
			nudAgcStep4.Name = "nudAgcStep4";
			nudAgcStep4.Size = new Size(0x33, 20);
			nudAgcStep4.TabIndex = 0x12;
			int[] numArray6 = new int[4];
			numArray6[0] = 9;
			nudAgcStep4.Value = new decimal(numArray6);
			nudAgcStep4.ValueChanged += new EventHandler(nudAgcStep_ValueChanged);
			nudAgcRefLevel.Location = new Point(110, 0x3f);
			int[] numArray7 = new int[4];
			numArray7[0] = 80;
			numArray7[3] = -2147483648;
			nudAgcRefLevel.Maximum = new decimal(numArray7);
			int[] numArray8 = new int[4];
			numArray8[0] = 0x8f;
			numArray8[3] = -2147483648;
			nudAgcRefLevel.Minimum = new decimal(numArray8);
			nudAgcRefLevel.Name = "nudAgcRefLevel";
			nudAgcRefLevel.Size = new Size(0x33, 20);
			nudAgcRefLevel.TabIndex = 3;
			nudAgcRefLevel.ThousandsSeparator = true;
			int[] numArray9 = new int[4];
			numArray9[0] = 80;
			numArray9[3] = -2147483648;
			nudAgcRefLevel.Value = new decimal(numArray9);
			nudAgcRefLevel.ValueChanged += new EventHandler(nudAgcRefLevel_ValueChanged);
			nudAgcStep3.Location = new Point(110, 0xa7);
			int[] numArray10 = new int[4];
			numArray10[0] = 15;
			nudAgcStep3.Maximum = new decimal(numArray10);
			nudAgcStep3.Name = "nudAgcStep3";
			nudAgcStep3.Size = new Size(0x33, 20);
			nudAgcStep3.TabIndex = 15;
			int[] numArray11 = new int[4];
			numArray11[0] = 11;
			nudAgcStep3.Value = new decimal(numArray11);
			nudAgcStep3.ValueChanged += new EventHandler(nudAgcStep_ValueChanged);
			nudAgcStep1.Location = new Point(110, 0x73);
			int[] numArray12 = new int[4];
			numArray12[0] = 0x1f;
			nudAgcStep1.Maximum = new decimal(numArray12);
			nudAgcStep1.Name = "nudAgcStep1";
			nudAgcStep1.Size = new Size(0x33, 20);
			nudAgcStep1.TabIndex = 9;
			int[] numArray13 = new int[4];
			numArray13[0] = 0x10;
			nudAgcStep1.Value = new decimal(numArray13);
			nudAgcStep1.ValueChanged += new EventHandler(nudAgcStep_ValueChanged);
			nudAgcStep2.Location = new Point(110, 0x8d);
			int[] numArray14 = new int[4];
			numArray14[0] = 15;
			nudAgcStep2.Maximum = new decimal(numArray14);
			nudAgcStep2.Name = "nudAgcStep2";
			nudAgcStep2.Size = new Size(0x33, 20);
			nudAgcStep2.TabIndex = 12;
			int[] numArray15 = new int[4];
			numArray15[0] = 7;
			nudAgcStep2.Value = new decimal(numArray15);
			nudAgcStep2.ValueChanged += new EventHandler(nudAgcStep_ValueChanged);
			gBoxRssi.Controls.Add(pnlRssiPhase);
			gBoxRssi.Controls.Add(label23);
			gBoxRssi.Controls.Add(btnRestartRx);
			gBoxRssi.Controls.Add(panel7);
			gBoxRssi.Controls.Add(label21);
			gBoxRssi.Controls.Add(btnRssiRead);
			gBoxRssi.Controls.Add(label17);
			gBoxRssi.Controls.Add(label54);
			gBoxRssi.Controls.Add(label55);
			gBoxRssi.Controls.Add(label56);
			gBoxRssi.Controls.Add(lblRssiValue);
			gBoxRssi.Controls.Add(nudRssiThresh);
			gBoxRssi.Controls.Add(ledRssiDone);
			gBoxRssi.Controls.Add(panel1);
			gBoxRssi.Controls.Add(label6);
			gBoxRssi.Controls.Add(nudTimeoutRxStart);
			gBoxRssi.Controls.Add(label9);
			gBoxRssi.Controls.Add(label14);
			gBoxRssi.Controls.Add(label11);
			gBoxRssi.Controls.Add(label15);
			gBoxRssi.Controls.Add(nudTimeoutRssiThresh);
			gBoxRssi.Location = new Point(0x126, 0xb1);
			gBoxRssi.Name = "gBoxRssi";
			gBoxRssi.Size = new Size(0x11d, 0xc3);
			gBoxRssi.TabIndex = 4;
			gBoxRssi.TabStop = false;
			gBoxRssi.Text = "RSSI";
			gBoxRssi.MouseLeave += new EventHandler(control_MouseLeave);
			gBoxRssi.MouseEnter += new EventHandler(control_MouseEnter);
			pnlRssiPhase.AutoSize = true;
			pnlRssiPhase.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			pnlRssiPhase.Controls.Add(rBtnRssiPhaseManual);
			pnlRssiPhase.Controls.Add(rBtnRssiPhaseAuto);
			pnlRssiPhase.Location = new Point(0x85, 0xa8);
			pnlRssiPhase.Margin = new Padding(3, 2, 3, 2);
			pnlRssiPhase.Name = "pnlRssiPhase";
			pnlRssiPhase.Size = new Size(0x77, 20);
			pnlRssiPhase.TabIndex = 20;
			rBtnRssiPhaseManual.AutoSize = true;
			rBtnRssiPhaseManual.Location = new Point(0x38, 3);
			rBtnRssiPhaseManual.Margin = new Padding(3, 0, 3, 0);
			rBtnRssiPhaseManual.Name = "rBtnRssiPhaseManual";
			rBtnRssiPhaseManual.Size = new Size(60, 0x11);
			rBtnRssiPhaseManual.TabIndex = 1;
			rBtnRssiPhaseManual.Text = "Manual";
			rBtnRssiPhaseManual.UseVisualStyleBackColor = true;
			rBtnRssiPhaseManual.CheckedChanged += new EventHandler(rBtnRssiPhaseManual_CheckedChanged);
			rBtnRssiPhaseAuto.AutoSize = true;
			rBtnRssiPhaseAuto.Checked = true;
			rBtnRssiPhaseAuto.Location = new Point(3, 3);
			rBtnRssiPhaseAuto.Margin = new Padding(3, 0, 3, 0);
			rBtnRssiPhaseAuto.Name = "rBtnRssiPhaseAuto";
			rBtnRssiPhaseAuto.Size = new Size(0x2f, 0x11);
			rBtnRssiPhaseAuto.TabIndex = 0;
			rBtnRssiPhaseAuto.TabStop = true;
			rBtnRssiPhaseAuto.Text = "Auto";
			rBtnRssiPhaseAuto.UseVisualStyleBackColor = true;
			rBtnRssiPhaseAuto.CheckedChanged += new EventHandler(rBtnRssiPhaseAuto_CheckedChanged);
			label23.AutoSize = true;
			label23.Location = new Point(6, 0xac);
			label23.Name = "label23";
			label23.Size = new Size(40, 13);
			label23.TabIndex = 0x12;
			label23.Text = "Phase:";
			label23.TextAlign = ContentAlignment.MiddleLeft;
			btnRestartRx.Location = new Point(60, 0xa7);
			btnRestartRx.Name = "btnRestartRx";
			btnRestartRx.Size = new Size(0x43, 0x17);
			btnRestartRx.TabIndex = 0x13;
			btnRestartRx.Text = "Restart Rx";
			btnRestartRx.UseVisualStyleBackColor = true;
			btnRestartRx.Click += new EventHandler(btnRestartRx_Click);
			panel7.AutoSize = true;
			panel7.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			panel7.Controls.Add(rBtnFastRxOff);
			panel7.Controls.Add(rBtnFastRxOn);
			panel7.Location = new Point(0x85, 0x13);
			panel7.Name = "panel7";
			panel7.Size = new Size(0x62, 0x11);
			panel7.TabIndex = 1;
			rBtnFastRxOff.AutoSize = true;
			rBtnFastRxOff.Location = new Point(50, 0);
			rBtnFastRxOff.Margin = new Padding(3, 0, 3, 0);
			rBtnFastRxOff.Name = "rBtnFastRxOff";
			rBtnFastRxOff.Size = new Size(0x2d, 0x11);
			rBtnFastRxOff.TabIndex = 1;
			rBtnFastRxOff.Text = "OFF";
			rBtnFastRxOff.UseVisualStyleBackColor = true;
			rBtnFastRxOn.AutoSize = true;
			rBtnFastRxOn.Checked = true;
			rBtnFastRxOn.Location = new Point(3, 0);
			rBtnFastRxOn.Margin = new Padding(3, 0, 3, 0);
			rBtnFastRxOn.Name = "rBtnFastRxOn";
			rBtnFastRxOn.Size = new Size(0x29, 0x11);
			rBtnFastRxOn.TabIndex = 0;
			rBtnFastRxOn.TabStop = true;
			rBtnFastRxOn.Text = "ON";
			rBtnFastRxOn.UseVisualStyleBackColor = true;
			rBtnFastRxOn.CheckedChanged += new EventHandler(rBtnFastRx_CheckedChanged);
			label21.AutoSize = true;
			label21.Location = new Point(3, 0x15);
			label21.Name = "label21";
			label21.Size = new Size(0x57, 13);
			label21.TabIndex = 0;
			label21.Text = "Fast Rx wakeup:";
			btnRssiRead.Location = new Point(0x56, 0x8e);
			btnRssiRead.Name = "btnRssiRead";
			btnRssiRead.Size = new Size(0x29, 0x17);
			btnRssiRead.TabIndex = 14;
			btnRssiRead.Text = "Read";
			btnRssiRead.UseVisualStyleBackColor = true;
			btnRssiRead.Visible = false;
			btnRssiRead.Click += new EventHandler(btnRssiStart_Click);
			label17.AutoSize = true;
			label17.BackColor = Color.Transparent;
			label17.Location = new Point(0xff, 0x79);
			label17.Name = "label17";
			label17.Size = new Size(0x1c, 13);
			label17.TabIndex = 12;
			label17.Text = "dBm";
			label17.TextAlign = ContentAlignment.MiddleCenter;
			label54.AutoSize = true;
			label54.BackColor = Color.Transparent;
			label54.Location = new Point(0xff, 0x93);
			label54.Name = "label54";
			label54.Size = new Size(0x1c, 13);
			label54.TabIndex = 0x11;
			label54.Text = "dBm";
			label54.TextAlign = ContentAlignment.MiddleCenter;
			label55.AutoSize = true;
			label55.BackColor = Color.Transparent;
			label55.Location = new Point(3, 0x79);
			label55.Margin = new Padding(0);
			label55.Name = "label55";
			label55.Size = new Size(0x39, 13);
			label55.TabIndex = 10;
			label55.Text = "Threshold:";
			label55.TextAlign = ContentAlignment.MiddleCenter;
			label56.AutoSize = true;
			label56.BackColor = Color.Transparent;
			label56.Location = new Point(3, 0x93);
			label56.Margin = new Padding(0);
			label56.Name = "label56";
			label56.Size = new Size(0x25, 13);
			label56.TabIndex = 13;
			label56.Text = "Value:";
			label56.TextAlign = ContentAlignment.MiddleCenter;
			lblRssiValue.BackColor = Color.Transparent;
			lblRssiValue.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			lblRssiValue.Location = new Point(0x85, 0x8f);
			lblRssiValue.Margin = new Padding(3);
			lblRssiValue.Name = "lblRssiValue";
			lblRssiValue.Size = new Size(0x62, 20);
			lblRssiValue.TabIndex = 15;
			lblRssiValue.Text = "0";
			lblRssiValue.TextAlign = ContentAlignment.MiddleCenter;
			nudRssiThresh.DecimalPlaces = 1;
			nudRssiThresh.Enabled = false;
			int[] numArray16 = new int[4];
			numArray16[0] = 5;
			numArray16[3] = 0x10000;
			nudRssiThresh.Increment = new decimal(numArray16);
			nudRssiThresh.Location = new Point(0x85, 0x75);
			int[] numArray17 = new int[4];
			nudRssiThresh.Maximum = new decimal(numArray17);
			int[] numArray18 = new int[4];
			numArray18[0] = 0x4fb;
			numArray18[3] = -2147418112;
			nudRssiThresh.Minimum = new decimal(numArray18);
			nudRssiThresh.Name = "nudRssiThresh";
			nudRssiThresh.Size = new Size(0x62, 20);
			nudRssiThresh.TabIndex = 11;
			nudRssiThresh.ThousandsSeparator = true;
			int[] numArray19 = new int[4];
			numArray19[0] = 80;
			numArray19[3] = -2147483648;
			nudRssiThresh.Value = new decimal(numArray19);
			nudRssiThresh.ValueChanged += new EventHandler(nudRssiThresh_ValueChanged);
			ledRssiDone.BackColor = Color.Transparent;
			ledRssiDone.LedColor = Color.Green;
			ledRssiDone.LedSize = new Size(11, 11);
			ledRssiDone.Location = new Point(0xea, 0x92);
			ledRssiDone.Name = "ledRssiDone";
			ledRssiDone.Size = new Size(15, 15);
			ledRssiDone.TabIndex = 0x10;
			ledRssiDone.Text = "led1";
			panel1.AutoSize = true;
			panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			panel1.Controls.Add(rBtnRssiAutoThreshOff);
			panel1.Controls.Add(rBtnRssiAutoThreshOn);
			panel1.Location = new Point(0x85, 0x5e);
			panel1.Name = "panel1";
			panel1.Size = new Size(0x62, 0x11);
			panel1.TabIndex = 9;
			rBtnRssiAutoThreshOff.AutoSize = true;
			rBtnRssiAutoThreshOff.Location = new Point(50, 0);
			rBtnRssiAutoThreshOff.Margin = new Padding(3, 0, 3, 0);
			rBtnRssiAutoThreshOff.Name = "rBtnRssiAutoThreshOff";
			rBtnRssiAutoThreshOff.Size = new Size(0x2d, 0x11);
			rBtnRssiAutoThreshOff.TabIndex = 1;
			rBtnRssiAutoThreshOff.Text = "OFF";
			rBtnRssiAutoThreshOff.UseVisualStyleBackColor = true;
			rBtnRssiAutoThreshOff.CheckedChanged += new EventHandler(rBtnRssiAutoThreshOn_CheckedChanged);
			rBtnRssiAutoThreshOn.AutoSize = true;
			rBtnRssiAutoThreshOn.Checked = true;
			rBtnRssiAutoThreshOn.Location = new Point(3, 0);
			rBtnRssiAutoThreshOn.Margin = new Padding(3, 0, 3, 0);
			rBtnRssiAutoThreshOn.Name = "rBtnRssiAutoThreshOn";
			rBtnRssiAutoThreshOn.Size = new Size(0x29, 0x11);
			rBtnRssiAutoThreshOn.TabIndex = 0;
			rBtnRssiAutoThreshOn.TabStop = true;
			rBtnRssiAutoThreshOn.Text = "ON";
			rBtnRssiAutoThreshOn.UseVisualStyleBackColor = true;
			rBtnRssiAutoThreshOn.CheckedChanged += new EventHandler(rBtnRssiAutoThreshOn_CheckedChanged);
			label6.AutoSize = true;
			label6.Location = new Point(3, 0x60);
			label6.Name = "label6";
			label6.Size = new Size(0x4e, 13);
			label6.TabIndex = 8;
			label6.Text = "Auto threshold:";
			nudTimeoutRxStart.Location = new Point(0x85, 0x2a);
			int[] numArray20 = new int[4];
			numArray20[0] = 850;
			nudTimeoutRxStart.Maximum = new decimal(numArray20);
			nudTimeoutRxStart.Name = "nudTimeoutRxStart";
			nudTimeoutRxStart.Size = new Size(0x62, 20);
			nudTimeoutRxStart.TabIndex = 3;
			nudTimeoutRxStart.ThousandsSeparator = true;
			nudTimeoutRxStart.ValueChanged += new EventHandler(nudTimeoutRxStart_ValueChanged);
			label9.AutoSize = true;
			label9.Location = new Point(3, 0x2e);
			label9.Name = "label9";
			label9.Size = new Size(0x57, 13);
			label9.TabIndex = 2;
			label9.Text = "Timeout Rx start:";
			label14.AutoSize = true;
			label14.Location = new Point(3, 0x48);
			label14.Name = "label14";
			label14.Size = new Size(0x5e, 13);
			label14.TabIndex = 5;
			label14.Text = "Timeout threshold:";
			label11.AutoSize = true;
			label11.Location = new Point(0xff, 0x2e);
			label11.Name = "label11";
			label11.Size = new Size(20, 13);
			label11.TabIndex = 4;
			label11.Text = "ms";
			label15.AutoSize = true;
			label15.Location = new Point(0xff, 0x48);
			label15.Name = "label15";
			label15.Size = new Size(20, 13);
			label15.TabIndex = 7;
			label15.Text = "ms";
			nudTimeoutRssiThresh.Location = new Point(0x85, 0x44);
			int[] numArray21 = new int[4];
			numArray21[0] = 850;
			nudTimeoutRssiThresh.Maximum = new decimal(numArray21);
			nudTimeoutRssiThresh.Name = "nudTimeoutRssiThresh";
			nudTimeoutRssiThresh.Size = new Size(0x62, 20);
			nudTimeoutRssiThresh.TabIndex = 6;
			nudTimeoutRssiThresh.ThousandsSeparator = true;
			nudTimeoutRssiThresh.ValueChanged += new EventHandler(nudTimeoutRssiThresh_ValueChanged);
			gBoxAfcFei.Controls.Add(nudLowBetaAfcOffset);
			gBoxAfcFei.Controls.Add(lblLowBetaAfcOffset);
			gBoxAfcFei.Controls.Add(lblAfcLowBeta);
			gBoxAfcFei.Controls.Add(label19);
			gBoxAfcFei.Controls.Add(lblLowBetaAfcOfssetUnit);
			gBoxAfcFei.Controls.Add(label20);
			gBoxAfcFei.Controls.Add(pnlAfcLowBeta);
			gBoxAfcFei.Controls.Add(btnFeiRead);
			gBoxAfcFei.Controls.Add(panel8);
			gBoxAfcFei.Controls.Add(ledFeiDone);
			gBoxAfcFei.Controls.Add(panel9);
			gBoxAfcFei.Controls.Add(lblFeiValue);
			gBoxAfcFei.Controls.Add(label12);
			gBoxAfcFei.Controls.Add(label18);
			gBoxAfcFei.Controls.Add(label10);
			gBoxAfcFei.Controls.Add(btnAfcClear);
			gBoxAfcFei.Controls.Add(btnAfcStart);
			gBoxAfcFei.Controls.Add(ledAfcDone);
			gBoxAfcFei.Controls.Add(lblAfcValue);
			gBoxAfcFei.Controls.Add(label22);
			gBoxAfcFei.Location = new Point(0x126, 3);
			gBoxAfcFei.Name = "gBoxAfcFei";
			gBoxAfcFei.Size = new Size(0x11d, 0xa8);
			gBoxAfcFei.TabIndex = 3;
			gBoxAfcFei.TabStop = false;
			gBoxAfcFei.Text = "AFC / FEI";
			gBoxAfcFei.MouseLeave += new EventHandler(control_MouseLeave);
			gBoxAfcFei.MouseEnter += new EventHandler(control_MouseEnter);
			int[] numArray22 = new int[4];
			numArray22[0] = 0x1e8;
			nudLowBetaAfcOffset.Increment = new decimal(numArray22);
			nudLowBetaAfcOffset.Location = new Point(0x85, 0x2a);
			int[] numArray23 = new int[4];
			numArray23[0] = 0xf218;
			nudLowBetaAfcOffset.Maximum = new decimal(numArray23);
			int[] numArray24 = new int[4];
			numArray24[0] = 0xf400;
			numArray24[3] = -2147483648;
			nudLowBetaAfcOffset.Minimum = new decimal(numArray24);
			nudLowBetaAfcOffset.Name = "nudLowBetaAfcOffset";
			nudLowBetaAfcOffset.Size = new Size(0x62, 20);
			nudLowBetaAfcOffset.TabIndex = 2;
			nudLowBetaAfcOffset.ThousandsSeparator = true;
			nudLowBetaAfcOffset.ValueChanged += new EventHandler(nudLowBetaAfcOffset_ValueChanged);
			lblLowBetaAfcOffset.AutoSize = true;
			lblLowBetaAfcOffset.Location = new Point(3, 0x2c);
			lblLowBetaAfcOffset.Name = "lblLowBetaAfcOffset";
			lblLowBetaAfcOffset.Size = new Size(0x66, 13);
			lblLowBetaAfcOffset.TabIndex = 3;
			lblLowBetaAfcOffset.Text = "AFC low beta offset:";
			lblAfcLowBeta.AutoSize = true;
			lblAfcLowBeta.Location = new Point(3, 0x15);
			lblAfcLowBeta.Name = "lblAfcLowBeta";
			lblAfcLowBeta.Size = new Size(0x49, 13);
			lblAfcLowBeta.TabIndex = 0;
			lblAfcLowBeta.Text = "AFC low beta:";
			label19.AutoSize = true;
			label19.Location = new Point(3, 70);
			label19.Name = "label19";
			label19.Size = new Size(80, 13);
			label19.TabIndex = 5;
			label19.Text = "AFC auto clear:";
			lblLowBetaAfcOfssetUnit.AutoSize = true;
			lblLowBetaAfcOfssetUnit.Location = new Point(0xff, 0x2e);
			lblLowBetaAfcOfssetUnit.Name = "lblLowBetaAfcOfssetUnit";
			lblLowBetaAfcOfssetUnit.Size = new Size(20, 13);
			lblLowBetaAfcOfssetUnit.TabIndex = 4;
			lblLowBetaAfcOfssetUnit.Text = "Hz";
			label20.AutoSize = true;
			label20.Location = new Point(3, 0x5d);
			label20.Name = "label20";
			label20.Size = new Size(0x36, 13);
			label20.TabIndex = 8;
			label20.Text = "AFC auto:";
			pnlAfcLowBeta.AutoSize = true;
			pnlAfcLowBeta.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			pnlAfcLowBeta.Controls.Add(rBtnAfcLowBetaOff);
			pnlAfcLowBeta.Controls.Add(rBtnAfcLowBetaOn);
			pnlAfcLowBeta.Location = new Point(0x85, 0x13);
			pnlAfcLowBeta.Name = "pnlAfcLowBeta";
			pnlAfcLowBeta.Size = new Size(0x62, 0x11);
			pnlAfcLowBeta.TabIndex = 1;
			rBtnAfcLowBetaOff.AutoSize = true;
			rBtnAfcLowBetaOff.Location = new Point(50, 0);
			rBtnAfcLowBetaOff.Margin = new Padding(3, 0, 3, 0);
			rBtnAfcLowBetaOff.Name = "rBtnAfcLowBetaOff";
			rBtnAfcLowBetaOff.Size = new Size(0x2d, 0x11);
			rBtnAfcLowBetaOff.TabIndex = 1;
			rBtnAfcLowBetaOff.Text = "OFF";
			rBtnAfcLowBetaOff.UseVisualStyleBackColor = true;
			rBtnAfcLowBetaOff.CheckedChanged += new EventHandler(rBtnAfcLowBeta_CheckedChanged);
			rBtnAfcLowBetaOn.AutoSize = true;
			rBtnAfcLowBetaOn.Checked = true;
			rBtnAfcLowBetaOn.Location = new Point(3, 0);
			rBtnAfcLowBetaOn.Margin = new Padding(3, 0, 3, 0);
			rBtnAfcLowBetaOn.Name = "rBtnAfcLowBetaOn";
			rBtnAfcLowBetaOn.Size = new Size(0x29, 0x11);
			rBtnAfcLowBetaOn.TabIndex = 0;
			rBtnAfcLowBetaOn.TabStop = true;
			rBtnAfcLowBetaOn.Text = "ON";
			rBtnAfcLowBetaOn.UseVisualStyleBackColor = true;
			rBtnAfcLowBetaOn.CheckedChanged += new EventHandler(rBtnAfcLowBeta_CheckedChanged);
			btnFeiRead.Location = new Point(0x56, 0x8b);
			btnFeiRead.Name = "btnFeiRead";
			btnFeiRead.Size = new Size(0x29, 0x17);
			btnFeiRead.TabIndex = 0x10;
			btnFeiRead.Text = "Read";
			btnFeiRead.UseVisualStyleBackColor = true;
			btnFeiRead.Click += new EventHandler(btnFeiStart_Click);
			panel8.AutoSize = true;
			panel8.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			panel8.Controls.Add(rBtnAfcAutoClearOff);
			panel8.Controls.Add(rBtnAfcAutoClearOn);
			panel8.Location = new Point(0x85, 0x44);
			panel8.Name = "panel8";
			panel8.Size = new Size(0x62, 0x11);
			panel8.TabIndex = 6;
			rBtnAfcAutoClearOff.AutoSize = true;
			rBtnAfcAutoClearOff.Location = new Point(50, 0);
			rBtnAfcAutoClearOff.Margin = new Padding(3, 0, 3, 0);
			rBtnAfcAutoClearOff.Name = "rBtnAfcAutoClearOff";
			rBtnAfcAutoClearOff.Size = new Size(0x2d, 0x11);
			rBtnAfcAutoClearOff.TabIndex = 1;
			rBtnAfcAutoClearOff.Text = "OFF";
			rBtnAfcAutoClearOff.UseVisualStyleBackColor = true;
			rBtnAfcAutoClearOff.CheckedChanged += new EventHandler(rBtnAfcAutoClearOn_CheckedChanged);
			rBtnAfcAutoClearOn.AutoSize = true;
			rBtnAfcAutoClearOn.Checked = true;
			rBtnAfcAutoClearOn.Location = new Point(3, 0);
			rBtnAfcAutoClearOn.Margin = new Padding(3, 0, 3, 0);
			rBtnAfcAutoClearOn.Name = "rBtnAfcAutoClearOn";
			rBtnAfcAutoClearOn.Size = new Size(0x29, 0x11);
			rBtnAfcAutoClearOn.TabIndex = 0;
			rBtnAfcAutoClearOn.TabStop = true;
			rBtnAfcAutoClearOn.Text = "ON";
			rBtnAfcAutoClearOn.UseVisualStyleBackColor = true;
			rBtnAfcAutoClearOn.CheckedChanged += new EventHandler(rBtnAfcAutoClearOn_CheckedChanged);
			ledFeiDone.BackColor = Color.Transparent;
			ledFeiDone.LedColor = Color.Green;
			ledFeiDone.LedSize = new Size(11, 11);
			ledFeiDone.Location = new Point(0xea, 0x8f);
			ledFeiDone.Name = "ledFeiDone";
			ledFeiDone.Size = new Size(15, 15);
			ledFeiDone.TabIndex = 0x12;
			ledFeiDone.Text = "led1";
			panel9.AutoSize = true;
			panel9.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			panel9.Controls.Add(rBtnAfcAutoOff);
			panel9.Controls.Add(rBtnAfcAutoOn);
			panel9.Location = new Point(0x85, 0x5b);
			panel9.Name = "panel9";
			panel9.Size = new Size(0x62, 0x11);
			panel9.TabIndex = 7;
			rBtnAfcAutoOff.AutoSize = true;
			rBtnAfcAutoOff.Location = new Point(50, 0);
			rBtnAfcAutoOff.Margin = new Padding(3, 0, 3, 0);
			rBtnAfcAutoOff.Name = "rBtnAfcAutoOff";
			rBtnAfcAutoOff.Size = new Size(0x2d, 0x11);
			rBtnAfcAutoOff.TabIndex = 1;
			rBtnAfcAutoOff.Text = "OFF";
			rBtnAfcAutoOff.UseVisualStyleBackColor = true;
			rBtnAfcAutoOff.CheckedChanged += new EventHandler(rBtnAfcAutoOn_CheckedChanged);
			rBtnAfcAutoOn.AutoSize = true;
			rBtnAfcAutoOn.Checked = true;
			rBtnAfcAutoOn.Location = new Point(3, 0);
			rBtnAfcAutoOn.Margin = new Padding(3, 0, 3, 0);
			rBtnAfcAutoOn.Name = "rBtnAfcAutoOn";
			rBtnAfcAutoOn.Size = new Size(0x29, 0x11);
			rBtnAfcAutoOn.TabIndex = 0;
			rBtnAfcAutoOn.TabStop = true;
			rBtnAfcAutoOn.Text = "ON";
			rBtnAfcAutoOn.UseVisualStyleBackColor = true;
			rBtnAfcAutoOn.CheckedChanged += new EventHandler(rBtnAfcAutoOn_CheckedChanged);
			lblFeiValue.BackColor = Color.Transparent;
			lblFeiValue.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			lblFeiValue.Location = new Point(0x85, 140);
			lblFeiValue.Margin = new Padding(3);
			lblFeiValue.Name = "lblFeiValue";
			lblFeiValue.Size = new Size(0x62, 20);
			lblFeiValue.TabIndex = 0x11;
			lblFeiValue.Text = "0";
			lblFeiValue.TextAlign = ContentAlignment.MiddleLeft;
			label12.AutoSize = true;
			label12.BackColor = Color.Transparent;
			label12.Location = new Point(3, 0x90);
			label12.Name = "label12";
			label12.Size = new Size(0x1a, 13);
			label12.TabIndex = 15;
			label12.Text = "FEI:";
			label12.TextAlign = ContentAlignment.MiddleCenter;
			label18.AutoSize = true;
			label18.Location = new Point(0xff, 0x76);
			label18.Name = "label18";
			label18.Size = new Size(20, 13);
			label18.TabIndex = 14;
			label18.Text = "Hz";
			label10.AutoSize = true;
			label10.Location = new Point(0xff, 0x90);
			label10.Name = "label10";
			label10.Size = new Size(20, 13);
			label10.TabIndex = 0x13;
			label10.Text = "Hz";
			btnAfcClear.Location = new Point(0x56, 0x71);
			btnAfcClear.Name = "btnAfcClear";
			btnAfcClear.Size = new Size(0x29, 0x17);
			btnAfcClear.TabIndex = 11;
			btnAfcClear.Text = "Clear";
			btnAfcClear.UseVisualStyleBackColor = true;
			btnAfcClear.Click += new EventHandler(btnAfcClear_Click);
			btnAfcStart.Location = new Point(0x27, 0x71);
			btnAfcStart.Name = "btnAfcStart";
			btnAfcStart.Size = new Size(0x29, 0x17);
			btnAfcStart.TabIndex = 10;
			btnAfcStart.Text = "Start";
			btnAfcStart.UseVisualStyleBackColor = true;
			btnAfcStart.Click += new EventHandler(btnAfcStart_Click);
			ledAfcDone.BackColor = Color.Transparent;
			ledAfcDone.LedColor = Color.Green;
			ledAfcDone.LedSize = new Size(11, 11);
			ledAfcDone.Location = new Point(0xea, 0x75);
			ledAfcDone.Name = "ledAfcDone";
			ledAfcDone.Size = new Size(15, 15);
			ledAfcDone.TabIndex = 13;
			ledAfcDone.Text = "led1";
			lblAfcValue.BackColor = Color.Transparent;
			lblAfcValue.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			lblAfcValue.Location = new Point(0x85, 0x72);
			lblAfcValue.Margin = new Padding(3);
			lblAfcValue.Name = "lblAfcValue";
			lblAfcValue.Size = new Size(0x62, 20);
			lblAfcValue.TabIndex = 12;
			lblAfcValue.Text = "0";
			lblAfcValue.TextAlign = ContentAlignment.MiddleLeft;
			label22.AutoSize = true;
			label22.BackColor = Color.Transparent;
			label22.Location = new Point(3, 0x76);
			label22.Name = "label22";
			label22.Size = new Size(30, 13);
			label22.TabIndex = 9;
			label22.Text = "AFC:";
			label22.TextAlign = ContentAlignment.MiddleCenter;
			gBoxOok.Controls.Add(cBoxOokThreshType);
			gBoxOok.Controls.Add(lblOokType);
			gBoxOok.Controls.Add(lblOokStep);
			gBoxOok.Controls.Add(lblOokDec);
			gBoxOok.Controls.Add(lblOokCutoff);
			gBoxOok.Controls.Add(lblOokFixed);
			gBoxOok.Controls.Add(suffixOOKstep);
			gBoxOok.Controls.Add(suffixOOKfixed);
			gBoxOok.Controls.Add(nudOokPeakThreshStep);
			gBoxOok.Controls.Add(nudOokFixedThresh);
			gBoxOok.Controls.Add(cBoxOokPeakThreshDec);
			gBoxOok.Controls.Add(cBoxOokAverageThreshFilt);
			gBoxOok.Location = new Point(3, 0xc5);
			gBoxOok.Name = "gBoxOok";
			gBoxOok.Size = new Size(0x11d, 0xaf);
			gBoxOok.TabIndex = 2;
			gBoxOok.TabStop = false;
			gBoxOok.Text = "OOK";
			gBoxOok.MouseLeave += new EventHandler(control_MouseLeave);
			gBoxOok.MouseEnter += new EventHandler(control_MouseEnter);
			cBoxOokThreshType.FormattingEnabled = true;
			cBoxOokThreshType.Items.AddRange(new object[] { "Fixed", "Peak", "Average" });
			cBoxOokThreshType.Location = new Point(0x7b, 30);
			cBoxOokThreshType.Name = "cBoxOokThreshType";
			cBoxOokThreshType.Size = new Size(0x7c, 0x15);
			cBoxOokThreshType.TabIndex = 1;
			cBoxOokThreshType.SelectedIndexChanged += new EventHandler(cBoxOokThreshType_SelectedIndexChanged);
			lblOokType.AutoSize = true;
			lblOokType.Location = new Point(6, 0x22);
			lblOokType.Name = "lblOokType";
			lblOokType.Size = new Size(80, 13);
			lblOokType.TabIndex = 0;
			lblOokType.Text = "Threshold type:";
			lblOokStep.AutoSize = true;
			lblOokStep.Location = new Point(6, 60);
			lblOokStep.Name = "lblOokStep";
			lblOokStep.Size = new Size(0x68, 13);
			lblOokStep.TabIndex = 2;
			lblOokStep.Text = "Peak threshold step:";
			lblOokDec.AutoSize = true;
			lblOokDec.Location = new Point(6, 0x56);
			lblOokDec.Name = "lblOokDec";
			lblOokDec.Size = new Size(0x66, 13);
			lblOokDec.TabIndex = 5;
			lblOokDec.Text = "Peak threshold dec:";
			lblOokCutoff.AutoSize = true;
			lblOokCutoff.Location = new Point(6, 0x70);
			lblOokCutoff.Name = "lblOokCutoff";
			lblOokCutoff.Size = new Size(0x69, 13);
			lblOokCutoff.TabIndex = 7;
			lblOokCutoff.Text = "Avg threshold cutoff:";
			lblOokFixed.AutoSize = true;
			lblOokFixed.Location = new Point(6, 0x8a);
			lblOokFixed.Name = "lblOokFixed";
			lblOokFixed.Size = new Size(0x51, 13);
			lblOokFixed.TabIndex = 9;
			lblOokFixed.Text = "Fixed threshold:";
			suffixOOKstep.AutoSize = true;
			suffixOOKstep.BackColor = Color.Transparent;
			suffixOOKstep.Location = new Point(0xfd, 60);
			suffixOOKstep.Name = "suffixOOKstep";
			suffixOOKstep.Size = new Size(20, 13);
			suffixOOKstep.TabIndex = 4;
			suffixOOKstep.Text = "dB";
			suffixOOKfixed.AutoSize = true;
			suffixOOKfixed.BackColor = Color.Transparent;
			suffixOOKfixed.Location = new Point(0xfd, 0x8a);
			suffixOOKfixed.Name = "suffixOOKfixed";
			suffixOOKfixed.Size = new Size(20, 13);
			suffixOOKfixed.TabIndex = 11;
			suffixOOKfixed.Text = "dB";
			nudOokPeakThreshStep.DecimalPlaces = 1;
			int[] numArray25 = new int[4];
			numArray25[0] = 5;
			numArray25[3] = 0x10000;
			nudOokPeakThreshStep.Increment = new decimal(numArray25);
			nudOokPeakThreshStep.Location = new Point(0x7b, 0x38);
			int[] numArray26 = new int[4];
			numArray26[0] = 60;
			numArray26[3] = 0x10000;
			nudOokPeakThreshStep.Maximum = new decimal(numArray26);
			int[] numArray27 = new int[4];
			numArray27[0] = 5;
			numArray27[3] = 0x10000;
			nudOokPeakThreshStep.Minimum = new decimal(numArray27);
			nudOokPeakThreshStep.Name = "nudOokPeakThreshStep";
			nudOokPeakThreshStep.Size = new Size(0x7c, 20);
			nudOokPeakThreshStep.TabIndex = 3;
			nudOokPeakThreshStep.ThousandsSeparator = true;
			int[] numArray28 = new int[4];
			numArray28[0] = 5;
			numArray28[3] = 0x10000;
			nudOokPeakThreshStep.Value = new decimal(numArray28);
			nudOokPeakThreshStep.ValueChanged += new EventHandler(nudOokPeakThreshStep_ValueChanged);
			nudOokPeakThreshStep.Validating += new CancelEventHandler(nudOokPeakThreshStep_Validating);
			nudOokFixedThresh.Location = new Point(0x7b, 0x86);
			int[] numArray29 = new int[4];
			numArray29[0] = 0xff;
			nudOokFixedThresh.Maximum = new decimal(numArray29);
			nudOokFixedThresh.Name = "nudOokFixedThresh";
			nudOokFixedThresh.Size = new Size(0x7c, 20);
			nudOokFixedThresh.TabIndex = 10;
			nudOokFixedThresh.ThousandsSeparator = true;
			int[] numArray30 = new int[4];
			numArray30[0] = 6;
			nudOokFixedThresh.Value = new decimal(numArray30);
			nudOokFixedThresh.ValueChanged += new EventHandler(nudOokFixedThresh_ValueChanged);
			cBoxOokPeakThreshDec.FormattingEnabled = true;
			cBoxOokPeakThreshDec.Items.AddRange(new object[] { "Once per chip", "Once every 2 chips", "Once every 4 chips", "Once every 8 chips", "2 times per chip", "4 times per chip", "8 times per chip", "16 times per chip" });
			cBoxOokPeakThreshDec.Location = new Point(0x7b, 0x52);
			cBoxOokPeakThreshDec.Name = "cBoxOokPeakThreshDec";
			cBoxOokPeakThreshDec.Size = new Size(0x7c, 0x15);
			cBoxOokPeakThreshDec.TabIndex = 6;
			cBoxOokPeakThreshDec.SelectedIndexChanged += new EventHandler(cBoxOokPeakThreshDec_SelectedIndexChanged);
			cBoxOokAverageThreshFilt.FormattingEnabled = true;
			cBoxOokAverageThreshFilt.Items.AddRange(new object[] { "Bitrate / 32π", "Bitrate / 8π", "Bitrate / 4π", "Bitrate / 2π" });
			cBoxOokAverageThreshFilt.Location = new Point(0x7b, 0x6c);
			cBoxOokAverageThreshFilt.Name = "cBoxOokAverageThreshFilt";
			cBoxOokAverageThreshFilt.Size = new Size(0x7c, 0x15);
			cBoxOokAverageThreshFilt.TabIndex = 8;
			cBoxOokAverageThreshFilt.SelectedIndexChanged += new EventHandler(cBoxOokAverageThreshFilt_SelectedIndexChanged);
			gBoxAfcBw.Controls.Add(nudAfcDccFreq);
			gBoxAfcBw.Controls.Add(lblAfcDcc);
			gBoxAfcBw.Controls.Add(lblAfcRxBw);
			gBoxAfcBw.Controls.Add(suffixAFCDCC);
			gBoxAfcBw.Controls.Add(suffixAFCRxBw);
			gBoxAfcBw.Controls.Add(nudRxFilterBwAfc);
			gBoxAfcBw.Location = new Point(3, 100);
			gBoxAfcBw.Name = "gBoxAfcBw";
			gBoxAfcBw.Size = new Size(0x11d, 0x5b);
			gBoxAfcBw.TabIndex = 1;
			gBoxAfcBw.TabStop = false;
			gBoxAfcBw.Text = "AFC bandwidth";
			gBoxAfcBw.MouseLeave += new EventHandler(control_MouseLeave);
			gBoxAfcBw.MouseEnter += new EventHandler(control_MouseEnter);
			nudAfcDccFreq.Location = new Point(0x7b, 0x1c);
			int[] numArray31 = new int[4];
			numArray31[0] = 0x679;
			nudAfcDccFreq.Maximum = new decimal(numArray31);
			int[] numArray32 = new int[4];
			numArray32[0] = 12;
			nudAfcDccFreq.Minimum = new decimal(numArray32);
			nudAfcDccFreq.Name = "nudAfcDccFreq";
			nudAfcDccFreq.Size = new Size(0x7c, 20);
			nudAfcDccFreq.TabIndex = 1;
			nudAfcDccFreq.ThousandsSeparator = true;
			int[] numArray33 = new int[4];
			numArray33[0] = 0x1f1;
			nudAfcDccFreq.Value = new decimal(numArray33);
			nudAfcDccFreq.ValueChanged += new EventHandler(nudAfcDccFreq_ValueChanged);
			lblAfcDcc.AutoSize = true;
			lblAfcDcc.Location = new Point(6, 30);
			lblAfcDcc.Name = "lblAfcDcc";
			lblAfcDcc.Size = new Size(0x52, 13);
			lblAfcDcc.TabIndex = 0;
			lblAfcDcc.Text = "DCC frequency:";
			lblAfcRxBw.AutoSize = true;
			lblAfcRxBw.Location = new Point(6, 0x39);
			lblAfcRxBw.Name = "lblAfcRxBw";
			lblAfcRxBw.Size = new Size(0x61, 13);
			lblAfcRxBw.TabIndex = 3;
			lblAfcRxBw.Text = "Rx filter bandwidth:";
			suffixAFCDCC.AutoSize = true;
			suffixAFCDCC.Location = new Point(0xfd, 0x20);
			suffixAFCDCC.Name = "suffixAFCDCC";
			suffixAFCDCC.Size = new Size(20, 13);
			suffixAFCDCC.TabIndex = 2;
			suffixAFCDCC.Text = "Hz";
			suffixAFCRxBw.AutoSize = true;
			suffixAFCRxBw.Location = new Point(0xfd, 0x3b);
			suffixAFCRxBw.Name = "suffixAFCRxBw";
			suffixAFCRxBw.Size = new Size(20, 13);
			suffixAFCRxBw.TabIndex = 5;
			suffixAFCRxBw.Text = "Hz";
			nudRxFilterBwAfc.Location = new Point(0x7b, 0x37);
			int[] numArray34 = new int[4];
			numArray34[0] = 0x61a80;
			nudRxFilterBwAfc.Maximum = new decimal(numArray34);
			int[] numArray35 = new int[4];
			numArray35[0] = 0xc35;
			nudRxFilterBwAfc.Minimum = new decimal(numArray35);
			nudRxFilterBwAfc.Name = "nudRxFilterBwAfc";
			nudRxFilterBwAfc.Size = new Size(0x7c, 20);
			nudRxFilterBwAfc.TabIndex = 4;
			nudRxFilterBwAfc.ThousandsSeparator = true;
			int[] numArray36 = new int[4];
			numArray36[0] = 0xc350;
			nudRxFilterBwAfc.Value = new decimal(numArray36);
			nudRxFilterBwAfc.ValueChanged += new EventHandler(nudRxFilterBwAfc_ValueChanged);
			gBoxRxBw.Controls.Add(nudDccFreq);
			gBoxRxBw.Controls.Add(lblDcc);
			gBoxRxBw.Controls.Add(lblRxBw);
			gBoxRxBw.Controls.Add(suffixDCC);
			gBoxRxBw.Controls.Add(suffixRxBw);
			gBoxRxBw.Controls.Add(nudRxFilterBw);
			gBoxRxBw.Location = new Point(3, 3);
			gBoxRxBw.Name = "gBoxRxBw";
			gBoxRxBw.Size = new Size(0x11d, 0x5b);
			gBoxRxBw.TabIndex = 0;
			gBoxRxBw.TabStop = false;
			gBoxRxBw.Text = "Rx bandwidth";
			gBoxRxBw.MouseLeave += new EventHandler(control_MouseLeave);
			gBoxRxBw.MouseEnter += new EventHandler(control_MouseEnter);
			nudDccFreq.Location = new Point(0x7b, 0x1d);
			int[] numArray37 = new int[4];
			numArray37[0] = 0x679;
			nudDccFreq.Maximum = new decimal(numArray37);
			int[] numArray38 = new int[4];
			numArray38[0] = 12;
			nudDccFreq.Minimum = new decimal(numArray38);
			nudDccFreq.Name = "nudDccFreq";
			nudDccFreq.Size = new Size(0x7c, 20);
			nudDccFreq.TabIndex = 1;
			nudDccFreq.ThousandsSeparator = true;
			int[] numArray39 = new int[4];
			numArray39[0] = 0x19e;
			nudDccFreq.Value = new decimal(numArray39);
			nudDccFreq.ValueChanged += new EventHandler(nudDccFreq_ValueChanged);
			lblDcc.AutoSize = true;
			lblDcc.Location = new Point(6, 0x1f);
			lblDcc.Name = "lblDcc";
			lblDcc.Size = new Size(0x52, 13);
			lblDcc.TabIndex = 0;
			lblDcc.Text = "DCC frequency:";
			lblRxBw.AutoSize = true;
			lblRxBw.Location = new Point(6, 0x39);
			lblRxBw.Name = "lblRxBw";
			lblRxBw.Size = new Size(0x61, 13);
			lblRxBw.TabIndex = 3;
			lblRxBw.Text = "Rx filter bandwidth:";
			suffixDCC.AutoSize = true;
			suffixDCC.Location = new Point(0xfd, 0x21);
			suffixDCC.Name = "suffixDCC";
			suffixDCC.Size = new Size(20, 13);
			suffixDCC.TabIndex = 2;
			suffixDCC.Text = "Hz";
			suffixRxBw.AutoSize = true;
			suffixRxBw.Location = new Point(0xfd, 0x39);
			suffixRxBw.Name = "suffixRxBw";
			suffixRxBw.Size = new Size(20, 13);
			suffixRxBw.TabIndex = 5;
			suffixRxBw.Text = "Hz";
			nudRxFilterBw.Location = new Point(0x7b, 0x35);
			int[] numArray40 = new int[4];
			numArray40[0] = 0x7a120;
			nudRxFilterBw.Maximum = new decimal(numArray40);
			int[] numArray41 = new int[4];
			numArray41[0] = 0xf42;
			nudRxFilterBw.Minimum = new decimal(numArray41);
			nudRxFilterBw.Name = "nudRxFilterBw";
			nudRxFilterBw.Size = new Size(0x7c, 20);
			nudRxFilterBw.TabIndex = 4;
			nudRxFilterBw.ThousandsSeparator = true;
			int[] numArray42 = new int[4];
			numArray42[0] = 0x28b1;
			nudRxFilterBw.Value = new decimal(numArray42);
			nudRxFilterBw.ValueChanged += new EventHandler(nudRxFilterBw_ValueChanged);
			gBoxLna.Controls.Add(panel5);
			gBoxLna.Controls.Add(label13);
			gBoxLna.Controls.Add(label16);
			gBoxLna.Controls.Add(lblAgcReference);
			gBoxLna.Controls.Add(label48);
			gBoxLna.Controls.Add(label49);
			gBoxLna.Controls.Add(label50);
			gBoxLna.Controls.Add(label51);
			gBoxLna.Controls.Add(label52);
			gBoxLna.Controls.Add(lblLnaGain1);
			gBoxLna.Controls.Add(label53);
			gBoxLna.Controls.Add(panel6);
			gBoxLna.Controls.Add(lblLnaGain2);
			gBoxLna.Controls.Add(lblLnaGain3);
			gBoxLna.Controls.Add(lblLnaGain4);
			gBoxLna.Controls.Add(lblLnaGain5);
			gBoxLna.Controls.Add(lblLnaGain6);
			gBoxLna.Controls.Add(lblAgcThresh1);
			gBoxLna.Controls.Add(lblAgcThresh2);
			gBoxLna.Controls.Add(lblAgcThresh3);
			gBoxLna.Controls.Add(lblAgcThresh4);
			gBoxLna.Controls.Add(lblAgcThresh5);
			gBoxLna.Controls.Add(label47);
			gBoxLna.Location = new Point(3, 0x17a);
			gBoxLna.Name = "gBoxLna";
			gBoxLna.Size = new Size(0x319, 0x70);
			gBoxLna.TabIndex = 7;
			gBoxLna.TabStop = false;
			gBoxLna.Text = "Lna gain";
			gBoxLna.MouseLeave += new EventHandler(control_MouseLeave);
			gBoxLna.MouseEnter += new EventHandler(control_MouseEnter);
			panel5.AutoSize = true;
			panel5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			panel5.Controls.Add(rBtnLnaGainAutoOff);
			panel5.Controls.Add(rBtnLnaGainAutoOn);
			panel5.Location = new Point(0x36, 0x51);
			panel5.Name = "panel5";
			panel5.Size = new Size(0x77, 0x11);
			panel5.TabIndex = 0x15;
			rBtnLnaGainAutoOff.AutoSize = true;
			rBtnLnaGainAutoOff.Location = new Point(0x38, 0);
			rBtnLnaGainAutoOff.Margin = new Padding(3, 0, 3, 0);
			rBtnLnaGainAutoOff.Name = "rBtnLnaGainAutoOff";
			rBtnLnaGainAutoOff.Size = new Size(60, 0x11);
			rBtnLnaGainAutoOff.TabIndex = 1;
			rBtnLnaGainAutoOff.Text = "Manual";
			rBtnLnaGainAutoOff.UseVisualStyleBackColor = true;
			rBtnLnaGainAutoOff.CheckedChanged += new EventHandler(rBtnLnaGain_CheckedChanged);
			rBtnLnaGainAutoOn.AutoSize = true;
			rBtnLnaGainAutoOn.Checked = true;
			rBtnLnaGainAutoOn.Location = new Point(3, 0);
			rBtnLnaGainAutoOn.Margin = new Padding(3, 0, 3, 0);
			rBtnLnaGainAutoOn.Name = "rBtnLnaGainAutoOn";
			rBtnLnaGainAutoOn.Size = new Size(0x2f, 0x11);
			rBtnLnaGainAutoOn.TabIndex = 0;
			rBtnLnaGainAutoOn.TabStop = true;
			rBtnLnaGainAutoOn.Text = "Auto";
			rBtnLnaGainAutoOn.UseVisualStyleBackColor = true;
			rBtnLnaGainAutoOn.CheckedChanged += new EventHandler(rBtnLnaGain_CheckedChanged);
			label13.BackColor = Color.Transparent;
			label13.Location = new Point(0x4f, 40);
			label13.Name = "label13";
			label13.Size = new Size(0x2a, 13);
			label13.TabIndex = 6;
			label13.Text = "AGC";
			label13.TextAlign = ContentAlignment.MiddleCenter;
			label16.BackColor = Color.Transparent;
			label16.Location = new Point(0x13, 0x51);
			label16.Margin = new Padding(0, 0, 0, 3);
			label16.Name = "label16";
			label16.Size = new Size(0x20, 0x11);
			label16.TabIndex = 20;
			label16.Text = "Gain:";
			label16.TextAlign = ContentAlignment.MiddleLeft;
			lblAgcReference.BackColor = Color.Transparent;
			lblAgcReference.Location = new Point(110, 40);
			lblAgcReference.Margin = new Padding(0, 0, 0, 3);
			lblAgcReference.Name = "lblAgcReference";
			lblAgcReference.Size = new Size(100, 13);
			lblAgcReference.TabIndex = 7;
			lblAgcReference.Text = "-80";
			lblAgcReference.TextAlign = ContentAlignment.MiddleCenter;
			label48.BackColor = Color.Transparent;
			label48.Location = new Point(110, 0x18);
			label48.Margin = new Padding(0, 0, 0, 3);
			label48.Name = "label48";
			label48.Size = new Size(100, 13);
			label48.TabIndex = 0;
			label48.Text = "Reference";
			label48.TextAlign = ContentAlignment.MiddleCenter;
			label49.BackColor = Color.Transparent;
			label49.Location = new Point(210, 0x18);
			label49.Margin = new Padding(0, 0, 0, 3);
			label49.Name = "label49";
			label49.Size = new Size(100, 13);
			label49.TabIndex = 1;
			label49.Text = "Threshold 1";
			label49.TextAlign = ContentAlignment.MiddleCenter;
			label50.BackColor = Color.Transparent;
			label50.Location = new Point(310, 0x18);
			label50.Margin = new Padding(0, 0, 0, 3);
			label50.Name = "label50";
			label50.Size = new Size(100, 13);
			label50.TabIndex = 2;
			label50.Text = "Threshold 2";
			label50.TextAlign = ContentAlignment.MiddleCenter;
			label51.BackColor = Color.Transparent;
			label51.Location = new Point(410, 0x18);
			label51.Margin = new Padding(0, 0, 0, 3);
			label51.Name = "label51";
			label51.Size = new Size(100, 13);
			label51.TabIndex = 3;
			label51.Text = "Threshold 3";
			label51.TextAlign = ContentAlignment.MiddleCenter;
			label52.BackColor = Color.Transparent;
			label52.Location = new Point(510, 0x18);
			label52.Margin = new Padding(0, 0, 0, 3);
			label52.Name = "label52";
			label52.Size = new Size(100, 13);
			label52.TabIndex = 4;
			label52.Text = "Threshold 4";
			label52.TextAlign = ContentAlignment.MiddleCenter;
			lblLnaGain1.BackColor = Color.LightSteelBlue;
			lblLnaGain1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			lblLnaGain1.Location = new Point(160, 0x38);
			lblLnaGain1.Margin = new Padding(0, 0, 0, 3);
			lblLnaGain1.Name = "lblLnaGain1";
			lblLnaGain1.Size = new Size(100, 20);
			lblLnaGain1.TabIndex = 14;
			lblLnaGain1.Text = "G1";
			lblLnaGain1.TextAlign = ContentAlignment.MiddleCenter;
			label53.BackColor = Color.Transparent;
			label53.Location = new Point(610, 0x18);
			label53.Margin = new Padding(0, 0, 0, 3);
			label53.Name = "label53";
			label53.Size = new Size(100, 13);
			label53.TabIndex = 5;
			label53.Text = "Threshold 5";
			label53.TextAlign = ContentAlignment.MiddleCenter;
			panel6.AutoSize = true;
			panel6.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			panel6.Controls.Add(rBtnLnaGain1);
			panel6.Controls.Add(rBtnLnaGain2);
			panel6.Controls.Add(rBtnLnaGain3);
			panel6.Controls.Add(rBtnLnaGain4);
			panel6.Controls.Add(rBtnLnaGain5);
			panel6.Controls.Add(rBtnLnaGain6);
			panel6.Location = new Point(0xc9, 0x53);
			panel6.Name = "panel6";
			panel6.Size = new Size(0x209, 13);
			panel6.TabIndex = 0x16;
			rBtnLnaGain1.AutoSize = true;
			rBtnLnaGain1.Checked = true;
			rBtnLnaGain1.Location = new Point(3, 0);
			rBtnLnaGain1.Margin = new Padding(3, 0, 3, 0);
			rBtnLnaGain1.Name = "rBtnLnaGain1";
			rBtnLnaGain1.Size = new Size(14, 13);
			rBtnLnaGain1.TabIndex = 0;
			rBtnLnaGain1.TabStop = true;
			rBtnLnaGain1.UseVisualStyleBackColor = true;
			rBtnLnaGain1.CheckedChanged += new EventHandler(rBtnLnaGain_CheckedChanged);
			rBtnLnaGain2.AutoSize = true;
			rBtnLnaGain2.Location = new Point(0x66, 0);
			rBtnLnaGain2.Margin = new Padding(3, 0, 3, 0);
			rBtnLnaGain2.Name = "rBtnLnaGain2";
			rBtnLnaGain2.Size = new Size(14, 13);
			rBtnLnaGain2.TabIndex = 1;
			rBtnLnaGain2.UseVisualStyleBackColor = true;
			rBtnLnaGain2.CheckedChanged += new EventHandler(rBtnLnaGain_CheckedChanged);
			rBtnLnaGain3.AutoSize = true;
			rBtnLnaGain3.Location = new Point(0xcb, 0);
			rBtnLnaGain3.Margin = new Padding(3, 0, 3, 0);
			rBtnLnaGain3.Name = "rBtnLnaGain3";
			rBtnLnaGain3.Size = new Size(14, 13);
			rBtnLnaGain3.TabIndex = 2;
			rBtnLnaGain3.UseVisualStyleBackColor = true;
			rBtnLnaGain3.CheckedChanged += new EventHandler(rBtnLnaGain_CheckedChanged);
			rBtnLnaGain4.AutoSize = true;
			rBtnLnaGain4.Location = new Point(0x12f, 0);
			rBtnLnaGain4.Margin = new Padding(3, 0, 3, 0);
			rBtnLnaGain4.Name = "rBtnLnaGain4";
			rBtnLnaGain4.Size = new Size(14, 13);
			rBtnLnaGain4.TabIndex = 3;
			rBtnLnaGain4.UseVisualStyleBackColor = true;
			rBtnLnaGain4.CheckedChanged += new EventHandler(rBtnLnaGain_CheckedChanged);
			rBtnLnaGain5.AutoSize = true;
			rBtnLnaGain5.Location = new Point(0x194, 0);
			rBtnLnaGain5.Margin = new Padding(3, 0, 3, 0);
			rBtnLnaGain5.Name = "rBtnLnaGain5";
			rBtnLnaGain5.Size = new Size(14, 13);
			rBtnLnaGain5.TabIndex = 4;
			rBtnLnaGain5.UseVisualStyleBackColor = true;
			rBtnLnaGain5.CheckedChanged += new EventHandler(rBtnLnaGain_CheckedChanged);
			rBtnLnaGain6.AutoSize = true;
			rBtnLnaGain6.Location = new Point(0x1f8, 0);
			rBtnLnaGain6.Margin = new Padding(3, 0, 3, 0);
			rBtnLnaGain6.Name = "rBtnLnaGain6";
			rBtnLnaGain6.Size = new Size(14, 13);
			rBtnLnaGain6.TabIndex = 5;
			rBtnLnaGain6.UseVisualStyleBackColor = true;
			rBtnLnaGain6.CheckedChanged += new EventHandler(rBtnLnaGain_CheckedChanged);
			lblLnaGain2.BackColor = Color.Transparent;
			lblLnaGain2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			lblLnaGain2.Location = new Point(260, 0x38);
			lblLnaGain2.Margin = new Padding(0, 0, 0, 3);
			lblLnaGain2.Name = "lblLnaGain2";
			lblLnaGain2.Size = new Size(100, 20);
			lblLnaGain2.TabIndex = 15;
			lblLnaGain2.Text = "G2";
			lblLnaGain2.TextAlign = ContentAlignment.MiddleCenter;
			lblLnaGain3.BackColor = Color.Transparent;
			lblLnaGain3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			lblLnaGain3.Location = new Point(360, 0x38);
			lblLnaGain3.Margin = new Padding(0, 0, 0, 3);
			lblLnaGain3.Name = "lblLnaGain3";
			lblLnaGain3.Size = new Size(100, 20);
			lblLnaGain3.TabIndex = 0x10;
			lblLnaGain3.Text = "G3";
			lblLnaGain3.TextAlign = ContentAlignment.MiddleCenter;
			lblLnaGain4.BackColor = Color.Transparent;
			lblLnaGain4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			lblLnaGain4.Location = new Point(460, 0x38);
			lblLnaGain4.Margin = new Padding(0, 0, 0, 3);
			lblLnaGain4.Name = "lblLnaGain4";
			lblLnaGain4.Size = new Size(100, 20);
			lblLnaGain4.TabIndex = 0x11;
			lblLnaGain4.Text = "G4";
			lblLnaGain4.TextAlign = ContentAlignment.MiddleCenter;
			lblLnaGain5.BackColor = Color.Transparent;
			lblLnaGain5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			lblLnaGain5.Location = new Point(560, 0x38);
			lblLnaGain5.Margin = new Padding(0, 0, 0, 3);
			lblLnaGain5.Name = "lblLnaGain5";
			lblLnaGain5.Size = new Size(100, 20);
			lblLnaGain5.TabIndex = 0x12;
			lblLnaGain5.Text = "G5";
			lblLnaGain5.TextAlign = ContentAlignment.MiddleCenter;
			lblLnaGain6.BackColor = Color.Transparent;
			lblLnaGain6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			lblLnaGain6.Location = new Point(660, 0x38);
			lblLnaGain6.Margin = new Padding(0, 0, 0, 3);
			lblLnaGain6.Name = "lblLnaGain6";
			lblLnaGain6.Size = new Size(100, 20);
			lblLnaGain6.TabIndex = 0x13;
			lblLnaGain6.Text = "G6";
			lblLnaGain6.TextAlign = ContentAlignment.MiddleCenter;
			lblAgcThresh1.BackColor = Color.Transparent;
			lblAgcThresh1.Location = new Point(210, 40);
			lblAgcThresh1.Margin = new Padding(0, 0, 0, 3);
			lblAgcThresh1.Name = "lblAgcThresh1";
			lblAgcThresh1.Size = new Size(100, 13);
			lblAgcThresh1.TabIndex = 8;
			lblAgcThresh1.Text = "0";
			lblAgcThresh1.TextAlign = ContentAlignment.MiddleCenter;
			lblAgcThresh2.BackColor = Color.Transparent;
			lblAgcThresh2.Location = new Point(310, 40);
			lblAgcThresh2.Margin = new Padding(0, 0, 0, 3);
			lblAgcThresh2.Name = "lblAgcThresh2";
			lblAgcThresh2.Size = new Size(100, 13);
			lblAgcThresh2.TabIndex = 9;
			lblAgcThresh2.Text = "0";
			lblAgcThresh2.TextAlign = ContentAlignment.MiddleCenter;
			lblAgcThresh3.BackColor = Color.Transparent;
			lblAgcThresh3.Location = new Point(410, 40);
			lblAgcThresh3.Margin = new Padding(0, 0, 0, 3);
			lblAgcThresh3.Name = "lblAgcThresh3";
			lblAgcThresh3.Size = new Size(100, 13);
			lblAgcThresh3.TabIndex = 10;
			lblAgcThresh3.Text = "0";
			lblAgcThresh3.TextAlign = ContentAlignment.MiddleCenter;
			lblAgcThresh4.BackColor = Color.Transparent;
			lblAgcThresh4.Location = new Point(510, 40);
			lblAgcThresh4.Margin = new Padding(0, 0, 0, 3);
			lblAgcThresh4.Name = "lblAgcThresh4";
			lblAgcThresh4.Size = new Size(100, 13);
			lblAgcThresh4.TabIndex = 11;
			lblAgcThresh4.Text = "0";
			lblAgcThresh4.TextAlign = ContentAlignment.MiddleCenter;
			lblAgcThresh5.BackColor = Color.Transparent;
			lblAgcThresh5.Location = new Point(610, 40);
			lblAgcThresh5.Margin = new Padding(0, 0, 0, 3);
			lblAgcThresh5.Name = "lblAgcThresh5";
			lblAgcThresh5.Size = new Size(100, 13);
			lblAgcThresh5.TabIndex = 12;
			lblAgcThresh5.Text = "0";
			lblAgcThresh5.TextAlign = ContentAlignment.MiddleCenter;
			label47.AutoSize = true;
			label47.BackColor = Color.Transparent;
			label47.Location = new Point(0x2c5, 40);
			label47.Margin = new Padding(0);
			label47.Name = "label47";
			label47.Size = new Size(0x40, 13);
			label47.TabIndex = 13;
			label47.Text = "-> Pin [dBm]";
			label47.TextAlign = ContentAlignment.MiddleLeft;
			gBoxDagc.Controls.Add(label34);
			gBoxDagc.Controls.Add(panel11);
			gBoxDagc.Location = new Point(0x249, 0x79);
			gBoxDagc.Name = "gBoxDagc";
			gBoxDagc.Size = new Size(0xd3, 50);
			gBoxDagc.TabIndex = 5;
			gBoxDagc.TabStop = false;
			gBoxDagc.Text = "DAGC";
			gBoxDagc.MouseLeave += new EventHandler(control_MouseLeave);
			gBoxDagc.MouseEnter += new EventHandler(control_MouseEnter);
			label34.AutoSize = true;
			label34.Location = new Point(11, 0x15);
			label34.Name = "label34";
			label34.Size = new Size(40, 13);
			label34.TabIndex = 3;
			label34.Text = "DAGC:";
			panel11.AutoSize = true;
			panel11.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			panel11.Controls.Add(rBtnDagcOff);
			panel11.Controls.Add(rBtnDagcOn);
			panel11.Location = new Point(0x6a, 0x13);
			panel11.Name = "panel11";
			panel11.Size = new Size(0x5d, 0x11);
			panel11.TabIndex = 4;
			rBtnDagcOff.AutoSize = true;
			rBtnDagcOff.Location = new Point(0x2d, 0);
			rBtnDagcOff.Margin = new Padding(3, 0, 3, 0);
			rBtnDagcOff.Name = "rBtnDagcOff";
			rBtnDagcOff.Size = new Size(0x2d, 0x11);
			rBtnDagcOff.TabIndex = 1;
			rBtnDagcOff.Text = "OFF";
			rBtnDagcOff.UseVisualStyleBackColor = true;
			rBtnDagcOff.CheckedChanged += new EventHandler(rBtnDagc_CheckedChanged);
			rBtnDagcOn.AutoSize = true;
			rBtnDagcOn.Checked = true;
			rBtnDagcOn.Location = new Point(3, 0);
			rBtnDagcOn.Margin = new Padding(3, 0, 3, 0);
			rBtnDagcOn.Name = "rBtnDagcOn";
			rBtnDagcOn.Size = new Size(0x29, 0x11);
			rBtnDagcOn.TabIndex = 0;
			rBtnDagcOn.TabStop = true;
			rBtnDagcOn.Text = "ON";
			rBtnDagcOn.UseVisualStyleBackColor = true;
			rBtnDagcOn.CheckedChanged += new EventHandler(rBtnDagc_CheckedChanged);
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(gBoxDagc);
			base.Controls.Add(gBoxLnaSensitivity);
			base.Controls.Add(gBoxAgc);
			base.Controls.Add(gBoxRssi);
			base.Controls.Add(gBoxAfcFei);
			base.Controls.Add(gBoxOok);
			base.Controls.Add(gBoxAfcBw);
			base.Controls.Add(gBoxRxBw);
			base.Controls.Add(gBoxLna);
			base.Name = "ReceiverViewControl";
			base.Size = new Size(0x31f, 0x1ed);
			((ISupportInitialize)errorProvider).EndInit();
			panel3.ResumeLayout(false);
			panel3.PerformLayout();
			panel4.ResumeLayout(false);
			panel4.PerformLayout();
			pnlSensitivityBoost.ResumeLayout(false);
			pnlSensitivityBoost.PerformLayout();
			gBoxLnaSensitivity.ResumeLayout(false);
			gBoxLnaSensitivity.PerformLayout();
			gBoxAgc.ResumeLayout(false);
			gBoxAgc.PerformLayout();
			panel2.ResumeLayout(false);
			panel2.PerformLayout();
			nudAgcStep5.EndInit();
			nudAgcSnrMargin.EndInit();
			nudAgcStep4.EndInit();
			nudAgcRefLevel.EndInit();
			nudAgcStep3.EndInit();
			nudAgcStep1.EndInit();
			nudAgcStep2.EndInit();
			gBoxRssi.ResumeLayout(false);
			gBoxRssi.PerformLayout();
			pnlRssiPhase.ResumeLayout(false);
			pnlRssiPhase.PerformLayout();
			panel7.ResumeLayout(false);
			panel7.PerformLayout();
			nudRssiThresh.EndInit();
			panel1.ResumeLayout(false);
			panel1.PerformLayout();
			nudTimeoutRxStart.EndInit();
			nudTimeoutRssiThresh.EndInit();
			gBoxAfcFei.ResumeLayout(false);
			gBoxAfcFei.PerformLayout();
			nudLowBetaAfcOffset.EndInit();
			pnlAfcLowBeta.ResumeLayout(false);
			pnlAfcLowBeta.PerformLayout();
			panel8.ResumeLayout(false);
			panel8.PerformLayout();
			panel9.ResumeLayout(false);
			panel9.PerformLayout();
			gBoxOok.ResumeLayout(false);
			gBoxOok.PerformLayout();
			nudOokPeakThreshStep.EndInit();
			nudOokFixedThresh.EndInit();
			gBoxAfcBw.ResumeLayout(false);
			gBoxAfcBw.PerformLayout();
			nudAfcDccFreq.EndInit();
			nudRxFilterBwAfc.EndInit();
			gBoxRxBw.ResumeLayout(false);
			gBoxRxBw.PerformLayout();
			nudDccFreq.EndInit();
			nudRxFilterBw.EndInit();
			gBoxLna.ResumeLayout(false);
			gBoxLna.PerformLayout();
			panel5.ResumeLayout(false);
			panel5.PerformLayout();
			panel6.ResumeLayout(false);
			panel6.PerformLayout();
			gBoxDagc.ResumeLayout(false);
			gBoxDagc.PerformLayout();
			panel11.ResumeLayout(false);
			panel11.PerformLayout();
			base.ResumeLayout(false);
		}

		private void nudAfcDccFreq_ValueChanged(object sender, EventArgs e)
		{
			int num = (int)((Math.Log10((double)((4.0M * AfcRxBw) / (6.283185307179580M * AfcDccFreq))) / Math.Log10(2.0)) - 2.0);
			int num2 = (int)((Math.Log10((double)((4.0M * AfcRxBw) / (6.283185307179580M * nudAfcDccFreq.Value))) / Math.Log10(2.0)) - 2.0);
			int num3 = (int)(nudAfcDccFreq.Value - AfcDccFreq);
			if (((num3 >= -1) && (num3 <= 1)) && (num == num2))
			{
				nudAfcDccFreq.ValueChanged -= new EventHandler(nudAfcDccFreq_ValueChanged);
				nudAfcDccFreq.Value = (4.0M * AfcRxBw) / (6.283185307179580M * ((decimal)Math.Pow(2.0, (double)((num2 - num3) + 2))));
				nudAfcDccFreq.ValueChanged += new EventHandler(nudAfcDccFreq_ValueChanged);
			}
			AfcDccFreq = nudAfcDccFreq.Value;
			OnAfcDccFreqChanged(AfcDccFreq);
		}

		private void nudAgcRefLevel_ValueChanged(object sender, EventArgs e)
		{
			AgcRefLevel = (int)nudAgcRefLevel.Value;
			OnAgcRefLevelChanged(AgcRefLevel);
		}

		private void nudAgcSnrMargin_ValueChanged(object sender, EventArgs e)
		{
			AgcSnrMargin = (byte)nudAgcSnrMargin.Value;
			OnAgcSnrMarginChanged(AgcSnrMargin);
		}

		private void nudAgcStep_ValueChanged(object sender, EventArgs e)
		{
			byte num = 0;
			byte id = 0;
			if (sender == nudAgcStep1)
			{
				num = AgcStep1 = (byte)nudAgcStep1.Value;
				id = 1;
			}
			else if (sender == nudAgcStep2)
			{
				num = AgcStep2 = (byte)nudAgcStep2.Value;
				id = 2;
			}
			else if (sender == nudAgcStep3)
			{
				num = AgcStep3 = (byte)nudAgcStep3.Value;
				id = 3;
			}
			else if (sender == nudAgcStep4)
			{
				num = AgcStep4 = (byte)nudAgcStep4.Value;
				id = 4;
			}
			else if (sender == nudAgcStep5)
			{
				num = AgcStep5 = (byte)nudAgcStep5.Value;
				id = 5;
			}
			OnAgcStepChanged(id, num);
		}

		private void nudDccFreq_ValueChanged(object sender, EventArgs e)
		{
			int num = (int)((Math.Log10((double)((4.0M * RxBw) / (6.283185307179580M * DccFreq))) / Math.Log10(2.0)) - 2.0);
			int num2 = (int)((Math.Log10((double)((4.0M * RxBw) / (6.283185307179580M * nudDccFreq.Value))) / Math.Log10(2.0)) - 2.0);
			int num3 = (int)(nudDccFreq.Value - DccFreq);
			if (((num3 >= -1) && (num3 <= 1)) && (num == num2))
			{
				nudDccFreq.ValueChanged -= new EventHandler(nudDccFreq_ValueChanged);
				nudDccFreq.Value = (4.0M * RxBw) / (6.283185307179580M * ((decimal)Math.Pow(2.0, (double)((num2 - num3) + 2))));
				nudDccFreq.ValueChanged += new EventHandler(nudDccFreq_ValueChanged);
			}
			DccFreq = nudDccFreq.Value;
			OnDccFreqChanged(DccFreq);
		}

		private void nudLowBetaAfcOffset_ValueChanged(object sender, EventArgs e)
		{
			int num1 = (int)Math.Round((decimal)(LowBetaAfcOffset / 488.0M), MidpointRounding.AwayFromZero);
			int num = (int)Math.Round((decimal)(nudLowBetaAfcOffset.Value / 488.0M), MidpointRounding.AwayFromZero);
			int num2 = (int)(nudLowBetaAfcOffset.Value - LowBetaAfcOffset);
			nudLowBetaAfcOffset.ValueChanged -= new EventHandler(nudLowBetaAfcOffset_ValueChanged);
			if ((num2 >= -1) && (num2 <= 1))
			{
				nudLowBetaAfcOffset.Value = (num - num2) * 0x1e8;
			}
			else
			{
				nudLowBetaAfcOffset.Value = num * 0x1e8;
			}
			nudLowBetaAfcOffset.ValueChanged += new EventHandler(nudLowBetaAfcOffset_ValueChanged);
			LowBetaAfcOffset = nudLowBetaAfcOffset.Value;
			OnLowBetaAfcOffsetChanged(LowBetaAfcOffset);
		}

		private void nudOokFixedThresh_ValueChanged(object sender, EventArgs e)
		{
			OokFixedThresh = (byte)nudOokFixedThresh.Value;
			OnOokFixedThreshChanged(OokFixedThresh);
		}

		private void nudOokPeakThreshStep_Validating(object sender, CancelEventArgs e)
		{
			bool flag1 = nudOokPeakThreshStep.Value < 2M;
		}

		private void nudOokPeakThreshStep_ValueChanged(object sender, EventArgs e)
		{
			try
			{
				nudOokPeakThreshStep.ValueChanged -= new EventHandler(nudOokPeakThreshStep_ValueChanged);
				decimal[] array = new decimal[] { 0.5M, 1.0M, 1.5M, 2.0M, 3.0M, 4.0M, 5.0M, 6.0M };
				int index = 0;
				decimal num2 = nudOokPeakThreshStep.Value - OokPeakThreshStep;
				decimal num3 = 10000000M;
				for (int i = 0; i < 8; i++)
				{
					if (Math.Abs((decimal)(nudOokPeakThreshStep.Value - array[i])) < num3)
					{
						num3 = Math.Abs((decimal)(nudOokPeakThreshStep.Value - array[i]));
						index = i;
					}
				}
				if (((num3 / Math.Abs(num2)) == 1M) && (num2 >= 0.5M))
				{
					if (num2 > 0M)
					{
						nudOokPeakThreshStep.Value += nudOokPeakThreshStep.Increment;
					}
					else
					{
						nudOokPeakThreshStep.Value -= nudOokPeakThreshStep.Increment;
					}
					index = Array.IndexOf<decimal>(array, nudOokPeakThreshStep.Value);
				}
				nudOokPeakThreshStep.Value = array[index];
				nudOokPeakThreshStep.ValueChanged += new EventHandler(nudOokPeakThreshStep_ValueChanged);
				OokPeakThreshStep = nudOokPeakThreshStep.Value;
				OnOokPeakThreshStepChanged(OokPeakThreshStep);
			}
			catch
			{
				nudOokPeakThreshStep.ValueChanged += new EventHandler(nudOokPeakThreshStep_ValueChanged);
			}
		}

		private void nudRssiThresh_ValueChanged(object sender, EventArgs e)
		{
			RssiThresh = nudRssiThresh.Value;
			OnRssiThreshChanged(RssiThresh);
		}

		private void nudRxFilterBw_ValueChanged(object sender, EventArgs e)
		{
			decimal[] array = SemtechLib.Devices.SX1231.SX1231.ComputeRxBwFreqTable(frequencyXo, modulationType);
			int index = 0;
			int num2 = (int)(nudRxFilterBw.Value - RxBw);
			if ((num2 >= -1) && (num2 <= 1))
				index = Array.IndexOf<decimal>(array, RxBw) - num2;
			else
			{
				int mant = 0;
				int exp = 0;
				decimal num5 = 0M;
				SemtechLib.Devices.SX1231.SX1231.ComputeRxBwMantExp(frequencyXo, ModulationType, nudRxFilterBw.Value, ref mant, ref exp);
				num5 = SemtechLib.Devices.SX1231.SX1231.ComputeRxBw(frequencyXo, ModulationType, mant, exp);
				index = Array.IndexOf<decimal>(array, num5);
			}
			nudRxFilterBw.ValueChanged -= new EventHandler(nudRxFilterBw_ValueChanged);
			nudRxFilterBw.Value = array[index];
			nudRxFilterBw.ValueChanged += new EventHandler(nudRxFilterBw_ValueChanged);
			RxBw = nudRxFilterBw.Value;
			OnRxBwChanged(RxBw);
		}

		private void nudRxFilterBwAfc_ValueChanged(object sender, EventArgs e)
		{
			decimal[] array = SemtechLib.Devices.SX1231.SX1231.ComputeRxBwFreqTable(frequencyXo, modulationType);
			int index = 0;
			int num2 = (int)(nudRxFilterBwAfc.Value - AfcRxBw);
			if ((num2 >= -1) && (num2 <= 1))
				index = Array.IndexOf<decimal>(array, AfcRxBw) - num2;
			else
			{
				int mant = 0;
				int exp = 0;
				decimal num5 = 0M;
				SemtechLib.Devices.SX1231.SX1231.ComputeRxBwMantExp(frequencyXo, ModulationType, nudRxFilterBwAfc.Value, ref mant, ref exp);
				num5 = SemtechLib.Devices.SX1231.SX1231.ComputeRxBw(frequencyXo, ModulationType, mant, exp);
				index = Array.IndexOf<decimal>(array, num5);
			}
			nudRxFilterBwAfc.ValueChanged -= new EventHandler(nudRxFilterBwAfc_ValueChanged);
			nudRxFilterBwAfc.Value = array[index];
			nudRxFilterBwAfc.ValueChanged += new EventHandler(nudRxFilterBwAfc_ValueChanged);
			AfcRxBw = nudRxFilterBwAfc.Value;
			OnAfcRxBwChanged(AfcRxBw);
		}

		private void nudTimeoutRssiThresh_ValueChanged(object sender, EventArgs e)
		{
			TimeoutRssiThresh = nudTimeoutRssiThresh.Value;
			OnTimeoutRssiThreshChanged(TimeoutRssiThresh);
		}

		private void nudTimeoutRxStart_ValueChanged(object sender, EventArgs e)
		{
			TimeoutRxStart = nudTimeoutRxStart.Value;
			OnTimeoutRxStartChanged(TimeoutRxStart);
		}

		private void OnAfcAutoClearOnChanged(bool value)
		{
			if (AfcAutoClearOnChanged != null)
				AfcAutoClearOnChanged(this, new BooleanEventArg(value));
		}

		private void OnAfcAutoOnChanged(bool value)
		{
			if (AfcAutoOnChanged != null)
				AfcAutoOnChanged(this, new BooleanEventArg(value));
		}

		private void OnAfcClearChanged()
		{
			if (AfcClearChanged != null)
				AfcClearChanged(this, EventArgs.Empty);
		}

		private void OnAfcDccFreqChanged(decimal value)
		{
			if (AfcDccFreqChanged != null)
				AfcDccFreqChanged(this, new DecimalEventArg(value));
		}

		private void OnAfcLowBetaOnChanged(bool value)
		{
			if (AfcLowBetaOnChanged != null)
				AfcLowBetaOnChanged(this, new BooleanEventArg(value));
		}

		private void OnAfcRxBwChanged(decimal value)
		{
			if (AfcRxBwChanged != null)
				AfcRxBwChanged(this, new DecimalEventArg(value));
		}

		private void OnAfcStartChanged()
		{
			if (AfcStartChanged != null)
				AfcStartChanged(this, EventArgs.Empty);
		}

		private void OnAgcAutoRefChanged(bool value)
		{
			if (AgcAutoRefChanged != null)
				AgcAutoRefChanged(this, new BooleanEventArg(value));
		}

		private void OnAgcRefLevelChanged(int value)
		{
			if (AgcRefLevelChanged != null)
				AgcRefLevelChanged(this, new Int32EventArg(value));
		}

		private void OnAgcSnrMarginChanged(byte value)
		{
			if (AgcSnrMarginChanged != null)
				AgcSnrMarginChanged(this, new ByteEventArg(value));
		}

		private void OnAgcStepChanged(byte id, byte value)
		{
			if (AgcStepChanged != null)
				AgcStepChanged(this, new AgcStepEventArg(id, value));
		}

		private void OnAutoRxRestartOnChanged(bool value)
		{
			if (AutoRxRestartOnChanged != null)
				AutoRxRestartOnChanged(this, new BooleanEventArg(value));
		}

		private void OnDagcOnChanged(bool value)
		{
			if (DagcOnChanged != null)
				DagcOnChanged(this, new BooleanEventArg(value));
		}

		private void OnDccFreqChanged(decimal value)
		{
			if (DccFreqChanged != null)
				DccFreqChanged(this, new DecimalEventArg(value));
		}

		private void OnDocumentationChanged(DocumentationChangedEventArgs e)
		{
			if (DocumentationChanged != null)
				DocumentationChanged(this, e);
		}

		private void OnFastRxChanged(bool value)
		{
			if (FastRxChanged != null)
				FastRxChanged(this, new BooleanEventArg(value));
		}

		private void OnFeiStartChanged()
		{
			if (FeiStartChanged != null)
				FeiStartChanged(this, EventArgs.Empty);
		}

		private void OnLnaGainChanged(LnaGainEnum value)
		{
			if (LnaGainChanged != null)
				LnaGainChanged(this, new LnaGainEventArg(value));
		}

		private void OnLnaLowPowerOnChanged(bool value)
		{
			if (LnaLowPowerOnChanged != null)
				LnaLowPowerOnChanged(this, new BooleanEventArg(value));
		}

		private void OnLnaZinChanged(LnaZinEnum value)
		{
			if (LnaZinChanged != null)
				LnaZinChanged(this, new LnaZinEventArg(value));
		}

		private void OnLowBetaAfcOffsetChanged(decimal value)
		{
			if (LowBetaAfcOffsetChanged != null)
				LowBetaAfcOffsetChanged(this, new DecimalEventArg(value));
		}

		private void OnOokAverageThreshFiltChanged(OokAverageThreshFiltEnum value)
		{
			if (OokAverageThreshFiltChanged != null)
				OokAverageThreshFiltChanged(this, new OokAverageThreshFiltEventArg(value));
		}

		private void OnOokFixedThreshChanged(byte value)
		{
			if (OokFixedThreshChanged != null)
				OokFixedThreshChanged(this, new ByteEventArg(value));
		}

		private void OnOokPeakThreshDecChanged(OokPeakThreshDecEnum value)
		{
			if (OokPeakThreshDecChanged != null)
				OokPeakThreshDecChanged(this, new OokPeakThreshDecEventArg(value));
		}

		private void OnOokPeakThreshStepChanged(decimal value)
		{
			if (OokPeakThreshStepChanged != null)
				OokPeakThreshStepChanged(this, new DecimalEventArg(value));
		}

		private void OnOokThreshTypeChanged(OokThreshTypeEnum value)
		{
			if (OokThreshTypeChanged != null)
			{
				OokThreshTypeChanged(this, new OokThreshTypeEventArg(value));
			}
		}

		private void OnRestartRxChanged()
		{
			if (RestartRxChanged != null)
				RestartRxChanged(this, EventArgs.Empty);
		}

		private void OnRssiAutoThreshChanged(bool value)
		{
			if (RssiAutoThreshChanged != null)
				RssiAutoThreshChanged(this, new BooleanEventArg(value));
		}

		private void OnRssiStartChanged()
		{
			if (RssiStartChanged != null)
				RssiStartChanged(this, EventArgs.Empty);
		}

		private void OnRssiThreshChanged(decimal value)
		{
			if (RssiThreshChanged != null)
				RssiThreshChanged(this, new DecimalEventArg(value));
		}

		private void OnRxBwChanged(decimal value)
		{
			if (RxBwChanged != null)
				RxBwChanged(this, new DecimalEventArg(value));
		}

		private void OnSensitivityBoostOnChanged(bool value)
		{
			if (SensitivityBoostOnChanged != null)
				SensitivityBoostOnChanged(this, new BooleanEventArg(value));
		}

		private void OnTimeoutRssiThreshChanged(decimal value)
		{
			if (TimeoutRssiThreshChanged != null)
				TimeoutRssiThreshChanged(this, new DecimalEventArg(value));
		}

		private void OnTimeoutRxStartChanged(decimal value)
		{
			if (TimeoutRxStartChanged != null)
				TimeoutRxStartChanged(this, new DecimalEventArg(value));
		}

		private void rBtnAfcAutoClearOn_CheckedChanged(object sender, EventArgs e)
		{
			AfcAutoClearOn = rBtnAfcAutoClearOn.Checked;
			OnAfcAutoClearOnChanged(AfcAutoClearOn);
		}

		private void rBtnAfcAutoOn_CheckedChanged(object sender, EventArgs e)
		{
			AfcAutoOn = rBtnAfcAutoOn.Checked;
			OnAfcAutoOnChanged(AfcAutoOn);
		}

		private void rBtnAfcLowBeta_CheckedChanged(object sender, EventArgs e)
		{
			AfcLowBetaOn = rBtnAfcLowBetaOn.Checked;
			OnAfcLowBetaOnChanged(AfcLowBetaOn);
		}

		private void rBtnAgcAutoRef_CheckedChanged(object sender, EventArgs e)
		{
			AgcAutoRefOn = rBtnAgcAutoRefOn.Checked;
			OnAgcAutoRefChanged(AgcAutoRefOn);
		}

		private void rBtnDagc_CheckedChanged(object sender, EventArgs e)
		{
			DagcOn = rBtnDagcOn.Checked;
			OnDagcOnChanged(DagcOn);
		}

		private void rBtnFastRx_CheckedChanged(object sender, EventArgs e)
		{
			FastRx = rBtnFastRxOn.Checked;
			OnFastRxChanged(FastRx);
		}

		private void rBtnLnaGain_CheckedChanged(object sender, EventArgs e)
		{
			if (rBtnLnaGainAutoOn.Checked)
				LnaGainSelect = LnaGainEnum.AGC;
			else if (rBtnLnaGain1.Checked)
				LnaGainSelect = LnaGainEnum.G1;
			else if (rBtnLnaGain2.Checked)
				LnaGainSelect = LnaGainEnum.G2;
			else if (rBtnLnaGain3.Checked)
				LnaGainSelect = LnaGainEnum.G3;
			else if (rBtnLnaGain4.Checked)
				LnaGainSelect = LnaGainEnum.G4;
			else if (rBtnLnaGain5.Checked)
				LnaGainSelect = LnaGainEnum.G5;
			else if (rBtnLnaGain6.Checked)
				LnaGainSelect = LnaGainEnum.G6;
			else
				LnaGainSelect = LnaGainEnum.AGC;
			OnLnaGainChanged(LnaGainSelect);
		}

		private void rBtnLnaLowPower_CheckedChanged(object sender, EventArgs e)
		{
			LnaLowPowerOn = rBtnLnaLowPowerOn.Checked;
			OnLnaLowPowerOnChanged(LnaLowPowerOn);
		}

		private void rBtnLnaZin_CheckedChanged(object sender, EventArgs e)
		{
			LnaZin = rBtnLnaZin50.Checked ? LnaZinEnum.ZIN_50 : LnaZinEnum.ZIN_200;
			OnLnaZinChanged(LnaZin);
		}

		private void rBtnRssiAutoThreshOn_CheckedChanged(object sender, EventArgs e)
		{
			RssiThresh = nudRssiThresh.Value;
			OnRssiThreshChanged(RssiThresh);
			RssiAutoThresh = rBtnRssiAutoThreshOn.Checked;
			OnRssiAutoThreshChanged(RssiAutoThresh);
		}

		private void rBtnRssiPhaseAuto_CheckedChanged(object sender, EventArgs e)
		{
			if (rBtnRssiPhaseAuto.Checked)
			{
				AutoRxRestartOn = rBtnRssiPhaseAuto.Checked;
				OnAutoRxRestartOnChanged(AutoRxRestartOn);
			}
		}

		private void rBtnRssiPhaseManual_CheckedChanged(object sender, EventArgs e)
		{
			if (rBtnRssiPhaseManual.Checked)
			{
				AutoRxRestartOn = rBtnRssiPhaseAuto.Checked;
				OnAutoRxRestartOnChanged(AutoRxRestartOn);
			}
		}

		private void rBtnSensitivityBoost_CheckedChanged(object sender, EventArgs e)
		{
			SensitivityBoostOn = rBtnSensitivityBoostOn.Checked;
			OnSensitivityBoostOnChanged(SensitivityBoostOn);
		}

		public bool AfcAutoClearOn
		{
			get { return rBtnAfcAutoClearOn.Checked; }
			set
			{
				rBtnAfcAutoClearOn.CheckedChanged -= new EventHandler(rBtnAfcAutoClearOn_CheckedChanged);
				rBtnAfcAutoClearOff.CheckedChanged -= new EventHandler(rBtnAfcAutoClearOn_CheckedChanged);
				if (value)
				{
					rBtnAfcAutoClearOn.Checked = true;
					rBtnAfcAutoClearOff.Checked = false;
				}
				else
				{
					rBtnAfcAutoClearOn.Checked = false;
					rBtnAfcAutoClearOff.Checked = true;
				}
				rBtnAfcAutoClearOn.CheckedChanged += new EventHandler(rBtnAfcAutoClearOn_CheckedChanged);
				rBtnAfcAutoClearOff.CheckedChanged += new EventHandler(rBtnAfcAutoClearOn_CheckedChanged);
			}
		}

		public bool AfcAutoOn
		{
			get { return rBtnAfcAutoOn.Checked; }
			set
			{
				rBtnAfcAutoOn.CheckedChanged -= new EventHandler(rBtnAfcAutoOn_CheckedChanged);
				rBtnAfcAutoOff.CheckedChanged -= new EventHandler(rBtnAfcAutoOn_CheckedChanged);
				if (value)
				{
					rBtnAfcAutoOn.Checked = true;
					rBtnAfcAutoOff.Checked = false;
				}
				else
				{
					rBtnAfcAutoOn.Checked = false;
					rBtnAfcAutoOff.Checked = true;
				}
				rBtnAfcAutoOn.CheckedChanged += new EventHandler(rBtnAfcAutoOn_CheckedChanged);
				rBtnAfcAutoOff.CheckedChanged += new EventHandler(rBtnAfcAutoOn_CheckedChanged);
			}
		}

		public decimal AfcDccFreq
		{
			get { return afcDccFreq; }
			set
			{
				try
				{
					nudAfcDccFreq.ValueChanged -= new EventHandler(nudAfcDccFreq_ValueChanged);
					byte num = (byte)((Math.Log10((double)((4.0M * AfcRxBw) / (6.283185307179580M * value))) / Math.Log10(2.0)) - 2.0);
					afcDccFreq = (4.0M * AfcRxBw) / (6.283185307179580M * ((decimal)Math.Pow(2.0, (double)(num + 2))));
					nudAfcDccFreq.Value = afcDccFreq;
					nudAfcDccFreq.ValueChanged += new EventHandler(nudAfcDccFreq_ValueChanged);
				}
				catch (Exception)
				{
					nudAfcDccFreq.ValueChanged += new EventHandler(nudAfcDccFreq_ValueChanged);
				}
			}
		}

		public decimal AfcDccFreqMax
		{
			get { return nudAfcDccFreq.Maximum; }
			set
			{
				nudAfcDccFreq.Maximum = value;
			}
		}

		public decimal AfcDccFreqMin
		{
			get { return nudAfcDccFreq.Minimum; }
			set
			{
				nudAfcDccFreq.Minimum = value;
			}
		}

		public bool AfcDone
		{
			get { return afcDone; }
			set
			{
				afcDone = value;
				ledAfcDone.Checked = value;
			}
		}

		public bool AfcLowBetaOn
		{
			get { return rBtnAfcLowBetaOn.Checked; }
			set
			{
				rBtnAfcLowBetaOn.CheckedChanged -= new EventHandler(rBtnAfcLowBeta_CheckedChanged);
				rBtnAfcLowBetaOff.CheckedChanged -= new EventHandler(rBtnAfcLowBeta_CheckedChanged);
				if (value)
				{
					rBtnAfcLowBetaOn.Checked = true;
					rBtnAfcLowBetaOff.Checked = false;
				}
				else
				{
					rBtnAfcLowBetaOn.Checked = false;
					rBtnAfcLowBetaOff.Checked = true;
				}
				rBtnAfcLowBetaOn.CheckedChanged += new EventHandler(rBtnAfcLowBeta_CheckedChanged);
				rBtnAfcLowBetaOff.CheckedChanged += new EventHandler(rBtnAfcLowBeta_CheckedChanged);
			}
		}

		public decimal AfcRxBw
		{
			get { return afcRxBw; }
			set
			{
				try
				{
					nudRxFilterBwAfc.ValueChanged -= new EventHandler(nudRxFilterBwAfc_ValueChanged);
					int mant = 0;
					int exp = 0;
					SemtechLib.Devices.SX1231.SX1231.ComputeRxBwMantExp(frequencyXo, ModulationType, value, ref mant, ref exp);
					afcRxBw = SemtechLib.Devices.SX1231.SX1231.ComputeRxBw(frequencyXo, ModulationType, mant, exp);
					nudRxFilterBwAfc.Value = afcRxBw;
					nudRxFilterBwAfc.ValueChanged += new EventHandler(nudRxFilterBwAfc_ValueChanged);
				}
				catch (Exception)
				{
					nudRxFilterBwAfc.ValueChanged += new EventHandler(nudRxFilterBwAfc_ValueChanged);
				}
			}
		}

		public decimal AfcRxBwMax
		{
			get { return nudRxFilterBwAfc.Maximum; }
			set { nudRxFilterBwAfc.Maximum = value; }
		}

		public decimal AfcRxBwMin
		{
			get { return nudRxFilterBwAfc.Minimum; }
			set { nudRxFilterBwAfc.Minimum = value; }
		}

		public decimal AfcValue
		{
			get { return afcValue; }
			set
			{
				afcValue = value;
				lblAfcValue.Text = afcValue.ToString("N0");
			}
		}

		public bool AgcAutoRefOn
		{
			get
			{
				return rBtnAgcAutoRefOn.Checked;
			}
			set
			{
				rBtnAgcAutoRefOn.CheckedChanged -= new EventHandler(rBtnAgcAutoRef_CheckedChanged);
				rBtnAgcAutoRefOff.CheckedChanged -= new EventHandler(rBtnAgcAutoRef_CheckedChanged);
				if (value)
				{
					rBtnAgcAutoRefOn.Checked = true;
					rBtnAgcAutoRefOff.Checked = false;
					nudAgcSnrMargin.Enabled = true;
					nudAgcRefLevel.Enabled = false;
				}
				else
				{
					rBtnAgcAutoRefOn.Checked = false;
					rBtnAgcAutoRefOff.Checked = true;
					nudAgcSnrMargin.Enabled = false;
					nudAgcRefLevel.Enabled = true;
				}
				rBtnAgcAutoRefOn.CheckedChanged += new EventHandler(rBtnAgcAutoRef_CheckedChanged);
				rBtnAgcAutoRefOff.CheckedChanged += new EventHandler(rBtnAgcAutoRef_CheckedChanged);
			}
		}

		public int AgcReference
		{
			get { return agcReference; }
			set
			{
				agcReference = value;
				lblAgcReference.Text = value.ToString();
				if (rssiAutoThresh)
				{
					RssiThresh = value;
				}
			}
		}

		public int AgcRefLevel
		{
			get { return (int)nudAgcRefLevel.Value; }
			set
			{
				try
				{
					nudAgcRefLevel.ValueChanged -= new EventHandler(nudAgcRefLevel_ValueChanged);
					nudAgcRefLevel.Value = value;
					nudAgcRefLevel.ValueChanged += new EventHandler(nudAgcRefLevel_ValueChanged);
				}
				catch (Exception)
				{
					nudAgcRefLevel.ValueChanged += new EventHandler(nudAgcRefLevel_ValueChanged);
				}
			}
		}

		public byte AgcSnrMargin
		{
			get { return (byte)nudAgcSnrMargin.Value; }
			set
			{
				try
				{
					nudAgcSnrMargin.ValueChanged -= new EventHandler(nudAgcSnrMargin_ValueChanged);
					nudAgcSnrMargin.Value = value;
					nudAgcSnrMargin.ValueChanged += new EventHandler(nudAgcSnrMargin_ValueChanged);
				}
				catch (Exception)
				{
					nudAgcSnrMargin.ValueChanged += new EventHandler(nudAgcSnrMargin_ValueChanged);
				}
			}
		}

		public byte AgcStep1
		{
			get { return (byte)nudAgcStep1.Value; }
			set
			{
				try
				{
					nudAgcStep1.ValueChanged -= new EventHandler(nudAgcStep_ValueChanged);
					nudAgcStep1.Value = value;
					nudAgcStep1.ValueChanged += new EventHandler(nudAgcStep_ValueChanged);
				}
				catch (Exception)
				{
					nudAgcStep1.ValueChanged += new EventHandler(nudAgcStep_ValueChanged);
				}
			}
		}

		public byte AgcStep2
		{
			get { return (byte)nudAgcStep2.Value; }
			set
			{
				try
				{
					nudAgcStep2.ValueChanged -= new EventHandler(nudAgcStep_ValueChanged);
					nudAgcStep2.Value = value;
					nudAgcStep2.ValueChanged += new EventHandler(nudAgcStep_ValueChanged);
				}
				catch (Exception)
				{
					nudAgcStep2.ValueChanged += new EventHandler(nudAgcStep_ValueChanged);
				}
			}
		}

		public byte AgcStep3
		{
			get { return (byte)nudAgcStep3.Value; }
			set
			{
				try
				{
					nudAgcStep3.ValueChanged -= new EventHandler(nudAgcStep_ValueChanged);
					nudAgcStep3.Value = value;
					nudAgcStep3.ValueChanged += new EventHandler(nudAgcStep_ValueChanged);
				}
				catch (Exception)
				{
					nudAgcStep3.ValueChanged += new EventHandler(nudAgcStep_ValueChanged);
				}
			}
		}

		public byte AgcStep4
		{
			get { return (byte)nudAgcStep4.Value; }
			set
			{
				try
				{
					nudAgcStep4.ValueChanged -= new EventHandler(nudAgcStep_ValueChanged);
					nudAgcStep4.Value = value;
					nudAgcStep4.ValueChanged += new EventHandler(nudAgcStep_ValueChanged);
				}
				catch (Exception)
				{
					nudAgcStep4.ValueChanged += new EventHandler(nudAgcStep_ValueChanged);
				}
			}
		}

		public byte AgcStep5
		{
			get { return (byte)nudAgcStep5.Value; }
			set
			{
				try
				{
					nudAgcStep5.ValueChanged -= new EventHandler(nudAgcStep_ValueChanged);
					nudAgcStep5.Value = value;
					nudAgcStep5.ValueChanged += new EventHandler(nudAgcStep_ValueChanged);
				}
				catch (Exception)
				{
					nudAgcStep5.ValueChanged += new EventHandler(nudAgcStep_ValueChanged);
				}
			}
		}

		public int AgcThresh1
		{
			get { return agcThresh1; }
			set
			{
				agcThresh1 = value;
				lblAgcThresh1.Text = value.ToString();
			}
		}

		public int AgcThresh2
		{
			get { return agcThresh2; }
			set
			{
				agcThresh2 = value;
				lblAgcThresh2.Text = value.ToString();
			}
		}

		public int AgcThresh3
		{
			get { return agcThresh3; }
			set
			{
				agcThresh3 = value;
				lblAgcThresh3.Text = value.ToString();
			}
		}

		public int AgcThresh4
		{
			get
			{
				return agcThresh4;
			}
			set
			{
				agcThresh4 = value;
				lblAgcThresh4.Text = value.ToString();
			}
		}

		public int AgcThresh5
		{
			get { return agcThresh5; }
			set
			{
				agcThresh5 = value;
				lblAgcThresh5.Text = value.ToString();
			}
		}

		public bool AutoRxRestartOn
		{
			get { return rBtnRssiPhaseAuto.Checked; }
			set
			{
				rBtnRssiPhaseAuto.Checked = value;
				rBtnRssiPhaseManual.Checked = !value;
				if (Version != "2.3")
				{
					if (value || ((DataMode == DataModeEnum.Packet) && !value))
						btnRestartRx.Enabled = false;
					else
						btnRestartRx.Enabled = true;
				}
			}
		}

		public decimal BitRate
		{
			get { return bitRate; }
			set
			{
				if (bitRate != value)
				{
					int ookAverageThreshFilt = (int)OokAverageThreshFilt;
					cBoxOokAverageThreshFilt.Items.Clear();
					for (int i = 0x20; i >= 2; i /= 2)
					{
						if (i != 0x10)
							cBoxOokAverageThreshFilt.Items.Add(Math.Round((decimal)(value / ((decimal)(i * 3.1415926535897931)))).ToString());
					}
					OokAverageThreshFilt = (OokAverageThreshFiltEnum)ookAverageThreshFilt;
					try
					{
						nudTimeoutRxStart.ValueChanged -= new EventHandler(nudTimeoutRxStart_ValueChanged);
						decimal num3 = (uint)Math.Round((decimal)((nudTimeoutRxStart.Value / 1000M) / (16M / bitRate)), MidpointRounding.AwayFromZero);
						nudTimeoutRxStart.Maximum = (255M * (16M / value)) * 1000M;
						nudTimeoutRxStart.Increment = nudTimeoutRxStart.Maximum / 255M;
						nudTimeoutRxStart.Value = (num3 * (16M / value)) * 1000M;
					}
					catch
					{
					}
					finally
					{
						nudTimeoutRxStart.ValueChanged += new EventHandler(nudTimeoutRxStart_ValueChanged);
					}
					try
					{
						nudTimeoutRssiThresh.ValueChanged -= new EventHandler(nudTimeoutRssiThresh_ValueChanged);
						decimal num4 = (uint)Math.Round((decimal)((nudTimeoutRssiThresh.Value / 1000M) / (16M / bitRate)), MidpointRounding.AwayFromZero);
						nudTimeoutRssiThresh.Maximum = (255M * (16M / value)) * 1000M;
						nudTimeoutRssiThresh.Increment = nudTimeoutRssiThresh.Maximum / 255M;
						nudTimeoutRssiThresh.Value = (num4 * (16M / value)) * 1000M;
					}
					catch
					{
					}
					finally
					{
						nudTimeoutRssiThresh.ValueChanged += new EventHandler(nudTimeoutRssiThresh_ValueChanged);
					}
					bitRate = value;
				}
			}
		}

		public bool DagcOn
		{
			get { return rBtnDagcOn.Checked; }
			set
			{
				rBtnDagcOn.CheckedChanged -= new EventHandler(rBtnDagc_CheckedChanged);
				rBtnDagcOff.CheckedChanged -= new EventHandler(rBtnDagc_CheckedChanged);
				if (value)
				{
					rBtnDagcOn.Checked = true;
					rBtnDagcOff.Checked = false;
				}
				else
				{
					rBtnDagcOn.Checked = false;
					rBtnDagcOff.Checked = true;
				}
				rBtnDagcOn.CheckedChanged += new EventHandler(rBtnDagc_CheckedChanged);
				rBtnDagcOff.CheckedChanged += new EventHandler(rBtnDagc_CheckedChanged);
			}
		}

		public DataModeEnum DataMode
		{
			get { return dataMode; }
			set
			{
				dataMode = value;
				if (Version != "2.3")
				{
					if (AutoRxRestartOn || ((DataMode == DataModeEnum.Packet) && !AutoRxRestartOn))
					{
						btnRestartRx.Enabled = false;
					}
					else
					{
						btnRestartRx.Enabled = true;
					}
				}
			}
		}

		public decimal DccFreq
		{
			get { return dccFreq; }
			set
			{
				try
				{
					nudDccFreq.ValueChanged -= new EventHandler(nudDccFreq_ValueChanged);
					byte num = (byte)((Math.Log10((double)((4.0M * RxBw) / (6.283185307179580M * value))) / Math.Log10(2.0)) - 2.0);
					dccFreq = (4.0M * RxBw) / (6.283185307179580M * ((decimal)Math.Pow(2.0, (double)(num + 2))));
					nudDccFreq.Value = dccFreq;
					nudDccFreq.ValueChanged += new EventHandler(nudDccFreq_ValueChanged);
				}
				catch (Exception)
				{
					nudDccFreq.ValueChanged += new EventHandler(nudDccFreq_ValueChanged);
				}
			}
		}

		public decimal DccFreqMax
		{
			get { return nudDccFreq.Maximum; }
			set
			{
				nudDccFreq.Maximum = value;
			}
		}

		public decimal DccFreqMin
		{
			get { return nudDccFreq.Minimum; }
			set { nudDccFreq.Minimum = value; }
		}

		public bool FastRx
		{
			get { return rBtnFastRxOn.Checked; }
			set
			{
				rBtnFastRxOn.CheckedChanged -= new EventHandler(rBtnFastRx_CheckedChanged);
				rBtnFastRxOff.CheckedChanged -= new EventHandler(rBtnFastRx_CheckedChanged);
				if (value)
				{
					rBtnFastRxOn.Checked = true;
					rBtnFastRxOff.Checked = false;
				}
				else
				{
					rBtnFastRxOn.Checked = false;
					rBtnFastRxOff.Checked = true;
				}
				rBtnFastRxOn.CheckedChanged += new EventHandler(rBtnFastRx_CheckedChanged);
				rBtnFastRxOff.CheckedChanged += new EventHandler(rBtnFastRx_CheckedChanged);
			}
		}

		public bool FeiDone
		{
			get { return feiDone; }
			set
			{
				feiDone = value;
				ledFeiDone.Checked = value;
			}
		}

		public decimal FeiValue
		{
			get { return feiValue; }
			set
			{
				feiValue = value;
				lblFeiValue.Text = feiValue.ToString("N0");
			}
		}

		public decimal FrequencyXo
		{
			get { return frequencyXo; }
			set { frequencyXo = value; }
		}

		public LnaGainEnum LnaCurrentGain
		{
			private get
			{
				if (!rBtnLnaGainAutoOn.Checked)
				{
					if (rBtnLnaGain1.Checked)
						return LnaGainEnum.G1;
					if (rBtnLnaGain2.Checked)
						return LnaGainEnum.G2;
					if (rBtnLnaGain3.Checked)
						return LnaGainEnum.G3;
					if (rBtnLnaGain4.Checked)
						return LnaGainEnum.G4;
					if (rBtnLnaGain5.Checked)
						return LnaGainEnum.G5;
					if (rBtnLnaGain6.Checked)
						return LnaGainEnum.G6;
				}
				return LnaGainEnum.AGC;
			}
			set
			{
				switch (value)
				{
					case LnaGainEnum.G1:
						lblLnaGain1.BackColor = Color.LightSteelBlue;
						lblLnaGain2.BackColor = Color.Transparent;
						lblLnaGain3.BackColor = Color.Transparent;
						lblLnaGain4.BackColor = Color.Transparent;
						lblLnaGain5.BackColor = Color.Transparent;
						lblLnaGain6.BackColor = Color.Transparent;
						return;

					case LnaGainEnum.G2:
						lblLnaGain1.BackColor = Color.Transparent;
						lblLnaGain2.BackColor = Color.LightSteelBlue;
						lblLnaGain3.BackColor = Color.Transparent;
						lblLnaGain4.BackColor = Color.Transparent;
						lblLnaGain5.BackColor = Color.Transparent;
						lblLnaGain6.BackColor = Color.Transparent;
						return;

					case LnaGainEnum.G3:
						lblLnaGain1.BackColor = Color.Transparent;
						lblLnaGain2.BackColor = Color.Transparent;
						lblLnaGain3.BackColor = Color.LightSteelBlue;
						lblLnaGain4.BackColor = Color.Transparent;
						lblLnaGain5.BackColor = Color.Transparent;
						lblLnaGain6.BackColor = Color.Transparent;
						return;

					case LnaGainEnum.G4:
						lblLnaGain1.BackColor = Color.Transparent;
						lblLnaGain2.BackColor = Color.Transparent;
						lblLnaGain3.BackColor = Color.Transparent;
						lblLnaGain4.BackColor = Color.LightSteelBlue;
						lblLnaGain5.BackColor = Color.Transparent;
						lblLnaGain6.BackColor = Color.Transparent;
						return;

					case LnaGainEnum.G5:
						lblLnaGain1.BackColor = Color.Transparent;
						lblLnaGain2.BackColor = Color.Transparent;
						lblLnaGain3.BackColor = Color.Transparent;
						lblLnaGain4.BackColor = Color.Transparent;
						lblLnaGain5.BackColor = Color.LightSteelBlue;
						lblLnaGain6.BackColor = Color.Transparent;
						return;

					case LnaGainEnum.G6:
						lblLnaGain1.BackColor = Color.Transparent;
						lblLnaGain2.BackColor = Color.Transparent;
						lblLnaGain3.BackColor = Color.Transparent;
						lblLnaGain4.BackColor = Color.Transparent;
						lblLnaGain5.BackColor = Color.Transparent;
						lblLnaGain6.BackColor = Color.LightSteelBlue;
						return;
				}
			}
		}

		public LnaGainEnum LnaGainSelect
		{
			private get
			{
				if (!rBtnLnaGainAutoOn.Checked)
				{
					if (rBtnLnaGain1.Checked)
						return LnaGainEnum.G1;
					if (rBtnLnaGain2.Checked)
						return LnaGainEnum.G2;
					if (rBtnLnaGain3.Checked)
						return LnaGainEnum.G3;
					if (rBtnLnaGain4.Checked)
						return LnaGainEnum.G4;
					if (rBtnLnaGain5.Checked)
						return LnaGainEnum.G5;
					if (rBtnLnaGain6.Checked)
						return LnaGainEnum.G6;
				}
				return LnaGainEnum.AGC;
			}
			set
			{
				rBtnLnaGain1.CheckedChanged -= new EventHandler(rBtnLnaGain_CheckedChanged);
				rBtnLnaGain2.CheckedChanged -= new EventHandler(rBtnLnaGain_CheckedChanged);
				rBtnLnaGain3.CheckedChanged -= new EventHandler(rBtnLnaGain_CheckedChanged);
				rBtnLnaGain4.CheckedChanged -= new EventHandler(rBtnLnaGain_CheckedChanged);
				rBtnLnaGain5.CheckedChanged -= new EventHandler(rBtnLnaGain_CheckedChanged);
				rBtnLnaGain6.CheckedChanged -= new EventHandler(rBtnLnaGain_CheckedChanged);
				rBtnLnaGainAutoOn.CheckedChanged -= new EventHandler(rBtnLnaGain_CheckedChanged);
				rBtnLnaGainAutoOff.CheckedChanged -= new EventHandler(rBtnLnaGain_CheckedChanged);
				switch (value)
				{
					case LnaGainEnum.AGC:
						rBtnLnaGainAutoOn.Checked = true;
						rBtnLnaGainAutoOff.Checked = false;
						rBtnLnaGain1.Checked = true;
						rBtnLnaGain2.Checked = false;
						rBtnLnaGain3.Checked = false;
						rBtnLnaGain4.Checked = false;
						rBtnLnaGain5.Checked = false;
						rBtnLnaGain6.Checked = false;
						rBtnLnaGain1.Enabled = false;
						rBtnLnaGain2.Enabled = false;
						rBtnLnaGain3.Enabled = false;
						rBtnLnaGain4.Enabled = false;
						rBtnLnaGain5.Enabled = false;
						rBtnLnaGain6.Enabled = false;
						break;

					case LnaGainEnum.G1:
						rBtnLnaGainAutoOn.Checked = false;
						rBtnLnaGainAutoOff.Checked = true;
						rBtnLnaGain1.Checked = true;
						rBtnLnaGain2.Checked = false;
						rBtnLnaGain3.Checked = false;
						rBtnLnaGain4.Checked = false;
						rBtnLnaGain5.Checked = false;
						rBtnLnaGain6.Checked = false;
						rBtnLnaGain1.Enabled = true;
						rBtnLnaGain2.Enabled = true;
						rBtnLnaGain3.Enabled = true;
						rBtnLnaGain4.Enabled = true;
						rBtnLnaGain5.Enabled = true;
						rBtnLnaGain6.Enabled = true;
						break;

					case LnaGainEnum.G2:
						rBtnLnaGainAutoOn.Checked = false;
						rBtnLnaGainAutoOff.Checked = true;
						rBtnLnaGain1.Checked = false;
						rBtnLnaGain2.Checked = true;
						rBtnLnaGain3.Checked = false;
						rBtnLnaGain4.Checked = false;
						rBtnLnaGain5.Checked = false;
						rBtnLnaGain6.Checked = false;
						rBtnLnaGain1.Enabled = true;
						rBtnLnaGain2.Enabled = true;
						rBtnLnaGain3.Enabled = true;
						rBtnLnaGain4.Enabled = true;
						rBtnLnaGain5.Enabled = true;
						rBtnLnaGain6.Enabled = true;
						break;

					case LnaGainEnum.G3:
						rBtnLnaGainAutoOn.Checked = false;
						rBtnLnaGainAutoOff.Checked = true;
						rBtnLnaGain1.Checked = false;
						rBtnLnaGain2.Checked = false;
						rBtnLnaGain3.Checked = true;
						rBtnLnaGain4.Checked = false;
						rBtnLnaGain5.Checked = false;
						rBtnLnaGain6.Checked = false;
						rBtnLnaGain1.Enabled = true;
						rBtnLnaGain2.Enabled = true;
						rBtnLnaGain3.Enabled = true;
						rBtnLnaGain4.Enabled = true;
						rBtnLnaGain5.Enabled = true;
						rBtnLnaGain6.Enabled = true;
						break;

					case LnaGainEnum.G4:
						rBtnLnaGainAutoOn.Checked = false;
						rBtnLnaGainAutoOff.Checked = true;
						rBtnLnaGain1.Checked = false;
						rBtnLnaGain2.Checked = false;
						rBtnLnaGain3.Checked = false;
						rBtnLnaGain4.Checked = true;
						rBtnLnaGain5.Checked = false;
						rBtnLnaGain6.Checked = false;
						rBtnLnaGain1.Enabled = true;
						rBtnLnaGain2.Enabled = true;
						rBtnLnaGain3.Enabled = true;
						rBtnLnaGain4.Enabled = true;
						rBtnLnaGain5.Enabled = true;
						rBtnLnaGain6.Enabled = true;
						break;

					case LnaGainEnum.G5:
						rBtnLnaGainAutoOn.Checked = false;
						rBtnLnaGainAutoOff.Checked = true;
						rBtnLnaGain1.Checked = false;
						rBtnLnaGain2.Checked = false;
						rBtnLnaGain3.Checked = false;
						rBtnLnaGain4.Checked = false;
						rBtnLnaGain5.Checked = true;
						rBtnLnaGain6.Checked = false;
						rBtnLnaGain1.Enabled = true;
						rBtnLnaGain2.Enabled = true;
						rBtnLnaGain3.Enabled = true;
						rBtnLnaGain4.Enabled = true;
						rBtnLnaGain5.Enabled = true;
						rBtnLnaGain6.Enabled = true;
						break;

					case LnaGainEnum.G6:
						rBtnLnaGainAutoOn.Checked = false;
						rBtnLnaGainAutoOff.Checked = true;
						rBtnLnaGain1.Checked = false;
						rBtnLnaGain2.Checked = false;
						rBtnLnaGain3.Checked = false;
						rBtnLnaGain4.Checked = false;
						rBtnLnaGain5.Checked = false;
						rBtnLnaGain6.Checked = true;
						rBtnLnaGain1.Enabled = true;
						rBtnLnaGain2.Enabled = true;
						rBtnLnaGain3.Enabled = true;
						rBtnLnaGain4.Enabled = true;
						rBtnLnaGain5.Enabled = true;
						rBtnLnaGain6.Enabled = true;
						break;
				}
				rBtnLnaGain1.CheckedChanged += new EventHandler(rBtnLnaGain_CheckedChanged);
				rBtnLnaGain2.CheckedChanged += new EventHandler(rBtnLnaGain_CheckedChanged);
				rBtnLnaGain3.CheckedChanged += new EventHandler(rBtnLnaGain_CheckedChanged);
				rBtnLnaGain4.CheckedChanged += new EventHandler(rBtnLnaGain_CheckedChanged);
				rBtnLnaGain5.CheckedChanged += new EventHandler(rBtnLnaGain_CheckedChanged);
				rBtnLnaGain6.CheckedChanged += new EventHandler(rBtnLnaGain_CheckedChanged);
				rBtnLnaGainAutoOn.CheckedChanged += new EventHandler(rBtnLnaGain_CheckedChanged);
				rBtnLnaGainAutoOff.CheckedChanged += new EventHandler(rBtnLnaGain_CheckedChanged);
			}
		}

		public bool LnaLowPowerOn
		{
			get { return rBtnLnaLowPowerOn.Checked; }
			set
			{
				rBtnLnaLowPowerOn.CheckedChanged -= new EventHandler(rBtnLnaLowPower_CheckedChanged);
				rBtnLnaLowPowerOff.CheckedChanged -= new EventHandler(rBtnLnaLowPower_CheckedChanged);
				if (value)
				{
					rBtnLnaLowPowerOn.Checked = true;
					rBtnLnaLowPowerOff.Checked = false;
				}
				else
				{
					rBtnLnaLowPowerOn.Checked = false;
					rBtnLnaLowPowerOff.Checked = true;
				}
				rBtnLnaLowPowerOn.CheckedChanged += new EventHandler(rBtnLnaLowPower_CheckedChanged);
				rBtnLnaLowPowerOff.CheckedChanged += new EventHandler(rBtnLnaLowPower_CheckedChanged);
			}
		}

		public LnaZinEnum LnaZin
		{
			get
			{
				if (rBtnLnaZin50.Checked)
					return LnaZinEnum.ZIN_50;
				return LnaZinEnum.ZIN_200;
			}
			set
			{
				rBtnLnaZin50.CheckedChanged -= new EventHandler(rBtnLnaZin_CheckedChanged);
				rBtnLnaZin200.CheckedChanged -= new EventHandler(rBtnLnaZin_CheckedChanged);
				if (value == LnaZinEnum.ZIN_50)
				{
					rBtnLnaZin50.Checked = true;
					rBtnLnaZin200.Checked = false;
				}
				else
				{
					rBtnLnaZin50.Checked = false;
					rBtnLnaZin200.Checked = true;
				}
				rBtnLnaZin50.CheckedChanged += new EventHandler(rBtnLnaZin_CheckedChanged);
				rBtnLnaZin200.CheckedChanged += new EventHandler(rBtnLnaZin_CheckedChanged);
			}
		}

		public decimal LowBetaAfcOffset
		{
			get { return lowBetaAfcOffset; }
			set
			{
				try
				{
					nudLowBetaAfcOffset.ValueChanged -= new EventHandler(nudLowBetaAfcOffset_ValueChanged);
					sbyte num = (sbyte)(value / 488.0M);
					lowBetaAfcOffset = num * 488.0M;
					nudLowBetaAfcOffset.Value = lowBetaAfcOffset;
				}
				catch (Exception)
				{
				}
				finally
				{
					nudLowBetaAfcOffset.ValueChanged += new EventHandler(nudLowBetaAfcOffset_ValueChanged);
				}
			}
		}

		public ModulationTypeEnum ModulationType
		{
			get { return modulationType; }
			set { modulationType = value; }
		}

		public OokAverageThreshFiltEnum OokAverageThreshFilt
		{
			get { return (OokAverageThreshFiltEnum)cBoxOokAverageThreshFilt.SelectedIndex; }
			set
			{
				cBoxOokAverageThreshFilt.SelectedIndexChanged -= new EventHandler(cBoxOokAverageThreshFilt_SelectedIndexChanged);
				cBoxOokAverageThreshFilt.SelectedIndex = (int)value;
				cBoxOokAverageThreshFilt.SelectedIndexChanged += new EventHandler(cBoxOokAverageThreshFilt_SelectedIndexChanged);
			}
		}

		public byte OokFixedThresh
		{
			get { return (byte)nudOokFixedThresh.Value; }
			set
			{
				try
				{
					nudOokFixedThresh.ValueChanged -= new EventHandler(nudOokFixedThresh_ValueChanged);
					nudOokFixedThresh.Value = value;
					nudOokFixedThresh.ValueChanged += new EventHandler(nudOokFixedThresh_ValueChanged);
				}
				catch (Exception)
				{
					nudOokFixedThresh.ValueChanged += new EventHandler(nudOokFixedThresh_ValueChanged);
				}
			}
		}

		public OokPeakThreshDecEnum OokPeakThreshDec
		{
			get
			{
				return (OokPeakThreshDecEnum)cBoxOokPeakThreshDec.SelectedIndex;
			}
			set
			{
				cBoxOokPeakThreshDec.SelectedIndexChanged -= new EventHandler(cBoxOokPeakThreshDec_SelectedIndexChanged);
				cBoxOokPeakThreshDec.SelectedIndex = (int)value;
				cBoxOokPeakThreshDec.SelectedIndexChanged += new EventHandler(cBoxOokPeakThreshDec_SelectedIndexChanged);
			}
		}

		public decimal OokPeakThreshStep
		{
			get { return ookPeakThreshStep; }
			set
			{
				try
				{
					nudOokPeakThreshStep.ValueChanged -= new EventHandler(nudOokPeakThreshStep_ValueChanged);
					decimal[] array = new decimal[] { 0.5M, 1.0M, 1.5M, 2.0M, 3.0M, 4.0M, 5.0M, 6.0M };
					int index = Array.IndexOf<decimal>(array, value);
					ookPeakThreshStep = array[index];
					nudOokPeakThreshStep.Value = ookPeakThreshStep;
					nudOokPeakThreshStep.ValueChanged += new EventHandler(nudOokPeakThreshStep_ValueChanged);
				}
				catch (Exception)
				{
					nudOokPeakThreshStep.ValueChanged += new EventHandler(nudOokPeakThreshStep_ValueChanged);
				}
			}
		}

		public OokThreshTypeEnum OokThreshType
		{
			get
			{
				return (OokThreshTypeEnum)cBoxOokThreshType.SelectedIndex;
			}
			set
			{
				cBoxOokThreshType.SelectedIndexChanged -= new EventHandler(cBoxOokThreshType_SelectedIndexChanged);
				switch (value)
				{
					case OokThreshTypeEnum.Fixed:
						cBoxOokThreshType.SelectedIndex = 0;
						nudOokPeakThreshStep.Enabled = false;
						cBoxOokPeakThreshDec.Enabled = false;
						cBoxOokAverageThreshFilt.Enabled = false;
						nudOokFixedThresh.Enabled = true;
						break;

					case OokThreshTypeEnum.Peak:
						cBoxOokThreshType.SelectedIndex = 1;
						nudOokPeakThreshStep.Enabled = true;
						cBoxOokPeakThreshDec.Enabled = true;
						cBoxOokAverageThreshFilt.Enabled = false;
						nudOokFixedThresh.Enabled = true;
						break;

					case OokThreshTypeEnum.Average:
						cBoxOokThreshType.SelectedIndex = 2;
						nudOokPeakThreshStep.Enabled = false;
						cBoxOokPeakThreshDec.Enabled = false;
						cBoxOokAverageThreshFilt.Enabled = true;
						nudOokFixedThresh.Enabled = false;
						break;

					default:
						cBoxOokThreshType.SelectedIndex = -1;
						break;
				}
				cBoxOokThreshType.SelectedIndexChanged += new EventHandler(cBoxOokThreshType_SelectedIndexChanged);
			}
		}

		public bool RssiAutoThresh
		{
			get { return rBtnRssiAutoThreshOn.Checked; }
			set
			{
				rBtnRssiAutoThreshOn.CheckedChanged -= new EventHandler(rBtnRssiAutoThreshOn_CheckedChanged);
				rBtnRssiAutoThreshOff.CheckedChanged -= new EventHandler(rBtnRssiAutoThreshOn_CheckedChanged);
				if (value)
				{
					rBtnRssiAutoThreshOn.Checked = true;
					rBtnRssiAutoThreshOff.Checked = false;
					nudRssiThresh.Enabled = false;
				}
				else
				{
					rBtnRssiAutoThreshOn.Checked = false;
					rBtnRssiAutoThreshOff.Checked = true;
					nudRssiThresh.Enabled = true;
				}
				rssiAutoThresh = value;
				rBtnRssiAutoThreshOn.CheckedChanged += new EventHandler(rBtnRssiAutoThreshOn_CheckedChanged);
				rBtnRssiAutoThreshOff.CheckedChanged += new EventHandler(rBtnRssiAutoThreshOn_CheckedChanged);
			}
		}

		public bool RssiDone
		{
			get { return rssiDone; }
			set
			{
				rssiDone = value;
				ledRssiDone.Checked = value;
			}
		}

		public decimal RssiThresh
		{
			get { return nudRssiThresh.Value; }
			set
			{
				try
				{
					nudRssiThresh.ValueChanged -= new EventHandler(nudRssiThresh_ValueChanged);
					if (!rssiAutoThresh)
						nudRssiThresh.Value = value;
					else if (AgcReference < -127)
						nudRssiThresh.Value = -127.5M;
					else
						nudRssiThresh.Value = AgcReference - AgcSnrMargin;
					nudRssiThresh.ValueChanged += new EventHandler(nudRssiThresh_ValueChanged);
				}
				catch (Exception)
				{
					nudRssiThresh.ValueChanged += new EventHandler(nudRssiThresh_ValueChanged);
				}
			}
		}

		public decimal RssiValue
		{
			get
			{
				return rssiValue;
			}
			set
			{
				rssiValue = value;
				lblRssiValue.Text = value.ToString("###.0");
			}
		}

		public decimal RxBw
		{
			get { return rxBw; }
			set
			{
				try
				{
					nudRxFilterBw.ValueChanged -= new EventHandler(nudRxFilterBw_ValueChanged);
					int mant = 0;
					int exp = 0;
					SemtechLib.Devices.SX1231.SX1231.ComputeRxBwMantExp(frequencyXo, ModulationType, value, ref mant, ref exp);
					rxBw = SemtechLib.Devices.SX1231.SX1231.ComputeRxBw(frequencyXo, ModulationType, mant, exp);
					nudRxFilterBw.Value = rxBw;
					nudRxFilterBw.ValueChanged += new EventHandler(nudRxFilterBw_ValueChanged);
				}
				catch (Exception)
				{
					nudRxFilterBw.ValueChanged += new EventHandler(nudRxFilterBw_ValueChanged);
				}
			}
		}

		public decimal RxBwMax
		{
			get { return nudRxFilterBw.Maximum; }
			set
			{
				nudRxFilterBw.Maximum = value;
			}
		}

		public decimal RxBwMin
		{
			get { return nudRxFilterBw.Minimum; }
			set { nudRxFilterBw.Minimum = value; }
		}

		public bool SensitivityBoostOn
		{
			get { return rBtnSensitivityBoostOn.Checked; }
			set
			{
				rBtnSensitivityBoostOn.CheckedChanged -= new EventHandler(rBtnSensitivityBoost_CheckedChanged);
				rBtnSensitivityBoostOff.CheckedChanged -= new EventHandler(rBtnSensitivityBoost_CheckedChanged);
				if (value)
				{
					rBtnSensitivityBoostOn.Checked = true;
					rBtnSensitivityBoostOff.Checked = false;
				}
				else
				{
					rBtnSensitivityBoostOn.Checked = false;
					rBtnSensitivityBoostOff.Checked = true;
				}
				rBtnSensitivityBoostOn.CheckedChanged += new EventHandler(rBtnSensitivityBoost_CheckedChanged);
				rBtnSensitivityBoostOff.CheckedChanged += new EventHandler(rBtnSensitivityBoost_CheckedChanged);
			}
		}

		public decimal TimeoutRssiThresh
		{
			get { return nudTimeoutRssiThresh.Value; }
			set
			{
				try
				{
					nudTimeoutRssiThresh.ValueChanged -= new EventHandler(nudTimeoutRssiThresh_ValueChanged);
					uint num = (uint)Math.Round((decimal)((value / 1000M) / (16M / BitRate)), MidpointRounding.AwayFromZero);
					nudTimeoutRssiThresh.Value = (num * (16M / BitRate)) * 1000M;
					nudTimeoutRssiThresh.ValueChanged += new EventHandler(nudTimeoutRssiThresh_ValueChanged);
				}
				catch (Exception)
				{
					nudTimeoutRssiThresh.ValueChanged += new EventHandler(nudTimeoutRssiThresh_ValueChanged);
				}
			}
		}

		public decimal TimeoutRxStart
		{
			get { return nudTimeoutRxStart.Value; }
			set
			{
				try
				{
					nudTimeoutRxStart.ValueChanged -= new EventHandler(nudTimeoutRxStart_ValueChanged);
					uint num = (uint)Math.Round((decimal)((value / 1000M) / (16M / BitRate)), MidpointRounding.AwayFromZero);
					nudTimeoutRxStart.Value = (num * (16M / BitRate)) * 1000M;
					nudTimeoutRxStart.ValueChanged += new EventHandler(nudTimeoutRxStart_ValueChanged);
				}
				catch (Exception)
				{
					nudTimeoutRxStart.ValueChanged += new EventHandler(nudTimeoutRxStart_ValueChanged);
				}
			}
		}

		public string Version
		{
			get { return version; }
			set
			{
				version = value;
				if (value == "2.1")
				{
					gBoxAgc.Visible = true;
					gBoxDagc.Visible = false;
					lblAfcLowBeta.Visible = false;
					pnlAfcLowBeta.Visible = false;
					lblLowBetaAfcOffset.Visible = false;
					nudLowBetaAfcOffset.Visible = false;
					lblLowBetaAfcOfssetUnit.Visible = false;
					label21.Visible = true;
					panel7.Visible = true;
					label4.Visible = true;
					panel4.Visible = true;
				}
				else
				{
					gBoxAgc.Visible = false;
					gBoxDagc.Visible = false;
					lblAfcLowBeta.Visible = true;
					pnlAfcLowBeta.Visible = true;
					lblLowBetaAfcOffset.Visible = true;
					nudLowBetaAfcOffset.Visible = true;
					lblLowBetaAfcOfssetUnit.Visible = true;
					label21.Visible = false;
					panel7.Visible = false;
					label4.Visible = false;
					panel4.Visible = false;
					if (value == "2.3")
						gBoxDagc.Visible = true;
				}
			}
		}
	}
}
