using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;

namespace SemtechLib.Controls
{
    public class PushBtn : Control
    {
        private Color backgroudColor = Color.GhostWhite;
        private int bevelDepth;
        private int bevelHeight = 1;
        private Color borderColor = Color.Black;
        private GraphicsPath bpath;
        private Color buttonColor = Color.Aqua;
        private int buttonPressOffset;
        private Color cColor = Color.White;
        private ContentAlignment controlAlign = ContentAlignment.MiddleCenter;
        private bool dome;
        private Blend edgeBlend;
        private LinearGradientBrush edgeBrush;
        private Color edgeColor1;
        private Color edgeColor2;
        private int edgeWidth = 1;
        private bool gotFocus;
        private GraphicsPath gpath;
        private Size itemSize = new Size();
        private float lightAngle = 50f;
        private int recessDepth = 1;

        public new event PaintEventHandler Paint;

        public PushBtn()
        {
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            base.SetStyle(ControlStyles.DoubleBuffer, true);
            base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            base.SetStyle(ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.ResizeRedraw, true);
            this.BackColor = Color.Transparent;
            base.Size = new Size(0x17, 0x17);
            base.MouseDown += new MouseEventHandler(this.mouseDown);
            base.MouseUp += new MouseEventHandler(this.mouseUp);
            base.Enter += new EventHandler(this.weGotFocus);
            base.Leave += new EventHandler(this.weLostFocus);
            base.KeyDown += new KeyEventHandler(this.keyDown);
            base.KeyUp += new KeyEventHandler(this.keyUp);
        }

        protected virtual void AddShape(GraphicsPath gpath, Rectangle rect)
        {
            gpath.AddEllipse(rect);
        }

        protected virtual void BuildGraphicsPath(Rectangle buttonRect)
        {
            this.bpath = new GraphicsPath();
            Rectangle rect = new Rectangle(buttonRect.X - 1, buttonRect.Y - 1, buttonRect.Width + 2, buttonRect.Height + 2);
            this.AddShape(this.bpath, rect);
            this.AddShape(this.bpath, buttonRect);
        }

        protected void buttonDown()
        {
            this.lightAngle = 230f;
            this.buttonPressOffset = 1;
            base.Invalidate();
        }

        protected void buttonUp()
        {
            this.lightAngle = 50f;
            this.buttonPressOffset = 0;
            base.Invalidate();
        }

        protected virtual void DrawButton(Graphics g, Rectangle buttonRect)
        {
            this.BuildGraphicsPath(buttonRect);
            PathGradientBrush brush = new PathGradientBrush(this.bpath);
            brush.SurroundColors = new Color[] { this.buttonColor };
            buttonRect.Offset(this.buttonPressOffset, this.buttonPressOffset);
            if (this.bevelHeight > 0)
            {
                buttonRect.Inflate(1, 1);
                brush.CenterPoint = new PointF((float) ((buttonRect.X + (buttonRect.Width / 8)) + this.buttonPressOffset), (float) ((buttonRect.Y + (buttonRect.Height / 8)) + this.buttonPressOffset));
                brush.CenterColor = this.cColor;
                this.FillShape(g, brush, buttonRect);
                this.ShrinkShape(ref g, ref buttonRect, this.bevelHeight);
            }
            if (this.bevelDepth > 0)
            {
                this.DrawInnerBevel(g, buttonRect, this.bevelDepth, this.buttonColor);
                this.ShrinkShape(ref g, ref buttonRect, this.bevelDepth);
            }
            brush.CenterColor = this.buttonColor;
            if (this.dome)
            {
                brush.CenterColor = this.cColor;
                brush.CenterPoint = new PointF((float) ((buttonRect.X + (buttonRect.Width / 8)) + this.buttonPressOffset), (float) ((buttonRect.Y + (buttonRect.Height / 8)) + this.buttonPressOffset));
            }
            this.FillShape(g, brush, buttonRect);
            if (this.gotFocus)
            {
                this.DrawFocus(g, buttonRect);
            }
        }

        protected virtual void DrawEdges(Graphics g, ref Rectangle edgeRect)
        {
            this.ShrinkShape(ref g, ref edgeRect, 1);
            Rectangle rect = edgeRect;
            rect.Inflate(1, 1);
            this.edgeBrush = new LinearGradientBrush(rect, this.edgeColor1, this.edgeColor2, this.GetLightAngle(this.lightAngle));
            this.edgeBlend = new Blend();
            this.edgeBlend.Positions = new float[] { 0f, 0.2f, 0.4f, 0.6f, 0.8f, 1f };
            this.edgeBlend.Factors = new float[] { 0f, 0f, 0.2f, 0.4f, 1f, 1f };
            this.edgeBrush.Blend = this.edgeBlend;
            this.FillShape(g, this.edgeBrush, edgeRect);
        }

