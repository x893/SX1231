namespace SemtechLib.Ftdi
{
    using FTD2XX_NET;
    using System;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;

    public class IoPort : FtdiIoPort
    {
        public override event FtdiIoPort.IoChangedEventHandler Io0Changed;
        public override event FtdiIoPort.IoChangedEventHandler Io1Changed;
        public override event FtdiIoPort.IoChangedEventHandler Io2Changed;
        public override event FtdiIoPort.IoChangedEventHandler Io3Changed;
        public override event FtdiIoPort.IoChangedEventHandler Io4Changed;
        public override event FtdiIoPort.IoChangedEventHandler Io5Changed;
        public override event FtdiIoPort.IoChangedEventHandler Io6Changed;
        public override event FtdiIoPort.IoChangedEventHandler Io7Changed;

        public IoPort(string device)
        {
            base.Device = device;
            base.portDir = 0;
            base.portValue = 0;
        }

        public override bool Init(uint frequency)
        {
            lock (base.syncThread)
            {
                base.ftStatus = base.SetBaudRate(frequency);
                if (base.ftStatus != FTDI.FT_STATUS.FT_OK)
                {
                    base.isInitialized = false;
                    return false;
                }
                base.ftStatus = base.SetBitMode(0, 1);
                if (base.ftStatus != FTDI.FT_STATUS.FT_OK)
                {
                    base.isInitialized = false;
                    return false;
                }
                base.readThreadContinue = true;
                base.readThread = new Thread(new ThreadStart(ReadThread));
                base.readThread.Start();
                base.isInitialized = true;
                return true;
            }
        }

        private void OnIo0Changed(bool state)
        {
            if (this.Io0Changed != null)
            {
                this.Io0Changed(this, new FtdiIoPort.IoChangedEventArgs(state));
            }
        }

        private void OnIo1Changed(bool state)
        {
            if (this.Io1Changed != null)
            {
                this.Io1Changed(this, new FtdiIoPort.IoChangedEventArgs(state));
            }
        }

        private void OnIo2Changed(bool state)
        {
            if (this.Io2Changed != null)
            {
                this.Io2Changed(this, new FtdiIoPort.IoChangedEventArgs(state));
            }
        }

        private void OnIo3Changed(bool state)
        {
            if (this.Io3Changed != null)
            {
                this.Io3Changed(this, new FtdiIoPort.IoChangedEventArgs(state));
            }
        }

        private void OnIo4Changed(bool state)
        {
            if (this.Io4Changed != null)
            {
                this.Io4Changed(this, new FtdiIoPort.IoChangedEventArgs(state));
            }
        }

        private void OnIo5Changed(bool state)
        {
            if (this.Io5Changed != null)
            {
                this.Io5Changed(this, new FtdiIoPort.IoChangedEventArgs(state));
            }
        }

        private void OnIo6Changed(bool state)
        {
            if (this.Io6Changed != null)
            {
                this.Io6Changed(this, new FtdiIoPort.IoChangedEventArgs(state));
            }
        }

        private void OnIo7Changed(bool state)
        {
            if (this.Io7Changed != null)
            {
                this.Io7Changed(this, new FtdiIoPort.IoChangedEventArgs(state));
            }
        }

        public override bool ReadBytes(out byte[] rxBuffer, uint bitCount)
        {
            lock (base.syncThread)
            {
                throw new NotImplementedException();
            }
        }

        private new void ReadThread()
        {
            byte bitMode = 0;
            while (base.readThreadContinue)
            {
                if (!base.isInitialized)
                {
                    Application.DoEvents();
                    Thread.Sleep(10);
                }
                else
                {
                    lock (base.syncThread)
                    {
                        base.ftStatus = base.GetPinStates(ref bitMode);
                    }
                    if (base.ftStatus == FTDI.FT_STATUS.FT_OK)
                    {
                        if ((bitMode & 0x80) == 0x80)
                        {
                            this.OnIo7Changed(true);
                        }
                        else
                        {
                            this.OnIo7Changed(false);
                        }
                        if ((bitMode & 0x40) == 0x40)
                        {
                            this.OnIo6Changed(true);
                        }
                        else
                        {
                            this.OnIo6Changed(false);
                        }
                        if ((bitMode & 0x20) == 0x20)
                        {
                            this.OnIo5Changed(true);
                        }
                        else
                        {
                            this.OnIo5Changed(false);
                        }
                        if ((bitMode & 0x10) == 0x10)
                        {
                            this.OnIo4Changed(true);
                        }
                        else
                        {
                            this.OnIo4Changed(false);
                        }
                        if ((bitMode & 8) == 8)
                        {
                            this.OnIo3Changed(true);
                        }
                        else
                        {
                            this.OnIo3Changed(false);
                        }
                        if ((bitMode & 4) == 4)
                        {
                            this.OnIo2Changed(true);
                        }
                        else
                        {
                            this.OnIo2Changed(false);
                        }
                        if ((bitMode & 2) == 2)
                        {
                            this.OnIo1Changed(true);
                        }
                        else
                        {
                            this.OnIo1Changed(false);
                        }
                        if ((bitMode & 1) == 1)
                        {
                            this.OnIo0Changed(true);
                        }
                        else
                        {
                            this.OnIo0Changed(false);
                        }
                    }
                    else
                    {
                        lock (base.syncThread)
                        {
                            base.Close();
                        }
                    }
                    Thread.Sleep(0);
                }
            }
        }

        public override bool SendBytes()
        {
            lock (base.syncThread)
            {
                byte[] array = new byte[base.txBuffer.Count];
                uint numBytesWritten = 0;
                base.txBuffer.CopyTo(array);
                base.ftStatus = base.Write(array, array.Length, ref numBytesWritten);
                base.txBuffer.Clear();
                return (base.ftStatus == FTDI.FT_STATUS.FT_OK);
            }
        }

        public override byte PortDir
        {
            get
            {
                return base.portDir;
            }
            set
            {
                base.portDir = value;
                base.SetBitMode(base.portDir, 1);
            }
        }

        public override byte PortValue
        {
            get
            {
                return base.portValue;
            }
            set
            {
                base.portValue = value;
                base.txBuffer.Add(base.portValue);
                base.txBuffer.Add(base.portValue);
                this.SendBytes();
            }
        }
    }
}

