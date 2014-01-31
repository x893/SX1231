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
            ListenEnd = (ListenEndEnum) cBoxListenEnd.SelectedIndex;
            OnListenEndChanged(ListenEnd);
        }

        private void cBoxListenResolIdle_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListenResolIdle = (ListenResolEnum) cBoxListenResolIdle.SelectedIndex;
            OnListenResolIdleChanged(ListenResolIdle);
        }

        private void cBoxListenResolRx_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListenResolRx = (ListenResolEnum) cBoxListenResolRx.SelectedIndex;
            OnListenResolRxChanged(ListenResolRx);
        }

        private void cBoxLowBatTrim_SelectedIndexChanged(object sender, EventArgs e)
        {
            LowBatTrim = (LowBatTrimEnum) cBoxLowBatTrim.SelectedIndex;
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
            components = new Container();
            btnRcCalibration = new Button();
            cBoxLowBatTrim = new ComboBox();
            panel4 = new Panel();
            rBtnLowBatOff = new RadioButton();
            rBtnLowBatOn = new RadioButton();
            label1 = new Label();
            panel2 = new Panel();
            rBtnModulationTypeOok = new RadioButton();
            rBtnModulationTypeFsk = new RadioButton();
            panel3 = new Panel();
            rBtnModulationShaping11 = new RadioButton();
            rBtnModulationShaping10 = new RadioButton();
            rBtnModulationShaping01 = new RadioButton();
            rBtnModulationShapingOff = new RadioButton();
            label5 = new Label();
            panel1 = new Panel();
            rBtnContinousBitSyncOff = new RadioButton();
            rBtnContinousBitSyncOn = new RadioButton();
            rBtnPacketHandler = new RadioButton();
            label7 = new Label();
            label6 = new Label();
            label10 = new Label();
            label9 = new Label();
            label14 = new Label();
            label16 = new Label();
            lblRcOscillatorCalStat = new Label();
            lblRcOscillatorCal = new Label();
            label15 = new Label();
            label13 = new Label();
            label17 = new Label();
            label11 = new Label();
            label18 = new Label();
            label8 = new Label();
            label12 = new Label();
            rBtnSequencerOff = new RadioButton();
            rBtnSequencerOn = new RadioButton();
            label19 = new Label();
            panel5 = new Panel();
            label20 = new Label();
            panel6 = new Panel();
            rBtnListenModeOff = new RadioButton();
            rBtnListenModeOn = new RadioButton();
            btnListenModeAbort = new Button();
            label21 = new Label();
            cBoxListenResolIdle = new ComboBox();
            label22 = new Label();
            panel7 = new Panel();
            rBtnListenCriteria1 = new RadioButton();
            rBtnListenCriteria0 = new RadioButton();
            label23 = new Label();
            label24 = new Label();
            cBoxListenEnd = new ComboBox();
            label25 = new Label();
            label26 = new Label();
            label27 = new Label();
            label28 = new Label();
            errorProvider = new ErrorProvider(components);
            nudBitRate = new NumericUpDownEx();
            nudFdev = new NumericUpDownEx();
            nudFrequencyRf = new NumericUpDownEx();
            lblListenResolRx = new Label();
            label30 = new Label();
            cBoxListenResolRx = new ComboBox();
            gBoxGeneral = new GroupBoxEx();
            gBoxBitSyncDataMode = new GroupBoxEx();
            gBoxModulation = new GroupBoxEx();
            gBoxOscillators = new GroupBoxEx();
            nudFrequencyXo = new NumericUpDownEx();
            ledRcCalibration = new Led();
            gBoxBatteryManagement = new GroupBoxEx();
            ledLowBatMonitor = new Led();
            gBoxListenMode = new GroupBoxEx();
            nudListenCoefRx = new NumericUpDownEx();
            nudListenCoefIdle = new NumericUpDownEx();
            panel4.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            panel1.SuspendLayout();
            panel5.SuspendLayout();
            panel6.SuspendLayout();
            panel7.SuspendLayout();
            ((ISupportInitialize) errorProvider).BeginInit();
            nudBitRate.BeginInit();
            nudFdev.BeginInit();
            nudFrequencyRf.BeginInit();
            gBoxGeneral.SuspendLayout();
            gBoxBitSyncDataMode.SuspendLayout();
            gBoxModulation.SuspendLayout();
            gBoxOscillators.SuspendLayout();
            nudFrequencyXo.BeginInit();
            gBoxBatteryManagement.SuspendLayout();
            gBoxListenMode.SuspendLayout();
            nudListenCoefRx.BeginInit();
            nudListenCoefIdle.BeginInit();
            base.SuspendLayout();
            btnRcCalibration.Location = new Point(0xa4, 0x33);
            btnRcCalibration.Name = "btnRcCalibration";
            btnRcCalibration.Size = new Size(0x4b, 0x17);
            btnRcCalibration.TabIndex = 4;
            btnRcCalibration.Text = "Calibrate";
            btnRcCalibration.UseVisualStyleBackColor = true;
            btnRcCalibration.Click += new EventHandler(btnRcCalibration_Click);
            cBoxLowBatTrim.DropDownStyle = ComboBoxStyle.DropDownList;
            cBoxLowBatTrim.FormattingEnabled = true;
            cBoxLowBatTrim.Items.AddRange(new object[] { "1.695", "1.764", "1.835", "1.905", "1.976", "2.045", "2.116", "2.185" });
            cBoxLowBatTrim.Location = new Point(0xa6, 0x2d);
            cBoxLowBatTrim.Name = "cBoxLowBatTrim";
            cBoxLowBatTrim.Size = new Size(0x7c, 0x15);
            cBoxLowBatTrim.TabIndex = 3;
            cBoxLowBatTrim.SelectedIndexChanged += new EventHandler(cBoxLowBatTrim_SelectedIndexChanged);
            panel4.AutoSize = true;
            panel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            panel4.Controls.Add(rBtnLowBatOff);
            panel4.Controls.Add(rBtnLowBatOn);
            panel4.Location = new Point(0xa6, 0x13);
            panel4.Name = "panel4";
            panel4.Size = new Size(0x66, 20);
            panel4.TabIndex = 1;
            rBtnLowBatOff.AutoSize = true;
            rBtnLowBatOff.Location = new Point(0x36, 3);
            rBtnLowBatOff.Margin = new Padding(3, 0, 3, 0);
            rBtnLowBatOff.Name = "rBtnLowBatOff";
            rBtnLowBatOff.Size = new Size(0x2d, 0x11);
            rBtnLowBatOff.TabIndex = 1;
            rBtnLowBatOff.Text = "OFF";
            rBtnLowBatOff.UseVisualStyleBackColor = true;
            rBtnLowBatOn.AutoSize = true;
            rBtnLowBatOn.Checked = true;
            rBtnLowBatOn.Location = new Point(3, 3);
            rBtnLowBatOn.Margin = new Padding(3, 0, 3, 0);
            rBtnLowBatOn.Name = "rBtnLowBatOn";
            rBtnLowBatOn.Size = new Size(0x29, 0x11);
            rBtnLowBatOn.TabIndex = 0;
            rBtnLowBatOn.TabStop = true;
            rBtnLowBatOn.Text = "ON";
            rBtnLowBatOn.UseVisualStyleBackColor = true;
            label1.AutoSize = true;
            label1.Location = new Point(6, 0x17);
            label1.Name = "label1";
            label1.Size = new Size(0x4e, 13);
            label1.TabIndex = 0;
            label1.Text = "XO Frequency:";
            panel2.AutoSize = true;
            panel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            panel2.Controls.Add(rBtnModulationTypeOok);
            panel2.Controls.Add(rBtnModulationTypeFsk);
            panel2.Location = new Point(0xa4, 0x13);
            panel2.Name = "panel2";
            panel2.Size = new Size(0x69, 0x17);
            panel2.TabIndex = 1;
            rBtnModulationTypeOok.AutoSize = true;
            rBtnModulationTypeOok.Location = new Point(0x36, 3);
            rBtnModulationTypeOok.Name = "rBtnModulationTypeOok";
            rBtnModulationTypeOok.Size = new Size(0x30, 0x11);
            rBtnModulationTypeOok.TabIndex = 1;
            rBtnModulationTypeOok.Text = "OOK";
            rBtnModulationTypeOok.UseVisualStyleBackColor = true;
            rBtnModulationTypeOok.CheckedChanged += new EventHandler(rBtnModulationType_CheckedChanged);
            rBtnModulationTypeFsk.AutoSize = true;
            rBtnModulationTypeFsk.Checked = true;
            rBtnModulationTypeFsk.Location = new Point(3, 3);
            rBtnModulationTypeFsk.Name = "rBtnModulationTypeFsk";
            rBtnModulationTypeFsk.Size = new Size(0x2d, 0x11);
            rBtnModulationTypeFsk.TabIndex = 0;
            rBtnModulationTypeFsk.TabStop = true;
            rBtnModulationTypeFsk.Text = "FSK";
            rBtnModulationTypeFsk.UseVisualStyleBackColor = true;
            rBtnModulationTypeFsk.CheckedChanged += new EventHandler(rBtnModulationType_CheckedChanged);
            panel3.AutoSize = true;
            panel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            panel3.Controls.Add(rBtnModulationShaping11);
            panel3.Controls.Add(rBtnModulationShaping10);
            panel3.Controls.Add(rBtnModulationShaping01);
            panel3.Controls.Add(rBtnModulationShapingOff);
            panel3.Location = new Point(0xa4, 0x30);
            panel3.Name = "panel3";
            panel3.Size = new Size(0x90, 0x5c);
            panel3.TabIndex = 3;
            rBtnModulationShaping11.AutoSize = true;
            rBtnModulationShaping11.Location = new Point(3, 0x48);
            rBtnModulationShaping11.Name = "rBtnModulationShaping11";
            rBtnModulationShaping11.Size = new Size(0x8a, 0x11);
            rBtnModulationShaping11.TabIndex = 3;
            rBtnModulationShaping11.Text = "Gaussian filter, BT = 0.3";
            rBtnModulationShaping11.UseVisualStyleBackColor = true;
            rBtnModulationShaping11.CheckedChanged += new EventHandler(rBtnModulationShaping_CheckedChanged);
            rBtnModulationShaping10.AutoSize = true;
            rBtnModulationShaping10.Location = new Point(3, 0x31);
            rBtnModulationShaping10.Name = "rBtnModulationShaping10";
            rBtnModulationShaping10.Size = new Size(0x8a, 0x11);
            rBtnModulationShaping10.TabIndex = 2;
            rBtnModulationShaping10.Text = "Gaussian filter, BT = 0.5";
            rBtnModulationShaping10.UseVisualStyleBackColor = true;
            rBtnModulationShaping10.CheckedChanged += new EventHandler(rBtnModulationShaping_CheckedChanged);
            rBtnModulationShaping01.AutoSize = true;
            rBtnModulationShaping01.Location = new Point(3, 0x1a);
            rBtnModulationShaping01.Name = "rBtnModulationShaping01";
            rBtnModulationShaping01.Size = new Size(0x8a, 0x11);
            rBtnModulationShaping01.TabIndex = 1;
            rBtnModulationShaping01.Text = "Gaussian filter, BT = 1.0";
            rBtnModulationShaping01.UseVisualStyleBackColor = true;
            rBtnModulationShaping01.CheckedChanged += new EventHandler(rBtnModulationShaping_CheckedChanged);
            rBtnModulationShapingOff.AutoSize = true;
            rBtnModulationShapingOff.Checked = true;
            rBtnModulationShapingOff.Location = new Point(3, 3);
            rBtnModulationShapingOff.Name = "rBtnModulationShapingOff";
            rBtnModulationShapingOff.Size = new Size(0x2d, 0x11);
            rBtnModulationShapingOff.TabIndex = 0;
            rBtnModulationShapingOff.TabStop = true;
            rBtnModulationShapingOff.Text = "OFF";
            rBtnModulationShapingOff.UseVisualStyleBackColor = true;
            rBtnModulationShapingOff.CheckedChanged += new EventHandler(rBtnModulationShaping_CheckedChanged);
            label5.AutoSize = true;
            label5.Location = new Point(6, 0x18);
            label5.Name = "label5";
            label5.Size = new Size(0x3e, 13);
            label5.TabIndex = 0;
            label5.Text = "Modulation:";
            panel1.AutoSize = true;
            panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            panel1.Controls.Add(rBtnContinousBitSyncOff);
            panel1.Controls.Add(rBtnContinousBitSyncOn);
            panel1.Controls.Add(rBtnPacketHandler);
            panel1.Location = new Point(0xa4, 0x13);
            panel1.Name = "panel1";
            panel1.Size = new Size(0x80, 0x45);
            panel1.TabIndex = 0;
            rBtnContinousBitSyncOff.AutoSize = true;
            rBtnContinousBitSyncOff.Location = new Point(3, 0x31);
            rBtnContinousBitSyncOff.Name = "rBtnContinousBitSyncOff";
            rBtnContinousBitSyncOff.Size = new Size(0x62, 0x11);
            rBtnContinousBitSyncOff.TabIndex = 2;
            rBtnContinousBitSyncOff.Text = "OFF- Continous";
            rBtnContinousBitSyncOff.UseVisualStyleBackColor = true;
            rBtnContinousBitSyncOff.CheckedChanged += new EventHandler(rBtnDataMode_CheckedChanged);
            rBtnContinousBitSyncOn.AutoSize = true;
            rBtnContinousBitSyncOn.Location = new Point(3, 0x1a);
            rBtnContinousBitSyncOn.Name = "rBtnContinousBitSyncOn";
            rBtnContinousBitSyncOn.Size = new Size(0x61, 0x11);
            rBtnContinousBitSyncOn.TabIndex = 1;
            rBtnContinousBitSyncOn.Text = "ON - Continous";
            rBtnContinousBitSyncOn.UseVisualStyleBackColor = true;
            rBtnContinousBitSyncOn.CheckedChanged += new EventHandler(rBtnDataMode_CheckedChanged);
            rBtnPacketHandler.AutoSize = true;
            rBtnPacketHandler.Checked = true;
            rBtnPacketHandler.Location = new Point(3, 3);
            rBtnPacketHandler.Name = "rBtnPacketHandler";
            rBtnPacketHandler.Size = new Size(0x7a, 0x11);
            rBtnPacketHandler.TabIndex = 0;
            rBtnPacketHandler.TabStop = true;
            rBtnPacketHandler.Text = "ON - Packet handler";
            rBtnPacketHandler.UseVisualStyleBackColor = true;
            rBtnPacketHandler.CheckedChanged += new EventHandler(rBtnDataMode_CheckedChanged);
            label7.AutoSize = true;
            label7.Location = new Point(6, 0x31);
            label7.Name = "label7";
            label7.Size = new Size(40, 13);
            label7.TabIndex = 3;
            label7.Text = "Bitrate:";
            label6.AutoSize = true;
            label6.Location = new Point(6, 0x35);
            label6.Name = "label6";
            label6.Size = new Size(0x66, 13);
            label6.TabIndex = 2;
            label6.Text = "Modulation shaping:";
            label10.AutoSize = true;
            label10.Location = new Point(6, 0x4b);
            label10.Name = "label10";
            label10.Size = new Size(0x22, 13);
            label10.TabIndex = 6;
            label10.Text = "Fdev:";
            label9.AutoSize = true;
            label9.Location = new Point(0x126, 0x17);
            label9.Name = "label9";
            label9.Size = new Size(20, 13);
            label9.TabIndex = 2;
            label9.Text = "Hz";
            label14.AutoSize = true;
            label14.Location = new Point(6, 0x17);
            label14.Name = "label14";
            label14.Size = new Size(0x4a, 13);
            label14.TabIndex = 0;
            label14.Text = "RF frequency:";
            label16.AutoSize = true;
            label16.Location = new Point(0x128, 0x31);
            label16.Name = "label16";
            label16.Size = new Size(14, 13);
            label16.TabIndex = 4;
            label16.Text = "V";
            lblRcOscillatorCalStat.AutoSize = true;
            lblRcOscillatorCalStat.Location = new Point(6, 0x51);
            lblRcOscillatorCalStat.Name = "lblRcOscillatorCalStat";
            lblRcOscillatorCalStat.Size = new Size(0x97, 13);
            lblRcOscillatorCalStat.TabIndex = 5;
            lblRcOscillatorCalStat.Text = "RC oscillator calibration status:";
            lblRcOscillatorCal.AutoSize = true;
            lblRcOscillatorCal.Location = new Point(6, 0x38);
            lblRcOscillatorCal.Name = "lblRcOscillatorCal";
            lblRcOscillatorCal.Size = new Size(120, 13);
            lblRcOscillatorCal.TabIndex = 3;
            lblRcOscillatorCal.Text = "RC oscillator calibration:";
            label15.AutoSize = true;
            label15.Location = new Point(8, 0x18);
            label15.Name = "label15";
            label15.Size = new Size(0x6b, 13);
            label15.TabIndex = 0;
            label15.Text = "Low battery detector:";
            label13.AutoSize = true;
            label13.Location = new Point(0x126, 0x17);
            label13.Name = "label13";
            label13.Size = new Size(20, 13);
            label13.TabIndex = 2;
            label13.Text = "Hz";
            label17.AutoSize = true;
            label17.Location = new Point(8, 0x30);
            label17.Name = "label17";
            label17.Size = new Size(130, 13);
            label17.TabIndex = 2;
            label17.Text = "Low battery threshold trim:";
            label11.AutoSize = true;
            label11.Location = new Point(0x126, 0x4b);
            label11.Name = "label11";
            label11.Size = new Size(20, 13);
            label11.TabIndex = 9;
            label11.Text = "Hz";
            label18.AutoSize = true;
            label18.Location = new Point(8, 0x49);
            label18.Name = "label18";
            label18.Size = new Size(0x6c, 13);
            label18.TabIndex = 5;
            label18.Text = "Low battery indicator:";
            label8.AutoSize = true;
            label8.Location = new Point(0x126, 0x31);
            label8.Name = "label8";
            label8.Size = new Size(0x18, 13);
            label8.TabIndex = 5;
            label8.Text = "bps";
            label12.AutoSize = true;
            label12.Location = new Point(0x89, 0x4b);
            label12.Name = "label12";
            label12.Size = new Size(0x15, 13);
            label12.TabIndex = 7;
            label12.Text = "+/-";
            rBtnSequencerOff.AutoSize = true;
            rBtnSequencerOff.Location = new Point(50, 0);
            rBtnSequencerOff.Margin = new Padding(3, 0, 3, 0);
            rBtnSequencerOff.Name = "rBtnSequencerOff";
            rBtnSequencerOff.Size = new Size(0x2d, 0x11);
            rBtnSequencerOff.TabIndex = 1;
            rBtnSequencerOff.TabStop = true;
            rBtnSequencerOff.Text = "OFF";
            rBtnSequencerOff.UseVisualStyleBackColor = true;
            rBtnSequencerOff.CheckedChanged += new EventHandler(rBtnSequencer_CheckedChanged);
            rBtnSequencerOn.AutoSize = true;
            rBtnSequencerOn.Location = new Point(3, 0);
            rBtnSequencerOn.Margin = new Padding(3, 0, 3, 0);
            rBtnSequencerOn.Name = "rBtnSequencerOn";
            rBtnSequencerOn.Size = new Size(0x29, 0x11);
            rBtnSequencerOn.TabIndex = 0;
            rBtnSequencerOn.TabStop = true;
            rBtnSequencerOn.Text = "ON";
            rBtnSequencerOn.UseVisualStyleBackColor = true;
            rBtnSequencerOn.CheckedChanged += new EventHandler(rBtnSequencer_CheckedChanged);
            label19.AutoSize = true;
            label19.Location = new Point(7, 0x63);
            label19.Name = "label19";
            label19.Size = new Size(0x3e, 13);
            label19.TabIndex = 10;
            label19.Text = "Sequencer:";
            panel5.AutoSize = true;
            panel5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            panel5.Controls.Add(rBtnSequencerOff);
            panel5.Controls.Add(rBtnSequencerOn);
            panel5.Location = new Point(0xa4, 0x61);
            panel5.Name = "panel5";
            panel5.Size = new Size(0x62, 0x11);
            panel5.TabIndex = 11;
            label20.AutoSize = true;
            label20.Location = new Point(8, 0x57);
            label20.Name = "label20";
            label20.Size = new Size(0x43, 13);
            label20.TabIndex = 0;
            label20.Text = "Listen mode:";
            panel6.AutoSize = true;
            panel6.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            panel6.Controls.Add(rBtnListenModeOff);
            panel6.Controls.Add(rBtnListenModeOn);
            panel6.Location = new Point(0xa5, 0x55);
            panel6.Name = "panel6";
            panel6.Size = new Size(0x62, 0x11);
            panel6.TabIndex = 1;
            rBtnListenModeOff.AutoSize = true;
            rBtnListenModeOff.Location = new Point(50, 0);
            rBtnListenModeOff.Margin = new Padding(3, 0, 3, 0);
            rBtnListenModeOff.Name = "rBtnListenModeOff";
            rBtnListenModeOff.Size = new Size(0x2d, 0x11);
            rBtnListenModeOff.TabIndex = 1;
            rBtnListenModeOff.TabStop = true;
            rBtnListenModeOff.Text = "OFF";
            rBtnListenModeOff.UseVisualStyleBackColor = true;
            rBtnListenModeOff.CheckedChanged += new EventHandler(rBtnListenMode_CheckedChanged);
            rBtnListenModeOn.AutoSize = true;
            rBtnListenModeOn.Location = new Point(3, 0);
            rBtnListenModeOn.Margin = new Padding(3, 0, 3, 0);
            rBtnListenModeOn.Name = "rBtnListenModeOn";
            rBtnListenModeOn.Size = new Size(0x29, 0x11);
            rBtnListenModeOn.TabIndex = 0;
            rBtnListenModeOn.TabStop = true;
            rBtnListenModeOn.Text = "ON";
            rBtnListenModeOn.UseVisualStyleBackColor = true;
            rBtnListenModeOn.CheckedChanged += new EventHandler(rBtnListenMode_CheckedChanged);
            btnListenModeAbort.Enabled = false;
            btnListenModeAbort.Location = new Point(0x10d, 0x52);
            btnListenModeAbort.Name = "btnListenModeAbort";
            btnListenModeAbort.Size = new Size(0x4b, 0x17);
            btnListenModeAbort.TabIndex = 2;
            btnListenModeAbort.Text = "Abort";
            btnListenModeAbort.UseVisualStyleBackColor = true;
            btnListenModeAbort.Visible = false;
            btnListenModeAbort.Click += new EventHandler(btnListenModeAbort_Click);
            label21.AutoSize = true;
            label21.Location = new Point(8, 0x72);
            label21.Name = "label21";
            label21.Size = new Size(0x69, 13);
            label21.TabIndex = 3;
            label21.Text = "Listen resolution idle:";
            cBoxListenResolIdle.DropDownStyle = ComboBoxStyle.DropDownList;
            cBoxListenResolIdle.FormattingEnabled = true;
            cBoxListenResolIdle.Items.AddRange(new object[] { "64", "4'100", "262'000" });
            cBoxListenResolIdle.Location = new Point(0xa5, 0x6c);
            cBoxListenResolIdle.Name = "cBoxListenResolIdle";
            cBoxListenResolIdle.Size = new Size(0x7c, 0x15);
            cBoxListenResolIdle.TabIndex = 4;
            cBoxListenResolIdle.SelectedIndexChanged += new EventHandler(cBoxListenResolIdle_SelectedIndexChanged);
            label22.AutoSize = true;
            label22.Location = new Point(0x127, 0x70);
            label22.Name = "label22";
            label22.Size = new Size(0x12, 13);
            label22.TabIndex = 5;
            label22.Text = "\x00b5s";
            panel7.AutoSize = true;
            panel7.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            panel7.BackColor = Color.Transparent;
            panel7.Controls.Add(rBtnListenCriteria1);
            panel7.Controls.Add(rBtnListenCriteria0);
            panel7.Location = new Point(0xa5, 0xa2);
            panel7.Name = "panel7";
            panel7.Size = new Size(0xe2, 0x2e);
            panel7.TabIndex = 10;
            rBtnListenCriteria1.AutoSize = true;
            rBtnListenCriteria1.Location = new Point(3, 0x1a);
            rBtnListenCriteria1.Name = "rBtnListenCriteria1";
            rBtnListenCriteria1.Size = new Size(220, 0x11);
            rBtnListenCriteria1.TabIndex = 1;
            rBtnListenCriteria1.Text = "> RssiThreshold && SyncAddress detected";
            rBtnListenCriteria1.UseVisualStyleBackColor = true;
            rBtnListenCriteria1.CheckedChanged += new EventHandler(rBtnListenCriteria_CheckedChanged);
            rBtnListenCriteria0.AutoSize = true;
            rBtnListenCriteria0.Checked = true;
            rBtnListenCriteria0.Location = new Point(3, 3);
            rBtnListenCriteria0.Name = "rBtnListenCriteria0";
            rBtnListenCriteria0.Size = new Size(0x65, 0x11);
            rBtnListenCriteria0.TabIndex = 0;
            rBtnListenCriteria0.TabStop = true;
            rBtnListenCriteria0.Text = "> RssiThreshold";
            rBtnListenCriteria0.UseVisualStyleBackColor = true;
            rBtnListenCriteria0.CheckedChanged += new EventHandler(rBtnListenCriteria_CheckedChanged);
            label23.AutoSize = true;
            label23.Location = new Point(8, 0xa7);
            label23.Name = "label23";
            label23.Size = new Size(0x48, 13);
            label23.TabIndex = 9;
            label23.Text = "Listen criteria:";
            label24.AutoSize = true;
            label24.Location = new Point(8, 0xd9);
            label24.Name = "label24";
            label24.Size = new Size(0x3b, 13);
            label24.TabIndex = 11;
            label24.Text = "Listen end:";
            cBoxListenEnd.DropDownStyle = ComboBoxStyle.DropDownList;
            cBoxListenEnd.FormattingEnabled = true;
            cBoxListenEnd.Items.AddRange(new object[] { "Rx", "Rx & Mode after IRQ", "Rx & Idle after IRQ" });
            cBoxListenEnd.Location = new Point(0xa5, 0xd6);
            cBoxListenEnd.Name = "cBoxListenEnd";
            cBoxListenEnd.Size = new Size(0x7c, 0x15);
            cBoxListenEnd.TabIndex = 12;
            cBoxListenEnd.SelectedIndexChanged += new EventHandler(cBoxListenEnd_SelectedIndexChanged);
            label25.AutoSize = true;
            label25.Location = new Point(0x127, 0xf5);
            label25.Name = "label25";
            label25.Size = new Size(20, 13);
            label25.TabIndex = 15;
            label25.Text = "ms";
            label26.AutoSize = true;
            label26.Location = new Point(0x127, 0x10f);
            label26.Name = "label26";
            label26.Size = new Size(20, 13);
            label26.TabIndex = 0x12;
            label26.Text = "ms";
            label27.AutoSize = true;
            label27.Location = new Point(8, 0xf5);
            label27.Name = "label27";
            label27.Size = new Size(0x4f, 13);
            label27.TabIndex = 13;
            label27.Text = "Listen idle time:";
            label28.AutoSize = true;
            label28.Location = new Point(8, 270);
            label28.Name = "label28";
            label28.Size = new Size(0x4c, 13);
            label28.TabIndex = 0x10;
            label28.Text = "Listen Rx time:";
            errorProvider.ContainerControl = this;
            errorProvider.SetIconPadding(nudBitRate, 30);
            nudBitRate.Location = new Point(0xa4, 0x2d);
            int[] bits = new int[4];
            bits[0] = 0x9367e;
            nudBitRate.Maximum = new decimal(bits);
            int[] numArray2 = new int[4];
            numArray2[0] = 600;
            nudBitRate.Minimum = new decimal(numArray2);
            nudBitRate.Name = "nudBitRate";
            nudBitRate.Size = new Size(0x7c, 20);
            nudBitRate.TabIndex = 4;
            nudBitRate.ThousandsSeparator = true;
            int[] numArray3 = new int[4];
            numArray3[0] = 0x12c0;
            nudBitRate.Value = new decimal(numArray3);
            nudBitRate.ValueChanged += new EventHandler(nudBitRate_ValueChanged);
            errorProvider.SetIconPadding(nudFdev, 30);
            int[] numArray4 = new int[4];
            numArray4[0] = 0x3d;
            nudFdev.Increment = new decimal(numArray4);
            nudFdev.Location = new Point(0xa4, 0x47);
            int[] numArray5 = new int[4];
            numArray5[0] = 0x493e0;
            nudFdev.Maximum = new decimal(numArray5);
            nudFdev.Name = "nudFdev";
            nudFdev.Size = new Size(0x7c, 20);
            nudFdev.TabIndex = 8;
            nudFdev.ThousandsSeparator = true;
            int[] numArray6 = new int[4];
            numArray6[0] = 0x138d;
            nudFdev.Value = new decimal(numArray6);
            nudFdev.ValueChanged += new EventHandler(nudFdev_ValueChanged);
            errorProvider.SetIconPadding(nudFrequencyRf, 30);
            int[] numArray7 = new int[4];
            numArray7[0] = 0x3d;
            nudFrequencyRf.Increment = new decimal(numArray7);
            nudFrequencyRf.Location = new Point(0xa4, 0x13);
            int[] numArray8 = new int[4];
            numArray8[0] = 0x3ccbf700;
            nudFrequencyRf.Maximum = new decimal(numArray8);
            int[] numArray9 = new int[4];
            numArray9[0] = 0x11490c80;
            nudFrequencyRf.Minimum = new decimal(numArray9);
            nudFrequencyRf.Name = "nudFrequencyRf";
            nudFrequencyRf.Size = new Size(0x7c, 20);
            nudFrequencyRf.TabIndex = 1;
            nudFrequencyRf.ThousandsSeparator = true;
            int[] numArray10 = new int[4];
            numArray10[0] = 0x3689cac0;
            nudFrequencyRf.Value = new decimal(numArray10);
            nudFrequencyRf.ValueChanged += new EventHandler(nudFrequencyRf_ValueChanged);
            lblListenResolRx.AutoSize = true;
            lblListenResolRx.Location = new Point(8, 0x8d);
            lblListenResolRx.Name = "lblListenResolRx";
            lblListenResolRx.Size = new Size(0x66, 13);
            lblListenResolRx.TabIndex = 6;
            lblListenResolRx.Text = "Listen resolution Rx:";
            label30.AutoSize = true;
            label30.Location = new Point(0x127, 0x8b);
            label30.Name = "label30";
            label30.Size = new Size(0x12, 13);
            label30.TabIndex = 8;
            label30.Text = "\x00b5s";
            cBoxListenResolRx.DropDownStyle = ComboBoxStyle.DropDownList;
            cBoxListenResolRx.FormattingEnabled = true;
            cBoxListenResolRx.Items.AddRange(new object[] { "64", "4'100", "262'000" });
            cBoxListenResolRx.Location = new Point(0xa5, 0x87);
            cBoxListenResolRx.Name = "cBoxListenResolRx";
            cBoxListenResolRx.Size = new Size(0x7c, 0x15);
            cBoxListenResolRx.TabIndex = 7;
            cBoxListenResolRx.SelectedIndexChanged += new EventHandler(cBoxListenResolRx_SelectedIndexChanged);
            gBoxGeneral.Controls.Add(nudBitRate);
            gBoxGeneral.Controls.Add(label12);
            gBoxGeneral.Controls.Add(label8);
            gBoxGeneral.Controls.Add(label11);
            gBoxGeneral.Controls.Add(label13);
            gBoxGeneral.Controls.Add(label14);
            gBoxGeneral.Controls.Add(panel5);
            gBoxGeneral.Controls.Add(label19);
            gBoxGeneral.Controls.Add(label10);
            gBoxGeneral.Controls.Add(label7);
            gBoxGeneral.Controls.Add(nudFdev);
            gBoxGeneral.Controls.Add(nudFrequencyRf);
            gBoxGeneral.Location = new Point(0x10, 9);
            gBoxGeneral.Name = "gBoxGeneral";
            gBoxGeneral.Size = new Size(0x163, 0x7a);
            gBoxGeneral.TabIndex = 0;
            gBoxGeneral.TabStop = false;
            gBoxGeneral.Text = "General";
            gBoxGeneral.MouseLeave += new EventHandler(control_MouseLeave);
            gBoxGeneral.MouseEnter += new EventHandler(control_MouseEnter);
            gBoxBitSyncDataMode.Controls.Add(panel1);
            gBoxBitSyncDataMode.Location = new Point(0x10, 0x89);
            gBoxBitSyncDataMode.Name = "gBoxBitSyncDataMode";
            gBoxBitSyncDataMode.Size = new Size(0x163, 0x5b);
            gBoxBitSyncDataMode.TabIndex = 1;
            gBoxBitSyncDataMode.TabStop = false;
            gBoxBitSyncDataMode.Text = "Bit synchronizer / data mode";
            gBoxBitSyncDataMode.MouseLeave += new EventHandler(control_MouseLeave);
            gBoxBitSyncDataMode.MouseEnter += new EventHandler(control_MouseEnter);
            gBoxModulation.Controls.Add(panel2);
            gBoxModulation.Controls.Add(label6);
            gBoxModulation.Controls.Add(label5);
            gBoxModulation.Controls.Add(panel3);
            gBoxModulation.Location = new Point(0x10, 0xea);
            gBoxModulation.Name = "gBoxModulation";
            gBoxModulation.Size = new Size(0x163, 0x8f);
            gBoxModulation.TabIndex = 2;
            gBoxModulation.TabStop = false;
            gBoxModulation.Text = "Modulation";
            gBoxModulation.MouseLeave += new EventHandler(control_MouseLeave);
            gBoxModulation.MouseEnter += new EventHandler(control_MouseEnter);
            gBoxOscillators.Controls.Add(nudFrequencyXo);
            gBoxOscillators.Controls.Add(label9);
            gBoxOscillators.Controls.Add(btnRcCalibration);
            gBoxOscillators.Controls.Add(label1);
            gBoxOscillators.Controls.Add(lblRcOscillatorCal);
            gBoxOscillators.Controls.Add(lblRcOscillatorCalStat);
            gBoxOscillators.Controls.Add(ledRcCalibration);
            gBoxOscillators.Location = new Point(0x10, 0x17f);
            gBoxOscillators.Name = "gBoxOscillators";
            gBoxOscillators.Size = new Size(0x163, 100);
            gBoxOscillators.TabIndex = 3;
            gBoxOscillators.TabStop = false;
            gBoxOscillators.Text = "Oscillators";
            gBoxOscillators.MouseLeave += new EventHandler(control_MouseLeave);
            gBoxOscillators.MouseEnter += new EventHandler(control_MouseEnter);
            nudFrequencyXo.Location = new Point(0xa4, 0x13);
            int[] numArray11 = new int[4];
            numArray11[0] = 0x1e84800;
            nudFrequencyXo.Maximum = new decimal(numArray11);
            int[] numArray12 = new int[4];
            numArray12[0] = 0x18cba80;
            nudFrequencyXo.Minimum = new decimal(numArray12);
            nudFrequencyXo.Name = "nudFrequencyXo";
            nudFrequencyXo.Size = new Size(0x7c, 20);
            nudFrequencyXo.TabIndex = 1;
            nudFrequencyXo.ThousandsSeparator = true;
            int[] numArray13 = new int[4];
            numArray13[0] = 0x1e84800;
            nudFrequencyXo.Value = new decimal(numArray13);
            nudFrequencyXo.ValueChanged += new EventHandler(nudFrequencyXo_ValueChanged);
            ledRcCalibration.BackColor = Color.Transparent;
            ledRcCalibration.LedColor = Color.Green;
            ledRcCalibration.LedSize = new Size(11, 11);
            ledRcCalibration.Location = new Point(0xa4, 80);
            ledRcCalibration.Name = "ledRcCalibration";
            ledRcCalibration.Size = new Size(15, 15);
            ledRcCalibration.TabIndex = 6;
            ledRcCalibration.Text = "led1";
            gBoxBatteryManagement.Controls.Add(panel4);
            gBoxBatteryManagement.Controls.Add(label18);
            gBoxBatteryManagement.Controls.Add(label17);
            gBoxBatteryManagement.Controls.Add(label15);
            gBoxBatteryManagement.Controls.Add(label16);
            gBoxBatteryManagement.Controls.Add(cBoxLowBatTrim);
            gBoxBatteryManagement.Controls.Add(ledLowBatMonitor);
            gBoxBatteryManagement.Location = new Point(0x179, 0x17f);
            gBoxBatteryManagement.Name = "gBoxBatteryManagement";
            gBoxBatteryManagement.Size = new Size(0x195, 100);
            gBoxBatteryManagement.TabIndex = 5;
            gBoxBatteryManagement.TabStop = false;
            gBoxBatteryManagement.Text = "Battery management";
            gBoxBatteryManagement.MouseLeave += new EventHandler(control_MouseLeave);
            gBoxBatteryManagement.MouseEnter += new EventHandler(control_MouseEnter);
            ledLowBatMonitor.BackColor = Color.Transparent;
            ledLowBatMonitor.LedColor = Color.Green;
            ledLowBatMonitor.LedSize = new Size(11, 11);
            ledLowBatMonitor.Location = new Point(0xa6, 0x48);
            ledLowBatMonitor.Name = "ledLowBatMonitor";
            ledLowBatMonitor.Size = new Size(15, 15);
            ledLowBatMonitor.TabIndex = 6;
            ledLowBatMonitor.Text = "Low battery";
            gBoxListenMode.Controls.Add(panel6);
            gBoxListenMode.Controls.Add(label20);
            gBoxListenMode.Controls.Add(label21);
            gBoxListenMode.Controls.Add(label23);
            gBoxListenMode.Controls.Add(lblListenResolRx);
            gBoxListenMode.Controls.Add(label24);
            gBoxListenMode.Controls.Add(label27);
            gBoxListenMode.Controls.Add(label28);
            gBoxListenMode.Controls.Add(btnListenModeAbort);
            gBoxListenMode.Controls.Add(label22);
            gBoxListenMode.Controls.Add(cBoxListenEnd);
            gBoxListenMode.Controls.Add(label25);
            gBoxListenMode.Controls.Add(cBoxListenResolRx);
            gBoxListenMode.Controls.Add(label30);
            gBoxListenMode.Controls.Add(cBoxListenResolIdle);
            gBoxListenMode.Controls.Add(label26);
            gBoxListenMode.Controls.Add(nudListenCoefRx);
            gBoxListenMode.Controls.Add(panel7);
            gBoxListenMode.Controls.Add(nudListenCoefIdle);
            gBoxListenMode.Location = new Point(0x179, 9);
            gBoxListenMode.Name = "gBoxListenMode";
            gBoxListenMode.Size = new Size(0x195, 0x170);
            gBoxListenMode.TabIndex = 4;
            gBoxListenMode.TabStop = false;
            gBoxListenMode.Text = "Listen mode";
            gBoxListenMode.MouseLeave += new EventHandler(control_MouseLeave);
            gBoxListenMode.MouseEnter += new EventHandler(control_MouseEnter);
            nudListenCoefRx.DecimalPlaces = 3;
            int[] numArray14 = new int[4];
            numArray14[0] = 0x29;
            numArray14[3] = 0x10000;
            nudListenCoefRx.Increment = new decimal(numArray14);
            nudListenCoefRx.Location = new Point(0xa5, 0x10b);
            int[] numArray15 = new int[4];
            numArray15[0] = 0x28d7;
            numArray15[3] = 0x10000;
            nudListenCoefRx.Maximum = new decimal(numArray15);
            nudListenCoefRx.Name = "nudListenCoefRx";
            nudListenCoefRx.Size = new Size(0x7c, 20);
            nudListenCoefRx.TabIndex = 0x11;
            nudListenCoefRx.ThousandsSeparator = true;
            int[] numArray16 = new int[4];
            numArray16[0] = 0x520;
            numArray16[3] = 0x10000;
            nudListenCoefRx.Value = new decimal(numArray16);
            nudListenCoefRx.ValueChanged += new EventHandler(nudListenCoefRx_ValueChanged);
            nudListenCoefIdle.DecimalPlaces = 3;
            int[] numArray17 = new int[4];
            numArray17[0] = 0x29;
            numArray17[3] = 0x10000;
            nudListenCoefIdle.Increment = new decimal(numArray17);
            nudListenCoefIdle.Location = new Point(0xa5, 0xf1);
            int[] numArray18 = new int[4];
            numArray18[0] = 0x28d7;
            numArray18[3] = 0x10000;
            nudListenCoefIdle.Maximum = new decimal(numArray18);
            nudListenCoefIdle.Name = "nudListenCoefIdle";
            nudListenCoefIdle.Size = new Size(0x7c, 20);
            nudListenCoefIdle.TabIndex = 14;
            nudListenCoefIdle.ThousandsSeparator = true;
            int[] numArray19 = new int[4];
            numArray19[0] = 0x273d;
            numArray19[3] = 0x10000;
            nudListenCoefIdle.Value = new decimal(numArray19);
            nudListenCoefIdle.ValueChanged += new EventHandler(nudListenCoefIdle_ValueChanged);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.Controls.Add(gBoxListenMode);
            base.Controls.Add(gBoxBatteryManagement);
            base.Controls.Add(gBoxOscillators);
            base.Controls.Add(gBoxModulation);
            base.Controls.Add(gBoxBitSyncDataMode);
            base.Controls.Add(gBoxGeneral);
            base.Name = "CommonViewControl";
            base.Size = new Size(0x31f, 0x1ed);
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel5.ResumeLayout(false);
            panel5.PerformLayout();
            panel6.ResumeLayout(false);
            panel6.PerformLayout();
            panel7.ResumeLayout(false);
            panel7.PerformLayout();
            ((ISupportInitialize) errorProvider).EndInit();
            nudBitRate.EndInit();
            nudFdev.EndInit();
            nudFrequencyRf.EndInit();
            gBoxGeneral.ResumeLayout(false);
            gBoxGeneral.PerformLayout();
            gBoxBitSyncDataMode.ResumeLayout(false);
            gBoxBitSyncDataMode.PerformLayout();
            gBoxModulation.ResumeLayout(false);
            gBoxModulation.PerformLayout();
            gBoxOscillators.ResumeLayout(false);
            gBoxOscillators.PerformLayout();
            nudFrequencyXo.EndInit();
            gBoxBatteryManagement.ResumeLayout(false);
            gBoxBatteryManagement.PerformLayout();
            gBoxListenMode.ResumeLayout(false);
            gBoxListenMode.PerformLayout();
            nudListenCoefRx.EndInit();
            nudListenCoefIdle.EndInit();
            base.ResumeLayout(false);
        }

        private void nudBitRate_ValueChanged(object sender, EventArgs e)
        {
            int num1 = (int) Math.Round((decimal) (FrequencyXo / BitRate), MidpointRounding.AwayFromZero);
            int num = (int) Math.Round((decimal) (FrequencyXo / nudBitRate.Value), MidpointRounding.AwayFromZero);
            int num2 = (int) (nudBitRate.Value - BitRate);
            nudBitRate.ValueChanged -= new EventHandler(nudBitRate_ValueChanged);
            if ((num2 >= -1) && (num2 <= 1))
            {
                nudBitRate.Value = Math.Round((decimal) (FrequencyXo / (num - num2)), MidpointRounding.AwayFromZero);
            }
            else
            {
                nudBitRate.Value = Math.Round((decimal) (FrequencyXo / num), MidpointRounding.AwayFromZero);
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
            {
                BitRateChanged(this, new DecimalEventArg(value));
            }
        }

        private void OnDataModeChanged(DataModeEnum value)
        {
            if (DataModeChanged != null)
            {
                DataModeChanged(this, new DataModeEventArg(value));
            }
        }

        private void OnDocumentationChanged(DocumentationChangedEventArgs e)
        {
            if (DocumentationChanged != null)
            {
                DocumentationChanged(this, e);
            }
        }

        private void OnFdevChanged(decimal value)
        {
            if (FdevChanged != null)
            {
                FdevChanged(this, new DecimalEventArg(value));
            }
        }

        private void OnFrequencyRfChanged(decimal value)
        {
            if (FrequencyRfChanged != null)
            {
                FrequencyRfChanged(this, new DecimalEventArg(value));
            }
        }

        private void OnFrequencyXoChanged(decimal value)
        {
            if (FrequencyXoChanged != null)
            {
                FrequencyXoChanged(this, new DecimalEventArg(value));
            }
        }

        private void OnListenCoefIdleChanged(decimal value)
        {
            if (ListenCoefIdleChanged != null)
            {
                ListenCoefIdleChanged(this, new DecimalEventArg(value));
            }
        }

        private void OnListenCoefRxChanged(decimal value)
        {
            if (ListenCoefRxChanged != null)
            {
                ListenCoefRxChanged(this, new DecimalEventArg(value));
            }
        }

        private void OnListenCriteriaChanged(ListenCriteriaEnum value)
        {
            if (ListenCriteriaChanged != null)
            {
                ListenCriteriaChanged(this, new ListenCriteriaEventArg(value));
            }
        }

        private void OnListenEndChanged(ListenEndEnum value)
        {
            if (ListenEndChanged != null)
            {
                ListenEndChanged(this, new ListenEndEventArg(value));
            }
        }

        private void OnListenModeAbortChanged()
        {
            if (ListenModeAbortChanged != null)
            {
                ListenModeAbortChanged(this, EventArgs.Empty);
            }
        }

        private void OnListenModeChanged(bool value)
        {
            if (ListenModeChanged != null)
            {
                ListenModeChanged(this, new BooleanEventArg(value));
            }
        }

        private void OnListenResolIdleChanged(ListenResolEnum value)
        {
            if (ListenResolIdleChanged != null)
            {
                ListenResolIdleChanged(this, new ListenResolEventArg(value));
            }
        }

        private void OnListenResolRxChanged(ListenResolEnum value)
        {
            if (ListenResolRxChanged != null)
            {
                ListenResolRxChanged(this, new ListenResolEventArg(value));
            }
        }

        private void OnLowBatOnChanged(bool value)
        {
            if (LowBatOnChanged != null)
            {
                LowBatOnChanged(this, new BooleanEventArg(value));
            }
        }

        private void OnLowBatTrimChanged(LowBatTrimEnum value)
        {
            if (LowBatTrimChanged != null)
            {
                LowBatTrimChanged(this, new LowBatTrimEventArg(value));
            }
        }

        private void OnModulationShapingChanged(byte value)
        {
            if (ModulationShapingChanged != null)
            {
                ModulationShapingChanged(this, new ByteEventArg(value));
            }
        }

        private void OnModulationTypeChanged(ModulationTypeEnum value)
        {
            if (ModulationTypeChanged != null)
            {
                ModulationTypeChanged(this, new ModulationTypeEventArg(value));
            }
        }

        private void OnRcCalibrationChanged()
        {
            if (RcCalibrationChanged != null)
            {
                RcCalibrationChanged(this, EventArgs.Empty);
            }
        }

        private void OnSequencerChanged(bool value)
        {
            if (SequencerChanged != null)
            {
                SequencerChanged(this, new BooleanEventArg(value));
            }
        }

        private void rBtnDataMode_CheckedChanged(object sender, EventArgs e)
        {
            if (rBtnPacketHandler.Checked)
            {
                DataMode = DataModeEnum.Packet;
            }
            else if (rBtnContinousBitSyncOn.Checked)
            {
                DataMode = DataModeEnum.ContinuousBitSync;
            }
            else if (rBtnContinousBitSyncOff.Checked)
            {
                DataMode = DataModeEnum.Continuous;
            }
            else
            {
                DataMode = DataModeEnum.Reserved;
            }
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
            {
                ModulationShaping = 0;
            }
            else if (rBtnModulationShaping01.Checked)
            {
                ModulationShaping = 1;
            }
            else if (rBtnModulationShaping10.Checked)
            {
                ModulationShaping = 2;
            }
            else if (rBtnModulationShaping11.Checked)
            {
                ModulationShaping = 3;
            }
            OnModulationShapingChanged(ModulationShaping);
        }

        private void rBtnModulationType_CheckedChanged(object sender, EventArgs e)
        {
            if (rBtnModulationTypeFsk.Checked)
            {
                ModulationType = ModulationTypeEnum.FSK;
            }
            else if (rBtnModulationTypeOok.Checked)
            {
                ModulationType = ModulationTypeEnum.OOK;
            }
            else
            {
                ModulationType = ModulationTypeEnum.Reserved;
            }
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
                    ushort num = (ushort) Math.Round((decimal) (FrequencyXo / value), MidpointRounding.AwayFromZero);
                    bitRate = Math.Round((decimal) (FrequencyXo / num), MidpointRounding.AwayFromZero);
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
                {
                    return DataModeEnum.Packet;
                }
                if (rBtnContinousBitSyncOn.Checked)
                {
                    return DataModeEnum.ContinuousBitSync;
                }
                if (rBtnContinousBitSyncOff.Checked)
                {
                    return DataModeEnum.Continuous;
                }
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
                    ushort num = (ushort) Math.Round((decimal) (value / FrequencyStep), MidpointRounding.AwayFromZero);
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
                    uint num = (uint) Math.Round((value / FrequencyStep), MidpointRounding.AwayFromZero);
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
                catch
                {
                }
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
                catch
                {
                }
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
                {
                    return ListenCriteriaEnum.RssiThresh;
                }
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
                return (ListenEndEnum) cBoxListenEnd.SelectedIndex;
            }
            set
            {
                try
                {
                    cBoxListenEnd.SelectedIndexChanged -= new EventHandler(cBoxListenEnd_SelectedIndexChanged);
                    if (value == ListenEndEnum.Reserved)
                    {
                        cBoxListenEnd.SelectedIndex = -1;
                    }
                    else
                    {
                        cBoxListenEnd.SelectedIndex = (int) value;
                    }
                }
                catch (Exception)
                {
                }
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
                return (ListenResolEnum) cBoxListenResolIdle.SelectedIndex;
            }
            set
            {
                cBoxListenResolIdle.SelectedIndexChanged -= new EventHandler(cBoxListenResolIdle_SelectedIndexChanged);
                cBoxListenResolIdle.SelectedIndex = (int) value;
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
                return (ListenResolEnum) cBoxListenResolRx.SelectedIndex;
            }
            set
            {
                try
                {
                    cBoxListenResolRx.SelectedIndexChanged -= new EventHandler(cBoxListenResolRx_SelectedIndexChanged);
                    cBoxListenResolRx.SelectedIndex = (int) value;
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
            get
            {
                return ledLowBatMonitor.Checked;
            }
            set
            {
                ledLowBatMonitor.Checked = value;
            }
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
                return (LowBatTrimEnum) cBoxLowBatTrim.SelectedIndex;
            }
            set
            {
                cBoxLowBatTrim.SelectedIndexChanged -= new EventHandler(cBoxLowBatTrim_SelectedIndexChanged);
                cBoxLowBatTrim.SelectedIndex = (int) value;
                cBoxLowBatTrim.SelectedIndexChanged += new EventHandler(cBoxLowBatTrim_SelectedIndexChanged);
            }
        }

        public byte ModulationShaping
        {
            get
            {
                if (rBtnModulationShapingOff.Checked)
                {
                    return 0;
                }
                if (rBtnModulationShaping01.Checked)
                {
                    return 1;
                }
                if (rBtnModulationShaping10.Checked)
                {
                    return 2;
                }
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
                {
                    return ModulationTypeEnum.OOK;
                }
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
            get
            {
                return ledRcCalibration.Checked;
            }
            set
            {
                ledRcCalibration.Checked = value;
            }
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
            get
            {
                return version;
            }
            set
            {
                version = value;
                if (value.ToString() == "2.1")
                {
                    cBoxListenResolRx.Enabled = false;
                }
                else
                {
                    cBoxListenResolRx.Enabled = true;
                }
            }
        }
    }
}

