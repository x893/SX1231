namespace SemtechLib.Controls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Threading;
    using System.Windows.Forms;

    public class Led : Control
    {
        private bool _checked;
        private Size itemSize = new Size(11, 11);
        private ContentAlignment ledAlign = ContentAlignment.MiddleCenter;
        private Color ledColor = Color.Green;

        public new event PaintEventHandler Paint;

        public Led()
        {
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            base.SetStyle(ControlStyles.DoubleBuffer, true);
            base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            base.SetStyle(ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.ResizeRedraw, true);
            this.BackColor = Color.Transparent;
            base.Size = new Size(15, 15);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.Paint != null)
            {
                this.Paint(this, e);
            }
            else
            {
                base.OnPaint(e);
                float num = 1f - (((float) base.Width) / ((float) base.Height));
                float angle = 50f - (15f * num);
                Rectangle rect = new Rectangle(this.PosFromAlignment.X, this.PosFromAlignment.Y, this.itemSize.Width, this.itemSize.Height);
                LinearGradientBrush brush = new LinearGradientBrush(rect, ControlPaint.Dark(base.Parent.BackColor), ControlPaint.LightLight(base.Parent.BackColor), angle);
                Blend blend = new Blend();
                blend.Positions = new float[] { 0f, 0.2f, 0.4f, 0.6f, 0.8f, 1f };
                blend.Factors = new float[] { 0.2f, 0.2f, 0.4f, 0.4f, 1f, 1f };
                brush.Blend = blend;
                Rectangle rectangle2 = rect;
                rectangle2.Inflate(1, 1);
                e.Graphics.FillEllipse(brush, rectangle2);
                if (base.Enabled)
                {
                    if (this.Checked)
                    {
                        e.Graphics.FillEllipse(new SolidBrush(ControlPaint.Light(this.ledColor)), this.PosFromAlignment.X, this.PosFromAlignment.Y, this.itemSize.Width, this.itemSize.Height);
                    }
                    else
                    {
                        e.Graphics.FillEllipse(new SolidBrush(ControlPaint.Dark(this.ledColor)), this.PosFromAlignment.X, this.PosFromAlignment.Y, this.itemSize.Width, this.itemSize.Height);
                    }
                }
                LinearGradientBrush brush2 = new LinearGradientBrush(rect, Color.FromArgb(150, 0xff, 0xff, 0xff), Color.Transparent, angle);
                LinearGradientBrush brush3 = new LinearGradientBrush(rect, Color.FromArgb(100, 0xff, 0xff, 0xff), Color.FromArgb(100, 0xff, 0xff, 0xff), angle);
                Blend blend2 = new Blend();
                blend2.Positions = new float[] { 0f, 0.2f, 0.4f, 0.6f, 0.8f, 1f };
                blend2.Factors = new float[] { 0.2f, 0.2f, 0.4f, 0.4f, 1f, 1f };
                brush2.Blend = blend2;
                brush3.Blend = blend2;
                e.Graphics.FillEllipse(brush3, (int) (this.PosFromAlignment.X + ((this.itemSize.Width * 13) / 100)), (int) (this.PosFromAlignment.Y + ((this.itemSize.Height * 13) / 100)), (int) ((this.itemSize.Width * 40) / 100), (int) ((this.itemSize.Height * 40) / 100));
                e.Graphics.FillEllipse(brush2, new Rectangle(this.PosFromAlignment, this.itemSize));
            }
        }

        [DefaultValue(false), Category("Appearance"), Description("Indicates whether the component is in the checked state")]
        public bool Checked
        {
            get
            {
                return this._checked;
            }
            set
            {
                this._checked = value;
                base.Invalidate();
            }
        }

        [DefaultValue(0x20), Category("Appearance"), Description("Indicates how the LED should be aligned")]
        public ContentAlignment LedAlign
        {
            get
            {
                return this.ledAlign;
            }
            set
            {
                this.ledAlign = value;
                base.Invalidate();
            }
        }

        [Category("Appearance"), Description("Indicates the color of the LED")]
        public Color LedColor
        {
            get
            {
                return this.ledColor;
            }
            set
            {
                this.ledColor = value;
                base.Invalidate();
            }
        }

        [Description("Sets the size of the led"), Category("Layout")]
        public Size LedSize
        {
            get
            {
                return this.itemSize;
            }
            set
            {
                this.itemSize = value;
                base.Invalidate();
            }
        }

        private Point PosFromAlignment
        {
            get
            {
                Point point = new Point();
                switch (this.ledAlign)
                {
                    case ContentAlignment.TopLeft:
                        point.X = 1;
                        point.Y = 1;
                        return point;

                    case ContentAlignment.TopCenter:
                        point.X = (int) ((((double) base.Width) / 2.0) - (((double) this.itemSize.Width) / 2.0));
                        point.Y = 1;
                        return point;

                    case ContentAlignment.TopRight:
                        point.X = (base.Width - this.itemSize.Width) - 1;
                        point.Y = 1;
                        return point;

                    case ContentAlignment.MiddleLeft:
                        point.X = 1;
                        point.Y = (int) ((((double) base.Height) / 2.0) - (((double) this.itemSize.Height) / 2.0));
                        return point;

                    case ContentAlignment.MiddleCenter:
                        point.X = (int) ((((double) base.Width) / 2.0) - (((double) this.itemSize.Width) / 2.0));
                        point.Y = (int) ((((double) base.Height) / 2.0) - (((double) this.itemSize.Height) / 2.0));
                        return point;

                    case ContentAlignment.BottomCenter:
                        point.X = (int) ((((double) base.Width) / 2.0) - (((double) this.itemSize.Width) / 2.0));
                        point.Y = (base.Height - this.itemSize.Height) - 1;
                        return point;

                    case ContentAlignment.BottomRight:
                        point.X = (base.Width - this.itemSize.Width) - 1;
                        point.Y = (base.Height - this.itemSize.Height) - 1;
                        return point;

                    case ContentAlignment.MiddleRight:
                        point.X = (base.Width - this.itemSize.Width) - 1;
                        point.Y = (int) ((((double) base.Height) / 2.0) - (((double) this.itemSize.Height) / 2.0));
                        return point;

                    case ContentAlignment.BottomLeft:
                        point.X = 0;
                        point.Y = (base.Height - this.itemSize.Height) - 1;
                        return point;
                }
                point.X = 0;
                point.Y = 0;
                return point;
            }
        }
    }
}

