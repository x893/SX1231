namespace SemtechLib.Controls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;

    public class Jumper : Control
    {
        private bool _checked;
        private Size itemSize = new Size();
        private ContentAlignment jumperAlign = ContentAlignment.MiddleCenter;

        public new event PaintEventHandler Paint;

        public Jumper()
        {
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            base.SetStyle(ControlStyles.DoubleBuffer, true);
            base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            base.SetStyle(ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.ResizeRedraw, true);

            BackColor = Color.Transparent;

            base.Size = new Size(0x13, 0x23);
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
                itemSize.Width = (base.Size.Width * 0x42) / 100;
                itemSize.Height = (base.Size.Height * 0x5c) / 100;
                if (base.Enabled)
                {
                    Size size = new Size((itemSize.Width * 40) / 100, (itemSize.Width * 40) / 100);
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(150, 150, 150)), PosFromAlignment.X, PosFromAlignment.Y, itemSize.Width, itemSize.Height);
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(0, 0, 0)), (PosFromAlignment.X + (itemSize.Width / 2)) - (size.Width / 2), (PosFromAlignment.Y + (itemSize.Height / 4)) - (size.Height / 2), size.Width, size.Height);
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(0, 0, 0)), (PosFromAlignment.X + (itemSize.Width / 2)) - (size.Width / 2), (PosFromAlignment.Y + (itemSize.Height / 2)) - (size.Height / 2), size.Width, size.Height);
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(0, 0, 0)), (PosFromAlignment.X + (itemSize.Width / 2)) - (size.Width / 2), (PosFromAlignment.Y + (3 * (itemSize.Height / 4))) - (size.Height / 2), size.Width, size.Height);
                    if (Checked)
                    {
                        e.Graphics.FillRectangle(new SolidBrush(ForeColor), PosFromAlignment.X, PosFromAlignment.Y, itemSize.Width, 3 * (itemSize.Height / 5));
                    }
                    else
                    {
                        e.Graphics.FillRectangle(new SolidBrush(ForeColor), PosFromAlignment.X, PosFromAlignment.Y + (2 * (itemSize.Height / 5)), itemSize.Width, 3 * (itemSize.Height / 5));
                    }
                }
                else
                {
                    Size size2 = new Size((itemSize.Width * 40) / 100, (itemSize.Width * 40) / 100);
                    e.Graphics.FillRectangle(new SolidBrush(SystemColors.InactiveCaption), PosFromAlignment.X, PosFromAlignment.Y, itemSize.Width, itemSize.Height);
                    e.Graphics.FillRectangle(new SolidBrush(SystemColors.InactiveBorder), (PosFromAlignment.X + (itemSize.Width / 2)) - (size2.Width / 2), (PosFromAlignment.Y + (itemSize.Height / 4)) - (size2.Height / 2), size2.Width, size2.Height);
                    e.Graphics.FillRectangle(new SolidBrush(SystemColors.InactiveBorder), (PosFromAlignment.X + (itemSize.Width / 2)) - (size2.Width / 2), (PosFromAlignment.Y + (itemSize.Height / 2)) - (size2.Height / 2), size2.Width, size2.Height);
                    e.Graphics.FillRectangle(new SolidBrush(SystemColors.InactiveBorder), (PosFromAlignment.X + (itemSize.Width / 2)) - (size2.Width / 2), (PosFromAlignment.Y + (3 * (itemSize.Height / 4))) - (size2.Height / 2), size2.Width, size2.Height);
                }
            }
        }

        [DefaultValue(false), Description("Indicates whether the component is in the checked state"), Category("Appearance")]
        public bool Checked
        {
            get
            {
                return _checked;
            }
            set
            {
                _checked = value;
                base.Invalidate();
            }
        }

        [Description("Indicates how the Jumper should be aligned"), Category("Appearance"), DefaultValue(0x20)]
        public ContentAlignment JumperAlign
        {
            get
            {
                return jumperAlign;
            }
            set
            {
                jumperAlign = value;
                base.Invalidate();
            }
        }

        private Point PosFromAlignment
        {
            get
            {
                Point point = new Point();
                switch (jumperAlign)
                {
                    case ContentAlignment.TopLeft:
                        point.X = 0;
                        point.Y = 0;
                        return point;

                    case ContentAlignment.TopCenter:
                        point.X = (int) ((((double) base.Width) / 2.0) - (((double) itemSize.Width) / 2.0));
                        point.Y = 0;
                        return point;

                    case ContentAlignment.TopRight:
                        point.X = base.Width - itemSize.Width;
                        point.Y = 0;
                        return point;

                    case ContentAlignment.MiddleLeft:
                        point.X = 0;
                        point.Y = (int) ((((double) base.Height) / 2.0) - (((double) itemSize.Height) / 2.0));
                        return point;

                    case ContentAlignment.MiddleCenter:
                        point.X = (int) ((((double) base.Width) / 2.0) - (((double) itemSize.Width) / 2.0));
                        point.Y = (int) ((((double) base.Height) / 2.0) - (((double) itemSize.Height) / 2.0));
                        return point;

                    case ContentAlignment.BottomCenter:
                        point.X = (int) ((((double) base.Width) / 2.0) - (((double) itemSize.Width) / 2.0));
                        point.Y = base.Height - itemSize.Height;
                        return point;

                    case ContentAlignment.BottomRight:
                        point.X = base.Width - itemSize.Width;
                        point.Y = base.Height - itemSize.Height;
                        return point;

                    case ContentAlignment.MiddleRight:
                        point.X = base.Width - itemSize.Width;
                        point.Y = (int) ((((double) base.Height) / 2.0) - (((double) itemSize.Height) / 2.0));
                        return point;

                    case ContentAlignment.BottomLeft:
                        point.X = 0;
                        point.Y = base.Height - itemSize.Height;
                        return point;
                }
                point.X = 0;
                point.Y = 0;
                return point;
            }
        }
    }
}

