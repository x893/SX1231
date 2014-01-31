namespace SemtechLib.Devices.SX1231.General
{
    using SemtechLib.Devices.SX1231.Enumerations;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Threading;

    public class Packet : INotifyPropertyChanged
    {
        private AddressFilteringEnum addressFiltering;
        private byte[] aesKey = new byte[0x10];
        private bool aesOn = true;
        private bool autoRxRestartOn = true;
        private byte broadcastAddress;
        private bool crcAutoClearOff;
        private bool crcOn = true;
        private DcFreeEnum dcFree;
        private EnterConditionEnum enterCondition;
        private ExitConditionEnum exitCondition;
        private FifoFillConditionEnum fifoFillCondition;
        private byte fifoThreshold = 15;
        private IntermediateModeEnum intermediateMode;
        private int interPacketRxDelay;
        private bool logEnabled;
        public const int MaxFifoSize = 0x42;
        private byte maxLengthIndex;
        private byte[] message = new byte[0];
        private readonly byte[] MessageLengthOffset = new byte[] { 0, 1, 1, 2, 0, 1, 1, 2 };
        private readonly byte[] MessageMaxLength = new byte[] { 0x42, 0x41, 0x41, 0x40, 0x40, 0x40, 0x40, 0x30 };
        private OperatingModeEnum mode = OperatingModeEnum.Stdby;
        private byte nodeAddress;
        private byte nodeAddressRx;
        private PacketFormatEnum packetFormat = PacketFormatEnum.Variable;
        private byte payloadLength = 0x42;
        private byte payloadLengthRx = 0x42;
        private readonly byte[] PayloadMaxLength = new byte[] { 0x42, 0x42, 0x42, 0x42, 0x40, 0x41, 0x41, 50 };
        private readonly byte[] PayloadMinLength = new byte[] { 1, 1, 2, 2, 1, 1, 2, 2 };
        private const ushort Polynome = 0x1021;
        private int preambleSize = 3;
        private decimal rssi = -127.5M;
        private bool syncOn = true;
        private byte syncSize = 4;
        private byte syncTol;
        private byte[] syncValue = new byte[] { 0x69, 0x81, 0x7e, 150 };
        private bool txStartCondition = true;

        public event PropertyChangedEventHandler PropertyChanged;

        public ushort ComputeCrc(byte[] packet)
        {
            ushort crc = 0x1D0F;
            for (int i = 0; i < packet.Length; i++)
            {
                crc = ComputeCrc(crc, packet[i]);
            }
            return (ushort)(~crc);
        }

        private ushort ComputeCrc(ushort crc, byte data)
        {
            for (int i = 0; i < 8; i++)
            {
                if ((((crc & 0x8000) >> 8) ^ (data & 0x80)) != 0)
                {
                    crc = (ushort) (crc << 1);
                    crc = (ushort) (crc ^ 0x1021);
                }
                else
                {
                    crc = (ushort) (crc << 1);
                }
                data = (byte) (data << 1);
            }
            return crc;
        }

        public string GetSaveData()
        {
            string str = (this.Mode == OperatingModeEnum.Tx) ? 1.ToString() : 0.ToString();
            str = (((str + ";") + ((this.AddressFiltering != AddressFilteringEnum.OFF) ? 1.ToString() : 0.ToString()) + ";") + this.payloadLength.ToString() + ";") + this.nodeAddress.ToString() + ";";
            if ((this.message == null) || (this.message.Length == 0))
            {
                return str;
            }
            int index = 0;
            while (index < (this.message.Length - 1))
            {
                str = str + this.message[index].ToString("X02") + ",";
                index++;
            }
            return (str + this.message[index].ToString("X02"));
        }

        private void OnPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        public void SetSaveData(string data)
        {
            string[] strArray = data.Split(new char[] { ';' });
            if (strArray.Length == 5)
            {
                strArray = strArray[4].Split(new char[] { ',' });
                if (this.message != null)
                {
                    Array.Resize<byte>(ref this.message, strArray.Length);
                }
                else
                {
                    this.message = new byte[strArray.Length];
                }
                for (int i = 0; i < strArray.Length; i++)
                {
                    if (strArray[i].Length != 0)
                    {
                        this.message[i] = Convert.ToByte(strArray[i], 0x10);
                    }
                }
                this.OnPropertyChanged("Message");
                this.OnPropertyChanged("MessageLength");
                this.OnPropertyChanged("PayloadLength");
                this.OnPropertyChanged("Crc");
                this.UpdatePayloadLengthMaxMin();
            }
        }

        public byte[] ToArray()
        {
            List<byte> list = new List<byte>();
            if (this.PacketFormat == PacketFormatEnum.Variable)
            {
                list.Add(this.MessageLength);
            }
            if (this.AddressFiltering != AddressFilteringEnum.OFF)
            {
                list.Add(this.NodeAddress);
            }
            for (int i = 0; i < this.Message.Length; i++)
            {
                list.Add(this.Message[i]);
            }
            return list.ToArray();
        }

        private void UpdatePayloadLengthMaxMin()
        {
            this.maxLengthIndex = this.AesOn ? ((byte) 4) : ((byte) 0);
            this.maxLengthIndex = (byte) (this.maxLengthIndex | ((this.PacketFormat == PacketFormatEnum.Variable) ? ((byte) 2) : ((byte) 0)));
            this.maxLengthIndex = (byte) (this.maxLengthIndex | ((this.AddressFiltering != AddressFilteringEnum.OFF) ? ((byte) 1) : ((byte) 0)));
            if (this.Message.Length > this.MessageMaxLength[this.maxLengthIndex])
            {
                Array.Resize<byte>(ref this.message, this.MessageMaxLength[this.maxLengthIndex]);
                this.OnPropertyChanged("Message");
                this.OnPropertyChanged("MessageLength");
                this.OnPropertyChanged("PayloadLength");
                this.OnPropertyChanged("Crc");
            }
        }

        public AddressFilteringEnum AddressFiltering
        {
            get
            {
                return this.addressFiltering;
            }
            set
            {
                this.addressFiltering = value;
                this.OnPropertyChanged("AddressFiltering");
                this.OnPropertyChanged("PayloadLength");
                this.OnPropertyChanged("MessageLength");
                this.OnPropertyChanged("Crc");
            }
        }

        public byte[] AesKey
        {
            get
            {
                return this.aesKey;
            }
            set
            {
                this.aesKey = value;
                this.OnPropertyChanged("AesKey");
            }
        }

        public bool AesOn
        {
            get
            {
                return this.aesOn;
            }
            set
            {
                this.aesOn = value;
                this.OnPropertyChanged("AesOn");
            }
        }

        public bool AutoRxRestartOn
        {
            get
            {
                return this.autoRxRestartOn;
            }
            set
            {
                this.autoRxRestartOn = value;
                this.OnPropertyChanged("AutoRxRestartOn");
            }
        }

        public byte BroadcastAddress
        {
            get
            {
                return this.broadcastAddress;
            }
            set
            {
                this.broadcastAddress = value;
                this.OnPropertyChanged("BroadcastAddress");
            }
        }

        public ushort Crc
        {
            get
            {
                byte[] array = new byte[0];
                int newSize = 0;
                int num2 = 0;
                if (this.PacketFormat == PacketFormatEnum.Variable)
                {
                    Array.Resize<byte>(ref array, ++newSize);
                    array[num2++] = this.MessageLength;
                }
                if (this.AddressFiltering != AddressFilteringEnum.OFF)
                {
                    Array.Resize<byte>(ref array, ++newSize);
                    if (this.Mode == OperatingModeEnum.Rx)
                    {
                        array[num2++] = this.NodeAddressRx;
                    }
                    else
                    {
                        array[num2++] = this.NodeAddress;
                    }
                }
                newSize += this.Message.Length;
                Array.Resize<byte>(ref array, newSize);
                for (int i = 0; i < this.Message.Length; i++)
                {
                    array[num2 + i] = this.Message[i];
                }
                return this.ComputeCrc(array);
            }
        }

        public bool CrcAutoClearOff
        {
            get
            {
                return this.crcAutoClearOff;
            }
            set
            {
                this.crcAutoClearOff = value;
                this.OnPropertyChanged("CrcAutoClearOff");
            }
        }

        public bool CrcOn
        {
            get
            {
                return this.crcOn;
            }
            set
            {
                this.crcOn = value;
                this.OnPropertyChanged("CrcOn");
                this.OnPropertyChanged("Crc");
            }
        }

        public DcFreeEnum DcFree
        {
            get
            {
                return this.dcFree;
            }
            set
            {
                this.dcFree = value;
                this.OnPropertyChanged("DcFree");
            }
        }

        public EnterConditionEnum EnterCondition
        {
            get
            {
                return this.enterCondition;
            }
            set
            {
                this.enterCondition = value;
                this.OnPropertyChanged("EnterCondition");
            }
        }

        public ExitConditionEnum ExitCondition
        {
            get
            {
                return this.exitCondition;
            }
            set
            {
                this.exitCondition = value;
                this.OnPropertyChanged("ExitCondition");
            }
        }

        public FifoFillConditionEnum FifoFillCondition
        {
            get
            {
                return this.fifoFillCondition;
            }
            set
            {
                this.fifoFillCondition = value;
                this.OnPropertyChanged("FifoFillCondition");
            }
        }

        public byte FifoThreshold
        {
            get
            {
                return this.fifoThreshold;
            }
            set
            {
                this.fifoThreshold = value;
                this.OnPropertyChanged("FifoThreshold");
            }
        }

        public IntermediateModeEnum IntermediateMode
        {
            get
            {
                return this.intermediateMode;
            }
            set
            {
                this.intermediateMode = value;
                this.OnPropertyChanged("IntermediateMode");
            }
        }

        public int InterPacketRxDelay
        {
            get
            {
                return this.interPacketRxDelay;
            }
            set
            {
                this.interPacketRxDelay = value;
                this.OnPropertyChanged("InterPacketRxDelay");
            }
        }

        public bool LogEnabled
        {
            get
            {
                return this.logEnabled;
            }
            set
            {
                this.logEnabled = value;
                this.OnPropertyChanged("LogEnabled");
            }
        }

        public byte[] Message
        {
            get
            {
                return this.message;
            }
            set
            {
                this.message = value;
                this.OnPropertyChanged("Message");
                this.OnPropertyChanged("PayloadLength");
                this.OnPropertyChanged("MessageLength");
                this.OnPropertyChanged("Crc");
            }
        }

        public byte MessageLength
        {
            get
            {
                byte num = 0;
                if (this.AddressFiltering != AddressFilteringEnum.OFF)
                {
                    num = (byte) (num + 1);
                }
                return (byte) (num + ((byte) this.Message.Length));
            }
        }

        public OperatingModeEnum Mode
        {
            get
            {
                return this.mode;
            }
            set
            {
                this.mode = value;
            }
        }

        public byte NodeAddress
        {
            get
            {
                return this.nodeAddress;
            }
            set
            {
                this.nodeAddress = value;
                this.OnPropertyChanged("NodeAddress");
                this.OnPropertyChanged("Crc");
            }
        }

        public byte NodeAddressRx
        {
            get
            {
                return this.nodeAddressRx;
            }
            set
            {
                this.nodeAddressRx = value;
                this.OnPropertyChanged("NodeAddressRx");
            }
        }

        public PacketFormatEnum PacketFormat
        {
            get
            {
                return this.packetFormat;
            }
            set
            {
                this.packetFormat = value;
                this.OnPropertyChanged("PacketFormat");
                this.OnPropertyChanged("Crc");
            }
        }

        public byte PayloadLength
        {
            get
            {
                if (this.Mode == OperatingModeEnum.Rx)
                {
                    return this.payloadLengthRx;
                }
                byte num = 0;
                if (this.PacketFormat == PacketFormatEnum.Variable)
                {
                    num = (byte) (num + 1);
                }
                if (this.AddressFiltering != AddressFilteringEnum.OFF)
                {
                    num = (byte) (num + 1);
                }
                return (byte) (num + ((byte) this.Message.Length));
            }
            set
            {
                if (this.Mode == OperatingModeEnum.Rx)
                {
                    this.payloadLengthRx = value;
                }
                else
                {
                    this.payloadLength = value;
                }
                this.OnPropertyChanged("PayloadLength");
            }
        }

        public int PreambleSize
        {
            get
            {
                return this.preambleSize;
            }
            set
            {
                this.preambleSize = value;
                this.OnPropertyChanged("PreambleSize");
            }
        }

        public decimal Rssi
        {
            get
            {
                return this.rssi;
            }
            set
            {
                this.rssi = value;
                this.OnPropertyChanged("Rssi");
            }
        }

        public bool SyncOn
        {
            get
            {
                return this.syncOn;
            }
            set
            {
                this.syncOn = value;
                this.OnPropertyChanged("SyncOn");
            }
        }

        public byte SyncSize
        {
            get
            {
                return this.syncSize;
            }
            set
            {
                this.syncSize = value;
                Array.Resize<byte>(ref this.syncValue, this.syncSize);
                this.OnPropertyChanged("SyncSize");
            }
        }

        public byte SyncTol
        {
            get
            {
                return this.syncTol;
            }
            set
            {
                this.syncTol = value;
                this.OnPropertyChanged("SyncTol");
            }
        }

        public byte[] SyncValue
        {
            get
            {
                return this.syncValue;
            }
            set
            {
                this.syncValue = value;
                this.OnPropertyChanged("SyncValue");
            }
        }

        public bool TxStartCondition
        {
            get
            {
                return this.txStartCondition;
            }
            set
            {
                this.txStartCondition = value;
                this.OnPropertyChanged("TxStartCondition");
            }
        }
    }
}

