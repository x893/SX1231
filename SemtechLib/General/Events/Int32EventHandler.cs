namespace SemtechLib.General.Events
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [Serializable, ComVisible(true)]
    public delegate void Int32EventHandler(object sender, Int32EventArg e);
}

