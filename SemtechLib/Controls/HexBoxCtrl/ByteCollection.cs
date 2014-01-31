namespace SemtechLib.Controls.HexBoxCtrl
{
    using System;
    using System.Collections;
    using System.Reflection;

    public class ByteCollection : CollectionBase
    {
        public ByteCollection()
        {
        }

        public ByteCollection(byte[] bs)
        {
            this.AddRange(bs);
        }

        public void Add(byte b)
        {
            base.List.Add(b);
        }

        public void AddRange(byte[] bs)
        {
            base.InnerList.AddRange(bs);
        }

        public bool Contains(byte b)
        {
            return base.InnerList.Contains(b);
        }

        public void CopyTo(byte[] bs, int index)
        {
            base.InnerList.CopyTo(bs, index);
        }

        public byte[] GetBytes()
        {
            byte[] array = new byte[base.Count];
            base.InnerList.CopyTo(0, array, 0, array.Length);
            return array;
        }

        public int IndexOf(byte b)
        {
            return base.InnerList.IndexOf(b);
        }

        public void Insert(int index, byte b)
        {
            base.InnerList.Insert(index, b);
        }

        public void InsertRange(int index, byte[] bs)
        {
            base.InnerList.InsertRange(index, bs);
        }

        public void Remove(byte b)
        {
            base.List.Remove(b);
        }

        public void RemoveRange(int index, int count)
        {
            base.InnerList.RemoveRange(index, count);
        }

        public byte[] ToArray()
        {
            byte[] bs = new byte[base.Count];
            this.CopyTo(bs, 0);
            return bs;
        }

        public byte this[int index]
        {
            get
            {
                return (byte) base.List[index];
            }
            set
            {
                base.List[index] = value;
            }
        }
    }
}

