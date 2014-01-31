namespace SemtechLib.General.Events
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [Serializable, ComVisible(true)]
    public delegate void ByteArrayEventHandler(object sender, ByteArrayEventArg e);
}

