namespace SemtechLib.Controls.HexBoxCtrl
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal struct BytePositionInfo
    {
        private int _characterPosition;
        private long _index;
        public BytePositionInfo(long index, int characterPosition)
        {
            this._index = index;
            this._characterPosition = characterPosition;
        }

        public int CharacterPosition
        {
            get
            {
                return this._characterPosition;
            }
        }
        public long Index
        {
            get
            {
                return this._index;
            }
        }
    }
}

