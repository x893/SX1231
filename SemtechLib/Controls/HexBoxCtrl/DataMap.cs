namespace SemtechLib.Controls.HexBoxCtrl
{
    using System;
    using System.Collections;

    internal class DataMap : ICollection, IEnumerable
    {
        internal int _count;
        internal DataBlock _firstBlock;
        private readonly object _syncRoot;
        internal int _version;

        public DataMap()
        {
            this._syncRoot = new object();
        }

        public DataMap(IEnumerable collection)
        {
            this._syncRoot = new object();
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            foreach (DataBlock block in collection)
            {
                this.AddLast(block);
            }
        }

        public void AddAfter(DataBlock block, DataBlock newBlock)
        {
            this.AddAfterInternal(block, newBlock);
        }

        private void AddAfterInternal(DataBlock block, DataBlock newBlock)
        {
            newBlock._previousBlock = block;
            newBlock._nextBlock = block._nextBlock;
            newBlock._map = this;
            if (block._nextBlock != null)
            {
                block._nextBlock._previousBlock = newBlock;
            }
            block._nextBlock = newBlock;
            this._version++;
            this._count++;
        }

        public void AddBefore(DataBlock block, DataBlock newBlock)
        {
            this.AddBeforeInternal(block, newBlock);
        }

        private void AddBeforeInternal(DataBlock block, DataBlock newBlock)
        {
            newBlock._nextBlock = block;
            newBlock._previousBlock = block._previousBlock;
            newBlock._map = this;
            if (block._previousBlock != null)
            {
                block._previousBlock._nextBlock = newBlock;
            }
            block._previousBlock = newBlock;
            if (this._firstBlock == block)
            {
                this._firstBlock = newBlock;
            }
            this._version++;
            this._count++;
        }

        private void AddBlockToEmptyMap(DataBlock block)
        {
            block._map = this;
            block._nextBlock = null;
            block._previousBlock = null;
            this._firstBlock = block;
            this._version++;
            this._count++;
        }

        public void AddFirst(DataBlock block)
        {
            if (this._firstBlock == null)
            {
                this.AddBlockToEmptyMap(block);
            }
            else
            {
                this.AddBeforeInternal(this._firstBlock, block);
            }
        }

        public void AddLast(DataBlock block)
        {
            if (this._firstBlock == null)
            {
                this.AddBlockToEmptyMap(block);
            }
            else
            {
                this.AddAfterInternal(this.GetLastBlock(), block);
            }
        }

        public void Clear()
        {
            DataBlock nextBlock;
            for (DataBlock block = this.FirstBlock; block != null; block = nextBlock)
            {
                nextBlock = block.NextBlock;
                this.InvalidateBlock(block);
            }
            this._firstBlock = null;
            this._count = 0;
            this._version++;
        }

        public void CopyTo(Array array, int index)
        {
            DataBlock[] blockArray = array as DataBlock[];
            for (DataBlock block = this.FirstBlock; block != null; block = block.NextBlock)
            {
                blockArray[index++] = block;
            }
        }

        public IEnumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        private DataBlock GetLastBlock()
        {
            DataBlock block = null;
            for (DataBlock block2 = this.FirstBlock; block2 != null; block2 = block2.NextBlock)
            {
                block = block2;
            }
            return block;
        }

        private void InvalidateBlock(DataBlock block)
        {
            block._map = null;
            block._nextBlock = null;
            block._previousBlock = null;
        }

        public void Remove(DataBlock block)
        {
            this.RemoveInternal(block);
        }

        public void RemoveFirst()
        {
            if (this._firstBlock == null)
            {
                throw new InvalidOperationException("The collection is empty.");
            }
            this.RemoveInternal(this._firstBlock);
        }

        private void RemoveInternal(DataBlock block)
        {
            DataBlock block2 = block._previousBlock;
            DataBlock block3 = block._nextBlock;
            if (block2 != null)
            {
                block2._nextBlock = block3;
            }
            if (block3 != null)
            {
                block3._previousBlock = block2;
            }
            if (this._firstBlock == block)
            {
                this._firstBlock = block3;
            }
            this.InvalidateBlock(block);
            this._count--;
            this._version++;
        }

        public void RemoveLast()
        {
            if (this._firstBlock == null)
            {
                throw new InvalidOperationException("The collection is empty.");
            }
            this.RemoveInternal(this.GetLastBlock());
        }

        public DataBlock Replace(DataBlock block, DataBlock newBlock)
        {
            this.AddAfterInternal(block, newBlock);
            this.RemoveInternal(block);
            return newBlock;
        }

        public int Count
        {
            get
            {
                return this._count;
            }
        }

        public DataBlock FirstBlock
        {
            get
            {
                return this._firstBlock;
            }
        }

        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        public object SyncRoot
        {
            get
            {
                return this._syncRoot;
            }
        }

        internal class Enumerator : IEnumerator, IDisposable
        {
            private DataBlock _current;
            private int _index;
            private DataMap _map;
            private int _version;

            internal Enumerator(DataMap map)
            {
                this._map = map;
                this._version = map._version;
                this._current = null;
                this._index = -1;
            }

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                if (this._version != this._map._version)
                {
                    throw new InvalidOperationException("Collection was modified after the enumerator was instantiated.");
                }
                if (this._index >= this._map.Count)
                {
                    return false;
                }
                if (++this._index == 0)
                {
                    this._current = this._map.FirstBlock;
                }
                else
                {
                    this._current = this._current.NextBlock;
                }
                return (this._index < this._map.Count);
            }

            void IEnumerator.Reset()
            {
                if (this._version != this._map._version)
                {
                    throw new InvalidOperationException("Collection was modified after the enumerator was instantiated.");
                }
                this._index = -1;
                this._current = null;
            }

            object IEnumerator.Current
            {
                get
                {
                    if ((this._index < 0) || (this._index > this._map.Count))
                    {
                        throw new InvalidOperationException("Enumerator is positioned before the first element or after the last element of the collection.");
                    }
                    return this._current;
                }
            }
        }
    }
}

