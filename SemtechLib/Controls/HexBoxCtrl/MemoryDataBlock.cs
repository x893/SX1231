namespace SemtechLib.Controls.HexBoxCtrl
{
    using System;

    internal sealed class MemoryDataBlock : DataBlock
    {
        private byte[] _data;

        public MemoryDataBlock(byte data)
        {
            this._data = new byte[] { data };
        }

        public MemoryDataBlock(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            this._data = (byte[]) data.Clone();
        }

        public void AddByteToEnd(byte value)
        {
            byte[] array = new byte[this._data.LongLength + 1L];
            this._data.CopyTo(array, 0);
            array[(int) ((IntPtr) (array.LongLength - 1L))] = value;
            this._data = array;
        }

        public void AddByteToStart(byte value)
        {
            byte[] array = new byte[this._data.LongLength + 1L];
            array[0] = value;
            this._data.CopyTo(array, 1);
            this._data = array;
        }

        public void InsertBytes(long position, byte[] data)
        {
            byte[] destinationArray = new byte[this._data.LongLength + data.LongLength];
            if (position > 0L)
            {
                Array.Copy(this._data, 0L, destinationArray, 0L, position);
            }
            Array.Copy(data, 0L, destinationArray, position, data.LongLength);
            if (position < this._data.LongLength)
            {
                Array.Copy(this._data, position, destinationArray, position + data.LongLength, this._data.LongLength - position);
            }
            this._data = destinationArray;
        }

        public override void RemoveBytes(long position, long count)
        {
            byte[] destinationArray = new byte[this._data.LongLength - count];
            if (position > 0L)
            {
                Array.Copy(this._data, 0L, destinationArray, 0L, position);
            }
            if ((position + count) < this._data.LongLength)
            {
                Array.Copy(this._data, position + count, destinationArray, position, destinationArray.LongLength - position);
            }
            this._data = destinationArray;
        }

        public byte[] Data
        {
            get
            {
                return this._data;
            }
        }

        public override long Length
        {
            get
            {
                return this._data.LongLength;
            }
        }
    }
}

