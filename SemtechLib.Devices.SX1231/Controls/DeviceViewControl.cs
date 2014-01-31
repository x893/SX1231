using SemtechLib.Controls;
using SemtechLib.Devices.SX1231;
using SemtechLib.Devices.SX1231.Enumerations;
using SemtechLib.Devices.SX1231.Events;
using SemtechLib.General.Events;
using SemtechLib.General.Interfaces;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;

namespace SemtechLib.Devices.SX1231.Controls
{
    public class DeviceViewControl : UserControl, INotifyDocumentationChanged
    {
		public event DocumentationChangedEventHandler DocumentationChanged;
		public event SemtechLib.General.Events.ErrorEventHandler Error;

		private delegate void SX1231PacketHandlerStartedDelegate(object sender, EventArgs e);
		private delegate void SX1231PacketHandlerStopedDelegate(object sender, EventArgs e);
		private delegate void SX1231PacketHandlerTransmittedDelegate(object sender, PacketStatusEventArg e);
		private delegate void SX1231PropertyChangedDelegate(object sender, PropertyChangedEventArgs e);

        private CommonViewControl commonViewControl1;
        private IContainer components;
        private GroupBoxEx gBoxIrqFlags;
        private GroupBoxEx gBoxOperatingMode;
        private IrqMapViewControl irqMapViewControl1;
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
        private Label label29;
        private Label label30;
        private Label label31;
        private Label lbModeReady;
        private Led ledAutoMode;
        private Led ledCrcOk;
        private Led ledFifoFull;
        private Led ledFifoLevel;
        private Led ledFifoNotEmpty;
        private Led ledFifoOverrun;
        private Led ledLowBat;
        private Led ledModeReady;
        private Led ledPacketSent;
        private Led ledPayloadReady;
        private Led ledPllLock;
        private Led ledRssi;
        private Led ledRxReady;
        private Led ledSyncAddressMatch;
        private Led ledTimeout;
        private Led ledTxReady;
        private PacketHandlerView packetHandlerView1;
        private RadioButton rBtnReceiver;
        private RadioButton rBtnSleep;
        private RadioButton rBtnStandby;
        private RadioButton rBtnSynthesizer;
        private RadioButton rBtnTransmitter;
        private ReceiverViewControl receiverViewControl1;
        private SemtechLib.Devices.SX1231.SX1231 sx1231;
        private TabPage tabCommon;
        private TabControl tabControl1;
        private TabPage tabIrqMap;
        private TabPage tabPacketHandler;
        private TabPage tabReceiver;
        private TabPage tabTemperature;
        private TabPage tabTransmitter;
        private TemperatureViewControl temperatureViewControl1;
        private TransmitterViewControl transmitterViewControl1;

        public DeviceViewControl()
        {
            InitializeComponent();
        }

