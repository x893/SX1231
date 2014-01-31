using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;

namespace SemtechLib.Devices.SX1231.Controls
{
	public class PayloadImg : Control
	{
		public new event PaintEventHandler Paint;

		public PayloadImg()
		{
			base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			base.SetStyle(ControlStyles.DoubleBuffer, true);
			base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			base.SetStyle(ControlStyles.UserPaint, true);
			base.SetStyle(ControlStyles.ResizeRedraw, true);
			BackColor = Color.Transparent;
			base.Size = new Size(0x20e, 20);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			if (Paint != null)
				Paint(this, e);
			else
			{
				base.OnPaint(e);
				e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
				Image image = new Bitmap(base.Width, base.Height);
				Graphics graphics = Graphics.FromImage(image);
				graphics.SmoothingMode = SmoothingMode.HighQuality;
				RectangleF rect = new RectangleF(0f, 0f, (float)base.Width, (float)base.Height);
				Brush brush = new SolidBrush(SystemColors.ActiveBorder);
				graphics.DrawLine(new Pen(brush, 2f), rect.Left, rect.Bottom, rect.Right - 138f, rect.Top);
				graphics.DrawLine(new Pen(brush, 2f), rect.Right - 52f, rect.Top, rect.Right, rect.Bottom);
				e.Graphics.DrawImage(image, rect);
			}
		}
	}
}