        protected virtual void DrawFocus(Graphics g, Rectangle rect)
        {
            rect.Inflate(-2, -2);
            Pen pen = new Pen(Color.Black);
            pen.DashStyle = DashStyle.Dot;
            this.DrawShape(g, pen, rect);
        }

        protected virtual void DrawInnerBevel(Graphics g, Rectangle rect, int depth, Color buttonColor)
        {
            Color color = ControlPaint.LightLight(buttonColor);
            Color color2 = ControlPaint.Dark(buttonColor);
            Blend blend = new Blend();
            blend.Positions = new float[] { 0f, 0.2f, 0.4f, 0.6f, 0.8f, 1f };
            blend.Factors = new float[] { 0.2f, 0.4f, 0.6f, 0.6f, 1f, 1f };
            Rectangle rectangle = rect;
            rectangle.Inflate(1, 1);
            LinearGradientBrush brush = new LinearGradientBrush(rectangle, color2, color, this.GetLightAngle(50f));
            brush.Blend = blend;
            this.FillShape(g, brush, rect);
        }

        protected virtual void DrawRecess(ref Graphics g, ref Rectangle recessRect)
        {
            LinearGradientBrush brush = new LinearGradientBrush(recessRect, ControlPaint.Dark(this.backgroudColor), ControlPaint.LightLight(this.backgroudColor), this.GetLightAngle(50f));
            Blend blend = new Blend();
            blend.Positions = new float[] { 0f, 0.2f, 0.4f, 0.6f, 0.8f, 1f };
            blend.Factors = new float[] { 0.2f, 0.2f, 0.4f, 0.4f, 1f, 1f };
            brush.Blend = blend;
            Rectangle rect = recessRect;
            this.ShrinkShape(ref g, ref rect, 1);
            this.FillShape(g, brush, rect);
            this.ShrinkShape(ref g, ref recessRect, this.recessDepth);
        }

        protected virtual void DrawShape(Graphics g, Pen pen, Rectangle rect)
        {
            g.DrawEllipse(pen, rect);
        }

        protected void FillBackground(Graphics g, Rectangle rect)
        {
            Rectangle rectangle = rect;
            rectangle.Inflate(1, 1);
            SolidBrush brush = new SolidBrush(Color.FromKnownColor(KnownColor.Transparent));
            brush.Color = this.backgroudColor;
            g.FillRectangle(brush, rectangle);
            brush.Dispose();
        }

        protected virtual void FillShape(Graphics g, object brush, Rectangle rect)
        {
            if (brush.GetType().ToString() == "System.Drawing.Drawing2D.LinearGradientBrush")
            {
                g.FillEllipse((LinearGradientBrush) brush, rect);
            }
            else if (brush.GetType().ToString() == "System.Drawing.Drawing2D.PathGradientBrush")
            {
                g.FillEllipse((PathGradientBrush) brush, rect);
            }
        }

        protected int GetEdgeWidth(Rectangle rect)
        {
            if ((rect.Width < 50) | (rect.Height < 50))
            {
                return 1;
            }
            return 2;
        }

        protected float GetLightAngle(float angle)
        {
            float num = 1f - (((float) base.Width) / ((float) base.Height));
            return (angle - (15f * num));
        }

