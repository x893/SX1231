namespace SemtechLib.Controls
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.StatusStrip | ToolStripItemDesignerAvailability.ToolStrip)]
    public class ToolStripLed : ToolStripControlHost
    {
        public ToolStripLed() : base(new Led())
        {
        }

        public bool Checked
        {
            get
            {
                return this.led.Checked;
            }
            set
            {
                this.led.Checked = value;
            }
        }

        public Led led
        {
            get
            {
                return (base.Control as Led);
            }
        }

        public ContentAlignment LedAlign
        {
            get
            {
                return this.led.LedAlign;
            }
            set
            {
                this.led.LedAlign = value;
            }
        }

        public Color LedColor
        {
            get
            {
                return this.led.LedColor;
            }
            set
            {
                this.led.LedColor = value;
            }
        }

        public Size LedSize
        {
            get
            {
                return this.led.LedSize;
            }
            set
            {
                this.led.LedSize = value;
            }
        }
    }
}

