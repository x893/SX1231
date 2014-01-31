namespace SemtechLib.Controls
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    public class ToolStripEx : ToolStrip
    {
        private bool clickThrough;
        private bool suppressHighlighting = true;

        protected override void WndProc(ref Message m)
        {
            if (((m.Msg != 0x200L) || !this.suppressHighlighting) || base.TopLevelControl.ContainsFocus)
            {
                base.WndProc(ref m);
                if (((m.Msg == 0x21L) && this.clickThrough) && (m.Result == ((IntPtr) 2L)))
                {
                    m.Result = (IntPtr) 1L;
                }
            }
        }

        [Category("Extended"), DefaultValue("false")]
        public bool ClickThrough
        {
            get
            {
                return this.clickThrough;
            }
            set
            {
                this.clickThrough = value;
            }
        }

        [DefaultValue("true"), Category("Extended")]
        public bool SuppressHighlighting
        {
            get
            {
                return this.suppressHighlighting;
            }
            set
            {
                this.suppressHighlighting = value;
            }
        }
    }
}