        protected void keyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString() == "Space")
            {
                this.buttonDown();
            }
        }

        protected void keyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString() == "Space")
            {
                this.buttonUp();
            }
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
            if (this.Paint != null)
            {
                this.Paint(this, e);
            }
            else
            {
                base.OnPaint(e);
                if (!base.Enabled)
                {
                    this.buttonColor = ControlPaint.Light(SystemColors.InactiveCaption);
                    this.backgroudColor = SystemColors.InactiveCaption;
                    this.borderColor = SystemColors.InactiveBorder;
                }
                else
                {
                    this.buttonColor = Color.Aqua;
                    this.backgroudColor = Color.GhostWhite;
                    this.borderColor = Color.Black;
                }
                this.edgeColor1 = ControlPaint.Light(this.buttonColor);
                this.edgeColor2 = ControlPaint.Dark(this.buttonColor);
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                this.itemSize.Width = (base.Size.Width * 80) / 100;
                this.itemSize.Height = (base.Size.Height * 80) / 100;
                int num = (base.Size.Width * 10) / 100;
                Rectangle rect = new Rectangle(this.PosFromAlignment.X + 2, this.PosFromAlignment.Y + 2, this.itemSize.Width - 4, this.itemSize.Height - 4);
                Rectangle rectangle2 = new Rectangle(this.PosFromAlignment.X - num, this.PosFromAlignment.Y - num, (this.itemSize.Width + (num * 2)) - 1, (this.itemSize.Height + (num * 2)) - 1);
                this.edgeWidth = this.GetEdgeWidth(rect);
                this.FillBackground(g, rectangle2);
                g.DrawRectangle(new Pen(new SolidBrush(this.borderColor)), rectangle2);
                if (this.RecessDepth > 0)
                {
                    this.DrawRecess(ref g, ref rect);
                }
                this.DrawEdges(g, ref rect);
                this.ShrinkShape(ref g, ref rect, this.edgeWidth);
                this.DrawButton(g, rect);
            }
        }

        protected virtual void SetClickableRegion()
        {
            this.gpath = new GraphicsPath();
            this.gpath.AddEllipse(base.ClientRectangle);
            base.Region = new Region(this.gpath);
        }

        protected virtual void ShrinkShape(ref Graphics g, ref Rectangle rect, int amount)
        {
            rect.Inflate(-amount, -amount);
        }

        protected void weGotFocus(object sender, EventArgs e)
        {
            this.gotFocus = true;
            base.Invalidate();
        }

        protected void weLostFocus(object sender, EventArgs e)
        {
            this.gotFocus = false;
            this.buttonUp();
            base.Invalidate();
        }

        private int BevelDepth
        {
            get
            {
                return this.bevelDepth;
            }
            set
            {
                if (value < 0)
                {
                    this.bevelDepth = 0;
                }
                else
                {
                    this.bevelDepth = value;
                }
                base.Invalidate();
            }
        }

        private int BevelHeight
        {
            get
            {
                return this.bevelHeight;
            }
            set
            {
                if (value < 0)
                {
                    this.bevelHeight = 0;
                }
                else
                {
                    this.bevelHeight = value;
                }
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

        private bool Dome
        {
            get
            {
                return this.dome;
            }
            set
            {
                this.dome = value;
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
                        point.X = (int) ((((double) base.Size.Width) / 2.0) - (((double) this.itemSize.Width) / 2.0));
                        point.Y = 0;
                        return point;

                    case ContentAlignment.TopRight:
                        point.X = base.Size.Width - this.itemSize.Width;
                        point.Y = 0;
                        return point;

                    case ContentAlignment.MiddleLeft:
                        point.X = 0;
                        point.Y = (int) ((((double) base.Size.Height) / 2.0) - (((double) this.itemSize.Height) / 2.0));
                        return point;

                    case ContentAlignment.MiddleCenter:
                        point.X = (int) ((((double) base.Size.Width) / 2.0) - (((double) this.itemSize.Width) / 2.0));
                        point.Y = (int) ((((double) base.Size.Height) / 2.0) - (((double) this.itemSize.Height) / 2.0));
                        return point;

                    case ContentAlignment.BottomCenter:
                        point.X = (int) ((((double) base.Width) / 2.0) - (((double) this.itemSize.Width) / 2.0));
                        point.Y = base.Size.Height - this.itemSize.Height;
                        return point;

                    case ContentAlignment.BottomRight:
                        point.X = base.Size.Width - this.itemSize.Width;
                        point.Y = base.Size.Height - this.itemSize.Height;
                        return point;

                    case ContentAlignment.MiddleRight:
                        point.X = base.Size.Width - this.itemSize.Width;
                        point.Y = (int) ((((double) base.Size.Height) / 2.0) - (((double) this.itemSize.Height) / 2.0));
                        return point;

                    case ContentAlignment.BottomLeft:
                        point.X = 0;
                        point.Y = base.Size.Height - this.itemSize.Height;
                        return point;
                }
                point.X = 0;
                point.Y = 0;
                return point;
            }
        }

        private int RecessDepth
        {
            get
            {
                return this.recessDepth;
            }
            set
            {
                if (value < 0)
                {
                    this.recessDepth = 0;
                }
                else if (value > 15)
                {
                    this.recessDepth = 15;
                }
                else
                {
                    this.recessDepth = value;
                }
                base.Invalidate();
            }
        }
    }
}

