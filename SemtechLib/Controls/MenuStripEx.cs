namespace SemtechLib.Controls
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    public class MenuStripEx : MenuStrip
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

        [DefaultValue("false"), Category("Extended")]
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

        [Category("Extended"), DefaultValue("true")]
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