        private void commonViewControl1_BitRateChanged(object sender, DecimalEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetBitRate(e.Value);
                receiverViewControl1.BitRate = sx1231.BitRate;
                packetHandlerView1.BitRate = sx1231.BitRate;
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void commonViewControl1_DataModeChanged(object sender, DataModeEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetDataMode(e.Value);
                irqMapViewControl1.DataMode = sx1231.DataMode;
                packetHandlerView1.DataMode = sx1231.DataMode;
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void commonViewControl1_DocumentationChanged(object sender, DocumentationChangedEventArgs e)
        {
            OnDocumentationChanged(e);
        }

        private void commonViewControl1_FdevChanged(object sender, DecimalEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetFdev(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void commonViewControl1_FrequencyRfChanged(object sender, DecimalEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetFrequencyRf(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void commonViewControl1_FrequencyXoChanged(object sender, DecimalEventArg e)
        {
            sx1231.FrequencyXo = commonViewControl1.FrequencyXo;
        }

        private void commonViewControl1_ListenCoefIdleChanged(object sender, DecimalEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetListenCoefIdle(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void commonViewControl1_ListenCoefRxChanged(object sender, DecimalEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetListenCoefRx(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void commonViewControl1_ListenCriteriaChanged(object sender, ListenCriteriaEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetListenCriteria(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void commonViewControl1_ListenEndChanged(object sender, ListenEndEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetListenEnd(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void commonViewControl1_ListenModeAbortChanged(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.ListenModeAbort();
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void commonViewControl1_ListenModeChanged(object sender, BooleanEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetListenMode(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void commonViewControl1_ListenResolIdleChanged(object sender, ListenResolEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetListenResolIdle(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void commonViewControl1_ListenResolRxChanged(object sender, ListenResolEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetListenResolRx(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void commonViewControl1_LowBatOnChanged(object sender, BooleanEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetLowBatOn(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void commonViewControl1_LowBatTrimChanged(object sender, LowBatTrimEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetLowBatTrim(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void commonViewControl1_ModulationShapingChanged(object sender, ByteEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetModulationShaping(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void commonViewControl1_ModulationTypeChanged(object sender, ModulationTypeEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetModulationType(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void commonViewControl1_RcCalibrationChanged(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.RcCalStart();
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void commonViewControl1_SequencerChanged(object sender, BooleanEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetSequencer(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void control_MouseEnter(object sender, EventArgs e)
        {
            if (sender == gBoxIrqFlags)
            {
                OnDocumentationChanged(new DocumentationChangedEventArgs("IrqMap", "Irq flags"));
            }
            else if (sender == gBoxOperatingMode)
            {
                OnDocumentationChanged(new DocumentationChangedEventArgs("Common", "Operating mode"));
            }
        }

        private void control_MouseLeave(object sender, EventArgs e)
        {
            OnDocumentationChanged(new DocumentationChangedEventArgs(".", "Overview"));
        }

        public void DisableControls()
        {
            commonViewControl1.Enabled = false;
            transmitterViewControl1.Enabled = false;
            receiverViewControl1.Enabled = false;
            irqMapViewControl1.Enabled = false;
            temperatureViewControl1.Enabled = false;
            gBoxOperatingMode.Enabled = false;
        }

        public new void Dispose()
        {
            base.Dispose();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        public void EnableControls()
        {
            commonViewControl1.Enabled = true;
            transmitterViewControl1.Enabled = true;
            receiverViewControl1.Enabled = true;
            irqMapViewControl1.Enabled = true;
            temperatureViewControl1.Enabled = true;
            gBoxOperatingMode.Enabled = true;
        }

		#region InitializeComponent()
		private void InitializeComponent()
        {
            tabControl1 = new TabControl();
            tabCommon = new TabPage();
            commonViewControl1 = new CommonViewControl();
            tabTransmitter = new TabPage();
            transmitterViewControl1 = new TransmitterViewControl();
            tabReceiver = new TabPage();
            receiverViewControl1 = new ReceiverViewControl();
            tabIrqMap = new TabPage();
            irqMapViewControl1 = new IrqMapViewControl();
            tabPacketHandler = new TabPage();
            packetHandlerView1 = new PacketHandlerView();
            tabTemperature = new TabPage();
            temperatureViewControl1 = new TemperatureViewControl();
            gBoxOperatingMode = new GroupBoxEx();
            rBtnTransmitter = new RadioButton();
            rBtnReceiver = new RadioButton();
            rBtnSynthesizer = new RadioButton();
            rBtnStandby = new RadioButton();
            rBtnSleep = new RadioButton();
            lbModeReady = new Label();
            label19 = new Label();
            label18 = new Label();
            label17 = new Label();
            label23 = new Label();
            label22 = new Label();
            label21 = new Label();
            label20 = new Label();
            label27 = new Label();
            label26 = new Label();
            label25 = new Label();
            label24 = new Label();
            label31 = new Label();
            label30 = new Label();
            label29 = new Label();
            label28 = new Label();
            gBoxIrqFlags = new GroupBoxEx();
            ledLowBat = new Led();
            ledCrcOk = new Led();
            ledRxReady = new Led();
            ledPayloadReady = new Led();
            ledTxReady = new Led();
            ledPacketSent = new Led();
            ledPllLock = new Led();
            ledModeReady = new Led();
            ledFifoOverrun = new Led();
            ledRssi = new Led();
            ledFifoLevel = new Led();
            ledTimeout = new Led();
            ledFifoNotEmpty = new Led();
            ledAutoMode = new Led();
            ledFifoFull = new Led();
            ledSyncAddressMatch = new Led();
            tabControl1.SuspendLayout();
            tabCommon.SuspendLayout();
            tabTransmitter.SuspendLayout();
            tabReceiver.SuspendLayout();
            tabIrqMap.SuspendLayout();
            tabPacketHandler.SuspendLayout();
            tabTemperature.SuspendLayout();
            gBoxOperatingMode.SuspendLayout();
            gBoxIrqFlags.SuspendLayout();
            base.SuspendLayout();
            tabControl1.Controls.Add(tabCommon);
            tabControl1.Controls.Add(tabTransmitter);
            tabControl1.Controls.Add(tabReceiver);
            tabControl1.Controls.Add(tabIrqMap);
            tabControl1.Controls.Add(tabPacketHandler);
            tabControl1.Controls.Add(tabTemperature);
            tabControl1.Location = new Point(3, 3);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(0x327, 0x207);
            tabControl1.TabIndex = 0;
            tabCommon.Controls.Add(commonViewControl1);
            tabCommon.Location = new Point(4, 0x16);
            tabCommon.Name = "tabCommon";
            tabCommon.Padding = new Padding(3);
            tabCommon.Size = new Size(0x31f, 0x1ed);
            tabCommon.TabIndex = 0;
            tabCommon.Text = "Common";
            tabCommon.UseVisualStyleBackColor = true;
            int[] bits = new int[4];
            bits[0] = 0x12c0;
            commonViewControl1.BitRate = new decimal(bits);
            commonViewControl1.DataMode = DataModeEnum.Packet;
            int[] numArray2 = new int[4];
            numArray2[0] = 0x138a;
            commonViewControl1.Fdev = new decimal(numArray2);
            int[] numArray3 = new int[4];
            numArray3[0] = 0x3689cac0;
            commonViewControl1.FrequencyRf = new decimal(numArray3);
            int[] numArray4 = new int[4];
            numArray4[0] = 0x3d;
            commonViewControl1.FrequencyStep = new decimal(numArray4);
            int[] numArray5 = new int[4];
            numArray5[0] = 0x1e84800;
            commonViewControl1.FrequencyXo = new decimal(numArray5);
            int[] numArray6 = new int[4];
            numArray6[0] = 0x273d;
            numArray6[3] = 0x10000;
            commonViewControl1.ListenCoefIdle = new decimal(numArray6);
            int[] numArray7 = new int[4];
            numArray7[0] = 0x83;
            commonViewControl1.ListenCoefRx = new decimal(numArray7);
            commonViewControl1.ListenCriteria = ListenCriteriaEnum.RssiThresh;
            commonViewControl1.ListenMode = false;
            commonViewControl1.ListenResolIdle = ListenResolEnum.Res004100;
            commonViewControl1.ListenResolRx = ListenResolEnum.Res004100;
            commonViewControl1.Location = new Point(0, 0);
            commonViewControl1.LowBatMonitor = true;
            commonViewControl1.LowBatOn = true;
            commonViewControl1.ModulationShaping = 0;
            commonViewControl1.ModulationType = ModulationTypeEnum.FSK;
            commonViewControl1.Name = "commonViewControl1";
            commonViewControl1.RcCalDone = false;
            commonViewControl1.Sequencer = false;
            commonViewControl1.Size = new Size(0x31f, 0x1ed);
            commonViewControl1.TabIndex = 0;
            commonViewControl1.Version = "2.3";
            commonViewControl1.FrequencyXoChanged += new DecimalEventHandler(commonViewControl1_FrequencyXoChanged);
            commonViewControl1.SequencerChanged += new BooleanEventHandler(commonViewControl1_SequencerChanged);
            commonViewControl1.ListenModeChanged += new BooleanEventHandler(commonViewControl1_ListenModeChanged);
            commonViewControl1.ListenModeAbortChanged += new EventHandler(commonViewControl1_ListenModeAbortChanged);
            commonViewControl1.DataModeChanged += new DataModeEventHandler(commonViewControl1_DataModeChanged);
            commonViewControl1.ModulationTypeChanged += new ModulationTypeEventHandler(commonViewControl1_ModulationTypeChanged);
            commonViewControl1.ModulationShapingChanged += new ByteEventHandler(commonViewControl1_ModulationShapingChanged);
            commonViewControl1.BitRateChanged += new DecimalEventHandler(commonViewControl1_BitRateChanged);
            commonViewControl1.FdevChanged += new DecimalEventHandler(commonViewControl1_FdevChanged);
            commonViewControl1.FrequencyRfChanged += new DecimalEventHandler(commonViewControl1_FrequencyRfChanged);
            commonViewControl1.RcCalibrationChanged += new EventHandler(commonViewControl1_RcCalibrationChanged);
            commonViewControl1.LowBatOnChanged += new BooleanEventHandler(commonViewControl1_LowBatOnChanged);
            commonViewControl1.LowBatTrimChanged += new LowBatTrimEventHandler(commonViewControl1_LowBatTrimChanged);
            commonViewControl1.ListenResolIdleChanged += new ListenResolEventHandler(commonViewControl1_ListenResolIdleChanged);
            commonViewControl1.ListenResolRxChanged += new ListenResolEventHandler(commonViewControl1_ListenResolRxChanged);
            commonViewControl1.ListenCriteriaChanged += new ListenCriteriaEventHandler(commonViewControl1_ListenCriteriaChanged);
            commonViewControl1.ListenEndChanged += new ListenEndEventHandler(commonViewControl1_ListenEndChanged);
            commonViewControl1.ListenCoefIdleChanged += new DecimalEventHandler(commonViewControl1_ListenCoefIdleChanged);
            commonViewControl1.ListenCoefRxChanged += new DecimalEventHandler(commonViewControl1_ListenCoefRxChanged);
            commonViewControl1.DocumentationChanged += new DocumentationChangedEventHandler(commonViewControl1_DocumentationChanged);
            tabTransmitter.Controls.Add(transmitterViewControl1);
            tabTransmitter.Location = new Point(4, 0x16);
            tabTransmitter.Name = "tabTransmitter";
            tabTransmitter.Padding = new Padding(3);
            tabTransmitter.Size = new Size(0x31f, 0x1ed);
            tabTransmitter.TabIndex = 1;
            tabTransmitter.Text = "Transmitter";
            tabTransmitter.UseVisualStyleBackColor = true;
            transmitterViewControl1.Location = new Point(0, 0);
            transmitterViewControl1.Name = "transmitterViewControl1";
            transmitterViewControl1.OcpOn = true;
            int[] numArray8 = new int[4];
            numArray8[0] = 0x3e8;
            numArray8[3] = 0x10000;
            transmitterViewControl1.OcpTrim = new decimal(numArray8);
            int[] numArray9 = new int[4];
            numArray9[0] = 13;
            transmitterViewControl1.OutputPower = new decimal(numArray9);
            transmitterViewControl1.PaMode = PaModeEnum.PA0;
            transmitterViewControl1.Size = new Size(0x31f, 0x1ed);
            transmitterViewControl1.TabIndex = 0;
            transmitterViewControl1.PaModeChanged += new PaModeEventHandler(transmitterViewControl1_PaModeChanged);
            transmitterViewControl1.OutputPowerChanged += new DecimalEventHandler(transmitterViewControl1_OutputPowerChanged);
            transmitterViewControl1.PaRampChanged += new PaRampEventHandler(transmitterViewControl1_PaRampChanged);
            transmitterViewControl1.OcpOnChanged += new BooleanEventHandler(transmitterViewControl1_OcpOnChanged);
            transmitterViewControl1.OcpTrimChanged += new DecimalEventHandler(transmitterViewControl1_OcpTrimChanged);
            transmitterViewControl1.DocumentationChanged += new DocumentationChangedEventHandler(transmitterViewControl1_DocumentationChanged);
            tabReceiver.Controls.Add(receiverViewControl1);
            tabReceiver.Location = new Point(4, 0x16);
            tabReceiver.Name = "tabReceiver";
            tabReceiver.Padding = new Padding(3);
            tabReceiver.Size = new Size(0x31f, 0x1ed);
            tabReceiver.TabIndex = 2;
            tabReceiver.Text = "Receiver";
            tabReceiver.UseVisualStyleBackColor = true;
            receiverViewControl1.AfcAutoClearOn = true;
            receiverViewControl1.AfcAutoOn = true;
            receiverViewControl1.AfcDccFreq = new decimal(new int[] { -905921831, 0x11a1c6ed, 0x20241e22, 0x190000 });
            int[] numArray10 = new int[4];
            numArray10[0] = 0x679;
            receiverViewControl1.AfcDccFreqMax = new decimal(numArray10);
            int[] numArray11 = new int[4];
            numArray11[0] = 12;
            receiverViewControl1.AfcDccFreqMin = new decimal(numArray11);
            receiverViewControl1.AfcDone = false;
            receiverViewControl1.AfcLowBetaOn = true;
            int[] numArray12 = new int[4];
            numArray12[0] = 0xc350;
            receiverViewControl1.AfcRxBw = new decimal(numArray12);
            int[] numArray13 = new int[4];
            numArray13[0] = 0x61a80;
            receiverViewControl1.AfcRxBwMax = new decimal(numArray13);
            int[] numArray14 = new int[4];
            numArray14[0] = 0xc35;
            receiverViewControl1.AfcRxBwMin = new decimal(numArray14);
            int[] numArray15 = new int[4];
            numArray15[3] = 0x10000;
            receiverViewControl1.AfcValue = new decimal(numArray15);
            receiverViewControl1.AgcAutoRefOn = true;
            receiverViewControl1.AgcReference = -80;
            receiverViewControl1.AgcRefLevel = -80;
            receiverViewControl1.AgcSnrMargin = 5;
            receiverViewControl1.AgcStep1 = 0x10;
            receiverViewControl1.AgcStep2 = 7;
            receiverViewControl1.AgcStep3 = 11;
            receiverViewControl1.AgcStep4 = 9;
            receiverViewControl1.AgcStep5 = 11;
            receiverViewControl1.AgcThresh1 = 0;
            receiverViewControl1.AgcThresh2 = 0;
            receiverViewControl1.AgcThresh3 = 0;
            receiverViewControl1.AgcThresh4 = 0;
            receiverViewControl1.AgcThresh5 = 0;
            receiverViewControl1.AutoRxRestartOn = true;
            int[] numArray16 = new int[4];
            numArray16[0] = 0x12c0;
            receiverViewControl1.BitRate = new decimal(numArray16);
            receiverViewControl1.DagcOn = true;
            receiverViewControl1.DataMode = DataModeEnum.Packet;
            receiverViewControl1.DccFreq = new decimal(new int[] { -163586584, -1389046539, -2048070723, 0x1a0000 });
            int[] numArray17 = new int[4];
            numArray17[0] = 0x679;
            receiverViewControl1.DccFreqMax = new decimal(numArray17);
            int[] numArray18 = new int[4];
            numArray18[0] = 12;
            receiverViewControl1.DccFreqMin = new decimal(numArray18);
            receiverViewControl1.FastRx = true;
            receiverViewControl1.FeiDone = false;
            int[] numArray19 = new int[4];
            numArray19[3] = 0x10000;
            receiverViewControl1.FeiValue = new decimal(numArray19);
            int[] numArray20 = new int[4];
            numArray20[0] = 0x1e84800;
            receiverViewControl1.FrequencyXo = new decimal(numArray20);
            receiverViewControl1.LnaLowPowerOn = true;
            receiverViewControl1.LnaZin = LnaZinEnum.ZIN_200;
            receiverViewControl1.Location = new Point(0, 0);
            int[] numArray21 = new int[4];
            numArray21[3] = 0x10000;
            receiverViewControl1.LowBetaAfcOffset = new decimal(numArray21);
            receiverViewControl1.ModulationType = ModulationTypeEnum.FSK;
            receiverViewControl1.Name = "receiverViewControl1";
            receiverViewControl1.OokAverageThreshFilt = OokAverageThreshFiltEnum.COEF_2;
            receiverViewControl1.OokFixedThresh = 6;
            receiverViewControl1.OokPeakThreshDec = OokPeakThreshDecEnum.EVERY_1_CHIPS_1_TIMES;
            int[] numArray22 = new int[4];
            numArray22[0] = 5;
            numArray22[3] = 0x10000;
            receiverViewControl1.OokPeakThreshStep = new decimal(numArray22);
            receiverViewControl1.OokThreshType = OokThreshTypeEnum.Peak;
            receiverViewControl1.RssiAutoThresh = true;
            receiverViewControl1.RssiDone = false;
            int[] numArray23 = new int[4];
            numArray23[0] = 0x55;
            numArray23[3] = -2147483648;
            receiverViewControl1.RssiThresh = new decimal(numArray23);
            int[] numArray24 = new int[4];
            numArray24[0] = 0x4fb;
            numArray24[3] = -2147418112;
            receiverViewControl1.RssiValue = new decimal(numArray24);
            receiverViewControl1.RxBw = new decimal(new int[] { 0x70aaaaab, -2135170438, 0x21a876f7, 0x180000 });
            int[] numArray25 = new int[4];
            numArray25[0] = 0x7a120;
            receiverViewControl1.RxBwMax = new decimal(numArray25);
            int[] numArray26 = new int[4];
            numArray26[0] = 0xf42;
            receiverViewControl1.RxBwMin = new decimal(numArray26);
            receiverViewControl1.SensitivityBoostOn = true;
            receiverViewControl1.Size = new Size(0x31f, 0x1ed);
            receiverViewControl1.TabIndex = 0;
            int[] numArray27 = new int[4];
            receiverViewControl1.TimeoutRssiThresh = new decimal(numArray27);
            int[] numArray28 = new int[4];
            receiverViewControl1.TimeoutRxStart = new decimal(numArray28);
            receiverViewControl1.Version = "2.3";
            receiverViewControl1.AfcLowBetaOnChanged += new BooleanEventHandler(receiverViewControl1_AfcLowBetaOnChanged);
            receiverViewControl1.LowBetaAfcOffsetChanged += new DecimalEventHandler(receiverViewControl1_LowBetaAfcOffsetChanged);
            receiverViewControl1.SensitivityBoostOnChanged += new BooleanEventHandler(receiverViewControl1_SensitivityBoostOnChanged);
            receiverViewControl1.RssiAutoThreshChanged += new BooleanEventHandler(receiverViewControl1_RssiAutoThreshChanged);
            receiverViewControl1.DagcOnChanged += new BooleanEventHandler(receiverViewControl1_DagcOnChanged);
            receiverViewControl1.AgcAutoRefChanged += new BooleanEventHandler(receiverViewControl1_AgcAutoRefChanged);
            receiverViewControl1.AgcSnrMarginChanged += new ByteEventHandler(receiverViewControl1_AgcSnrMarginChanged);
            receiverViewControl1.AgcRefLevelChanged += new Int32EventHandler(receiverViewControl1_AgcRefLevelChanged);
            receiverViewControl1.AgcStepChanged += new AgcStepEventHandler(receiverViewControl1_AgcStepChanged);
            receiverViewControl1.LnaZinChanged += new LnaZinEventHandler(receiverViewControl1_LnaZinChanged);
            receiverViewControl1.LnaLowPowerOnChanged += new BooleanEventHandler(receiverViewControl1_LnaLowPowerOnChanged);
            receiverViewControl1.LnaGainChanged += new LnaGainEventHandler(receiverViewControl1_LnaGainChanged);
            receiverViewControl1.DccFreqChanged += new DecimalEventHandler(receiverViewControl1_DccFreqChanged);
            receiverViewControl1.RxBwChanged += new DecimalEventHandler(receiverViewControl1_RxBwChanged);
            receiverViewControl1.AfcDccFreqChanged += new DecimalEventHandler(receiverViewControl1_AfcDccFreqChanged);
            receiverViewControl1.AfcRxBwChanged += new DecimalEventHandler(receiverViewControl1_AfcRxBwChanged);
            receiverViewControl1.OokThreshTypeChanged += new OokThreshTypeEventHandler(receiverViewControl1_OokThreshTypeChanged);
            receiverViewControl1.OokPeakThreshStepChanged += new DecimalEventHandler(receiverViewControl1_OokPeakThreshStepChanged);
            receiverViewControl1.OokPeakThreshDecChanged += new OokPeakThreshDecEventHandler(receiverViewControl1_OokPeakThreshDecChanged);
            receiverViewControl1.OokAverageThreshFiltChanged += new OokAverageThreshFiltEventHandler(receiverViewControl1_OokAverageThreshFiltChanged);
            receiverViewControl1.OokFixedThreshChanged += new ByteEventHandler(receiverViewControl1_OokFixedThreshChanged);
            receiverViewControl1.FeiStartChanged += new EventHandler(receiverViewControl1_FeiStartChanged);
            receiverViewControl1.AfcAutoClearOnChanged += new BooleanEventHandler(receiverViewControl1_AfcAutoClearOnChanged);
            receiverViewControl1.AfcAutoOnChanged += new BooleanEventHandler(receiverViewControl1_AfcAutoOnChanged);
            receiverViewControl1.AfcClearChanged += new EventHandler(receiverViewControl1_AfcClearChanged);
            receiverViewControl1.AfcStartChanged += new EventHandler(receiverViewControl1_AfcStartChanged);
            receiverViewControl1.FastRxChanged += new BooleanEventHandler(receiverViewControl1_FastRxChanged);
            receiverViewControl1.RssiThreshChanged += new DecimalEventHandler(receiverViewControl1_RssiThreshChanged);
            receiverViewControl1.RssiStartChanged += new EventHandler(receiverViewControl1_RssiStartChanged);
            receiverViewControl1.TimeoutRxStartChanged += new DecimalEventHandler(receiverViewControl1_TimeoutRxStartChanged);
            receiverViewControl1.TimeoutRssiThreshChanged += new DecimalEventHandler(receiverViewControl1_TimeoutRssiThreshChanged);
            receiverViewControl1.RestartRxChanged += new EventHandler(receiverViewControl1_RestartRxChanged);
            receiverViewControl1.AutoRxRestartOnChanged += new BooleanEventHandler(receiverViewControl1_AutoRxRestartOnChanged);
            receiverViewControl1.DocumentationChanged += new DocumentationChangedEventHandler(receiverViewControl1_DocumentationChanged);
            tabIrqMap.Controls.Add(irqMapViewControl1);
            tabIrqMap.Location = new Point(4, 0x16);
            tabIrqMap.Name = "tabIrqMap";
            tabIrqMap.Padding = new Padding(3);
            tabIrqMap.Size = new Size(0x31f, 0x1ed);
            tabIrqMap.TabIndex = 3;
            tabIrqMap.Text = "IRQ & Map";
            tabIrqMap.UseVisualStyleBackColor = true;
            irqMapViewControl1.AutoMode = false;
            irqMapViewControl1.CrcOk = false;
            irqMapViewControl1.DataMode = DataModeEnum.Packet;
            irqMapViewControl1.FifoFull = false;
            irqMapViewControl1.FifoLevel = false;
            irqMapViewControl1.FifoNotEmpty = false;
            irqMapViewControl1.FifoOverrun = false;
            int[] numArray29 = new int[4];
            numArray29[0] = 0x1e84800;
            irqMapViewControl1.FrequencyXo = new decimal(numArray29);
            irqMapViewControl1.Location = new Point(0, 0);
            irqMapViewControl1.LowBat = false;
            irqMapViewControl1.Mode = OperatingModeEnum.Stdby;
            irqMapViewControl1.ModeReady = false;
            irqMapViewControl1.Name = "irqMapViewControl1";
            irqMapViewControl1.PacketSent = false;
            irqMapViewControl1.PayloadReady = false;
            irqMapViewControl1.PllLock = false;
            irqMapViewControl1.Rssi = false;
            irqMapViewControl1.RxReady = false;
            irqMapViewControl1.Size = new Size(0x31f, 0x1ed);
            irqMapViewControl1.SyncAddressMatch = false;
            irqMapViewControl1.TabIndex = 0;
            irqMapViewControl1.Timeout = false;
            irqMapViewControl1.TxReady = false;
            irqMapViewControl1.DioMappingChanged += new DioMappingEventHandler(irqMapViewControl1_DioMappingChanged);
            irqMapViewControl1.ClockOutChanged += new ClockOutEventHandler(irqMapViewControl1_ClockOutChanged);
            irqMapViewControl1.DocumentationChanged += new DocumentationChangedEventHandler(irqMapViewControl1_DocumentationChanged);
            tabPacketHandler.Controls.Add(packetHandlerView1);
            tabPacketHandler.Location = new Point(4, 0x16);
            tabPacketHandler.Name = "tabPacketHandler";
            tabPacketHandler.Padding = new Padding(3);
            tabPacketHandler.Size = new Size(0x31f, 0x1ed);
            tabPacketHandler.TabIndex = 4;
            tabPacketHandler.Text = "Packet Handler";
            tabPacketHandler.UseVisualStyleBackColor = true;
            packetHandlerView1.AddressFiltering = AddressFilteringEnum.OFF;
            packetHandlerView1.AesKey = new byte[0x10];
            packetHandlerView1.AesOn = true;
            int[] numArray30 = new int[4];
            packetHandlerView1.BitRate = new decimal(numArray30);
            packetHandlerView1.BroadcastAddress = 0;
            packetHandlerView1.Crc = 0;
            packetHandlerView1.CrcAutoClearOff = false;
            packetHandlerView1.CrcOn = true;
            packetHandlerView1.DataMode = DataModeEnum.Packet;
            packetHandlerView1.DcFree = DcFreeEnum.OFF;
            packetHandlerView1.EnterCondition = EnterConditionEnum.OFF;
            packetHandlerView1.ExitCondition = ExitConditionEnum.OFF;
            packetHandlerView1.FifoFillCondition = FifoFillConditionEnum.OnSyncAddressIrq;
            packetHandlerView1.FifoThreshold = 15;
            packetHandlerView1.IntermediateMode = IntermediateModeEnum.Sleep;
            packetHandlerView1.InterPacketRxDelay = 0;
            packetHandlerView1.Location = new Point(0, 0);
            packetHandlerView1.MaxPacketNumber = 0;
            packetHandlerView1.Message = new byte[0];
            packetHandlerView1.MessageLength = 0;
            packetHandlerView1.Mode = OperatingModeEnum.Stdby;
            packetHandlerView1.Name = "packetHandlerView1";
            packetHandlerView1.NodeAddress = 0;
            packetHandlerView1.NodeAddressRx = 0;
            packetHandlerView1.PacketFormat = PacketFormatEnum.Fixed;
            packetHandlerView1.PacketNumber = 0;
            packetHandlerView1.PayloadLength = 0x42;
            packetHandlerView1.PreambleSize = 3;
            packetHandlerView1.Size = new Size(0x31f, 0x1ed);
            packetHandlerView1.StartStop = false;
            packetHandlerView1.SyncOn = true;
            packetHandlerView1.SyncSize = 4;
            packetHandlerView1.SyncTol = 0;
            packetHandlerView1.SyncValue = new byte[] { 170, 170, 170, 170 };
            packetHandlerView1.TabIndex = 0;
            packetHandlerView1.TxStartCondition = true;
            packetHandlerView1.Error += new SemtechLib.General.Events.ErrorEventHandler(packetHandlerView1_Error);
            packetHandlerView1.PreambleSizeChanged += new Int32EventHandler(packetHandlerView1_PreambleSizeChanged);
            packetHandlerView1.SyncOnChanged += new BooleanEventHandler(packetHandlerView1_SyncOnChanged);
            packetHandlerView1.FifoFillConditionChanged += new FifoFillConditionEventHandler(packetHandlerView1_FifoFillConditionChanged);
            packetHandlerView1.SyncSizeChanged += new ByteEventHandler(packetHandlerView1_SyncSizeChanged);
            packetHandlerView1.SyncTolChanged += new ByteEventHandler(packetHandlerView1_SyncTolChanged);
            packetHandlerView1.SyncValueChanged += new ByteArrayEventHandler(packetHandlerView1_SyncValueChanged);
            packetHandlerView1.PacketFormatChanged += new PacketFormatEventHandler(packetHandlerView1_PacketFormatChanged);
            packetHandlerView1.DcFreeChanged += new DcFreeEventHandler(packetHandlerView1_DcFreeChanged);
            packetHandlerView1.CrcOnChanged += new BooleanEventHandler(packetHandlerView1_CrcOnChanged);
            packetHandlerView1.CrcAutoClearOffChanged += new BooleanEventHandler(packetHandlerView1_CrcAutoClearOffChanged);
            packetHandlerView1.AddressFilteringChanged += new AddressFilteringEventHandler(packetHandlerView1_AddressFilteringChanged);
            packetHandlerView1.PayloadLengthChanged += new ByteEventHandler(packetHandlerView1_PayloadLengthChanged);
            packetHandlerView1.NodeAddressChanged += new ByteEventHandler(packetHandlerView1_NodeAddressChanged);
            packetHandlerView1.BroadcastAddressChanged += new ByteEventHandler(packetHandlerView1_BroadcastAddressChanged);
            packetHandlerView1.EnterConditionChanged += new EnterConditionEventHandler(packetHandlerView1_EnterConditionChanged);
            packetHandlerView1.ExitConditionChanged += new ExitConditionEventHandler(packetHandlerView1_ExitConditionChanged);
            packetHandlerView1.IntermediateModeChanged += new IntermediateModeEventHandler(packetHandlerView1_IntermediateModeChanged);
            packetHandlerView1.TxStartConditionChanged += new BooleanEventHandler(packetHandlerView1_TxStartConditionChanged);
            packetHandlerView1.FifoThresholdChanged += new ByteEventHandler(packetHandlerView1_FifoThresholdChanged);
            packetHandlerView1.InterPacketRxDelayChanged += new Int32EventHandler(packetHandlerView1_InterPacketRxDelayChanged);
            packetHandlerView1.AesOnChanged += new BooleanEventHandler(packetHandlerView1_AesOnChanged);
            packetHandlerView1.AesKeyChanged += new ByteArrayEventHandler(packetHandlerView1_AesKeyChanged);
            packetHandlerView1.MessageLengthChanged += new Int32EventHandler(packetHandlerView1_MessageLengthChanged);
            packetHandlerView1.MessageChanged += new ByteArrayEventHandler(packetHandlerView1_MessageChanged);
            packetHandlerView1.StartStopChanged += new BooleanEventHandler(packetHandlerView1_StartStopChanged);
            packetHandlerView1.MaxPacketNumberChanged += new Int32EventHandler(packetHandlerView1_MaxPacketNumberChanged);
            packetHandlerView1.PacketHandlerLogEnableChanged += new BooleanEventHandler(packetHandlerView1_PacketHandlerLogEnableChanged);
            packetHandlerView1.DocumentationChanged += new DocumentationChangedEventHandler(packetHandlerView1_DocumentationChanged);
            tabTemperature.Controls.Add(temperatureViewControl1);
            tabTemperature.Location = new Point(4, 0x16);
            tabTemperature.Name = "tabTemperature";
            tabTemperature.Padding = new Padding(3);
            tabTemperature.Size = new Size(0x31f, 0x1ed);
            tabTemperature.TabIndex = 5;
            tabTemperature.Text = "Temperature";
            tabTemperature.UseVisualStyleBackColor = true;
            temperatureViewControl1.AdcLowPowerOn = true;
            temperatureViewControl1.Location = new Point(0, 0);
            temperatureViewControl1.Mode = OperatingModeEnum.Stdby;
            temperatureViewControl1.Name = "temperatureViewControl1";
            temperatureViewControl1.Size = new Size(0x31f, 0x1ed);
            temperatureViewControl1.TabIndex = 0;
            temperatureViewControl1.TempCalDone = false;
            temperatureViewControl1.TempMeasRunning = false;
            int[] numArray31 = new int[4];
            numArray31[0] = 0x19;
            temperatureViewControl1.TempValue = new decimal(numArray31);
            int[] numArray32 = new int[4];
            numArray32[0] = 250;
            numArray32[3] = 0x10000;
            temperatureViewControl1.TempValueRoom = new decimal(numArray32);
            temperatureViewControl1.AdcLowPowerOnChanged += new BooleanEventHandler(temperatureViewControl1_AdcLowPowerOnChanged);
            temperatureViewControl1.TempCalibrateChanged += new DecimalEventHandler(temperatureViewControl1_TempCalibrateChanged);
            temperatureViewControl1.DocumentationChanged += new DocumentationChangedEventHandler(temperatureViewControl1_DocumentationChanged);
            gBoxOperatingMode.Controls.Add(rBtnTransmitter);
            gBoxOperatingMode.Controls.Add(rBtnReceiver);
            gBoxOperatingMode.Controls.Add(rBtnSynthesizer);
            gBoxOperatingMode.Controls.Add(rBtnStandby);
            gBoxOperatingMode.Controls.Add(rBtnSleep);
            gBoxOperatingMode.Location = new Point(0x330, 0x19b);
            gBoxOperatingMode.Name = "gBoxOperatingMode";
            gBoxOperatingMode.Size = new Size(0xbd, 0x6b);
            gBoxOperatingMode.TabIndex = 2;
            gBoxOperatingMode.TabStop = false;
            gBoxOperatingMode.Text = "Operating mode";
            gBoxOperatingMode.MouseEnter += new EventHandler(control_MouseEnter);
            gBoxOperatingMode.MouseLeave += new EventHandler(control_MouseLeave);
            rBtnTransmitter.AutoSize = true;
            rBtnTransmitter.Location = new Point(0x5e, 80);
            rBtnTransmitter.Name = "rBtnTransmitter";
            rBtnTransmitter.Size = new Size(0x4d, 0x11);
            rBtnTransmitter.TabIndex = 4;
            rBtnTransmitter.Text = "Transmitter";
            rBtnTransmitter.UseVisualStyleBackColor = true;
            rBtnTransmitter.CheckedChanged += new EventHandler(rBtnOperatingMode_CheckedChanged);
            rBtnReceiver.AutoSize = true;
            rBtnReceiver.Location = new Point(0x10, 80);
            rBtnReceiver.Name = "rBtnReceiver";
            rBtnReceiver.Size = new Size(0x44, 0x11);
            rBtnReceiver.TabIndex = 3;
            rBtnReceiver.Text = "Receiver";
            rBtnReceiver.UseVisualStyleBackColor = true;
            rBtnReceiver.CheckedChanged += new EventHandler(rBtnOperatingMode_CheckedChanged);
            rBtnSynthesizer.AutoSize = true;
            rBtnSynthesizer.Location = new Point(0x5e, 0x33);
            rBtnSynthesizer.Name = "rBtnSynthesizer";
            rBtnSynthesizer.Size = new Size(0x4f, 0x11);
            rBtnSynthesizer.TabIndex = 2;
            rBtnSynthesizer.Text = "Synthesizer";
            rBtnSynthesizer.UseVisualStyleBackColor = true;
            rBtnSynthesizer.CheckedChanged += new EventHandler(rBtnOperatingMode_CheckedChanged);
            rBtnStandby.AutoSize = true;
            rBtnStandby.Checked = true;
            rBtnStandby.Location = new Point(0x10, 0x33);
            rBtnStandby.Name = "rBtnStandby";
            rBtnStandby.Size = new Size(0x40, 0x11);
            rBtnStandby.TabIndex = 1;
            rBtnStandby.TabStop = true;
            rBtnStandby.Text = "Standby";
            rBtnStandby.UseVisualStyleBackColor = true;
            rBtnStandby.CheckedChanged += new EventHandler(rBtnOperatingMode_CheckedChanged);
            rBtnSleep.AutoSize = true;
            rBtnSleep.Location = new Point(0x10, 20);
            rBtnSleep.Name = "rBtnSleep";
            rBtnSleep.Size = new Size(0x34, 0x11);
            rBtnSleep.TabIndex = 0;
            rBtnSleep.Text = "Sleep";
            rBtnSleep.UseVisualStyleBackColor = true;
            rBtnSleep.CheckedChanged += new EventHandler(rBtnOperatingMode_CheckedChanged);
            lbModeReady.AutoSize = true;
            lbModeReady.Location = new Point(0x37, 20);
            lbModeReady.Name = "lbModeReady";
            lbModeReady.Size = new Size(0x41, 13);
            lbModeReady.TabIndex = 1;
            lbModeReady.Text = "ModeReady";
            label19.AutoSize = true;
            label19.Location = new Point(0x37, 0x29);
            label19.Name = "label19";
            label19.Size = new Size(0x33, 13);
            label19.TabIndex = 3;
            label19.Text = "RxReady";
            label18.AutoSize = true;
            label18.Location = new Point(0x37, 0x3e);
            label18.Name = "label18";
            label18.Size = new Size(50, 13);
            label18.TabIndex = 5;
            label18.Text = "TxReady";
            label17.AutoSize = true;
            label17.Location = new Point(0x37, 0x53);
            label17.Name = "label17";
            label17.Size = new Size(0x2a, 13);
            label17.TabIndex = 7;
            label17.Text = "PllLock";
            label23.AutoSize = true;
            label23.Location = new Point(0x37, 110);
            label23.Name = "label23";
            label23.Size = new Size(0x1b, 13);
            label23.TabIndex = 9;
            label23.Text = "Rssi";
            label22.AutoSize = true;
            label22.Location = new Point(0x37, 0x83);
            label22.Name = "label22";
            label22.Size = new Size(0x2d, 13);
            label22.TabIndex = 11;
            label22.Text = "Timeout";
            label21.AutoSize = true;
            label21.Location = new Point(0x37, 0x98);
            label21.Name = "label21";
            label21.Size = new Size(0x38, 13);
            label21.TabIndex = 13;
            label21.Text = "AutoMode";
            label20.AutoSize = true;
            label20.Location = new Point(0x37, 0xad);
            label20.Name = "label20";
            label20.Size = new Size(0x63, 13);
            label20.TabIndex = 15;
            label20.Text = "SyncAddressMatch";
            label27.AutoSize = true;
            label27.Location = new Point(0x37, 200);
            label27.Name = "label27";
            label27.Size = new Size(40, 13);
            label27.TabIndex = 0x11;
            label27.Text = "FifoFull";
            label26.AutoSize = true;
            label26.Location = new Point(0x37, 0xdd);
            label26.Name = "label26";
            label26.Size = new Size(70, 13);
            label26.TabIndex = 0x13;
            label26.Text = "FifoNotEmpty";
            label25.AutoSize = true;
            label25.Location = new Point(0x37, 0xf2);
            label25.Name = "label25";
            label25.Size = new Size(50, 13);
            label25.TabIndex = 0x15;
            label25.Text = "FifoLevel";
            label24.AutoSize = true;
            label24.Location = new Point(0x37, 0x107);
            label24.Name = "label24";
            label24.Size = new Size(0x3e, 13);
            label24.TabIndex = 0x17;
            label24.Text = "FifoOverrun";
            label31.AutoSize = true;
            label31.Location = new Point(0x37, 290);
            label31.Name = "label31";
            label31.Size = new Size(0x3f, 13);
            label31.TabIndex = 0x19;
            label31.Text = "PacketSent";
            label30.AutoSize = true;
            label30.Location = new Point(0x37, 0x137);
            label30.Name = "label30";
            label30.Size = new Size(0x4c, 13);
            label30.TabIndex = 0x1b;
            label30.Text = "PayloadReady";
            label29.AutoSize = true;
            label29.Location = new Point(0x37, 0x14c);
            label29.Name = "label29";
            label29.Size = new Size(0x25, 13);
            label29.TabIndex = 0x1d;
            label29.Text = "CrcOk";
            label28.AutoSize = true;
            label28.Location = new Point(0x37, 0x161);
            label28.Name = "label28";
            label28.Size = new Size(0x2b, 13);
            label28.TabIndex = 0x1f;
            label28.Text = "LowBat";
            gBoxIrqFlags.Controls.Add(ledLowBat);
            gBoxIrqFlags.Controls.Add(lbModeReady);
            gBoxIrqFlags.Controls.Add(ledCrcOk);
            gBoxIrqFlags.Controls.Add(ledRxReady);
            gBoxIrqFlags.Controls.Add(ledPayloadReady);
            gBoxIrqFlags.Controls.Add(ledTxReady);
            gBoxIrqFlags.Controls.Add(ledPacketSent);
            gBoxIrqFlags.Controls.Add(label17);
            gBoxIrqFlags.Controls.Add(label31);
            gBoxIrqFlags.Controls.Add(ledPllLock);
            gBoxIrqFlags.Controls.Add(label30);
            gBoxIrqFlags.Controls.Add(label18);
            gBoxIrqFlags.Controls.Add(label29);
            gBoxIrqFlags.Controls.Add(label19);
            gBoxIrqFlags.Controls.Add(label28);
            gBoxIrqFlags.Controls.Add(ledModeReady);
            gBoxIrqFlags.Controls.Add(ledFifoOverrun);
            gBoxIrqFlags.Controls.Add(ledRssi);
            gBoxIrqFlags.Controls.Add(ledFifoLevel);
            gBoxIrqFlags.Controls.Add(ledTimeout);
            gBoxIrqFlags.Controls.Add(label27);
            gBoxIrqFlags.Controls.Add(label20);
            gBoxIrqFlags.Controls.Add(ledFifoNotEmpty);
            gBoxIrqFlags.Controls.Add(label21);
            gBoxIrqFlags.Controls.Add(label26);
            gBoxIrqFlags.Controls.Add(ledAutoMode);
            gBoxIrqFlags.Controls.Add(label25);
            gBoxIrqFlags.Controls.Add(label22);
            gBoxIrqFlags.Controls.Add(label24);
            gBoxIrqFlags.Controls.Add(label23);
            gBoxIrqFlags.Controls.Add(ledFifoFull);
            gBoxIrqFlags.Controls.Add(ledSyncAddressMatch);
            gBoxIrqFlags.Location = new Point(0x330, 0x19);
            gBoxIrqFlags.Name = "gBoxIrqFlags";
            gBoxIrqFlags.Size = new Size(0xbd, 380);
            gBoxIrqFlags.TabIndex = 1;
            gBoxIrqFlags.TabStop = false;
            gBoxIrqFlags.Text = "Irq flags";
            gBoxIrqFlags.MouseEnter += new EventHandler(control_MouseEnter);
            gBoxIrqFlags.MouseLeave += new EventHandler(control_MouseLeave);
            ledLowBat.BackColor = Color.Transparent;
            ledLowBat.LedColor = Color.Green;
            ledLowBat.LedSize = new Size(11, 11);
            ledLowBat.Location = new Point(0x22, 0x160);
            ledLowBat.Name = "ledLowBat";
            ledLowBat.Size = new Size(15, 15);
            ledLowBat.TabIndex = 30;
            ledLowBat.Text = "led1";
            ledCrcOk.BackColor = Color.Transparent;
            ledCrcOk.LedColor = Color.Green;
            ledCrcOk.LedSize = new Size(11, 11);
            ledCrcOk.Location = new Point(0x22, 0x14b);
            ledCrcOk.Name = "ledCrcOk";
            ledCrcOk.Size = new Size(15, 15);
            ledCrcOk.TabIndex = 0x1c;
            ledCrcOk.Text = "led1";
            ledRxReady.BackColor = Color.Transparent;
            ledRxReady.LedColor = Color.Green;
            ledRxReady.LedSize = new Size(11, 11);
            ledRxReady.Location = new Point(0x22, 40);
            ledRxReady.Name = "ledRxReady";
            ledRxReady.Size = new Size(15, 15);
            ledRxReady.TabIndex = 2;
            ledRxReady.Text = "led1";
            ledPayloadReady.BackColor = Color.Transparent;
            ledPayloadReady.LedColor = Color.Green;
            ledPayloadReady.LedSize = new Size(11, 11);
            ledPayloadReady.Location = new Point(0x22, 310);
            ledPayloadReady.Name = "ledPayloadReady";
            ledPayloadReady.Size = new Size(15, 15);
            ledPayloadReady.TabIndex = 0x1a;
            ledPayloadReady.Text = "led1";
            ledTxReady.BackColor = Color.Transparent;
            ledTxReady.LedColor = Color.Green;
            ledTxReady.LedSize = new Size(11, 11);
            ledTxReady.Location = new Point(0x22, 0x3d);
            ledTxReady.Name = "ledTxReady";
            ledTxReady.Size = new Size(15, 15);
            ledTxReady.TabIndex = 4;
            ledTxReady.Text = "led1";
            ledPacketSent.BackColor = Color.Transparent;
            ledPacketSent.LedColor = Color.Green;
            ledPacketSent.LedSize = new Size(11, 11);
            ledPacketSent.Location = new Point(0x22, 0x121);
            ledPacketSent.Margin = new Padding(3, 6, 3, 3);
            ledPacketSent.Name = "ledPacketSent";
            ledPacketSent.Size = new Size(15, 15);
            ledPacketSent.TabIndex = 0x18;
            ledPacketSent.Text = "led1";
            ledPllLock.BackColor = Color.Transparent;
            ledPllLock.LedColor = Color.Green;
            ledPllLock.LedSize = new Size(11, 11);
            ledPllLock.Location = new Point(0x22, 0x52);
            ledPllLock.Margin = new Padding(3, 3, 3, 6);
            ledPllLock.Name = "ledPllLock";
            ledPllLock.Size = new Size(15, 15);
            ledPllLock.TabIndex = 6;
            ledPllLock.Text = "led1";
            ledModeReady.BackColor = Color.Transparent;
            ledModeReady.LedColor = Color.Green;
            ledModeReady.LedSize = new Size(11, 11);
            ledModeReady.Location = new Point(0x22, 0x13);
            ledModeReady.Name = "ledModeReady";
            ledModeReady.Size = new Size(15, 15);
            ledModeReady.TabIndex = 0;
            ledModeReady.Text = "Mode Ready";
            ledFifoOverrun.BackColor = Color.Transparent;
            ledFifoOverrun.LedColor = Color.Green;
            ledFifoOverrun.LedSize = new Size(11, 11);
            ledFifoOverrun.Location = new Point(0x22, 0x106);
            ledFifoOverrun.Margin = new Padding(3, 3, 3, 6);
            ledFifoOverrun.Name = "ledFifoOverrun";
            ledFifoOverrun.Size = new Size(15, 15);
            ledFifoOverrun.TabIndex = 0x16;
            ledFifoOverrun.Text = "led1";
            ledRssi.BackColor = Color.Transparent;
            ledRssi.LedColor = Color.Green;
            ledRssi.LedSize = new Size(11, 11);
            ledRssi.Location = new Point(0x22, 0x6d);
            ledRssi.Margin = new Padding(3, 6, 3, 3);
            ledRssi.Name = "ledRssi";
            ledRssi.Size = new Size(15, 15);
            ledRssi.TabIndex = 8;
            ledRssi.Text = "led1";
            ledFifoLevel.BackColor = Color.Transparent;
            ledFifoLevel.LedColor = Color.Green;
            ledFifoLevel.LedSize = new Size(11, 11);
            ledFifoLevel.Location = new Point(0x22, 0xf1);
            ledFifoLevel.Name = "ledFifoLevel";
            ledFifoLevel.Size = new Size(15, 15);
            ledFifoLevel.TabIndex = 20;
            ledFifoLevel.Text = "led1";
            ledTimeout.BackColor = Color.Transparent;
            ledTimeout.LedColor = Color.Green;
            ledTimeout.LedSize = new Size(11, 11);
            ledTimeout.Location = new Point(0x22, 130);
            ledTimeout.Name = "ledTimeout";
            ledTimeout.Size = new Size(15, 15);
            ledTimeout.TabIndex = 10;
            ledTimeout.Text = "led1";
            ledFifoNotEmpty.BackColor = Color.Transparent;
            ledFifoNotEmpty.LedColor = Color.Green;
            ledFifoNotEmpty.LedSize = new Size(11, 11);
            ledFifoNotEmpty.Location = new Point(0x22, 220);
            ledFifoNotEmpty.Name = "ledFifoNotEmpty";
            ledFifoNotEmpty.Size = new Size(15, 15);
            ledFifoNotEmpty.TabIndex = 0x12;
            ledFifoNotEmpty.Text = "led1";
            ledAutoMode.BackColor = Color.Transparent;
            ledAutoMode.LedColor = Color.Green;
            ledAutoMode.LedSize = new Size(11, 11);
            ledAutoMode.Location = new Point(0x22, 0x97);
            ledAutoMode.Name = "ledAutoMode";
            ledAutoMode.Size = new Size(15, 15);
            ledAutoMode.TabIndex = 12;
            ledAutoMode.Text = "led1";
            ledFifoFull.BackColor = Color.Transparent;
            ledFifoFull.LedColor = Color.Green;
            ledFifoFull.LedSize = new Size(11, 11);
            ledFifoFull.Location = new Point(0x22, 0xc7);
            ledFifoFull.Margin = new Padding(3, 6, 3, 3);
            ledFifoFull.Name = "ledFifoFull";
            ledFifoFull.Size = new Size(15, 15);
            ledFifoFull.TabIndex = 0x10;
            ledFifoFull.Text = "led1";
            ledSyncAddressMatch.BackColor = Color.Transparent;
            ledSyncAddressMatch.LedColor = Color.Green;
            ledSyncAddressMatch.LedSize = new Size(11, 11);
            ledSyncAddressMatch.Location = new Point(0x22, 0xac);
            ledSyncAddressMatch.Margin = new Padding(3, 3, 3, 6);
            ledSyncAddressMatch.Name = "ledSyncAddressMatch";
            ledSyncAddressMatch.Size = new Size(15, 15);
            ledSyncAddressMatch.TabIndex = 14;
            ledSyncAddressMatch.Text = "led1";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.Controls.Add(gBoxOperatingMode);
            base.Controls.Add(tabControl1);
            base.Controls.Add(gBoxIrqFlags);
            base.Name = "DeviceViewControl";
            base.Size = new Size(0x3f0, 0x20d);
            tabControl1.ResumeLayout(false);
            tabCommon.ResumeLayout(false);
            tabTransmitter.ResumeLayout(false);
            tabReceiver.ResumeLayout(false);
            tabIrqMap.ResumeLayout(false);
            tabPacketHandler.ResumeLayout(false);
            tabTemperature.ResumeLayout(false);
            gBoxOperatingMode.ResumeLayout(false);
            gBoxOperatingMode.PerformLayout();
            gBoxIrqFlags.ResumeLayout(false);
            gBoxIrqFlags.PerformLayout();
            base.ResumeLayout(false);
        }
		#endregion

		private void irqMapViewControl1_ClockOutChanged(object sender, ClockOutEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetClockOut(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void irqMapViewControl1_DioMappingChanged(object sender, DioMappingEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetDioMapping(e.Id, e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void irqMapViewControl1_DocumentationChanged(object sender, DocumentationChangedEventArgs e)
        {
            OnDocumentationChanged(e);
        }

        private void OnDocumentationChanged(DocumentationChangedEventArgs e)
        {
            if (DocumentationChanged != null)
                DocumentationChanged(this, e);
        }

        private void OnError(byte status, string message)
        {
            if (Error != null)
                Error(this, new ErrorEventArgs(status, message));
        }

        private void OnSX1231PacketHandlerReceived(object sender, PacketStatusEventArg e)
        {
            packetHandlerView1.PacketNumber = e.Number;
        }

        private void OnSX1231PacketHandlerStarted(object sender, EventArgs e)
        {
            DisableControls();
        }

        private void OnSX1231PacketHandlerStoped(object sender, EventArgs e)
        {
            EnableControls();
            packetHandlerView1.StartStop = false;
        }

        private void OnSX1231PacketHandlerTransmitted(object sender, PacketStatusEventArg e)
        {
            packetHandlerView1.PacketNumber = e.Number;
        }

        private void OnSX1231PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "FrequencyXo":
                    commonViewControl1.FrequencyXo = sx1231.FrequencyXo;
                    receiverViewControl1.FrequencyXo = sx1231.FrequencyXo;
                    irqMapViewControl1.FrequencyXo = sx1231.FrequencyXo;
                    return;

                case "SpectrumOn":
                    if (!sx1231.SpectrumOn)
                    {
                        EnableControls();
                        packetHandlerView1.Enabled = true;
                        return;
                    }
                    DisableControls();
                    packetHandlerView1.Enabled = false;
                    return;

                case "Sequencer":
                    commonViewControl1.Sequencer = sx1231.Sequencer;
                    return;

                case "Listen":
                    commonViewControl1.ListenMode = sx1231.ListenMode;
                    return;

                case "Mode":
                    switch (sx1231.Mode)
                    {
                        case OperatingModeEnum.Sleep:
                            rBtnSleep.Checked = true;
                            goto Label_0A80;

                        case OperatingModeEnum.Stdby:
                            rBtnStandby.Checked = true;
                            goto Label_0A80;

                        case OperatingModeEnum.Fs:
                            rBtnSynthesizer.Checked = true;
                            goto Label_0A80;

                        case OperatingModeEnum.Tx:
                            rBtnTransmitter.Checked = true;
                            goto Label_0A80;

                        case OperatingModeEnum.Rx:
                            rBtnReceiver.Checked = true;
                            goto Label_0A80;
                    }
                    break;

                case "DataMode":
                    commonViewControl1.DataMode = sx1231.DataMode;
                    receiverViewControl1.DataMode = sx1231.DataMode;
                    irqMapViewControl1.DataMode = sx1231.DataMode;
                    packetHandlerView1.DataMode = sx1231.DataMode;
                    return;

                case "ModulationType":
                    commonViewControl1.ModulationType = sx1231.ModulationType;
                    receiverViewControl1.ModulationType = sx1231.ModulationType;
                    return;

                case "ModulationShaping":
                    commonViewControl1.ModulationShaping = sx1231.ModulationShaping;
                    return;

                case "BitRate":
                    commonViewControl1.BitRate = sx1231.BitRate;
                    receiverViewControl1.BitRate = sx1231.BitRate;
                    packetHandlerView1.BitRate = sx1231.BitRate;
                    return;

                case "Fdev":
                    commonViewControl1.Fdev = sx1231.Fdev;
                    return;

                case "FrequencyStep":
                    commonViewControl1.FrequencyStep = sx1231.FrequencyStep;
                    return;

                case "FrequencyRf":
                    commonViewControl1.FrequencyRf = sx1231.FrequencyRf;
                    return;

                case "RcCalDone":
                    commonViewControl1.RcCalDone = sx1231.RcCalDone;
                    return;

                case "LowBatMonitor":
                    commonViewControl1.LowBatMonitor = sx1231.LowBatMonitor;
                    return;

                case "LowBatOn":
                    commonViewControl1.LowBatOn = sx1231.LowBatOn;
                    return;

                case "LowBatTrim":
                    commonViewControl1.LowBatTrim = sx1231.LowBatTrim;
                    return;

                case "ListenResolIdle":
                    commonViewControl1.ListenResolIdle = sx1231.ListenResolIdle;
                    return;

                case "ListenResolRx":
                    commonViewControl1.ListenResolRx = sx1231.ListenResolRx;
                    return;

                case "ListenCriteria":
                    commonViewControl1.ListenCriteria = sx1231.ListenCriteria;
                    return;

                case "ListenEnd":
                    commonViewControl1.ListenEnd = sx1231.ListenEnd;
                    return;

                case "ListenCoefIdle":
                    commonViewControl1.ListenCoefIdle = sx1231.ListenCoefIdle;
                    return;

                case "ListenCoefRx":
                    commonViewControl1.ListenCoefRx = sx1231.ListenCoefRx;
                    return;

                case "Version":
                    commonViewControl1.Version = sx1231.Version;
                    receiverViewControl1.Version = sx1231.Version;
                    return;

                case "PaMode":
                    transmitterViewControl1.PaMode = sx1231.PaMode;
                    return;

                case "OutputPower":
                    transmitterViewControl1.OutputPower = sx1231.OutputPower;
                    return;

                case "PaRamp":
                    transmitterViewControl1.PaRamp = sx1231.PaRamp;
                    return;

                case "OcpOn":
                    transmitterViewControl1.OcpOn = sx1231.OcpOn;
                    return;

                case "OcpTrim":
                    transmitterViewControl1.OcpTrim = sx1231.OcpTrim;
                    return;

                case "AfcLowBetaOn":
                    receiverViewControl1.AfcLowBetaOn = sx1231.AfcLowBetaOn;
                    return;

                case "SensitivityBoostOn":
                    receiverViewControl1.SensitivityBoostOn = sx1231.SensitivityBoostOn;
                    return;

                case "LowBetaAfcOffset":
                    receiverViewControl1.LowBetaAfcOffset = sx1231.LowBetaAfcOffset;
                    return;

                case "RssiAutoThresh":
                    receiverViewControl1.RssiAutoThresh = sx1231.RssiAutoThresh;
                    return;

                case "DagcOn":
                    receiverViewControl1.DagcOn = sx1231.DagcOn;
                    return;

                case "AgcReference":
                    receiverViewControl1.AgcReference = sx1231.AgcReference;
                    return;

                case "AgcThresh1":
                    receiverViewControl1.AgcThresh1 = sx1231.AgcThresh1;
                    return;

                case "AgcThresh2":
                    receiverViewControl1.AgcThresh2 = sx1231.AgcThresh2;
                    return;

                case "AgcThresh3":
                    receiverViewControl1.AgcThresh3 = sx1231.AgcThresh3;
                    return;

                case "AgcThresh4":
                    receiverViewControl1.AgcThresh4 = sx1231.AgcThresh4;
                    return;

                case "AgcThresh5":
                    receiverViewControl1.AgcThresh5 = sx1231.AgcThresh5;
                    return;

                case "DccFreqMin":
                    receiverViewControl1.DccFreqMin = sx1231.DccFreqMin;
                    return;

                case "DccFreqMax":
                    receiverViewControl1.DccFreqMax = sx1231.DccFreqMax;
                    return;

                case "RxBwMin":
                    receiverViewControl1.RxBwMin = sx1231.RxBwMin;
                    return;

                case "RxBwMax":
                    receiverViewControl1.RxBwMax = sx1231.RxBwMax;
                    return;

                case "AfcDccFreqMin":
                    receiverViewControl1.AfcDccFreqMin = sx1231.AfcDccFreqMin;
                    return;

                case "AfcDccFreqMax":
                    receiverViewControl1.AfcDccFreqMax = sx1231.AfcDccFreqMax;
                    return;

                case "AfcRxBwMin":
                    receiverViewControl1.AfcRxBwMin = sx1231.AfcRxBwMin;
                    return;

                case "AfcRxBwMax":
                    receiverViewControl1.AfcRxBwMax = sx1231.AfcRxBwMax;
                    return;

                case "AgcAutoRefOn":
                    receiverViewControl1.AgcAutoRefOn = sx1231.AgcAutoRefOn;
                    return;

                case "AgcRefLevel":
                    receiverViewControl1.AgcRefLevel = sx1231.AgcRefLevel;
                    return;

                case "AgcSnrMargin":
                    receiverViewControl1.AgcSnrMargin = sx1231.AgcSnrMargin;
                    return;

                case "AgcStep1":
                    receiverViewControl1.AgcStep1 = sx1231.AgcStep1;
                    return;

                case "AgcStep2":
                    receiverViewControl1.AgcStep2 = sx1231.AgcStep2;
                    return;

                case "AgcStep3":
                    receiverViewControl1.AgcStep3 = sx1231.AgcStep3;
                    return;

                case "AgcStep4":
                    receiverViewControl1.AgcStep4 = sx1231.AgcStep4;
                    return;

                case "AgcStep5":
                    receiverViewControl1.AgcStep5 = sx1231.AgcStep5;
                    return;

                case "LnaZin":
                    receiverViewControl1.LnaZin = sx1231.LnaZin;
                    return;

                case "LnaLowPowerOn":
                    receiverViewControl1.LnaLowPowerOn = sx1231.LnaLowPowerOn;
                    return;

                case "LnaCurrentGain":
                    receiverViewControl1.LnaCurrentGain = sx1231.LnaCurrentGain;
                    return;

                case "LnaGainSelect":
                    receiverViewControl1.LnaGainSelect = sx1231.LnaGainSelect;
                    return;

                case "DccFreq":
                    receiverViewControl1.DccFreq = sx1231.DccFreq;
                    return;

                case "RxBw":
                    receiverViewControl1.RxBw = sx1231.RxBw;
                    return;

                case "AfcDccFreq":
                    receiverViewControl1.AfcDccFreq = sx1231.AfcDccFreq;
                    return;

                case "AfcRxBw":
                    receiverViewControl1.AfcRxBw = sx1231.AfcRxBw;
                    return;

                case "OokThreshType":
                    receiverViewControl1.OokThreshType = sx1231.OokThreshType;
                    return;

                case "OokPeakThreshStep":
                    receiverViewControl1.OokPeakThreshStep = sx1231.OokPeakThreshStep;
                    return;

                case "OokPeakThreshDec":
                    receiverViewControl1.OokPeakThreshDec = sx1231.OokPeakThreshDec;
                    return;

                case "OokAverageThreshFilt":
                    receiverViewControl1.OokAverageThreshFilt = sx1231.OokAverageThreshFilt;
                    return;

                case "OokFixedThresh":
                    receiverViewControl1.OokFixedThresh = sx1231.OokFixedThresh;
                    return;

                case "FeiDone":
                    receiverViewControl1.FeiDone = sx1231.FeiDone;
                    return;

                case "AfcDone":
                    receiverViewControl1.AfcDone = sx1231.AfcDone;
                    return;

                case "AfcAutoClearOn":
                    receiverViewControl1.AfcAutoClearOn = sx1231.AfcAutoClearOn;
                    return;

                case "AfcAutoOn":
                    receiverViewControl1.AfcAutoOn = sx1231.AfcAutoOn;
                    return;

                case "AfcValue":
                    receiverViewControl1.AfcValue = sx1231.AfcValue;
                    return;

                case "FeiValue":
                    receiverViewControl1.FeiValue = sx1231.FeiValue;
                    return;

                case "FastRx":
                    receiverViewControl1.FastRx = sx1231.FastRx;
                    return;

                case "RssiDone":
                    receiverViewControl1.RssiDone = sx1231.RssiDone;
                    return;

                case "RssiValue":
                    receiverViewControl1.RssiValue = sx1231.RssiValue;
                    return;

                case "AutoRxRestartOn":
                    receiverViewControl1.AutoRxRestartOn = sx1231.Packet.AutoRxRestartOn;
                    return;

                case "Dio0Mapping":
                    irqMapViewControl1.Dio0Mapping = sx1231.Dio0Mapping;
                    return;

                case "Dio1Mapping":
                    irqMapViewControl1.Dio1Mapping = sx1231.Dio1Mapping;
                    return;

                case "Dio2Mapping":
                    irqMapViewControl1.Dio2Mapping = sx1231.Dio2Mapping;
                    return;

                case "Dio3Mapping":
                    irqMapViewControl1.Dio3Mapping = sx1231.Dio3Mapping;
                    return;

                case "Dio4Mapping":
                    irqMapViewControl1.Dio4Mapping = sx1231.Dio4Mapping;
                    return;

                case "Dio5Mapping":
                    irqMapViewControl1.Dio5Mapping = sx1231.Dio5Mapping;
                    return;

                case "ClockOut":
                    irqMapViewControl1.ClockOut = sx1231.ClockOut;
                    return;

                case "ModeReady":
                    irqMapViewControl1.ModeReady = sx1231.ModeReady;
                    ledModeReady.Checked = sx1231.ModeReady;
                    return;

                case "RxReady":
                    irqMapViewControl1.RxReady = sx1231.RxReady;
                    ledRxReady.Checked = sx1231.RxReady;
                    return;

                case "TxReady":
                    irqMapViewControl1.TxReady = sx1231.TxReady;
                    ledTxReady.Checked = sx1231.TxReady;
                    return;

                case "PllLock":
                    irqMapViewControl1.PllLock = sx1231.PllLock;
                    ledPllLock.Checked = sx1231.PllLock;
                    return;

                case "Rssi":
                    irqMapViewControl1.Rssi = sx1231.Rssi;
                    ledRssi.Checked = sx1231.Rssi;
                    return;

                case "Timeout":
                    irqMapViewControl1.Timeout = sx1231.Timeout;
                    ledTimeout.Checked = sx1231.Timeout;
                    return;

                case "AutoMode":
                    irqMapViewControl1.AutoMode = sx1231.AutoMode;
                    ledAutoMode.Checked = sx1231.AutoMode;
                    return;

                case "SyncAddressMatch":
                    irqMapViewControl1.SyncAddressMatch = sx1231.SyncAddressMatch;
                    ledSyncAddressMatch.Checked = sx1231.SyncAddressMatch;
                    return;

                case "FifoFull":
                    irqMapViewControl1.FifoFull = sx1231.FifoFull;
                    ledFifoFull.Checked = sx1231.FifoFull;
                    return;

                case "FifoNotEmpty":
                    irqMapViewControl1.FifoNotEmpty = sx1231.FifoNotEmpty;
                    ledFifoNotEmpty.Checked = sx1231.FifoNotEmpty;
                    return;

                case "FifoLevel":
                    irqMapViewControl1.FifoLevel = sx1231.FifoLevel;
                    ledFifoLevel.Checked = sx1231.FifoLevel;
                    return;

                case "FifoOverrun":
                    irqMapViewControl1.FifoOverrun = sx1231.FifoOverrun;
                    ledFifoOverrun.Checked = sx1231.FifoOverrun;
                    return;

                case "PacketSent":
                    irqMapViewControl1.PacketSent = sx1231.PacketSent;
                    ledPacketSent.Checked = sx1231.PacketSent;
                    return;

                case "PayloadReady":
                    irqMapViewControl1.PayloadReady = sx1231.PayloadReady;
                    ledPayloadReady.Checked = sx1231.PayloadReady;
                    return;

                case "CrcOk":
                    irqMapViewControl1.CrcOk = sx1231.CrcOk;
                    ledCrcOk.Checked = sx1231.CrcOk;
                    return;

                case "LowBat":
                    irqMapViewControl1.LowBat = sx1231.LowBat;
                    ledLowBat.Checked = sx1231.LowBat;
                    return;

                case "RssiThresh":
                    receiverViewControl1.RssiThresh = sx1231.RssiThresh;
                    return;

                case "TimeoutRxStart":
                    receiverViewControl1.TimeoutRxStart = sx1231.TimeoutRxStart;
                    return;

                case "TimeoutRssiThresh":
                    receiverViewControl1.TimeoutRssiThresh = sx1231.TimeoutRssiThresh;
                    return;

                case "Packet":
                    packetHandlerView1.PreambleSize = sx1231.Packet.PreambleSize;
                    packetHandlerView1.SyncOn = sx1231.Packet.SyncOn;
                    packetHandlerView1.FifoFillCondition = sx1231.Packet.FifoFillCondition;
                    packetHandlerView1.SyncSize = sx1231.Packet.SyncSize;
                    packetHandlerView1.SyncTol = sx1231.Packet.SyncTol;
                    packetHandlerView1.SyncValue = sx1231.Packet.SyncValue;
                    packetHandlerView1.PacketFormat = sx1231.Packet.PacketFormat;
                    packetHandlerView1.DcFree = sx1231.Packet.DcFree;
                    packetHandlerView1.CrcOn = sx1231.Packet.CrcOn;
                    packetHandlerView1.CrcAutoClearOff = sx1231.Packet.CrcAutoClearOff;
                    packetHandlerView1.AddressFiltering = sx1231.Packet.AddressFiltering;
                    packetHandlerView1.PayloadLength = sx1231.Packet.PayloadLength;
                    packetHandlerView1.NodeAddress = sx1231.Packet.NodeAddress;
                    packetHandlerView1.BroadcastAddress = sx1231.Packet.BroadcastAddress;
                    packetHandlerView1.EnterCondition = sx1231.Packet.EnterCondition;
                    packetHandlerView1.ExitCondition = sx1231.Packet.ExitCondition;
                    packetHandlerView1.IntermediateMode = sx1231.Packet.IntermediateMode;
                    packetHandlerView1.TxStartCondition = sx1231.Packet.TxStartCondition;
                    packetHandlerView1.FifoThreshold = sx1231.Packet.FifoThreshold;
                    packetHandlerView1.InterPacketRxDelay = sx1231.Packet.InterPacketRxDelay;
                    packetHandlerView1.AesOn = sx1231.Packet.AesOn;
                    packetHandlerView1.AesKey = sx1231.Packet.AesKey;
                    packetHandlerView1.MessageLength = sx1231.Packet.MessageLength;
                    packetHandlerView1.Message = sx1231.Packet.Message;
                    packetHandlerView1.Crc = sx1231.Packet.Crc;
                    return;

                case "PreambleSize":
                    packetHandlerView1.PreambleSize = sx1231.Packet.PreambleSize;
                    return;

                case "SyncOn":
                    packetHandlerView1.SyncOn = sx1231.Packet.SyncOn;
                    return;

                case "FifoFillCondition":
                    packetHandlerView1.FifoFillCondition = sx1231.Packet.FifoFillCondition;
                    return;

                case "SyncSize":
                    packetHandlerView1.SyncSize = sx1231.Packet.SyncSize;
                    return;

                case "SyncTol":
                    packetHandlerView1.SyncTol = sx1231.Packet.SyncTol;
                    return;

                case "SyncValue":
                    packetHandlerView1.SyncValue = sx1231.Packet.SyncValue;
                    return;

                case "PacketFormat":
                    packetHandlerView1.PacketFormat = sx1231.Packet.PacketFormat;
                    return;

                case "DcFree":
                    packetHandlerView1.DcFree = sx1231.Packet.DcFree;
                    return;

                case "CrcOn":
                    packetHandlerView1.CrcOn = sx1231.Packet.CrcOn;
                    return;

                case "CrcAutoClearOff":
                    packetHandlerView1.CrcAutoClearOff = sx1231.Packet.CrcAutoClearOff;
                    return;

                case "AddressFiltering":
                    packetHandlerView1.AddressFiltering = sx1231.Packet.AddressFiltering;
                    return;

                case "PayloadLength":
                    packetHandlerView1.PayloadLength = sx1231.Packet.PayloadLength;
                    return;

                case "NodeAddress":
                    packetHandlerView1.NodeAddress = sx1231.Packet.NodeAddress;
                    return;

                case "NodeAddressRx":
                    packetHandlerView1.NodeAddressRx = sx1231.Packet.NodeAddressRx;
                    return;

                case "BroadcastAddress":
                    packetHandlerView1.BroadcastAddress = sx1231.Packet.BroadcastAddress;
                    return;

                case "EnterCondition":
                    packetHandlerView1.EnterCondition = sx1231.Packet.EnterCondition;
                    return;

                case "ExitCondition":
                    packetHandlerView1.ExitCondition = sx1231.Packet.ExitCondition;
                    return;

                case "IntermediateMode":
                    packetHandlerView1.IntermediateMode = sx1231.Packet.IntermediateMode;
                    return;

                case "TxStartCondition":
                    packetHandlerView1.TxStartCondition = sx1231.Packet.TxStartCondition;
                    return;

                case "FifoThreshold":
                    packetHandlerView1.FifoThreshold = sx1231.Packet.FifoThreshold;
                    return;

                case "InterPacketRxDelay":
                    packetHandlerView1.InterPacketRxDelay = sx1231.Packet.InterPacketRxDelay;
                    return;

                case "AesOn":
                    packetHandlerView1.AesOn = sx1231.Packet.AesOn;
                    return;

                case "AesKey":
                    packetHandlerView1.AesKey = sx1231.Packet.AesKey;
                    return;

                case "MessageLength":
                    packetHandlerView1.MessageLength = sx1231.Packet.MessageLength;
                    return;

                case "Message":
                    packetHandlerView1.Message = sx1231.Packet.Message;
                    return;

                case "Crc":
                    packetHandlerView1.Crc = sx1231.Packet.Crc;
                    return;

                case "LogEnabled":
                    packetHandlerView1.LogEnabled = sx1231.Packet.LogEnabled;
                    return;

                case "TempMeasRunning":
                    temperatureViewControl1.TempMeasRunning = sx1231.TempMeasRunning;
                    return;

                case "AdcLowPowerOn":
                    temperatureViewControl1.AdcLowPowerOn = sx1231.AdcLowPowerOn;
                    return;

                case "TempValue":
                    temperatureViewControl1.TempValue = sx1231.TempValue;
                    return;

                case "TempCalDone":
                    temperatureViewControl1.TempCalDone = sx1231.TempCalDone;
                    return;

                case "TempValueRoom":
                    temperatureViewControl1.TempValueRoom = sx1231.TempValueRoom;
                    return;

                default:
                    return;
            }
        Label_0A80:
            irqMapViewControl1.Mode = sx1231.Mode;
            packetHandlerView1.Mode = sx1231.Mode;
            temperatureViewControl1.Mode = sx1231.Mode;
        }

        private void packetHandlerView1_AddressFilteringChanged(object sender, AddressFilteringEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetAddressFiltering(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void packetHandlerView1_AesKeyChanged(object sender, ByteArrayEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetAesKey(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void packetHandlerView1_AesOnChanged(object sender, BooleanEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetAesOn(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void packetHandlerView1_BroadcastAddressChanged(object sender, ByteEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetBroadcastAddress(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void packetHandlerView1_CrcAutoClearOffChanged(object sender, BooleanEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetCrcAutoClearOff(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void packetHandlerView1_CrcOnChanged(object sender, BooleanEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetCrcOn(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void packetHandlerView1_DcFreeChanged(object sender, DcFreeEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetDcFree(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void packetHandlerView1_DocumentationChanged(object sender, DocumentationChangedEventArgs e)
        {
            OnDocumentationChanged(e);
        }

        private void packetHandlerView1_EnterConditionChanged(object sender, EnterConditionEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetEnterCondition(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void packetHandlerView1_Error(object sender, ErrorEventArgs e)
        {
            OnError(e.Status, e.Message);
        }

        private void packetHandlerView1_ExitConditionChanged(object sender, ExitConditionEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetExitCondition(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void packetHandlerView1_FifoFillConditionChanged(object sender, FifoFillConditionEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetFifoFillCondition(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void packetHandlerView1_FifoThresholdChanged(object sender, ByteEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetFifoThreshold(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void packetHandlerView1_IntermediateModeChanged(object sender, IntermediateModeEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetIntermediateMode(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void packetHandlerView1_InterPacketRxDelayChanged(object sender, Int32EventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetInterPacketRxDelay(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void packetHandlerView1_MaxPacketNumberChanged(object sender, Int32EventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetMaxPacketNumber(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void packetHandlerView1_MessageChanged(object sender, ByteArrayEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetMessage(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void packetHandlerView1_MessageLengthChanged(object sender, Int32EventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetMessageLength(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void packetHandlerView1_NodeAddressChanged(object sender, ByteEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetNodeAddress(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void packetHandlerView1_PacketFormatChanged(object sender, PacketFormatEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetPacketFormat(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void packetHandlerView1_PacketHandlerLogEnableChanged(object sender, BooleanEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetPacketHandlerLogEnable(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void packetHandlerView1_PayloadLengthChanged(object sender, ByteEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetPayloadLength(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void packetHandlerView1_PreambleSizeChanged(object sender, Int32EventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetPreambleSize(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void packetHandlerView1_StartStopChanged(object sender, BooleanEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetPacketHandlerStartStop(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void packetHandlerView1_SyncOnChanged(object sender, BooleanEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetSyncOn(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void packetHandlerView1_SyncSizeChanged(object sender, ByteEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetSyncSize(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void packetHandlerView1_SyncTolChanged(object sender, ByteEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetSyncTol(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void packetHandlerView1_SyncValueChanged(object sender, ByteArrayEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetSyncValue(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void packetHandlerView1_TxStartConditionChanged(object sender, BooleanEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetTxStartCondition(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void rBtnOperatingMode_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                if (rBtnSleep.Checked)
                    sx1231.SetOperatingMode(OperatingModeEnum.Sleep);
                else if (rBtnStandby.Checked)
                    sx1231.SetOperatingMode(OperatingModeEnum.Stdby);
                else if (rBtnSynthesizer.Checked)
                    sx1231.SetOperatingMode(OperatingModeEnum.Fs);
                else if (rBtnReceiver.Checked)
                    sx1231.SetOperatingMode(OperatingModeEnum.Rx);
                else if (rBtnTransmitter.Checked)
                    sx1231.SetOperatingMode(OperatingModeEnum.Tx);
                irqMapViewControl1.Mode = sx1231.Mode;
                packetHandlerView1.Mode = sx1231.Mode;
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void receiverViewControl1_AfcAutoClearOnChanged(object sender, BooleanEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetAfcAutoClearOn(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void receiverViewControl1_AfcAutoOnChanged(object sender, BooleanEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetAfcAutoOn(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void receiverViewControl1_AfcClearChanged(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetAfcClear();
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void receiverViewControl1_AfcDccFreqChanged(object sender, DecimalEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetAfcDccFreq(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void receiverViewControl1_AfcLowBetaOnChanged(object sender, BooleanEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetAfcLowBetaOn(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void receiverViewControl1_AfcRxBwChanged(object sender, DecimalEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetAfcRxBw(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void receiverViewControl1_AfcStartChanged(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetAfcStart();
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void receiverViewControl1_AgcAutoRefChanged(object sender, BooleanEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetAgcAutoRefOn(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void receiverViewControl1_AgcRefLevelChanged(object sender, Int32EventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetAgcRefLevel(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void receiverViewControl1_AgcSnrMarginChanged(object sender, ByteEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetAgcSnrMargin(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void receiverViewControl1_AgcStepChanged(object sender, AgcStepEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetAgcStep(e.Id, e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void receiverViewControl1_AutoRxRestartOnChanged(object sender, BooleanEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetAutoRxRestartOn(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void receiverViewControl1_DagcOnChanged(object sender, BooleanEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetDagcOn(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void receiverViewControl1_DccFreqChanged(object sender, DecimalEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetDccFreq(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void receiverViewControl1_DocumentationChanged(object sender, DocumentationChangedEventArgs e)
        {
            OnDocumentationChanged(e);
        }

        private void receiverViewControl1_FastRxChanged(object sender, BooleanEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetFastRx(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void receiverViewControl1_FeiStartChanged(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetFeiStart();
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void receiverViewControl1_LnaGainChanged(object sender, LnaGainEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetLnaGainSelect(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void receiverViewControl1_LnaLowPowerOnChanged(object sender, BooleanEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetLnaLowPowerOn(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void receiverViewControl1_LnaZinChanged(object sender, LnaZinEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetLnaZin(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void receiverViewControl1_LowBetaAfcOffsetChanged(object sender, DecimalEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetLowBetaAfcOffset(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void receiverViewControl1_OokAverageThreshFiltChanged(object sender, OokAverageThreshFiltEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetOokAverageThreshFilt(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void receiverViewControl1_OokFixedThreshChanged(object sender, ByteEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetOokFixedThresh(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void receiverViewControl1_OokPeakThreshDecChanged(object sender, OokPeakThreshDecEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetOokPeakThreshDec(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void receiverViewControl1_OokPeakThreshStepChanged(object sender, DecimalEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetOokPeakThreshStep(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void receiverViewControl1_OokThreshTypeChanged(object sender, OokThreshTypeEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetOokThreshType(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void receiverViewControl1_RestartRxChanged(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetRestartRx();
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void receiverViewControl1_RssiAutoThreshChanged(object sender, BooleanEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.RssiAutoThresh = e.Value;
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void receiverViewControl1_RssiStartChanged(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetRssiStart();
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void receiverViewControl1_RssiThreshChanged(object sender, DecimalEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetRssiThresh(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void receiverViewControl1_RxBwChanged(object sender, DecimalEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetRxBw(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void receiverViewControl1_SensitivityBoostOnChanged(object sender, BooleanEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetSensitivityBoostOn(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void receiverViewControl1_TimeoutRssiThreshChanged(object sender, DecimalEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetTimeoutRssiThresh(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void receiverViewControl1_TimeoutRxStartChanged(object sender, DecimalEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetTimeoutRxStart(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void sx1231_BitRateLimitStatusChanged(object sender, LimitCheckStatusEventArg e)
        {
            commonViewControl1.UpdateBitRateLimits(e.Status, e.Message);
        }

        private void sx1231_FdevLimitStatusChanged(object sender, LimitCheckStatusEventArg e)
        {
            commonViewControl1.UpdateFdevLimits(e.Status, e.Message);
        }

        private void sx1231_FrequencyRfLimitStatusChanged(object sender, LimitCheckStatusEventArg e)
        {
            commonViewControl1.UpdateFrequencyRfLimits(e.Status, e.Message);
        }

        private void sx1231_PacketHandlerReceived(object sender, PacketStatusEventArg e)
        {
            if (base.InvokeRequired)
            {
                base.BeginInvoke(new SX1231PacketHandlerTransmittedDelegate(OnSX1231PacketHandlerTransmitted), new object[] { sender, e });
            }
            else
            {
                OnSX1231PacketHandlerReceived(sender, e);
            }
        }

        private void sx1231_PacketHandlerStarted(object sender, EventArgs e)
        {
            if (base.InvokeRequired)
            {
                base.BeginInvoke(new SX1231PacketHandlerStartedDelegate(OnSX1231PacketHandlerStarted), new object[] { sender, e });
            }
            else
            {
                OnSX1231PacketHandlerStarted(sender, e);
            }
        }

        private void sx1231_PacketHandlerStoped(object sender, EventArgs e)
        {
            if (base.InvokeRequired)
            {
                base.BeginInvoke(new SX1231PacketHandlerStopedDelegate(OnSX1231PacketHandlerStoped), new object[] { sender, e });
            }
            else
            {
                OnSX1231PacketHandlerStoped(sender, e);
            }
        }

        private void sx1231_PacketHandlerTransmitted(object sender, PacketStatusEventArg e)
        {
            if (base.InvokeRequired)
            {
                base.BeginInvoke(new SX1231PacketHandlerTransmittedDelegate(OnSX1231PacketHandlerTransmitted), new object[] { sender, e });
            }
            else
            {
                OnSX1231PacketHandlerTransmitted(sender, e);
            }
        }

        private void sx1231_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (base.InvokeRequired)
            {
                base.BeginInvoke(new SX1231PropertyChangedDelegate(OnSX1231PropertyChanged), new object[] { sender, e });
            }
            else
            {
                OnSX1231PropertyChanged(sender, e);
            }
        }

        private void sx1231_SyncValueLimitChanged(object sender, LimitCheckStatusEventArg e)
        {
            packetHandlerView1.UpdateSyncValueLimits(e.Status, e.Message);
        }

        private void temperatureViewControl1_AdcLowPowerOnChanged(object sender, BooleanEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetAdcLowPowerOn(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void temperatureViewControl1_DocumentationChanged(object sender, DocumentationChangedEventArgs e)
        {
            OnDocumentationChanged(e);
        }

        private void temperatureViewControl1_TempCalibrateChanged(object sender, DecimalEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetTempCalibrate(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void transmitterViewControl1_DocumentationChanged(object sender, DocumentationChangedEventArgs e)
        {
            OnDocumentationChanged(e);
        }

        private void transmitterViewControl1_OcpOnChanged(object sender, BooleanEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetOcpOn(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void transmitterViewControl1_OcpTrimChanged(object sender, DecimalEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetOcpTrim(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void transmitterViewControl1_OutputPowerChanged(object sender, DecimalEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetOutputPower(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void transmitterViewControl1_PaModeChanged(object sender, PaModeEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetPaMode(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void transmitterViewControl1_PaRampChanged(object sender, PaRampEventArg e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OnError(0, "-");
                sx1231.SetPaRamp(e.Value);
            }
            catch (Exception exception)
            {
                OnError(1, exception.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        public new bool Enabled
        {
            get
            {
                return base.Enabled;
            }
            set
            {
                if (base.Enabled != value)
                {
                    base.Enabled = value;
                }
            }
        }

        public SemtechLib.Devices.SX1231.SX1231 SX1231
        {
            set
            {
                if (sx1231 != value)
                {
                    sx1231 = value;
                    sx1231.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(sx1231_PropertyChanged);
                    sx1231.BitRateLimitStatusChanged += new SemtechLib.Devices.SX1231.SX1231.LimitCheckStatusChangedEventHandler(sx1231_BitRateLimitStatusChanged);
                    sx1231.FdevLimitStatusChanged += new SemtechLib.Devices.SX1231.SX1231.LimitCheckStatusChangedEventHandler(sx1231_FdevLimitStatusChanged);
                    sx1231.FrequencyRfLimitStatusChanged += new SemtechLib.Devices.SX1231.SX1231.LimitCheckStatusChangedEventHandler(sx1231_FrequencyRfLimitStatusChanged);
                    sx1231.SyncValueLimitChanged += new SemtechLib.Devices.SX1231.SX1231.LimitCheckStatusChangedEventHandler(sx1231_SyncValueLimitChanged);
                    sx1231.PacketHandlerStarted += new EventHandler(sx1231_PacketHandlerStarted);
                    sx1231.PacketHandlerStoped += new EventHandler(sx1231_PacketHandlerStoped);
                    sx1231.PacketHandlerTransmitted += new SemtechLib.Devices.SX1231.SX1231.PacketHandlerTransmittedEventHandler(sx1231_PacketHandlerTransmitted);
                    sx1231.PacketHandlerReceived += new SemtechLib.Devices.SX1231.SX1231.PacketHandlerReceivedEventHandler(sx1231_PacketHandlerReceived);
                    commonViewControl1.FrequencyXo = sx1231.FrequencyXo;
                    commonViewControl1.FrequencyStep = sx1231.FrequencyStep;
                    commonViewControl1.Sequencer = sx1231.Sequencer;
                    commonViewControl1.ListenMode = sx1231.ListenMode;
                    commonViewControl1.DataMode = sx1231.DataMode;
                    commonViewControl1.ModulationType = sx1231.ModulationType;
                    commonViewControl1.ModulationShaping = sx1231.ModulationShaping;
                    commonViewControl1.BitRate = sx1231.BitRate;
                    commonViewControl1.Fdev = sx1231.Fdev;
                    commonViewControl1.FrequencyRf = sx1231.FrequencyRf;
                }
            }
        }
    }
}

