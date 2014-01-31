namespace SemtechLib.Devices.SX1231.Events
{
    using SemtechLib.Devices.SX1231.Enumerations;
    using System;

    public class LimitCheckStatusEventArg : EventArgs
    {
        private string message;
        private LimitCheckStatusEnum status;

        public LimitCheckStatusEventArg(LimitCheckStatusEnum status, string message)
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

        public LimitCheckStatusEnum Status
        {
            get
            {
                return this.status;
            }
        }
    }
}

