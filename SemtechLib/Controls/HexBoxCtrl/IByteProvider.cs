namespace SemtechLib.Controls.HexBoxCtrl
{
    using System;

    public interface IByteProvider
    {
        event EventHandler Changed;

        event EventHandler LengthChanged;

        void ApplyChanges();
        void DeleteBytes(long index, long length);
        bool HasChanges();
        void InsertBytes(long index, byte[] bs);
        byte ReadByte(long index);
        bool SupportsDeleteBytes();
        bool SupportsInsertBytes();
        bool SupportsWriteByte();
        void WriteByte(long index, byte value);

        long Length { get; }
    }
}

