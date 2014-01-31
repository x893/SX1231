namespace SemtechLib.General.Interfaces
{
    using System;

    public class DocumentationChangedEventArgs : EventArgs
    {
        private string docFolder;
        private string docName;

        public DocumentationChangedEventArgs(string docFolder, string docName)
        {
            this.docFolder = docFolder;
            this.docName = docName;
        }

        public string DocFolder
        {
            get
            {
                return this.docFolder;
            }
        }

        public string DocName
        {
            get
            {
                return this.docName;
            }
        }
    }
}

