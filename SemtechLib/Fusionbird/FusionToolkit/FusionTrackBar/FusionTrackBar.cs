using Fusionbird.FusionToolkit;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Fusionbird.FusionToolkit.FusionTrackBar
{
	public class FusionTrackBar : System.Windows.Forms.TrackBar
	{
		public event EventHandler<TrackBarDrawItemEventArgs> DrawChannel;
		public event EventHandler<TrackBarDrawItemEventArgs> DrawThumb;
		public event EventHandler<TrackBarDrawItemEventArgs> DrawTicks;

		private Rectangle ChannelBounds;
		private TrackBarOwnerDrawParts m_OwnerDrawParts;
		private Rectangle ThumbBounds;
		private int ThumbState;

		public FusionTrackBar()
		{
			base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
		}

		private void DrawHorizontalTicks(Graphics g, Color color)
		{
			RectangleF ef;
			int num = (base.Maximum / base.TickFrequency) - 1;
			Pen pen = new Pen(color);
			RectangleF ef2 = new RectangleF((float)(ChannelBounds.Left + (ThumbBounds.Width / 2)), (float)(ThumbBounds.Top - 5), 0f, 3f);
			RectangleF ef3 = new RectangleF((float)((ChannelBounds.Right - (ThumbBounds.Width / 2)) - 1), (float)(ThumbBounds.Top - 5), 0f, 3f);
			float x = (ef3.Right - ef2.Left) / ((float)(num + 1));
			if (base.TickStyle != TickStyle.BottomRight)
			{
				g.DrawLine(pen, ef2.Left, ef2.Top, ef2.Right, ef2.Bottom);
				g.DrawLine(pen, ef3.Left, ef3.Top, ef3.Right, ef3.Bottom);
				ef = ef2;
				ef.Height--;
				ef.Offset(x, 1f);
				int num3 = num - 1;
				for (int i = 0; i <= num3; i++)
				{
					g.DrawLine(pen, ef.Left, ef.Top, ef.Left, ef.Bottom);
					ef.Offset(x, 0f);
				}
			}
			ef2.Offset(0f, (float)(ThumbBounds.Height + 6));
			ef3.Offset(0f, (float)(ThumbBounds.Height + 6));
			if (base.TickStyle != TickStyle.TopLeft)
			{
				g.DrawLine(pen, ef2.Left, ef2.Top, ef2.Left, ef2.Bottom);
				g.DrawLine(pen, ef3.Left, ef3.Top, ef3.Left, ef3.Bottom);
				ef = ef2;
				ef.Height--;
				ef.Offset(x, 0f);
				int num5 = num - 1;
				for (int j = 0; j <= num5; j++)
				{
					g.DrawLine(pen, ef.Left, ef.Top, ef.Left, ef.Bottom);
					ef.Offset(x, 0f);
				}
			}
			pen.Dispose();
		}

		private void DrawPointerDown(Graphics g)
		{
			Point[] points = new Point[] { new Point(ThumbBounds.Left + (ThumbBounds.Width / 2), ThumbBounds.Bottom - 1), new Point(ThumbBounds.Left, (ThumbBounds.Bottom - (ThumbBounds.Width / 2)) - 1), ThumbBounds.Location, new Point(ThumbBounds.Right - 1, ThumbBounds.Top), new Point(ThumbBounds.Right - 1, (ThumbBounds.Bottom - (ThumbBounds.Width / 2)) - 1), new Point(ThumbBounds.Left + (ThumbBounds.Width / 2), ThumbBounds.Bottom - 1) };
			GraphicsPath path = new GraphicsPath();
			path.AddLines(points);
			Region region = new Region(path);
			g.Clip = region;

			if (ThumbState == 3 || !base.Enabled)
				ControlPaint.DrawButton(g, ThumbBounds, ButtonState.All);
			else
				g.Clear(SystemColors.Control);

			g.ResetClip();
			region.Dispose();
			path.Dispose();
			Point[] pointArray2 = new Point[] { points[0], points[1], points[2], points[3] };
			g.DrawLines(SystemPens.ControlLightLight, pointArray2);
			pointArray2 = new Point[] { points[3], points[4], points[5] };
			g.DrawLines(SystemPens.ControlDarkDark, pointArray2);
			points[0].Offset(0, -1);
			points[1].Offset(1, 0);
			points[2].Offset(1, 1);
			points[3].Offset(-1, 1);
			points[4].Offset(-1, 0);
			points[5] = points[0];
			pointArray2 = new Point[] { points[0], points[1], points[2], points[3] };
			g.DrawLines(SystemPens.ControlLight, pointArray2);
			pointArray2 = new Point[] { points[3], points[4], points[5] };
			g.DrawLines(SystemPens.ControlDark, pointArray2);
		}

		private void DrawPointerLeft(Graphics g)
		{
			Point[] points = new Point[] { new Point(ThumbBounds.Left, ThumbBounds.Top + (ThumbBounds.Height / 2)), new Point(ThumbBounds.Left + (ThumbBounds.Height / 2), ThumbBounds.Top), new Point(ThumbBounds.Right - 1, ThumbBounds.Top), new Point(ThumbBounds.Right - 1, ThumbBounds.Bottom - 1), new Point(ThumbBounds.Left + (ThumbBounds.Height / 2), ThumbBounds.Bottom - 1), new Point(ThumbBounds.Left, ThumbBounds.Top + (ThumbBounds.Height / 2)) };
			GraphicsPath path = new GraphicsPath();
			path.AddLines(points);
			Region region = new Region(path);
			g.Clip = region;

			if ((ThumbState == 3) || !base.Enabled)
				ControlPaint.DrawButton(g, ThumbBounds, ButtonState.All);
			else
				g.Clear(SystemColors.Control);

			g.ResetClip();
			region.Dispose();
			path.Dispose();
			Point[] pointArray2 = new Point[] { points[0], points[1], points[2] };
			g.DrawLines(SystemPens.ControlLightLight, pointArray2);
			pointArray2 = new Point[] { points[2], points[3], points[4], points[5] };
			g.DrawLines(SystemPens.ControlDarkDark, pointArray2);
			points[0].Offset(1, 0);
			points[1].Offset(0, 1);
			points[2].Offset(-1, 1);
			points[3].Offset(-1, -1);
			points[4].Offset(0, -1);
			points[5] = points[0];
			pointArray2 = new Point[] { points[0], points[1], points[2] };
			g.DrawLines(SystemPens.ControlLight, pointArray2);
			pointArray2 = new Point[] { points[2], points[3], points[4], points[5] };
			g.DrawLines(SystemPens.ControlDark, pointArray2);
		}

		private void DrawPointerRight(Graphics g)
		{
			Point[] points = new Point[] { new Point(ThumbBounds.Left, ThumbBounds.Bottom - 1), new Point(ThumbBounds.Left, ThumbBounds.Top), new Point((ThumbBounds.Right - (ThumbBounds.Height / 2)) - 1, ThumbBounds.Top), new Point(ThumbBounds.Right - 1, ThumbBounds.Top + (ThumbBounds.Height / 2)), new Point((ThumbBounds.Right - (ThumbBounds.Height / 2)) - 1, ThumbBounds.Bottom - 1), new Point(ThumbBounds.Left, ThumbBounds.Bottom - 1) };
			GraphicsPath path = new GraphicsPath();
			path.AddLines(points);
			Region region = new Region(path);
			g.Clip = region;

			if ((ThumbState == 3) || !base.Enabled)
				ControlPaint.DrawButton(g, ThumbBounds, ButtonState.All);
			else
				g.Clear(SystemColors.Control);

			g.ResetClip();
			region.Dispose();
			path.Dispose();
			Point[] pointArray2 = new Point[] { points[0], points[1], points[2], points[3] };
			g.DrawLines(SystemPens.ControlLightLight, pointArray2);
			pointArray2 = new Point[] { points[3], points[4], points[5] };
			g.DrawLines(SystemPens.ControlDarkDark, pointArray2);
			points[0].Offset(1, -1);
			points[1].Offset(1, 1);
			points[2].Offset(0, 1);
			points[3].Offset(-1, 0);
			points[4].Offset(0, -1);
			points[5] = points[0];
			pointArray2 = new Point[] { points[0], points[1], points[2], points[3] };
			g.DrawLines(SystemPens.ControlLight, pointArray2);
			pointArray2 = new Point[] { points[3], points[4], points[5] };
			g.DrawLines(SystemPens.ControlDark, pointArray2);
		}

		private void DrawPointerUp(Graphics g)
		{
			Point[] points = new Point[] { new Point(ThumbBounds.Left, ThumbBounds.Bottom - 1), new Point(ThumbBounds.Left, ThumbBounds.Top + (ThumbBounds.Width / 2)), new Point(ThumbBounds.Left + (ThumbBounds.Width / 2), ThumbBounds.Top), new Point(ThumbBounds.Right - 1, ThumbBounds.Top + (ThumbBounds.Width / 2)), new Point(ThumbBounds.Right - 1, ThumbBounds.Bottom - 1), new Point(ThumbBounds.Left, ThumbBounds.Bottom - 1) };
			GraphicsPath path = new GraphicsPath();
			path.AddLines(points);
			Region region = new Region(path);
			g.Clip = region;

			if ((ThumbState == 3) || !base.Enabled)
				ControlPaint.DrawButton(g, ThumbBounds, ButtonState.All);
			else
				g.Clear(SystemColors.Control);

			g.ResetClip();
			region.Dispose();
			path.Dispose();
			Point[] pointArray2 = new Point[] { points[0], points[1], points[2] };
			g.DrawLines(SystemPens.ControlLightLight, pointArray2);
			pointArray2 = new Point[] { points[2], points[3], points[4], points[5] };
			g.DrawLines(SystemPens.ControlDarkDark, pointArray2);
			points[0].Offset(1, -1);
			points[1].Offset(1, 0);
			points[2].Offset(0, 1);
			points[3].Offset(-1, 0);
			points[4].Offset(-1, -1);
			points[5] = points[0];
			pointArray2 = new Point[] { points[0], points[1], points[2] };
			g.DrawLines(SystemPens.ControlLight, pointArray2);
			pointArray2 = new Point[] { points[2], points[3], points[4], points[5] };
			g.DrawLines(SystemPens.ControlDark, pointArray2);
		}

		private void DrawVerticalTicks(Graphics g, Color color)
		{
			RectangleF ef;
			int num = (base.Maximum / base.TickFrequency) - 1;
			Pen pen = new Pen(color);
			RectangleF ef2 = new RectangleF((float)(ThumbBounds.Left - 5), (float)((ChannelBounds.Bottom - (ThumbBounds.Height / 2)) - 1), 3f, 0f);
			RectangleF ef3 = new RectangleF((float)(ThumbBounds.Left - 5), (float)(ChannelBounds.Top + (ThumbBounds.Height / 2)), 3f, 0f);
			float y = (ef3.Bottom - ef2.Top) / ((float)(num + 1));
			if (base.TickStyle != TickStyle.BottomRight)
			{
				g.DrawLine(pen, ef2.Left, ef2.Top, ef2.Right, ef2.Bottom);
				g.DrawLine(pen, ef3.Left, ef3.Top, ef3.Right, ef3.Bottom);
				ef = ef2;
				ef.Width--;
				ef.Offset(1f, y);
				int num3 = num - 1;
				for (int i = 0; i <= num3; i++)
				{
					g.DrawLine(pen, ef.Left, ef.Top, ef.Right, ef.Bottom);
					ef.Offset(0f, y);
				}
			}
			ef2.Offset((float)(ThumbBounds.Width + 6), 0f);
			ef3.Offset((float)(ThumbBounds.Width + 6), 0f);
			if (base.TickStyle != TickStyle.TopLeft)
			{
				g.DrawLine(pen, ef2.Left, ef2.Top, ef2.Right, ef2.Bottom);
				g.DrawLine(pen, ef3.Left, ef3.Top, ef3.Right, ef3.Bottom);
				ef = ef2;
				ef.Width--;
				ef.Offset(0f, y);
				int num5 = num - 1;
				for (int j = 0; j <= num5; j++)
				{
					g.DrawLine(pen, ef.Left, ef.Top, ef.Right, ef.Bottom);
					ef.Offset(0f, y);
				}
			}
			pen.Dispose();
		}

		protected virtual void OnDrawChannel(IntPtr hdc)
		{
			Graphics graphics = Graphics.FromHdc(hdc);
			if (((OwnerDrawParts & TrackBarOwnerDrawParts.Channel) == TrackBarOwnerDrawParts.Channel) && !base.DesignMode)
			{
				TrackBarDrawItemEventArgs e = new TrackBarDrawItemEventArgs(graphics, ChannelBounds, (TrackBarItemState)ThumbState);
				if (DrawChannel != null)
					DrawChannel(this, e);
			}
			else
			{
				if (ChannelBounds.Equals(Rectangle.Empty))
					return;
				if (VisualStyleRenderer.IsSupported)
				{
					new VisualStyleRenderer("TRACKBAR", 1, 1).DrawBackground(graphics, ChannelBounds);
					graphics.ResetClip();
					graphics.Dispose();
					return;
				}
				ControlPaint.DrawBorder3D(graphics, ChannelBounds, Border3DStyle.Sunken);
			}
			graphics.Dispose();
		}

		protected virtual void OnDrawThumb(IntPtr hdc)
		{
			Graphics graphics = Graphics.FromHdc(hdc);
			graphics.Clip = new Region(ThumbBounds);
			if (((OwnerDrawParts & TrackBarOwnerDrawParts.Thumb) == TrackBarOwnerDrawParts.Thumb) && !base.DesignMode)
			{
				TrackBarDrawItemEventArgs e = new TrackBarDrawItemEventArgs(graphics, ThumbBounds, (TrackBarItemState)ThumbState);
				if (DrawThumb != null)
					DrawThumb(this, e);
			}
			else
			{
				Fusionbird.FusionToolkit.NativeMethods.TrackBarParts parts = Fusionbird.FusionToolkit.NativeMethods.TrackBarParts.TKP_THUMB;
				if (ThumbBounds.Equals(Rectangle.Empty))
					return;
				switch (base.TickStyle)
				{
					case TickStyle.None:
					case TickStyle.BottomRight:
						parts = (base.Orientation != Orientation.Horizontal) ? Fusionbird.FusionToolkit.NativeMethods.TrackBarParts.TKP_THUMBRIGHT : Fusionbird.FusionToolkit.NativeMethods.TrackBarParts.TKP_THUMBBOTTOM;
						break;

					case TickStyle.TopLeft:
						parts = (base.Orientation != Orientation.Horizontal) ? Fusionbird.FusionToolkit.NativeMethods.TrackBarParts.TKP_THUMBLEFT : Fusionbird.FusionToolkit.NativeMethods.TrackBarParts.TKP_THUMBTOP;
						break;

					case TickStyle.Both:
						parts = (base.Orientation != Orientation.Horizontal) ? Fusionbird.FusionToolkit.NativeMethods.TrackBarParts.TKP_THUMBVERT : Fusionbird.FusionToolkit.NativeMethods.TrackBarParts.TKP_THUMB;
						break;
				}
				if (VisualStyleRenderer.IsSupported)
				{
					new VisualStyleRenderer("TRACKBAR", (int)parts, ThumbState).DrawBackground(graphics, ThumbBounds);
					graphics.ResetClip();
					graphics.Dispose();
					return;
				}
				switch (parts)
				{
					case Fusionbird.FusionToolkit.NativeMethods.TrackBarParts.TKP_THUMBBOTTOM:
						DrawPointerDown(graphics);
						break;

					case Fusionbird.FusionToolkit.NativeMethods.TrackBarParts.TKP_THUMBTOP:
						DrawPointerUp(graphics);
						break;

					case Fusionbird.FusionToolkit.NativeMethods.TrackBarParts.TKP_THUMBLEFT:
						DrawPointerLeft(graphics);
						break;

					case Fusionbird.FusionToolkit.NativeMethods.TrackBarParts.TKP_THUMBRIGHT:
						DrawPointerRight(graphics);
						break;
					default:
						if (ThumbState == 3 || !base.Enabled)
							ControlPaint.DrawButton(graphics, ThumbBounds, ButtonState.All);
						else
							graphics.FillRectangle(SystemBrushes.Control, ThumbBounds);

						ControlPaint.DrawBorder3D(graphics, ThumbBounds, Border3DStyle.Raised);
						break;
				}
			}

			graphics.ResetClip();
			graphics.Dispose();
		}

		protected virtual void OnDrawTicks(IntPtr hdc)
		{
			Graphics graphics = Graphics.FromHdc(hdc);
			if (((OwnerDrawParts & TrackBarOwnerDrawParts.Ticks) == TrackBarOwnerDrawParts.Ticks) && !base.DesignMode)
			{
				TrackBarDrawItemEventArgs e = new TrackBarDrawItemEventArgs(graphics, base.ClientRectangle, (TrackBarItemState)ThumbState);
				if (DrawTicks != null)
					DrawTicks(this, e);
			}
			else
			{
				if (base.TickStyle == TickStyle.None)
					return;
				if (ThumbBounds.Equals(Rectangle.Empty))
					return;
				Color black = Color.Black;
				if (VisualStyleRenderer.IsSupported)
					black = new VisualStyleRenderer("TRACKBAR", 9, ThumbState).GetColor(ColorProperty.TextColor);
				if (base.Orientation == Orientation.Horizontal)
					DrawHorizontalTicks(graphics, black);
				else
					DrawVerticalTicks(graphics, black);
			}
			graphics.Dispose();
		}

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == 20)
				m.Result = IntPtr.Zero;
			else
			{
				base.WndProc(ref m);
				if (m.Msg == 0x204e)
				{
					Fusionbird.FusionToolkit.NativeMethods.NMHDR structure = (Fusionbird.FusionToolkit.NativeMethods.NMHDR)Marshal.PtrToStructure(m.LParam, typeof(Fusionbird.FusionToolkit.NativeMethods.NMHDR));
					if (structure.code == -12)
					{
						IntPtr ptr;
						Marshal.StructureToPtr(structure, m.LParam, false);
						Fusionbird.FusionToolkit.NativeMethods.NMCUSTOMDRAW nmcustomdraw = (Fusionbird.FusionToolkit.NativeMethods.NMCUSTOMDRAW)Marshal.PtrToStructure(m.LParam, typeof(Fusionbird.FusionToolkit.NativeMethods.NMCUSTOMDRAW));
						if (nmcustomdraw.dwDrawStage == Fusionbird.FusionToolkit.NativeMethods.CustomDrawDrawStage.CDDS_PREPAINT)
						{
							Graphics graphics = Graphics.FromHdc(nmcustomdraw.hdc);
							PaintEventArgs e = new PaintEventArgs(graphics, base.Bounds);
							e.Graphics.TranslateTransform((float)-base.Left, (float)-base.Top);
							base.InvokePaintBackground(base.Parent, e);
							base.InvokePaint(base.Parent, e);
							SolidBrush brush = new SolidBrush(BackColor);
							e.Graphics.FillRectangle(brush, base.Bounds);
							brush.Dispose();
							e.Graphics.ResetTransform();
							e.Dispose();
							graphics.Dispose();
							ptr = new IntPtr(0x30);
							m.Result = ptr;
						}
						else if (nmcustomdraw.dwDrawStage == Fusionbird.FusionToolkit.NativeMethods.CustomDrawDrawStage.CDDS_POSTPAINT)
						{
							OnDrawTicks(nmcustomdraw.hdc);
							OnDrawChannel(nmcustomdraw.hdc);
							OnDrawThumb(nmcustomdraw.hdc);
						}
						else if (nmcustomdraw.dwDrawStage == Fusionbird.FusionToolkit.NativeMethods.CustomDrawDrawStage.CDDS_ITEMPREPAINT)
						{
							if (nmcustomdraw.dwItemSpec.ToInt32() == 2)
							{
								ThumbBounds = nmcustomdraw.rc.ToRectangle();
								if (base.Enabled)
								{
									if (nmcustomdraw.uItemState == Fusionbird.FusionToolkit.NativeMethods.CustomDrawItemState.CDIS_SELECTED)
										ThumbState = 3;
									else
										ThumbState = 1;
								}
								else
								{
									ThumbState = 5;
								}
								OnDrawThumb(nmcustomdraw.hdc);
							}
							else if (nmcustomdraw.dwItemSpec.ToInt32() == 3)
							{
								ChannelBounds = nmcustomdraw.rc.ToRectangle();
								OnDrawChannel(nmcustomdraw.hdc);
							}
							else if (nmcustomdraw.dwItemSpec.ToInt32() == 1)
							{
								OnDrawTicks(nmcustomdraw.hdc);
							}
							ptr = new IntPtr(4);
							m.Result = ptr;
						}
					}
				}
			}
		}

		[DefaultValue(typeof(TrackBarOwnerDrawParts), "None"), Description("Gets/sets the trackbar parts that will be OwnerDrawn."), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Editor(typeof(TrackDrawModeEditor), typeof(UITypeEditor))]
		public TrackBarOwnerDrawParts OwnerDrawParts
		{
			get { return m_OwnerDrawParts; }
			set { m_OwnerDrawParts = value; }
		}
	}
}
