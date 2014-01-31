namespace SemtechLib.Devices.SX1231.General
{
    using SemtechLib.Devices.SX1231;
    using SemtechLib.General.Events;
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Threading;

    public class DataLog : INotifyPropertyChanged
    {
        private CultureInfo ci = CultureInfo.InvariantCulture;
        private string fileName = "sx1231-Rssi.log";
        private FileStream fileStream;
        private ulong maxSamples = 0x3e8L;
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
            if (this.sx1231.RfPaSwitchEnabled != 0)
            {
                str = "#\tTime\tRF_PA RSSI\tRF_IO RSSI";
            }
            else
            {
                str = "#\tTime\tRSSI";
            }
            this.streamWriter.WriteLine("#\tSX1231 data log generated the " + DateTime.Now.ToShortDateString() + " at " + DateTime.Now.ToShortTimeString());
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

        private void sx1231_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (this.sx1231.RfPaSwitchEnabled != 0)
            {
                string propertyName = e.PropertyName;
                if (propertyName != null)
                {
                    if (!(propertyName == "RfPaRssiValue"))
                    {
                        if (!(propertyName == "RfIoRssiValue"))
                        {
                            return;
                        }
                    }
                    else
                    {
                        if (this.sx1231.RfPaSwitchEnabled == 1)
                        {
                            this.Update();
                        }
                        return;
                    }
                    this.Update();
                }
            }
            else
            {
                string str2;
                if (((str2 = e.PropertyName) != null) && (str2 == "RssiValue"))
                {
                    this.Update();
                }
            }
        }

        private void Update()
        {
            string str = "\t";
            if ((this.sx1231 != null) && this.state)
            {
                if ((this.samples < this.maxSamples) || (this.maxSamples == 0L))
                {
                    if (this.sx1231.RfPaSwitchEnabled != 0)
                    {
                        string str2 = str;
                        str = str2 + DateTime.Now.ToString("HH:mm:ss.fff", this.ci) + "\t" + this.sx1231.RfPaRssiValue.ToString("F1") + "\t" + this.sx1231.RfIoRssiValue.ToString("F1");
                    }
                    else
                    {
                        str = str + DateTime.Now.ToString("HH:mm:ss.fff", this.ci) + "\t" + this.sx1231.RssiValue.ToString("F1");
                    }
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
                }
            }
        }
    }
}

