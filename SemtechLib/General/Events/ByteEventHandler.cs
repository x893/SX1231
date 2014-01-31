namespace SemtechLib.General.Events
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [Serializable, ComVisible(true)]
    public delegate void ByteEventHandler(object sender, ByteEventArg e);
}

