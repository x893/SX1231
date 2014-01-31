namespace SemtechLib.Controls.HexBoxCtrl
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;

    public sealed class DynamicFileByteProvider : IByteProvider, IDisposable
    {
        private DataMap _dataMap;
        private string _fileName;
        private bool _readOnly;
        private Stream _stream;
        private long _totalLength;
        private const int COPY_BLOCK_SIZE = 0x1000;

        public event EventHandler Changed;

        public event EventHandler LengthChanged;

        public DynamicFileByteProvider(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            if (!stream.CanSeek)
            {
                throw new ArgumentException("stream must supported seek operations(CanSeek)");
            }
            this._stream = stream;
            this._readOnly = !stream.CanWrite;
            this.ReInitialize();
        }

        public DynamicFileByteProvider(string fileName) : this(fileName, false)
        {
        }

        public DynamicFileByteProvider(string fileName, bool readOnly)
        {
            this._fileName = fileName;
            if (!readOnly)
            {
                this._stream = File.Open(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
            }
            else
            {
                this._stream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            }
            this._readOnly = readOnly;
            this.ReInitialize();
        }

        public void ApplyChanges()
        {
            if (this._readOnly)
            {
                throw new OperationCanceledException("File is in read-only mode");
            }
            if (this._totalLength > this._stream.Length)
            {
                this._stream.SetLength(this._totalLength);
            }
            long dataOffset = 0L;
            for (DataBlock block = this._dataMap.FirstBlock; block != null; block = block.NextBlock)
            {
                FileDataBlock fileBlock = block as FileDataBlock;
                if ((fileBlock != null) && (fileBlock.FileOffset != dataOffset))
                {
                    this.MoveFileBlock(fileBlock, dataOffset);
                }
                dataOffset += block.Length;
            }
            dataOffset = 0L;
            for (DataBlock block3 = this._dataMap.FirstBlock; block3 != null; block3 = block3.NextBlock)
            {
                MemoryDataBlock block4 = block3 as MemoryDataBlock;
                if (block4 != null)
                {
                    this._stream.Position = dataOffset;
                    for (int i = 0; i < block4.Length; i += 0x1000)
                    {
                        this._stream.Write(block4.Data, i, (int) Math.Min((long) 0x1000L, (long) (block4.Length - i)));
                    }
                }
                dataOffset += block3.Length;
            }
            this._stream.SetLength(this._totalLength);
            this.ReInitialize();
        }

        public void DeleteBytes(long index, long length)
        {
            try
            {
                long num2;
                DataBlock nextBlock;
                long num = length;
                for (DataBlock block = this.GetDataBlock(index, out num2); (num > 0L) && (block != null); block = (num > 0L) ? nextBlock : null)
                {
                    long num3 = block.Length;
                    nextBlock = block.NextBlock;
                    long count = Math.Min(num, num3 - (index - num2));
                    block.RemoveBytes(index - num2, count);
                    if (block.Length == 0L)
                    {
                        this._dataMap.Remove(block);
                        if (this._dataMap.FirstBlock == null)
                        {
                            this._dataMap.AddFirst(new MemoryDataBlock(new byte[0]));
                        }
                    }
                    num -= count;
                    num2 += block.Length;
                }
            }
            finally
            {
                this._totalLength -= length;
                this.OnLengthChanged(EventArgs.Empty);
                this.OnChanged(EventArgs.Empty);
            }
        }

        public void Dispose()
        {
            if (this._stream != null)
            {
                this._stream.Close();
                this._stream = null;
            }
            this._fileName = null;
            this._dataMap = null;
            GC.SuppressFinalize(this);
        }

        ~DynamicFileByteProvider()
        {
            this.Dispose();
        }

        private DataBlock GetDataBlock(long findOffset, out long blockOffset)
        {
            if ((findOffset < 0L) || (findOffset > this._totalLength))
            {
                throw new ArgumentOutOfRangeException("index");
            }
            blockOffset = 0L;
            for (DataBlock block = this._dataMap.FirstBlock; block != null; block = block.NextBlock)
            {
                if ((blockOffset <= findOffset) && ((blockOffset + block.Length) > findOffset))
                {
                    return block;
                }
                if (block.NextBlock == null)
                {
                    return block;
                }
                blockOffset += block.Length;
            }
            return null;
        }

        private FileDataBlock GetNextFileDataBlock(DataBlock block, long dataOffset, out long nextDataOffset)
        {
            nextDataOffset = dataOffset + block.Length;
            block = block.NextBlock;
            while (block != null)
            {
                FileDataBlock block2 = block as FileDataBlock;
                if (block2 != null)
                {
                    return block2;
                }
                nextDataOffset += block.Length;
                block = block.NextBlock;
            }
            return null;
        }

        public bool HasChanges()
        {
            if (this._readOnly)
            {
                return false;
            }
            if (this._totalLength != this._stream.Length)
            {
                return true;
            }
            long num = 0L;
            for (DataBlock block = this._dataMap.FirstBlock; block != null; block = block.NextBlock)
            {
                FileDataBlock block2 = block as FileDataBlock;
                if (block2 == null)
                {
                    return true;
                }
                if (block2.FileOffset != num)
                {
                    return true;
                }
                num += block2.Length;
            }
            return (num != this._stream.Length);
        }

        public void InsertBytes(long index, byte[] bs)
        {
            try
            {
                long num;
                DataBlock dataBlock = this.GetDataBlock(index, out num);
                MemoryDataBlock block2 = dataBlock as MemoryDataBlock;
                if (block2 != null)
                {
                    block2.InsertBytes(index - num, bs);
                }
                else
                {
                    FileDataBlock block3 = (FileDataBlock) dataBlock;
                    if ((num == index) && (dataBlock.PreviousBlock != null))
                    {
                        MemoryDataBlock previousBlock = dataBlock.PreviousBlock as MemoryDataBlock;
                        if (previousBlock != null)
                        {
                            previousBlock.InsertBytes(previousBlock.Length, bs);
                            return;
                        }
                    }
                    FileDataBlock newBlock = null;
                    if (index > num)
                    {
                        newBlock = new FileDataBlock(block3.FileOffset, index - num);
                    }
                    FileDataBlock block6 = null;
                    if (index < (num + block3.Length))
                    {
                        block6 = new FileDataBlock((block3.FileOffset + index) - num, block3.Length - (index - num));
                    }
                    dataBlock = this._dataMap.Replace(dataBlock, new MemoryDataBlock(bs));
                    if (newBlock != null)
                    {
                        this._dataMap.AddBefore(dataBlock, newBlock);
                    }
                    if (block6 != null)
                    {
                        this._dataMap.AddAfter(dataBlock, block6);
                    }
                }
            }
            finally
            {
                this._totalLength += bs.Length;
                this.OnLengthChanged(EventArgs.Empty);
                this.OnChanged(EventArgs.Empty);
            }
        }

        private void MoveFileBlock(FileDataBlock fileBlock, long dataOffset)
        {
            long num;
            FileDataBlock block = this.GetNextFileDataBlock(fileBlock, dataOffset, out num);
            if ((block != null) && ((dataOffset + fileBlock.Length) > block.FileOffset))
            {
                this.MoveFileBlock(block, num);
            }
            if (fileBlock.FileOffset > dataOffset)
            {
                byte[] buffer = new byte[0x1000];
                for (long i = 0L; i < fileBlock.Length; i += buffer.Length)
                {
                    long num3 = fileBlock.FileOffset + i;
                    int count = (int) Math.Min((long) buffer.Length, fileBlock.Length - i);
                    this._stream.Position = num3;
                    this._stream.Read(buffer, 0, count);
                    long num5 = dataOffset + i;
                    this._stream.Position = num5;
                    this._stream.Write(buffer, 0, count);
                }
            }
            else
            {
                byte[] buffer2 = new byte[0x1000];
                for (long j = 0L; j < fileBlock.Length; j += buffer2.Length)
                {
                    int num7 = (int) Math.Min((long) buffer2.Length, fileBlock.Length - j);
                    long num8 = ((fileBlock.FileOffset + fileBlock.Length) - j) - num7;
                    this._stream.Position = num8;
                    this._stream.Read(buffer2, 0, num7);
                    long num9 = ((dataOffset + fileBlock.Length) - j) - num7;
                    this._stream.Position = num9;
                    this._stream.Write(buffer2, 0, num7);
                }
            }
            fileBlock.SetFileOffset(dataOffset);
        }

        private void OnChanged(EventArgs e)
        {
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
            long num;
            DataBlock dataBlock = this.GetDataBlock(index, out num);
            FileDataBlock block2 = dataBlock as FileDataBlock;
            if (block2 != null)
            {
                return this.ReadByteFromFile((block2.FileOffset + index) - num);
            }
            MemoryDataBlock block3 = (MemoryDataBlock) dataBlock;
            return block3.Data[(int) ((IntPtr) (index - num))];
        }

        private byte ReadByteFromFile(long fileOffset)
        {
            if (this._stream.Position != fileOffset)
            {
                this._stream.Position = fileOffset;
            }
            return (byte) this._stream.ReadByte();
        }

        private void ReInitialize()
        {
            this._dataMap = new DataMap();
            this._dataMap.AddFirst(new FileDataBlock(0L, this._stream.Length));
            this._totalLength = this._stream.Length;
        }

        public bool SupportsDeleteBytes()
        {
            return !this._readOnly;
        }

        public bool SupportsInsertBytes()
        {
            return !this._readOnly;
        }

        public bool SupportsWriteByte()
        {
            return !this._readOnly;
        }

        public void WriteByte(long index, byte value)
        {
            try
            {
                long num;
                DataBlock dataBlock = this.GetDataBlock(index, out num);
                MemoryDataBlock block2 = dataBlock as MemoryDataBlock;
                if (block2 != null)
                {
                    block2.Data[(int) ((IntPtr) (index - num))] = value;
                }
                else
                {
                    FileDataBlock block = (FileDataBlock) dataBlock;
                    if ((num == index) && (dataBlock.PreviousBlock != null))
                    {
                        MemoryDataBlock previousBlock = dataBlock.PreviousBlock as MemoryDataBlock;
                        if (previousBlock != null)
                        {
                            previousBlock.AddByteToEnd(value);
                            block.RemoveBytesFromStart(1L);
                            if (block.Length == 0L)
                            {
                                this._dataMap.Remove(block);
                            }
                            return;
                        }
                    }
                    if ((((num + block.Length) - 1L) == index) && (dataBlock.NextBlock != null))
                    {
                        MemoryDataBlock nextBlock = dataBlock.NextBlock as MemoryDataBlock;
                        if (nextBlock != null)
                        {
                            nextBlock.AddByteToStart(value);
                            block.RemoveBytesFromEnd(1L);
                            if (block.Length == 0L)
                            {
                                this._dataMap.Remove(block);
                            }
                            return;
                        }
                    }
                    FileDataBlock newBlock = null;
                    if (index > num)
                    {
                        newBlock = new FileDataBlock(block.FileOffset, index - num);
                    }
                    FileDataBlock block7 = null;
                    if (index < ((num + block.Length) - 1L))
                    {
                        block7 = new FileDataBlock(((block.FileOffset + index) - num) + 1L, block.Length - ((index - num) + 1L));
                    }
                    dataBlock = this._dataMap.Replace(dataBlock, new MemoryDataBlock(value));
                    if (newBlock != null)
                    {
                        this._dataMap.AddBefore(dataBlock, newBlock);
                    }
                    if (block7 != null)
                    {
                        this._dataMap.AddAfter(dataBlock, block7);
                    }
                }
            }
            finally
            {
                this.OnChanged(EventArgs.Empty);
            }
        }

        public long Length
        {
            get
            {
                return this._totalLength;
            }
        }

        public bool ReadOnly
        {
            get
            {
                return this._readOnly;
            }
        }
    }
}

