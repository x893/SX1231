using SemtechLib.Controls;
using SemtechLib.Controls.HexBoxCtrl;
using SemtechLib.Devices.SX1231.Enumerations;
using SemtechLib.Devices.SX1231.Events;
using SemtechLib.Devices.SX1231.General;
using SemtechLib.General.Events;
using SemtechLib.General.Interfaces;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace SemtechLib.Devices.SX1231.Controls
{
	public class PacketHandlerView : UserControl, INotifyDocumentationChanged
	{
		private byte[] aesKey = new byte[0x10];
		private MaskValidationType aesWord = new MaskValidationType("00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00");
		private decimal bitRate;
		private ComboBox cBoxEnterCondition;
		private ComboBox cBoxExitCondition;
		private ComboBox cBoxIntermediateMode;
		private ComboBox cBoxInterPacketRxDelay;
		private CheckBox cBtnLog;
		private CheckBox cBtnPacketHandlerStartStop;
		private IContainer components;
		private ushort crc;
		private DataModeEnum dataMode;
		private ErrorProvider errorProvider;
		private GroupBoxEx gBoxControl;
		private GroupBoxEx gBoxDeviceStatus;
		private GroupBoxEx gBoxMessage;
		private GroupBoxEx gBoxPacket;
		private HexBox hexBoxPayload;
		private PayloadImg imgPacketMessage;
		private bool inHexPayloadDataChanged;
		private Label label1;
		private Label label10;
		private Label label11;
		private Label label12;
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
		private Label label35;
		private Label label36;
		private Label label37;
		private Label label38;
		private Label label39;
		private Label label4;
		private Label label5;
		private Label label6;
		private Label label7;
		private Label label8;
		private Label label9;
		private Label lblBitSynchroniser;
		private Label lblBroadcastAddress;
		private Label lblDataMode;
		private Label lblInterPacketRxDelayUnit;
		private Label lblNodeAddress;
		private Label lblOperatingMode;
		private Label lblPacketAddr;
		private Label lblPacketCrc;
		private Label lblPacketLength;
		private Label lblPacketPreamble;
		private Label lblPacketsNb;
		private Label lblPacketsRepeatValue;
		private Label lblPacketSyncValue;
		private Label lblPayload;
		private Label lblPayloadLength;
		private Led ledPacketCrc;
		private byte[] message;
		private OperatingModeEnum mode = OperatingModeEnum.Stdby;
		private NumericUpDownEx nudBroadcastAddress;
		private NumericUpDownEx nudFifoThreshold;
		private NumericUpDownEx nudNodeAddress;
		private NumericUpDownEx nudPayloadLength;
		private NumericUpDownEx nudPreambleSize;
		private NumericUpDownEx nudSyncSize;
		private NumericUpDownEx nudSyncTol;
		private Panel pnlAddressFiltering;
		private Panel pnlAddressInPayload;
		private Panel pnlAesEncryption;
		private Panel pnlBroadcastAddress;
		private Panel pnlCrcAutoClear;
		private Panel pnlCrcCalculation;
		private Panel pnlDcFree;
		private Panel pnlFifoFillCondition;
		private Panel pnlNodeAddress;
		private Panel pnlPacketAddr;
		private Panel pnlPacketCrc;
		private Panel pnlPacketFormat;
		private Panel pnlPayloadLength;
		private Panel pnlSync;
		private Panel pnlTxStart;
		private RadioButton rBtnAddressFilteringNode;
		private RadioButton rBtnAddressFilteringNodeBroadcast;
		private RadioButton rBtnAddressFilteringOff;
		private RadioButton rBtnAesOff;
		private RadioButton rBtnAesOn;
		private RadioButton rBtnCrcAutoClearOff;
		private RadioButton rBtnCrcAutoClearOn;
		private RadioButton rBtnCrcOff;
		private RadioButton rBtnCrcOn;
		private RadioButton rBtnDcFreeManchester;
		private RadioButton rBtnDcFreeOff;
		private RadioButton rBtnDcFreeWhitening;
		private RadioButton rBtnFifoFillAlways;
		private RadioButton rBtnFifoFillSyncAddress;
		private RadioButton rBtnNodeAddressInPayloadNo;
		private RadioButton rBtnNodeAddressInPayloadYes;
		private RadioButton rBtnPacketFormatFixed;
		private RadioButton rBtnPacketFormatVariable;
		private RadioButton rBtnSyncOff;
		private RadioButton rBtnSyncOn;
		private RadioButton rBtnTxStartFifoLevel;
		private RadioButton rBtnTxStartFifoNotEmpty;
		private byte[] syncValue = new byte[] { 0x69, 0x81, 0x7e, 150 };
		private MaskValidationType syncWord = new MaskValidationType("69-81-7E-96");
		private TableLayoutPanel tableLayoutPanel1;
		private TableLayoutPanel tableLayoutPanel2;
		private TableLayoutPanel tblPacket;
		private TableLayoutPanel tblPayloadMessage;
		private MaskedTextBox tBoxAesKey;
		private TextBox tBoxPacketsNb;
		private TextBox tBoxPacketsRepeatValue;
		private MaskedTextBox tBoxSyncValue;

		public event AddressFilteringEventHandler AddressFilteringChanged;
		public event ByteArrayEventHandler AesKeyChanged;
		public event BooleanEventHandler AesOnChanged;
		public event ByteEventHandler BroadcastAddressChanged;
		public event BooleanEventHandler CrcAutoClearOffChanged;
		public event BooleanEventHandler CrcOnChanged;
		public event DcFreeEventHandler DcFreeChanged;
		public event DocumentationChangedEventHandler DocumentationChanged;
		public event EnterConditionEventHandler EnterConditionChanged;
		public event ErrorEventHandler Error;
		public event ExitConditionEventHandler ExitConditionChanged;
		public event FifoFillConditionEventHandler FifoFillConditionChanged;
		public event ByteEventHandler FifoThresholdChanged;
		public event IntermediateModeEventHandler IntermediateModeChanged;
		public event Int32EventHandler InterPacketRxDelayChanged;
		public event Int32EventHandler MaxPacketNumberChanged;
		public event ByteArrayEventHandler MessageChanged;
		public event Int32EventHandler MessageLengthChanged;
		public event ByteEventHandler NodeAddressChanged;
		public event PacketFormatEventHandler PacketFormatChanged;
		public event BooleanEventHandler PacketHandlerLogEnableChanged;
		public event ByteEventHandler PayloadLengthChanged;
		public event Int32EventHandler PreambleSizeChanged;
		public event BooleanEventHandler StartStopChanged;
		public event BooleanEventHandler SyncOnChanged;
		public event ByteEventHandler SyncSizeChanged;
		public event ByteEventHandler SyncTolChanged;
		public event ByteArrayEventHandler SyncValueChanged;
		public event BooleanEventHandler TxStartConditionChanged;

		public PacketHandlerView()
		{
			InitializeComponent();
			tBoxSyncValue.TextChanged -= new EventHandler(tBoxSyncValue_TextChanged);
			tBoxSyncValue.MaskInputRejected -= new MaskInputRejectedEventHandler(tBoxSyncValue_MaskInputRejected);
			tBoxSyncValue.ValidatingType = typeof(MaskValidationType);
			tBoxSyncValue.Mask = "&&-&&-&&-&&";
			tBoxSyncValue.TextChanged += new EventHandler(tBoxSyncValue_TextChanged);
			tBoxSyncValue.MaskInputRejected += new MaskInputRejectedEventHandler(tBoxSyncValue_MaskInputRejected);
			tBoxAesKey.TextChanged -= new EventHandler(tBoxAesKey_TextChanged);
			tBoxAesKey.MaskInputRejected -= new MaskInputRejectedEventHandler(tBoxAesKey_MaskInputRejected);
			tBoxAesKey.ValidatingType = typeof(MaskValidationType);
			tBoxAesKey.Mask = "&&-&&-&&-&&-&&-&&-&&-&&-&&-&&-&&-&&-&&-&&-&&-&&";
			tBoxAesKey.TextChanged += new EventHandler(tBoxAesKey_TextChanged);
			tBoxAesKey.MaskInputRejected += new MaskInputRejectedEventHandler(tBoxAesKey_MaskInputRejected);
			message = new byte[0];
			byte[] data = new byte[Message.Length];
			hexBoxPayload.ByteProvider = new DynamicByteProvider(data);
			hexBoxPayload.ByteProvider.Changed += new EventHandler(hexBoxPayload_DataChanged);
			hexBoxPayload.ByteProvider.ApplyChanges();
		}

		private void cBoxEnterCondition_SelectedIndexChanged(object sender, EventArgs e)
		{
			EnterCondition = (EnterConditionEnum)cBoxEnterCondition.SelectedIndex;
			OnEnterConditionChanged(EnterCondition);
		}

		private void cBoxExitCondition_SelectedIndexChanged(object sender, EventArgs e)
		{
			ExitCondition = (ExitConditionEnum)cBoxExitCondition.SelectedIndex;
			OnExitConditionChanged(ExitCondition);
		}

		private void cBoxIntermediateMode_SelectedIndexChanged(object sender, EventArgs e)
		{
			IntermediateMode = (IntermediateModeEnum)cBoxIntermediateMode.SelectedIndex;
			OnIntermediateModeChanged(IntermediateMode);
		}

		private void cBtnLog_CheckedChanged(object sender, EventArgs e)
		{
			OnPacketHandlerLogEnableChanged(cBtnLog.Checked);
		}

		private void cBtnPacketHandlerStartStop_CheckedChanged(object sender, EventArgs e)
		{
			if (cBtnPacketHandlerStartStop.Checked)
				cBtnPacketHandlerStartStop.Text = "Stop";
			else
				cBtnPacketHandlerStartStop.Text = "Start";
			tableLayoutPanel1.Enabled = !cBtnPacketHandlerStartStop.Checked;
			tableLayoutPanel2.Enabled = !cBtnPacketHandlerStartStop.Checked;
			gBoxPacket.Enabled = !cBtnPacketHandlerStartStop.Checked;
			tBoxPacketsRepeatValue.Enabled = !cBtnPacketHandlerStartStop.Checked;
			try
			{
				MaxPacketNumber = Convert.ToInt32(tBoxPacketsRepeatValue.Text);
			}
			catch
			{
				MaxPacketNumber = 0;
				tBoxPacketsRepeatValue.Text = "0";
				OnError(1, "Wrong max packet value! Value has been reseted.");
			}
			OnMaxPacketNumberChanged(MaxPacketNumber);
			OnStartStopChanged(cBtnPacketHandlerStartStop.Checked);
		}

		private void control_MouseEnter(object sender, EventArgs e)
		{
			if (sender == nudPreambleSize)
				OnDocumentationChanged(new DocumentationChangedEventArgs("Packet handler", "Preamble size"));
			else if (((sender == pnlSync) || (sender == rBtnSyncOn)) || (sender == rBtnSyncOff))
				OnDocumentationChanged(new DocumentationChangedEventArgs("Packet handler", "Sync word"));
			else if (((sender == pnlFifoFillCondition) || (sender == rBtnFifoFillSyncAddress)) || (sender == rBtnFifoFillAlways))
				OnDocumentationChanged(new DocumentationChangedEventArgs("Packet handler", "Fifo fill condition"));
			else if (sender == nudSyncSize)
				OnDocumentationChanged(new DocumentationChangedEventArgs("Packet handler", "Sync word size"));
			else if (sender == nudSyncTol)
				OnDocumentationChanged(new DocumentationChangedEventArgs("Packet handler", "Sync word tolerance"));
			else if (sender == tBoxSyncValue)
				OnDocumentationChanged(new DocumentationChangedEventArgs("Packet handler", "Sync word value"));
			else if (((sender == pnlPacketFormat) || (sender == rBtnPacketFormatFixed)) || (sender == rBtnPacketFormatVariable))
				OnDocumentationChanged(new DocumentationChangedEventArgs("Packet handler", "Packet format"));
			else if (((sender == pnlPayloadLength) || (sender == nudPayloadLength)) || (sender == lblPayloadLength))
				OnDocumentationChanged(new DocumentationChangedEventArgs("Packet handler", "Payload length"));
			else if (sender == cBoxEnterCondition)
				OnDocumentationChanged(new DocumentationChangedEventArgs("Packet handler", "Intermediate mode enter"));
			else if (sender == cBoxExitCondition)
				OnDocumentationChanged(new DocumentationChangedEventArgs("Packet handler", "Intermediate mode exit"));
			else if (sender == cBoxIntermediateMode)
				OnDocumentationChanged(new DocumentationChangedEventArgs("Packet handler", "Intermediate mode"));
			else if (((sender == pnlAddressInPayload) || (sender == rBtnNodeAddressInPayloadYes)) || (sender == rBtnNodeAddressInPayloadNo))
				OnDocumentationChanged(new DocumentationChangedEventArgs("Packet handler", "Address in payload"));
			else if (((sender == pnlAddressFiltering) || (sender == rBtnAddressFilteringOff)) || ((sender == rBtnAddressFilteringNode) || (sender == rBtnAddressFilteringNodeBroadcast)))
				OnDocumentationChanged(new DocumentationChangedEventArgs("Packet handler", "Address filtering"));
			else if (((sender == pnlNodeAddress) || (sender == nudNodeAddress)) || (sender == lblNodeAddress))
				OnDocumentationChanged(new DocumentationChangedEventArgs("Packet handler", "Node address"));
			else if (((sender == pnlBroadcastAddress) || (sender == nudBroadcastAddress)) || (sender == lblBroadcastAddress))
				OnDocumentationChanged(new DocumentationChangedEventArgs("Packet handler", "Broadcast address"));
			else if (((sender == pnlDcFree) || (sender == rBtnDcFreeOff)) || ((sender == rBtnDcFreeManchester) || (sender == rBtnDcFreeWhitening)))
				OnDocumentationChanged(new DocumentationChangedEventArgs("Packet handler", "Dc free"));
			else if (((sender == pnlCrcCalculation) || (sender == rBtnCrcOn)) || (sender == rBtnCrcOff))
				OnDocumentationChanged(new DocumentationChangedEventArgs("Packet handler", "Crc calculation"));
			else if (((sender == pnlCrcAutoClear) || (sender == rBtnCrcAutoClearOn)) || (sender == rBtnCrcAutoClearOff))
				OnDocumentationChanged(new DocumentationChangedEventArgs("Packet handler", "Crc auto clear"));
			else if (((sender == pnlAesEncryption) || (sender == rBtnAesOn)) || (sender == rBtnAesOff))
				OnDocumentationChanged(new DocumentationChangedEventArgs("Packet handler", "Aes encryption"));
			else if (sender == tBoxAesKey)
				OnDocumentationChanged(new DocumentationChangedEventArgs("Packet handler", "Aes key value"));
			else if (((sender == pnlTxStart) || (sender == rBtnTxStartFifoLevel)) || (sender == rBtnTxStartFifoNotEmpty))
				OnDocumentationChanged(new DocumentationChangedEventArgs("Packet handler", "Tx start"));
			else if (sender == nudFifoThreshold)
				OnDocumentationChanged(new DocumentationChangedEventArgs("Packet handler", "Fifo threshold"));
			else if (sender == cBoxInterPacketRxDelay)
				OnDocumentationChanged(new DocumentationChangedEventArgs("Packet handler", "Inter packet rx delay"));
			else if (sender == gBoxControl)
				OnDocumentationChanged(new DocumentationChangedEventArgs("Packet handler", "Control"));
			else if (sender == gBoxPacket)
				OnDocumentationChanged(new DocumentationChangedEventArgs("Packet handler", "Packet"));
			else if (sender == gBoxMessage)
				OnDocumentationChanged(new DocumentationChangedEventArgs("Packet handler", "Message"));
			else if (sender == gBoxDeviceStatus)
				OnDocumentationChanged(new DocumentationChangedEventArgs("Common", "Device status"));
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

		private void hexBoxPayload_DataChanged(object sender, EventArgs e)
		{
			if (!inHexPayloadDataChanged)
			{
				inHexPayloadDataChanged = true;
				if (hexBoxPayload.ByteProvider.Length > 0x40L)
				{
					hexBoxPayload.ByteProvider.DeleteBytes(0x40L, hexBoxPayload.ByteProvider.Length - 0x40L);
					hexBoxPayload.SelectionStart = 0x40L;
					hexBoxPayload.SelectionLength = 0L;
				}
				Array.Resize<byte>(ref message, (int)hexBoxPayload.ByteProvider.Length);
				for (int i = 0; i < message.Length; i++)
					message[i] = hexBoxPayload.ByteProvider.ReadByte((long)i);
				OnMessageChanged(Message);
				inHexPayloadDataChanged = false;
			}
		}

		private void InitializeComponent()
		{
			components = new Container();
			errorProvider = new ErrorProvider(components);
			tBoxSyncValue = new MaskedTextBox();
			nudPreambleSize = new NumericUpDownEx();
			label12 = new Label();
			label1 = new Label();
			label6 = new Label();
			label8 = new Label();
			label2 = new Label();
			label18 = new Label();
			tBoxAesKey = new MaskedTextBox();
			label11 = new Label();
			label20 = new Label();
			label10 = new Label();
			label21 = new Label();
			label7 = new Label();
			label5 = new Label();
			label25 = new Label();
			label24 = new Label();
			label19 = new Label();
			lblInterPacketRxDelayUnit = new Label();
			cBoxEnterCondition = new ComboBox();
			label14 = new Label();
			cBoxExitCondition = new ComboBox();
			label15 = new Label();
			cBoxIntermediateMode = new ComboBox();
			label28 = new Label();
			label16 = new Label();
			label27 = new Label();
			label26 = new Label();
			pnlAesEncryption = new Panel();
			rBtnAesOff = new RadioButton();
			rBtnAesOn = new RadioButton();
			pnlDcFree = new Panel();
			rBtnDcFreeWhitening = new RadioButton();
			rBtnDcFreeManchester = new RadioButton();
			rBtnDcFreeOff = new RadioButton();
			pnlAddressInPayload = new Panel();
			rBtnNodeAddressInPayloadNo = new RadioButton();
			rBtnNodeAddressInPayloadYes = new RadioButton();
			label17 = new Label();
			pnlFifoFillCondition = new Panel();
			rBtnFifoFillAlways = new RadioButton();
			rBtnFifoFillSyncAddress = new RadioButton();
			label4 = new Label();
			pnlSync = new Panel();
			rBtnSyncOff = new RadioButton();
			rBtnSyncOn = new RadioButton();
			label3 = new Label();
			label9 = new Label();
			pnlCrcAutoClear = new Panel();
			rBtnCrcAutoClearOff = new RadioButton();
			rBtnCrcAutoClearOn = new RadioButton();
			label23 = new Label();
			pnlCrcCalculation = new Panel();
			rBtnCrcOff = new RadioButton();
			rBtnCrcOn = new RadioButton();
			label22 = new Label();
			pnlTxStart = new Panel();
			rBtnTxStartFifoNotEmpty = new RadioButton();
			rBtnTxStartFifoLevel = new RadioButton();
			pnlAddressFiltering = new Panel();
			rBtnAddressFilteringNodeBroadcast = new RadioButton();
			rBtnAddressFilteringNode = new RadioButton();
			rBtnAddressFilteringOff = new RadioButton();
			lblNodeAddress = new Label();
			lblPayloadLength = new Label();
			lblBroadcastAddress = new Label();
			pnlPacketFormat = new Panel();
			rBtnPacketFormatFixed = new RadioButton();
			rBtnPacketFormatVariable = new RadioButton();
			tableLayoutPanel1 = new TableLayoutPanel();
			pnlPayloadLength = new Panel();
			nudPayloadLength = new NumericUpDownEx();
			nudSyncSize = new NumericUpDownEx();
			nudSyncTol = new NumericUpDownEx();
			pnlNodeAddress = new Panel();
			nudNodeAddress = new NumericUpDownEx();
			pnlBroadcastAddress = new Panel();
			nudBroadcastAddress = new NumericUpDownEx();
			tableLayoutPanel2 = new TableLayoutPanel();
			nudFifoThreshold = new NumericUpDownEx();
			cBoxInterPacketRxDelay = new ComboBox();
			gBoxDeviceStatus = new GroupBoxEx();
			lblOperatingMode = new Label();
			label37 = new Label();
			lblBitSynchroniser = new Label();
			lblDataMode = new Label();
			label38 = new Label();
			label39 = new Label();
			gBoxControl = new GroupBoxEx();
			tBoxPacketsNb = new TextBox();
			cBtnLog = new CheckBox();
			cBtnPacketHandlerStartStop = new CheckBox();
			lblPacketsNb = new Label();
			tBoxPacketsRepeatValue = new TextBox();
			lblPacketsRepeatValue = new Label();
			gBoxPacket = new GroupBoxEx();
			imgPacketMessage = new PayloadImg();
			gBoxMessage = new GroupBoxEx();
			tblPayloadMessage = new TableLayoutPanel();
			hexBoxPayload = new HexBox();
			label36 = new Label();
			label35 = new Label();
			tblPacket = new TableLayoutPanel();
			label29 = new Label();
			label30 = new Label();
			label31 = new Label();
			label32 = new Label();
			label33 = new Label();
			label34 = new Label();
			lblPacketPreamble = new Label();
			lblPayload = new Label();
			pnlPacketCrc = new Panel();
			ledPacketCrc = new Led();
			lblPacketCrc = new Label();
			pnlPacketAddr = new Panel();
			lblPacketAddr = new Label();
			lblPacketLength = new Label();
			lblPacketSyncValue = new Label();
			((ISupportInitialize)errorProvider).BeginInit();
			nudPreambleSize.BeginInit();
			pnlAesEncryption.SuspendLayout();
			pnlDcFree.SuspendLayout();
			pnlAddressInPayload.SuspendLayout();
			pnlFifoFillCondition.SuspendLayout();
			pnlSync.SuspendLayout();
			pnlCrcAutoClear.SuspendLayout();
			pnlCrcCalculation.SuspendLayout();
			pnlTxStart.SuspendLayout();
			pnlAddressFiltering.SuspendLayout();
			pnlPacketFormat.SuspendLayout();
			tableLayoutPanel1.SuspendLayout();
			pnlPayloadLength.SuspendLayout();
			nudPayloadLength.BeginInit();
			nudSyncSize.BeginInit();
			nudSyncTol.BeginInit();
			pnlNodeAddress.SuspendLayout();
			nudNodeAddress.BeginInit();
			pnlBroadcastAddress.SuspendLayout();
			nudBroadcastAddress.BeginInit();
			tableLayoutPanel2.SuspendLayout();
			nudFifoThreshold.BeginInit();
			gBoxDeviceStatus.SuspendLayout();
			gBoxControl.SuspendLayout();
			gBoxPacket.SuspendLayout();
			gBoxMessage.SuspendLayout();
			tblPayloadMessage.SuspendLayout();
			tblPacket.SuspendLayout();
			pnlPacketCrc.SuspendLayout();
			pnlPacketAddr.SuspendLayout();
			base.SuspendLayout();
			errorProvider.ContainerControl = this;
			tBoxSyncValue.Anchor = AnchorStyles.Left;
			errorProvider.SetIconPadding(tBoxSyncValue, 6);
			tBoxSyncValue.InsertKeyMode = InsertKeyMode.Overwrite;
			tBoxSyncValue.Location = new Point(0xa3, 0x7a);
			tBoxSyncValue.Margin = new Padding(3, 2, 3, 2);
			tBoxSyncValue.Mask = "&&-&&-&&-&&-&&-&&-&&-&&";
			tBoxSyncValue.Name = "tBoxSyncValue";
			tBoxSyncValue.Size = new Size(0x8f, 20);
			tBoxSyncValue.TabIndex = 14;
			tBoxSyncValue.Text = "AAAAAAAAAAAAAAAA";
			tBoxSyncValue.MaskInputRejected += new MaskInputRejectedEventHandler(tBoxSyncValue_MaskInputRejected);
			tBoxSyncValue.TypeValidationCompleted += new TypeValidationEventHandler(tBoxSyncValue_TypeValidationCompleted);
			tBoxSyncValue.TextChanged += new EventHandler(tBoxSyncValue_TextChanged);
			tBoxSyncValue.KeyDown += new KeyEventHandler(tBoxSyncValue_KeyDown);
			tBoxSyncValue.MouseEnter += new EventHandler(control_MouseEnter);
			tBoxSyncValue.MouseLeave += new EventHandler(control_MouseLeave);
			tBoxSyncValue.Validated += new EventHandler(tBox_Validated);
			nudPreambleSize.Anchor = AnchorStyles.Left;
			errorProvider.SetIconPadding(nudPreambleSize, 6);
			nudPreambleSize.Location = new Point(0xa3, 2);
			nudPreambleSize.Margin = new Padding(3, 2, 3, 2);
			int[] bits = new int[4];
			bits[0] = 0xffff;
			nudPreambleSize.Maximum = new decimal(bits);
			nudPreambleSize.Name = "nudPreambleSize";
			nudPreambleSize.Size = new Size(0x3b, 20);
			nudPreambleSize.TabIndex = 1;
			int[] numArray2 = new int[4];
			numArray2[0] = 3;
			nudPreambleSize.Value = new decimal(numArray2);
			nudPreambleSize.MouseEnter += new EventHandler(control_MouseEnter);
			nudPreambleSize.MouseLeave += new EventHandler(control_MouseLeave);
			nudPreambleSize.ValueChanged += new EventHandler(nudPreambleSize_ValueChanged);
			label12.Anchor = AnchorStyles.None;
			label12.AutoSize = true;
			label12.Location = new Point(0x155, 0xad);
			label12.Name = "label12";
			label12.Size = new Size(0x20, 13);
			label12.TabIndex = 0x13;
			label12.Text = "bytes";
			label12.TextAlign = ContentAlignment.MiddleLeft;
			label1.Anchor = AnchorStyles.Left;
			label1.AutoSize = true;
			label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			label1.Location = new Point(3, 5);
			label1.Name = "label1";
			label1.Size = new Size(0x4b, 13);
			label1.TabIndex = 0;
			label1.Text = "Preamble size:";
			label1.TextAlign = ContentAlignment.MiddleLeft;
			label6.Anchor = AnchorStyles.None;
			label6.AutoSize = true;
			label6.Location = new Point(0x155, 0x4d);
			label6.Name = "label6";
			label6.Size = new Size(0x20, 13);
			label6.TabIndex = 9;
			label6.Text = "bytes";
			label6.TextAlign = ContentAlignment.MiddleLeft;
			label8.Anchor = AnchorStyles.None;
			label8.AutoSize = true;
			label8.Location = new Point(0x159, 0x65);
			label8.Name = "label8";
			label8.Size = new Size(0x17, 13);
			label8.TabIndex = 12;
			label8.Text = "bits";
			label8.TextAlign = ContentAlignment.MiddleLeft;
			label2.Anchor = AnchorStyles.None;
			label2.AutoSize = true;
			label2.Location = new Point(0x155, 5);
			label2.Name = "label2";
			label2.Size = new Size(0x20, 13);
			label2.TabIndex = 2;
			label2.Text = "bytes";
			label2.TextAlign = ContentAlignment.MiddleLeft;
			label18.Anchor = AnchorStyles.Left;
			label18.AutoSize = true;
			label18.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			label18.Location = new Point(3, 0x1d);
			label18.Name = "label18";
			label18.Size = new Size(0x74, 13);
			label18.TabIndex = 2;
			label18.Text = "Address based filtering:";
			label18.TextAlign = ContentAlignment.MiddleLeft;
			tBoxAesKey.Anchor = AnchorStyles.Left;
			tableLayoutPanel2.SetColumnSpan(tBoxAesKey, 2);
			tBoxAesKey.InsertKeyMode = InsertKeyMode.Overwrite;
			tBoxAesKey.Location = new Point(0x81, 0xc3);
			tBoxAesKey.Margin = new Padding(3, 3, 3, 4);
			tBoxAesKey.Mask = "&&-&&-&&-&&-&&-&&-&&-&&-&&-&&-&&-&&-&&-&&-&&-&&";
			tBoxAesKey.Name = "tBoxAesKey";
			tBoxAesKey.Size = new Size(0x115, 20);
			tBoxAesKey.TabIndex = 15;
			tBoxAesKey.Text = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA";
			tBoxAesKey.TextAlign = HorizontalAlignment.Center;
			tBoxAesKey.MaskInputRejected += new MaskInputRejectedEventHandler(tBoxAesKey_MaskInputRejected);
			tBoxAesKey.TypeValidationCompleted += new TypeValidationEventHandler(tBoxAesKey_TypeValidationCompleted);
			tBoxAesKey.TextChanged += new EventHandler(tBoxAesKey_TextChanged);
			tBoxAesKey.KeyDown += new KeyEventHandler(tBoxAesKey_KeyDown);
			tBoxAesKey.MouseEnter += new EventHandler(control_MouseEnter);
			tBoxAesKey.MouseLeave += new EventHandler(control_MouseLeave);
			tBoxAesKey.Validated += new EventHandler(tBox_Validated);
			label11.Anchor = AnchorStyles.Left;
			label11.AutoSize = true;
			label11.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			label11.Location = new Point(3, 0xad);
			label11.Name = "label11";
			label11.Size = new Size(80, 13);
			label11.TabIndex = 0x11;
			label11.Text = "Payload length:";
			label11.TextAlign = ContentAlignment.MiddleLeft;
			label20.Anchor = AnchorStyles.Left;
			label20.AutoSize = true;
			label20.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			label20.Location = new Point(3, 0x4d);
			label20.Name = "label20";
			label20.Size = new Size(0x62, 13);
			label20.TabIndex = 5;
			label20.Text = "Broadcast address:";
			label20.TextAlign = ContentAlignment.MiddleLeft;
			label10.Anchor = AnchorStyles.Left;
			label10.AutoSize = true;
			label10.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			label10.Location = new Point(3, 0x95);
			label10.Name = "label10";
			label10.Size = new Size(0x4c, 13);
			label10.TabIndex = 15;
			label10.Text = "Packet format:";
			label10.TextAlign = ContentAlignment.MiddleLeft;
			label21.Anchor = AnchorStyles.Left;
			label21.AutoSize = true;
			label21.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			label21.Location = new Point(3, 0x65);
			label21.Name = "label21";
			label21.Size = new Size(0x2e, 13);
			label21.TabIndex = 6;
			label21.Text = "DC-free:";
			label21.TextAlign = ContentAlignment.MiddleLeft;
			label7.Anchor = AnchorStyles.Left;
			label7.AutoSize = true;
			label7.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			label7.Location = new Point(3, 0x65);
			label7.Name = "label7";
			label7.Size = new Size(0x6b, 13);
			label7.TabIndex = 10;
			label7.Text = "Sync word tolerance:";
			label7.TextAlign = ContentAlignment.MiddleLeft;
			label5.Anchor = AnchorStyles.Left;
			label5.AutoSize = true;
			label5.BackColor = Color.Transparent;
			label5.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			label5.Location = new Point(3, 0x4d);
			label5.Name = "label5";
			label5.Size = new Size(0x51, 13);
			label5.TabIndex = 7;
			label5.Text = "Sync word size:";
			label5.TextAlign = ContentAlignment.MiddleLeft;
			label25.Anchor = AnchorStyles.Left;
			label25.AutoSize = true;
			label25.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			label25.Location = new Point(3, 0xc7);
			label25.Name = "label25";
			label25.Size = new Size(0x33, 13);
			label25.TabIndex = 14;
			label25.Text = "AES key:";
			label25.TextAlign = ContentAlignment.MiddleLeft;
			label24.Anchor = AnchorStyles.Left;
			label24.AutoSize = true;
			label24.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			label24.Location = new Point(3, 0xad);
			label24.Name = "label24";
			label24.Size = new Size(0x1f, 13);
			label24.TabIndex = 12;
			label24.Text = "AES:";
			label24.TextAlign = ContentAlignment.MiddleLeft;
			label19.Anchor = AnchorStyles.Left;
			label19.AutoSize = true;
			label19.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			label19.Location = new Point(3, 0x35);
			label19.Name = "label19";
			label19.Size = new Size(0x4c, 13);
			label19.TabIndex = 4;
			label19.Text = "Node address:";
			label19.TextAlign = ContentAlignment.MiddleLeft;
			lblInterPacketRxDelayUnit.Anchor = AnchorStyles.None;
			lblInterPacketRxDelayUnit.AutoSize = true;
			lblInterPacketRxDelayUnit.Location = new Point(0x176, 0x113);
			lblInterPacketRxDelayUnit.Name = "lblInterPacketRxDelayUnit";
			lblInterPacketRxDelayUnit.Size = new Size(20, 13);
			lblInterPacketRxDelayUnit.TabIndex = 0x16;
			lblInterPacketRxDelayUnit.Text = "ms";
			lblInterPacketRxDelayUnit.TextAlign = ContentAlignment.MiddleLeft;
			cBoxEnterCondition.Anchor = AnchorStyles.Left;
			cBoxEnterCondition.DropDownStyle = ComboBoxStyle.DropDownList;
			cBoxEnterCondition.FormattingEnabled = true;
			cBoxEnterCondition.Items.AddRange(new object[] { "None ( Auto Modes OFF )", "Rising edge of FifoNotEmpty", "Rising edge of FifoLevel", "Rising edge of CrcOk", "Rising edge of PayloadReady", "Rising edge of SyncAddress", "Rising edge of PacketSent", "Falling edge of FifoNotEmpty" });
			cBoxEnterCondition.Location = new Point(0xa3, 0xc2);
			cBoxEnterCondition.Margin = new Padding(3, 2, 3, 2);
			cBoxEnterCondition.Name = "cBoxEnterCondition";
			cBoxEnterCondition.Size = new Size(0xac, 0x15);
			cBoxEnterCondition.TabIndex = 0x17;
			cBoxEnterCondition.SelectedIndexChanged += new EventHandler(cBoxEnterCondition_SelectedIndexChanged);
			cBoxEnterCondition.MouseEnter += new EventHandler(control_MouseEnter);
			cBoxEnterCondition.MouseLeave += new EventHandler(control_MouseLeave);
			label14.Anchor = AnchorStyles.Left;
			label14.AutoSize = true;
			label14.Location = new Point(3, 0xc6);
			label14.Name = "label14";
			label14.Size = new Size(0x7c, 13);
			label14.TabIndex = 0x16;
			label14.Text = "Intermediate mode enter:";
			label14.TextAlign = ContentAlignment.MiddleLeft;
			cBoxExitCondition.Anchor = AnchorStyles.Left;
			cBoxExitCondition.DropDownStyle = ComboBoxStyle.DropDownList;
			cBoxExitCondition.FormattingEnabled = true;
			cBoxExitCondition.Items.AddRange(new object[] { "None ( Auto Modes OFF )", "Falling edge of FifoNotEmpty", "Rising edge of FifoLevel or Timeout", "Rising edge of CrcOk or TimeOut", "Rising edge of PayloadReady or Timeout", "Rising edge of SyncAddress or Timeout", "Rising edge of PacketSent", "Rising edge of Timeout" });
			cBoxExitCondition.Location = new Point(0xa3, 0xdb);
			cBoxExitCondition.Margin = new Padding(3, 2, 3, 2);
			cBoxExitCondition.Name = "cBoxExitCondition";
			cBoxExitCondition.Size = new Size(0xac, 0x15);
			cBoxExitCondition.TabIndex = 0x19;
			cBoxExitCondition.SelectedIndexChanged += new EventHandler(cBoxExitCondition_SelectedIndexChanged);
			cBoxExitCondition.MouseEnter += new EventHandler(control_MouseEnter);
			cBoxExitCondition.MouseLeave += new EventHandler(control_MouseLeave);
			label15.Anchor = AnchorStyles.Left;
			label15.AutoSize = true;
			label15.Location = new Point(3, 0xdf);
			label15.Name = "label15";
			label15.Size = new Size(0x74, 13);
			label15.TabIndex = 0x18;
			label15.Text = "Intermediate mode exit:";
			label15.TextAlign = ContentAlignment.MiddleLeft;
			cBoxIntermediateMode.Anchor = AnchorStyles.Left;
			cBoxIntermediateMode.DropDownStyle = ComboBoxStyle.DropDownList;
			cBoxIntermediateMode.FormattingEnabled = true;
			cBoxIntermediateMode.Items.AddRange(new object[] { "Sleep", "Standby", "Rx", "Tx" });
			cBoxIntermediateMode.Location = new Point(0xa3, 0xf4);
			cBoxIntermediateMode.Margin = new Padding(3, 2, 3, 2);
			cBoxIntermediateMode.Name = "cBoxIntermediateMode";
			cBoxIntermediateMode.Size = new Size(0xac, 0x15);
			cBoxIntermediateMode.TabIndex = 0x1b;
			cBoxIntermediateMode.SelectedIndexChanged += new EventHandler(cBoxIntermediateMode_SelectedIndexChanged);
			cBoxIntermediateMode.MouseEnter += new EventHandler(control_MouseEnter);
			cBoxIntermediateMode.MouseLeave += new EventHandler(control_MouseLeave);
			label28.Anchor = AnchorStyles.Left;
			label28.AutoSize = true;
			label28.Location = new Point(3, 0x113);
			label28.Name = "label28";
			label28.Size = new Size(0x6f, 13);
			label28.TabIndex = 20;
			label28.Text = "Inter packet Rx delay:";
			label28.TextAlign = ContentAlignment.MiddleLeft;
			label16.Anchor = AnchorStyles.Left;
			label16.AutoSize = true;
			label16.Location = new Point(3, 0xf8);
			label16.Name = "label16";
			label16.Size = new Size(100, 13);
			label16.TabIndex = 0x1a;
			label16.Text = "Intermediate  mode:";
			label16.TextAlign = ContentAlignment.MiddleLeft;
			label27.Anchor = AnchorStyles.Left;
			label27.AutoSize = true;
			label27.Location = new Point(3, 250);
			label27.Name = "label27";
			label27.Size = new Size(0x53, 13);
			label27.TabIndex = 0x12;
			label27.Text = "FIFO Threshold:";
			label27.TextAlign = ContentAlignment.MiddleLeft;
			label26.Anchor = AnchorStyles.Left;
			label26.AutoSize = true;
			label26.Location = new Point(3, 0xe1);
			label26.Name = "label26";
			label26.Size = new Size(0x2d, 13);
			label26.TabIndex = 0x10;
			label26.Text = "Tx start:";
			label26.TextAlign = ContentAlignment.MiddleLeft;
			pnlAesEncryption.Anchor = AnchorStyles.Left;
			pnlAesEncryption.AutoSize = true;
			pnlAesEncryption.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			pnlAesEncryption.Controls.Add(rBtnAesOff);
			pnlAesEncryption.Controls.Add(rBtnAesOn);
			pnlAesEncryption.Location = new Point(0x81, 170);
			pnlAesEncryption.Margin = new Padding(3, 2, 3, 2);
			pnlAesEncryption.Name = "pnlAesEncryption";
			pnlAesEncryption.Size = new Size(0x66, 20);
			pnlAesEncryption.TabIndex = 13;
			pnlAesEncryption.MouseEnter += new EventHandler(control_MouseEnter);
			pnlAesEncryption.MouseLeave += new EventHandler(control_MouseLeave);
			rBtnAesOff.AutoSize = true;
			rBtnAesOff.Location = new Point(0x36, 3);
			rBtnAesOff.Margin = new Padding(3, 0, 3, 0);
			rBtnAesOff.Name = "rBtnAesOff";
			rBtnAesOff.Size = new Size(0x2d, 0x11);
			rBtnAesOff.TabIndex = 1;
			rBtnAesOff.Text = "OFF";
			rBtnAesOff.UseVisualStyleBackColor = true;
			rBtnAesOff.CheckedChanged += new EventHandler(rBtnAesOff_CheckedChanged);
			rBtnAesOff.MouseEnter += new EventHandler(control_MouseEnter);
			rBtnAesOff.MouseLeave += new EventHandler(control_MouseLeave);
			rBtnAesOn.AutoSize = true;
			rBtnAesOn.Checked = true;
			rBtnAesOn.Location = new Point(3, 3);
			rBtnAesOn.Margin = new Padding(3, 0, 3, 0);
			rBtnAesOn.Name = "rBtnAesOn";
			rBtnAesOn.Size = new Size(0x29, 0x11);
			rBtnAesOn.TabIndex = 0;
			rBtnAesOn.TabStop = true;
			rBtnAesOn.Text = "ON";
			rBtnAesOn.UseVisualStyleBackColor = true;
			rBtnAesOn.CheckedChanged += new EventHandler(rBtnAesOn_CheckedChanged);
			rBtnAesOn.MouseEnter += new EventHandler(control_MouseEnter);
			rBtnAesOn.MouseLeave += new EventHandler(control_MouseLeave);
			pnlDcFree.Anchor = AnchorStyles.Left;
			pnlDcFree.AutoSize = true;
			pnlDcFree.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			pnlDcFree.Controls.Add(rBtnDcFreeWhitening);
			pnlDcFree.Controls.Add(rBtnDcFreeManchester);
			pnlDcFree.Controls.Add(rBtnDcFreeOff);
			pnlDcFree.Location = new Point(0x81, 0x62);
			pnlDcFree.Margin = new Padding(3, 2, 3, 2);
			pnlDcFree.Name = "pnlDcFree";
			pnlDcFree.Size = new Size(0xd9, 20);
			pnlDcFree.TabIndex = 7;
			pnlDcFree.MouseEnter += new EventHandler(control_MouseEnter);
			pnlDcFree.MouseLeave += new EventHandler(control_MouseLeave);
			rBtnDcFreeWhitening.AutoSize = true;
			rBtnDcFreeWhitening.Location = new Point(0x8d, 3);
			rBtnDcFreeWhitening.Margin = new Padding(3, 0, 3, 0);
			rBtnDcFreeWhitening.Name = "rBtnDcFreeWhitening";
			rBtnDcFreeWhitening.Size = new Size(0x49, 0x11);
			rBtnDcFreeWhitening.TabIndex = 2;
			rBtnDcFreeWhitening.Text = "Whitening";
			rBtnDcFreeWhitening.UseVisualStyleBackColor = true;
			rBtnDcFreeWhitening.CheckedChanged += new EventHandler(rBtnDcFreeWhitening_CheckedChanged);
			rBtnDcFreeWhitening.MouseEnter += new EventHandler(control_MouseEnter);
			rBtnDcFreeWhitening.MouseLeave += new EventHandler(control_MouseLeave);
			rBtnDcFreeManchester.AutoSize = true;
			rBtnDcFreeManchester.Location = new Point(0x36, 3);
			rBtnDcFreeManchester.Margin = new Padding(3, 0, 3, 0);
			rBtnDcFreeManchester.Name = "rBtnDcFreeManchester";
			rBtnDcFreeManchester.Size = new Size(0x51, 0x11);
			rBtnDcFreeManchester.TabIndex = 1;
			rBtnDcFreeManchester.Text = "Manchester";
			rBtnDcFreeManchester.UseVisualStyleBackColor = true;
			rBtnDcFreeManchester.CheckedChanged += new EventHandler(rBtnDcFreeManchester_CheckedChanged);
			rBtnDcFreeManchester.MouseEnter += new EventHandler(control_MouseEnter);
			rBtnDcFreeManchester.MouseLeave += new EventHandler(control_MouseLeave);
			rBtnDcFreeOff.AutoSize = true;
			rBtnDcFreeOff.Checked = true;
			rBtnDcFreeOff.Location = new Point(3, 3);
			rBtnDcFreeOff.Margin = new Padding(3, 0, 3, 0);
			rBtnDcFreeOff.Name = "rBtnDcFreeOff";
			rBtnDcFreeOff.Size = new Size(0x2d, 0x11);
			rBtnDcFreeOff.TabIndex = 0;
			rBtnDcFreeOff.TabStop = true;
			rBtnDcFreeOff.Text = "OFF";
			rBtnDcFreeOff.UseVisualStyleBackColor = true;
			rBtnDcFreeOff.CheckedChanged += new EventHandler(rBtnDcFreeOff_CheckedChanged);
			rBtnDcFreeOff.MouseEnter += new EventHandler(control_MouseEnter);
			rBtnDcFreeOff.MouseLeave += new EventHandler(control_MouseLeave);
			pnlAddressInPayload.Anchor = AnchorStyles.Left;
			pnlAddressInPayload.AutoSize = true;
			pnlAddressInPayload.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			pnlAddressInPayload.Controls.Add(rBtnNodeAddressInPayloadNo);
			pnlAddressInPayload.Controls.Add(rBtnNodeAddressInPayloadYes);
			pnlAddressInPayload.Location = new Point(0x81, 2);
			pnlAddressInPayload.Margin = new Padding(3, 2, 3, 2);
			pnlAddressInPayload.Name = "pnlAddressInPayload";
			pnlAddressInPayload.Size = new Size(0x62, 20);
			pnlAddressInPayload.TabIndex = 1;
			pnlAddressInPayload.Visible = false;
			pnlAddressInPayload.MouseEnter += new EventHandler(control_MouseEnter);
			pnlAddressInPayload.MouseLeave += new EventHandler(control_MouseLeave);
			rBtnNodeAddressInPayloadNo.AutoSize = true;
			rBtnNodeAddressInPayloadNo.Location = new Point(0x36, 3);
			rBtnNodeAddressInPayloadNo.Margin = new Padding(3, 0, 3, 0);
			rBtnNodeAddressInPayloadNo.Name = "rBtnNodeAddressInPayloadNo";
			rBtnNodeAddressInPayloadNo.Size = new Size(0x29, 0x11);
			rBtnNodeAddressInPayloadNo.TabIndex = 1;
			rBtnNodeAddressInPayloadNo.Text = "NO";
			rBtnNodeAddressInPayloadNo.UseVisualStyleBackColor = true;
			rBtnNodeAddressInPayloadNo.MouseEnter += new EventHandler(control_MouseEnter);
			rBtnNodeAddressInPayloadNo.MouseLeave += new EventHandler(control_MouseLeave);
			rBtnNodeAddressInPayloadYes.AutoSize = true;
			rBtnNodeAddressInPayloadYes.Checked = true;
			rBtnNodeAddressInPayloadYes.Location = new Point(3, 3);
			rBtnNodeAddressInPayloadYes.Margin = new Padding(3, 0, 3, 0);
			rBtnNodeAddressInPayloadYes.Name = "rBtnNodeAddressInPayloadYes";
			rBtnNodeAddressInPayloadYes.Size = new Size(0x2e, 0x11);
			rBtnNodeAddressInPayloadYes.TabIndex = 0;
			rBtnNodeAddressInPayloadYes.TabStop = true;
			rBtnNodeAddressInPayloadYes.Text = "YES";
			rBtnNodeAddressInPayloadYes.UseVisualStyleBackColor = true;
			rBtnNodeAddressInPayloadYes.MouseEnter += new EventHandler(control_MouseEnter);
			rBtnNodeAddressInPayloadYes.MouseLeave += new EventHandler(control_MouseLeave);
			label17.Anchor = AnchorStyles.Left;
			label17.AutoSize = true;
			label17.Location = new Point(3, 5);
			label17.Name = "label17";
			label17.Size = new Size(120, 13);
			label17.TabIndex = 0;
			label17.Text = "Add address in payload:";
			label17.TextAlign = ContentAlignment.MiddleLeft;
			label17.Visible = false;
			pnlFifoFillCondition.Anchor = AnchorStyles.Left;
			pnlFifoFillCondition.AutoSize = true;
			pnlFifoFillCondition.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			pnlFifoFillCondition.Controls.Add(rBtnFifoFillAlways);
			pnlFifoFillCondition.Controls.Add(rBtnFifoFillSyncAddress);
			pnlFifoFillCondition.Location = new Point(0xa3, 50);
			pnlFifoFillCondition.Margin = new Padding(3, 2, 3, 2);
			pnlFifoFillCondition.Name = "pnlFifoFillCondition";
			pnlFifoFillCondition.Size = new Size(0x9f, 20);
			pnlFifoFillCondition.TabIndex = 6;
			pnlFifoFillCondition.MouseEnter += new EventHandler(control_MouseEnter);
			pnlFifoFillCondition.MouseLeave += new EventHandler(control_MouseLeave);
			rBtnFifoFillAlways.AutoSize = true;
			rBtnFifoFillAlways.Location = new Point(0x62, 3);
			rBtnFifoFillAlways.Margin = new Padding(3, 0, 3, 0);
			rBtnFifoFillAlways.Name = "rBtnFifoFillAlways";
			rBtnFifoFillAlways.Size = new Size(0x3a, 0x11);
			rBtnFifoFillAlways.TabIndex = 1;
			rBtnFifoFillAlways.Text = "Always";
			rBtnFifoFillAlways.UseVisualStyleBackColor = true;
			rBtnFifoFillAlways.CheckedChanged += new EventHandler(rBtnFifoFill_CheckedChanged);
			rBtnFifoFillAlways.MouseEnter += new EventHandler(control_MouseEnter);
			rBtnFifoFillAlways.MouseLeave += new EventHandler(control_MouseLeave);
			rBtnFifoFillSyncAddress.AutoSize = true;
			rBtnFifoFillSyncAddress.Checked = true;
			rBtnFifoFillSyncAddress.Location = new Point(3, 3);
			rBtnFifoFillSyncAddress.Margin = new Padding(3, 0, 3, 0);
			rBtnFifoFillSyncAddress.Name = "rBtnFifoFillSyncAddress";
			rBtnFifoFillSyncAddress.Size = new Size(0x59, 0x11);
			rBtnFifoFillSyncAddress.TabIndex = 0;
			rBtnFifoFillSyncAddress.TabStop = true;
			rBtnFifoFillSyncAddress.Text = "Sync address";
			rBtnFifoFillSyncAddress.UseVisualStyleBackColor = true;
			rBtnFifoFillSyncAddress.CheckedChanged += new EventHandler(rBtnFifoFill_CheckedChanged);
			rBtnFifoFillSyncAddress.MouseEnter += new EventHandler(control_MouseEnter);
			rBtnFifoFillSyncAddress.MouseLeave += new EventHandler(control_MouseLeave);
			label4.Anchor = AnchorStyles.Left;
			label4.AutoSize = true;
			label4.Location = new Point(3, 0x35);
			label4.Name = "label4";
			label4.Size = new Size(0x5b, 13);
			label4.TabIndex = 5;
			label4.Text = "FIFO fill condition:";
			label4.TextAlign = ContentAlignment.MiddleLeft;
			pnlSync.Anchor = AnchorStyles.Left;
			pnlSync.AutoSize = true;
			pnlSync.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			pnlSync.Controls.Add(rBtnSyncOff);
			pnlSync.Controls.Add(rBtnSyncOn);
			pnlSync.Location = new Point(0xa3, 0x1a);
			pnlSync.Margin = new Padding(3, 2, 3, 2);
			pnlSync.Name = "pnlSync";
			pnlSync.Size = new Size(0x62, 20);
			pnlSync.TabIndex = 4;
			pnlSync.MouseEnter += new EventHandler(control_MouseEnter);
			pnlSync.MouseLeave += new EventHandler(control_MouseLeave);
			rBtnSyncOff.AutoSize = true;
			rBtnSyncOff.Location = new Point(50, 3);
			rBtnSyncOff.Margin = new Padding(3, 0, 3, 0);
			rBtnSyncOff.Name = "rBtnSyncOff";
			rBtnSyncOff.Size = new Size(0x2d, 0x11);
			rBtnSyncOff.TabIndex = 1;
			rBtnSyncOff.Text = "OFF";
			rBtnSyncOff.UseVisualStyleBackColor = true;
			rBtnSyncOff.CheckedChanged += new EventHandler(rBtnSyncOn_CheckedChanged);
			rBtnSyncOff.MouseEnter += new EventHandler(control_MouseEnter);
			rBtnSyncOff.MouseLeave += new EventHandler(control_MouseLeave);
			rBtnSyncOn.AutoSize = true;
			rBtnSyncOn.Checked = true;
			rBtnSyncOn.Location = new Point(3, 3);
			rBtnSyncOn.Margin = new Padding(3, 0, 3, 0);
			rBtnSyncOn.Name = "rBtnSyncOn";
			rBtnSyncOn.Size = new Size(0x29, 0x11);
			rBtnSyncOn.TabIndex = 0;
			rBtnSyncOn.TabStop = true;
			rBtnSyncOn.Text = "ON";
			rBtnSyncOn.UseVisualStyleBackColor = true;
			rBtnSyncOn.CheckedChanged += new EventHandler(rBtnSyncOn_CheckedChanged);
			rBtnSyncOn.MouseEnter += new EventHandler(control_MouseEnter);
			rBtnSyncOn.MouseLeave += new EventHandler(control_MouseLeave);
			label3.Anchor = AnchorStyles.Left;
			label3.AutoSize = true;
			label3.Location = new Point(3, 0x1d);
			label3.Name = "label3";
			label3.Size = new Size(60, 13);
			label3.TabIndex = 3;
			label3.Text = "Sync word:";
			label3.TextAlign = ContentAlignment.MiddleLeft;
			label9.Anchor = AnchorStyles.Left;
			label9.AutoSize = true;
			label9.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			label9.Location = new Point(3, 0x7d);
			label9.Name = "label9";
			label9.Size = new Size(0x59, 13);
			label9.TabIndex = 13;
			label9.Text = "Sync word value:";
			label9.TextAlign = ContentAlignment.MiddleLeft;
			pnlCrcAutoClear.Anchor = AnchorStyles.Left;
			pnlCrcAutoClear.AutoSize = true;
			pnlCrcAutoClear.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			pnlCrcAutoClear.Controls.Add(rBtnCrcAutoClearOff);
			pnlCrcAutoClear.Controls.Add(rBtnCrcAutoClearOn);
			pnlCrcAutoClear.Location = new Point(0x81, 0x92);
			pnlCrcAutoClear.Margin = new Padding(3, 2, 3, 2);
			pnlCrcAutoClear.Name = "pnlCrcAutoClear";
			pnlCrcAutoClear.Size = new Size(0x66, 20);
			pnlCrcAutoClear.TabIndex = 11;
			pnlCrcAutoClear.MouseEnter += new EventHandler(control_MouseEnter);
			pnlCrcAutoClear.MouseLeave += new EventHandler(control_MouseLeave);
			rBtnCrcAutoClearOff.AutoSize = true;
			rBtnCrcAutoClearOff.Location = new Point(0x36, 3);
			rBtnCrcAutoClearOff.Margin = new Padding(3, 0, 3, 0);
			rBtnCrcAutoClearOff.Name = "rBtnCrcAutoClearOff";
			rBtnCrcAutoClearOff.Size = new Size(0x2d, 0x11);
			rBtnCrcAutoClearOff.TabIndex = 1;
			rBtnCrcAutoClearOff.Text = "OFF";
			rBtnCrcAutoClearOff.UseVisualStyleBackColor = true;
			rBtnCrcAutoClearOff.CheckedChanged += new EventHandler(rBtnCrcAutoClearOff_CheckedChanged);
			rBtnCrcAutoClearOff.MouseEnter += new EventHandler(control_MouseEnter);
			rBtnCrcAutoClearOff.MouseLeave += new EventHandler(control_MouseLeave);
			rBtnCrcAutoClearOn.AutoSize = true;
			rBtnCrcAutoClearOn.Checked = true;
			rBtnCrcAutoClearOn.Location = new Point(3, 3);
			rBtnCrcAutoClearOn.Margin = new Padding(3, 0, 3, 0);
			rBtnCrcAutoClearOn.Name = "rBtnCrcAutoClearOn";
			rBtnCrcAutoClearOn.Size = new Size(0x29, 0x11);
			rBtnCrcAutoClearOn.TabIndex = 0;
			rBtnCrcAutoClearOn.TabStop = true;
			rBtnCrcAutoClearOn.Text = "ON";
			rBtnCrcAutoClearOn.UseVisualStyleBackColor = true;
			rBtnCrcAutoClearOn.CheckedChanged += new EventHandler(rBtnCrcAutoClearOn_CheckedChanged);
			rBtnCrcAutoClearOn.MouseEnter += new EventHandler(control_MouseEnter);
			rBtnCrcAutoClearOn.MouseLeave += new EventHandler(control_MouseLeave);
			label23.Anchor = AnchorStyles.Left;
			label23.AutoSize = true;
			label23.Location = new Point(3, 0x95);
			label23.Name = "label23";
			label23.Size = new Size(0x52, 13);
			label23.TabIndex = 10;
			label23.Text = "CRC auto clear:";
			label23.TextAlign = ContentAlignment.MiddleLeft;
			pnlCrcCalculation.Anchor = AnchorStyles.Left;
			pnlCrcCalculation.AutoSize = true;
			pnlCrcCalculation.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			pnlCrcCalculation.Controls.Add(rBtnCrcOff);
			pnlCrcCalculation.Controls.Add(rBtnCrcOn);
			pnlCrcCalculation.Location = new Point(0x81, 0x7a);
			pnlCrcCalculation.Margin = new Padding(3, 2, 3, 2);
			pnlCrcCalculation.Name = "pnlCrcCalculation";
			pnlCrcCalculation.Size = new Size(0x66, 20);
			pnlCrcCalculation.TabIndex = 9;
			pnlCrcCalculation.MouseEnter += new EventHandler(control_MouseEnter);
			pnlCrcCalculation.MouseLeave += new EventHandler(control_MouseLeave);
			rBtnCrcOff.AutoSize = true;
			rBtnCrcOff.Location = new Point(0x36, 3);
			rBtnCrcOff.Margin = new Padding(3, 0, 3, 0);
			rBtnCrcOff.Name = "rBtnCrcOff";
			rBtnCrcOff.Size = new Size(0x2d, 0x11);
			rBtnCrcOff.TabIndex = 1;
			rBtnCrcOff.Text = "OFF";
			rBtnCrcOff.UseVisualStyleBackColor = true;
			rBtnCrcOff.CheckedChanged += new EventHandler(rBtnCrcOff_CheckedChanged);
			rBtnCrcOff.MouseEnter += new EventHandler(control_MouseEnter);
			rBtnCrcOff.MouseLeave += new EventHandler(control_MouseLeave);
			rBtnCrcOn.AutoSize = true;
			rBtnCrcOn.Checked = true;
			rBtnCrcOn.Location = new Point(3, 3);
			rBtnCrcOn.Margin = new Padding(3, 0, 3, 0);
			rBtnCrcOn.Name = "rBtnCrcOn";
			rBtnCrcOn.Size = new Size(0x29, 0x11);
			rBtnCrcOn.TabIndex = 0;
			rBtnCrcOn.TabStop = true;
			rBtnCrcOn.Text = "ON";
			rBtnCrcOn.UseVisualStyleBackColor = true;
			rBtnCrcOn.CheckedChanged += new EventHandler(rBtnCrcOn_CheckedChanged);
			rBtnCrcOn.MouseEnter += new EventHandler(control_MouseEnter);
			rBtnCrcOn.MouseLeave += new EventHandler(control_MouseLeave);
			label22.Anchor = AnchorStyles.Left;
			label22.AutoSize = true;
			label22.Location = new Point(3, 0x7d);
			label22.Name = "label22";
			label22.Size = new Size(0x56, 13);
			label22.TabIndex = 8;
			label22.Text = "CRC calculation:";
			label22.TextAlign = ContentAlignment.MiddleLeft;
			pnlTxStart.Anchor = AnchorStyles.Left;
			pnlTxStart.AutoSize = true;
			pnlTxStart.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			pnlTxStart.Controls.Add(rBtnTxStartFifoNotEmpty);
			pnlTxStart.Controls.Add(rBtnTxStartFifoLevel);
			pnlTxStart.Location = new Point(0x81, 0xde);
			pnlTxStart.Margin = new Padding(3, 3, 3, 2);
			pnlTxStart.Name = "pnlTxStart";
			pnlTxStart.Size = new Size(0xa8, 20);
			pnlTxStart.TabIndex = 0x11;
			pnlTxStart.MouseEnter += new EventHandler(control_MouseEnter);
			pnlTxStart.MouseLeave += new EventHandler(control_MouseLeave);
			rBtnTxStartFifoNotEmpty.AutoSize = true;
			rBtnTxStartFifoNotEmpty.Checked = true;
			rBtnTxStartFifoNotEmpty.Location = new Point(0x4d, 3);
			rBtnTxStartFifoNotEmpty.Margin = new Padding(3, 0, 3, 0);
			rBtnTxStartFifoNotEmpty.Name = "rBtnTxStartFifoNotEmpty";
			rBtnTxStartFifoNotEmpty.Size = new Size(0x58, 0x11);
			rBtnTxStartFifoNotEmpty.TabIndex = 1;
			rBtnTxStartFifoNotEmpty.TabStop = true;
			rBtnTxStartFifoNotEmpty.Text = "FifoNotEmpty";
			rBtnTxStartFifoNotEmpty.UseVisualStyleBackColor = true;
			rBtnTxStartFifoNotEmpty.CheckedChanged += new EventHandler(rBtnTxStartFifoNotEmpty_CheckedChanged);
			rBtnTxStartFifoNotEmpty.MouseEnter += new EventHandler(control_MouseEnter);
			rBtnTxStartFifoNotEmpty.MouseLeave += new EventHandler(control_MouseLeave);
			rBtnTxStartFifoLevel.AutoSize = true;
			rBtnTxStartFifoLevel.Location = new Point(3, 3);
			rBtnTxStartFifoLevel.Margin = new Padding(3, 0, 3, 0);
			rBtnTxStartFifoLevel.Name = "rBtnTxStartFifoLevel";
			rBtnTxStartFifoLevel.Size = new Size(0x44, 0x11);
			rBtnTxStartFifoLevel.TabIndex = 0;
			rBtnTxStartFifoLevel.Text = "FifoLevel";
			rBtnTxStartFifoLevel.UseVisualStyleBackColor = true;
			rBtnTxStartFifoLevel.CheckedChanged += new EventHandler(rBtnTxStartFifoLevel_CheckedChanged);
			rBtnTxStartFifoLevel.MouseEnter += new EventHandler(control_MouseEnter);
			rBtnTxStartFifoLevel.MouseLeave += new EventHandler(control_MouseLeave);
			pnlAddressFiltering.Anchor = AnchorStyles.Left;
			pnlAddressFiltering.AutoSize = true;
			pnlAddressFiltering.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			pnlAddressFiltering.Controls.Add(rBtnAddressFilteringNodeBroadcast);
			pnlAddressFiltering.Controls.Add(rBtnAddressFilteringNode);
			pnlAddressFiltering.Controls.Add(rBtnAddressFilteringOff);
			pnlAddressFiltering.Location = new Point(0x81, 0x1a);
			pnlAddressFiltering.Margin = new Padding(3, 2, 3, 2);
			pnlAddressFiltering.Name = "pnlAddressFiltering";
			pnlAddressFiltering.Size = new Size(0xe4, 20);
			pnlAddressFiltering.TabIndex = 3;
			pnlAddressFiltering.MouseEnter += new EventHandler(control_MouseEnter);
			pnlAddressFiltering.MouseLeave += new EventHandler(control_MouseLeave);
			rBtnAddressFilteringNodeBroadcast.AutoSize = true;
			rBtnAddressFilteringNodeBroadcast.Location = new Point(0x6f, 3);
			rBtnAddressFilteringNodeBroadcast.Margin = new Padding(3, 0, 3, 0);
			rBtnAddressFilteringNodeBroadcast.Name = "rBtnAddressFilteringNodeBroadcast";
			rBtnAddressFilteringNodeBroadcast.Size = new Size(0x72, 0x11);
			rBtnAddressFilteringNodeBroadcast.TabIndex = 2;
			rBtnAddressFilteringNodeBroadcast.Text = "Node or Broadcast";
			rBtnAddressFilteringNodeBroadcast.UseVisualStyleBackColor = true;
			rBtnAddressFilteringNodeBroadcast.CheckedChanged += new EventHandler(rBtnAddressFilteringNodeBroadcast_CheckedChanged);
			rBtnAddressFilteringNodeBroadcast.MouseEnter += new EventHandler(control_MouseEnter);
			rBtnAddressFilteringNodeBroadcast.MouseLeave += new EventHandler(control_MouseLeave);
			rBtnAddressFilteringNode.AutoSize = true;
			rBtnAddressFilteringNode.Location = new Point(0x36, 3);
			rBtnAddressFilteringNode.Margin = new Padding(3, 0, 3, 0);
			rBtnAddressFilteringNode.Name = "rBtnAddressFilteringNode";
			rBtnAddressFilteringNode.Size = new Size(0x33, 0x11);
			rBtnAddressFilteringNode.TabIndex = 1;
			rBtnAddressFilteringNode.Text = "Node";
			rBtnAddressFilteringNode.UseVisualStyleBackColor = true;
			rBtnAddressFilteringNode.CheckedChanged += new EventHandler(rBtnAddressFilteringNode_CheckedChanged);
			rBtnAddressFilteringNode.MouseEnter += new EventHandler(control_MouseEnter);
			rBtnAddressFilteringNode.MouseLeave += new EventHandler(control_MouseLeave);
			rBtnAddressFilteringOff.AutoSize = true;
			rBtnAddressFilteringOff.Checked = true;
			rBtnAddressFilteringOff.Location = new Point(3, 3);
			rBtnAddressFilteringOff.Margin = new Padding(3, 0, 3, 0);
			rBtnAddressFilteringOff.Name = "rBtnAddressFilteringOff";
			rBtnAddressFilteringOff.Size = new Size(0x2d, 0x11);
			rBtnAddressFilteringOff.TabIndex = 0;
			rBtnAddressFilteringOff.TabStop = true;
			rBtnAddressFilteringOff.Text = "OFF";
			rBtnAddressFilteringOff.UseVisualStyleBackColor = true;
			rBtnAddressFilteringOff.CheckedChanged += new EventHandler(rBtnAddressFilteringOff_CheckedChanged);
			rBtnAddressFilteringOff.MouseEnter += new EventHandler(control_MouseEnter);
			rBtnAddressFilteringOff.MouseLeave += new EventHandler(control_MouseLeave);
			lblNodeAddress.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			lblNodeAddress.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			lblNodeAddress.Location = new Point(0x41, 0);
			lblNodeAddress.Name = "lblNodeAddress";
			lblNodeAddress.Size = new Size(0x3b, 20);
			lblNodeAddress.TabIndex = 1;
			lblNodeAddress.Text = "0x00";
			lblNodeAddress.TextAlign = ContentAlignment.MiddleCenter;
			lblNodeAddress.MouseEnter += new EventHandler(control_MouseEnter);
			lblNodeAddress.MouseLeave += new EventHandler(control_MouseLeave);
			lblPayloadLength.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			lblPayloadLength.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			lblPayloadLength.Location = new Point(0x41, 0);
			lblPayloadLength.Name = "lblPayloadLength";
			lblPayloadLength.Size = new Size(0x3b, 20);
			lblPayloadLength.TabIndex = 1;
			lblPayloadLength.Text = "0x00";
			lblPayloadLength.TextAlign = ContentAlignment.MiddleCenter;
			lblPayloadLength.MouseEnter += new EventHandler(control_MouseEnter);
			lblPayloadLength.MouseLeave += new EventHandler(control_MouseLeave);
			lblBroadcastAddress.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			lblBroadcastAddress.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			lblBroadcastAddress.Location = new Point(0x41, 0);
			lblBroadcastAddress.Name = "lblBroadcastAddress";
			lblBroadcastAddress.Size = new Size(0x3b, 20);
			lblBroadcastAddress.TabIndex = 1;
			lblBroadcastAddress.Text = "0x00";
			lblBroadcastAddress.TextAlign = ContentAlignment.MiddleCenter;
			lblBroadcastAddress.MouseEnter += new EventHandler(control_MouseEnter);
			lblBroadcastAddress.MouseLeave += new EventHandler(control_MouseLeave);
			pnlPacketFormat.Anchor = AnchorStyles.Left;
			pnlPacketFormat.AutoSize = true;
			pnlPacketFormat.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			pnlPacketFormat.Controls.Add(rBtnPacketFormatFixed);
			pnlPacketFormat.Controls.Add(rBtnPacketFormatVariable);
			pnlPacketFormat.Location = new Point(0xa3, 0x92);
			pnlPacketFormat.Margin = new Padding(3, 2, 3, 2);
			pnlPacketFormat.Name = "pnlPacketFormat";
			pnlPacketFormat.Size = new Size(0x7d, 20);
			pnlPacketFormat.TabIndex = 0x10;
			pnlPacketFormat.MouseEnter += new EventHandler(control_MouseEnter);
			pnlPacketFormat.MouseLeave += new EventHandler(control_MouseLeave);
			rBtnPacketFormatFixed.AutoSize = true;
			rBtnPacketFormatFixed.Location = new Point(0x48, 3);
			rBtnPacketFormatFixed.Margin = new Padding(3, 0, 3, 0);
			rBtnPacketFormatFixed.Name = "rBtnPacketFormatFixed";
			rBtnPacketFormatFixed.Size = new Size(50, 0x11);
			rBtnPacketFormatFixed.TabIndex = 1;
			rBtnPacketFormatFixed.Text = "Fixed";
			rBtnPacketFormatFixed.UseVisualStyleBackColor = true;
			rBtnPacketFormatFixed.CheckedChanged += new EventHandler(rBtnPacketFormat_CheckedChanged);
			rBtnPacketFormatFixed.MouseEnter += new EventHandler(control_MouseEnter);
			rBtnPacketFormatFixed.MouseLeave += new EventHandler(control_MouseLeave);
			rBtnPacketFormatVariable.AutoSize = true;
			rBtnPacketFormatVariable.Checked = true;
			rBtnPacketFormatVariable.Location = new Point(3, 3);
			rBtnPacketFormatVariable.Margin = new Padding(3, 0, 3, 0);
			rBtnPacketFormatVariable.Name = "rBtnPacketFormatVariable";
			rBtnPacketFormatVariable.Size = new Size(0x3f, 0x11);
			rBtnPacketFormatVariable.TabIndex = 0;
			rBtnPacketFormatVariable.TabStop = true;
			rBtnPacketFormatVariable.Text = "Variable";
			rBtnPacketFormatVariable.UseVisualStyleBackColor = true;
			rBtnPacketFormatVariable.CheckedChanged += new EventHandler(rBtnPacketFormat_CheckedChanged);
			rBtnPacketFormatVariable.MouseEnter += new EventHandler(control_MouseEnter);
			rBtnPacketFormatVariable.MouseLeave += new EventHandler(control_MouseLeave);
			tableLayoutPanel1.AutoSize = true;
			tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			tableLayoutPanel1.ColumnCount = 3;
			tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160f));
			tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
			tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
			tableLayoutPanel1.Controls.Add(pnlPayloadLength, 1, 7);
			tableLayoutPanel1.Controls.Add(label1, 0, 0);
			tableLayoutPanel1.Controls.Add(pnlPacketFormat, 1, 6);
			tableLayoutPanel1.Controls.Add(label3, 0, 1);
			tableLayoutPanel1.Controls.Add(label4, 0, 2);
			tableLayoutPanel1.Controls.Add(label5, 0, 3);
			tableLayoutPanel1.Controls.Add(label7, 0, 4);
			tableLayoutPanel1.Controls.Add(label9, 0, 5);
			tableLayoutPanel1.Controls.Add(label10, 0, 6);
			tableLayoutPanel1.Controls.Add(label11, 0, 7);
			tableLayoutPanel1.Controls.Add(pnlFifoFillCondition, 1, 2);
			tableLayoutPanel1.Controls.Add(pnlSync, 1, 1);
			tableLayoutPanel1.Controls.Add(cBoxEnterCondition, 1, 8);
			tableLayoutPanel1.Controls.Add(cBoxExitCondition, 1, 9);
			tableLayoutPanel1.Controls.Add(tBoxSyncValue, 1, 5);
			tableLayoutPanel1.Controls.Add(label12, 2, 7);
			tableLayoutPanel1.Controls.Add(label14, 0, 8);
			tableLayoutPanel1.Controls.Add(label15, 0, 9);
			tableLayoutPanel1.Controls.Add(label16, 0, 10);
			tableLayoutPanel1.Controls.Add(cBoxIntermediateMode, 1, 10);
			tableLayoutPanel1.Controls.Add(nudPreambleSize, 1, 0);
			tableLayoutPanel1.Controls.Add(label2, 2, 0);
			tableLayoutPanel1.Controls.Add(nudSyncSize, 1, 3);
			tableLayoutPanel1.Controls.Add(label6, 2, 3);
			tableLayoutPanel1.Controls.Add(nudSyncTol, 1, 4);
			tableLayoutPanel1.Controls.Add(label8, 2, 4);
			tableLayoutPanel1.Location = new Point(3, 3);
			tableLayoutPanel1.Name = "tableLayoutPanel1";
			tableLayoutPanel1.RowCount = 11;
			tableLayoutPanel1.RowStyles.Add(new RowStyle());
			tableLayoutPanel1.RowStyles.Add(new RowStyle());
			tableLayoutPanel1.RowStyles.Add(new RowStyle());
			tableLayoutPanel1.RowStyles.Add(new RowStyle());
			tableLayoutPanel1.RowStyles.Add(new RowStyle());
			tableLayoutPanel1.RowStyles.Add(new RowStyle());
			tableLayoutPanel1.RowStyles.Add(new RowStyle());
			tableLayoutPanel1.RowStyles.Add(new RowStyle());
			tableLayoutPanel1.RowStyles.Add(new RowStyle());
			tableLayoutPanel1.RowStyles.Add(new RowStyle());
			tableLayoutPanel1.RowStyles.Add(new RowStyle());
			tableLayoutPanel1.Size = new Size(0x178, 0x10b);
			tableLayoutPanel1.TabIndex = 0;
			pnlPayloadLength.Anchor = AnchorStyles.Left;
			pnlPayloadLength.AutoSize = true;
			pnlPayloadLength.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			pnlPayloadLength.Controls.Add(lblPayloadLength);
			pnlPayloadLength.Controls.Add(nudPayloadLength);
			pnlPayloadLength.Location = new Point(0xa3, 170);
			pnlPayloadLength.Margin = new Padding(3, 2, 3, 2);
			pnlPayloadLength.Name = "pnlPayloadLength";
			pnlPayloadLength.Size = new Size(0x7f, 20);
			pnlPayloadLength.TabIndex = 0x12;
			pnlPayloadLength.MouseEnter += new EventHandler(control_MouseEnter);
			pnlPayloadLength.MouseLeave += new EventHandler(control_MouseLeave);
			nudPayloadLength.Location = new Point(3, 0);
			nudPayloadLength.Margin = new Padding(3, 0, 3, 0);
			int[] numArray3 = new int[4];
			numArray3[0] = 0x42;
			nudPayloadLength.Maximum = new decimal(numArray3);
			nudPayloadLength.Name = "nudPayloadLength";
			nudPayloadLength.Size = new Size(0x3b, 20);
			nudPayloadLength.TabIndex = 0;
			int[] numArray4 = new int[4];
			numArray4[0] = 0x42;
			nudPayloadLength.Value = new decimal(numArray4);
			nudPayloadLength.MouseEnter += new EventHandler(control_MouseEnter);
			nudPayloadLength.MouseLeave += new EventHandler(control_MouseLeave);
			nudPayloadLength.ValueChanged += new EventHandler(nudPayloadLength_ValueChanged);
			nudSyncSize.Anchor = AnchorStyles.Left;
			nudSyncSize.Location = new Point(0xa3, 0x4a);
			nudSyncSize.Margin = new Padding(3, 2, 3, 2);
			int[] numArray5 = new int[4];
			numArray5[0] = 8;
			nudSyncSize.Maximum = new decimal(numArray5);
			int[] numArray6 = new int[4];
			numArray6[0] = 1;
			nudSyncSize.Minimum = new decimal(numArray6);
			nudSyncSize.Name = "nudSyncSize";
			nudSyncSize.Size = new Size(0x3b, 20);
			nudSyncSize.TabIndex = 8;
			int[] numArray7 = new int[4];
			numArray7[0] = 4;
			nudSyncSize.Value = new decimal(numArray7);
			nudSyncSize.MouseEnter += new EventHandler(control_MouseEnter);
			nudSyncSize.MouseLeave += new EventHandler(control_MouseLeave);
			nudSyncSize.ValueChanged += new EventHandler(nudSyncSize_ValueChanged);
			nudSyncTol.Anchor = AnchorStyles.Left;
			nudSyncTol.Location = new Point(0xa3, 0x62);
			nudSyncTol.Margin = new Padding(3, 2, 3, 2);
			int[] numArray8 = new int[4];
			numArray8[0] = 7;
			nudSyncTol.Maximum = new decimal(numArray8);
			nudSyncTol.Name = "nudSyncTol";
			nudSyncTol.Size = new Size(0x3b, 20);
			nudSyncTol.TabIndex = 11;
			nudSyncTol.MouseEnter += new EventHandler(control_MouseEnter);
			nudSyncTol.MouseLeave += new EventHandler(control_MouseLeave);
			nudSyncTol.ValueChanged += new EventHandler(nudSyncTol_ValueChanged);
			pnlNodeAddress.Anchor = AnchorStyles.Left;
			pnlNodeAddress.AutoSize = true;
			pnlNodeAddress.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			pnlNodeAddress.Controls.Add(nudNodeAddress);
			pnlNodeAddress.Controls.Add(lblNodeAddress);
			pnlNodeAddress.Location = new Point(0x81, 50);
			pnlNodeAddress.Margin = new Padding(3, 2, 3, 2);
			pnlNodeAddress.Name = "pnlNodeAddress";
			pnlNodeAddress.Size = new Size(0x7f, 20);
			pnlNodeAddress.TabIndex = 0x3b;
			pnlNodeAddress.MouseEnter += new EventHandler(control_MouseEnter);
			pnlNodeAddress.MouseLeave += new EventHandler(control_MouseLeave);
			nudNodeAddress.Location = new Point(0, 0);
			nudNodeAddress.Margin = new Padding(3, 0, 3, 0);
			int[] numArray9 = new int[4];
			numArray9[0] = 0xff;
			nudNodeAddress.Maximum = new decimal(numArray9);
			nudNodeAddress.Name = "nudNodeAddress";
			nudNodeAddress.Size = new Size(0x3b, 20);
			nudNodeAddress.TabIndex = 0;
			nudNodeAddress.MouseEnter += new EventHandler(control_MouseEnter);
			nudNodeAddress.MouseLeave += new EventHandler(control_MouseLeave);
			nudNodeAddress.ValueChanged += new EventHandler(nudNodeAddress_ValueChanged);
			pnlBroadcastAddress.Anchor = AnchorStyles.Left;
			pnlBroadcastAddress.AutoSize = true;
			pnlBroadcastAddress.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			pnlBroadcastAddress.Controls.Add(nudBroadcastAddress);
			pnlBroadcastAddress.Controls.Add(lblBroadcastAddress);
			pnlBroadcastAddress.Location = new Point(0x81, 0x4a);
			pnlBroadcastAddress.Margin = new Padding(3, 2, 3, 2);
			pnlBroadcastAddress.Name = "pnlBroadcastAddress";
			pnlBroadcastAddress.Size = new Size(0x7f, 20);
			pnlBroadcastAddress.TabIndex = 60;
			pnlBroadcastAddress.MouseEnter += new EventHandler(control_MouseEnter);
			pnlBroadcastAddress.MouseLeave += new EventHandler(control_MouseLeave);
			nudBroadcastAddress.Location = new Point(0, 0);
			nudBroadcastAddress.Margin = new Padding(3, 0, 3, 0);
			int[] numArray10 = new int[4];
			numArray10[0] = 0xff;
			nudBroadcastAddress.Maximum = new decimal(numArray10);
			nudBroadcastAddress.Name = "nudBroadcastAddress";
			nudBroadcastAddress.Size = new Size(0x3b, 20);
			nudBroadcastAddress.TabIndex = 0;
			nudBroadcastAddress.MouseEnter += new EventHandler(control_MouseEnter);
			nudBroadcastAddress.MouseLeave += new EventHandler(control_MouseLeave);
			nudBroadcastAddress.ValueChanged += new EventHandler(nudBroadcastAddress_ValueChanged);
			tableLayoutPanel2.AutoSize = true;
			tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			tableLayoutPanel2.ColumnCount = 3;
			tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
			tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
			tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
			tableLayoutPanel2.Controls.Add(label17, 0, 0);
			tableLayoutPanel2.Controls.Add(pnlBroadcastAddress, 1, 3);
			tableLayoutPanel2.Controls.Add(lblInterPacketRxDelayUnit, 2, 11);
			tableLayoutPanel2.Controls.Add(pnlTxStart, 1, 9);
			tableLayoutPanel2.Controls.Add(label18, 0, 1);
			tableLayoutPanel2.Controls.Add(pnlAesEncryption, 1, 7);
			tableLayoutPanel2.Controls.Add(nudFifoThreshold, 1, 10);
			tableLayoutPanel2.Controls.Add(pnlCrcAutoClear, 1, 6);
			tableLayoutPanel2.Controls.Add(pnlNodeAddress, 1, 2);
			tableLayoutPanel2.Controls.Add(pnlCrcCalculation, 1, 5);
			tableLayoutPanel2.Controls.Add(tBoxAesKey, 1, 8);
			tableLayoutPanel2.Controls.Add(label19, 0, 2);
			tableLayoutPanel2.Controls.Add(pnlDcFree, 1, 4);
			tableLayoutPanel2.Controls.Add(label20, 0, 3);
			tableLayoutPanel2.Controls.Add(pnlAddressFiltering, 1, 1);
			tableLayoutPanel2.Controls.Add(label21, 0, 4);
			tableLayoutPanel2.Controls.Add(label22, 0, 5);
			tableLayoutPanel2.Controls.Add(label23, 0, 6);
			tableLayoutPanel2.Controls.Add(label24, 0, 7);
			tableLayoutPanel2.Controls.Add(label25, 0, 8);
			tableLayoutPanel2.Controls.Add(label26, 0, 9);
			tableLayoutPanel2.Controls.Add(label27, 0, 10);
			tableLayoutPanel2.Controls.Add(label28, 0, 11);
			tableLayoutPanel2.Controls.Add(pnlAddressInPayload, 1, 0);
			tableLayoutPanel2.Controls.Add(cBoxInterPacketRxDelay, 1, 11);
			tableLayoutPanel2.Location = new Point(0x183, 3);
			tableLayoutPanel2.Name = "tableLayoutPanel2";
			tableLayoutPanel2.RowCount = 12;
			tableLayoutPanel2.RowStyles.Add(new RowStyle());
			tableLayoutPanel2.RowStyles.Add(new RowStyle());
			tableLayoutPanel2.RowStyles.Add(new RowStyle());
			tableLayoutPanel2.RowStyles.Add(new RowStyle());
			tableLayoutPanel2.RowStyles.Add(new RowStyle());
			tableLayoutPanel2.RowStyles.Add(new RowStyle());
			tableLayoutPanel2.RowStyles.Add(new RowStyle());
			tableLayoutPanel2.RowStyles.Add(new RowStyle());
			tableLayoutPanel2.RowStyles.Add(new RowStyle());
			tableLayoutPanel2.RowStyles.Add(new RowStyle());
			tableLayoutPanel2.RowStyles.Add(new RowStyle());
			tableLayoutPanel2.RowStyles.Add(new RowStyle());
			tableLayoutPanel2.Size = new Size(0x199, 0x126);
			tableLayoutPanel2.TabIndex = 1;
			nudFifoThreshold.Anchor = AnchorStyles.Left;
			nudFifoThreshold.Location = new Point(0x81, 0xf7);
			nudFifoThreshold.Margin = new Padding(3, 3, 3, 2);
			int[] numArray11 = new int[4];
			numArray11[0] = 0x80;
			nudFifoThreshold.Maximum = new decimal(numArray11);
			nudFifoThreshold.Name = "nudFifoThreshold";
			nudFifoThreshold.Size = new Size(0x3b, 20);
			nudFifoThreshold.TabIndex = 0x13;
			nudFifoThreshold.MouseEnter += new EventHandler(control_MouseEnter);
			nudFifoThreshold.MouseLeave += new EventHandler(control_MouseLeave);
			nudFifoThreshold.ValueChanged += new EventHandler(nudFifoThreshold_ValueChanged);
			cBoxInterPacketRxDelay.Anchor = AnchorStyles.Left;
			cBoxInterPacketRxDelay.DropDownStyle = ComboBoxStyle.DropDownList;
			cBoxInterPacketRxDelay.FormatString = "###0.000";
			cBoxInterPacketRxDelay.FormattingEnabled = true;
			cBoxInterPacketRxDelay.Items.AddRange(new object[] { "0.208" });
			cBoxInterPacketRxDelay.Location = new Point(0x81, 0x10f);
			cBoxInterPacketRxDelay.Margin = new Padding(3, 2, 3, 2);
			cBoxInterPacketRxDelay.Name = "cBoxInterPacketRxDelay";
			cBoxInterPacketRxDelay.Size = new Size(0x5d, 0x15);
			cBoxInterPacketRxDelay.TabIndex = 0x15;
			cBoxInterPacketRxDelay.SelectedIndexChanged += new EventHandler(nudInterPacketRxDelay_SelectedIndexChanged);
			cBoxInterPacketRxDelay.MouseEnter += new EventHandler(control_MouseEnter);
			cBoxInterPacketRxDelay.MouseLeave += new EventHandler(control_MouseLeave);
			gBoxDeviceStatus.Controls.Add(lblOperatingMode);
			gBoxDeviceStatus.Controls.Add(label37);
			gBoxDeviceStatus.Controls.Add(lblBitSynchroniser);
			gBoxDeviceStatus.Controls.Add(lblDataMode);
			gBoxDeviceStatus.Controls.Add(label38);
			gBoxDeviceStatus.Controls.Add(label39);
			gBoxDeviceStatus.Location = new Point(0x235, 0x13d);
			gBoxDeviceStatus.Name = "gBoxDeviceStatus";
			gBoxDeviceStatus.Size = new Size(0xe7, 0x4d);
			gBoxDeviceStatus.TabIndex = 3;
			gBoxDeviceStatus.TabStop = false;
			gBoxDeviceStatus.Text = "Device status";
			gBoxDeviceStatus.MouseEnter += new EventHandler(control_MouseEnter);
			gBoxDeviceStatus.MouseLeave += new EventHandler(control_MouseLeave);
			lblOperatingMode.AutoSize = true;
			lblOperatingMode.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
			lblOperatingMode.Location = new Point(0x92, 0x3a);
			lblOperatingMode.Margin = new Padding(3);
			lblOperatingMode.Name = "lblOperatingMode";
			lblOperatingMode.Size = new Size(0x27, 13);
			lblOperatingMode.TabIndex = 5;
			lblOperatingMode.Text = "Sleep";
			lblOperatingMode.TextAlign = ContentAlignment.MiddleLeft;
			label37.AutoSize = true;
			label37.Location = new Point(3, 0x3a);
			label37.Margin = new Padding(3);
			label37.Name = "label37";
			label37.Size = new Size(0x55, 13);
			label37.TabIndex = 4;
			label37.Text = "Operating mode:";
			label37.TextAlign = ContentAlignment.MiddleLeft;
			lblBitSynchroniser.AutoSize = true;
			lblBitSynchroniser.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
			lblBitSynchroniser.Location = new Point(0x92, 20);
			lblBitSynchroniser.Margin = new Padding(3);
			lblBitSynchroniser.Name = "lblBitSynchroniser";
			lblBitSynchroniser.Size = new Size(0x19, 13);
			lblBitSynchroniser.TabIndex = 1;
			lblBitSynchroniser.Text = "ON";
			lblBitSynchroniser.TextAlign = ContentAlignment.MiddleLeft;
			lblDataMode.AutoSize = true;
			lblDataMode.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
			lblDataMode.Location = new Point(0x92, 0x27);
			lblDataMode.Margin = new Padding(3);
			lblDataMode.Name = "lblDataMode";
			lblDataMode.Size = new Size(0x2f, 13);
			lblDataMode.TabIndex = 3;
			lblDataMode.Text = "Packet";
			lblDataMode.TextAlign = ContentAlignment.MiddleLeft;
			label38.AutoSize = true;
			label38.Location = new Point(3, 20);
			label38.Margin = new Padding(3);
			label38.Name = "label38";
			label38.Size = new Size(0x56, 13);
			label38.TabIndex = 0;
			label38.Text = "Bit Synchronizer:";
			label38.TextAlign = ContentAlignment.MiddleLeft;
			label39.AutoSize = true;
			label39.Location = new Point(3, 0x27);
			label39.Margin = new Padding(3);
			label39.Name = "label39";
			label39.Size = new Size(0x3e, 13);
			label39.TabIndex = 2;
			label39.Text = "Data mode:";
			label39.TextAlign = ContentAlignment.MiddleLeft;
			gBoxControl.Controls.Add(tBoxPacketsNb);
			gBoxControl.Controls.Add(cBtnLog);
			gBoxControl.Controls.Add(cBtnPacketHandlerStartStop);
			gBoxControl.Controls.Add(lblPacketsNb);
			gBoxControl.Controls.Add(tBoxPacketsRepeatValue);
			gBoxControl.Controls.Add(lblPacketsRepeatValue);
			gBoxControl.Location = new Point(0x235, 0x18a);
			gBoxControl.Name = "gBoxControl";
			gBoxControl.Size = new Size(0xe7, 0x60);
			gBoxControl.TabIndex = 4;
			gBoxControl.TabStop = false;
			gBoxControl.Text = "Control";
			gBoxControl.MouseEnter += new EventHandler(control_MouseEnter);
			gBoxControl.MouseLeave += new EventHandler(control_MouseLeave);
			tBoxPacketsNb.Location = new Point(0x95, 0x30);
			tBoxPacketsNb.Name = "tBoxPacketsNb";
			tBoxPacketsNb.ReadOnly = true;
			tBoxPacketsNb.Size = new Size(0x4f, 20);
			tBoxPacketsNb.TabIndex = 2;
			tBoxPacketsNb.Text = "0";
			tBoxPacketsNb.TextAlign = HorizontalAlignment.Right;
			cBtnLog.Appearance = Appearance.Button;
			cBtnLog.Location = new Point(0x76, 0x13);
			cBtnLog.Name = "cBtnLog";
			cBtnLog.Size = new Size(0x4b, 0x17);
			cBtnLog.TabIndex = 0;
			cBtnLog.Text = "Log";
			cBtnLog.TextAlign = ContentAlignment.MiddleCenter;
			cBtnLog.UseVisualStyleBackColor = true;
			cBtnLog.CheckedChanged += new EventHandler(cBtnLog_CheckedChanged);
			cBtnPacketHandlerStartStop.Appearance = Appearance.Button;
			cBtnPacketHandlerStartStop.Location = new Point(0x25, 0x13);
			cBtnPacketHandlerStartStop.Name = "cBtnPacketHandlerStartStop";
			cBtnPacketHandlerStartStop.Size = new Size(0x4b, 0x17);
			cBtnPacketHandlerStartStop.TabIndex = 0;
			cBtnPacketHandlerStartStop.Text = "Start";
			cBtnPacketHandlerStartStop.TextAlign = ContentAlignment.MiddleCenter;
			cBtnPacketHandlerStartStop.UseVisualStyleBackColor = true;
			cBtnPacketHandlerStartStop.CheckedChanged += new EventHandler(cBtnPacketHandlerStartStop_CheckedChanged);
			lblPacketsNb.AutoSize = true;
			lblPacketsNb.Location = new Point(3, 0x33);
			lblPacketsNb.Name = "lblPacketsNb";
			lblPacketsNb.Size = new Size(0x40, 13);
			lblPacketsNb.TabIndex = 1;
			lblPacketsNb.Text = "Tx Packets:";
			lblPacketsNb.TextAlign = ContentAlignment.MiddleLeft;
			tBoxPacketsRepeatValue.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
			tBoxPacketsRepeatValue.Location = new Point(0x95, 70);
			tBoxPacketsRepeatValue.Name = "tBoxPacketsRepeatValue";
			tBoxPacketsRepeatValue.Size = new Size(0x4f, 20);
			tBoxPacketsRepeatValue.TabIndex = 4;
			tBoxPacketsRepeatValue.Text = "0";
			tBoxPacketsRepeatValue.TextAlign = HorizontalAlignment.Right;
			lblPacketsRepeatValue.AutoSize = true;
			lblPacketsRepeatValue.Location = new Point(3, 0x49);
			lblPacketsRepeatValue.Name = "lblPacketsRepeatValue";
			lblPacketsRepeatValue.Size = new Size(0x4a, 13);
			lblPacketsRepeatValue.TabIndex = 3;
			lblPacketsRepeatValue.Text = "Repeat value:";
			lblPacketsRepeatValue.TextAlign = ContentAlignment.MiddleLeft;
			gBoxPacket.Controls.Add(imgPacketMessage);
			gBoxPacket.Controls.Add(gBoxMessage);
			gBoxPacket.Controls.Add(tblPacket);
			gBoxPacket.Location = new Point(3, 0x13d);
			gBoxPacket.Margin = new Padding(3, 1, 3, 1);
			gBoxPacket.Name = "gBoxPacket";
			gBoxPacket.Size = new Size(0x22d, 0xac);
			gBoxPacket.TabIndex = 2;
			gBoxPacket.TabStop = false;
			gBoxPacket.Text = "Packet";
			gBoxPacket.MouseEnter += new EventHandler(control_MouseEnter);
			gBoxPacket.MouseLeave += new EventHandler(control_MouseLeave);
			imgPacketMessage.BackColor = Color.Transparent;
			imgPacketMessage.Location = new Point(5, 0x3d);
			imgPacketMessage.Margin = new Padding(0);
			imgPacketMessage.Name = "imgPacketMessage";
			imgPacketMessage.Size = new Size(0x223, 5);
			imgPacketMessage.TabIndex = 1;
			imgPacketMessage.Text = "payloadImg1";
			gBoxMessage.Controls.Add(tblPayloadMessage);
			gBoxMessage.Location = new Point(6, 0x43);
			gBoxMessage.Margin = new Padding(1);
			gBoxMessage.Name = "gBoxMessage";
			gBoxMessage.Size = new Size(0x223, 0x65);
			gBoxMessage.TabIndex = 2;
			gBoxMessage.TabStop = false;
			gBoxMessage.Text = "Message";
			gBoxMessage.MouseEnter += new EventHandler(control_MouseEnter);
			gBoxMessage.MouseLeave += new EventHandler(control_MouseLeave);
			tblPayloadMessage.AutoSize = true;
			tblPayloadMessage.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			tblPayloadMessage.ColumnCount = 2;
			tblPayloadMessage.ColumnStyles.Add(new ColumnStyle());
			tblPayloadMessage.ColumnStyles.Add(new ColumnStyle());
			tblPayloadMessage.Controls.Add(hexBoxPayload, 0, 1);
			tblPayloadMessage.Controls.Add(label36, 1, 0);
			tblPayloadMessage.Controls.Add(label35, 0, 0);
			tblPayloadMessage.Location = new Point(20, 0x13);
			tblPayloadMessage.Name = "tblPayloadMessage";
			tblPayloadMessage.RowCount = 2;
			tblPayloadMessage.RowStyles.Add(new RowStyle());
			tblPayloadMessage.RowStyles.Add(new RowStyle());
			tblPayloadMessage.Size = new Size(0x1fb, 0x4f);
			tblPayloadMessage.TabIndex = 0;
			hexBoxPayload.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
			tblPayloadMessage.SetColumnSpan(hexBoxPayload, 2);
			hexBoxPayload.Font = new Font("Courier New", 8.25f);
			hexBoxPayload.LineInfoDigits = 2;
			hexBoxPayload.LineInfoForeColor = Color.Empty;
			hexBoxPayload.Location = new Point(3, 0x10);
			hexBoxPayload.Name = "hexBoxPayload";
			hexBoxPayload.ShadowSelectionColor = Color.FromArgb(100, 60, 0xbc, 0xff);
			hexBoxPayload.Size = new Size(0x1f5, 60);
			hexBoxPayload.StringViewVisible = true;
			hexBoxPayload.TabIndex = 2;
			hexBoxPayload.UseFixedBytesPerLine = true;
			hexBoxPayload.VScrollBarVisible = true;
			label36.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
			label36.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
			label36.Location = new Point(0x149, 0);
			label36.Name = "label36";
			label36.Size = new Size(0xaf, 13);
			label36.TabIndex = 1;
			label36.Text = "ASCII";
			label36.TextAlign = ContentAlignment.MiddleCenter;
			label35.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
			label35.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
			label35.Location = new Point(3, 0);
			label35.Name = "label35";
			label35.Size = new Size(320, 13);
			label35.TabIndex = 0;
			label35.Text = "HEXADECIMAL";
			label35.TextAlign = ContentAlignment.MiddleCenter;
			tblPacket.AutoSize = true;
			tblPacket.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			tblPacket.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
			tblPacket.ColumnCount = 6;
			tblPacket.ColumnStyles.Add(new ColumnStyle());
			tblPacket.ColumnStyles.Add(new ColumnStyle());
			tblPacket.ColumnStyles.Add(new ColumnStyle());
			tblPacket.ColumnStyles.Add(new ColumnStyle());
			tblPacket.ColumnStyles.Add(new ColumnStyle());
			tblPacket.ColumnStyles.Add(new ColumnStyle());
			tblPacket.Controls.Add(label29, 0, 0);
			tblPacket.Controls.Add(label30, 1, 0);
			tblPacket.Controls.Add(label31, 2, 0);
			tblPacket.Controls.Add(label32, 3, 0);
			tblPacket.Controls.Add(label33, 4, 0);
			tblPacket.Controls.Add(label34, 5, 0);
			tblPacket.Controls.Add(lblPacketPreamble, 0, 1);
			tblPacket.Controls.Add(lblPayload, 4, 1);
			tblPacket.Controls.Add(pnlPacketCrc, 5, 1);
			tblPacket.Controls.Add(pnlPacketAddr, 3, 1);
			tblPacket.Controls.Add(lblPacketLength, 2, 1);
			tblPacket.Controls.Add(lblPacketSyncValue, 1, 1);
			tblPacket.Location = new Point(5, 0x11);
			tblPacket.Margin = new Padding(1);
			tblPacket.Name = "tblPacket";
			tblPacket.RowCount = 2;
			tblPacket.RowStyles.Add(new RowStyle());
			tblPacket.RowStyles.Add(new RowStyle());
			tblPacket.Size = new Size(0x223, 0x2b);
			tblPacket.TabIndex = 0;
			label29.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
			label29.Location = new Point(1, 1);
			label29.Margin = new Padding(0);
			label29.Name = "label29";
			label29.Size = new Size(0x67, 20);
			label29.TabIndex = 0;
			label29.Text = "Preamble";
			label29.TextAlign = ContentAlignment.MiddleCenter;
			label30.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
			label30.Location = new Point(0x6c, 1);
			label30.Margin = new Padding(0);
			label30.Name = "label30";
			label30.Size = new Size(0x98, 20);
			label30.TabIndex = 1;
			label30.Text = "Sync";
			label30.TextAlign = ContentAlignment.MiddleCenter;
			label31.BackColor = Color.LightGray;
			label31.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
			label31.Location = new Point(0x105, 1);
			label31.Margin = new Padding(0);
			label31.Name = "label31";
			label31.Size = new Size(0x3b, 20);
			label31.TabIndex = 2;
			label31.Text = "Length";
			label31.TextAlign = ContentAlignment.MiddleCenter;
			label32.BackColor = Color.LightGray;
			label32.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
			label32.Location = new Point(0x141, 1);
			label32.Margin = new Padding(0);
			label32.Name = "label32";
			label32.Size = new Size(0x57, 20);
			label32.TabIndex = 3;
			label32.Text = "Node Address";
			label32.TextAlign = ContentAlignment.MiddleCenter;
			label33.BackColor = Color.LightGray;
			label33.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
			label33.ForeColor = SystemColors.WindowText;
			label33.Location = new Point(0x199, 1);
			label33.Margin = new Padding(0);
			label33.Name = "label33";
			label33.Size = new Size(0x55, 20);
			label33.TabIndex = 4;
			label33.Text = "Message";
			label33.TextAlign = ContentAlignment.MiddleCenter;
			label34.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
			label34.Location = new Point(0x1ef, 1);
			label34.Margin = new Padding(0);
			label34.Name = "label34";
			label34.Size = new Size(0x33, 20);
			label34.TabIndex = 5;
			label34.Text = "CRC";
			label34.TextAlign = ContentAlignment.MiddleCenter;
			lblPacketPreamble.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			lblPacketPreamble.Location = new Point(1, 0x16);
			lblPacketPreamble.Margin = new Padding(0);
			lblPacketPreamble.Name = "lblPacketPreamble";
			lblPacketPreamble.Size = new Size(0x6a, 20);
			lblPacketPreamble.TabIndex = 6;
			lblPacketPreamble.Text = "55-55-55-55-...-55";
			lblPacketPreamble.TextAlign = ContentAlignment.MiddleCenter;
			lblPayload.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
			lblPayload.Location = new Point(0x199, 0x16);
			lblPayload.Margin = new Padding(0);
			lblPayload.Name = "lblPayload";
			lblPayload.Size = new Size(0x55, 20);
			lblPayload.TabIndex = 9;
			lblPayload.TextAlign = ContentAlignment.MiddleCenter;
			pnlPacketCrc.Controls.Add(ledPacketCrc);
			pnlPacketCrc.Controls.Add(lblPacketCrc);
			pnlPacketCrc.Location = new Point(0x1ef, 0x16);
			pnlPacketCrc.Margin = new Padding(0);
			pnlPacketCrc.Name = "pnlPacketCrc";
			pnlPacketCrc.Size = new Size(0x33, 20);
			pnlPacketCrc.TabIndex = 0x12;
			ledPacketCrc.BackColor = Color.Transparent;
			ledPacketCrc.LedColor = Color.Green;
			ledPacketCrc.LedSize = new Size(11, 11);
			ledPacketCrc.Location = new Point(0x11, 3);
			ledPacketCrc.Name = "ledPacketCrc";
			ledPacketCrc.Size = new Size(15, 15);
			ledPacketCrc.TabIndex = 1;
			ledPacketCrc.Text = "CRC";
			ledPacketCrc.Visible = false;
			lblPacketCrc.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			lblPacketCrc.Location = new Point(0, 0);
			lblPacketCrc.Margin = new Padding(0);
			lblPacketCrc.Name = "lblPacketCrc";
			lblPacketCrc.Size = new Size(0x33, 20);
			lblPacketCrc.TabIndex = 0;
			lblPacketCrc.Text = "XX-XX";
			lblPacketCrc.TextAlign = ContentAlignment.MiddleCenter;
			pnlPacketAddr.Controls.Add(lblPacketAddr);
			pnlPacketAddr.Location = new Point(0x141, 0x16);
			pnlPacketAddr.Margin = new Padding(0);
			pnlPacketAddr.Name = "pnlPacketAddr";
			pnlPacketAddr.Size = new Size(0x57, 20);
			pnlPacketAddr.TabIndex = 11;
			lblPacketAddr.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			lblPacketAddr.Location = new Point(0, 0);
			lblPacketAddr.Margin = new Padding(0);
			lblPacketAddr.Name = "lblPacketAddr";
			lblPacketAddr.Size = new Size(0x57, 20);
			lblPacketAddr.TabIndex = 0;
			lblPacketAddr.Text = "00";
			lblPacketAddr.TextAlign = ContentAlignment.MiddleCenter;
			lblPacketLength.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			lblPacketLength.Location = new Point(0x105, 0x16);
			lblPacketLength.Margin = new Padding(0);
			lblPacketLength.Name = "lblPacketLength";
			lblPacketLength.Size = new Size(0x3b, 20);
			lblPacketLength.TabIndex = 8;
			lblPacketLength.Text = "00";
			lblPacketLength.TextAlign = ContentAlignment.MiddleCenter;
			lblPacketSyncValue.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			lblPacketSyncValue.Location = new Point(0x6c, 0x16);
			lblPacketSyncValue.Margin = new Padding(0);
			lblPacketSyncValue.Name = "lblPacketSyncValue";
			lblPacketSyncValue.Size = new Size(0x98, 20);
			lblPacketSyncValue.TabIndex = 7;
			lblPacketSyncValue.Text = "AA-AA-AA-AA-AA-AA-AA-AA";
			lblPacketSyncValue.TextAlign = ContentAlignment.MiddleCenter;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(gBoxDeviceStatus);
			base.Controls.Add(tableLayoutPanel2);
			base.Controls.Add(tableLayoutPanel1);
			base.Controls.Add(gBoxControl);
			base.Controls.Add(gBoxPacket);
			base.Name = "PacketHandlerView";
			base.Size = new Size(0x31f, 0x1ed);
			((ISupportInitialize)errorProvider).EndInit();
			nudPreambleSize.EndInit();
			pnlAesEncryption.ResumeLayout(false);
			pnlAesEncryption.PerformLayout();
			pnlDcFree.ResumeLayout(false);
			pnlDcFree.PerformLayout();
			pnlAddressInPayload.ResumeLayout(false);
			pnlAddressInPayload.PerformLayout();
			pnlFifoFillCondition.ResumeLayout(false);
			pnlFifoFillCondition.PerformLayout();
			pnlSync.ResumeLayout(false);
			pnlSync.PerformLayout();
			pnlCrcAutoClear.ResumeLayout(false);
			pnlCrcAutoClear.PerformLayout();
			pnlCrcCalculation.ResumeLayout(false);
			pnlCrcCalculation.PerformLayout();
			pnlTxStart.ResumeLayout(false);
			pnlTxStart.PerformLayout();
			pnlAddressFiltering.ResumeLayout(false);
			pnlAddressFiltering.PerformLayout();
			pnlPacketFormat.ResumeLayout(false);
			pnlPacketFormat.PerformLayout();
			tableLayoutPanel1.ResumeLayout(false);
			tableLayoutPanel1.PerformLayout();
			pnlPayloadLength.ResumeLayout(false);
			nudPayloadLength.EndInit();
			nudSyncSize.EndInit();
			nudSyncTol.EndInit();
			pnlNodeAddress.ResumeLayout(false);
			nudNodeAddress.EndInit();
			pnlBroadcastAddress.ResumeLayout(false);
			nudBroadcastAddress.EndInit();
			tableLayoutPanel2.ResumeLayout(false);
			tableLayoutPanel2.PerformLayout();
			nudFifoThreshold.EndInit();
			gBoxDeviceStatus.ResumeLayout(false);
			gBoxDeviceStatus.PerformLayout();
			gBoxControl.ResumeLayout(false);
			gBoxControl.PerformLayout();
			gBoxPacket.ResumeLayout(false);
			gBoxPacket.PerformLayout();
			gBoxMessage.ResumeLayout(false);
			gBoxMessage.PerformLayout();
			tblPayloadMessage.ResumeLayout(false);
			tblPacket.ResumeLayout(false);
			pnlPacketCrc.ResumeLayout(false);
			pnlPacketAddr.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		private void nudBroadcastAddress_ValueChanged(object sender, EventArgs e)
		{
			BroadcastAddress = (byte)nudBroadcastAddress.Value;
			OnBroadcastAddressChanged(BroadcastAddress);
		}

		private void nudFifoThreshold_ValueChanged(object sender, EventArgs e)
		{
			FifoThreshold = (byte)nudFifoThreshold.Value;
			OnFifoThresholdChanged(FifoThreshold);
		}

		private void nudInterPacketRxDelay_SelectedIndexChanged(object sender, EventArgs e)
		{
			InterPacketRxDelay = cBoxInterPacketRxDelay.SelectedIndex;
			OnInterPacketRxDelayChanged(InterPacketRxDelay);
		}

		private void nudNodeAddress_ValueChanged(object sender, EventArgs e)
		{
			NodeAddress = (byte)nudNodeAddress.Value;
			OnNodeAddressChanged(NodeAddress);
		}

		private void nudPayloadLength_ValueChanged(object sender, EventArgs e)
		{
			PayloadLength = (byte)nudPayloadLength.Value;
			OnPayloadLengthChanged(PayloadLength);
		}

		private void nudPreambleSize_ValueChanged(object sender, EventArgs e)
		{
			PreambleSize = (int)nudPreambleSize.Value;
			OnPreambleSizeChanged(PreambleSize);
		}

		private void nudSyncSize_ValueChanged(object sender, EventArgs e)
		{
			SyncSize = (byte)nudSyncSize.Value;
			OnSyncSizeChanged(SyncSize);
		}

		private void nudSyncTol_ValueChanged(object sender, EventArgs e)
		{
			SyncTol = (byte)nudSyncTol.Value;
			OnSyncTolChanged(SyncTol);
		}

		private void OnAddressFilteringChanged(AddressFilteringEnum value)
		{
			if (AddressFilteringChanged != null)
				AddressFilteringChanged(this, new AddressFilteringEventArg(value));
		}

		private void OnAesKeyChanged(byte[] value)
		{
			if (AesKeyChanged != null)
				AesKeyChanged(this, new ByteArrayEventArg(value));
		}

		private void OnAesOnChanged(bool value)
		{
			if (AesOnChanged != null)
				AesOnChanged(this, new BooleanEventArg(value));
		}

		private void OnBroadcastAddressChanged(byte value)
		{
			if (BroadcastAddressChanged != null)
				BroadcastAddressChanged(this, new ByteEventArg(value));
		}

		private void OnCrcAutoClearOffChanged(bool value)
		{
			if (CrcAutoClearOffChanged != null)
				CrcAutoClearOffChanged(this, new BooleanEventArg(value));
		}

		private void OnCrcOnChanged(bool value)
		{
			if (CrcOnChanged != null)
				CrcOnChanged(this, new BooleanEventArg(value));
		}

		private void OnDcFreeChanged(DcFreeEnum value)
		{
			if (DcFreeChanged != null)
				DcFreeChanged(this, new DcFreeEventArg(value));
		}

		private void OnDocumentationChanged(DocumentationChangedEventArgs e)
		{
			if (DocumentationChanged != null)
				DocumentationChanged(this, e);
		}

		private void OnEnterConditionChanged(EnterConditionEnum value)
		{
			if (EnterConditionChanged != null)
				EnterConditionChanged(this, new EnterConditionEventArg(value));
		}

		private void OnError(byte status, string message)
		{
			if (Error != null)
				Error(this, new ErrorEventArgs(status, message));
		}

		private void OnExitConditionChanged(ExitConditionEnum value)
		{
			if (ExitConditionChanged != null)
				ExitConditionChanged(this, new ExitConditionEventArg(value));
		}

		private void OnFifoFillConditionChanged(FifoFillConditionEnum value)
		{
			if (FifoFillConditionChanged != null)
				FifoFillConditionChanged(this, new FifoFillConditionEventArg(value));
		}

		private void OnFifoThresholdChanged(byte value)
		{
			if (FifoThresholdChanged != null)
				FifoThresholdChanged(this, new ByteEventArg(value));
		}

		private void OnIntermediateModeChanged(IntermediateModeEnum value)
		{
			if (IntermediateModeChanged != null)
				IntermediateModeChanged(this, new IntermediateModeEventArg(value));
		}

		private void OnInterPacketRxDelayChanged(int value)
		{
			if (InterPacketRxDelayChanged != null)
				InterPacketRxDelayChanged(this, new Int32EventArg(value));
		}

		private void OnMaxPacketNumberChanged(int value)
		{
			if (MaxPacketNumberChanged != null)
				MaxPacketNumberChanged(this, new Int32EventArg(value));
		}

		private void OnMessageChanged(byte[] value)
		{
			if (MessageChanged != null)
				MessageChanged(this, new ByteArrayEventArg(value));
		}

		private void OnMessageLengthChanged(int value)
		{
			if (MessageLengthChanged != null)
				MessageLengthChanged(this, new Int32EventArg(value));
		}

		private void OnNodeAddressChanged(byte value)
		{
			if (NodeAddressChanged != null)
				NodeAddressChanged(this, new ByteEventArg(value));
		}

		private void OnPacketFormatChanged(PacketFormatEnum value)
		{
			if (PacketFormatChanged != null)
				PacketFormatChanged(this, new PacketFormatEventArg(value));
		}

		private void OnPacketHandlerLogEnableChanged(bool value)
		{
			if (PacketHandlerLogEnableChanged != null)
				PacketHandlerLogEnableChanged(this, new BooleanEventArg(value));
		}

		private void OnPayloadLengthChanged(byte value)
		{
			if (PayloadLengthChanged != null)
				PayloadLengthChanged(this, new ByteEventArg(value));
		}

		private void OnPreambleSizeChanged(int value)
		{
			if (PreambleSizeChanged != null)
				PreambleSizeChanged(this, new Int32EventArg(value));
		}

		private void OnStartStopChanged(bool value)
		{
			if (StartStopChanged != null)
				StartStopChanged(this, new BooleanEventArg(value));
		}

		private void OnSyncOnChanged(bool value)
		{
			if (SyncOnChanged != null)
				SyncOnChanged(this, new BooleanEventArg(value));
		}

		private void OnSyncSizeChanged(byte value)
		{
			if (SyncSizeChanged != null)
				SyncSizeChanged(this, new ByteEventArg(value));
		}

		private void OnSyncTolChanged(byte value)
		{
			if (SyncTolChanged != null)
			{
				SyncTolChanged(this, new ByteEventArg(value));
			}
		}

		private void OnSyncValueChanged(byte[] value)
		{
			if (SyncValueChanged != null)
				SyncValueChanged(this, new ByteArrayEventArg(value));
		}

		private void OnTxStartConditionChanged(bool value)
		{
			if (TxStartConditionChanged != null)
				TxStartConditionChanged(this, new BooleanEventArg(value));
		}

		private void rBtnAddressFilteringNode_CheckedChanged(object sender, EventArgs e)
		{
			if (rBtnAddressFilteringNode.Checked)
			{
				AddressFiltering = AddressFilteringEnum.Node;
				OnAddressFilteringChanged(AddressFiltering);
			}
		}

		private void rBtnAddressFilteringNodeBroadcast_CheckedChanged(object sender, EventArgs e)
		{
			if (rBtnAddressFilteringNodeBroadcast.Checked)
			{
				AddressFiltering = AddressFilteringEnum.NodeBroadcast;
				OnAddressFilteringChanged(AddressFiltering);
			}
		}

		private void rBtnAddressFilteringOff_CheckedChanged(object sender, EventArgs e)
		{
			if (rBtnAddressFilteringOff.Checked)
			{
				AddressFiltering = AddressFilteringEnum.OFF;
				OnAddressFilteringChanged(AddressFiltering);
			}
		}

		private void rBtnAesOff_CheckedChanged(object sender, EventArgs e)
		{
			if (rBtnAesOff.Checked)
			{
				AesOn = rBtnAesOn.Checked;
				OnAesOnChanged(AesOn);
			}
		}

		private void rBtnAesOn_CheckedChanged(object sender, EventArgs e)
		{
			if (rBtnAesOn.Checked)
			{
				AesOn = rBtnAesOn.Checked;
				OnAesOnChanged(AesOn);
			}
		}

		private void rBtnCrcAutoClearOff_CheckedChanged(object sender, EventArgs e)
		{
			if (rBtnCrcAutoClearOff.Checked)
			{
				CrcAutoClearOff = rBtnCrcAutoClearOff.Checked;
				OnCrcAutoClearOffChanged(CrcAutoClearOff);
			}
		}

		private void rBtnCrcAutoClearOn_CheckedChanged(object sender, EventArgs e)
		{
			if (rBtnCrcAutoClearOn.Checked)
			{
				CrcAutoClearOff = rBtnCrcAutoClearOff.Checked;
				OnCrcAutoClearOffChanged(CrcAutoClearOff);
			}
		}

		private void rBtnCrcOff_CheckedChanged(object sender, EventArgs e)
		{
			if (rBtnCrcOff.Checked)
			{
				CrcOn = rBtnCrcOn.Checked;
				OnCrcOnChanged(CrcOn);
			}
		}

		private void rBtnCrcOn_CheckedChanged(object sender, EventArgs e)
		{
			if (rBtnCrcOn.Checked)
			{
				CrcOn = rBtnCrcOn.Checked;
				OnCrcOnChanged(CrcOn);
			}
		}

		private void rBtnDcFreeManchester_CheckedChanged(object sender, EventArgs e)
		{
			if (rBtnDcFreeManchester.Checked)
			{
				DcFree = DcFreeEnum.Manchester;
				OnDcFreeChanged(DcFree);
			}
		}

		private void rBtnDcFreeOff_CheckedChanged(object sender, EventArgs e)
		{
			if (rBtnDcFreeOff.Checked)
			{
				DcFree = DcFreeEnum.OFF;
				OnDcFreeChanged(DcFree);
			}
		}

		private void rBtnDcFreeWhitening_CheckedChanged(object sender, EventArgs e)
		{
			if (rBtnDcFreeWhitening.Checked)
			{
				DcFree = DcFreeEnum.Whitening;
				OnDcFreeChanged(DcFree);
			}
		}

		private void rBtnFifoFill_CheckedChanged(object sender, EventArgs e)
		{
			FifoFillCondition = rBtnFifoFillSyncAddress.Checked ? FifoFillConditionEnum.OnSyncAddressIrq : FifoFillConditionEnum.Allways;
			OnFifoFillConditionChanged(FifoFillCondition);
		}

		private void rBtnPacketFormat_CheckedChanged(object sender, EventArgs e)
		{
			if (rBtnPacketFormatVariable.Checked)
				PacketFormat = PacketFormatEnum.Variable;
			else
				PacketFormat = PacketFormatEnum.Fixed;
			OnPacketFormatChanged(PacketFormat);
		}

		private void rBtnSyncOn_CheckedChanged(object sender, EventArgs e)
		{
			SyncOn = rBtnSyncOn.Checked;
			OnSyncOnChanged(SyncOn);
		}

		private void rBtnTxStartFifoLevel_CheckedChanged(object sender, EventArgs e)
		{
			if (rBtnTxStartFifoLevel.Checked)
			{
				TxStartCondition = !rBtnTxStartFifoLevel.Checked;
				OnTxStartConditionChanged(TxStartCondition);
			}
		}

		private void rBtnTxStartFifoNotEmpty_CheckedChanged(object sender, EventArgs e)
		{
			if (rBtnTxStartFifoNotEmpty.Checked)
			{
				TxStartCondition = !rBtnTxStartFifoLevel.Checked;
				OnTxStartConditionChanged(TxStartCondition);
			}
		}

		private void tBox_Validated(object sender, EventArgs e)
		{
			if (sender == tBoxSyncValue)
			{
				tBoxSyncValue.Text = tBoxSyncValue.Text.ToUpper();
				lblPacketSyncValue.Text = tBoxSyncValue.Text;
				OnSyncValueChanged(SyncValue);
			}
			else if (sender == tBoxAesKey)
			{
				tBoxAesKey.Text = tBoxAesKey.Text.ToUpper();
				OnAesKeyChanged(AesKey);
			}
			else
			{
				TextBox box = (TextBox)sender;
				if ((box.Text != "") && !box.Text.StartsWith("0x", true, null))
					box.Text = "0x" + Convert.ToByte(box.Text, 0x10).ToString("X02");
			}
		}

		private void tBoxAesKey_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Shift
				|| e.Control
				|| Uri.IsHexDigit((char)e.KeyData)
				|| (e.KeyData >= Keys.NumPad0 && e.KeyData <= Keys.NumPad9)
				|| e.KeyData == Keys.Back || e.KeyData == Keys.Delete || e.KeyData == Keys.Left || e.KeyData == Keys.Right
				)
			{
				OnError(0, "-");
			}
			else
			{
				e.Handled = true;
				e.SuppressKeyPress = true;
			}
		}

		private void tBoxAesKey_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
		{
			OnError(1, "Input rejected at position: " + e.Position.ToString(CultureInfo.CurrentCulture));
		}

		private void tBoxAesKey_TextChanged(object sender, EventArgs e)
		{
			OnError(0, "-");
			aesWord.StringValue = tBoxAesKey.Text;
			aesKey = aesWord.ArrayValue;
		}

		private void tBoxAesKey_TypeValidationCompleted(object sender, TypeValidationEventArgs e)
		{
			try
			{
				if (!e.IsValidInput)
				{
					AesKey = MaskValidationType.InvalidMask.ArrayValue;
					throw new Exception("Wrong AES key entered.  Message: " + e.Message);
				}
				if (e.ReturnValue is MaskValidationType)
					AesKey = (e.ReturnValue as MaskValidationType).ArrayValue;
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		private void tBoxSyncValue_KeyDown(object sender, KeyEventArgs e)
		{
			if ((((e.Shift || e.Control) || Uri.IsHexDigit((char)((ushort)e.KeyData))) || ((e.KeyData >= Keys.NumPad0) && (e.KeyData <= Keys.NumPad9))) || (((e.KeyData == Keys.Back) || (e.KeyData == Keys.Delete)) || ((e.KeyData == Keys.Left) || (e.KeyData == Keys.Right))))
			{
				OnError(0, "-");
			}
			else
			{
				e.Handled = true;
				e.SuppressKeyPress = true;
			}
		}

		private void tBoxSyncValue_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
		{
			OnError(1, "Input rejected at position: " + e.Position.ToString(CultureInfo.CurrentCulture));
		}

		private void tBoxSyncValue_TextChanged(object sender, EventArgs e)
		{
			OnError(0, "-");
			syncWord.StringValue = tBoxSyncValue.Text;
			syncValue = syncWord.ArrayValue;
			lblPacketSyncValue.Text = syncWord.StringValue;
		}

		private void tBoxSyncValue_TypeValidationCompleted(object sender, TypeValidationEventArgs e)
		{
			try
			{
				if (!e.IsValidInput)
				{
					SyncValue = MaskValidationType.InvalidMask.ArrayValue;
					throw new Exception("Wrong Sync word entered.  Message: " + e.Message);
				}
				if (e.ReturnValue is MaskValidationType)
					SyncValue = (e.ReturnValue as MaskValidationType).ArrayValue;
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void UpdateSyncValueLimits(LimitCheckStatusEnum status, string message)
		{
			switch (status)
			{
				case LimitCheckStatusEnum.OK:
					tBoxSyncValue.BackColor = SystemColors.Window;
					break;

				case LimitCheckStatusEnum.OUT_OF_RANGE:
					tBoxSyncValue.BackColor = ControlPaint.LightLight(Color.Orange);
					break;

				case LimitCheckStatusEnum.ERROR:
					tBoxSyncValue.BackColor = ControlPaint.LightLight(Color.Red);
					break;
			}
			errorProvider.SetError(tBoxSyncValue, message);
		}

		public AddressFilteringEnum AddressFiltering
		{
			get
			{
				if (!rBtnAddressFilteringOff.Checked)
				{
					if (rBtnAddressFilteringNode.Checked)
					{
						return AddressFilteringEnum.Node;
					}
					if (rBtnAddressFilteringNodeBroadcast.Checked)
					{
						return AddressFilteringEnum.NodeBroadcast;
					}
				}
				return AddressFilteringEnum.OFF;
			}
			set
			{
				if (value == AddressFilteringEnum.Node)
				{
					rBtnAddressFilteringNode.Checked = true;
					lblPacketAddr.Visible = true;
					nudNodeAddress.Enabled = true;
					lblNodeAddress.Enabled = true;
					nudBroadcastAddress.Enabled = false;
					lblBroadcastAddress.Enabled = false;
				}
				else if (value == AddressFilteringEnum.NodeBroadcast)
				{
					rBtnAddressFilteringNodeBroadcast.Checked = true;
					lblPacketAddr.Visible = true;
					nudNodeAddress.Enabled = true;
					lblNodeAddress.Enabled = true;
					nudBroadcastAddress.Enabled = true;
					lblBroadcastAddress.Enabled = true;
				}
				else
				{
					rBtnAddressFilteringOff.Checked = true;
					lblPacketAddr.Visible = false;
					nudNodeAddress.Enabled = false;
					lblNodeAddress.Enabled = false;
					nudBroadcastAddress.Enabled = false;
					lblBroadcastAddress.Enabled = false;
				}
			}
		}

		public byte[] AesKey
		{
			get
			{
				return aesKey;
			}
			set
			{
				aesKey = value;
				try
				{
					tBoxAesKey.TextChanged -= new EventHandler(tBoxAesKey_TextChanged);
					tBoxAesKey.MaskInputRejected -= new MaskInputRejectedEventHandler(tBoxAesKey_MaskInputRejected);
					aesWord.ArrayValue = aesKey;
					tBoxAesKey.Text = aesWord.StringValue;
				}
				catch (Exception exception)
				{
					OnError(1, exception.Message);
				}
				finally
				{
					tBoxAesKey.TextChanged += new EventHandler(tBoxAesKey_TextChanged);
					tBoxAesKey.MaskInputRejected += new MaskInputRejectedEventHandler(tBoxAesKey_MaskInputRejected);
				}
			}
		}

		public bool AesOn
		{
			get
			{
				return rBtnAesOn.Checked;
			}
			set
			{
				tBoxAesKey.Enabled = value;
				rBtnAesOn.Checked = value;
				rBtnAesOff.Checked = !value;
			}
		}

		public decimal BitRate
		{
			get
			{
				return bitRate;
			}
			set
			{
				if (bitRate != value)
				{
					try
					{
						int selectedIndex = cBoxInterPacketRxDelay.SelectedIndex;
						cBoxInterPacketRxDelay.Items.Clear();
						for (int i = 0; i < 12; i++)
						{
							cBoxInterPacketRxDelay.Items.Add((Math.Pow(2.0, (double)i) / ((double)value)) * 1000.0);
						}
						cBoxInterPacketRxDelay.Items.Add(0.0);
						if (selectedIndex < cBoxInterPacketRxDelay.Items.Count)
						{
							cBoxInterPacketRxDelay.SelectedIndex = selectedIndex;
						}
						else
						{
							cBoxInterPacketRxDelay.SelectedIndex = 0;
						}
					}
					catch
					{
					}
					bitRate = value;
				}
			}
		}

		public byte BroadcastAddress
		{
			get
			{
				return (byte)nudBroadcastAddress.Value;
			}
			set
			{
				nudBroadcastAddress.Value = value;
				lblBroadcastAddress.Text = "0x" + value.ToString("X02");
			}
		}

		public ushort Crc
		{
			get
			{
				return crc;
			}
			set
			{
				crc = value;
				lblPacketCrc.Text = (((value >> 8) & 0xff)).ToString("X02") + "-" + ((value & 0xff)).ToString("X02");
			}
		}

		public bool CrcAutoClearOff
		{
			get
			{
				return rBtnCrcAutoClearOff.Checked;
			}
			set
			{
				rBtnCrcAutoClearOn.Checked = !value;
				rBtnCrcAutoClearOff.Checked = value;
			}
		}

		public bool CrcOn
		{
			get
			{
				return rBtnCrcOn.Checked;
			}
			set
			{
				lblPacketCrc.Visible = value;
				rBtnCrcOn.Checked = value;
				rBtnCrcOff.Checked = !value;
			}
		}

		public DataModeEnum DataMode
		{
			get
			{
				return dataMode;
			}
			set
			{
				dataMode = value;
				if ((DataMode == DataModeEnum.Packet) && ((mode == OperatingModeEnum.Tx) || (mode == OperatingModeEnum.Rx)))
				{
					cBtnPacketHandlerStartStop.Enabled = true;
					tBoxPacketsNb.Enabled = true;
					if (!cBtnPacketHandlerStartStop.Checked)
					{
						tBoxPacketsRepeatValue.Enabled = true;
					}
				}
				else
				{
					cBtnPacketHandlerStartStop.Enabled = false;
					tBoxPacketsNb.Enabled = false;
					tBoxPacketsRepeatValue.Enabled = false;
				}
				switch (dataMode)
				{
					case DataModeEnum.Packet:
						lblBitSynchroniser.Text = "ON";
						lblDataMode.Text = "Packet";
						return;

					case DataModeEnum.Reserved:
						lblBitSynchroniser.Text = "";
						lblDataMode.Text = "";
						return;

					case DataModeEnum.ContinuousBitSync:
						lblBitSynchroniser.Text = "ON";
						lblDataMode.Text = "Continuous";
						return;

					case DataModeEnum.Continuous:
						lblBitSynchroniser.Text = "OFF";
						lblDataMode.Text = "Continuous";
						return;
				}
			}
		}

		public DcFreeEnum DcFree
		{
			get
			{
				if (!rBtnDcFreeOff.Checked)
				{
					if (rBtnDcFreeManchester.Checked)
					{
						return DcFreeEnum.Manchester;
					}
					if (rBtnDcFreeWhitening.Checked)
					{
						return DcFreeEnum.Whitening;
					}
				}
				return DcFreeEnum.OFF;
			}
			set
			{
				if (value == DcFreeEnum.Manchester)
				{
					rBtnDcFreeManchester.Checked = true;
				}
				else if (value == DcFreeEnum.Whitening)
				{
					rBtnDcFreeWhitening.Checked = true;
				}
				else
				{
					rBtnDcFreeOff.Checked = true;
				}
			}
		}

		public EnterConditionEnum EnterCondition
		{
			get
			{
				return (EnterConditionEnum)cBoxEnterCondition.SelectedIndex;
			}
			set
			{
				cBoxEnterCondition.SelectedIndex = (int)value;
			}
		}

		public ExitConditionEnum ExitCondition
		{
			get
			{
				return (ExitConditionEnum)cBoxExitCondition.SelectedIndex;
			}
			set
			{
				cBoxExitCondition.SelectedIndex = (int)value;
			}
		}

		public FifoFillConditionEnum FifoFillCondition
		{
			get
			{
				if (!rBtnFifoFillSyncAddress.Checked)
				{
					return FifoFillConditionEnum.Allways;
				}
				return FifoFillConditionEnum.OnSyncAddressIrq;
			}
			set
			{
				if (value == FifoFillConditionEnum.OnSyncAddressIrq)
				{
					rBtnFifoFillSyncAddress.Checked = true;
				}
				else
				{
					rBtnFifoFillAlways.Checked = true;
				}
			}
		}

		public byte FifoThreshold
		{
			get
			{
				return (byte)nudFifoThreshold.Value;
			}
			set
			{
				nudFifoThreshold.Value = value;
			}
		}

		public IntermediateModeEnum IntermediateMode
		{
			get
			{
				return (IntermediateModeEnum)cBoxIntermediateMode.SelectedIndex;
			}
			set
			{
				cBoxIntermediateMode.SelectedIndex = (int)value;
			}
		}

		public int InterPacketRxDelay
		{
			get
			{
				return cBoxInterPacketRxDelay.SelectedIndex;
			}
			set
			{
				if (value >= 12)
				{
					cBoxInterPacketRxDelay.SelectedIndex = cBoxInterPacketRxDelay.Items.Count - 1;
				}
				else
				{
					cBoxInterPacketRxDelay.SelectedIndex = value;
				}
			}
		}

		public bool LogEnabled
		{
			get
			{
				return cBtnLog.Checked;
			}
			set
			{
				cBtnLog.Checked = value;
			}
		}

		public int MaxPacketNumber
		{
			get
			{
				return Convert.ToInt32(tBoxPacketsRepeatValue.Text);
			}
			set
			{
				tBoxPacketsRepeatValue.Text = value.ToString();
			}
		}

		public byte[] Message
		{
			get
			{
				return message;
			}
			set
			{
				message = value;
				DynamicByteProvider byteProvider = hexBoxPayload.ByteProvider as DynamicByteProvider;
				byteProvider.Bytes.Clear();
				byteProvider.Bytes.AddRange(value);
				hexBoxPayload.ByteProvider.ApplyChanges();
				hexBoxPayload.Invalidate();
			}
		}

		public int MessageLength
		{
			get
			{
				return Convert.ToInt32(lblPacketLength.Text, 0x10);
			}
			set
			{
				lblPacketLength.Text = value.ToString("X02");
			}
		}

		public OperatingModeEnum Mode
		{
			get
			{
				return mode;
			}
			set
			{
				mode = value;
				if ((DataMode == DataModeEnum.Packet) && ((mode == OperatingModeEnum.Tx) || (mode == OperatingModeEnum.Rx)))
				{
					cBtnPacketHandlerStartStop.Enabled = true;
					tBoxPacketsNb.Enabled = true;
					tBoxPacketsRepeatValue.Enabled = true;
				}
				else
				{
					cBtnPacketHandlerStartStop.Enabled = false;
					tBoxPacketsNb.Enabled = false;
					tBoxPacketsRepeatValue.Enabled = false;
				}
				switch (mode)
				{
					case OperatingModeEnum.Sleep:
						lblOperatingMode.Text = "Sleep";
						nudPayloadLength.Enabled = false;
						rBtnNodeAddressInPayloadYes.Enabled = false;
						rBtnNodeAddressInPayloadNo.Enabled = false;
						lblPacketsNb.Visible = false;
						tBoxPacketsNb.Visible = false;
						lblPacketsRepeatValue.Visible = false;
						tBoxPacketsRepeatValue.Visible = false;
						return;

					case OperatingModeEnum.Stdby:
						lblOperatingMode.Text = "Standby";
						nudPayloadLength.Enabled = false;
						rBtnNodeAddressInPayloadYes.Enabled = false;
						rBtnNodeAddressInPayloadNo.Enabled = false;
						lblPacketsNb.Visible = false;
						tBoxPacketsNb.Visible = false;
						lblPacketsRepeatValue.Visible = false;
						tBoxPacketsRepeatValue.Visible = false;
						return;

					case OperatingModeEnum.Fs:
						lblOperatingMode.Text = "Synthesizer";
						nudPayloadLength.Enabled = false;
						rBtnNodeAddressInPayloadYes.Enabled = false;
						rBtnNodeAddressInPayloadNo.Enabled = false;
						lblPacketsNb.Visible = false;
						tBoxPacketsNb.Visible = false;
						lblPacketsRepeatValue.Visible = false;
						tBoxPacketsRepeatValue.Visible = false;
						return;

					case OperatingModeEnum.Tx:
						lblOperatingMode.Text = "Transmitter";
						nudPayloadLength.Enabled = false;
						rBtnNodeAddressInPayloadYes.Enabled = true;
						rBtnNodeAddressInPayloadNo.Enabled = true;
						lblPacketsNb.Text = "Tx Packets:";
						lblPacketsNb.Visible = true;
						tBoxPacketsNb.Visible = true;
						lblPacketsRepeatValue.Visible = true;
						tBoxPacketsRepeatValue.Visible = true;
						return;

					case OperatingModeEnum.Rx:
						lblOperatingMode.Text = "Receiver";
						nudPayloadLength.Enabled = true;
						rBtnNodeAddressInPayloadYes.Enabled = false;
						rBtnNodeAddressInPayloadNo.Enabled = false;
						lblPacketsNb.Text = "Rx packets:";
						lblPacketsNb.Visible = true;
						tBoxPacketsNb.Visible = true;
						lblPacketsRepeatValue.Visible = false;
						tBoxPacketsRepeatValue.Visible = false;
						return;
				}
			}
		}

		public byte NodeAddress
		{
			get
			{
				return (byte)nudNodeAddress.Value;
			}
			set
			{
				nudNodeAddress.Value = value;
				lblPacketAddr.Text = value.ToString("X02");
				lblNodeAddress.Text = "0x" + value.ToString("X02");
			}
		}

		public byte NodeAddressRx
		{
			get
			{
				return 0;
			}
			set
			{
				lblPacketAddr.Text = value.ToString("X02");
			}
		}

		public PacketFormatEnum PacketFormat
		{
			get
			{
				if (rBtnPacketFormatVariable.Checked)
				{
					return PacketFormatEnum.Variable;
				}
				return PacketFormatEnum.Fixed;
			}
			set
			{
				if (Mode == OperatingModeEnum.Tx)
				{
					nudPayloadLength.Enabled = false;
				}
				else if (Mode == OperatingModeEnum.Rx)
				{
					nudPayloadLength.Enabled = true;
				}
				else
				{
					nudPayloadLength.Enabled = false;
				}
				if (value == PacketFormatEnum.Variable)
				{
					lblPacketLength.Visible = true;
					rBtnPacketFormatVariable.Checked = true;
				}
				else
				{
					lblPacketLength.Visible = false;
					rBtnPacketFormatFixed.Checked = true;
				}
			}
		}

		public int PacketNumber
		{
			get
			{
				return Convert.ToInt32(tBoxPacketsNb.Text);
			}
			set
			{
				tBoxPacketsNb.Text = value.ToString();
			}
		}

		public byte PayloadLength
		{
			get
			{
				return (byte)nudPayloadLength.Value;
			}
			set
			{
				nudPayloadLength.Value = value;
				lblPayloadLength.Text = "0x" + value.ToString("X02");
			}
		}

		public int PreambleSize
		{
			get
			{
				return (int)nudPreambleSize.Value;
			}
			set
			{
				nudPreambleSize.Value = value;
				switch (value)
				{
					case 0:
						lblPacketPreamble.Text = "";
						break;

					case 1:
						lblPacketPreamble.Text = "55";
						break;

					case 2:
						lblPacketPreamble.Text = "55-55";
						break;

					case 3:
						lblPacketPreamble.Text = "55-55-55";
						break;

					case 4:
						lblPacketPreamble.Text = "55-55-55-55";
						break;

					case 5:
						lblPacketPreamble.Text = "55-55-55-55-55";
						break;

					default:
						lblPacketPreamble.Text = "55-55-55-55-...-55";
						break;
				}
				if (nudPreambleSize.Value < 2M)
				{
					nudPreambleSize.BackColor = ControlPaint.LightLight(Color.Red);
					errorProvider.SetError(nudPreambleSize, "Preamble size must be greater than 12 bits!");
				}
				else
				{
					nudPreambleSize.BackColor = SystemColors.Window;
					errorProvider.SetError(nudPreambleSize, "");
				}
			}
		}

		public bool StartStop
		{
			get
			{
				return cBtnPacketHandlerStartStop.Checked;
			}
			set
			{
				cBtnPacketHandlerStartStop.Checked = value;
			}
		}

		public bool SyncOn
		{
			get
			{
				return rBtnSyncOn.Checked;
			}
			set
			{
				rBtnSyncOn.Checked = value;
				rBtnSyncOff.Checked = !value;
				nudSyncSize.Enabled = value;
				nudSyncTol.Enabled = value;
				tBoxSyncValue.Enabled = value;
				lblPacketSyncValue.Visible = value;
			}
		}

		public byte SyncSize
		{
			get { return (byte)nudSyncSize.Value; }
			set
			{
				try
				{
					nudSyncSize.Value = value;
					string text = tBoxSyncValue.Text;
					switch (((byte)nudSyncSize.Value))
					{
						case 1:
							tBoxSyncValue.Mask = "&&";
							break;

						case 2:
							tBoxSyncValue.Mask = "&&-&&";
							break;

						case 3:
							tBoxSyncValue.Mask = "&&-&&-&&";
							break;

						case 4:
							tBoxSyncValue.Mask = "&&-&&-&&-&&";
							break;

						case 5:
							tBoxSyncValue.Mask = "&&-&&-&&-&&-&&";
							break;

						case 6:
							tBoxSyncValue.Mask = "&&-&&-&&-&&-&&-&&";
							break;

						case 7:
							tBoxSyncValue.Mask = "&&-&&-&&-&&-&&-&&-&&";
							break;

						case 8:
							tBoxSyncValue.Mask = "&&-&&-&&-&&-&&-&&-&&-&&";
							break;

						default:
							throw new Exception("Wrong sync word size!");
					}
					tBoxSyncValue.Text = text;
				}
				catch (Exception exception)
				{
					OnError(1, exception.Message);
				}
			}
		}

		public byte SyncTol
		{
			get { return (byte)nudSyncTol.Value; }
			set { nudSyncTol.Value = value; }
		}

		public byte[] SyncValue
		{
			get { return syncValue; }
			set
			{
				syncValue = value;
				try
				{
					tBoxSyncValue.TextChanged -= new EventHandler(tBoxSyncValue_TextChanged);
					tBoxSyncValue.MaskInputRejected -= new MaskInputRejectedEventHandler(tBoxSyncValue_MaskInputRejected);
					syncWord.ArrayValue = syncValue;
					lblPacketSyncValue.Text = tBoxSyncValue.Text = syncWord.StringValue;
				}
				catch (Exception exception)
				{
					OnError(1, exception.Message);
				}
				finally
				{
					tBoxSyncValue.TextChanged += new EventHandler(tBoxSyncValue_TextChanged);
					tBoxSyncValue.MaskInputRejected += new MaskInputRejectedEventHandler(tBoxSyncValue_MaskInputRejected);
				}
			}
		}

		public bool TxStartCondition
		{
			get { return rBtnTxStartFifoNotEmpty.Checked; }
			set
			{
				rBtnTxStartFifoNotEmpty.Checked = value;
				rBtnTxStartFifoLevel.Checked = !value;
			}
		}
	}
}
