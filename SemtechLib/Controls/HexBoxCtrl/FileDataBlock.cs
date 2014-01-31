namespace SemtechLib.Controls.HexBoxCtrl
{
    using System;

    internal sealed class FileDataBlock : DataBlock
    {
        private long _fileOffset;
        private long _length;

        public FileDataBlock(long fileOffset, long length)
        {
            this._fileOffset = fileOffset;
            this._length = length;
        }

        public override void RemoveBytes(long position, long count)
        {
            if (position > this._length)
            {
                throw new ArgumentOutOfRangeException("offset");
            }
            if ((position + count) > this._length)
            {
                throw new ArgumentOutOfRangeException("count");
            }
            long num = position;
            long num2 = this._fileOffset;
            long length = (this._length - count) - num;
            long fileOffset = (this._fileOffset + position) + count;
            if ((num > 0L) && (length > 0L))
            {
                this._fileOffset = num2;
                this._length = num;
                base._map.AddAfter(this, new FileDataBlock(fileOffset, length));
            }
            else if (num > 0L)
            {
                this._fileOffset = num2;
                this._length = num;
            }
            else
            {
                this._fileOffset = fileOffset;
                this._length = length;
            }
        }

        public void RemoveBytesFromEnd(long count)
        {
            if (count > this._length)
            {
                throw new ArgumentOutOfRangeException("count");
            }
            this._length -= count;
        }

        public void RemoveBytesFromStart(long count)
        {
            if (count > this._length)
            {
                throw new ArgumentOutOfRangeException("count");
            }
            this._fileOffset += count;
            this._length -= count;
        }

        public void SetFileOffset(long value)
        {
            this._fileOffset = value;
        }

        public long FileOffset
        {
            get
            {
                return this._fileOffset;
            }
        }

        public override long Length
        {
            get
            {
                return this._length;
            }
        }
    }
}

