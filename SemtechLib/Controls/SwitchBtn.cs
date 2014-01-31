namespace SemtechLib.Controls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;

    public class SwitchBtn : Control
    {
        private bool _checked;
        private ContentAlignment controlAlign = ContentAlignment.MiddleCenter;
        private Size itemSize = new Size();

        public new event PaintEventHandler Paint;

        public SwitchBtn()
        {
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            base.SetStyle(ControlStyles.DoubleBuffer, true);
            base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            base.SetStyle(ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.ResizeRedraw, true);
            this.BackColor = Color.Transparent;
            base.Width = 15;
            base.Height = 0x19;
            this.itemSize.Width = 10;
            this.itemSize.Height = 0x17;
            base.MouseDown += new MouseEventHandler(this.mouseDown);
            base.MouseUp += new MouseEventHandler(this.mouseUp);
        }

        protected void buttonDown()
        {
            base.Invalidate();
        }

        protected void buttonUp()
        {
            this.Checked = !this.Checked;
            base.Invalidate();
        }

        protected void mouseDown(object sender, MouseEventArgs e)
        {
            this.buttonDown();
        }

        protected void mouseUp(object sender, MouseEventArgs e)
        {
            this.buttonUp();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (Paint != null)
            {
                Paint(this, e);
            }
            else
            {
                base.OnPaint(e);
                if (base.Enabled)
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(0xff, 0, 0)), this.PosFromAlignment.X, this.PosFromAlignment.Y, this.itemSize.Width, this.itemSize.Height);
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(150, 150, 150)), (int) (this.PosFromAlignment.X + 2), (int) (this.PosFromAlignment.Y + 5), (int) (this.itemSize.Width - 4), (int) (this.itemSize.Height - 10));
                    if (this.Checked)
                    {
                        e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(0, 0, 0)), (int) (this.PosFromAlignment.X + 3), (int) (this.PosFromAlignment.Y + 6), (int) (this.itemSize.Width - 6), (int) (this.itemSize.Height - 0x10));
                    }
                    else
                    {
                        e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(0, 0, 0)), (int) (this.PosFromAlignment.X + 3), (int) (this.PosFromAlignment.Y + 10), (int) (this.itemSize.Width - 6), (int) (this.itemSize.Height - 0x10));
                    }
                }
                else
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(200, 120, 120)), this.PosFromAlignment.X, this.PosFromAlignment.Y, this.itemSize.Width, this.itemSize.Height);
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(150, 150, 150)), (int) (this.PosFromAlignment.X + 2), (int) (this.PosFromAlignment.Y + 5), (int) (this.itemSize.Width - 4), (int) (this.itemSize.Height - 10));
                    if (this.Checked)
                    {
                        e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(100, 100, 100)), (int) (this.PosFromAlignment.X + 3), (int) (this.PosFromAlignment.Y + 6), (int) (this.itemSize.Width - 6), (int) (this.itemSize.Height - 0x10));
                    }
                    else
                    {
                        e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(100, 100, 100)), (int) (this.PosFromAlignment.X + 3), (int) (this.PosFromAlignment.Y + 10), (int) (this.itemSize.Width - 6), (int) (this.itemSize.Height - 0x10));
                    }
                }
            }
        }

        [DefaultValue(false), Description("Indicates whether the component is in the checked state"), Category("Appearance")]
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
        public ContentAlignment ControlAlign
        {
            get
            {
                return this.controlAlign;
            }
            set
            {
                this.controlAlign = value;
                base.Invalidate();
            }
        }

        protected Size ItemSize
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
                switch (this.controlAlign)
                {
                    case ContentAlignment.TopLeft:
                        point.X = 0;
                        point.Y = 0;
                        return point;

                    case ContentAlignment.TopCenter:
                        point.X = (int) ((((double) base.Width) / 2.0) - (((double) this.itemSize.Width) / 2.0));
                        point.Y = 0;
                        return point;

                    case ContentAlignment.TopRight:
                        point.X = base.Width - this.itemSize.Width;
                        point.Y = 0;
                        return point;

                    case ContentAlignment.MiddleLeft:
                        point.X = 0;
                        point.Y = (int) ((((double) base.Height) / 2.0) - (((double) this.itemSize.Height) / 2.0));
                        return point;

                    case ContentAlignment.MiddleCenter:
                        point.X = (int) ((((double) base.Width) / 2.0) - (((double) this.itemSize.Width) / 2.0));
                        point.Y = (int) ((((double) base.Height) / 2.0) - (((double) this.itemSize.Height) / 2.0));
                        return point;

                    case ContentAlignment.BottomCenter:
                        point.X = (int) ((((double) base.Width) / 2.0) - (((double) this.itemSize.Width) / 2.0));
                        point.Y = base.Height - this.itemSize.Height;
                        return point;

                    case ContentAlignment.BottomRight:
                        point.X = base.Width - this.itemSize.Width;
                        point.Y = base.Height - this.itemSize.Height;
                        return point;

                    case ContentAlignment.MiddleRight:
                        point.X = base.Width - this.itemSize.Width;
                        point.Y = (int) ((((double) base.Height) / 2.0) - (((double) this.itemSize.Height) / 2.0));
                        return point;

                    case ContentAlignment.BottomLeft:
                        point.X = 0;
                        point.Y = base.Height - this.itemSize.Height;
                        return point;
                }
                point.X = 0;
                point.Y = 0;
                return point;
            }
        }
    }
}

