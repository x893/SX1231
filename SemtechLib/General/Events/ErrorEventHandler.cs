namespace SemtechLib.General.Events
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [Serializable, ComVisible(true)]
    public delegate void ErrorEventHandler(object sender, ErrorEventArgs e);
}

