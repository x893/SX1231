using SemtechLib.General.Interfaces;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace SX1231SKB
{
    public class HelpForm : Form
    {
        private IContainer components;
        private string docPath = (Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName) + @"\Doc");
        private WebBrowser docViewer;

        public HelpForm()
        {
            this.InitializeComponent();
            if (File.Exists(this.docPath + @"\overview.html"))
            {
                this.docViewer.Navigate(this.docPath + @"\overview.html");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(HelpForm));
            this.docViewer = new WebBrowser();
            base.SuspendLayout();
            this.docViewer.AllowWebBrowserDrop = false;
            this.docViewer.Dock = DockStyle.Fill;
            this.docViewer.IsWebBrowserContextMenuEnabled = false;
            this.docViewer.Location = new Point(0, 0);
            this.docViewer.MinimumSize = new Size(20, 20);
            this.docViewer.Name = "docViewer";
            this.docViewer.Size = new Size(0x124, 0x252);
            this.docViewer.TabIndex = 2;
            this.docViewer.TabStop = false;
            this.docViewer.Url = new Uri("", UriKind.Relative);
            this.docViewer.WebBrowserShortcutsEnabled = false;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0x124, 0x252);
            base.Controls.Add(this.docViewer);
            base.Icon = (Icon) resources.GetObject("$this.Icon");
            base.Name = "HelpForm";
            base.StartPosition = FormStartPosition.Manual;
            this.Text = "HelpForm";
            base.ResumeLayout(false);
        }

        public void UpdateDocument(DocumentationChangedEventArgs e)
        {
            string path = this.docPath + @"\" + e.DocFolder + @"\" + e.DocName + ".html";
            if (File.Exists(path))
            {
                this.docViewer.Navigate(path);
            }
            else if (File.Exists(this.docPath + @"\overview.html"))
            {
                this.docViewer.Navigate(this.docPath + @"\overview.html");
            }
        }
    }
}

