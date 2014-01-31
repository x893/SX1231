namespace SemtechLib.General.Events
{
    using System;

    public class ErrorEventArgs : EventArgs
    {
        private string message;
        private byte status;

        public ErrorEventArgs(byte status, string message)
        {
            this.status = status;
            this.message = message;
        }

        public string Message
        {
            get
            {
                return this.message;
            }
        }

        public byte Status
        {
            get
            {
                return this.status;
            }
        }
    }
}

