namespace SemtechLib.General.Events
{
    using System;

    public class ProgressEventArg : EventArgs
    {
        private ulong progress;

        public ProgressEventArg(ulong progress)
        {
            this.progress = progress;
        }

        public ulong Progress
        {
            get
            {
                return this.progress;
            }
        }
    }
}

