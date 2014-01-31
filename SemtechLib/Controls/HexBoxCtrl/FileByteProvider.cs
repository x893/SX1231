namespace SemtechLib.Controls.HexBoxCtrl
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Reflection;
    using System.Threading;

    public class FileByteProvider : IByteProvider, IDisposable
    {
        private string _fileName;
        private FileStream _fileStream;
        private bool _readOnly;
        private WriteCollection _writes = new WriteCollection();

        public event EventHandler Changed;

        public event EventHandler LengthChanged;

        public FileByteProvider(string fileName)
        {
            this._fileName = fileName;
            try
            {
                this._fileStream = File.Open(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
            }
            catch
            {
                try
                {
                    this._fileStream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    this._readOnly = true;
                }
                catch
                {
                    throw;
                }
            }
        }

        public void ApplyChanges()
        {
            if (this._readOnly)
            {
                throw new Exception("File is in read-only mode.");
            }
            if (this.HasChanges())
            {
                IDictionaryEnumerator enumerator = this._writes.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    long key = (long) enumerator.Key;
                    byte num2 = (byte) enumerator.Value;
                    if (this._fileStream.Position != key)
                    {
                        this._fileStream.Position = key;
                    }
                    this._fileStream.Write(new byte[] { num2 }, 0, 1);
                }
                this._writes.Clear();
            }
        }

        public void DeleteBytes(long index, long length)
        {
            throw new NotSupportedException("FileByteProvider.DeleteBytes");
        }

        public void Dispose()
        {
            if (this._fileStream != null)
            {
                this._fileName = null;
                this._fileStream.Close();
                this._fileStream = null;
            }
            GC.SuppressFinalize(this);
        }

        ~FileByteProvider()
        {
            this.Dispose();
        }

        public bool HasChanges()
        {
            return (this._writes.Count > 0);
        }

        public void InsertBytes(long index, byte[] bs)
        {
            throw new NotSupportedException("FileByteProvider.InsertBytes");
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
            if (this._writes.Contains(index))
            {
                return this._writes[index];
            }
            if (this._fileStream.Position != index)
            {
                this._fileStream.Position = index;
            }
            return (byte) this._fileStream.ReadByte();
        }

        public void RejectChanges()
        {
            this._writes.Clear();
        }

        public bool SupportsDeleteBytes()
        {
            return false;
        }

        public bool SupportsInsertBytes()
        {
            return false;
        }

        public bool SupportsWriteByte()
        {
            return !this._readOnly;
        }

        public void WriteByte(long index, byte value)
        {
            if (this._writes.Contains(index))
            {
                this._writes[index] = value;
            }
            else
            {
                this._writes.Add(index, value);
            }
            this.OnChanged(EventArgs.Empty);
        }

        public string FileName
        {
            get
            {
                return this._fileName;
            }
        }

        public long Length
        {
            get
            {
                return this._fileStream.Length;
            }
        }

        private class WriteCollection : DictionaryBase
        {
            public void Add(long index, byte value)
            {
                base.Dictionary.Add(index, value);
            }

            public bool Contains(long index)
            {
                return base.Dictionary.Contains(index);
            }

            public byte this[long index]
            {
                get
                {
                    return (byte) base.Dictionary[index];
                }
                set
                {
                    base.Dictionary[index] = value;
                }
            }
        }
    }
}

