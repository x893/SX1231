using SemtechLib.Devices.SX1231.Enumerations;
using SemtechLib.Devices.SX1231.Events;
using SemtechLib.Devices.SX1231.General;
using SemtechLib.Ftdi;
using SemtechLib.General;
using SemtechLib.General.Events;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SemtechLib.Devices.SX1231
{
	public class SX1231 : INotifyPropertyChanged, IDisposable
	{
		private const byte PA3_CS = 0x8;
		private const byte PA5_RESET = 0x20;
		private const byte PB6_LED1 = 0x40;
		private const byte PB7_LED2 = 0x80;
		private const byte PB_LEDS = 0xC0;

		public delegate void ErrorEventHandler(object sender, SemtechLib.General.Events.ErrorEventArgs e);
		public delegate void LimitCheckStatusChangedEventHandler(object sender, LimitCheckStatusEventArg e);
		public delegate void PacketHandlerReceivedEventHandler(object sender, PacketStatusEventArg e);
		public delegate void PacketHandlerTransmittedEventHandler(object sender, PacketStatusEventArg e);

		public event LimitCheckStatusChangedEventHandler BitRateLimitStatusChanged;
		public event EventHandler Connected;
		public event EventHandler Disconected;
		public event ErrorEventHandler Error;
		public event LimitCheckStatusChangedEventHandler FdevLimitStatusChanged;
		public event LimitCheckStatusChangedEventHandler FrequencyRfLimitStatusChanged;
		public event PacketHandlerReceivedEventHandler PacketHandlerReceived;
		public event EventHandler PacketHandlerStarted;
		public event EventHandler PacketHandlerStoped;
		public event PacketHandlerTransmittedEventHandler PacketHandlerTransmitted;
		public event PropertyChangedEventHandler PropertyChanged;
		public event LimitCheckStatusChangedEventHandler SyncValueLimitChanged;

		#region Private variables
		private const int NOISE_ABSOLUTE_ZERO = -174;
		private const int NOISE_FIGURE = 7;
		private const int BR_MIN = 600;
		private const int BRF_MAX = 300000;
		private const int BRO_MAX = 32768;
		private const int BW_SSB_MAX = 500000;
		private const int DEMOD_SNR = 8;
		private const int FDA_MAX = 300000;
		private const int FDA_MIN = 600;
		private const int FR_BAND_1_MAX = 340000000;
		private const int FR_BAND_1_MIN = 290000000;
		private const int FR_BAND_2_MAX = 510000000;
		private const int FR_BAND_2_MIN = 424000000;
		private const int FR_BAND_3_MAX = 1020000000;
		private const int FR_BAND_3_MIN = 862000000;

		private bool adcLowPowerOn;
		private bool afcAutoClearOn;
		private bool afcAutoOn;
		private decimal afcDccFreq = 497M;
		private bool afcDone;
		private bool afcLowBetaOn;
		private decimal afcRxBw = 25000M;
		private decimal afcValue = 0.0M;
		private bool agcAutoRefOn = true;
		private int agcRefLevel = -80;
		private byte agcSnrMargin = 5;
		private byte agcStep1 = 0x10;
		private byte agcStep2 = 7;
		private byte agcStep3 = 11;
		private byte agcStep4 = 9;
		private byte agcStep5 = 11;
		private bool autoMode;
		private decimal bitRate = 4800M;
		protected bool bitRateFdevCheckDisbale;
		private ClockOutEnum clockOut = ClockOutEnum.CLOCK_OUT_111;
		private bool crcOk;
		private bool dagcOn;
		private DataModeEnum dataMode;
		private decimal dccFreq = 414M;
		private string deviceName;
		private DioMappingEnum dio0Mapping;
		private DioMappingEnum dio1Mapping;
		private DioMappingEnum dio2Mapping;
		private DioMappingEnum dio3Mapping;
		private DioMappingEnum dio4Mapping;
		private DioMappingEnum dio5Mapping;
		private bool fastRx;
		private decimal fdev = 5000M;
		private bool feiDone;
		private decimal feiValue = 0.0M;
		private byte[] FifoData = new byte[0x42];
		private bool fifoFull;
		private bool fifoLevel;
		private bool fifoNotEmpty;
		private bool fifoOverrun;
		private bool firstTransmit;
		private bool frameReceived;
		private bool frameTransmitted;
		private decimal frequencyRf = 915000000M;
		protected bool frequencyRfCheckDisable;
		private decimal frequencyStep = (32000000M / ((decimal)Math.Pow(2.0, 19.0)));
		private decimal frequencyXo = 32000000M;
		private FtdiDevice ftdi;
		private bool isOpen;
		private bool isPacketModeRunning;
		private decimal listenCoefIdle = 1004.5M;
		private decimal listenCoefRx = 131.2M;
		private ListenCriteriaEnum listenCriteria;
		private ListenEndEnum listenEnd = ListenEndEnum.RxMode;
		private bool listenMode;
		private ListenResolEnum listenResolIdle = ListenResolEnum.Res004100;
		private ListenResolEnum listenResolRx = ListenResolEnum.Res004100;
		private LnaGainEnum lnaCurrentGain = LnaGainEnum.G1;
		private LnaGainEnum lnaGainSelect = LnaGainEnum.G1;
		private bool lnaLowPowerOn = true;
		private LnaZinEnum lnaZin = LnaZinEnum.ZIN_200;
		private bool lowBat;
		private bool lowBatMonitor;
		private bool lowBatOn;
		private LowBatTrimEnum lowBatTrim = LowBatTrimEnum.Trim1_835;
		private decimal lowBetaAfcOffset;
		private int maxPacketNumber;
		private OperatingModeEnum mode = OperatingModeEnum.Stdby;
		private bool modeReady;
		private byte modulationShaping;
		private ModulationTypeEnum modulationType;
		private bool monitor = true;
		private bool m_ocpOn = true;
		private decimal m_ocpTrim = 100M;
		private OokAverageThreshFiltEnum m_ookAverageThreshFilt = OokAverageThreshFiltEnum.COEF_2;
		private byte m_ookFixedThresh = 6;
		private OokPeakThreshDecEnum m_ookPeakThreshDec;
		private decimal m_ookPeakThreshStep = 0.5M;
		private decimal[] ookPeakThreshStepTable = new decimal[] { 0.5M, 1.0M, 1.5M, 2.0M, 3.0M, 4.0M, 5.0M, 6.0M };
		private OokThreshTypeEnum m_ookThreshType = OokThreshTypeEnum.Peak;
		private decimal m_outputPower = 13.0M;
		private SemtechLib.Devices.SX1231.General.Packet m_packet;
		private int packetNumber;
		private bool packetSent;
		private PaModeEnum m_paMode;
		private PaRampEnum m_paRamp = PaRampEnum.PaRamp_40;
		private bool m_payloadReady;
		private bool m_pllLock;
		private LnaGainEnum prevLnaGainSelect;
		private OperatingModeEnum prevMode;
		private ModulationTypeEnum prevModulationType;
		private bool prevMonitorOn;
		private int prevRfPaSwitchEnabled;
		private RfPaSwitchSelEnum prevRfPaSwitchSel;
		private bool prevRssiAutoThresh;
		private decimal prevRssiThresh;
		private decimal prevRssiValue = -127.5M;
		private bool m_rcCalDone;
		protected int readLock;
		private RegisterCollection m_registers;
		protected Thread regUpdateThread;
		protected bool regUpdateThreadContinue;
		protected bool restartRx;
		private decimal rfIoRssiValue = -127.5M;
		private decimal rfPaRssiValue = -127.5M;
		private int rfPaSwitchEnabled;
		private RfPaSwitchSelEnum rfPaSwitchSel;
		private bool rssi;
		private bool rssiAutoThresh = true;
		private bool rssiDone;
		private decimal rssiThresh = -114M;
		private decimal rssiValue = -127.5M;
		private decimal rxBw = 5208M;
		private bool rxReady;
		private bool sensitivityBoostOn;
		private bool sequencer = true;
		private int spectrumFreqId;
		private decimal spectrumFreqSpan = 1000000M;
		private bool spectrumOn;
		private decimal spectrumRssiValue = -127.5M;
		private int spiSpeed = 2000000;
		private bool syncAddressMatch;
		protected object syncThread = new object();
		private bool tempCalDone;
		private bool tempMeasRunning;
		private decimal tempValue = 165.0M;
		private decimal tempValueCal = 165.0M;
		private decimal tempValueRoom = 25.0M;
		private bool test;
		private bool timeout;
		private decimal timeoutRssiThresh;
		private decimal timeoutRxStart;
		private bool txReady;
		private string version = "0.0";
		protected int writeLock;
		#endregion

		public SX1231()
		{
			PropertyChanged += new PropertyChangedEventHandler(SX1231_PropertyChanged);

			ftdi = new FtdiDevice(FtdiDevice.MpsseProtocol.SPI);

			ftdi.Opened += new EventHandler(ftdi_Opened);
			ftdi.Closed += new EventHandler(ftdi_Closed);

			ftdi.PortB.Io0Changed += new FtdiIoPort.IoChangedEventHandler(sx131_PB0_RXTX_Changed);
			ftdi.PortB.Io1Changed += new FtdiIoPort.IoChangedEventHandler(sx131_PB_Dio1or5Changed);
			ftdi.PortB.Io2Changed += new FtdiIoPort.IoChangedEventHandler(sx131_PB_Dio2Changed);
			ftdi.PortB.Io3Changed += new FtdiIoPort.IoChangedEventHandler(sx131_PB_Dio3Changed);
			ftdi.PortB.Io4Changed += new FtdiIoPort.IoChangedEventHandler(sx131_PB_Dio4Changed);
			ftdi.PortB.Io5Changed += new FtdiIoPort.IoChangedEventHandler(sx131_PB_Dio1or5Changed);

			ftdi.PortA.Io7Changed += new FtdiIoPort.IoChangedEventHandler(sx131_PA_Dio7Changed);

			PopulateRegisters();
		}

		private void sx131_PB0_RXTX_Changed(object sender, FtdiIoPort.IoChangedEventArgs e)
		{
			lock (syncThread)
				if (isPacketModeRunning && (e.State || firstTransmit))
				{
					firstTransmit = false;
					if (Mode == OperatingModeEnum.Tx)
					{
						OnPacketHandlerTransmitted();
						PacketHandlerTransmit();
					}
					else if (Mode == OperatingModeEnum.Rx)
						PacketHandlerReceive();
				}
		}

		private void sx131_PB_Dio1or5Changed(object sender, FtdiIoPort.IoChangedEventArgs e) { }
		private void sx131_PB_Dio4Changed(object sender, FtdiIoPort.IoChangedEventArgs e) { }
		private void sx131_PB_Dio2Changed(object sender, FtdiIoPort.IoChangedEventArgs e) { }
		private void sx131_PA_Dio7Changed(object sender, FtdiIoPort.IoChangedEventArgs e) { }
		private void sx131_PB_Dio3Changed(object sender, FtdiIoPort.IoChangedEventArgs e) { }

		private void BitRateFdevCheck(decimal bitRate, decimal fdev)
		{
			decimal num = 300000M;
			decimal num2 = 600M;
			if (!bitRateFdevCheckDisbale)
			{
				if (modulationType == ModulationTypeEnum.OOK)
					num = 32768M;
				if ((bitRate < num2) || (bitRate > num))
					OnBitRateLimitStatusChanged(LimitCheckStatusEnum.OUT_OF_RANGE, "The bitrate is out of range.\nThe valid range is [" + num2.ToString() + ", " + num.ToString() + "]");
				else
					OnBitRateLimitStatusChanged(LimitCheckStatusEnum.OK, "");

				if (modulationType != ModulationTypeEnum.OOK)
				{
					if (fdev < 600M || fdev > 300000M)
						OnFdevLimitStatusChanged(LimitCheckStatusEnum.OUT_OF_RANGE, "The frequency deviation is out of range.\nThe valid range is [600, 300000]");
					else if ((fdev + (bitRate / 2M)) > 500000M)
						OnFdevLimitStatusChanged(LimitCheckStatusEnum.ERROR, "The single sided band width has been exceeded.\n Fdev + ( Bitrate / 2 ) > 500000 Hz");
					else
					{
						decimal num3 = (2.0M * fdev) / bitRate;
						if (0.4969M <= num3 && num3 <= 10.0M)
							OnFdevLimitStatusChanged(LimitCheckStatusEnum.OK, "");
						else
							OnFdevLimitStatusChanged(LimitCheckStatusEnum.ERROR, "The modulation index is out of range.\nThe valid range is [0.5, 10]");
					}
				}
				else
					OnFdevLimitStatusChanged(LimitCheckStatusEnum.OK, "");
			}
		}

		public bool Close()
		{
			if (isOpen || (ftdi != null && ftdi.IsOpen))
			{
				ftdi.Close();
				isOpen = false;
			}
			return true;
		}

		private decimal ComputeDccFreq(decimal bw, uint register)
		{
			return ((4.0M * bw) / (6.283185307179580M * ((decimal)Math.Pow(2.0, (double)((register >> 5) + 2)))));
		}

		public static decimal ComputeRxBw(decimal frequencyXo, ModulationTypeEnum mod, int mant, int exp)
		{
			if (mod == ModulationTypeEnum.FSK)
				return (frequencyXo / (mant * ((decimal)Math.Pow(2.0, (double)(exp + 2)))));
			return (frequencyXo / (mant * ((decimal)Math.Pow(2.0, (double)(exp + 3)))));
		}

		public static decimal[] ComputeRxBwFreqTable(decimal frequencyXo, ModulationTypeEnum mod)
		{
			decimal[] numArray = new decimal[0x18];
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			for (num = 0; num < 8; num++)
			{
				for (num2 = 16; num2 <= 24; num2 += 4)
				{
					if (mod == ModulationTypeEnum.FSK)
						numArray[num3++] = frequencyXo / (num2 * ((decimal)Math.Pow(2.0, (double)(num + 2))));
					else
						numArray[num3++] = frequencyXo / (num2 * ((decimal)Math.Pow(2.0, (double)(num + 3))));
				}
			}
			return numArray;
		}

		public static void ComputeRxBwMantExp(decimal frequencyXo, ModulationTypeEnum mod, decimal value, ref int mant, ref int exp)
		{
			int num = 0;
			int num2 = 0;
			decimal num3 = 0M;
			decimal num4 = 10000000M;
			for (num = 0; num < 8; num++)
			{
				for (num2 = 0x10; num2 <= 0x18; num2 += 4)
				{
					if (mod == ModulationTypeEnum.FSK)
						num3 = frequencyXo / (num2 * ((decimal)Math.Pow(2.0, (double)(num + 2))));
					else
						num3 = frequencyXo / (num2 * ((decimal)Math.Pow(2.0, (double)(num + 3))));

					if (Math.Abs((decimal)(num3 - value)) < num4)
					{
						num4 = Math.Abs((decimal)(num3 - value));
						mant = num2;
						exp = num;
					}
				}
			}
		}

		private decimal ComputeRxBwMax()
		{
			if (ModulationType == ModulationTypeEnum.FSK)
				return (FrequencyXo / (16M * ((decimal)Math.Pow(2.0, 2.0))));
			return (FrequencyXo / (16M * ((decimal)Math.Pow(2.0, 3.0))));
		}

		private decimal ComputeRxBwMin()
		{
			if (ModulationType == ModulationTypeEnum.FSK)
				return (FrequencyXo / (24M * ((decimal)Math.Pow(2.0, 9.0))));
			return (FrequencyXo / (24M * ((decimal)Math.Pow(2.0, 10.0))));
		}

		public void Dispose()
		{
			Close();
		}

		private void FrequencyRfCheck(decimal value)
		{
			if (!frequencyRfCheckDisable)
			{
				if ((value >= 290000000M && value <= 340000000M)
				|| (value >= 424000000M && value <= 510000000M)
				|| (value >= 862000000M && value <= 1020000000M)
					)
					OnFrequencyRfLimitStatusChanged(LimitCheckStatusEnum.OK, "");
				else
				{
					string[] strArray2 = new string[] {
						string.Concat(new string[] { "[", 290000000M.ToString(), ", ", 340000000M.ToString(), "]" }),
						string.Concat(new string[] { "[", 424000000M.ToString(), ", ", 510000000M.ToString(), "]" }),
						string.Concat(new string[] { "[", 862000000M.ToString(), ", ", 1020000000M.ToString(), "]" })
					};
					OnFrequencyRfLimitStatusChanged(LimitCheckStatusEnum.OUT_OF_RANGE, "The RF frequency is out of range.\nThe valid ranges are:\n" + strArray2[0] + "\n" + strArray2[1] + "\n" + strArray2[2]);
				}
			}
		}

		private void ftdi_Closed(object sender, EventArgs e)
		{
			spectrumOn = false;
			isOpen = false;
			regUpdateThreadContinue = false;
			OnDisconnected();
			OnError(0, "-");
		}

		private void ftdi_Opened(object sender, EventArgs e)
		{
			if (isOpen)
				OnConnected();
		}

		public void ListenModeAbort()
		{
			try
			{
				byte num = (byte)m_registers["RegOpMode"].Value;
				num = (byte)(num & 0x9f);
				num = (byte)(num | 0x20);
				m_registers["RegOpMode"].Value = num;
				ReadRegister(m_registers["RegOpMode"]);
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		private void OnBitRateLimitStatusChanged(LimitCheckStatusEnum status, string message)
		{
			if (BitRateLimitStatusChanged != null)
				BitRateLimitStatusChanged(this, new LimitCheckStatusEventArg(status, message));
		}

		private void OnConnected()
		{
			if (Connected != null)
				Connected(this, EventArgs.Empty);
		}

		private void OnDisconnected()
		{
			if (Disconected != null)
				Disconected(this, EventArgs.Empty);
		}

		private void OnError(byte status, string message)
		{
			if (Error != null)
				Error(this, new SemtechLib.General.Events.ErrorEventArgs(status, message));
		}

		private void OnFdevLimitStatusChanged(LimitCheckStatusEnum status, string message)
		{
			if (FdevLimitStatusChanged != null)
				FdevLimitStatusChanged(this, new LimitCheckStatusEventArg(status, message));
		}

		private void OnFrequencyRfLimitStatusChanged(LimitCheckStatusEnum status, string message)
		{
			if (FrequencyRfLimitStatusChanged != null)
				FrequencyRfLimitStatusChanged(this, new LimitCheckStatusEventArg(status, message));
		}

		private void OnPacketHandlerReceived()
		{
			if (PacketHandlerReceived != null)
				PacketHandlerReceived(this, new PacketStatusEventArg(packetNumber, maxPacketNumber));
		}

		private void OnPacketHandlerStarted()
		{
			if (PacketHandlerStarted != null)
				PacketHandlerStarted(this, new EventArgs());
		}

		private void OnPacketHandlerStoped()
		{
			if (PacketHandlerStoped != null)
				PacketHandlerStoped(this, new EventArgs());
		}

		private void OnPacketHandlerTransmitted()
		{
			if (PacketHandlerTransmitted != null)
				PacketHandlerTransmitted(this, new PacketStatusEventArg(packetNumber, maxPacketNumber));
		}

		private void OnPropertyChanged(string propName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propName));
		}

		private void OnSyncValueLimitChanged(LimitCheckStatusEnum status, string message)
		{
			if (SyncValueLimitChanged != null)
				SyncValueLimitChanged(this, new LimitCheckStatusEventArg(status, message));
		}

		public bool Open(string name)
		{
			try
			{
				deviceName = name;
				Close();

				if (ftdi.Open(name)
				&& ftdi.PortA.Init((uint)spiSpeed)
				&& ftdi.PortB.Init(1000000)
					)
				{
					ftdi.PortA.PortDir = 0x0B;
					ftdi.PortA.PortValue = 0x0E;

					ftdi.PortB.PortDir = 0xC0;

					if (test)
						ftdi.PortB.PortValue = 0xC0;
					else
						ftdi.PortB.PortValue = 0;

					ftdi.PortB.SendBytes();

					isOpen = true;

					PopulateRegisters();
					regUpdateThreadContinue = true;
					regUpdateThread = new Thread(new ThreadStart(RegUpdateThread));
					regUpdateThread.Start();
					OnConnected();

					return true;
				}
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
			return false;
		}

		public void Open(ref FileStream configFileStream)
		{
			OnError(0, "-");
			StreamReader reader = new StreamReader(configFileStream, Encoding.ASCII);
			int lineNumber = 1;
			int num2 = 0;
			string data = "";
			try
			{
				string line;
				while ((line = reader.ReadLine()) != null)
				{
					if (line[0] == '#')
					{
						lineNumber++;
						continue;
					}
					if (line[0] != 'R' && line[0] != 'P')
						throw new Exception("At line " + lineNumber.ToString() + ": A configuration line must start either by\n\"#\" for comments\nor a\n\"R\" for the register name.");

					string[] tokens = line.Split(new char[] { '\t' });
					if (tokens.Length != 4)
					{
						if (tokens.Length != 2)
							throw new Exception(string.Concat("At line ", lineNumber.ToString(), ": The number of columns is ", tokens.Length.ToString(), " and it should be 4 or 2."));
						if (tokens[0] != "PKT")
							throw new Exception("At line " + lineNumber.ToString() + ": Invalid Packet.");
						data = tokens[1];
					}
					else
					{
						bool flag = true;
						for (int i = 0; i < m_registers.Count; i++)
						{
							if (m_registers[i].Name == tokens[1])
							{
								flag = false;
								break;
							}
							switch (tokens[1])
							{
								case "RegAgcThres1":
									tokens[1] = "RegAgcThresh1";
									flag = false;
									break;
								case "RegAgcThres2":
									tokens[1] = "RegAgcThresh2";
									flag = false;
									break;
								case "RegAgcThres3":
									tokens[1] = "RegAgcThresh3";
									flag = false;
									break;
							}

							switch (Version)
							{
								case "2.1":
									switch (tokens[1])
									{
										case "RegAfcCtrl":
											tokens[1] = "RegOsc2";
											flag = false;
											break;
										case "Reserved14":
											tokens[1] = "RegAgcRef";
											flag = false;
											break;
										case "Reserved15":
											tokens[1] = "RegAgcThresh1";
											flag = false;
											break;
										case "Reserved16":
											tokens[1] = "RegAgcThresh2";
											flag = false;
											break;
										case "Reserved17":
											tokens[1] = "RegAgcThresh3";
											flag = false;
											break;
										case "RegTestLna":
											flag = false;
											break;
										case "RegTestAfc":
											flag = false;
											break;
										case "RegTestDagc":
											flag = false;
											break;
									}
									break;
								case "2.2":
								case "2.3":
									switch (tokens[1])
									{
										case "RegOsc2":
											tokens[1] = "RegAfcCtrl";
											flag = false;
											break;
										case "RegAgcRef":
											tokens[1] = "Reserved14";
											flag = false;
											break;
										case "RegAgcThresh1":
											tokens[1] = "Reserved15";
											flag = false;
											break;
										case "RegAgcThresh2":
											tokens[1] = "Reserved16";
											flag = false;
											break;
										case "RegAgcThresh3":
											tokens[1] = "Reserved17";
											flag = false;
											break;
									}
									break;
							}

							if (!flag)
								break;
						}
						if (flag)
							throw new Exception("At line " + lineNumber.ToString() + ": Invalid register name.");

						if (tokens[1] != "RegVersion"
						&& (Version != "2.1" || (tokens[1] != "RegTestLna" && tokens[1] != "RegTestAfc" && tokens[1] != "RegTestDagc"))
							)
						{
							m_registers[tokens[1]].Value = Convert.ToByte(tokens[3], 0x10);
							num2++;
						}
					}
					lineNumber++;
				}
				m_packet.SetSaveData(data);
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
			finally
			{
				reader.Close();
				if (!IsOpen)
					ReadRegisters();
			}
		}

		private void packet_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			OnPropertyChanged(e.PropertyName);
		}

		private void PacketHandlerReceive()
		{
			object obj2;
			System.Threading.Monitor.Enter(obj2 = syncThread);
			try
			{
				byte[] buffer;
				SetModeLeds(OperatingModeEnum.Rx);
				byte data = 0;
				ReadRegister(m_registers["RegRssiValue"], ref data);
				m_packet.Rssi = -data / 2.0M;
				frameReceived = ReceiveRfData(out buffer);
				if (m_packet.PacketFormat == PacketFormatEnum.Fixed)
				{
					if (m_packet.AddressFiltering != AddressFilteringEnum.OFF)
					{
						m_packet.NodeAddressRx = buffer[0];
						Array.Copy(buffer, 1, buffer, 0, buffer.Length - 1);
						Array.Resize<byte>(ref buffer, m_packet.PayloadLength - 1);
					}
					else
						Array.Resize<byte>(ref buffer, m_packet.PayloadLength);
				}
				else if (m_packet.PacketFormat == PacketFormatEnum.Variable)
				{
					int newSize = buffer[0];
					Array.Copy(buffer, 1, buffer, 0, buffer.Length - 1);
					Array.Resize<byte>(ref buffer, newSize);
					if (m_packet.AddressFiltering != AddressFilteringEnum.OFF)
					{
						newSize--;
						m_packet.NodeAddressRx = buffer[0];
						Array.Copy(buffer, 1, buffer, 0, buffer.Length - 1);
						Array.Resize<byte>(ref buffer, newSize);
					}
				}
				m_packet.Message = buffer;
				packetNumber++;
				OnPacketHandlerReceived();
				if (!isPacketModeRunning)
					PacketHandlerStop();
				SetModeLeds(OperatingModeEnum.Sleep);
			}
			catch (Exception exception)
			{
				PacketHandlerStop();
				OnError(1, exception.Message);
			}
			finally
			{
				System.Threading.Monitor.Exit(obj2);
			}
		}

		private void PacketHandlerStart()
		{
			object obj2;
			System.Threading.Monitor.Enter(obj2 = syncThread);
			try
			{
				SetModeLeds(OperatingModeEnum.Sleep);
				packetNumber = 0;
				SetDataMode(DataModeEnum.Packet);
				if (Mode == OperatingModeEnum.Tx)
				{
					if (m_packet.MessageLength == 0)
					{
						MessageBox.Show("Message must be at least one byte long", "SX1231SKB-PacketHandler", MessageBoxButtons.OK, MessageBoxIcon.Hand);
						throw new Exception("Message must be at least one byte long");
					}
					SetDioMapping(0, DioMappingEnum.DIO_MAP_00);
					SetDioMapping(1, DioMappingEnum.DIO_MAP_01);
				}
				else if (Mode == OperatingModeEnum.Rx)
				{
					SetDioMapping(0, DioMappingEnum.DIO_MAP_01);
					SetDioMapping(1, DioMappingEnum.DIO_MAP_10);
				}
				frameTransmitted = false;
				frameReceived = false;
				if (Mode == OperatingModeEnum.Tx)
				{
					SetOperatingMode(OperatingModeEnum.Tx, true);
					firstTransmit = true;
				}
				else
				{
					SetOperatingMode(OperatingModeEnum.Rx, true);
					OnPacketHandlerReceived();
				}
				isPacketModeRunning = true;
				OnPacketHandlerStarted();
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
				PacketHandlerStop();
			}
			finally
			{
				System.Threading.Monitor.Exit(obj2);
			}
		}

		private void PacketHandlerStop()
		{
			try
			{
				lock (syncThread)
				{
					isPacketModeRunning = false;
					SetOperatingMode(Mode);
					frameTransmitted = false;
					frameReceived = false;
					firstTransmit = false;
				}
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
			finally
			{
				OnPacketHandlerStoped();
			}
		}

		private void PacketHandlerTransmit()
		{
			lock (syncThread)
			{
				try
				{
					SetModeLeds(OperatingModeEnum.Tx);
					if ((maxPacketNumber != 0 && packetNumber >= maxPacketNumber) || !isPacketModeRunning)
						PacketHandlerStop();
					else
					{
						frameTransmitted = TransmitRfData(m_packet.ToArray());
						packetNumber++;
					}
				}
				catch (Exception exception)
				{
					PacketHandlerStop();
					OnError(1, exception.Message);
				}
				finally
				{
					SetModeLeds(OperatingModeEnum.Sleep);
				}
			}
		}

		private void PopulateRegisters()
		{
			if (IsOpen)
			{
				byte data = 0;
				if (!Read(0x10, ref data))
					throw new Exception("Unable to read register RegVersion");
				if (!Read(0x10, ref data))
					throw new Exception("Unable to read register RegVersion");
				Version = (((data & 0xF0) >> 4)).ToString() + "." + ((data & 0x0F)).ToString();
			}

			m_registers = new RegisterCollection();

			m_registers.Add(new Register("RegFifo", 0x00, 0x00, true, true));
			m_registers.Add(new Register("RegOpMode", 0x01, 0x04, false, true));
			m_registers.Add(new Register("RegDataModul", 0x02, 0, false, true));
			m_registers.Add(new Register("RegBitrateMsb", 0x03, 0x1A, false, true));
			m_registers.Add(new Register("RegBitrateLsb", 0x04, 0x0B, false, true));
			m_registers.Add(new Register("RegFdevMsb", 0x05, 0x00, false, true));
			m_registers.Add(new Register("RegFdevLsb", 0x06, 0x52, false, true));
			m_registers.Add(new Register("RegFrfMsb", 0x07, 0xE4, false, true));
			m_registers.Add(new Register("RegFrfMid", 0x08, 0xC0, false, true));
			m_registers.Add(new Register("RegFrfLsb", 0x09, 0x00, false, true));
			m_registers.Add(new Register("RegOsc1", 0x0A, 0x41, false, true));

			if (Version == "2.1")
				m_registers.Add(new Register("RegOsc2", 0x0B, 0x40, false, true));
			else
				m_registers.Add(new Register("RegAfcCtrl", 0x0B, 0, false, true));

			m_registers.Add(new Register("RegLowBat", 0x0C, 2, false, true));
			m_registers.Add(new Register("RegListen1", 0x0D, 0xA2, false, true));
			m_registers.Add(new Register("RegListen2", 0x0E, 0xF5, false, true));
			m_registers.Add(new Register("RegListen3", 0x0F, 0x20, false, true));
			m_registers.Add(new Register("RegVersion", 0x10, 0x23, true, true));
			m_registers.Add(new Register("RegPaLevel", 0x11, 0x9F, false, true));
			m_registers.Add(new Register("RegPaRamp", 0x12, 0x09, false, true));
			m_registers.Add(new Register("RegOcp", 0x13, 0x1B, false, true));
			if (Version == "2.1")
			{
				m_registers.Add(new Register("RegAgcRef", 0x14, 0x40, false, true));
				m_registers.Add(new Register("RegAgcThresh1", 0x15, 0xB0, false, true));
				m_registers.Add(new Register("RegAgcThresh2", 0x16, 0x7B, false, true));
				m_registers.Add(new Register("RegAgcThresh3", 0x17, 0x9B, false, true));
			}
			else
			{
				m_registers.Add(new Register("Reserved14", 0x14, 0x40, false, true));
				m_registers.Add(new Register("Reserved15", 0x15, 0xb0, false, true));
				m_registers.Add(new Register("Reserved16", 0x16, 0x7b, false, true));
				m_registers.Add(new Register("Reserved17", 0x17, 0x9b, false, true));
			}

			m_registers.Add(new Register("RegLna", 0x18, 0x88, false, true));
			m_registers.Add(new Register("RegRxBw", 0x19, 0x55, false, true));
			m_registers.Add(new Register("RegAfcBw", 0x1A, 0x8b, false, true));
			m_registers.Add(new Register("RegOokPeak", 0x1B, 0x40, false, true));
			m_registers.Add(new Register("RegOokAvg", 0x1C, 0x80, false, true));
			m_registers.Add(new Register("RegOokFix", 0x1D, 6, false, true));
			m_registers.Add(new Register("RegAfcFei", 0x1E, 0x10, false, true));
			m_registers.Add(new Register("RegAfcMsb", 0x1F, 0, false, true));
			m_registers.Add(new Register("RegAfcLsb", 0x20, 0, false, true));
			m_registers.Add(new Register("RegFeiMsb", 0x21, 0, false, true));
			m_registers.Add(new Register("RegFeiLsb", 0x22, 0, false, true));
			m_registers.Add(new Register("RegRssiConfig", 0x23, 2, true, true));
			m_registers.Add(new Register("RegRssiValue", 0x24, 0xFF, true, true));
			m_registers.Add(new Register("RegDioMapping1", 0x25, 0x00, false, true));
			m_registers.Add(new Register("RegDioMapping2", 0x26, 7, false, true));
			m_registers.Add(new Register("RegIrqFlags1", 0x27, 0x80, true, true));
			m_registers.Add(new Register("RegIrqFlags2", 0x28, 0, true, true));
			m_registers.Add(new Register("RegRssiThresh", 0x29, 0xe4, false, true));
			m_registers.Add(new Register("RegRxTimeout1", 0x2A, 0, false, true));
			m_registers.Add(new Register("RegRxTimeout2", 0x2B, 0, false, true));
			m_registers.Add(new Register("RegPreambleMsb", 0x2C, 0, false, true));
			m_registers.Add(new Register("RegPreambleLsb", 0x2D, 3, false, true));
			m_registers.Add(new Register("RegSyncConfig", 0x2E, 0x98, false, true));
			m_registers.Add(new Register("RegSyncValue1", 0x2F, 0, false, true));
			m_registers.Add(new Register("RegSyncValue2", 0x30, 0, false, true));
			m_registers.Add(new Register("RegSyncValue3", 0x31, 0, false, true));
			m_registers.Add(new Register("RegSyncValue4", 0x32, 0, false, true));
			m_registers.Add(new Register("RegSyncValue5", 0x33, 0, false, true));
			m_registers.Add(new Register("RegSyncValue6", 0x34, 0, false, true));
			m_registers.Add(new Register("RegSyncValue7", 0x35, 0, false, true));
			m_registers.Add(new Register("RegSyncValue8", 0x36, 0, false, true));
			m_registers.Add(new Register("RegPacketConfig1", 0x37, 1, false, true));
			m_registers.Add(new Register("RegPayloadLength", 0x38, 0x40, false, true));
			m_registers.Add(new Register("RegNodeAdrs", 0x39, 0, false, true));
			m_registers.Add(new Register("RegBroadcastAdrs", 0x3A, 0, false, true));
			m_registers.Add(new Register("RegAutoModes", 0x3B, 0, false, true));
			m_registers.Add(new Register("RegFifoThresh", 0x3C, 0x8F, false, true));
			m_registers.Add(new Register("RegPacketConfig2", 0x3D, 2, false, true));
			m_registers.Add(new Register("RegAesKey1", 0x3E, 0, false, true));
			m_registers.Add(new Register("RegAesKey2", 0x3F, 0, false, true));
			m_registers.Add(new Register("RegAesKey3", 0x40, 0, false, true));
			m_registers.Add(new Register("RegAesKey4", 0x41, 0, false, true));
			m_registers.Add(new Register("RegAesKey5", 0x42, 0, false, true));
			m_registers.Add(new Register("RegAesKey6", 0x43, 0, false, true));
			m_registers.Add(new Register("RegAesKey7", 0x44, 0, false, true));
			m_registers.Add(new Register("RegAesKey8", 0x45, 0, false, true));
			m_registers.Add(new Register("RegAesKey9", 0x46, 0, false, true));
			m_registers.Add(new Register("RegAesKey10", 0x47, 0, false, true));
			m_registers.Add(new Register("RegAesKey11", 0x48, 0, false, true));
			m_registers.Add(new Register("RegAesKey12", 0x49, 0, false, true));
			m_registers.Add(new Register("RegAesKey13", 0x4A, 0, false, true));
			m_registers.Add(new Register("RegAesKey14", 0x4B, 0, false, true));
			m_registers.Add(new Register("RegAesKey15", 0x4C, 0, false, true));
			m_registers.Add(new Register("RegAesKey16", 0x4D, 0, false, true));
			m_registers.Add(new Register("RegTemp1", 0x4E, 1, true, true));
			m_registers.Add(new Register("RegTemp2", 0x4F, 0, true, true));

			if (Version != "2.1")
			{
				m_registers.Add(new Register("RegTestLna", 0x58, 0x1b, false, true));
				if (Version == "2.3")
					m_registers.Add(new Register("RegTestDagc", 0x6F, 0x30, false, true));

				m_registers.Add(new Register("RegTestAfc", 0x71, 0, false, true));
			}

			foreach (Register item in m_registers)
				item.PropertyChanged += new PropertyChangedEventHandler(registers_PropertyChanged);

			Packet = new SemtechLib.Devices.SX1231.General.Packet();
		}

		private void PreambleCheck()
		{
		}

		public void RcCalStart()
		{
			lock (syncThread)
			{
				byte data = 0;
				if (Mode == OperatingModeEnum.Stdby)
				{
					if (!Write(0x57, 0x80))
						throw new Exception("Unable to write register at address 0x57.");
					for (int i = 0; i < 2; i++)
					{
						ReadRegister(m_registers["RegOsc1"], ref data);
						WriteRegister(m_registers["RegOsc1"], (byte)(data | 0x80));
						DateTime now = DateTime.Now;
						bool flag = false;
						do
						{
							data = 0;
							ReadRegister(m_registers["RegOsc1"], ref data);
							TimeSpan span = (TimeSpan)(DateTime.Now - now);
							flag = span.TotalMilliseconds >= 1000.0;
						}
						while ((((byte)(data & 0x40)) == 0) && !flag);
						if (flag)
							throw new Exception("RC oscillator calibration timeout.");

					}
					if (!Write(0x57, 0))
						throw new Exception("Unable to write register at address 0x57.");
				}
				else
				{
					MessageBox.Show("The chip must be in Standby mode in order to calibrate the RC oscillator!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					throw new Exception("The chip must be in Standby mode in order to calibrate the RC oscillator!");
				}
			}
		}

		public bool Read(byte address, ref byte data)
		{
			Mpsse portA = ftdi.PortA;
			portA.PortValue = (byte)(portA.PortValue & ~PA3_CS);
			portA.ScanOut(8, new byte[] { (byte)(address & 0x7F) }, true);
			portA.ScanIn(8, true);

			portA.PortValue = (byte)(portA.PortValue | PA3_CS);
			portA.TxBufferAdd(0x87);

			bool flag = portA.SendBytes();
			byte[] rxBuffer = new byte[1];
			if (flag && portA.ReadBytes(out rxBuffer, 8))
			{
				data = rxBuffer[rxBuffer.Length - 1];
				return true;
			}
			return false;
		}

		public bool Read(byte address, ref byte[] data)
		{
			Mpsse portA = ftdi.PortA;

			portA.PortValue = (byte)(portA.PortValue & ~PA3_CS);
			portA.ScanOut(8, new byte[] { (byte)(address & 0x7F) }, true);
			for (int i = 0; i < data.Length; i++)
				portA.ScanIn(8, true);

			portA.PortValue = (byte)(portA.PortValue | PA3_CS);
			portA.TxBufferAdd(0x87);
			bool flag = portA.SendBytes();
			byte[] rxBuffer = new byte[1];
			if (flag && portA.ReadBytes(out rxBuffer, (uint)(data.Length * 8)))
			{
				Array.Copy(rxBuffer, rxBuffer.Length - data.Length, data, 0, data.Length);
				return true;
			}
			data = null;
			return false;
		}

		private bool ReadFifo(ref byte[] data)
		{
			return Read(0x00, ref data);
		}

		private void ReadIrqFlags()
		{
			ReadRegister(m_registers["RegIrqFlags1"]);
			ReadRegister(m_registers["RegIrqFlags2"]);
		}

		private bool ReadRegister(Register r)
		{
			byte data = 0;
			return ReadRegister(r, ref data);
		}

		private bool ReadRegister(Register reg, ref byte data)
		{
			bool flag;
			lock (syncThread)
			{
				try
				{
					readLock++;
					if (!Read((byte)reg.Address, ref data))
						throw new Exception("Unable to read register: " + reg.Name);
					reg.Value = data;
					flag = true;
				}
				catch (Exception exception)
				{
					OnError(1, exception.Message);
					flag = false;
				}
				finally
				{
					readLock--;
				}
			}
			return flag;
		}

		public void ReadRegisters()
		{
			lock (syncThread)
			{
				try
				{
					readLock++;
					foreach (Register register in m_registers)
						if (register.Address != 0)
						{
							if (IsOpen)
							{
								byte data = 0;
								if (!Read((byte)register.Address, ref data))
									throw new Exception("Reading register " + register.Name);
								register.Value = data;
							}
							else
								UpdateRegisterValue(register);
						}
				}
				catch (Exception exception)
				{
					OnError(1, exception.Message);
				}
				finally
				{
					readLock--;
				}
			}
		}

		public bool ReceiveRfData(out byte[] buffer)
		{
			bool flag2;
			object obj2;
			System.Threading.Monitor.Enter(obj2 = syncThread);
			try
			{
				bool flag = false;
				SetOperatingMode(OperatingModeEnum.Sleep, true);
				buffer = FifoData;
				flag = ReadFifo(ref buffer);
				SetOperatingMode(OperatingModeEnum.Rx, true);
				flag2 = flag;
			}
			catch (Exception exception)
			{
				throw exception;
			}
			finally
			{
				System.Threading.Monitor.Exit(obj2);
			}
			return flag2;
		}

		private void registers_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			lock (syncThread)
			{
				Register register = (Register)sender;

				if (e.PropertyName == "Value")
				{
					UpdateRegisterValue(register);
					if (readLock == 0 && !Write((byte)register.Address, (byte)register.Value))
						OnError(1, "Unable to write register " + register.Name);

					if (register.Name == "RegOpMode")
					{
						if (Mode == OperatingModeEnum.Rx)
						{
							ReadRegister(m_registers["RegLna"]);
							ReadRegister(m_registers["RegFeiMsb"]);
							ReadRegister(m_registers["RegFeiLsb"]);
							ReadRegister(m_registers["RegAfcMsb"]);
							ReadRegister(m_registers["RegAfcLsb"]);
							ReadRegister(m_registers["RegRssiValue"]);
						}
						ReadIrqFlags();
					}
				}
			}
		}

		private void RegUpdateThread()
		{
			int retries = 0;
			while (regUpdateThreadContinue)
			{
				if (!ftdi.IsOpen)
				{
					Application.DoEvents();
					Thread.Sleep(10);
				}
				else
				{
					try
					{
						lock (syncThread)
						{
							if (!monitor)
							{
								Thread.Sleep(10);
								continue;
							}
							if ((retries % 10) == 0)
							{
								ReadIrqFlags();
								if (!SpectrumOn)
								{
									if (RfPaSwitchEnabled == 2)
									{
										RfPaSwitchSel = RfPaSwitchSelEnum.RF_IO_RFIO;
										SetRssiStart();
										RfPaSwitchSel = RfPaSwitchSelEnum.RF_IO_PA_BOOST;
										SetRssiStart();
									}
									else
										SetRssiStart();
								}
								else
									SpectrumProcess();

								if (TempCalDone && ((Mode == OperatingModeEnum.Stdby) || (Mode == OperatingModeEnum.Fs)))
								{
									tempMeasRunning = false;
									OnPropertyChanged("TempMeasRunning");
								}
							}
							if (retries >= 200)
							{
								if (restartRx)
								{
									restartRx = false;
									ReadRegister(m_registers["RegLna"]);
									ReadRegister(m_registers["RegFeiMsb"]);
									ReadRegister(m_registers["RegFeiLsb"]);
									ReadRegister(m_registers["RegAfcMsb"]);
									ReadRegister(m_registers["RegAfcLsb"]);
								}
								if (TempCalDone && ((Mode == OperatingModeEnum.Stdby) || (Mode == OperatingModeEnum.Fs)))
								{
									tempMeasRunning = true;
									OnPropertyChanged("TempMeasRunning");
								}
								SetTempMeasStart(false);
								retries = 0;
							}
						}
					}
					catch { }
					retries++;
					Thread.Sleep(1);
				}
			}
		}

		#region Reset() 
		/// <summary>
		/// Reset module
		/// </summary>
		public void Reset()
		{
			object sync;
			System.Threading.Monitor.Enter(sync = syncThread);
			try
			{
				bool spectrumOn = SpectrumOn;
				if (SpectrumOn)
					SpectrumOn = false;

				tempCalDone = false;
				PacketHandlerStop();

				Mpsse portA = ftdi.PortA;
				portA.PortDir = (byte)(portA.PortDir | PA5_RESET);
				portA.PortValue = (byte)(portA.PortValue | PA5_RESET);

				if (!portA.SendBytes())
					throw new Exception("Unable to send bytes over USB device");

				Thread.Sleep(1);

				portA.PortDir = (byte)(portA.PortDir & ~PA5_RESET);
				portA.PortValue = (byte)(portA.PortValue & ~PA5_RESET);

				if (!portA.SendBytes())
					throw new Exception("Unable to send bytes over USB device");

				Thread.Sleep(5);

				ReadRegisters();
				if (Version == "2.1")
					RcCalStart();

				SetDefaultValues();
				ReadRegisters();
				RfPaSwitchEnabled = 0;
				RfPaSwitchSel = RfPaSwitchSelEnum.RF_IO_RFIO;
				if (spectrumOn)
					SpectrumOn = true;
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
			finally
			{
				System.Threading.Monitor.Exit(sync);
			}
		}
		#endregion

		public void Save(ref FileStream stream)
		{
			OnError(0, "-");
			StreamWriter writer = new StreamWriter(stream, Encoding.ASCII);
			try
			{
				writer.WriteLine("#Type\tRegister Name\tAddress[Hex]\tValue[Hex]");
				for (int i = 0; i < m_registers.Count; i++)
					writer.WriteLine(
						"REG\t{0}\t0x{1}\t0x{2}",
						m_registers[i].Name,
						m_registers[i].Address.ToString("X02"),
						m_registers[i].Value.ToString("X02")
						);
				writer.WriteLine("PKT\t{0}", m_packet.GetSaveData());
			}
			catch (Exception exception)
			{
				throw exception;
			}
			finally
			{
				writer.Close();
			}
		}

		public void SendData(byte[] buffer)
		{
			throw new NotImplementedException();
		}

		public void SetAdcLowPowerOn(bool value)
		{
			try
			{
				lock (syncThread)
					m_registers["RegTemp1"].Value = (m_registers["RegTemp1"].Value & 0xFE) | (uint)(value ? 1 : 0);
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetAddressFiltering(AddressFilteringEnum value)
		{
			try
			{
				lock (syncThread)
					m_registers["RegPacketConfig1"].Value = (m_registers["RegPacketConfig1"].Value & 0xF9) | (((uint)value & 0x03) << 1);
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetAesKey(byte[] value)
		{
			try
			{
				lock (syncThread)
				{
					int address = (int)m_registers["RegAesKey1"].Address;
					for (int i = 0; i < value.Length; i++)
						m_registers[address + i].Value = value[i];
				}
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetAesOn(bool value)
		{
			try
			{
				lock (syncThread)
					m_registers["RegPacketConfig2"].Value = (m_registers["RegPacketConfig2"].Value & 0xFE) | (uint)(value ? 1 : 0);
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetAfcAutoClearOn(bool value)
		{
			try
			{
				lock (syncThread)
					m_registers["RegAfcFei"].Value = (m_registers["RegAfcFei"].Value & 0xF7) | (uint)(value ? 8 : 0);
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetAfcAutoOn(bool value)
		{
			try
			{
				lock (syncThread)
				{
					byte num = (byte)m_registers["RegAfcFei"].Value;
					num = (byte)(num & 0xfb);
					num = (byte)(num | (value ? ((byte)4) : ((byte)0)));
					m_registers["RegAfcFei"].Value = num;
				}
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetAfcClear()
		{
			try
			{
				lock (syncThread)
				{
					m_registers["RegAfcFei"].Value = m_registers["RegAfcFei"].Value | 2;
					ReadRegister(m_registers["RegAfcMsb"]);
					ReadRegister(m_registers["RegAfcLsb"]);
				}
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetAfcDccFreq(decimal value)
		{
			try
			{
				lock (syncThread)
					m_registers["RegAfcBw"].Value = (m_registers["RegAfcBw"].Value & 0x1F) |
						((uint)((Math.Log10((double)((4.0M * AfcRxBw) / (6.283185307179580M * value))) / Math.Log10(2.0)) - 2.0) << 5);
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetAfcLowBetaOn(bool value)
		{
			if (Version != "2.1")
			{
				try
				{
					lock (syncThread)
						m_registers["RegAfcCtrl"].Value = (m_registers["RegAfcCtrl"].Value & 0xDF) | (uint)(value ? 0x20 : 0);
				}
				catch (Exception exception)
				{
					OnError(1, exception.Message);
				}
			}
		}

		public void SetAfcRxBw(decimal value)
		{
			try
			{
				lock (syncThread)
				{
					uint num = m_registers["RegAfcBw"].Value & 0xE0;
					int exp = 0;
					int mant = 0;
					ComputeRxBwMantExp(frequencyXo, ModulationType, value, ref mant, ref exp);
					switch (mant)
					{
						case 0x10:
							num |= (uint)(0x00 | exp & 7);
							break;

						case 20:
							num |= (uint)(0x08 | (exp & 7));
							break;

						case 0x18:
							num |= (uint)(0x10 | (exp & 7));
							break;

						default:
							throw new Exception("Invalid RxBwMant parameter");
					}
					m_registers["RegAfcBw"].Value = num;
				}
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetAfcStart()
		{
			lock (syncThread)
			{
				byte data = 0;
				ReadRegister(m_registers["RegAfcFei"], ref data);
				WriteRegister(m_registers["RegAfcFei"], (byte)(data | 1));
				DateTime now = DateTime.Now;
				bool flag = false;
				do
				{
					data = 0;
					ReadRegister(m_registers["RegAfcFei"], ref data);
					TimeSpan span = (TimeSpan)(DateTime.Now - now);
					flag = span.TotalMilliseconds >= 1000.0;
				}

				while (((data & 0x10) == 0) && !flag);

				if (flag)
					OnError(1, "AFC read timeout.");
				else
				{
					ReadRegister(m_registers["RegAfcMsb"]);
					ReadRegister(m_registers["RegAfcLsb"]);
				}
			}
		}

		public void SetAgcAutoRefOn(bool value)
		{
			try
			{
				lock (syncThread)
				{
					uint num;
					if (Version == "2.1")
						num = m_registers["RegAgcRef"].Value;
					else
						num = m_registers["Reserved14"].Value;

					num &= 0xBF;
					num |= (uint)(value ? 0x40 : 0);

					if (Version == "2.1")
						m_registers["RegAgcRef"].Value = num;
					else
						m_registers["Reserved14"].Value = num;
				}
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetAgcRefLevel(int value)
		{
			try
			{
				lock (syncThread)
				{
					uint num;
					if (Version == "2.1")
						num = m_registers["RegAgcRef"].Value;
					else
						num = m_registers["Reserved14"].Value;

					num &= 0xC0;
					num |= (uint)((-value - 80) & 0x3F);

					if (Version == "2.1")
						m_registers["RegAgcRef"].Value = num;
					else
						m_registers["Reserved14"].Value = num;
				}
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetAgcSnrMargin(byte value)
		{
			try
			{
				lock (syncThread)
				{
					uint num;
					if (Version == "2.1")
						num = (byte)m_registers["RegAgcThresh1"].Value;
					else
						num = (byte)m_registers["Reserved15"].Value;
					num &= 0x1F;
					num |= (uint)((value & 7) << 5);
					if (Version == "2.1")
						m_registers["RegAgcThresh1"].Value = num;
					else
						m_registers["Reserved15"].Value = num;
				}
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetAgcStep(byte id, byte value)
		{
			try
			{
				lock (syncThread)
				{
					Register register;
					switch (id)
					{
						case 1:
							if (Version == "2.1")
								register = m_registers["RegAgcThresh1"];
							else
								register = m_registers["Reserved15"];
							break;

						case 2:
						case 3:
							if (Version == "2.1")
								register = m_registers["RegAgcThresh2"];
							else
								register = m_registers["Reserved16"];
							break;

						case 4:
						case 5:
							if (Version == "2.1")
								register = m_registers["RegAgcThresh3"];
							else
								register = m_registers["Reserved17"];
							break;

						default:
							throw new Exception("Invalid AGC step ID!");
					}
					uint num = register.Value;
					switch (id)
					{
						case 1:
							num &= 0xE0;
							num |= (uint)value;
							break;

						case 2:
							num &= 0x0F;
							num |= (uint)(value << 4);
							break;

						case 3:
							num &= 0xF0;
							num |= (uint)(value & 0x0F);
							break;

						case 4:
							num &= 0x0F;
							num |= (uint)(value << 4);
							break;

						case 5:
							num &= 0xF0;
							num |= (uint)(value & 0x0F);
							break;

						default:
							throw new Exception("Invalid AGC step ID!");
					}
					register.Value = num;
				}
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetAutoRxRestartOn(bool value)
		{
			try
			{
				lock (syncThread)
					m_registers["RegPacketConfig2"].Value = (m_registers["RegPacketConfig2"].Value & 0xFD) | (uint)(value ? 2 : 0);
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetBitRate(decimal value)
		{
			try
			{
				lock (syncThread)
				{
					uint msb = (uint)(((long)Math.Round(frequencyXo / value, 1)) >> 8);
					uint lsb = (uint)((long)Math.Round(frequencyXo / value, MidpointRounding.AwayFromZero));
					bitRateFdevCheckDisbale = true;
					m_registers["RegBitrateMsb"].Value = msb;
					bitRateFdevCheckDisbale = false;
					m_registers["RegBitrateLsb"].Value = lsb;
				}
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetBroadcastAddress(byte value)
		{
			try
			{
				lock (syncThread)
					m_registers["RegBroadcastAdrs"].Value = value;
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetClockOut(ClockOutEnum value)
		{
			try
			{
				lock (syncThread)
					m_registers["RegDioMapping2"].Value = (m_registers["RegDioMapping2"].Value & 0xF8) | (uint)(value & ClockOutEnum.CLOCK_OUT_111);
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetCrcAutoClearOff(bool value)
		{
			try
			{
				lock (syncThread)
					m_registers["RegPacketConfig1"].Value = (m_registers["RegPacketConfig1"].Value & 0xF7) | (uint)(value ? 8 : 0);
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetCrcEnable(bool value)
		{
			try
			{
				m_packet.CrcOn = value;
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetCrcOn(bool value)
		{
			try
			{
				lock (syncThread)
					m_registers["RegPacketConfig1"].Value = (m_registers["RegPacketConfig1"].Value & 0xEF) | (uint)(value ? 0x10 : 0);
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetDagcOn(bool value)
		{
			try
			{
				lock (syncThread)
				{
					m_registers["RegTestDagc"].Value =
						(AfcLowBetaOn
							? (uint)(value ? 0x10 : 0)
							: (uint)(value ? 0x30 : 0)
						); ;
				}
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetDataMode(DataModeEnum value)
		{
			try
			{
				lock (syncThread)
					m_registers["RegDataModul"].Value = (m_registers["RegDataModul"].Value & 0x9F) | ((uint)value << 5);
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetDccFreq(decimal value)
		{
			try
			{
				lock (syncThread)
					m_registers["RegRxBw"].Value = (m_registers["RegRxBw"].Value & 0x1F) | ((uint)((Math.Log10((double)((double)((4.0M * RxBw) / (6.283185307179580M * value)))) / Math.Log10(2.0)) - 2.0) << 5);
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetDcFree(DcFreeEnum value)
		{
			try
			{
				lock (syncThread)
					m_registers["RegPacketConfig1"].Value = (m_registers["RegPacketConfig1"].Value & 0x9F) | (((uint)value & 3) << 5);
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetDefaultValues()
		{
			TempCalDone = false;
			if (IsOpen)
			{
				if (Version == "2.1")
				{
					if (!Write((byte)m_registers["RegListen1"].Address, (byte)0xa2))
						throw new Exception("Unable to write register: " + m_registers["RegListen1"].Name);
					if (!Write((byte)m_registers["RegOcp"].Address, (byte)0x1b))
						throw new Exception("Unable to write register: " + m_registers["RegOcp"].Name);
				}
				if (!Write((byte)m_registers["RegLna"].Address, new byte[] { 0x88, 0x55, 0x8b }))
					throw new Exception("Unable to write register: " + m_registers["RegLna"].Name);
				if (!Write((byte)m_registers["RegDioMapping2"].Address, (byte)7))
					throw new Exception("Unable to write register: " + m_registers["RegDioMapping2"].Name);
				if (!Write((byte)m_registers["RegRssiThresh"].Address, (byte)0xe4))
					throw new Exception("Unable to write register: " + m_registers["RegRssiThresh"].Name);
				if (!Write((byte)m_registers["RegSyncValue1"].Address, new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 }))
					throw new Exception("Unable to write register: " + m_registers["RegSyncValue1"].Name);
				if (!Write((byte)m_registers["RegFifoThresh"].Address, (byte)0x8f))
					throw new Exception("Unable to write register: " + m_registers["RegFifoThresh"].Name);
				if ((Version == "2.3") && !Write((byte)m_registers["RegTestDagc"].Address, (byte)0x30))
					throw new Exception("Unable to write register: " + m_registers["RegTestDagc"].Name);
				if ((Version == "2.1") && !Write(110, (byte)12))
					throw new Exception("Unable to write register at address 0x6E ");
			}
			else
			{
				m_registers["RegLna"].Value = 0x88;
				m_registers["RegRxBw"].Value = 0x55;
				m_registers["RegAfcBw"].Value = 0x8b;
				m_registers["RegDioMapping2"].Value = 7;
				m_registers["RegRssiThresh"].Value = 0xe4;
				m_registers["RegSyncValue1"].Value = 1;
				m_registers["RegSyncValue2"].Value = 1;
				m_registers["RegSyncValue3"].Value = 1;
				m_registers["RegSyncValue4"].Value = 1;
				m_registers["RegSyncValue5"].Value = 1;
				m_registers["RegSyncValue6"].Value = 1;
				m_registers["RegSyncValue7"].Value = 1;
				m_registers["RegSyncValue8"].Value = 1;
				m_registers["RegFifoThresh"].Value = 0x8f;
				if (Version == "2.3")
					m_registers["RegTestDagc"].Value = 0x30;
				ReadRegisters();
			}
		}

		public void SetDioMapping(byte id, DioMappingEnum value)
		{
			try
			{
				lock (syncThread)
				{
					Register register;
					switch (id)
					{
						case 0:
						case 1:
						case 2:
						case 3:
							register = m_registers["RegDioMapping1"];
							break;

						case 4:
						case 5:
							register = m_registers["RegDioMapping2"];
							break;

						default:
							throw new Exception("Invalid DIO ID!");
					}
					uint num = (byte)register.Value;
					switch (id)
					{
						case 0:
							num &= 0x3f;
							num |= ((uint)value) << 6;
							break;

						case 1:
							num &= 0xcf;
							num |= ((uint)value) << 4;
							break;

						case 2:
							num &= 0xf3;
							num |= ((uint)value) << 2;
							break;

						case 3:
							num &= 0xfc;
							num = (uint)(((DioMappingEnum)num) | (value & DioMappingEnum.DIO_MAP_11));
							break;

						case 4:
							num &= 0x3f;
							num |= ((uint)value) << 6;
							break;

						case 5:
							num &= 0xcf;
							num |= ((uint)value) << 4;
							break;

						default:
							throw new Exception("Invalid DIO ID!");
					}
					register.Value = num;
				}
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetEnterCondition(EnterConditionEnum value)
		{
			try
			{
				lock (syncThread)
				{
					byte num = (byte)m_registers["RegAutoModes"].Value;
					num = (byte)(num & 0x1f);
					num = (byte)(num | ((byte)((((byte)value) & 7) << 5)));
					m_registers["RegAutoModes"].Value = num;
				}
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetExitCondition(ExitConditionEnum value)
		{
			try
			{
				lock (syncThread)
				{
					byte num = (byte)m_registers["RegAutoModes"].Value;
					num = (byte)(num & 0xe3);
					num = (byte)(num | ((byte)((((byte)value) & 7) << 2)));
					m_registers["RegAutoModes"].Value = num;
				}
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetFastRx(bool value)
		{
			try
			{
				lock (syncThread)
				{
					byte num = (byte)m_registers["RegAfcFei"].Value;
					num = (byte)(num & 0xf7);
					num = (byte)(num | (value ? ((byte)8) : ((byte)0)));
					m_registers["RegAfcFei"].Value = num;
				}
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetFdev(decimal value)
		{
			try
			{
				lock (syncThread)
				{
					byte msb = (byte)m_registers["RegFdevMsb"].Value;
					byte lsb = (byte)m_registers["RegFdevLsb"].Value;
					msb = (byte)(((long)(value / frequencyStep)) >> 8);
					lsb = (byte)((long)(value / frequencyStep));
					bitRateFdevCheckDisbale = true;
					m_registers["RegFdevMsb"].Value = msb;
					bitRateFdevCheckDisbale = false;
					m_registers["RegFdevLsb"].Value = lsb;
				}
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetFeiStart()
		{
			lock (syncThread)
			{
				byte data = 0;
				ReadRegister(m_registers["RegAfcFei"], ref data);
				WriteRegister(m_registers["RegAfcFei"], (byte)(data | 0x20));
				DateTime now = DateTime.Now;
				bool flag = false;
				do
				{
					data = 0;
					ReadRegister(m_registers["RegAfcFei"], ref data);
					TimeSpan span = (TimeSpan)(DateTime.Now - now);
					flag = span.TotalMilliseconds >= 1000.0;
				}
				while ((((byte)(data & 0x40)) == 0) && !flag);
				if (flag)
				{
					OnError(1, "FEI read timeout.");
				}
				else
				{
					ReadRegister(m_registers["RegFeiMsb"]);
					ReadRegister(m_registers["RegFeiLsb"]);
				}
			}
		}

		public void SetFifoFillCondition(FifoFillConditionEnum value)
		{
			try
			{
				lock (syncThread)
					m_registers["RegSyncConfig"].Value = (m_registers["RegSyncConfig"].Value & 0xBF) | (uint)(value == FifoFillConditionEnum.Allways ? 0x40 : 0);
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetFifoThreshold(byte value)
		{
			try
			{
				lock (syncThread)
				{
					byte num = (byte)m_registers["RegFifoThresh"].Value;
					num = (byte)(num & 0x80);
					num = (byte)(num | ((byte)(value & 0x7f)));
					m_registers["RegFifoThresh"].Value = num;
				}
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetFrequencyRf(decimal value)
		{
			try
			{
				lock (syncThread)
				{
					byte msb = (byte)m_registers["RegFrfMsb"].Value;
					byte mid = (byte)m_registers["RegFrfMid"].Value;
					byte lsb = (byte)m_registers["RegFrfLsb"].Value;

					msb = (byte)(((long)(value / frequencyStep)) >> 0x10);
					mid = (byte)(((long)(value / frequencyStep)) >> 8);
					lsb = (byte)((long)(value / frequencyStep));

					frequencyRfCheckDisable = true;
					m_registers["RegFrfMsb"].Value = msb;
					m_registers["RegFrfMid"].Value = mid;
					frequencyRfCheckDisable = false;
					m_registers["RegFrfLsb"].Value = lsb;
				}
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetIntermediateMode(IntermediateModeEnum value)
		{
			try
			{
				lock (syncThread)
					m_registers["RegAutoModes"].Value = (m_registers["RegAutoModes"].Value & 0xFC) | ((uint)value & 3);
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetInterPacketRxDelay(int value)
		{
			try
			{
				lock (syncThread)
					m_registers["RegPacketConfig2"].Value = (m_registers["RegPacketConfig2"].Value & 0x0F) | ((uint)value << 4);
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetListenCoefIdle(decimal value)
		{
			try
			{
				lock (syncThread)
				{
					byte num = (byte)m_registers["RegListen2"].Value;
					switch (ListenResolIdle)
					{
						case ListenResolEnum.Res000064:
							num = (byte)(value / 0.064M);
							break;

						case ListenResolEnum.Res004100:
							num = (byte)(value / 4.1M);
							break;

						case ListenResolEnum.Res262000:
							num = (byte)(value / 262M);
							break;
					}
					m_registers["RegListen2"].Value = num;
				}
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetListenCoefRx(decimal value)
		{
			try
			{
				lock (syncThread)
				{
					byte num = (byte)m_registers["RegListen3"].Value;
					switch (ListenResolRx)
					{
						case ListenResolEnum.Res000064:
							num = (byte)(value / 0.064M);
							break;

						case ListenResolEnum.Res004100:
							num = (byte)(value / 4.1M);
							break;

						case ListenResolEnum.Res262000:
							num = (byte)(value / 262M);
							break;
					}
					m_registers["RegListen3"].Value = num;
				}
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetListenCriteria(ListenCriteriaEnum value)
		{
			try
			{
				lock (syncThread)
					m_registers["RegListen1"].Value = (m_registers["RegListen1"].Value & 0xF7) | (uint)(value == ListenCriteriaEnum.RssiThresh ? 0 : 8);
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetListenEnd(ListenEndEnum value)
		{
			try
			{
				lock (syncThread)
					m_registers["RegListen1"].Value = (m_registers["RegListen1"].Value & 0xF9) | ((uint)value << 1);
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetListenMode(bool value)
		{
			try
			{
				if (Mode == OperatingModeEnum.Sleep)
					SetOperatingMode(OperatingModeEnum.Stdby);

				m_registers["RegOpMode"].Value = (m_registers["RegOpMode"].Value & 0x9F) | (uint)(value ? 0x40 : 0x20);

				ReadRegister(m_registers["RegOpMode"]);
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetListenResolIdle(ListenResolEnum value)
		{
			try
			{
				lock (syncThread)
				{
					byte num = (byte)m_registers["RegListen1"].Value;
					num = (byte)(num & 0x3f);
					num = (byte)(num | ((byte)(((int)(value + 1)) << 6)));
					if (Version == "2.1")
					{
						num = (byte)(num & 0xcf);
						num = (byte)(num | ((byte)(((int)(value + 1)) << 4)));
					}
					m_registers["RegListen1"].Value = num;
				}
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetListenResolRx(ListenResolEnum value)
		{
			try
			{
				lock (syncThread)
					m_registers["RegListen1"].Value = (m_registers["RegListen1"].Value & 0xCF) | ((uint)(value + 1) << 4);
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetLnaGainSelect(LnaGainEnum value)
		{
			try
			{
				lock (syncThread)
				{
					m_registers["RegLna"].Value = (m_registers["RegLna"].Value & 0xF8) | (uint)value;
					if (LnaGainSelect != LnaGainEnum.AGC)
						ReadRegister(m_registers["RegLna"]);
				}
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetLnaLowPowerOn(bool value)
		{
			try
			{
				lock (syncThread)
					m_registers["RegLna"].Value = (m_registers["RegLna"].Value & 0xBF) | (uint)(value ? 0x40 : 0);
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetLnaZin(LnaZinEnum value)
		{
			try
			{
				lock (syncThread)
					m_registers["RegLna"].Value = (m_registers["RegLna"].Value & 0x7F) | (uint)(value == LnaZinEnum.ZIN_200 ? 0x80 : 0);
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetLowBatOn(bool value)
		{
			try
			{
				lock (syncThread)
					m_registers["RegLowBat"].Value = (m_registers["RegLowBat"].Value & 0xF7) | (uint)(value ? 8 : 0);
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetLowBatTrim(LowBatTrimEnum value)
		{
			try
			{
				lock (syncThread)
					m_registers["RegLowBat"].Value = (m_registers["RegLowBat"].Value & 0xf8) | (uint)value;
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetLowBetaAfcOffset(decimal value)
		{
			try
			{
				lock (syncThread)
					m_registers["RegTestAfc"].Value = (byte)((sbyte)(value / 488.0M));
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetMaxPacketNumber(int value)
		{
			try
			{
				lock (syncThread)
					maxPacketNumber = value;
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetMessage(byte[] value)
		{
			try
			{
				lock (syncThread)
					m_packet.Message = value;
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetMessageLength(int value)
		{
			try
			{
				lock (syncThread) { }
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		private void SetModeLeds(OperatingModeEnum mode)
		{
			if (!test)
			{
				IoPort portB = ftdi.PortB;
				portB.PortValue = (byte)(portB.PortValue & ~PB_LEDS);
				switch (mode)
				{
					case OperatingModeEnum.Tx:
						if (!isPacketModeRunning)
						{
							portB.PortValue = (byte)(portB.PortValue | PB_LEDS);
							break;
						}
						portB.PortValue = (byte)(portB.PortValue | PB6_LED1);
						break;
					case OperatingModeEnum.Rx:
						portB.PortValue = (byte)(portB.PortValue | PB7_LED2);
						break;
					default:
						break;
				}
				ftdi.PortB.SendBytes();
			}
		}

		public void SetModulationShaping(byte value)
		{
			try
			{
				lock (syncThread)
					m_registers["RegDataModul"].Value = (m_registers["RegDataModul"].Value & 0xFC) | (uint)value;
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetModulationType(ModulationTypeEnum value)
		{
			try
			{
				lock (syncThread)
					m_registers["RegDataModul"].Value = (m_registers["RegDataModul"].Value & 0xE7) | ((uint)value << 3);
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetNodeAddress(byte value)
		{
			try
			{
				lock (syncThread)
					m_registers["RegNodeAdrs"].Value = value;
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetOcpOn(bool value)
		{
			try
			{
				lock (syncThread)
					m_registers["RegOcp"].Value = (m_registers["RegOcp"].Value & 0xEF) | (uint)(value ? 0x10 : 0);
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetOcpTrim(decimal value)
		{
			try
			{
				lock (syncThread)
					m_registers["RegOcp"].Value = (m_registers["RegOcp"].Value & 0xF0) | ((uint)((value - 45M) / 5M) & 0x0F);
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetOokAverageThreshFilt(OokAverageThreshFiltEnum value)
		{
			try
			{
				lock (syncThread)
					m_registers["RegOokAvg"].Value = (m_registers["RegOokAvg"].Value & 0x3F) | (((uint)value & 3) << 6);
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetOokFixedThresh(byte value)
		{
			try
			{
				lock (syncThread)
					m_registers["RegOokFix"].Value = value;
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetOokPeakThreshDec(OokPeakThreshDecEnum value)
		{
			try
			{
				lock (syncThread)
					m_registers["RegOokPeak"].Value = (m_registers["RegOokPeak"].Value & 0xF8) | ((uint)value & 7);
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetOokPeakThreshStep(decimal value)
		{
			try
			{
				lock (syncThread)
					m_registers["RegOokPeak"].Value = (m_registers["RegOokPeak"].Value & 0xC7) | (((uint)Array.IndexOf<decimal>(OoPeakThreshStepTable, value) & 7) << 3);
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetOokThreshType(OokThreshTypeEnum value)
		{
			try
			{
				lock (syncThread)
					m_registers["RegOokPeak"].Value = (m_registers["RegOokPeak"].Value & 0x3f) | (((uint)value & 3) << 6);
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetOperatingMode(OperatingModeEnum value)
		{
			SetOperatingMode(value, false);
		}

		public void SetOperatingMode(OperatingModeEnum value, bool isQuiet)
		{
			try
			{
				byte data = (byte)m_registers["RegOpMode"].Value;
				data = (byte)(data & 0xe3);
				data = (byte)(data | ((byte)(((byte)value) << 2)));
				if (!isQuiet)
				{
					m_registers["RegOpMode"].Value = data;
				}
				else
				{
					lock (syncThread)
					{
						if (!Write((byte)m_registers["RegOpMode"].Address, data))
						{
							throw new Exception("Unable to write register " + m_registers["RegOpMode"].Name);
						}
						if (Mode == OperatingModeEnum.Rx)
						{
							ReadRegister(m_registers["RegLna"]);
							ReadRegister(m_registers["RegFeiMsb"]);
							ReadRegister(m_registers["RegFeiLsb"]);
							ReadRegister(m_registers["RegAfcMsb"]);
							ReadRegister(m_registers["RegAfcLsb"]);
						}
					}
				}
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetOutputPower(decimal value)
		{
			try
			{
				lock (syncThread)
				{
					byte num = (byte)m_registers["RegPaLevel"].Value;
					num = (byte)(num & 0xe0);
					if (PaMode != PaModeEnum.PA1_PA2)
					{
						if (value > 13M)
						{
							value = 13M;
						}
						num = (byte)(num | ((byte)(((uint)(value + 18M)) & 0x1f)));
					}
					else
					{
						if (value < -14M)
						{
							value = -14M;
						}
						num = (byte)(num | ((byte)(((uint)(value + 14M)) & 0x1f)));
					}
					m_registers["RegPaLevel"].Value = num;
				}
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetPacketFormat(PacketFormatEnum value)
		{
			try
			{
				lock (syncThread)
					m_registers["RegPacketConfig1"].Value = (m_registers["RegPacketConfig1"].Value & 0x7F) | (uint)(value == PacketFormatEnum.Variable ? 0x80 : 0);
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetPacketHandlerLogEnable(bool value)
		{
			try
			{
				lock (syncThread)
				{
					m_packet.LogEnabled = value;
				}
			}
			catch (Exception exception)
			{
				m_packet.LogEnabled = false;
				OnError(1, exception.Message);
			}
		}

		public void SetPacketHandlerStartStop(bool value)
		{
			try
			{
				lock (syncThread)
				{
					if (value)
					{
						PacketHandlerStart();
					}
					else
					{
						PacketHandlerStop();
					}
				}
			}
			catch (Exception exception)
			{
				PacketHandlerStop();
				OnError(1, exception.Message);
			}
		}

		public void SetPaMode(PaModeEnum value)
		{
			try
			{
				lock (syncThread)
				{
					byte num = (byte)m_registers["RegPaLevel"].Value;
					num = (byte)(num & 0x1f);
					switch (value)
					{
						case PaModeEnum.PA0:
							num = (byte)(num | 0x80);
							break;

						case PaModeEnum.PA1:
							num = (byte)(num | 0x40);
							break;

						case PaModeEnum.PA1_PA2:
							num = (byte)(num | 0x60);
							break;

						default:
							num = (byte)(num | 0x80);
							break;
					}
					m_registers["RegPaLevel"].Value = num;
				}
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetPaRamp(PaRampEnum value)
		{
			try
			{
				lock (syncThread)
				{
					byte num = (byte)m_registers["RegPaRamp"].Value;
					num = (byte)(num & 240);
					num = (byte)(num | ((byte)(((byte)value) & 15)));
					m_registers["RegPaRamp"].Value = num;
				}
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetPayloadLength(byte value)
		{
			try
			{
				lock (syncThread)
				{
					m_registers["RegPayloadLength"].Value = value;
				}
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetPreambleSize(int value)
		{
			try
			{
				lock (syncThread)
				{
					m_registers["RegPreambleMsb"].Value = (byte)(value >> 8);
					m_registers["RegPreambleLsb"].Value = (byte)value;
				}
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetRestartRx()
		{
			try
			{
				lock (syncThread)
				{
					byte num = (byte)m_registers["RegPacketConfig2"].Value;
					num = (byte)(num & 0xfb);
					num = (byte)(num | 4);
					m_registers["RegPacketConfig2"].Value = num;
					restartRx = true;
				}
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetRssiStart()
		{
			lock (syncThread)
			{
				if ((Mode == OperatingModeEnum.Rx) && RxReady)
				{
					byte data = 0;
					ReadRegister(m_registers["RegRssiConfig"], ref data);
					WriteRegister(m_registers["RegRssiConfig"], (byte)(data | 1));
					ReadRegister(m_registers["RegRssiConfig"]);
					ReadRegister(m_registers["RegRssiValue"]);
				}
			}
		}

		public void SetRssiThresh(decimal value)
		{
			try
			{
				lock (syncThread)
				{
					m_registers["RegRssiThresh"].Value = (uint)(-value * 2M);
				}
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetRxBw(decimal value)
		{
			try
			{
				lock (syncThread)
				{
					byte num = (byte)m_registers["RegRxBw"].Value;
					num = (byte)(num & 0xe0);
					int exp = 0;
					int mant = 0;
					ComputeRxBwMantExp(frequencyXo, ModulationType, value, ref mant, ref exp);
					switch (mant)
					{
						case 0x10:
							num = (byte)(num | ((byte)(exp & 7)));
							break;

						case 20:
							num = (byte)(num | ((byte)(8 | (exp & 7))));
							break;

						case 0x18:
							num = (byte)(num | ((byte)(0x10 | (exp & 7))));
							break;

						default:
							throw new Exception("Invalid RxBwMant parameter");
					}
					m_registers["RegRxBw"].Value = num;
				}
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetSensitivityBoostOn(bool value)
		{
			try
			{
				lock (syncThread)
				{
					byte num = (byte)m_registers["RegTestLna"].Value;
					num = value ? ((byte)0x2d) : ((byte)0x1b);
					m_registers["RegTestLna"].Value = num;
				}
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetSequencer(bool value)
		{
			try
			{
				lock (syncThread)
				{
					byte num = (byte)m_registers["RegOpMode"].Value;
					num = (byte)(num & 0x7f);
					num = (byte)(num | (value ? ((byte)0) : ((byte)0x80)));
					m_registers["RegOpMode"].Value = num;
				}
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetSyncOn(bool value)
		{
			try
			{
				lock (syncThread)
				{
					byte num = (byte)m_registers["RegSyncConfig"].Value;
					num = (byte)(num & 0x7f);
					num = (byte)(num | (value ? ((byte)0x80) : ((byte)0)));
					m_registers["RegSyncConfig"].Value = num;
				}
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetSyncSize(byte value)
		{
			try
			{
				lock (syncThread)
				{
					byte num = (byte)m_registers["RegSyncConfig"].Value;
					num = (byte)(num & 0xc7);
					num = (byte)(num | ((byte)(((value - 1) & 7) << 3)));
					m_registers["RegSyncConfig"].Value = num;
				}
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetSyncTol(byte value)
		{
			try
			{
				lock (syncThread)
				{
					byte num = (byte)m_registers["RegSyncConfig"].Value;
					num = (byte)(num & 0xf8);
					num = (byte)(num | ((byte)(value & 7)));
					m_registers["RegSyncConfig"].Value = num;
				}
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetSyncValue(byte[] value)
		{
			try
			{
				lock (syncThread)
				{
					byte address = (byte)m_registers["RegSyncValue1"].Address;
					for (int i = 0; i < value.Length; i++)
					{
						m_registers[address + i].Value = value[i];
					}
				}
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetTempCalibrate(decimal tempRoomValue)
		{
			lock (syncThread)
			{
				TempCalDone = false;
				TempValueRoom = tempRoomValue;
				SetTempMeasStart(true);
				TempValueCal = m_registers["RegTemp2"].Value;
				SetTempMeasStart(false);
				TempCalDone = true;
			}
		}

		public void SetTempMeasStart(bool calibrating)
		{
			lock (syncThread)
			{
				if ((calibrating || TempCalDone) && ((Mode == OperatingModeEnum.Stdby) || (Mode == OperatingModeEnum.Fs)))
				{
					byte data = 0;
					ReadRegister(m_registers["RegTemp1"], ref data);
					WriteRegister(m_registers["RegTemp1"], (byte)(data | 8));
					int num2 = 50;
					do
					{
						data = 0;
						ReadRegister(m_registers["RegTemp1"], ref data);
					}
					while (((data & 4) == 4) && num2-- >= 0);
					ReadRegister(m_registers["RegTemp2"]);
				}
			}
		}

		public void SetTimeoutRssiThresh(decimal value)
		{
			try
			{
				lock (syncThread)
					m_registers["RegRxTimeout2"].Value = (uint)Math.Round((decimal)((value / 1000M) / (16M / BitRate)), MidpointRounding.AwayFromZero);
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetTimeoutRxStart(decimal value)
		{
			try
			{
				lock (syncThread)
					m_registers["RegRxTimeout1"].Value = (uint)Math.Round((decimal)((value / 1000M) / (16M / BitRate)), MidpointRounding.AwayFromZero);
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		public void SetTxStartCondition(bool value)
		{
			try
			{
				lock (syncThread)
				{
					byte num = (byte)m_registers["RegFifoThresh"].Value;
					num = (byte)(num & 0x7f);
					num = (byte)(num | (value ? ((byte)0x80) : ((byte)0)));
					m_registers["RegFifoThresh"].Value = num;
				}
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
		}

		private void SpectrumProcess()
		{
			decimal num = SpectrumFrequencyMin + (SpectrumFrequencyStep * SpectrumFrequencyId);
			byte b2 = (byte)(((long)(num / frequencyStep)) >> 16);
			byte b1 = (byte)(((long)(num / frequencyStep)) >> 8);
			byte b0 = (byte)(((long)(num / frequencyStep)) >> 0);

			if (!Write((byte)m_registers["RegFrfMsb"].Address, b2))
				OnError(1, "Unable to write register " + m_registers["RegFrfMsb"].Name);
			if (!Write((byte)m_registers["RegFrfMid"].Address, b1))
				OnError(1, "Unable to write register " + m_registers["RegFrfMid"].Name);
			if (!Write((byte)m_registers["RegFrfLsb"].Address, b0))
				OnError(1, "Unable to write register " + m_registers["RegFrfLsb"].Name);

			SetRssiStart();

			SpectrumFrequencyId++;
			if (SpectrumFrequencyId >= SpectrumNbFrequenciesMax)
				SpectrumFrequencyId = 0;
		}

		private void SX1231_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			string propertyName = e.PropertyName;
			if (propertyName == "Version")
			{
				PopulateRegisters();
				ReadRegisters();
				return;
			}
			/*
			if (propertyName == "RxReady")
			{
				bool rxReady = RxReady;
			}
			*/
		}

		private void SyncValueCheck(byte[] value)
		{
			int num = 0;
			if (value == null)
				num++;
			else if (value[0] == 0)
				num++;
			if (num != 0)
				OnSyncValueLimitChanged(LimitCheckStatusEnum.ERROR, "First sync word byte must be different of 0!");
			else
				OnSyncValueLimitChanged(LimitCheckStatusEnum.OK, "");
		}

		public bool TransmitRfData(byte[] buffer)
		{
			System.Threading.Monitor.Enter(syncThread);
			bool flag = false;
			try
			{
				SetOperatingMode(OperatingModeEnum.Sleep, true);
				Thread.Sleep(60);
				flag = WriteFifo(buffer);
				SetOperatingMode(OperatingModeEnum.Tx, true);
			}
			catch (Exception exception)
			{
				throw exception;
			}
			finally
			{
				System.Threading.Monitor.Exit(syncThread);
			}
			return flag;
		}

		private void UpdateAesKey()
		{
			int address = (int)m_registers["RegAesKey1"].Address;
			for (int i = 0; i < m_packet.AesKey.Length; i++)
				m_packet.AesKey[i] = (byte)m_registers[address + i].Value;
			OnPropertyChanged("AesKey");
		}

		private void UpdateReceiverData()
		{
			OnPropertyChanged("RxBwMin");
			OnPropertyChanged("RxBwMax");
			switch (((uint)((m_registers["RegRxBw"].Value & 0x18) >> 3)))
			{
				case 0:
					rxBw = ComputeRxBw(frequencyXo, modulationType, 0x10, ((int)m_registers["RegRxBw"].Value) & 7);
					break;

				case 1:
					rxBw = ComputeRxBw(frequencyXo, modulationType, 20, ((int)m_registers["RegRxBw"].Value) & 7);
					break;

				case 2:
					rxBw = ComputeRxBw(frequencyXo, modulationType, 0x18, ((int)m_registers["RegRxBw"].Value) & 7);
					break;

				default:
					throw new Exception("Invalid RxBwMant parameter");
			}
			OnPropertyChanged("RxBw");
			OnPropertyChanged("DccFreqMin");
			OnPropertyChanged("DccFreqMax");
			dccFreq = ComputeDccFreq(RxBw, m_registers["RegRxBw"].Value);
			OnPropertyChanged("DccFreq");
			OnPropertyChanged("AfcRxBwMin");
			OnPropertyChanged("AfcRxBwMax");
			switch (((uint)((m_registers["RegAfcBw"].Value & 0x18) >> 3)))
			{
				case 0:
					afcRxBw = ComputeRxBw(frequencyXo, modulationType, 0x10, ((int)m_registers["RegAfcBw"].Value) & 7);
					break;

				case 1:
					afcRxBw = ComputeRxBw(frequencyXo, modulationType, 20, ((int)m_registers["RegAfcBw"].Value) & 7);
					break;

				case 2:
					afcRxBw = ComputeRxBw(frequencyXo, modulationType, 0x18, ((int)m_registers["RegAfcBw"].Value) & 7);
					break;

				default:
					throw new Exception("Invalid RxBwMant parameter");
			}
			OnPropertyChanged("AfcRxBw");
			OnPropertyChanged("AfcDccFreqMin");
			OnPropertyChanged("AfcDccFreqMax");
			afcDccFreq = ComputeDccFreq(AfcRxBw, m_registers["RegAfcBw"].Value);
			OnPropertyChanged("AfcDccFreq");
		}

		private void UpdateRegisterValue(Register reg)
		{
			int mant;
			switch (reg.Name)
			{
				case "RegOpMode":
					{
						Sequencer = (reg.Value & 0x80) != 0x80;
						ListenMode = (reg.Value & 0x40) == 0x40;
						byte num = (byte)((reg.Value >> 2) & 7);
						if (num > 4)
							num = 0;

						Mode = (OperatingModeEnum)num;
						if (m_packet.Mode != Mode)
							m_packet.Mode = Mode;

						if (m_registers["RegPayloadLength"].Value != m_packet.PayloadLength)
							m_registers["RegPayloadLength"].Value = m_packet.PayloadLength;

						lock (syncThread)
						{
							SetModeLeds(Mode);
							return;
						}
					}
				case "RegDataModul":
					break;

				case "RegBitrateMsb":
				case "RegBitrateLsb":
					if (((m_registers["RegBitrateMsb"].Value << 8) | m_registers["RegBitrateLsb"].Value) == 0)
						m_registers["RegBitrateLsb"].Value = 1;
					BitRate = frequencyXo / ((m_registers["RegBitrateMsb"].Value << 8) | m_registers["RegBitrateLsb"].Value);
					return;

				case "RegFdevMsb":
				case "RegFdevLsb":
					Fdev = ((m_registers["RegFdevMsb"].Value << 8) | m_registers["RegFdevLsb"].Value) * FrequencyStep;
					return;

				case "RegFrfMsb":
				case "RegFrfMid":
				case "RegFrfLsb":
					FrequencyRf = (
						(m_registers["RegFrfMsb"].Value << 0x10) |
						(m_registers["RegFrfMid"].Value << 8) |
						m_registers["RegFrfLsb"].Value
						) * FrequencyStep;
					return;

				case "RegOsc1":
					m_rcCalDone = ((reg.Value >> 6) & 1) == 1;
					OnPropertyChanged("RcCalDone");
					return;

				case "RegOsc2":
				case "RegAfcCtrl":
					if (Version != "2.1")
					{
						AfcLowBetaOn = ((reg.Value >> 5) & 1) == 1;
						if (Version != "2.3")
							return;
						SetDagcOn(DagcOn);
					}
					return;

				case "RegLowBat":
					lowBatMonitor = ((reg.Value >> 4) & 1) == 1;
					OnPropertyChanged("LowBatMonitor");
					LowBatOn = ((reg.Value >> 3) & 1) == 1;
					LowBatTrim = ((LowBatTrimEnum)reg.Value) & LowBatTrimEnum.Trim2_185;
					return;

				case "RegListen1":
					if ((reg.Value & 0xc0) == 0)
						reg.Value = (byte)((reg.Value & 0x3f) | 0x40);
					if ((reg.Value & 0x30) == 0)
						reg.Value = (byte)((reg.Value & 0xcf) | 0x10);
					if ((reg.Value & 6) == 6)
						reg.Value = (byte)((reg.Value & 0xf9) | 2);
					ListenResolIdle = ((ListenResolEnum)((reg.Value >> 6) - 1)) & (ListenResolEnum.Res262000 | ListenResolEnum.Res004100);
					ListenResolRx = ((ListenResolEnum)((reg.Value >> 4) - 1)) & (ListenResolEnum.Res262000 | ListenResolEnum.Res004100);
					ListenCriteria = ((ListenCriteriaEnum)(reg.Value >> 3)) & ListenCriteriaEnum.RssiThreshSyncAddress;
					ListenEnd = ((ListenEndEnum)(reg.Value >> 1)) & ListenEndEnum.Reserved;
					return;

				case "RegListen2":
					switch (ListenResolIdle)
					{
						case ListenResolEnum.Res000064:
							ListenCoefIdle = reg.Value * 0.064M;
							return;

						case ListenResolEnum.Res004100:
							ListenCoefIdle = reg.Value * 4.1M;
							return;

						case ListenResolEnum.Res262000:
							ListenCoefIdle = reg.Value * 262M;
							return;
					}
					return;

				case "RegListen3":
					switch (ListenResolRx)
					{
						case ListenResolEnum.Res000064:
							ListenCoefRx = reg.Value * 0.064M;
							return;

						case ListenResolEnum.Res004100:
							ListenCoefRx = reg.Value * 4.1M;
							return;

						case ListenResolEnum.Res262000:
							ListenCoefRx = reg.Value * 262M;
							return;
					}
					return;

				case "RegVersion":
					Version = ((reg.Value & 0xF0) >> 4).ToString() + "." + (reg.Value & 0x0F).ToString();
					return;

				case "RegPaLevel":
					if ((reg.Value & 0xE0) != 0x80
					&& (reg.Value & 0xe0) != 0x40
					&& (reg.Value & 0xe0) != 0x60
						)
						reg.Value = (reg.Value & 0x1F) | 0x80;
					switch ((reg.Value >> 5) & 0x07)
					{
						case 2:
							PaMode = PaModeEnum.PA1;
							break;
						case 3:
							PaMode = PaModeEnum.PA1_PA2;
							break;
						case 4:
							PaMode = PaModeEnum.PA0;
							break;
					}
					if (PaMode != PaModeEnum.PA1_PA2)
					{
						OutputPower = -18.0M + (reg.Value & 0x1f);
						return;
					}
					OutputPower = -14.0M + (reg.Value & 0x1f);
					return;

				case "RegPaRamp":
					PaRamp = ((PaRampEnum)reg.Value) & PaRampEnum.PaRamp_10;
					return;

				case "RegOcp":
					OcpOn = ((reg.Value >> 4) & 1) == 1;
					OcpTrim = 0x2d + (5 * (reg.Value & 15));
					return;

				case "RegAgcRef":
				case "Reserved14":
					AgcAutoRefOn = ((reg.Value >> 6) & 1) == 1;
					AgcRefLevel = (int)(18446744073709551536L - (reg.Value & 0x3f));
					OnPropertyChanged("AgcReference");
					OnPropertyChanged("AgcThresh1");
					OnPropertyChanged("AgcThresh2");
					OnPropertyChanged("AgcThresh3");
					OnPropertyChanged("AgcThresh4");
					OnPropertyChanged("AgcThresh5");
					return;

				case "RegAgcThresh1":
				case "Reserved15":
					AgcSnrMargin = (byte)(reg.Value >> 5);
					AgcStep1 = (byte)(reg.Value & 0x1f);
					OnPropertyChanged("AgcReference");
					OnPropertyChanged("AgcThresh1");
					OnPropertyChanged("AgcThresh2");
					OnPropertyChanged("AgcThresh3");
					OnPropertyChanged("AgcThresh4");
					OnPropertyChanged("AgcThresh5");
					return;

				case "RegAgcThresh2":
				case "Reserved16":
					AgcStep2 = (byte)(reg.Value >> 4);
					AgcStep3 = (byte)(reg.Value & 15);
					OnPropertyChanged("AgcThresh2");
					OnPropertyChanged("AgcThresh3");
					OnPropertyChanged("AgcThresh4");
					OnPropertyChanged("AgcThresh5");
					return;

				case "RegAgcThresh3":
				case "Reserved17":
					AgcStep4 = (byte)(reg.Value >> 4);
					AgcStep5 = (byte)(reg.Value & 15);
					OnPropertyChanged("AgcThresh4");
					OnPropertyChanged("AgcThresh5");
					return;

				case "RegLna":
					LnaZin = ((reg.Value & 0x80) == 0x80) ? LnaZinEnum.ZIN_200 : LnaZinEnum.ZIN_50;
					LnaLowPowerOn = (reg.Value & 0x40) == 0x40;
					LnaCurrentGain = (LnaGainEnum)((reg.Value & 0x38) >> 3);
					LnaGainSelect = ((LnaGainEnum)reg.Value) & LnaGainEnum.Reserved;
					return;

				case "RegRxBw":
					mant = 0;
					switch (((uint)((reg.Value & 0x18) >> 3)))
					{
						case 0:
							mant = 0x10;
							break;
						case 1:
							mant = 20;
							break;

						case 2:
							mant = 0x18;
							break;
						default:
							throw new Exception("Invalid RxBwMant parameter");
					}
					rxBw = ComputeRxBw(frequencyXo, modulationType, mant, ((int)reg.Value) & 7);
					OnPropertyChanged("DccFreqMin");
					OnPropertyChanged("DccFreqMax");
					OnPropertyChanged("RxBwMin");
					OnPropertyChanged("RxBwMax");
					OnPropertyChanged("RxBw");
					OnPropertyChanged("AgcReference");
					OnPropertyChanged("AgcThresh1");
					OnPropertyChanged("AgcThresh2");
					OnPropertyChanged("AgcThresh3");
					OnPropertyChanged("AgcThresh4");
					OnPropertyChanged("AgcThresh5");
					DccFreq = ComputeDccFreq(rxBw, reg.Value);
					return;

				case "RegAfcBw":
					mant = 0;
					switch ((reg.Value & 0x18) >> 3)
					{
						case 0:
							mant = 0x10;
							break;
						case 1:
							mant = 0x14;
							break;
						case 2:
							mant = 0x18;
							break;
						default:
							throw new Exception("Invalid RxBwMant parameter");
					}
					afcRxBw = ComputeRxBw(frequencyXo, modulationType, mant, ((int)reg.Value) & 7);
					OnPropertyChanged("AfcDccFreqMin");
					OnPropertyChanged("AfcDccFreqMax");
					OnPropertyChanged("AfcRxBwMin");
					OnPropertyChanged("AfcRxBwMax");
					OnPropertyChanged("AfcRxBw");
					AfcDccFreq = ComputeDccFreq(afcRxBw, reg.Value);
					return;

				case "RegOokPeak":
					OokThreshType = (OokThreshTypeEnum)(reg.Value >> 6);
					OokPeakThreshStep = OoPeakThreshStepTable[(int)((IntPtr)((reg.Value & 0x38) >> 3))];
					OokPeakThreshDec = ((OokPeakThreshDecEnum)reg.Value) & OokPeakThreshDecEnum.EVERY_1_CHIPS_16_TIMES;
					return;

				case "RegOokAvg":
					OokAverageThreshFilt = (OokAverageThreshFiltEnum)(reg.Value >> 6);
					return;

				case "RegOokFix":
					OokFixedThresh = (byte)reg.Value;
					return;

				case "RegAfcFei":
					feiDone = ((reg.Value >> 6) & 1) == 1;
					OnPropertyChanged("FeiDone");
					afcDone = ((reg.Value >> 4) & 1) == 1;
					OnPropertyChanged("AfcDone");
					AfcAutoClearOn = ((reg.Value >> 3) & 1) == 1;
					AfcAutoOn = ((reg.Value >> 2) & 1) == 1;
					return;

				case "RegAfcMsb":
				case "RegAfcLsb":
					AfcValue = ((short)((m_registers["RegAfcMsb"].Value << 8) | m_registers["RegAfcLsb"].Value)) * FrequencyStep;
					return;

				case "RegFeiMsb":
				case "RegFeiLsb":
					FeiValue = ((short)((m_registers["RegFeiMsb"].Value << 8) | m_registers["RegFeiLsb"].Value)) * FrequencyStep;
					return;

				case "RegRssiConfig":
					FastRx = ((reg.Value >> 3) & 1) == 1;
					rssiDone = ((reg.Value >> 1) & 1) == 1;
					OnPropertyChanged("RssiDone");
					return;

				case "RegRssiValue":
					prevRssiValue = rssiValue;
					rssiValue = -reg.Value / 2.0M;
					if (RfPaSwitchEnabled != 0)
					{
						if (RfPaSwitchSel != RfPaSwitchSelEnum.RF_IO_RFIO)
						{
							if (RfPaSwitchSel == RfPaSwitchSelEnum.RF_IO_PA_BOOST)
							{
								if (RfPaSwitchEnabled == 1)
									rfIoRssiValue = -127.7M;
								rfPaRssiValue = rssiValue;
								OnPropertyChanged("RfPaRssiValue");
							}
						}
						else
						{
							if (RfPaSwitchEnabled == 1)
								rfPaRssiValue = -127.7M;
							rfIoRssiValue = rssiValue;
							OnPropertyChanged("RfIoRssiValue");
						}
					}
					spectrumRssiValue = rssiValue;
					OnPropertyChanged("RssiValue");
					OnPropertyChanged("SpectrumData");
					return;

				case "RegDioMapping1":
					Dio0Mapping = ((DioMappingEnum)(reg.Value >> 6)) & DioMappingEnum.DIO_MAP_11;
					Dio1Mapping = ((DioMappingEnum)(reg.Value >> 4)) & DioMappingEnum.DIO_MAP_11;
					Dio2Mapping = ((DioMappingEnum)(reg.Value >> 2)) & DioMappingEnum.DIO_MAP_11;
					Dio3Mapping = ((DioMappingEnum)reg.Value) & DioMappingEnum.DIO_MAP_11;
					return;

				case "RegDioMapping2":
					Dio4Mapping = ((DioMappingEnum)(reg.Value >> 6)) & DioMappingEnum.DIO_MAP_11;
					Dio5Mapping = ((DioMappingEnum)(reg.Value >> 4)) & DioMappingEnum.DIO_MAP_11;
					ClockOut = ((ClockOutEnum)reg.Value) & ClockOutEnum.CLOCK_OUT_111;
					return;

				case "RegIrqFlags1":
					{
						modeReady = ((reg.Value >> 7) & 1) == 1;
						OnPropertyChanged("ModeReady");
						bool flag = ((reg.Value >> 6) & 1) == 1;
						if (!rxReady && flag)
							restartRx = true;

						rxReady = flag;
						OnPropertyChanged("RxReady");
						txReady = ((reg.Value >> 5) & 1) == 1;
						OnPropertyChanged("TxReady");
						m_pllLock = ((reg.Value >> 4) & 1) == 1;
						OnPropertyChanged("PllLock");
						rssi = ((reg.Value >> 3) & 1) == 1;
						OnPropertyChanged("Rssi");
						timeout = ((reg.Value >> 2) & 1) == 1;
						OnPropertyChanged("Timeout");
						autoMode = ((reg.Value >> 1) & 1) == 1;
						OnPropertyChanged("AutoMode");
						syncAddressMatch = (reg.Value & 1) == 1;
						OnPropertyChanged("SyncAddressMatch");
						return;
					}
				case "RegIrqFlags2":
					fifoFull = ((reg.Value >> 7) & 1) == 1;
					OnPropertyChanged("FifoFull");
					fifoNotEmpty = ((reg.Value >> 6) & 1) == 1;
					OnPropertyChanged("FifoNotEmpty");
					fifoLevel = ((reg.Value >> 5) & 1) == 1;
					OnPropertyChanged("FifoLevel");
					fifoOverrun = ((reg.Value >> 4) & 1) == 1;
					OnPropertyChanged("FifoOverrun");
					packetSent = ((reg.Value >> 3) & 1) == 1;
					OnPropertyChanged("PacketSent");
					m_payloadReady = ((reg.Value >> 2) & 1) == 1;
					OnPropertyChanged("PayloadReady");
					crcOk = ((reg.Value >> 1) & 1) == 1;
					OnPropertyChanged("CrcOk");
					lowBat = (reg.Value & 1) == 1;
					OnPropertyChanged("LowBat");
					return;

				case "RegRssiThresh":
					RssiThresh = -reg.Value / 2.0M;
					return;

				case "RegRxTimeout1":
					TimeoutRxStart = (reg.Value * (16M / BitRate)) * 1000M;
					return;

				case "RegRxTimeout2":
					TimeoutRssiThresh = (reg.Value * (16M / BitRate)) * 1000M;
					return;

				case "RegPreambleMsb":
				case "RegPreambleLsb":
					m_packet.PreambleSize = (int)((m_registers["RegPreambleMsb"].Value << 8) | m_registers["RegPreambleLsb"].Value);
					return;

				case "RegSyncConfig":
					m_packet.SyncOn = ((reg.Value >> 7) & 1) == 1;
					m_packet.FifoFillCondition = (((reg.Value >> 6) & 1) == 1) ? FifoFillConditionEnum.Allways : FifoFillConditionEnum.OnSyncAddressIrq;
					m_packet.SyncSize = (byte)(((reg.Value & 0x38) >> 3) + 1);
					UpdateSyncValue();
					m_packet.SyncTol = (byte)(reg.Value & 7);
					return;

				case "RegSyncValue1":
				case "RegSyncValue2":
				case "RegSyncValue3":
				case "RegSyncValue4":
				case "RegSyncValue5":
				case "RegSyncValue6":
				case "RegSyncValue7":
				case "RegSyncValue8":
					UpdateSyncValue();
					return;

				case "RegPacketConfig1":
					m_packet.PacketFormat = (((reg.Value >> 7) & 1) == 1) ? PacketFormatEnum.Variable : PacketFormatEnum.Fixed;
					m_packet.DcFree = ((DcFreeEnum)(reg.Value >> 5)) & DcFreeEnum.Reserved;
					m_packet.CrcOn = ((reg.Value >> 4) & 1) == 1;
					m_packet.CrcAutoClearOff = ((reg.Value >> 3) & 1) == 1;
					m_packet.AddressFiltering = ((AddressFilteringEnum)(reg.Value >> 1)) & AddressFilteringEnum.Reserved;
					return;

				case "RegPayloadLength":
					m_packet.PayloadLength = (byte)reg.Value;
					return;

				case "RegNodeAdrs":
					m_packet.NodeAddress = (byte)reg.Value;
					return;

				case "RegBroadcastAdrs":
					m_packet.BroadcastAddress = (byte)reg.Value;
					return;

				case "RegAutoModes":
					m_packet.EnterCondition = (EnterConditionEnum)((reg.Value & 0xe0) >> 5);
					m_packet.ExitCondition = (ExitConditionEnum)((reg.Value & 0x1c) >> 2);
					m_packet.IntermediateMode = ((IntermediateModeEnum)reg.Value) & IntermediateModeEnum.Tx;
					return;

				case "RegFifoThresh":
					m_packet.TxStartCondition = ((reg.Value >> 7) & 1) == 1;
					m_packet.FifoThreshold = (byte)(reg.Value & 0x7f);
					return;

				case "RegPacketConfig2":
					m_packet.InterPacketRxDelay = (int)(reg.Value >> 4);
					m_packet.AutoRxRestartOn = ((reg.Value >> 1) & 1) == 1;
					m_packet.AesOn = (reg.Value & 1) == 1;
					return;

				case "RegAesKey1":
				case "RegAesKey2":
				case "RegAesKey3":
				case "RegAesKey4":
				case "RegAesKey5":
				case "RegAesKey6":
				case "RegAesKey7":
				case "RegAesKey8":
				case "RegAesKey9":
				case "RegAesKey10":
				case "RegAesKey11":
				case "RegAesKey12":
				case "RegAesKey13":
				case "RegAesKey14":
				case "RegAesKey15":
				case "RegAesKey16":
					UpdateAesKey();
					return;

				case "RegTemp1":
					AdcLowPowerOn = (reg.Value & 1) == 1;
					return;

				case "RegTemp2":
					tempValue = (-((byte)reg.Value) + tempValueRoom) + tempValueCal;
					OnPropertyChanged("TempValue");
					return;

				case "RegTestLna":
					SensitivityBoostOn = reg.Value == 0x2d;
					return;

				case "RegTestDagc":
					DagcOn = ((reg.Value & 0x30) == 0x30) || ((reg.Value & 0x10) == 0x10);
					return;

				case "RegTestAfc":
					LowBetaAfcOffset = ((sbyte)reg.Value) * 488.0M;
					return;

				default:
					return;
			}
			if ((reg.Value & 0x60) == 0x20)
				reg.Value &= 0x9F;

			if ((reg.Value & 0x18) == 0x10 || (reg.Value & 0x18) == 0x18)
				reg.Value &= 0xE7;

			DataMode = ((DataModeEnum)(reg.Value >> 5)) & DataModeEnum.Continuous;
			ModulationType = ((ModulationTypeEnum)(reg.Value >> 3)) & ModulationTypeEnum.Reserved;
			ModulationShaping = (byte)(reg.Value & 0x03);
			UpdateReceiverData();
			BitRateFdevCheck(bitRate, fdev);
		}

		private void UpdateSyncValue()
		{
			int address = (int)m_registers["RegSyncValue1"].Address;
			for (int i = 0; i < m_packet.SyncValue.Length; i++)
				m_packet.SyncValue[i] = (byte)m_registers[address + i].Value;

			SyncValueCheck(m_packet.SyncValue);
			OnPropertyChanged("SyncValue");
		}

		public bool Write(byte address, byte data)
		{
			Mpsse portA = ftdi.PortA;
			portA.PortValue = (byte)(portA.PortValue & ~PA3_CS);
			portA.ScanOut(8, new byte[] { (byte)(address | 0x80) }, true);
			portA.ScanOut(8, new byte[] { data }, true);
			portA.PortValue = (byte)(portA.PortValue | PA3_CS);
			portA.TxBufferAdd(0x87);
			return portA.SendBytes();
		}

		public bool Write(byte address, byte[] data)
		{
			Mpsse portA = ftdi.PortA;
			portA.PortValue = (byte)(portA.PortValue & ~PA3_CS);
			portA.ScanOut(8, new byte[] { (byte)(address | 0x80) }, true);
			for (int i = 0; i < data.Length; i++)
				portA.ScanOut(8, new byte[] { data[i] }, true);
			portA.PortValue = (byte)(portA.PortValue | PA3_CS);
			portA.TxBufferAdd(0x87);
			return portA.SendBytes();
		}

		public bool WriteFifo(byte[] data)
		{
			return Write(0, data);
		}

		private bool WriteRegister(Register r, byte data)
		{
			bool flag;
			lock (syncThread)
			{
				try
				{
					writeLock++;
					if (!Write((byte)r.Address, data))
						throw new Exception("Unable to read register: " + r.Name);
					flag = true;
				}
				catch (Exception exception)
				{
					OnError(1, exception.Message);
					flag = false;
				}
				finally
				{
					writeLock--;
				}
			}
			return flag;
		}

		public void WriteRegisters()
		{
			object obj2;
			System.Threading.Monitor.Enter(obj2 = syncThread);
			try
			{
				foreach (Register register in m_registers)
				{
					if ((register.Address != 0) && !Write((byte)register.Address, (byte)register.Value))
					{
						throw new Exception("Writing register " + register.Name);
					}
				}
			}
			catch (Exception exception)
			{
				OnError(1, exception.Message);
			}
			finally
			{
				System.Threading.Monitor.Exit(obj2);
			}
		}

		public bool AdcLowPowerOn
		{
			get
			{
				return adcLowPowerOn;
			}
			set
			{
				adcLowPowerOn = value;
				OnPropertyChanged("AdcLowPowerOn");
			}
		}

		public bool AfcAutoClearOn
		{
			get
			{
				return afcAutoClearOn;
			}
			set
			{
				afcAutoClearOn = value;
				OnPropertyChanged("AfcAutoClearOn");
			}
		}

		public bool AfcAutoOn
		{
			get
			{
				return afcAutoOn;
			}
			set
			{
				afcAutoOn = value;
				OnPropertyChanged("AfcAutoOn");
			}
		}

		public decimal AfcDccFreq
		{
			get
			{
				return afcDccFreq;
			}
			set
			{
				afcDccFreq = value;
				OnPropertyChanged("AfcDccFreq");
			}
		}

		public decimal AfcDccFreqMax
		{
			get
			{
				return ((4.0M * AfcRxBw) / (6.283185307179580M * ((decimal)Math.Pow(2.0, 2.0))));
			}
		}

		public decimal AfcDccFreqMin
		{
			get
			{
				return ((4.0M * AfcRxBw) / (6.283185307179580M * ((decimal)Math.Pow(2.0, 9.0))));
			}
		}

		public bool AfcDone
		{
			get
			{
				return afcDone;
			}
		}

		public bool AfcLowBetaOn
		{
			get
			{
				return afcLowBetaOn;
			}
			set
			{
				afcLowBetaOn = value;
				OnPropertyChanged("AfcLowBetaOn");
			}
		}

		public decimal AfcRxBw
		{
			get
			{
				return afcRxBw;
			}
			set
			{
				afcRxBw = value;
				OnPropertyChanged("AfcRxBw");
			}
		}

		public decimal AfcRxBwMax
		{
			get
			{
				return ComputeRxBwMax();
			}
		}

		public decimal AfcRxBwMin
		{
			get
			{
				return ComputeRxBwMin();
			}
		}

		public decimal AfcValue
		{
			get
			{
				return afcValue;
			}
			set
			{
				afcValue = value;
				OnPropertyChanged("AfcValue");
			}
		}

		public bool AgcAutoRefOn
		{
			get
			{
				return agcAutoRefOn;
			}
			set
			{
				agcAutoRefOn = value;
				OnPropertyChanged("AgcAutoRefOn");
				OnPropertyChanged("AgcReference");
				OnPropertyChanged("AgcThresh1");
				OnPropertyChanged("AgcThresh2");
				OnPropertyChanged("AgcThresh3");
				OnPropertyChanged("AgcThresh4");
				OnPropertyChanged("AgcThresh5");
			}
		}

		public int AgcReference
		{
			get
			{
				if (agcAutoRefOn)
				{
					return (int)Math.Round((double)((-159.0 + (10.0 * Math.Log10((double)(2M * RxBw)))) + AgcSnrMargin), MidpointRounding.AwayFromZero);
				}
				return AgcRefLevel;
			}
		}

		public int AgcRefLevel
		{
			get
			{
				return agcRefLevel;
			}
			set
			{
				agcRefLevel = value;
				OnPropertyChanged("AgcRefLevel");
				OnPropertyChanged("AgcReference");
				OnPropertyChanged("AgcThresh1");
				OnPropertyChanged("AgcThresh2");
				OnPropertyChanged("AgcThresh3");
				OnPropertyChanged("AgcThresh4");
				OnPropertyChanged("AgcThresh5");
			}
		}

		public byte AgcSnrMargin
		{
			get
			{
				return agcSnrMargin;
			}
			set
			{
				agcSnrMargin = value;
				OnPropertyChanged("AgcSnrMargin");
				OnPropertyChanged("AgcReference");
				OnPropertyChanged("AgcThresh1");
				OnPropertyChanged("AgcThresh2");
				OnPropertyChanged("AgcThresh3");
				OnPropertyChanged("AgcThresh4");
				OnPropertyChanged("AgcThresh5");
			}
		}

		public byte AgcStep1
		{
			get
			{
				return agcStep1;
			}
			set
			{
				agcStep1 = value;
				OnPropertyChanged("AgcStep1");
				OnPropertyChanged("AgcThresh1");
				OnPropertyChanged("AgcThresh2");
				OnPropertyChanged("AgcThresh3");
				OnPropertyChanged("AgcThresh4");
				OnPropertyChanged("AgcThresh5");
			}
		}

		public byte AgcStep2
		{
			get
			{
				return agcStep2;
			}
			set
			{
				agcStep2 = value;
				OnPropertyChanged("AgcStep2");
				OnPropertyChanged("AgcThresh2");
				OnPropertyChanged("AgcThresh3");
				OnPropertyChanged("AgcThresh4");
				OnPropertyChanged("AgcThresh5");
			}
		}

		public byte AgcStep3
		{
			get
			{
				return agcStep3;
			}
			set
			{
				agcStep3 = value;
				OnPropertyChanged("AgcStep3");
				OnPropertyChanged("AgcThresh3");
				OnPropertyChanged("AgcThresh4");
				OnPropertyChanged("AgcThresh5");
			}
		}

		public byte AgcStep4
		{
			get
			{
				return agcStep4;
			}
			set
			{
				agcStep4 = value;
				OnPropertyChanged("AgcStep4");
				OnPropertyChanged("AgcThresh4");
				OnPropertyChanged("AgcThresh5");
			}
		}

		public byte AgcStep5
		{
			get
			{
				return agcStep5;
			}
			set
			{
				agcStep5 = value;
				OnPropertyChanged("AgcStep5");
				OnPropertyChanged("AgcThresh5");
			}
		}

		public int AgcThresh1
		{
			get
			{
				return (AgcReference + agcStep1);
			}
		}

		public int AgcThresh2
		{
			get
			{
				return (AgcThresh1 + agcStep2);
			}
		}

		public int AgcThresh3
		{
			get
			{
				return (AgcThresh2 + agcStep3);
			}
		}

		public int AgcThresh4
		{
			get
			{
				return (AgcThresh3 + agcStep4);
			}
		}

		public int AgcThresh5
		{
			get
			{
				return (AgcThresh4 + agcStep5);
			}
		}

		public bool AutoMode
		{
			get
			{
				return autoMode;
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
				bitRate = value;
				BitRateFdevCheck(value, fdev);
				OnPropertyChanged("BitRate");
			}
		}

		public ClockOutEnum ClockOut
		{
			get
			{
				return clockOut;
			}
			set
			{
				clockOut = value;
				OnPropertyChanged("ClockOut");
			}
		}

		public bool CrcOk
		{
			get
			{
				return crcOk;
			}
		}

		public bool DagcOn
		{
			get
			{
				return dagcOn;
			}
			set
			{
				dagcOn = value;
				OnPropertyChanged("DagcOn");
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
				OnPropertyChanged("DataMode");
			}
		}

		public decimal DccFreq
		{
			get
			{
				return dccFreq;
			}
			set
			{
				dccFreq = value;
				OnPropertyChanged("DccFreq");
			}
		}

		public decimal DccFreqMax
		{
			get
			{
				return ((4.0M * RxBw) / (6.283185307179580M * ((decimal)Math.Pow(2.0, 2.0))));
			}
		}

		public decimal DccFreqMin
		{
			get
			{
				return ((4.0M * RxBw) / (6.283185307179580M * ((decimal)Math.Pow(2.0, 9.0))));
			}
		}

		public string DeviceName
		{
			get
			{
				return deviceName;
			}
		}

		public DioMappingEnum Dio0Mapping
		{
			get
			{
				return dio0Mapping;
			}
			set
			{
				dio0Mapping = value;
				OnPropertyChanged("Dio0Mapping");
			}
		}

		public DioMappingEnum Dio1Mapping
		{
			get
			{
				return dio1Mapping;
			}
			set
			{
				dio1Mapping = value;
				OnPropertyChanged("Dio1Mapping");
			}
		}

		public DioMappingEnum Dio2Mapping
		{
			get
			{
				return dio2Mapping;
			}
			set
			{
				dio2Mapping = value;
				OnPropertyChanged("Dio2Mapping");
			}
		}

		public DioMappingEnum Dio3Mapping
		{
			get
			{
				return dio3Mapping;
			}
			set
			{
				dio3Mapping = value;
				OnPropertyChanged("Dio3Mapping");
			}
		}

		public DioMappingEnum Dio4Mapping
		{
			get
			{
				return dio4Mapping;
			}
			set
			{
				dio4Mapping = value;
				OnPropertyChanged("Dio4Mapping");
			}
		}

		public DioMappingEnum Dio5Mapping
		{
			get
			{
				return dio5Mapping;
			}
			set
			{
				dio5Mapping = value;
				OnPropertyChanged("Dio5Mapping");
			}
		}

		public bool FastRx
		{
			get
			{
				return fastRx;
			}
			set
			{
				fastRx = value;
				OnPropertyChanged("FastRx");
			}
		}

		public decimal Fdev
		{
			get
			{
				return fdev;
			}
			set
			{
				fdev = value;
				BitRateFdevCheck(bitRate, value);
				OnPropertyChanged("Fdev");
			}
		}

		public bool FeiDone
		{
			get
			{
				return feiDone;
			}
		}

		public decimal FeiValue
		{
			get
			{
				return feiValue;
			}
			set
			{
				feiValue = value;
				OnPropertyChanged("FeiValue");
			}
		}

		public bool FifoFull
		{
			get
			{
				return fifoFull;
			}
		}

		public bool FifoLevel
		{
			get
			{
				return fifoLevel;
			}
		}

		public bool FifoNotEmpty
		{
			get
			{
				return fifoNotEmpty;
			}
		}

		public bool FifoOverrun
		{
			get
			{
				return fifoOverrun;
			}
		}

		public decimal FrequencyRf
		{
			get
			{
				return frequencyRf;
			}
			set
			{
				frequencyRf = value;
				OnPropertyChanged("FrequencyRf");
				FrequencyRfCheck(value);
			}
		}

		public decimal FrequencyStep
		{
			get
			{
				return frequencyStep;
			}
			set
			{
				frequencyStep = value;
				OnPropertyChanged("FrequencyStep");
			}
		}

		public decimal FrequencyXo
		{
			get
			{
				return frequencyXo;
			}
			set
			{
				frequencyXo = value;
				FrequencyStep = frequencyXo / ((decimal)Math.Pow(2.0, 19.0));
				FrequencyRf = (((m_registers["RegFrfMsb"].Value << 0x10) | (m_registers["RegFrfMid"].Value << 8)) | m_registers["RegFrfLsb"].Value) * FrequencyStep;
				Fdev = ((m_registers["RegFdevMsb"].Value << 8) | m_registers["RegFdevLsb"].Value) * FrequencyStep;
				BitRate = frequencyXo / ((m_registers["RegBitrateMsb"].Value << 8) | m_registers["RegBitrateLsb"].Value);
				OnPropertyChanged("FrequencyXo");
				int mant = 0;
				switch (((uint)((m_registers["RegRxBw"].Value & 0x18) >> 3)))
				{
					case 0:
						mant = 0x10;
						break;

					case 1:
						mant = 20;
						break;

					case 2:
						mant = 0x18;
						break;

					default:
						throw new Exception("Invalid RxBwMant parameter");
				}
				RxBw = ComputeRxBw(value, modulationType, mant, ((int)m_registers["RegRxBw"].Value) & 7);
				mant = 0;
				switch (((uint)((m_registers["RegAfcBw"].Value & 0x18) >> 3)))
				{
					case 0:
						mant = 0x10;
						break;

					case 1:
						mant = 20;
						break;

					case 2:
						mant = 0x18;
						break;

					default:
						throw new Exception("Invalid RxBwMant parameter");
				}
				AfcRxBw = ComputeRxBw(value, modulationType, mant, ((int)m_registers["RegAfcBw"].Value) & 7);
				DccFreq = ComputeDccFreq(RxBw, m_registers["RegRxBw"].Value);
				AfcDccFreq = ComputeDccFreq(AfcRxBw, m_registers["RegAfcBw"].Value);
			}
		}

		public bool IsOpen
		{
			get
			{
				return isOpen;
			}
			set
			{
				isOpen = value;
				OnPropertyChanged("IsOpen");
			}
		}

		public decimal ListenCoefIdle
		{
			get
			{
				return listenCoefIdle;
			}
			set
			{
				listenCoefIdle = value;
				OnPropertyChanged("ListenCoefIdle");
			}
		}

		public decimal ListenCoefRx
		{
			get
			{
				return listenCoefRx;
			}
			set
			{
				listenCoefRx = value;
				OnPropertyChanged("ListenCoefRx");
			}
		}

		public ListenCriteriaEnum ListenCriteria
		{
			get
			{
				return listenCriteria;
			}
			set
			{
				listenCriteria = value;
				OnPropertyChanged("ListenCriteria");
			}
		}

		public ListenEndEnum ListenEnd
		{
			get
			{
				return listenEnd;
			}
			set
			{
				listenEnd = value;
				OnPropertyChanged("ListenEnd");
			}
		}

		public bool ListenMode
		{
			get
			{
				return listenMode;
			}
			set
			{
				listenMode = value;
				OnPropertyChanged("Listen");
			}
		}

		public ListenResolEnum ListenResolIdle
		{
			get
			{
				return listenResolIdle;
			}
			set
			{
				listenResolIdle = value;
				OnPropertyChanged("ListenResolIdle");
				switch (value)
				{
					case ListenResolEnum.Res000064:
						ListenCoefIdle = m_registers["RegListen2"].Value * 0.064M;
						return;

					case ListenResolEnum.Res004100:
						ListenCoefIdle = m_registers["RegListen2"].Value * 4.1M;
						return;

					case ListenResolEnum.Res262000:
						ListenCoefIdle = m_registers["RegListen2"].Value * 262M;
						return;
				}
			}
		}

		public ListenResolEnum ListenResolRx
		{
			get
			{
				return listenResolRx;
			}
			set
			{
				listenResolRx = value;
				OnPropertyChanged("ListenResolRx");
				switch (value)
				{
					case ListenResolEnum.Res000064:
						ListenCoefRx = m_registers["RegListen3"].Value * 0.064M;
						return;

					case ListenResolEnum.Res004100:
						ListenCoefRx = m_registers["RegListen3"].Value * 4.1M;
						return;

					case ListenResolEnum.Res262000:
						ListenCoefRx = m_registers["RegListen3"].Value * 262M;
						return;
				}
			}
		}

		public LnaGainEnum LnaCurrentGain
		{
			get
			{
				return lnaCurrentGain;
			}
			set
			{
				lnaCurrentGain = value;
				OnPropertyChanged("LnaCurrentGain");
			}
		}

		public LnaGainEnum LnaGainSelect
		{
			get
			{
				return lnaGainSelect;
			}
			set
			{
				lnaGainSelect = value;
				OnPropertyChanged("LnaGainSelect");
			}
		}

		public bool LnaLowPowerOn
		{
			get
			{
				return lnaLowPowerOn;
			}
			set
			{
				lnaLowPowerOn = value;
				OnPropertyChanged("LnaLowPowerOn");
			}
		}

		public LnaZinEnum LnaZin
		{
			get
			{
				return lnaZin;
			}
			set
			{
				lnaZin = value;
				OnPropertyChanged("LnaZin");
			}
		}

		public bool LowBat
		{
			get
			{
				return lowBat;
			}
		}

		public bool LowBatMonitor
		{
			get
			{
				return lowBatMonitor;
			}
		}

		public bool LowBatOn
		{
			get
			{
				return lowBatOn;
			}
			set
			{
				lowBatOn = value;
				OnPropertyChanged("LowBatOn");
			}
		}

		public LowBatTrimEnum LowBatTrim
		{
			get
			{
				return lowBatTrim;
			}
			set
			{
				lowBatTrim = value;
				OnPropertyChanged("LowBatTrim");
			}
		}

		public decimal LowBetaAfcOffset
		{
			get
			{
				return lowBetaAfcOffset;
			}
			set
			{
				lowBetaAfcOffset = value;
				OnPropertyChanged("LowBetaAfcOffset");
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
				OnPropertyChanged("Mode");
			}
		}

		public bool ModeReady
		{
			get
			{
				return modeReady;
			}
		}

		public byte ModulationShaping
		{
			get
			{
				return modulationShaping;
			}
			set
			{
				modulationShaping = value;
				OnPropertyChanged("ModulationShaping");
			}
		}

		public ModulationTypeEnum ModulationType
		{
			get { return modulationType; }
			set
			{
				modulationType = value;
				OnPropertyChanged("ModulationType");
			}
		}

		public bool Monitor
		{
			get
			{
				lock (syncThread)
					return monitor;
			}
			set
			{
				lock (syncThread)
				{
					monitor = value;
					OnPropertyChanged("Monitor");
				}
			}
		}

		public bool OcpOn
		{
			get { return m_ocpOn; }
			set
			{
				m_ocpOn = value;
				OnPropertyChanged("OcpOn");
			}
		}

		public decimal OcpTrim
		{
			get { return m_ocpTrim; }
			set
			{
				m_ocpTrim = value;
				OnPropertyChanged("OcpTrim");
			}
		}

		public OokAverageThreshFiltEnum OokAverageThreshFilt
		{
			get { return m_ookAverageThreshFilt; }
			set
			{
				m_ookAverageThreshFilt = value;
				OnPropertyChanged("OokAverageThreshFilt");
			}
		}

		public byte OokFixedThresh
		{
			get { return m_ookFixedThresh; }
			set
			{
				m_ookFixedThresh = value;
				OnPropertyChanged("ookFixedThresh");
			}
		}

		public OokPeakThreshDecEnum OokPeakThreshDec
		{
			get { return m_ookPeakThreshDec; }
			set
			{
				m_ookPeakThreshDec = value;
				OnPropertyChanged("OokPeakThreshDec");
			}
		}

		public decimal OokPeakThreshStep
		{
			get { return m_ookPeakThreshStep; }
			set
			{
				m_ookPeakThreshStep = value;
				OnPropertyChanged("OokPeakThreshStep");
			}
		}

		public OokThreshTypeEnum OokThreshType
		{
			get { return m_ookThreshType; }
			set
			{
				m_ookThreshType = value;
				OnPropertyChanged("OokThreshType");
			}
		}

		public decimal[] OoPeakThreshStepTable
		{
			get { return ookPeakThreshStepTable; }
		}

		public decimal OutputPower
		{
			get { return m_outputPower; }
			set
			{
				m_outputPower = value;
				OnPropertyChanged("OutputPower");
			}
		}

		public SemtechLib.Devices.SX1231.General.Packet Packet
		{
			get { return m_packet; }
			set
			{
				m_packet = value;
				m_packet.PropertyChanged += new PropertyChangedEventHandler(packet_PropertyChanged);
				OnPropertyChanged("Packet");
			}
		}

		public bool PacketSent
		{
			get { return packetSent; }
		}

		public PaModeEnum PaMode
		{
			get { return m_paMode; }
			set
			{
				m_paMode = value;
				OnPropertyChanged("PaMode");
			}
		}

		public PaRampEnum PaRamp
		{
			get { return m_paRamp; }
			set
			{
				m_paRamp = value;
				OnPropertyChanged("PaRamp");
			}
		}

		public bool PayloadReady
		{
			get { return m_payloadReady; }
		}

		public bool PllLock
		{
			get { return m_pllLock; }
		}

		public bool RcCalDone
		{
			get { return m_rcCalDone; }
		}

		public RegisterCollection Registers
		{
			get { return m_registers; }
			set
			{
				m_registers = value;
				OnPropertyChanged("Registers");
			}
		}

		public decimal RfIoRssiValue
		{
			get { return rfIoRssiValue; }
		}

		public decimal RfPaRssiValue
		{
			get { return rfPaRssiValue; }
		}

		public int RfPaSwitchEnabled
		{
			get { return rfPaSwitchEnabled; }
			set
			{
				object obj2;
				System.Threading.Monitor.Enter(obj2 = syncThread);
				try
				{
					prevRfPaSwitchEnabled = rfPaSwitchEnabled;
					rfPaSwitchEnabled = value;
					if (prevRfPaSwitchEnabled != rfPaSwitchEnabled)
					{
						if (rfPaSwitchEnabled == 2)
							prevRfPaSwitchSel = rfPaSwitchSel;
						else
							rfPaSwitchSel = prevRfPaSwitchSel;
					}

					if (rfPaSwitchEnabled != 0)
						ftdi.PortA.PortAcDir = 1;
					else
						ftdi.PortA.PortAcDir = 0;

					ftdi.PortA.SendBytes();
					OnPropertyChanged("RfPaSwitchEnabled");
					OnPropertyChanged("RfPaSwitchSel");
				}
				catch (Exception exception)
				{
					OnError(1, exception.Message);
				}
				finally
				{
					System.Threading.Monitor.Exit(obj2);
				}
			}
		}

		public RfPaSwitchSelEnum RfPaSwitchSel
		{
			get { return rfPaSwitchSel; }
			set
			{
				object obj2;
				System.Threading.Monitor.Enter(obj2 = syncThread);
				try
				{
					Mpsse portA = ftdi.PortA;
					rfPaSwitchSel = value;
					switch (value)
					{
						case RfPaSwitchSelEnum.RF_IO_RFIO:
							{
								portA.PortAcValue = (byte)(portA.PortAcValue & ~1);
								break;
							}
						case RfPaSwitchSelEnum.RF_IO_PA_BOOST:
							{
								portA.PortAcValue = (byte)(portA.PortAcValue | 1);
								break;
							}
					}
					portA.SendBytes();
					OnPropertyChanged("RfPaSwitchSel");
				}
				catch (Exception exception)
				{
					OnError(1, exception.Message);
				}
				finally
				{
					System.Threading.Monitor.Exit(obj2);
				}
			}
		}

		public bool Rssi
		{
			get { return rssi; }
		}

		public bool RssiAutoThresh
		{
			get { return rssiAutoThresh; }
			set
			{
				rssiAutoThresh = value;
				OnPropertyChanged("RssiAutoThresh");
			}
		}

		public bool RssiDone
		{
			get { return rssiDone; }
		}

		public decimal RssiThresh
		{
			get { return rssiThresh; }
			set
			{
				rssiThresh = value;
				OnPropertyChanged("RssiThresh");
			}
		}

		public decimal RssiValue
		{
			get { return rssiValue; }
		}

		public decimal RxBw
		{
			get { return rxBw; }
			set
			{
				rxBw = value;
				OnPropertyChanged("RxBw");
			}
		}

		public decimal RxBwMax
		{
			get { return ComputeRxBwMax(); }
		}

		public decimal RxBwMin
		{
			get { return ComputeRxBwMin(); }
		}

		public bool RxReady
		{
			get { return rxReady; }
		}

		public bool SensitivityBoostOn
		{
			get { return sensitivityBoostOn; }
			set
			{
				sensitivityBoostOn = value;
				OnPropertyChanged("SensitivityBoostOn");
			}
		}

		public bool Sequencer
		{
			get { return sequencer; }
			set
			{
				sequencer = value;
				OnPropertyChanged("Sequencer");
			}
		}

		public int SpectrumFrequencyId
		{
			get { return spectrumFreqId; }
			set
			{
				spectrumFreqId = value;
				OnPropertyChanged("SpectrumFreqId");
			}
		}

		public decimal SpectrumFrequencyMax
		{
			get { return (FrequencyRf + (SpectrumFrequencySpan / 2.0M)); }
		}

		public decimal SpectrumFrequencyMin
		{
			get { return (FrequencyRf - (SpectrumFrequencySpan / 2.0M)); }
		}

		public decimal SpectrumFrequencySpan
		{
			get { return spectrumFreqSpan; }
			set
			{
				spectrumFreqSpan = value;
				OnPropertyChanged("SpectrumFreqSpan");
			}
		}

		public decimal SpectrumFrequencyStep
		{
			get { return (RxBw / 3.0M); }
		}

		public int SpectrumNbFrequenciesMax
		{
			get { return (int)((SpectrumFrequencyMax - SpectrumFrequencyMin) / SpectrumFrequencyStep); }
		}

		public bool SpectrumOn
		{
			get { return spectrumOn; }
			set
			{
				spectrumOn = value;
				if (spectrumOn)
				{
					RfPaSwitchEnabled = 0;
					prevRssiAutoThresh = RssiAutoThresh;
					RssiAutoThresh = false;
					prevRssiThresh = RssiThresh;
					SetRssiThresh(-127.5M);
					prevModulationType = ModulationType;
					SetModulationType(ModulationTypeEnum.OOK);
					prevLnaGainSelect = LnaGainSelect;
					SetLnaGainSelect(LnaGainEnum.G1);
					prevMode = Mode;
					SetOperatingMode(OperatingModeEnum.Rx);
					prevMonitorOn = Monitor;
					Monitor = true;
				}
				else
				{
					SetFrequencyRf(FrequencyRf);
					RfPaSwitchEnabled = prevRfPaSwitchEnabled;
					RssiAutoThresh = prevRssiAutoThresh;
					SetRssiThresh(prevRssiThresh);
					SetModulationType(prevModulationType);
					SetLnaGainSelect(prevLnaGainSelect);
					SetOperatingMode(prevMode);
					Monitor = prevMonitorOn;
				}
				OnPropertyChanged("SpectrumOn");
			}
		}

		public decimal SpectrumRssiValue
		{
			get { return spectrumRssiValue; }
		}

		public int SPISpeed
		{
			get { return spiSpeed; }
			set
			{
				spiSpeed = value;
				OnPropertyChanged("SPISpeed");
			}
		}

		public bool SyncAddressMatch
		{
			get { return syncAddressMatch; }
		}

		public bool TempCalDone
		{
			get { return tempCalDone; }
			set
			{
				tempCalDone = value;
				OnPropertyChanged("TempCalDone");
			}
		}

		public bool TempMeasRunning
		{
			get { return tempMeasRunning; }
		}

		public decimal TempValue
		{
			get { return tempValue; }
		}

		public decimal TempValueCal
		{
			get { return tempValueCal; }
			set { tempValueCal = value; }
		}

		public decimal TempValueRoom
		{
			get { return tempValueRoom; }
			set
			{
				tempValueRoom = value;
				OnPropertyChanged("TempValueRoom");
			}
		}

		public bool Test
		{
			get { return test; }
			set { test = value; }
		}

		public bool Timeout
		{
			get { return timeout; }
		}

		public decimal TimeoutRssiThresh
		{
			get { return timeoutRssiThresh; }
			set
			{
				timeoutRssiThresh = value;
				OnPropertyChanged("TimeoutRssiThresh");
			}
		}

		public decimal TimeoutRxStart
		{
			get { return timeoutRxStart; }
			set
			{
				timeoutRxStart = value;
				OnPropertyChanged("TimeoutRxStart");
			}
		}

		public bool TxReady
		{
			get { return txReady; }
		}

		public string Version
		{
			get { return version; }
			set
			{
				if (version != value)
				{
					version = value;
					OnPropertyChanged("Version");
				}
			}
		}
	}
}