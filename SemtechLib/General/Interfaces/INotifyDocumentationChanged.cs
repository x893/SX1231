namespace SemtechLib.General.Interfaces
{
    using System;

    public interface INotifyDocumentationChanged
    {
        event DocumentationChangedEventHandler DocumentationChanged;
    }
}

