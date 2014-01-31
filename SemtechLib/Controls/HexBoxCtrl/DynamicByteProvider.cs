namespace SemtechLib.Controls.HexBoxCtrl
{
    using System;
    using System.Threading;

    public class DynamicByteProvider : IByteProvider
    {
        private ByteCollection _bytes;
        private bool _hasChanges;

        public event EventHandler Changed;

        public event EventHandler LengthChanged;

        public DynamicByteProvider(byte[] data) : this(new ByteCollection(data))
        {
        }

        public DynamicByteProvider(ByteCollection bytes)
        {
            this._bytes = bytes;
        }

        public void ApplyChanges()
        {
            this._hasChanges = false;
        }

        public void DeleteBytes(long index, long length)
        {
            int num = (int) Math.Max(0L, index);
            int count = (int) Math.Min((long) ((int) this.Length), length);
            this._bytes.RemoveRange(num, count);
            this.OnLengthChanged(EventArgs.Empty);
            this.OnChanged(EventArgs.Empty);
        }

        public bool HasChanges()
        {
            return this._hasChanges;
        }

        public void InsertBytes(long index, byte[] bs)
        {
            this._bytes.InsertRange((int) index, bs);
            this.OnLengthChanged(EventArgs.Empty);
            this.OnChanged(EventArgs.Empty);
        }

        private void OnChanged(EventArgs e)
        {
            this._hasChanges = true;
            if (this.Changed != null)
            {
                this.Changed(this, e);
            }
        }

        private void OnLengthChanged(EventArgs e)
        {
            if (this.LengthChanged != null)
            {
                this.LengthChanged(this, e);
            }
        }

        public byte ReadByte(long index)
        {
            return this._bytes[(int) index];
        }

        public bool SupportsDeleteBytes()
        {
            return true;
        }

        public bool SupportsInsertBytes()
        {
            return true;
        }

        public bool SupportsWriteByte()
        {
            return true;
        }

        public void WriteByte(long index, byte value)
        {
            this._bytes[(int) index] = value;
            this.OnChanged(EventArgs.Empty);
        }

        public ByteCollection Bytes
        {
            get
            {
                return this._bytes;
            }
        }

        public long Length
        {
            get
            {
                return (long) this._bytes.Count;
            }
        }
    }
}

