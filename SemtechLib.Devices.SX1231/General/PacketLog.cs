namespace SemtechLib.Devices.SX1231.General
{
    using SemtechLib.Devices.SX1231;
    using SemtechLib.Devices.SX1231.Enumerations;
    using SemtechLib.Devices.SX1231.Events;
    using SemtechLib.General.Events;
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Threading;

    public class PacketLog : INotifyPropertyChanged
    {
        private CultureInfo ci = CultureInfo.InvariantCulture;
        private string fileName = "sx1231-pkt.log";
        private FileStream fileStream;
        private int maxPacketNumber;
        private ulong maxSamples = 0x3e8L;
        private int packetNumber;
        private string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        private ulong samples;
        private bool state;
        private StreamWriter streamWriter;
        private SemtechLib.Devices.SX1231.SX1231 sx1231;

        public event ProgressEventHandler ProgressChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler Stoped;

        private void GenerateFileHeader()
        {
            string str = "";
            str = "#\tTime\tMode\tRssi\tPkt Max\tPkt #\tPreamble Size\tSync\tLength\tNode Address\tMessage\tCRC";
            this.streamWriter.WriteLine("#\tSX1231 packet log generated the " + DateTime.Now.ToShortDateString() + " at " + DateTime.Now.ToShortTimeString());
            this.streamWriter.WriteLine(str);
        }

        private void OnProgressChanged(ulong progress)
        {
            if (this.ProgressChanged != null)
            {
                this.ProgressChanged(this, new ProgressEventArg(progress));
            }
        }

        private void OnPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        private void OnStop()
        {
            if (this.Stoped != null)
            {
                this.Stoped(this, EventArgs.Empty);
            }
        }

        public void Start()
        {
            try
            {
                this.fileStream = new FileStream(this.path + @"\" + this.fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
                this.streamWriter = new StreamWriter(this.fileStream, Encoding.ASCII);
                this.GenerateFileHeader();
                this.samples = 0L;
                this.state = true;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public void Stop()
        {
            try
            {
                this.state = false;
                this.streamWriter.Close();
            }
            catch (Exception)
            {
            }
        }

        private void sx1231_PacketHandlerReceived(object sender, PacketStatusEventArg e)
        {
            this.maxPacketNumber = e.Max;
            this.packetNumber = e.Number;
            this.Update();
        }

        private void sx1231_PacketHandlerStarted(object sender, EventArgs e)
        {
        }

        private void sx1231_PacketHandlerStoped(object sender, EventArgs e)
        {
        }

        private void sx1231_PacketHandlerTransmitted(object sender, PacketStatusEventArg e)
        {
            this.maxPacketNumber = e.Max;
            this.packetNumber = e.Number;
            this.Update();
        }

        private void sx1231_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string propertyName = e.PropertyName;
            if (propertyName != null)
            {
                bool flag1 = propertyName == "RssiValue";
            }
        }

        private void Update()
        {
            string str = "\t";
            if ((this.sx1231 != null) && this.state)
            {
                if ((this.samples < this.maxSamples) || (this.maxSamples == 0L))
                {
                    str = ((((str + DateTime.Now.ToString("HH:mm:ss.fff", this.ci) + "\t") + ((this.sx1231.Mode == OperatingModeEnum.Tx) ? "Tx\t" : ((this.sx1231.Mode == OperatingModeEnum.Rx) ? "Rx\t" : "\t")) + ((this.sx1231.Mode == OperatingModeEnum.Rx) ? (this.sx1231.Packet.Rssi.ToString("F1") + "\t") : "\t")) + this.maxPacketNumber.ToString() + "\t") + this.packetNumber.ToString() + "\t") + this.sx1231.Packet.PreambleSize.ToString() + "\t";
                    MaskValidationType type = new MaskValidationType(this.sx1231.Packet.SyncValue);
                    str = (str + type.StringValue + "\t") + this.sx1231.Packet.MessageLength.ToString("X02") + "\t";
                    if (this.sx1231.Mode == OperatingModeEnum.Rx)
                    {
                        str = str + ((this.sx1231.Packet.AddressFiltering != AddressFilteringEnum.OFF) ? this.sx1231.Packet.NodeAddressRx.ToString("X02") : "");
                    }
                    else
                    {
                        str = str + ((this.sx1231.Packet.AddressFiltering != AddressFilteringEnum.OFF) ? this.sx1231.Packet.NodeAddress.ToString("X02") : "");
                    }
                    str = str + "\t";
                    if ((this.sx1231.Packet.Message != null) && (this.sx1231.Packet.Message.Length != 0))
                    {
                        int index = 0;
                        while (index < (this.sx1231.Packet.Message.Length - 1))
                        {
                            str = str + this.sx1231.Packet.Message[index].ToString("X02") + "-";
                            index++;
                        }
                        str = str + this.sx1231.Packet.Message[index].ToString("X02") + "\t";
                    }
                    str = str + (this.sx1231.Packet.CrcOn ? (((this.sx1231.Packet.Crc >> 8)).ToString("X02") + "-" + ((this.sx1231.Packet.Crc & 0xff)).ToString("X02") + "\t") : "\t");
                    this.streamWriter.WriteLine(str);
                    if (this.maxSamples != 0L)
                    {
                        this.samples += (ulong) 1L;
                        this.OnProgressChanged((ulong) ((this.samples * 100M) / this.maxSamples));
                    }
                    else
                    {
                        this.OnProgressChanged(0L);
                    }
                }
                else
                {
                    this.OnStop();
                }
            }
        }

        public string FileName
        {
            get
            {
                return this.fileName;
            }
            set
            {
                this.fileName = value;
                this.OnPropertyChanged("FileName");
            }
        }

        public ulong MaxSamples
        {
            get
            {
                return this.maxSamples;
            }
            set
            {
                this.maxSamples = value;
                this.OnPropertyChanged("MaxSamples");
            }
        }

        public string Path
        {
            get
            {
                return this.path;
            }
            set
            {
                this.path = value;
                this.OnPropertyChanged("Path");
            }
        }

        public SemtechLib.Devices.SX1231.SX1231 SX1231
        {
            set
            {
                if (this.sx1231 != value)
                {
                    this.sx1231 = value;
                    this.sx1231.PropertyChanged += new PropertyChangedEventHandler(this.sx1231_PropertyChanged);
                    this.sx1231.PacketHandlerStarted += new EventHandler(this.sx1231_PacketHandlerStarted);
                    this.sx1231.PacketHandlerStoped += new EventHandler(this.sx1231_PacketHandlerStoped);
                    this.sx1231.PacketHandlerTransmitted += new SemtechLib.Devices.SX1231.SX1231.PacketHandlerTransmittedEventHandler(this.sx1231_PacketHandlerTransmitted);
                    this.sx1231.PacketHandlerReceived += new SemtechLib.Devices.SX1231.SX1231.PacketHandlerReceivedEventHandler(this.sx1231_PacketHandlerReceived);
                }
            }
        }

        public enum PacketHandlerModeEnum
        {
            IDLE,
            RX,
            TX
        }
    }
}

