namespace SemtechLib.Controls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;

    public class TempCtrl : Control
    {
        private Image backgroundImg;
        private SolidBrush blackBrush;
        private LinearGradientBrush bulbBrush;
        private Color colorBack;
        private Color colorBackground;
        private Color colorFore;
        private Color colorOutline;
        private Color colorScale;
        private Color colorScaleText;
        private bool drawTics = true;
        private bool enableTransparentBackground;
        private SolidBrush fillBrush;
        private Font fntText = new Font("Arial", 10f, FontStyle.Bold);
        private Pen forePen;
        private float fRange;
        private float fTmpWidth;
        private int largeTicFreq = 10;
        private Pen outlinePen;
        private PointF pointCenter;
        private Ranges range = new Ranges();
        private RectangleF rectBackgroundImg;
        private RectangleF rectBulb;
        private RectangleF rectCylinder;
        private bool requiresRedraw;
        private Pen scalePen;
        private int smallTicFreq = 5;
        private StringFormat strfmtText = new StringFormat();
        private double value = 25.0;

        public new event PaintEventHandler Paint;

        public TempCtrl()
        {
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            base.SetStyle(ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.ResizeRedraw, true);
            base.Size = new Size(0x4b, 0xfd);
            this.ForeColor = Color.Red;
            this.colorFore = this.ForeColor;
            this.colorBack = SystemColors.Control;
            this.colorScale = Color.FromArgb(0, 0, 0);
            this.colorScaleText = Color.FromArgb(0, 0, 0);
            this.colorOutline = Color.FromArgb(0x40, 0, 0);
            this.colorBackground = Color.FromKnownColor(KnownColor.Transparent);
            base.EnabledChanged += new EventHandler(this.TempCtrl_EnabledChanged);
            base.SizeChanged += new EventHandler(this.TempCtrl_SizeChanged);
        }

        private double Celsius2Fahrenheit(double celsius)
        {
            return ((celsius * 1.8) + 32.0);
        }

        private void DrawBulb(Graphics g, RectangleF rect, bool enabled)
        {
            g.FillEllipse(this.bulbBrush, this.rectBulb);
            g.DrawEllipse(this.outlinePen, this.rectBulb);
        }

        private void DrawCylinder(Graphics g, RectangleF rect, bool enabled)
        {
            this.FillCylinder(g, this.rectCylinder, this.fillBrush, this.colorOutline);
        }

        private void DrawTicks(Graphics g, RectangleF rect, bool enabled)
        {
            if (this.drawTics)
            {
                Point point;
                Point point2;
                PointF tf;
                this.fRange = (float) (this.Range.Max - this.Range.Min);
                Font font = new Font("Arial", 7f);
                StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Far;
                format.LineAlignment = StringAlignment.Center;
                float num = this.rectCylinder.Height / this.fRange;
                float num2 = num * this.largeTicFreq;
                long max = (long) this.Range.Max;
                for (float i = this.rectCylinder.Top; i <= this.rectCylinder.Bottom; i += num2)
                {
                    point = new Point(((int) this.rectCylinder.Right) + 3, (int) i);
                    point2 = new Point(((int) this.rectCylinder.Right) + 10, (int) i);
                    g.DrawLine(this.scalePen, point, point2);
                    tf = new PointF(this.rectCylinder.Right + 30f, i);
                    g.DrawString(max.ToString(), font, this.blackBrush, tf, format);
                    max -= this.largeTicFreq;
                }
                num2 = num * this.smallTicFreq;
                for (float j = this.rectCylinder.Top; j <= this.rectCylinder.Bottom; j += num2)
                {
                    point = new Point(((int) this.rectCylinder.Right) + 3, (int) j);
                    point2 = new Point(((int) this.rectCylinder.Right) + 8, (int) j);
                    g.DrawLine(this.scalePen, point, point2);
                }
                double num6 = this.Celsius2Fahrenheit(this.Range.Max);
                int num7 = (int) (num6 % 10.0);
                if (num7 != 0)
                {
                    num7 = 10 - num7;
                }
                num6 -= num7;
                this.fRange = (float) (this.Celsius2Fahrenheit(this.Range.Max) - this.Celsius2Fahrenheit(this.Range.Min));
                num = this.rectCylinder.Height / this.fRange;
                num2 = num * this.largeTicFreq;
                max = (long) this.Celsius2Fahrenheit(this.Range.Min);
                for (float k = this.rectCylinder.Bottom; k >= this.rectCylinder.Top; k -= num2)
                {
                    point = new Point(((int) this.rectCylinder.Left) - 10, (int) k);
                    point2 = new Point(((int) this.rectCylinder.Left) - 3, (int) k);
                    g.DrawLine(this.scalePen, point, point2);
                    tf = new PointF(this.rectCylinder.Left - 15f, k);
                    g.DrawString(max.ToString(), font, this.blackBrush, tf, format);
                    max += this.largeTicFreq;
                }
                num2 = num * this.smallTicFreq;
                for (float m = this.rectCylinder.Bottom; m >= this.rectCylinder.Top; m -= num2)
                {
                    point = new Point(((int) this.rectCylinder.Left) - 8, (int) m);
                    point2 = new Point(((int) this.rectCylinder.Left) - 3, (int) m);
                    g.DrawLine(this.scalePen, point, point2);
                }
            }
        }

        private void DrawValue(Graphics g, RectangleF rect, bool enabled)
        {
            if (enabled)
            {
                this.fRange = (float) (this.Range.Max - this.Range.Min);
                float num = (float) this.value;
                if (this.Range.Min < 0.0)
                {
                    num += Math.Abs((int) this.Range.Min);
                }
                if (num > 0f)
                {
                    float num2 = (num / this.fRange) * 100f;
                    float height = (this.rectCylinder.Height / 100f) * num2;
                    RectangleF ctrl = new RectangleF(this.rectCylinder.Left, this.rectCylinder.Bottom - height, this.rectCylinder.Width, height);
                    this.FillCylinder(g, ctrl, this.bulbBrush, this.colorOutline);
                }
                RectangleF layoutRectangle = new RectangleF(this.pointCenter.X + 10f, this.rectBulb.Bottom + 5f, 70f, 20f);
                g.DrawString(this.value.ToString("0 [\x00b0C]"), this.fntText, this.blackBrush, layoutRectangle, this.strfmtText);
                layoutRectangle = new RectangleF(this.pointCenter.X - 80f, this.rectBulb.Bottom + 5f, 70f, 20f);
                g.DrawString(this.Celsius2Fahrenheit(this.value).ToString("0 [\x00b0F]"), this.fntText, this.blackBrush, layoutRectangle, this.strfmtText);
            }
        }

        private double Fahrenheit2Celsius(double fahrenheit)
        {
            return ((fahrenheit - 32.0) / 1.8);
        }

        protected void FillCylinder(Graphics g, RectangleF ctrl, Brush fillBrush, Color outlineColor)
        {
            RectangleF rect = new RectangleF(ctrl.X, ctrl.Y - 5f, ctrl.Width, 5f);
            RectangleF ef2 = new RectangleF(ctrl.X, ctrl.Bottom - 5f, ctrl.Width, 5f);
            Pen pen = new Pen(outlineColor);
            GraphicsPath path = new GraphicsPath();
            path.AddArc(rect, 0f, 180f);
            path.AddArc(ef2, 180f, -180f);
            path.CloseFigure();
            g.FillPath(fillBrush, path);
            g.DrawPath(pen, path);
            path.Reset();
            path.AddEllipse(rect);
            g.FillPath(fillBrush, path);
            g.DrawPath(pen, path);
        }

        protected Color OffsetColor(Color color, short offset)
        {
            byte r = 0;
            byte g = 0;
            byte b = 0;

            short nR = offset;
            short nG = offset;
            short nB = offset;
            if ((offset < -255) || (offset > 255))
            {
                return color;
            }
            r = color.R;
            g = color.G;
            b = color.B;
            if (offset > 0)
            {
                if ((r + offset) > 255)
                {
                    nR = (short) (255 - r);
                }
                if ((g + offset) > 255)
                {
                    nG = (short) (255 - g);
                }
                if ((b + offset) > 255)
                {
                    nB = (short) (255 - b);
                }
                offset = Math.Min(Math.Min(nR, nG), nB);
            }
            else
            {
                if ((r + offset) < 0)
                {
                    nR = (short)(-(short)r);
                }
                if ((g + offset) < 0)
                {
                    nG = (short)(-(short)g);
                }
                if ((b + offset) < 0)
                {
                    nB = (short)(-(short)b);
                }
                offset = Math.Max(Math.Max(nR, nG), nB);
            }
            return Color.FromArgb(color.A, r + offset, g + offset, b + offset);
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
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                Image image = new Bitmap(base.Width, base.Height);
                Graphics g = Graphics.FromImage(image);
                g.SmoothingMode = SmoothingMode.HighQuality;
                RectangleF rect = new RectangleF(0f, 0f, (float) base.Width, (float) base.Height);
                this.DrawValue(g, rect, base.Enabled);
                e.Graphics.DrawImage(image, rect);
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (!this.enableTransparentBackground)
            {
                base.OnPaintBackground(e);
            }
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.FillRectangle(new SolidBrush(Color.Transparent), new RectangleF(0f, 0f, (float) base.Width, (float) base.Height));
            if ((this.backgroundImg == null) || this.requiresRedraw)
            {
                this.backgroundImg = new Bitmap(base.Width, base.Height);
                Graphics g = Graphics.FromImage(this.backgroundImg);
                g.SmoothingMode = SmoothingMode.HighQuality;
                this.rectBackgroundImg = new RectangleF(0f, 0f, (float) base.Width, (float) base.Height);
                this.pointCenter = new PointF(this.rectBackgroundImg.Left + (this.rectBackgroundImg.Width / 2f), this.rectBackgroundImg.Top + (this.rectBackgroundImg.Height / 2f));
                this.fTmpWidth = this.rectBackgroundImg.Width / 5f;
                this.rectBulb = new RectangleF(this.pointCenter.X - this.fTmpWidth, this.rectBackgroundImg.Bottom - ((this.fTmpWidth * 2f) + 25f), this.fTmpWidth * 2f, this.fTmpWidth * 2f);
                this.rectCylinder = new RectangleF(this.pointCenter.X - (this.fTmpWidth / 2f), this.rectBackgroundImg.Top + (this.drawTics ? ((float) 0x19) : ((float) 10)), this.fTmpWidth, (this.rectBulb.Top - this.rectBackgroundImg.Top) - (this.drawTics ? ((float) 20) : ((float) 5)));
                if (!base.Enabled)
                {
                    this.colorFore = SystemColors.ControlDark;
                    this.colorScale = SystemColors.GrayText;
                    this.colorScaleText = SystemColors.GrayText;
                    this.colorOutline = SystemColors.ControlDark;
                }
                else
                {
                    this.colorFore = this.ForeColor;
                    this.colorScale = Color.FromArgb(0, 0, 0);
                    this.colorScaleText = Color.FromArgb(0, 0, 0);
                    this.colorOutline = Color.FromArgb(0x40, 0, 0);
                }
                this.forePen = new Pen(this.colorFore);
                this.scalePen = new Pen(this.colorScale);
                this.outlinePen = new Pen(this.colorOutline);
                this.blackBrush = new SolidBrush(this.colorScaleText);
                this.fillBrush = new SolidBrush(this.colorBack);
                this.bulbBrush = new LinearGradientBrush(this.rectBulb, this.OffsetColor(this.colorFore, 0x37), this.OffsetColor(this.colorFore, -55), LinearGradientMode.Horizontal);
                this.strfmtText.Alignment = StringAlignment.Center;
                this.strfmtText.LineAlignment = StringAlignment.Center;
                this.DrawBulb(g, this.rectBackgroundImg, base.Enabled);
                this.DrawCylinder(g, this.rectBackgroundImg, base.Enabled);
                RectangleF rect = new RectangleF(this.rectCylinder.X, this.rectCylinder.Y - 5f, this.rectCylinder.Width, 5f);
                g.DrawEllipse(this.outlinePen, rect);
                this.DrawTicks(g, this.rectBackgroundImg, base.Enabled);
                this.requiresRedraw = false;
            }
            e.Graphics.DrawImage(this.backgroundImg, this.rectBackgroundImg);
        }

        private void TempCtrl_EnabledChanged(object sender, EventArgs e)
        {
            this.requiresRedraw = true;
            this.Refresh();
        }

        private void TempCtrl_SizeChanged(object sender, EventArgs e)
        {
            this.requiresRedraw = true;
            this.Refresh();
        }

        public bool DrawTics
        {
            get
            {
                return this.drawTics;
            }
            set
            {
                this.drawTics = value;
                this.requiresRedraw = true;
                base.Invalidate();
            }
        }

        [DefaultValue(false), Description("Enables or Disables Transparent Background color. Note: Enabling this will reduce the performance and may make the control flicker.")]
        public bool EnableTransparentBackground
        {
            get
            {
                return this.enableTransparentBackground;
            }
            set
            {
                this.enableTransparentBackground = value;
                base.SetStyle(ControlStyles.OptimizedDoubleBuffer, !this.enableTransparentBackground);
                this.requiresRedraw = true;
                this.Refresh();
            }
        }

        public int LargeTicFreq
        {
            get
            {
                return this.largeTicFreq;
            }
            set
            {
                this.largeTicFreq = value;
                this.requiresRedraw = true;
                base.Invalidate();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Ranges Range
        {
            get
            {
                return this.range;
            }
            set
            {
                this.range = value;
                this.requiresRedraw = true;
                base.Invalidate();
            }
        }

        public int SmallTicFreq
        {
            get
            {
                return this.smallTicFreq;
            }
            set
            {
                this.smallTicFreq = value;
                this.requiresRedraw = true;
                base.Invalidate();
            }
        }

        public double Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
                if (value > this.range.Max)
                {
                    this.value = this.range.Max;
                }
                if (value < this.range.Min)
                {
                    this.value = this.range.Min;
                }
                base.Invalidate();
            }
        }

        [TypeConverter(typeof(TempCtrl.RangeTypeConverter)), Category("Behavior"), Description("Range.")]
        public class Ranges
        {
            private double max;
            private double min;

            public event PropertyChangedEventHandler PropertyChanged;

            public Ranges()
            {
                this.min = -40.0;
                this.Max = 365.0;
            }

            public Ranges(double min, double max)
            {
                this.min = min;
                this.max = max;
            }

            public override string ToString()
            {
                return (this.max + "; " + this.min);
            }

            [Description("Maximum value.")]
            public double Max
            {
                get
                {
                    return this.max;
                }
                set
                {
                    this.max = value;
                    if (this.PropertyChanged != null)
                    {
                        this.PropertyChanged();
                    }
                }
            }

            [Description("Minimum value.")]
            public double Min
            {
                get
                {
                    return this.min;
                }
                set
                {
                    this.min = value;
                    if (this.PropertyChanged != null)
                    {
                        this.PropertyChanged();
                    }
                }
            }

            public delegate void PropertyChangedEventHandler();
        }

        public class RangeTypeConverter : TypeConverter
        {
            public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
            {
                return TypeDescriptor.GetProperties(typeof(TempCtrl.Ranges));
            }

            public override bool GetPropertiesSupported(ITypeDescriptorContext context)
            {
                return true;
            }
        }
    }
}

