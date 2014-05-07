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
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabCommon = new System.Windows.Forms.TabPage();
			this.commonViewControl1 = new SemtechLib.Devices.SX1231.Controls.CommonViewControl();
			this.tabTransmitter = new System.Windows.Forms.TabPage();
			this.transmitterViewControl1 = new SemtechLib.Devices.SX1231.Controls.TransmitterViewControl();
			this.tabReceiver = new System.Windows.Forms.TabPage();
			this.receiverViewControl1 = new SemtechLib.Devices.SX1231.Controls.ReceiverViewControl();
			this.tabIrqMap = new System.Windows.Forms.TabPage();
			this.irqMapViewControl1 = new SemtechLib.Devices.SX1231.Controls.IrqMapViewControl();
			this.tabPacketHandler = new System.Windows.Forms.TabPage();
			this.packetHandlerView1 = new SemtechLib.Devices.SX1231.Controls.PacketHandlerView();
			this.tabTemperature = new System.Windows.Forms.TabPage();
			this.temperatureViewControl1 = new SemtechLib.Devices.SX1231.Controls.TemperatureViewControl();
			this.gBoxOperatingMode = new SemtechLib.Controls.GroupBoxEx();
			this.rBtnTransmitter = new System.Windows.Forms.RadioButton();
			this.rBtnReceiver = new System.Windows.Forms.RadioButton();
			this.rBtnSynthesizer = new System.Windows.Forms.RadioButton();
			this.rBtnStandby = new System.Windows.Forms.RadioButton();
			this.rBtnSleep = new System.Windows.Forms.RadioButton();
			this.lbModeReady = new System.Windows.Forms.Label();
			this.label19 = new System.Windows.Forms.Label();
			this.label18 = new System.Windows.Forms.Label();
			this.label17 = new System.Windows.Forms.Label();
			this.label23 = new System.Windows.Forms.Label();
			this.label22 = new System.Windows.Forms.Label();
			this.label21 = new System.Windows.Forms.Label();
			this.label20 = new System.Windows.Forms.Label();
			this.label27 = new System.Windows.Forms.Label();
			this.label26 = new System.Windows.Forms.Label();
			this.label25 = new System.Windows.Forms.Label();
			this.label24 = new System.Windows.Forms.Label();
			this.label31 = new System.Windows.Forms.Label();
			this.label30 = new System.Windows.Forms.Label();
			this.label29 = new System.Windows.Forms.Label();
			this.label28 = new System.Windows.Forms.Label();
			this.gBoxIrqFlags = new SemtechLib.Controls.GroupBoxEx();
			this.ledLowBat = new SemtechLib.Controls.Led();
			this.ledCrcOk = new SemtechLib.Controls.Led();
			this.ledRxReady = new SemtechLib.Controls.Led();
			this.ledPayloadReady = new SemtechLib.Controls.Led();
			this.ledTxReady = new SemtechLib.Controls.Led();
			this.ledPacketSent = new SemtechLib.Controls.Led();
			this.ledPllLock = new SemtechLib.Controls.Led();
			this.ledModeReady = new SemtechLib.Controls.Led();
			this.ledFifoOverrun = new SemtechLib.Controls.Led();
			this.ledRssi = new SemtechLib.Controls.Led();
			this.ledFifoLevel = new SemtechLib.Controls.Led();
			this.ledTimeout = new SemtechLib.Controls.Led();
			this.ledFifoNotEmpty = new SemtechLib.Controls.Led();
			this.ledAutoMode = new SemtechLib.Controls.Led();
			this.ledFifoFull = new SemtechLib.Controls.Led();
			this.ledSyncAddressMatch = new SemtechLib.Controls.Led();
			this.tabControl1.SuspendLayout();
			this.tabCommon.SuspendLayout();
			this.tabTransmitter.SuspendLayout();
			this.tabReceiver.SuspendLayout();
			this.tabIrqMap.SuspendLayout();
			this.tabPacketHandler.SuspendLayout();
			this.tabTemperature.SuspendLayout();
			this.gBoxOperatingMode.SuspendLayout();
			this.gBoxIrqFlags.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabCommon);
			this.tabControl1.Controls.Add(this.tabTransmitter);
			this.tabControl1.Controls.Add(this.tabReceiver);
			this.tabControl1.Controls.Add(this.tabIrqMap);
			this.tabControl1.Controls.Add(this.tabPacketHandler);
			this.tabControl1.Controls.Add(this.tabTemperature);
			this.tabControl1.Location = new System.Drawing.Point(3, 3);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(807, 519);
			this.tabControl1.TabIndex = 0;
			// 
			// tabCommon
			// 
			this.tabCommon.Controls.Add(this.commonViewControl1);
			this.tabCommon.Location = new System.Drawing.Point(4, 22);
			this.tabCommon.Name = "tabCommon";
			this.tabCommon.Padding = new System.Windows.Forms.Padding(3);
			this.tabCommon.Size = new System.Drawing.Size(799, 493);
			this.tabCommon.TabIndex = 0;
			this.tabCommon.Text = "Common";
			this.tabCommon.UseVisualStyleBackColor = true;
			// 
			// commonViewControl1
			// 
			this.commonViewControl1.BitRate = new decimal(new int[] {
            4800,
            0,
            0,
            0});
			this.commonViewControl1.DataMode = SemtechLib.Devices.SX1231.Enumerations.DataModeEnum.Packet;
			this.commonViewControl1.Fdev = new decimal(new int[] {
            5002,
            0,
            0,
            0});
			this.commonViewControl1.FrequencyRf = new decimal(new int[] {
            915000000,
            0,
            0,
            0});
			this.commonViewControl1.FrequencyStep = new decimal(new int[] {
            61,
            0,
            0,
            0});
			this.commonViewControl1.FrequencyXo = new decimal(new int[] {
            32000000,
            0,
            0,
            0});
			this.commonViewControl1.ListenCoefIdle = new decimal(new int[] {
            10045,
            0,
            0,
            65536});
			this.commonViewControl1.ListenCoefRx = new decimal(new int[] {
            131,
            0,
            0,
            0});
			this.commonViewControl1.ListenCriteria = SemtechLib.Devices.SX1231.Enumerations.ListenCriteriaEnum.RssiThresh;
			this.commonViewControl1.ListenMode = false;
			this.commonViewControl1.ListenResolIdle = SemtechLib.Devices.SX1231.Enumerations.ListenResolEnum.Res004100;
			this.commonViewControl1.ListenResolRx = SemtechLib.Devices.SX1231.Enumerations.ListenResolEnum.Res004100;
			this.commonViewControl1.Location = new System.Drawing.Point(0, 0);
			this.commonViewControl1.LowBatMonitor = true;
			this.commonViewControl1.LowBatOn = true;
			this.commonViewControl1.ModulationShaping = ((byte)(0));
			this.commonViewControl1.ModulationType = SemtechLib.Devices.SX1231.Enumerations.ModulationTypeEnum.FSK;
			this.commonViewControl1.Name = "commonViewControl1";
			this.commonViewControl1.RcCalDone = false;
			this.commonViewControl1.Sequencer = false;
			this.commonViewControl1.Size = new System.Drawing.Size(799, 493);
			this.commonViewControl1.TabIndex = 0;
			this.commonViewControl1.Version = "2.3";
			this.commonViewControl1.BitRateChanged += new SemtechLib.General.Events.DecimalEventHandler(this.commonViewControl1_BitRateChanged);
			this.commonViewControl1.DataModeChanged += new SemtechLib.Devices.SX1231.Events.DataModeEventHandler(this.commonViewControl1_DataModeChanged);
			this.commonViewControl1.DocumentationChanged += new SemtechLib.General.Interfaces.DocumentationChangedEventHandler(this.commonViewControl1_DocumentationChanged);
			this.commonViewControl1.FdevChanged += new SemtechLib.General.Events.DecimalEventHandler(this.commonViewControl1_FdevChanged);
			this.commonViewControl1.FrequencyRfChanged += new SemtechLib.General.Events.DecimalEventHandler(this.commonViewControl1_FrequencyRfChanged);
			this.commonViewControl1.FrequencyXoChanged += new SemtechLib.General.Events.DecimalEventHandler(this.commonViewControl1_FrequencyXoChanged);
			this.commonViewControl1.ListenCoefIdleChanged += new SemtechLib.General.Events.DecimalEventHandler(this.commonViewControl1_ListenCoefIdleChanged);
			this.commonViewControl1.ListenCoefRxChanged += new SemtechLib.General.Events.DecimalEventHandler(this.commonViewControl1_ListenCoefRxChanged);
			this.commonViewControl1.ListenCriteriaChanged += new SemtechLib.Devices.SX1231.Events.ListenCriteriaEventHandler(this.commonViewControl1_ListenCriteriaChanged);
			this.commonViewControl1.ListenEndChanged += new SemtechLib.Devices.SX1231.Events.ListenEndEventHandler(this.commonViewControl1_ListenEndChanged);
			this.commonViewControl1.ListenModeAbortChanged += new System.EventHandler(this.commonViewControl1_ListenModeAbortChanged);
			this.commonViewControl1.ListenModeChanged += new SemtechLib.General.Events.BooleanEventHandler(this.commonViewControl1_ListenModeChanged);
			this.commonViewControl1.ListenResolIdleChanged += new SemtechLib.Devices.SX1231.Events.ListenResolEventHandler(this.commonViewControl1_ListenResolIdleChanged);
			this.commonViewControl1.ListenResolRxChanged += new SemtechLib.Devices.SX1231.Events.ListenResolEventHandler(this.commonViewControl1_ListenResolRxChanged);
			this.commonViewControl1.LowBatOnChanged += new SemtechLib.General.Events.BooleanEventHandler(this.commonViewControl1_LowBatOnChanged);
			this.commonViewControl1.LowBatTrimChanged += new SemtechLib.Devices.SX1231.Events.LowBatTrimEventHandler(this.commonViewControl1_LowBatTrimChanged);
			this.commonViewControl1.ModulationShapingChanged += new SemtechLib.General.Events.ByteEventHandler(this.commonViewControl1_ModulationShapingChanged);
			this.commonViewControl1.ModulationTypeChanged += new SemtechLib.Devices.SX1231.Events.ModulationTypeEventHandler(this.commonViewControl1_ModulationTypeChanged);
			this.commonViewControl1.RcCalibrationChanged += new System.EventHandler(this.commonViewControl1_RcCalibrationChanged);
			this.commonViewControl1.SequencerChanged += new SemtechLib.General.Events.BooleanEventHandler(this.commonViewControl1_SequencerChanged);
			// 
			// tabTransmitter
			// 
			this.tabTransmitter.Controls.Add(this.transmitterViewControl1);
			this.tabTransmitter.Location = new System.Drawing.Point(4, 22);
			this.tabTransmitter.Name = "tabTransmitter";
			this.tabTransmitter.Padding = new System.Windows.Forms.Padding(3);
			this.tabTransmitter.Size = new System.Drawing.Size(799, 493);
			this.tabTransmitter.TabIndex = 1;
			this.tabTransmitter.Text = "Transmitter";
			this.tabTransmitter.UseVisualStyleBackColor = true;
			// 
			// transmitterViewControl1
			// 
			this.transmitterViewControl1.Location = new System.Drawing.Point(0, 0);
			this.transmitterViewControl1.Name = "transmitterViewControl1";
			this.transmitterViewControl1.OcpOn = true;
			this.transmitterViewControl1.OcpTrim = new decimal(new int[] {
            1000,
            0,
            0,
            65536});
			this.transmitterViewControl1.OutputPower = new decimal(new int[] {
            13,
            0,
            0,
            0});
			this.transmitterViewControl1.PaMode = SemtechLib.Devices.SX1231.Enumerations.PaModeEnum.PA0;
			this.transmitterViewControl1.Size = new System.Drawing.Size(799, 493);
			this.transmitterViewControl1.TabIndex = 0;
			this.transmitterViewControl1.DocumentationChanged += new SemtechLib.General.Interfaces.DocumentationChangedEventHandler(this.transmitterViewControl1_DocumentationChanged);
			this.transmitterViewControl1.OcpOnChanged += new SemtechLib.General.Events.BooleanEventHandler(this.transmitterViewControl1_OcpOnChanged);
			this.transmitterViewControl1.OcpTrimChanged += new SemtechLib.General.Events.DecimalEventHandler(this.transmitterViewControl1_OcpTrimChanged);
			this.transmitterViewControl1.OutputPowerChanged += new SemtechLib.General.Events.DecimalEventHandler(this.transmitterViewControl1_OutputPowerChanged);
			this.transmitterViewControl1.PaModeChanged += new SemtechLib.Devices.SX1231.Events.PaModeEventHandler(this.transmitterViewControl1_PaModeChanged);
			this.transmitterViewControl1.PaRampChanged += new SemtechLib.Devices.SX1231.Events.PaRampEventHandler(this.transmitterViewControl1_PaRampChanged);
			// 
			// tabReceiver
			// 
			this.tabReceiver.Controls.Add(this.receiverViewControl1);
			this.tabReceiver.Location = new System.Drawing.Point(4, 22);
			this.tabReceiver.Name = "tabReceiver";
			this.tabReceiver.Padding = new System.Windows.Forms.Padding(3);
			this.tabReceiver.Size = new System.Drawing.Size(799, 493);
			this.tabReceiver.TabIndex = 2;
			this.tabReceiver.Text = "Receiver";
			this.tabReceiver.UseVisualStyleBackColor = true;
			// 
			// receiverViewControl1
			// 
			this.receiverViewControl1.AfcAutoClearOn = true;
			this.receiverViewControl1.AfcAutoOn = true;
			this.receiverViewControl1.AfcDccFreq = new decimal(new int[] {
            -905921831,
            295814893,
            539237922,
            1638400});
			this.receiverViewControl1.AfcDccFreqMax = new decimal(new int[] {
            1657,
            0,
            0,
            0});
			this.receiverViewControl1.AfcDccFreqMin = new decimal(new int[] {
            12,
            0,
            0,
            0});
			this.receiverViewControl1.AfcDone = false;
			this.receiverViewControl1.AfcLowBetaOn = true;
			this.receiverViewControl1.AfcRxBw = new decimal(new int[] {
            50000,
            0,
            0,
            0});
			this.receiverViewControl1.AfcRxBwMax = new decimal(new int[] {
            400000,
            0,
            0,
            0});
			this.receiverViewControl1.AfcRxBwMin = new decimal(new int[] {
            3125,
            0,
            0,
            0});
			this.receiverViewControl1.AfcValue = new decimal(new int[] {
            0,
            0,
            0,
            65536});
			this.receiverViewControl1.AgcAutoRefOn = true;
			this.receiverViewControl1.AgcReference = -80;
			this.receiverViewControl1.AgcRefLevel = -80;
			this.receiverViewControl1.AgcSnrMargin = ((byte)(5));
			this.receiverViewControl1.AgcStep1 = ((byte)(16));
			this.receiverViewControl1.AgcStep2 = ((byte)(7));
			this.receiverViewControl1.AgcStep3 = ((byte)(11));
			this.receiverViewControl1.AgcStep4 = ((byte)(9));
			this.receiverViewControl1.AgcStep5 = ((byte)(11));
			this.receiverViewControl1.AgcThresh1 = 0;
			this.receiverViewControl1.AgcThresh2 = 0;
			this.receiverViewControl1.AgcThresh3 = 0;
			this.receiverViewControl1.AgcThresh4 = 0;
			this.receiverViewControl1.AgcThresh5 = 0;
			this.receiverViewControl1.AutoRxRestartOn = true;
			this.receiverViewControl1.BitRate = new decimal(new int[] {
            4800,
            0,
            0,
            0});
			this.receiverViewControl1.DagcOn = true;
			this.receiverViewControl1.DataMode = SemtechLib.Devices.SX1231.Enumerations.DataModeEnum.Packet;
			this.receiverViewControl1.DccFreq = new decimal(new int[] {
            -163586584,
            -1389046539,
            -2048070723,
            1703936});
			this.receiverViewControl1.DccFreqMax = new decimal(new int[] {
            1657,
            0,
            0,
            0});
			this.receiverViewControl1.DccFreqMin = new decimal(new int[] {
            12,
            0,
            0,
            0});
			this.receiverViewControl1.FastRx = true;
			this.receiverViewControl1.FeiDone = false;
			this.receiverViewControl1.FeiValue = new decimal(new int[] {
            0,
            0,
            0,
            65536});
			this.receiverViewControl1.FrequencyXo = new decimal(new int[] {
            32000000,
            0,
            0,
            0});
			this.receiverViewControl1.LnaLowPowerOn = true;
			this.receiverViewControl1.LnaZin = SemtechLib.Devices.SX1231.Enumerations.LnaZinEnum.ZIN_200;
			this.receiverViewControl1.Location = new System.Drawing.Point(0, 0);
			this.receiverViewControl1.LowBetaAfcOffset = new decimal(new int[] {
            0,
            0,
            0,
            65536});
			this.receiverViewControl1.ModulationType = SemtechLib.Devices.SX1231.Enumerations.ModulationTypeEnum.FSK;
			this.receiverViewControl1.Name = "receiverViewControl1";
			this.receiverViewControl1.OokAverageThreshFilt = SemtechLib.Devices.SX1231.Enumerations.OokAverageThreshFiltEnum.COEF_2;
			this.receiverViewControl1.OokFixedThresh = ((byte)(6));
			this.receiverViewControl1.OokPeakThreshDec = SemtechLib.Devices.SX1231.Enumerations.OokPeakThreshDecEnum.EVERY_1_CHIPS_1_TIMES;
			this.receiverViewControl1.OokPeakThreshStep = new decimal(new int[] {
            5,
            0,
            0,
            65536});
			this.receiverViewControl1.OokThreshType = SemtechLib.Devices.SX1231.Enumerations.OokThreshTypeEnum.Peak;
			this.receiverViewControl1.RssiAutoThresh = true;
			this.receiverViewControl1.RssiDone = false;
			this.receiverViewControl1.RssiThresh = new decimal(new int[] {
            85,
            0,
            0,
            -2147483648});
			this.receiverViewControl1.RssiValue = new decimal(new int[] {
            1275,
            0,
            0,
            -2147418112});
			this.receiverViewControl1.RxBw = new decimal(new int[] {
            1890233003,
            -2135170438,
            564688631,
            1572864});
			this.receiverViewControl1.RxBwMax = new decimal(new int[] {
            500000,
            0,
            0,
            0});
			this.receiverViewControl1.RxBwMin = new decimal(new int[] {
            3906,
            0,
            0,
            0});
			this.receiverViewControl1.SensitivityBoostOn = true;
			this.receiverViewControl1.Size = new System.Drawing.Size(799, 493);
			this.receiverViewControl1.TabIndex = 0;
			this.receiverViewControl1.TimeoutRssiThresh = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.receiverViewControl1.TimeoutRxStart = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.receiverViewControl1.Version = "2.3";
			this.receiverViewControl1.AfcAutoClearOnChanged += new SemtechLib.General.Events.BooleanEventHandler(this.receiverViewControl1_AfcAutoClearOnChanged);
			this.receiverViewControl1.AfcAutoOnChanged += new SemtechLib.General.Events.BooleanEventHandler(this.receiverViewControl1_AfcAutoOnChanged);
			this.receiverViewControl1.AfcClearChanged += new System.EventHandler(this.receiverViewControl1_AfcClearChanged);
			this.receiverViewControl1.AfcDccFreqChanged += new SemtechLib.General.Events.DecimalEventHandler(this.receiverViewControl1_AfcDccFreqChanged);
			this.receiverViewControl1.AfcLowBetaOnChanged += new SemtechLib.General.Events.BooleanEventHandler(this.receiverViewControl1_AfcLowBetaOnChanged);
			this.receiverViewControl1.AfcRxBwChanged += new SemtechLib.General.Events.DecimalEventHandler(this.receiverViewControl1_AfcRxBwChanged);
			this.receiverViewControl1.AfcStartChanged += new System.EventHandler(this.receiverViewControl1_AfcStartChanged);
			this.receiverViewControl1.AgcAutoRefChanged += new SemtechLib.General.Events.BooleanEventHandler(this.receiverViewControl1_AgcAutoRefChanged);
			this.receiverViewControl1.AgcRefLevelChanged += new SemtechLib.General.Events.Int32EventHandler(this.receiverViewControl1_AgcRefLevelChanged);
			this.receiverViewControl1.AgcSnrMarginChanged += new SemtechLib.General.Events.ByteEventHandler(this.receiverViewControl1_AgcSnrMarginChanged);
			this.receiverViewControl1.AgcStepChanged += new SemtechLib.Devices.SX1231.Events.AgcStepEventHandler(this.receiverViewControl1_AgcStepChanged);
			this.receiverViewControl1.AutoRxRestartOnChanged += new SemtechLib.General.Events.BooleanEventHandler(this.receiverViewControl1_AutoRxRestartOnChanged);
			this.receiverViewControl1.DagcOnChanged += new SemtechLib.General.Events.BooleanEventHandler(this.receiverViewControl1_DagcOnChanged);
			this.receiverViewControl1.DccFreqChanged += new SemtechLib.General.Events.DecimalEventHandler(this.receiverViewControl1_DccFreqChanged);
			this.receiverViewControl1.DocumentationChanged += new SemtechLib.General.Interfaces.DocumentationChangedEventHandler(this.receiverViewControl1_DocumentationChanged);
			this.receiverViewControl1.FastRxChanged += new SemtechLib.General.Events.BooleanEventHandler(this.receiverViewControl1_FastRxChanged);
			this.receiverViewControl1.FeiStartChanged += new System.EventHandler(this.receiverViewControl1_FeiStartChanged);
			this.receiverViewControl1.LnaGainChanged += new SemtechLib.Devices.SX1231.Events.LnaGainEventHandler(this.receiverViewControl1_LnaGainChanged);
			this.receiverViewControl1.LnaLowPowerOnChanged += new SemtechLib.General.Events.BooleanEventHandler(this.receiverViewControl1_LnaLowPowerOnChanged);
			this.receiverViewControl1.LnaZinChanged += new SemtechLib.Devices.SX1231.Events.LnaZinEventHandler(this.receiverViewControl1_LnaZinChanged);
			this.receiverViewControl1.LowBetaAfcOffsetChanged += new SemtechLib.General.Events.DecimalEventHandler(this.receiverViewControl1_LowBetaAfcOffsetChanged);
			this.receiverViewControl1.OokAverageThreshFiltChanged += new SemtechLib.Devices.SX1231.Events.OokAverageThreshFiltEventHandler(this.receiverViewControl1_OokAverageThreshFiltChanged);
			this.receiverViewControl1.OokFixedThreshChanged += new SemtechLib.General.Events.ByteEventHandler(this.receiverViewControl1_OokFixedThreshChanged);
			this.receiverViewControl1.OokPeakThreshDecChanged += new SemtechLib.Devices.SX1231.Events.OokPeakThreshDecEventHandler(this.receiverViewControl1_OokPeakThreshDecChanged);
			this.receiverViewControl1.OokPeakThreshStepChanged += new SemtechLib.General.Events.DecimalEventHandler(this.receiverViewControl1_OokPeakThreshStepChanged);
			this.receiverViewControl1.OokThreshTypeChanged += new SemtechLib.Devices.SX1231.Events.OokThreshTypeEventHandler(this.receiverViewControl1_OokThreshTypeChanged);
			this.receiverViewControl1.RestartRxChanged += new System.EventHandler(this.receiverViewControl1_RestartRxChanged);
			this.receiverViewControl1.RssiAutoThreshChanged += new SemtechLib.General.Events.BooleanEventHandler(this.receiverViewControl1_RssiAutoThreshChanged);
			this.receiverViewControl1.RssiStartChanged += new System.EventHandler(this.receiverViewControl1_RssiStartChanged);
			this.receiverViewControl1.RssiThreshChanged += new SemtechLib.General.Events.DecimalEventHandler(this.receiverViewControl1_RssiThreshChanged);
			this.receiverViewControl1.RxBwChanged += new SemtechLib.General.Events.DecimalEventHandler(this.receiverViewControl1_RxBwChanged);
			this.receiverViewControl1.SensitivityBoostOnChanged += new SemtechLib.General.Events.BooleanEventHandler(this.receiverViewControl1_SensitivityBoostOnChanged);
			this.receiverViewControl1.TimeoutRssiThreshChanged += new SemtechLib.General.Events.DecimalEventHandler(this.receiverViewControl1_TimeoutRssiThreshChanged);
			this.receiverViewControl1.TimeoutRxStartChanged += new SemtechLib.General.Events.DecimalEventHandler(this.receiverViewControl1_TimeoutRxStartChanged);
			// 
			// tabIrqMap
			// 
			this.tabIrqMap.Controls.Add(this.irqMapViewControl1);
			this.tabIrqMap.Location = new System.Drawing.Point(4, 22);
			this.tabIrqMap.Name = "tabIrqMap";
			this.tabIrqMap.Padding = new System.Windows.Forms.Padding(3);
			this.tabIrqMap.Size = new System.Drawing.Size(799, 493);
			this.tabIrqMap.TabIndex = 3;
			this.tabIrqMap.Text = "IRQ & Map";
			this.tabIrqMap.UseVisualStyleBackColor = true;
			// 
			// irqMapViewControl1
			// 
			this.irqMapViewControl1.AutoMode = false;
			this.irqMapViewControl1.CrcOk = false;
			this.irqMapViewControl1.DataMode = SemtechLib.Devices.SX1231.Enumerations.DataModeEnum.Packet;
			this.irqMapViewControl1.FifoFull = false;
			this.irqMapViewControl1.FifoLevel = false;
			this.irqMapViewControl1.FifoNotEmpty = false;
			this.irqMapViewControl1.FifoOverrun = false;
			this.irqMapViewControl1.FrequencyXo = new decimal(new int[] {
            32000000,
            0,
            0,
            0});
			this.irqMapViewControl1.Location = new System.Drawing.Point(0, 0);
			this.irqMapViewControl1.LowBat = false;
			this.irqMapViewControl1.Mode = SemtechLib.Devices.SX1231.Enumerations.OperatingModeEnum.Stdby;
			this.irqMapViewControl1.ModeReady = false;
			this.irqMapViewControl1.Name = "irqMapViewControl1";
			this.irqMapViewControl1.PacketSent = false;
			this.irqMapViewControl1.PayloadReady = false;
			this.irqMapViewControl1.PllLock = false;
			this.irqMapViewControl1.Rssi = false;
			this.irqMapViewControl1.RxReady = false;
			this.irqMapViewControl1.Size = new System.Drawing.Size(799, 493);
			this.irqMapViewControl1.SyncAddressMatch = false;
			this.irqMapViewControl1.TabIndex = 0;
			this.irqMapViewControl1.Timeout = false;
			this.irqMapViewControl1.TxReady = false;
			this.irqMapViewControl1.ClockOutChanged += new SemtechLib.Devices.SX1231.Events.ClockOutEventHandler(this.irqMapViewControl1_ClockOutChanged);
			this.irqMapViewControl1.DioMappingChanged += new SemtechLib.Devices.SX1231.Events.DioMappingEventHandler(this.irqMapViewControl1_DioMappingChanged);
			this.irqMapViewControl1.DocumentationChanged += new SemtechLib.General.Interfaces.DocumentationChangedEventHandler(this.irqMapViewControl1_DocumentationChanged);
			this.irqMapViewControl1.Load += new System.EventHandler(this.irqMapViewControl1_Load);
			// 
			// tabPacketHandler
			// 
			this.tabPacketHandler.Controls.Add(this.packetHandlerView1);
			this.tabPacketHandler.Location = new System.Drawing.Point(4, 22);
			this.tabPacketHandler.Name = "tabPacketHandler";
			this.tabPacketHandler.Padding = new System.Windows.Forms.Padding(3);
			this.tabPacketHandler.Size = new System.Drawing.Size(799, 493);
			this.tabPacketHandler.TabIndex = 4;
			this.tabPacketHandler.Text = "Packet Handler";
			this.tabPacketHandler.UseVisualStyleBackColor = true;
			// 
			// packetHandlerView1
			// 
			this.packetHandlerView1.AddressFiltering = SemtechLib.Devices.SX1231.Enumerations.AddressFilteringEnum.OFF;
			this.packetHandlerView1.AesKey = new byte[] {
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0))};
			this.packetHandlerView1.AesOn = true;
			this.packetHandlerView1.BitRate = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.packetHandlerView1.BroadcastAddress = ((byte)(0));
			this.packetHandlerView1.Crc = ((ushort)(0));
			this.packetHandlerView1.CrcAutoClearOff = false;
			this.packetHandlerView1.CrcOn = true;
			this.packetHandlerView1.DataMode = SemtechLib.Devices.SX1231.Enumerations.DataModeEnum.Packet;
			this.packetHandlerView1.DcFree = SemtechLib.Devices.SX1231.Enumerations.DcFreeEnum.OFF;
			this.packetHandlerView1.EnterCondition = SemtechLib.Devices.SX1231.Enumerations.EnterConditionEnum.OFF;
			this.packetHandlerView1.ExitCondition = SemtechLib.Devices.SX1231.Enumerations.ExitConditionEnum.OFF;
			this.packetHandlerView1.FifoFillCondition = SemtechLib.Devices.SX1231.Enumerations.FifoFillConditionEnum.OnSyncAddressIrq;
			this.packetHandlerView1.FifoThreshold = ((byte)(15));
			this.packetHandlerView1.IntermediateMode = SemtechLib.Devices.SX1231.Enumerations.IntermediateModeEnum.Sleep;
			this.packetHandlerView1.InterPacketRxDelay = 0;
			this.packetHandlerView1.Location = new System.Drawing.Point(0, 0);
			this.packetHandlerView1.LogEnabled = false;
			this.packetHandlerView1.MaxPacketNumber = 0;
			this.packetHandlerView1.Message = new byte[0];
			this.packetHandlerView1.MessageLength = 0;
			this.packetHandlerView1.Mode = SemtechLib.Devices.SX1231.Enumerations.OperatingModeEnum.Stdby;
			this.packetHandlerView1.Name = "packetHandlerView1";
			this.packetHandlerView1.NodeAddress = ((byte)(0));
			this.packetHandlerView1.NodeAddressRx = ((byte)(0));
			this.packetHandlerView1.PacketFormat = SemtechLib.Devices.SX1231.Enumerations.PacketFormatEnum.Fixed;
			this.packetHandlerView1.PacketNumber = 0;
			this.packetHandlerView1.PayloadLength = ((byte)(66));
			this.packetHandlerView1.PreambleSize = 3;
			this.packetHandlerView1.Size = new System.Drawing.Size(799, 493);
			this.packetHandlerView1.StartStop = false;
			this.packetHandlerView1.SyncOn = true;
			this.packetHandlerView1.SyncSize = ((byte)(4));
			this.packetHandlerView1.SyncTol = ((byte)(0));
			this.packetHandlerView1.SyncValue = new byte[] {
        ((byte)(170)),
        ((byte)(170)),
        ((byte)(170)),
        ((byte)(170))};
			this.packetHandlerView1.TabIndex = 0;
			this.packetHandlerView1.TxStartCondition = true;
			this.packetHandlerView1.AddressFilteringChanged += new SemtechLib.Devices.SX1231.Events.AddressFilteringEventHandler(this.packetHandlerView1_AddressFilteringChanged);
			this.packetHandlerView1.AesKeyChanged += new SemtechLib.General.Events.ByteArrayEventHandler(this.packetHandlerView1_AesKeyChanged);
			this.packetHandlerView1.AesOnChanged += new SemtechLib.General.Events.BooleanEventHandler(this.packetHandlerView1_AesOnChanged);
			this.packetHandlerView1.BroadcastAddressChanged += new SemtechLib.General.Events.ByteEventHandler(this.packetHandlerView1_BroadcastAddressChanged);
			this.packetHandlerView1.CrcAutoClearOffChanged += new SemtechLib.General.Events.BooleanEventHandler(this.packetHandlerView1_CrcAutoClearOffChanged);
			this.packetHandlerView1.CrcOnChanged += new SemtechLib.General.Events.BooleanEventHandler(this.packetHandlerView1_CrcOnChanged);
			this.packetHandlerView1.DcFreeChanged += new SemtechLib.Devices.SX1231.Events.DcFreeEventHandler(this.packetHandlerView1_DcFreeChanged);
			this.packetHandlerView1.DocumentationChanged += new SemtechLib.General.Interfaces.DocumentationChangedEventHandler(this.packetHandlerView1_DocumentationChanged);
			this.packetHandlerView1.EnterConditionChanged += new SemtechLib.Devices.SX1231.Events.EnterConditionEventHandler(this.packetHandlerView1_EnterConditionChanged);
			this.packetHandlerView1.Error += new SemtechLib.General.Events.ErrorEventHandler(this.packetHandlerView1_Error);
			this.packetHandlerView1.ExitConditionChanged += new SemtechLib.Devices.SX1231.Events.ExitConditionEventHandler(this.packetHandlerView1_ExitConditionChanged);
			this.packetHandlerView1.FifoFillConditionChanged += new SemtechLib.Devices.SX1231.Events.FifoFillConditionEventHandler(this.packetHandlerView1_FifoFillConditionChanged);
			this.packetHandlerView1.FifoThresholdChanged += new SemtechLib.General.Events.ByteEventHandler(this.packetHandlerView1_FifoThresholdChanged);
			this.packetHandlerView1.IntermediateModeChanged += new SemtechLib.Devices.SX1231.Events.IntermediateModeEventHandler(this.packetHandlerView1_IntermediateModeChanged);
			this.packetHandlerView1.InterPacketRxDelayChanged += new SemtechLib.General.Events.Int32EventHandler(this.packetHandlerView1_InterPacketRxDelayChanged);
			this.packetHandlerView1.MaxPacketNumberChanged += new SemtechLib.General.Events.Int32EventHandler(this.packetHandlerView1_MaxPacketNumberChanged);
			this.packetHandlerView1.MessageChanged += new SemtechLib.General.Events.ByteArrayEventHandler(this.packetHandlerView1_MessageChanged);
			this.packetHandlerView1.MessageLengthChanged += new SemtechLib.General.Events.Int32EventHandler(this.packetHandlerView1_MessageLengthChanged);
			this.packetHandlerView1.NodeAddressChanged += new SemtechLib.General.Events.ByteEventHandler(this.packetHandlerView1_NodeAddressChanged);
			this.packetHandlerView1.PacketFormatChanged += new SemtechLib.Devices.SX1231.Events.PacketFormatEventHandler(this.packetHandlerView1_PacketFormatChanged);
			this.packetHandlerView1.PacketHandlerLogEnableChanged += new SemtechLib.General.Events.BooleanEventHandler(this.packetHandlerView1_PacketHandlerLogEnableChanged);
			this.packetHandlerView1.PayloadLengthChanged += new SemtechLib.General.Events.ByteEventHandler(this.packetHandlerView1_PayloadLengthChanged);
			this.packetHandlerView1.PreambleSizeChanged += new SemtechLib.General.Events.Int32EventHandler(this.packetHandlerView1_PreambleSizeChanged);
			this.packetHandlerView1.StartStopChanged += new SemtechLib.General.Events.BooleanEventHandler(this.packetHandlerView1_StartStopChanged);
			this.packetHandlerView1.SyncOnChanged += new SemtechLib.General.Events.BooleanEventHandler(this.packetHandlerView1_SyncOnChanged);
			this.packetHandlerView1.SyncSizeChanged += new SemtechLib.General.Events.ByteEventHandler(this.packetHandlerView1_SyncSizeChanged);
			this.packetHandlerView1.SyncTolChanged += new SemtechLib.General.Events.ByteEventHandler(this.packetHandlerView1_SyncTolChanged);
			this.packetHandlerView1.SyncValueChanged += new SemtechLib.General.Events.ByteArrayEventHandler(this.packetHandlerView1_SyncValueChanged);
			this.packetHandlerView1.TxStartConditionChanged += new SemtechLib.General.Events.BooleanEventHandler(this.packetHandlerView1_TxStartConditionChanged);
			// 
			// tabTemperature
			// 
			this.tabTemperature.Controls.Add(this.temperatureViewControl1);
			this.tabTemperature.Location = new System.Drawing.Point(4, 22);
			this.tabTemperature.Name = "tabTemperature";
			this.tabTemperature.Padding = new System.Windows.Forms.Padding(3);
			this.tabTemperature.Size = new System.Drawing.Size(799, 493);
			this.tabTemperature.TabIndex = 5;
			this.tabTemperature.Text = "Temperature";
			this.tabTemperature.UseVisualStyleBackColor = true;
			// 
			// temperatureViewControl1
			// 
			this.temperatureViewControl1.AdcLowPowerOn = true;
			this.temperatureViewControl1.Location = new System.Drawing.Point(0, 0);
			this.temperatureViewControl1.Mode = SemtechLib.Devices.SX1231.Enumerations.OperatingModeEnum.Stdby;
			this.temperatureViewControl1.Name = "temperatureViewControl1";
			this.temperatureViewControl1.Size = new System.Drawing.Size(799, 493);
			this.temperatureViewControl1.TabIndex = 0;
			this.temperatureViewControl1.TempCalDone = false;
			this.temperatureViewControl1.TempMeasRunning = false;
			this.temperatureViewControl1.TempValue = new decimal(new int[] {
            25,
            0,
            0,
            0});
			this.temperatureViewControl1.TempValueRoom = new decimal(new int[] {
            250,
            0,
            0,
            65536});
			this.temperatureViewControl1.AdcLowPowerOnChanged += new SemtechLib.General.Events.BooleanEventHandler(this.temperatureViewControl1_AdcLowPowerOnChanged);
			this.temperatureViewControl1.DocumentationChanged += new SemtechLib.General.Interfaces.DocumentationChangedEventHandler(this.temperatureViewControl1_DocumentationChanged);
			this.temperatureViewControl1.TempCalibrateChanged += new SemtechLib.General.Events.DecimalEventHandler(this.temperatureViewControl1_TempCalibrateChanged);
			// 
			// gBoxOperatingMode
			// 
			this.gBoxOperatingMode.Controls.Add(this.rBtnTransmitter);
			this.gBoxOperatingMode.Controls.Add(this.rBtnReceiver);
			this.gBoxOperatingMode.Controls.Add(this.rBtnSynthesizer);
			this.gBoxOperatingMode.Controls.Add(this.rBtnStandby);
			this.gBoxOperatingMode.Controls.Add(this.rBtnSleep);
			this.gBoxOperatingMode.Location = new System.Drawing.Point(816, 411);
			this.gBoxOperatingMode.Name = "gBoxOperatingMode";
			this.gBoxOperatingMode.Size = new System.Drawing.Size(189, 107);
			this.gBoxOperatingMode.TabIndex = 2;
			this.gBoxOperatingMode.TabStop = false;
			this.gBoxOperatingMode.Text = "Operating mode";
			this.gBoxOperatingMode.MouseEnter += new System.EventHandler(this.control_MouseEnter);
			this.gBoxOperatingMode.MouseLeave += new System.EventHandler(this.control_MouseLeave);
			// 
			// rBtnTransmitter
			// 
			this.rBtnTransmitter.AutoSize = true;
			this.rBtnTransmitter.Location = new System.Drawing.Point(94, 80);
			this.rBtnTransmitter.Name = "rBtnTransmitter";
			this.rBtnTransmitter.Size = new System.Drawing.Size(77, 17);
			this.rBtnTransmitter.TabIndex = 4;
			this.rBtnTransmitter.Text = "Transmitter";
			this.rBtnTransmitter.UseVisualStyleBackColor = true;
			this.rBtnTransmitter.CheckedChanged += new System.EventHandler(this.rBtnOperatingMode_CheckedChanged);
			// 
			// rBtnReceiver
			// 
			this.rBtnReceiver.AutoSize = true;
			this.rBtnReceiver.Location = new System.Drawing.Point(16, 80);
			this.rBtnReceiver.Name = "rBtnReceiver";
			this.rBtnReceiver.Size = new System.Drawing.Size(68, 17);
			this.rBtnReceiver.TabIndex = 3;
			this.rBtnReceiver.Text = "Receiver";
			this.rBtnReceiver.UseVisualStyleBackColor = true;
			this.rBtnReceiver.CheckedChanged += new System.EventHandler(this.rBtnOperatingMode_CheckedChanged);
			// 
			// rBtnSynthesizer
			// 
			this.rBtnSynthesizer.AutoSize = true;
			this.rBtnSynthesizer.Location = new System.Drawing.Point(94, 51);
			this.rBtnSynthesizer.Name = "rBtnSynthesizer";
			this.rBtnSynthesizer.Size = new System.Drawing.Size(79, 17);
			this.rBtnSynthesizer.TabIndex = 2;
			this.rBtnSynthesizer.Text = "Synthesizer";
			this.rBtnSynthesizer.UseVisualStyleBackColor = true;
			this.rBtnSynthesizer.CheckedChanged += new System.EventHandler(this.rBtnOperatingMode_CheckedChanged);
			// 
			// rBtnStandby
			// 
			this.rBtnStandby.AutoSize = true;
			this.rBtnStandby.Checked = true;
			this.rBtnStandby.Location = new System.Drawing.Point(16, 51);
			this.rBtnStandby.Name = "rBtnStandby";
			this.rBtnStandby.Size = new System.Drawing.Size(64, 17);
			this.rBtnStandby.TabIndex = 1;
			this.rBtnStandby.TabStop = true;
			this.rBtnStandby.Text = "Standby";
			this.rBtnStandby.UseVisualStyleBackColor = true;
			this.rBtnStandby.CheckedChanged += new System.EventHandler(this.rBtnOperatingMode_CheckedChanged);
			// 
			// rBtnSleep
			// 
			this.rBtnSleep.AutoSize = true;
			this.rBtnSleep.Location = new System.Drawing.Point(16, 20);
			this.rBtnSleep.Name = "rBtnSleep";
			this.rBtnSleep.Size = new System.Drawing.Size(52, 17);
			this.rBtnSleep.TabIndex = 0;
			this.rBtnSleep.Text = "Sleep";
			this.rBtnSleep.UseVisualStyleBackColor = true;
			this.rBtnSleep.CheckedChanged += new System.EventHandler(this.rBtnOperatingMode_CheckedChanged);
			// 
			// lbModeReady
			// 
			this.lbModeReady.AutoSize = true;
			this.lbModeReady.Location = new System.Drawing.Point(55, 20);
			this.lbModeReady.Name = "lbModeReady";
			this.lbModeReady.Size = new System.Drawing.Size(65, 13);
			this.lbModeReady.TabIndex = 1;
			this.lbModeReady.Text = "ModeReady";
			// 
			// label19
			// 
			this.label19.AutoSize = true;
			this.label19.Location = new System.Drawing.Point(55, 41);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(51, 13);
			this.label19.TabIndex = 3;
			this.label19.Text = "RxReady";
			// 
			// label18
			// 
			this.label18.AutoSize = true;
			this.label18.Location = new System.Drawing.Point(55, 62);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(50, 13);
			this.label18.TabIndex = 5;
			this.label18.Text = "TxReady";
			// 
			// label17
			// 
			this.label17.AutoSize = true;
			this.label17.Location = new System.Drawing.Point(55, 83);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(42, 13);
			this.label17.TabIndex = 7;
			this.label17.Text = "PllLock";
			// 
			// label23
			// 
			this.label23.AutoSize = true;
			this.label23.Location = new System.Drawing.Point(55, 110);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(27, 13);
			this.label23.TabIndex = 9;
			this.label23.Text = "Rssi";
			// 
			// label22
			// 
			this.label22.AutoSize = true;
			this.label22.Location = new System.Drawing.Point(55, 131);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(45, 13);
			this.label22.TabIndex = 11;
			this.label22.Text = "Timeout";
			// 
			// label21
			// 
			this.label21.AutoSize = true;
			this.label21.Location = new System.Drawing.Point(55, 152);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(56, 13);
			this.label21.TabIndex = 13;
			this.label21.Text = "AutoMode";
			// 
			// label20
			// 
			this.label20.AutoSize = true;
			this.label20.Location = new System.Drawing.Point(55, 173);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(99, 13);
			this.label20.TabIndex = 15;
			this.label20.Text = "SyncAddressMatch";
			// 
			// label27
			// 
			this.label27.AutoSize = true;
			this.label27.Location = new System.Drawing.Point(55, 200);
			this.label27.Name = "label27";
			this.label27.Size = new System.Drawing.Size(40, 13);
			this.label27.TabIndex = 17;
			this.label27.Text = "FifoFull";
			// 
			// label26
			// 
			this.label26.AutoSize = true;
			this.label26.Location = new System.Drawing.Point(55, 221);
			this.label26.Name = "label26";
			this.label26.Size = new System.Drawing.Size(70, 13);
			this.label26.TabIndex = 19;
			this.label26.Text = "FifoNotEmpty";
			// 
			// label25
			// 
			this.label25.AutoSize = true;
			this.label25.Location = new System.Drawing.Point(55, 242);
			this.label25.Name = "label25";
			this.label25.Size = new System.Drawing.Size(50, 13);
			this.label25.TabIndex = 21;
			this.label25.Text = "FifoLevel";
			// 
			// label24
			// 
			this.label24.AutoSize = true;
			this.label24.Location = new System.Drawing.Point(55, 263);
			this.label24.Name = "label24";
			this.label24.Size = new System.Drawing.Size(62, 13);
			this.label24.TabIndex = 23;
			this.label24.Text = "FifoOverrun";
			// 
			// label31
			// 
			this.label31.AutoSize = true;
			this.label31.Location = new System.Drawing.Point(55, 290);
			this.label31.Name = "label31";
			this.label31.Size = new System.Drawing.Size(63, 13);
			this.label31.TabIndex = 25;
			this.label31.Text = "PacketSent";
			// 
			// label30
			// 
			this.label30.AutoSize = true;
			this.label30.Location = new System.Drawing.Point(55, 311);
			this.label30.Name = "label30";
			this.label30.Size = new System.Drawing.Size(76, 13);
			this.label30.TabIndex = 27;
			this.label30.Text = "PayloadReady";
			// 
			// label29
			// 
			this.label29.AutoSize = true;
			this.label29.Location = new System.Drawing.Point(55, 332);
			this.label29.Name = "label29";
			this.label29.Size = new System.Drawing.Size(37, 13);
			this.label29.TabIndex = 29;
			this.label29.Text = "CrcOk";
			// 
			// label28
			// 
			this.label28.AutoSize = true;
			this.label28.Location = new System.Drawing.Point(55, 353);
			this.label28.Name = "label28";
			this.label28.Size = new System.Drawing.Size(43, 13);
			this.label28.TabIndex = 31;
			this.label28.Text = "LowBat";
			// 
			// gBoxIrqFlags
			// 
			this.gBoxIrqFlags.Controls.Add(this.ledLowBat);
			this.gBoxIrqFlags.Controls.Add(this.lbModeReady);
			this.gBoxIrqFlags.Controls.Add(this.ledCrcOk);
			this.gBoxIrqFlags.Controls.Add(this.ledRxReady);
			this.gBoxIrqFlags.Controls.Add(this.ledPayloadReady);
			this.gBoxIrqFlags.Controls.Add(this.ledTxReady);
			this.gBoxIrqFlags.Controls.Add(this.ledPacketSent);
			this.gBoxIrqFlags.Controls.Add(this.label17);
			this.gBoxIrqFlags.Controls.Add(this.label31);
			this.gBoxIrqFlags.Controls.Add(this.ledPllLock);
			this.gBoxIrqFlags.Controls.Add(this.label30);
			this.gBoxIrqFlags.Controls.Add(this.label18);
			this.gBoxIrqFlags.Controls.Add(this.label29);
			this.gBoxIrqFlags.Controls.Add(this.label19);
			this.gBoxIrqFlags.Controls.Add(this.label28);
			this.gBoxIrqFlags.Controls.Add(this.ledModeReady);
			this.gBoxIrqFlags.Controls.Add(this.ledFifoOverrun);
			this.gBoxIrqFlags.Controls.Add(this.ledRssi);
			this.gBoxIrqFlags.Controls.Add(this.ledFifoLevel);
			this.gBoxIrqFlags.Controls.Add(this.ledTimeout);
			this.gBoxIrqFlags.Controls.Add(this.label27);
			this.gBoxIrqFlags.Controls.Add(this.label20);
			this.gBoxIrqFlags.Controls.Add(this.ledFifoNotEmpty);
			this.gBoxIrqFlags.Controls.Add(this.label21);
			this.gBoxIrqFlags.Controls.Add(this.label26);
			this.gBoxIrqFlags.Controls.Add(this.ledAutoMode);
			this.gBoxIrqFlags.Controls.Add(this.label25);
			this.gBoxIrqFlags.Controls.Add(this.label22);
			this.gBoxIrqFlags.Controls.Add(this.label24);
			this.gBoxIrqFlags.Controls.Add(this.label23);
			this.gBoxIrqFlags.Controls.Add(this.ledFifoFull);
			this.gBoxIrqFlags.Controls.Add(this.ledSyncAddressMatch);
			this.gBoxIrqFlags.Location = new System.Drawing.Point(816, 25);
			this.gBoxIrqFlags.Name = "gBoxIrqFlags";
			this.gBoxIrqFlags.Size = new System.Drawing.Size(189, 380);
			this.gBoxIrqFlags.TabIndex = 1;
			this.gBoxIrqFlags.TabStop = false;
			this.gBoxIrqFlags.Text = "Irq flags";
			this.gBoxIrqFlags.MouseEnter += new System.EventHandler(this.control_MouseEnter);
			this.gBoxIrqFlags.MouseLeave += new System.EventHandler(this.control_MouseLeave);
			// 
			// ledLowBat
			// 
			this.ledLowBat.BackColor = System.Drawing.Color.Transparent;
			this.ledLowBat.LedColor = System.Drawing.Color.Green;
			this.ledLowBat.LedSize = new System.Drawing.Size(11, 11);
			this.ledLowBat.Location = new System.Drawing.Point(34, 352);
			this.ledLowBat.Name = "ledLowBat";
			this.ledLowBat.Size = new System.Drawing.Size(15, 15);
			this.ledLowBat.TabIndex = 30;
			this.ledLowBat.Text = "led1";
			// 
			// ledCrcOk
			// 
			this.ledCrcOk.BackColor = System.Drawing.Color.Transparent;
			this.ledCrcOk.LedColor = System.Drawing.Color.Green;
			this.ledCrcOk.LedSize = new System.Drawing.Size(11, 11);
			this.ledCrcOk.Location = new System.Drawing.Point(34, 331);
			this.ledCrcOk.Name = "ledCrcOk";
			this.ledCrcOk.Size = new System.Drawing.Size(15, 15);
			this.ledCrcOk.TabIndex = 28;
			this.ledCrcOk.Text = "led1";
			// 
			// ledRxReady
			// 
			this.ledRxReady.BackColor = System.Drawing.Color.Transparent;
			this.ledRxReady.LedColor = System.Drawing.Color.Green;
			this.ledRxReady.LedSize = new System.Drawing.Size(11, 11);
			this.ledRxReady.Location = new System.Drawing.Point(34, 40);
			this.ledRxReady.Name = "ledRxReady";
			this.ledRxReady.Size = new System.Drawing.Size(15, 15);
			this.ledRxReady.TabIndex = 2;
			this.ledRxReady.Text = "led1";
			// 
			// ledPayloadReady
			// 
			this.ledPayloadReady.BackColor = System.Drawing.Color.Transparent;
			this.ledPayloadReady.LedColor = System.Drawing.Color.Green;
			this.ledPayloadReady.LedSize = new System.Drawing.Size(11, 11);
			this.ledPayloadReady.Location = new System.Drawing.Point(34, 310);
			this.ledPayloadReady.Name = "ledPayloadReady";
			this.ledPayloadReady.Size = new System.Drawing.Size(15, 15);
			this.ledPayloadReady.TabIndex = 26;
			this.ledPayloadReady.Text = "led1";
			// 
			// ledTxReady
			// 
			this.ledTxReady.BackColor = System.Drawing.Color.Transparent;
			this.ledTxReady.LedColor = System.Drawing.Color.Green;
			this.ledTxReady.LedSize = new System.Drawing.Size(11, 11);
			this.ledTxReady.Location = new System.Drawing.Point(34, 61);
			this.ledTxReady.Name = "ledTxReady";
			this.ledTxReady.Size = new System.Drawing.Size(15, 15);
			this.ledTxReady.TabIndex = 4;
			this.ledTxReady.Text = "led1";
			// 
			// ledPacketSent
			// 
			this.ledPacketSent.BackColor = System.Drawing.Color.Transparent;
			this.ledPacketSent.LedColor = System.Drawing.Color.Green;
			this.ledPacketSent.LedSize = new System.Drawing.Size(11, 11);
			this.ledPacketSent.Location = new System.Drawing.Point(34, 289);
			this.ledPacketSent.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
			this.ledPacketSent.Name = "ledPacketSent";
			this.ledPacketSent.Size = new System.Drawing.Size(15, 15);
			this.ledPacketSent.TabIndex = 24;
			this.ledPacketSent.Text = "led1";
			// 
			// ledPllLock
			// 
			this.ledPllLock.BackColor = System.Drawing.Color.Transparent;
			this.ledPllLock.LedColor = System.Drawing.Color.Green;
			this.ledPllLock.LedSize = new System.Drawing.Size(11, 11);
			this.ledPllLock.Location = new System.Drawing.Point(34, 82);
			this.ledPllLock.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
			this.ledPllLock.Name = "ledPllLock";
			this.ledPllLock.Size = new System.Drawing.Size(15, 15);
			this.ledPllLock.TabIndex = 6;
			this.ledPllLock.Text = "led1";
			// 
			// ledModeReady
			// 
			this.ledModeReady.BackColor = System.Drawing.Color.Transparent;
			this.ledModeReady.LedColor = System.Drawing.Color.Green;
			this.ledModeReady.LedSize = new System.Drawing.Size(11, 11);
			this.ledModeReady.Location = new System.Drawing.Point(34, 19);
			this.ledModeReady.Name = "ledModeReady";
			this.ledModeReady.Size = new System.Drawing.Size(15, 15);
			this.ledModeReady.TabIndex = 0;
			this.ledModeReady.Text = "Mode Ready";
			// 
			// ledFifoOverrun
			// 
			this.ledFifoOverrun.BackColor = System.Drawing.Color.Transparent;
			this.ledFifoOverrun.LedColor = System.Drawing.Color.Green;
			this.ledFifoOverrun.LedSize = new System.Drawing.Size(11, 11);
			this.ledFifoOverrun.Location = new System.Drawing.Point(34, 262);
			this.ledFifoOverrun.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
			this.ledFifoOverrun.Name = "ledFifoOverrun";
			this.ledFifoOverrun.Size = new System.Drawing.Size(15, 15);
			this.ledFifoOverrun.TabIndex = 22;
			this.ledFifoOverrun.Text = "led1";
			// 
			// ledRssi
			// 
			this.ledRssi.BackColor = System.Drawing.Color.Transparent;
			this.ledRssi.LedColor = System.Drawing.Color.Green;
			this.ledRssi.LedSize = new System.Drawing.Size(11, 11);
			this.ledRssi.Location = new System.Drawing.Point(34, 109);
			this.ledRssi.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
			this.ledRssi.Name = "ledRssi";
			this.ledRssi.Size = new System.Drawing.Size(15, 15);
			this.ledRssi.TabIndex = 8;
			this.ledRssi.Text = "led1";
			// 
			// ledFifoLevel
			// 
			this.ledFifoLevel.BackColor = System.Drawing.Color.Transparent;
			this.ledFifoLevel.LedColor = System.Drawing.Color.Green;
			this.ledFifoLevel.LedSize = new System.Drawing.Size(11, 11);
			this.ledFifoLevel.Location = new System.Drawing.Point(34, 241);
			this.ledFifoLevel.Name = "ledFifoLevel";
			this.ledFifoLevel.Size = new System.Drawing.Size(15, 15);
			this.ledFifoLevel.TabIndex = 20;
			this.ledFifoLevel.Text = "led1";
			// 
			// ledTimeout
			// 
			this.ledTimeout.BackColor = System.Drawing.Color.Transparent;
			this.ledTimeout.LedColor = System.Drawing.Color.Green;
			this.ledTimeout.LedSize = new System.Drawing.Size(11, 11);
			this.ledTimeout.Location = new System.Drawing.Point(34, 130);
			this.ledTimeout.Name = "ledTimeout";
			this.ledTimeout.Size = new System.Drawing.Size(15, 15);
			this.ledTimeout.TabIndex = 10;
			this.ledTimeout.Text = "led1";
			// 
			// ledFifoNotEmpty
			// 
			this.ledFifoNotEmpty.BackColor = System.Drawing.Color.Transparent;
			this.ledFifoNotEmpty.LedColor = System.Drawing.Color.Green;
			this.ledFifoNotEmpty.LedSize = new System.Drawing.Size(11, 11);
			this.ledFifoNotEmpty.Location = new System.Drawing.Point(34, 220);
			this.ledFifoNotEmpty.Name = "ledFifoNotEmpty";
			this.ledFifoNotEmpty.Size = new System.Drawing.Size(15, 15);
			this.ledFifoNotEmpty.TabIndex = 18;
			this.ledFifoNotEmpty.Text = "led1";
			// 
			// ledAutoMode
			// 
			this.ledAutoMode.BackColor = System.Drawing.Color.Transparent;
			this.ledAutoMode.LedColor = System.Drawing.Color.Green;
			this.ledAutoMode.LedSize = new System.Drawing.Size(11, 11);
			this.ledAutoMode.Location = new System.Drawing.Point(34, 151);
			this.ledAutoMode.Name = "ledAutoMode";
			this.ledAutoMode.Size = new System.Drawing.Size(15, 15);
			this.ledAutoMode.TabIndex = 12;
			this.ledAutoMode.Text = "led1";
			// 
			// ledFifoFull
			// 
			this.ledFifoFull.BackColor = System.Drawing.Color.Transparent;
			this.ledFifoFull.LedColor = System.Drawing.Color.Green;
			this.ledFifoFull.LedSize = new System.Drawing.Size(11, 11);
			this.ledFifoFull.Location = new System.Drawing.Point(34, 199);
			this.ledFifoFull.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
			this.ledFifoFull.Name = "ledFifoFull";
			this.ledFifoFull.Size = new System.Drawing.Size(15, 15);
			this.ledFifoFull.TabIndex = 16;
			this.ledFifoFull.Text = "led1";
			// 
			// ledSyncAddressMatch
			// 
			this.ledSyncAddressMatch.BackColor = System.Drawing.Color.Transparent;
			this.ledSyncAddressMatch.LedColor = System.Drawing.Color.Green;
			this.ledSyncAddressMatch.LedSize = new System.Drawing.Size(11, 11);
			this.ledSyncAddressMatch.Location = new System.Drawing.Point(34, 172);
			this.ledSyncAddressMatch.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
			this.ledSyncAddressMatch.Name = "ledSyncAddressMatch";
			this.ledSyncAddressMatch.Size = new System.Drawing.Size(15, 15);
			this.ledSyncAddressMatch.TabIndex = 14;
			this.ledSyncAddressMatch.Text = "led1";
			// 
			// DeviceViewControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.gBoxOperatingMode);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.gBoxIrqFlags);
			this.Name = "DeviceViewControl";
			this.Size = new System.Drawing.Size(1008, 525);
			this.tabControl1.ResumeLayout(false);
			this.tabCommon.ResumeLayout(false);
			this.tabTransmitter.ResumeLayout(false);
			this.tabReceiver.ResumeLayout(false);
			this.tabIrqMap.ResumeLayout(false);
			this.tabPacketHandler.ResumeLayout(false);
			this.tabTemperature.ResumeLayout(false);
			this.gBoxOperatingMode.ResumeLayout(false);
			this.gBoxOperatingMode.PerformLayout();
			this.gBoxIrqFlags.ResumeLayout(false);
			this.gBoxIrqFlags.PerformLayout();
			this.ResumeLayout(false);

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

		private void irqMapViewControl1_Load(object sender, EventArgs e)
		{

		}
    }
}

