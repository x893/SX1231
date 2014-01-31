namespace SemtechLib.Controls.HexBoxCtrl
{
    using System;

    internal abstract class DataBlock
    {
        internal DataMap _map;
        internal DataBlock _nextBlock;
        internal DataBlock _previousBlock;

        protected DataBlock()
        {
        }

        public abstract void RemoveBytes(long position, long count);

        public abstract long Length { get; }

        public DataMap Map
        {
            get
            {
                return this._map;
            }
        }

        public DataBlock NextBlock
        {
            get
            {
                return this._nextBlock;
            }
        }

        public DataBlock PreviousBlock
        {
            get
            {
                return this._previousBlock;
            }
        }
    }
}

